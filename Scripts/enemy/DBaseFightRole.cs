using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ESkillStep
{
    None = 0, //不处于释放技能状态中
    Tracking,//追踪敌人中，即朝敌人走去
    Selecting,//选择攻击对象，有些技能需要指定施放地点 或者被击对象
    BeginAttack,//开始攻击

    End
}
public enum RoleAnimatorState
{
    Idle = 0,
    Walk,
    Run,
    Attack,
    Hitted,
    Death,
    Max
}

public enum RoleFightState
{
    ControlWalk = 0,
    Patrol,
    AutoAttack,
    Freeze,
    Dissy,
    Max
}

public enum AttackState //攻击时状态
{
    Tracking = 0,
    Idle,
    Attack
}

//战斗角色基类
public class DBaseFightRole : MonoBehaviour
{

    public RoleAnimatorState aniState = RoleAnimatorState.Idle;

    public RoleFightState fightstate = RoleFightState.Patrol;

    public AttackState attackState = AttackState.Idle;

    public DRoleData roledata;//人物基本攻防属性

    public List<DBuffData> buffdata = new List<DBuffData>();//人物所有受到的buff；
   
    private DAnimatorController animatorControl;
    private NavMeshAgent agent;

    

    private Color normal;
    private float red_time = 1;//显示被击中的时间
    private float attack_timer = 0;//攻击计时器

    public AudioClip sound_miss;    
    public Renderer bodyrender;

    public Transform target;


    private bool isCasteringSkill = false; //当前是否还处在施放技能中

    private SkillCasterData casterdata;

    public  const float NEAR_ATTACK_DISTANCE = 2f;

    public int side = 0; //战斗中处于哪一方，默认0 是自己方，1 是敌方

    public bool isMainRole = false;//是否队伍中的主角

    public GameObject roleModel;

    public ESkillStep skillstep = ESkillStep.None;
    public void setSide(int fside,GameObject modelprefab,Vector3 pos,Vector3 rotate)
    {
        side = fside;
        roleModel = GameObject.Instantiate(modelprefab) as GameObject;
        //agent = roleModel.GetComponent<NavMeshAgent>();
        roleModel.transform.position = pos;
        roleModel.transform.parent = transform;
        roleModel.tag = Tags.player;

        roleModel.transform.localEulerAngles = rotate;
       // roleModel.transform.localScale = new Vector3(8, 8, 8);
        animatorControl = roleModel.AddComponent<DAnimatorController>();
        agent = roleModel.AddComponent<NavMeshAgent>();
        StartCoroutine(resetModelPos());
        roledata = new DRoleData();

    }

    IEnumerator resetModelPos()
    {
        yield return  null;
       // roleModel.transform.localPosition = Vector3.zero + Vector3.up * 0.26f;
       // roleModel.transform.position = transform.position;
        animatorControl.resetToIdle();
        agent.SetDestination(roleModel.transform.position);

    }

    //使用技能发动攻击
    public bool useSkill(DSkillBaseData baseSkilldata)
    {
        if (isCasteringSkill)
            return false;

        if (baseSkilldata != null)
        {
            isCasteringSkill = true;
            casterdata = new SkillCasterData();
            casterdata.castRole = gameObject;
            casterdata.skilldata = baseSkilldata;

            GameObject firstenemy = FightRoleManager._instance.findAttackEnemy(this);

            if(firstenemy != null)
            {
                float dist = FightRoleManager._instance.getFightRoleDistance(this, firstenemy.GetComponent<DBaseFightRole>());
                roleModel.transform.LookAt(firstenemy.GetComponent<DBaseFightRole>().roleModel.transform, Vector3.up);
                if(dist > DefaultSkillParam.PathFindingDist)//当距离大于可自动寻路距离，不作攻击，只表现一下施放动作和特性
                {
                    StartCoroutine(showAttackMov());
                    
                }
                else if (dist > baseSkilldata.minAttackDist)
                {
                    StartCoroutine(gotoEnemy());
                }
                else
                {
                    //roleModel.transform.LookAt(firstenemy.transform, Vector3.up);
                    if (casterdata.skilldata.isNeedAppoint)
                    {
                        skillstep = ESkillStep.Selecting;
                    }
                    else
                    {
                        if (casterdata.skilldata.isUseMoveEff)
                            StartCoroutine(ShowMoveEffectToEnemy(firstenemy));
                        else
                            StartCoroutine(showBeantonMov());
                        if (casterdata.skilldata.isShakeCamera)
                            Invoke("shakeCamera", casterdata.skilldata.shakeTime);
                        StartCoroutine(showAttackMov());
                    }
                }
            }
            return true;
        }
        return false;

    }


    void showFireEff()
    {
        //BoxCollider boxcld = gameObject.GetComponent<BoxCollider>();
        Vector3 firepos = Vector3.zero;
        if (casterdata.skilldata.fireEffPos == EFirePos.Center)
        {
            firepos = roleModel.transform.position;
        }
        else if (casterdata.skilldata.fireEffPos == EFirePos.Forward)
        {
            firepos = roleModel.transform.position + roleModel.transform.forward * agent.radius;
        }
        else if (casterdata.skilldata.fireEffPos == EFirePos.Back)
        {
            firepos = roleModel.transform.position - roleModel.transform.forward * agent.radius;
        }
        else if (casterdata.skilldata.fireEffPos == EFirePos.Top)
        {
            firepos = roleModel.transform.position + roleModel.transform.up * agent.height/2;
        }
        else if (casterdata.skilldata.fireEffPos == EFirePos.Bottom)
        {
            firepos = roleModel.transform.position - roleModel.transform.up * agent.height/2 + Vector3.up * 0.2f;
        }
        else if (casterdata.skilldata.fireEffPos == EFirePos.Left)
        {
            firepos = roleModel.transform.position - roleModel.transform.right * agent.radius;
        }
        else if (casterdata.skilldata.fireEffPos == EFirePos.Right)
        {
            firepos = roleModel.transform.position + roleModel.transform.right * agent.radius;
        }

        GameObject attackEff = new GameObject(casterdata.skilldata.fireEffUrl);
        EffectAsset effectasset = attackEff.AddComponent<EffectAsset>();

        effectasset.setEffectParam(casterdata.skilldata.fireEffUrl, firepos, roleModel.transform.rotation, EEffectType.Attack);
    }

    IEnumerator showAttackMov()
    {
      //  while (animatorControl.currentState != eAnimatorState.await)
        {
       //     yield return null;
        }

        if (!string.IsNullOrEmpty(casterdata.skilldata.fireEffUrl))
        {
            Invoke("showFireEff", casterdata.skilldata.fireTime);
        }

        if (animatorControl != null)
            animatorControl.changeToState(casterdata.skilldata.animatorClip);
        yield return null;

        while(animatorControl.currentState != eAnimatorState.await)
        {
            yield return null;
        }

        isCasteringSkill = false;
        //casterdata = null;

    }
    IEnumerator gotoEnemy()
    {
        while (true)
        {
            yield return null;
            GameObject firstenemy = FightRoleManager._instance.findAttackEnemy(this);
            if (firstenemy != null)
            {
                float dist = FightRoleManager._instance.getFightRoleDistance(this, firstenemy.GetComponent<DBaseFightRole>());

                if (dist > casterdata.skilldata.minAttackDist)
                {
                    agent.SetDestination(firstenemy.GetComponent<DBaseFightRole>().roleModel.transform.position);
                    animatorControl.changeToState(eAnimatorState.arun);
                }
                else
                {
                    agent.SetDestination(roleModel.transform.position);

                  //  roleModel.transform.LookAt(firstenemy.transform, Vector3.up);
                    animatorControl.resetToIdle();
                    if (casterdata.skilldata.isNeedAppoint)
                    {
                        skillstep = ESkillStep.Selecting;
                    }
                    else
                    {
                        if (casterdata.skilldata.isUseMoveEff)
                            StartCoroutine(ShowMoveEffectToEnemy(firstenemy));
                        else
                            StartCoroutine(showBeantonMov());
                        if (casterdata.skilldata.isShakeCamera)
                            Invoke("shakeCamera", casterdata.skilldata.shakeTime);
                        StartCoroutine(showAttackMov());
                    }
                    break;
                }
            }
            else
            {
                agent.SetDestination(roleModel.transform.position);
                animatorControl.resetToIdle();
                isCasteringSkill = false;
                break;
            }
        }
    }

    void shakeCamera()
    {
        ShakeCamera.shakeCamera();
    }
    void beginAttack()
    {
        animatorControl.changeToState(casterdata.skilldata.animatorClip);
        SendMessageUpwards("attackBegin");

    }
    public void TakeDamage(DBaseFightRole attackrole,SkillInfo skill)
    {


    }

    public void AddBuff()
    {


    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (skillstep == ESkillStep.Selecting && Input.GetMouseButtonDown(0))
        {
          OnLockTarget();
        }

	}

    void OnLockTarget()
    {
        StartCoroutine(OnLockMultiTarget());
    }
    IEnumerator OnLockMultiTarget()
    {
       // CursorManager.instance.SetNormal();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitinfo;

        LayerMask mask = 1 << LayerMask.NameToLayer("GroundLayer");

        bool isCollider = Physics.Raycast(ray, out hitinfo, 1000, mask);
        if (isCollider && hitinfo.collider.tag == Tags.ground)
        {
            roleModel.transform.LookAt(hitinfo.point,Vector3.up);

            StartCoroutine(showAttackMov());

            yield return null;
            skillstep = ESkillStep.None;

            if (casterdata.skilldata.isUseMoveEff)
            {
                //开始播放移到特效
                StartCoroutine(ShowMoveEffect(hitinfo.point));
            }
            else if (!string.IsNullOrEmpty(casterdata.skilldata.explodeEffUrl))
            {
                showExplodeEff(hitinfo.point);
            }
           
        }
        else
            skillstep = ESkillStep.None;
    }

    //移到特效指定的地点飞去 一般是地面
    IEnumerator ShowMoveEffect(Vector3 targetPos)
    {
        yield return new WaitForSeconds(casterdata.skilldata.moveBeginTime);
        if (casterdata != null && !string.IsNullOrEmpty(casterdata.skilldata.moveEffUrl))
        {
            GameObject moveEff = new GameObject(casterdata.skilldata.moveEffUrl);
            EffectAsset effectasset = moveEff.AddComponent<EffectAsset>();
            Vector3 pos = roleModel.transform.position + roleModel.transform.forward * 0.5f + Vector3.up;
            effectasset.setEffectParam(casterdata.skilldata.moveEffUrl, pos, Quaternion.identity, EEffectType.Move);

            MoveEffect effectmove = moveEff.AddComponent<MoveEffect>();
            effectmove.SetTargetPos(targetPos, casterdata.clone());
        }
    }

    //移到特效朝敌人飞去
    //firstfightEnemy 指定一个攻击对象，如果是单体攻击，那特效只朝这个敌人飞去，不需要重新找攻击对象
    IEnumerator ShowMoveEffectToEnemy(GameObject firstfightEnemy)
    {
        yield return new WaitForSeconds(casterdata.skilldata.moveBeginTime);
        if (casterdata.skilldata.isUseMoveEff)
        {
            if (casterdata.skilldata.isQunGong == false || casterdata.skilldata.isSingleMove)
            {
                casterdata.beatonRoles = new List<GameObject>();
                casterdata.beatonRoles.Add(firstfightEnemy);
                GameObject moveEff = new GameObject(casterdata.skilldata.moveEffUrl);
                EffectAsset effectasset = moveEff.AddComponent<EffectAsset>();
                Vector3 pos = roleModel.transform.position + roleModel.transform.forward + Vector3.up;
                effectasset.setEffectParam(casterdata.skilldata.moveEffUrl, pos, Quaternion.identity, EEffectType.Move);

                MoveEffect effectmove = moveEff.AddComponent<MoveEffect>();
                effectmove.SetTargetPos(firstfightEnemy, casterdata.clone());
            }
            else
            {
                casterdata.beatonRoles = FightRoleManager._instance.getHarmListByDist(side,roleModel.transform.position, casterdata.skilldata.harmDist);
                foreach (GameObject enemy in casterdata.beatonRoles)
                {
                    GameObject moveEff = new GameObject(casterdata.skilldata.moveEffUrl);
                    EffectAsset effectasset = moveEff.AddComponent<EffectAsset>();
                    Vector3 pos = roleModel.transform.position + roleModel.transform.forward + Vector3.up;
                    effectasset.setEffectParam(casterdata.skilldata.moveEffUrl, pos, Quaternion.identity, EEffectType.Move);

                    MoveEffect effectmove = moveEff.AddComponent<MoveEffect>();
                    effectmove.SetTargetPos(enemy, casterdata.clone());
                }
            }
        }      

    }
    //播放爆炸特效
    void showExplodeEff(Vector3 pos)
    {
        if (string.IsNullOrEmpty(casterdata.skilldata.explodeEffUrl))
            return;
        GameObject explodeEff = new GameObject(casterdata.skilldata.explodeEffUrl);
        EffectAsset effectasset = explodeEff.AddComponent<EffectAsset>();

        effectasset.setEffectParam(casterdata.skilldata.explodeEffUrl, pos, Quaternion.identity, EEffectType.Explode);
    }

    IEnumerator showBeantonMov()
    {
        //if (casterdata != null)
       //     yield return new WaitForSeconds(casterdata.skilldata.beatonTime);
       // else
        yield return null;
        if (casterdata != null)
        {
            casterdata.beatonRoles = FightRoleManager._instance.getHarmListByDist(side,roleModel.transform.position, casterdata.skilldata.harmDist);
            DBaseFightRole frole;
            foreach (GameObject enemy in casterdata.beatonRoles)
            {
                frole = enemy.GetComponent<DBaseFightRole>() ;
                if (frole != null)
                {
                    frole.showBeatonBySkill(casterdata);
                }
            }
        }
    }

    //被击退或击飞
    IEnumerator  beatonToBackOrFly(BeatonData beatondata)
    {
        yield return new WaitForSeconds(beatondata.beatonTime);

        if (!string.IsNullOrEmpty(beatondata.beatonEffUrl))
        {
            GameObject beatonEff = new GameObject(beatondata.beatonEffUrl);
            EffectAsset effectasset = beatonEff.AddComponent<EffectAsset>();

            effectasset.setEffectParam(beatondata.beatonEffUrl, roleModel.transform.position + Vector3.up, Quaternion.identity, EEffectType.Beaton);
        }

        if (beatondata.eBeatonbackFly == EBeatonToBackFly.None)
        {
            animatorControl.changeToState(beatondata.animatorBeatonClip);
            yield return null;
        }
        if (beatondata.eBeatonbackFly == EBeatonToBackFly.Back)
        {
            Vector3 backpos = roleModel.transform.position - roleModel.transform.forward * DefaultSkillParam.BeatonBackMaxDist;

            if (agent != null && agent.enabled)
            {
                NavMeshPath path = new NavMeshPath();
                agent.CalculatePath(backpos, path);
                if (path.corners.Length == 2)
                {
                    agent.enabled = false;
                    iTween.MoveTo(roleModel, iTween.Hash("position", backpos, "easeType", "easeOutQuart", "delay",0, "time", 0.3f, "oncomplete", "resetAgent"));
                    yield return new WaitForSeconds(0.3f);
                    agent.enabled = true;
                    animatorControl.changeToState(beatondata.animatorBeatonClip);

                }
                else
                {
                    animatorControl.changeToState(beatondata.animatorBeatonClip);
                }
            }
        }
        else if (beatondata.eBeatonbackFly == EBeatonToBackFly.Fly)
        {
            agent.enabled = false;
            animatorControl.changeToState(beatondata.animatorBeatonClip);

            Vector3 originpos = roleModel.transform.position;
            Vector3 topos = roleModel.transform.position + Vector3.up * DefaultSkillParam.BeatonUpMaxDist;
            iTween.MoveTo(roleModel, iTween.Hash("position", topos, "easeType", "easeOutCirc", "delay", 0, "time", 0.3f));
            yield return new WaitForSeconds(0.33f);

            iTween.MoveTo(roleModel, iTween.Hash("position", originpos, "easeType", "easeInCubic", "delay", 0, "time", 0.3f));
            yield return new WaitForSeconds(0.35f);

            agent.enabled = true;

        }
    }


    public void showBeatonBySkill(SkillCasterData casterdata)
    {
            if (casterdata != null && casterdata.skilldata != null)
            {
                BeatonData beatdata;
                for (int i = 0; i < casterdata.skilldata.beatonDatas.Count; i++)
                {
                    beatdata = casterdata.skilldata.beatonDatas[i];

                   
                    StartCoroutine(beatonToBackOrFly(beatdata));
                }
                
                //animatorControl.changeToState(casterdata.skilldata.animatorBeatonClip);
            }
    }

    
    public void gotoPoint(Vector3 targetpos)
    {
        if(agent != null && agent.enabled)
        {
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(targetpos, path);
            if (path.corners.Length >= 2)
            {
                agent.SetDestination(targetpos);
            }
        }
        

    }
}
