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
    Dissy,
    Chaos,//混乱，会攻击己方
    Freeze,
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

    public DRoleData roledata;//人物基本属性

    public List<DBuffData> buffdataList = new List<DBuffData>();//人物所有受到的buff；
   
    private DAnimatorController animatorControl;
    public NavMeshAgent agent;
   // private NavMeshObstacle obstacle;

    private CharacterController cc;


    public AudioClip sound_miss;    

    public Transform target;


    public bool isCasteringSkill = false; //当前是否还处在施放技能中

    private SkillCasterData casterdata;

    public  const float NEAR_ATTACK_DISTANCE = 2f;

    public int side = 0; //战斗中处于哪一方，默认0 是自己方，1 是敌方

    public bool isMainRole = false;//是否队伍中的主角

    public GameObject roleModel;

    public ESkillStep skillstep = ESkillStep.None;

    private bool isDead = false;
    private HpBarControl hpbarui;

    private FightRoleSkill skillManager;

    private float bodyRadius = 0;
    private float bodyHeight = 0;

    public Vector3 destpoint = Vector3.zero;

    private FollowFighter followfight;
    public void setSide(int fside,int roleid,Vector3 pos,Vector3 rotate)
    {
        side = fside;

        roledata = new DRoleData(roleid);

       // GameObject rolemodel = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/RPG/actors/" + roledata.defaultRoledata.modelUrl + ".prefab",typeof(GameObject)) as GameObject;

        GameObject rolemodel = ResourceManager.loadAsset<GameObject>("Assets/RPG/actors/" + roledata.defaultRoledata.modelUrl + ".prefab");
        roleModel = GameObject.Instantiate(rolemodel) as GameObject;
        //agent = roleModel.GetComponent<NavMeshAgent>();
        roleModel.transform.position = pos;
        roleModel.transform.parent = transform;
        //if(fside == 0)
       // roleModel.tag = Tags.enemy;

        rolelastpos = pos;
        roleModel.transform.localEulerAngles = rotate;
       // roleModel.transform.localScale = new Vector3(8, 8, 8);
        animatorControl = roleModel.AddComponent<DAnimatorController>();
        cc = roleModel.GetComponent<CharacterController>();
        bodyRadius = cc.radius;
        bodyHeight = cc.height;
        cc.enabled = false;

        agent = roleModel.AddComponent<NavMeshAgent>();
       // obstacle = roleModel.AddComponent<NavMeshObstacle>();
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        agent.autoRepath = true;
        agent.radius = bodyRadius;
        agent.height = bodyHeight;
      //  obstacle.radius = bodyRadius;
       // obstacle.carving = true;
        skillManager = gameObject.AddComponent<FightRoleSkill>();
        skillManager.initSkill(roledata.defaultRoledata.skillIdList);

        followfight = gameObject.AddComponent<FollowFighter>();

        hpbarui = gameObject.AddComponent<HpBarControl>();

        StartCoroutine(resetModelPos());
        

    }

    IEnumerator resetModelPos()
    {
        yield return  null;
       // roleModel.transform.localPosition = Vector3.zero + Vector3.up * 0.26f;
       // roleModel.transform.position = transform.position;
        animatorControl.resetToIdle();
        //agent.enabled = true;
       // gotoDestination(rolePosition);

    }

    public bool canControled()
    {
        if (isCasteringSkill)
            return false;
        checkRoleState();

        if (fightstate == RoleFightState.Freeze)
            return false;
        if (animatorControl.currentState != eAnimatorState.await && animatorControl.currentState != eAnimatorState.arun)
            return false;
      

        return true;
    }
    private bool canUseSkillNow()
    {
        if (isCasteringSkill)
            return false;
        checkRoleState();

        if (fightstate == RoleFightState.Dissy || fightstate == RoleFightState.Freeze)
            return false;
        if (animatorControl.currentState != eAnimatorState.await)
            return false;
        //if (agent.enabled == false)// agent.enabled = false 一般这个 时候 角色 处于被击飞 击退的 时候
        //    return false;

        return true;
    }
    //使用技能发动攻击
    public bool useSkill(DSkillBaseData baseSkilldata)
    {
        if (!canUseSkillNow())
            return false;

        if (baseSkilldata != null)
        {
            GameObject firstenemy = FightRoleManager._instance.findAttackEnemy(this, EAttackStragety.EAttackStragety_Nearest, isAttackEnemy(baseSkilldata));

            if (firstenemy != null)
            {
                isCasteringSkill = true;
                casterdata = new SkillCasterData();
                casterdata.castRole = gameObject;
                casterdata.skilldata = baseSkilldata;

                float dist = FightRoleManager._instance.getFightRoleDistance(this, firstenemy.GetComponent<DBaseFightRole>());
                roleModel.transform.LookAt(firstenemy.GetComponent<DBaseFightRole>().roleModel.transform, Vector3.up);

                if (skillManager != null)
                {
                    skillManager.useSkill(baseSkilldata);
                }
                if (dist > DefaultSkillParam.PathFindingDist)//当距离大于可自动寻路距离，不作攻击，只表现一下施放动作和特性
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
            else
            {
                if (skillManager != null)
                {
                    skillManager.useSkill(baseSkilldata);
                }
                isCasteringSkill = true;
                casterdata = new SkillCasterData();
                casterdata.castRole = gameObject;
                casterdata.skilldata = baseSkilldata;
                StartCoroutine(showAttackMov());
                return true;
            }
        }
        return false;

    }


    void showFireEff()
    {
        //BoxCollider boxcld = gameObject.GetComponent<BoxCollider>();
        Vector3 firepos = Vector3.zero;
        if (casterdata.skilldata.fireEffPos == EFirePos.Center)
        {
            firepos = rolePosition;
        }
        else if (casterdata.skilldata.fireEffPos == EFirePos.Forward)
        {
            firepos = rolePosition + roleModel.transform.forward * agent.radius;
        }
        else if (casterdata.skilldata.fireEffPos == EFirePos.Back)
        {
            firepos = rolePosition - roleModel.transform.forward * agent.radius;
        }
        else if (casterdata.skilldata.fireEffPos == EFirePos.Top)
        {
            firepos = rolePosition + roleModel.transform.up * agent.height / 2;
        }
        else if (casterdata.skilldata.fireEffPos == EFirePos.Bottom)
        {
            firepos = rolePosition - roleModel.transform.up * agent.height / 2 + Vector3.up * 0.2f;
        }
        else if (casterdata.skilldata.fireEffPos == EFirePos.Left)
        {
            firepos = rolePosition - roleModel.transform.right * agent.radius;
        }
        else if (casterdata.skilldata.fireEffPos == EFirePos.Right)
        {
            firepos = rolePosition + roleModel.transform.right * agent.radius;
        }

        GameObject attackEff = new GameObject(casterdata.skilldata.fireEffUrl);
        EffectAsset effectasset = attackEff.AddComponent<EffectAsset>();

        effectasset.setEffectParam(casterdata.skilldata.fireEffUrl, firepos, roleRotation, EEffectType.Attack);
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

        AddBuff();
        isCasteringSkill = false;
        


    }
    IEnumerator gotoEnemy()
    {
        while (true)
        {
            yield return null;
            GameObject firstenemy = FightRoleManager._instance.findAttackEnemy(this, EAttackStragety.EAttackStragety_Nearest, isAttackEnemy(casterdata.skilldata));
            if (firstenemy != null)
            {
                float dist = FightRoleManager._instance.getFightRoleDistance(this, firstenemy.GetComponent<DBaseFightRole>()) - firstenemy.GetComponent<DBaseFightRole>().roleRadius;

                if (dist > casterdata.skilldata.minAttackDist)
                {
                    Vector3 destpoint = FightRoleManager._instance.getAttackPointByDist(this, firstenemy.GetComponent<DBaseFightRole>(), casterdata.skilldata.minAttackDist);
                    gotoDestination(destpoint);
                    if (FightRoleManager._instance.isCollisionOther(this))
                    {
                      //  gotoDestination(rolePosition);
                        isCasteringSkill = false;
                        break;
                    }
                }
                else
                {
                   // agent.enabled = true;
                    gotoDestination(rolePosition);

                    roleModel.transform.LookAt(firstenemy.GetComponent<DBaseFightRole>().roleModel.transform, Vector3.up);
                   // animatorControl.resetToIdle();
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
                //agent.enabled = true;
                gotoDestination(rolePosition);
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
        if (casterdata != null && casterdata.skilldata != null)
        {
            DSkillDefaultData skillDefdata;

            if (ConfigManager.intance.skillDefaultDic.TryGetValue(casterdata.skilldata.id, out skillDefdata))
            {
                DBuffData buffdata;
                List<DBaseFightRole> selfside = FightRoleManager._instance.getRolesBySide(side);
                foreach (int buffid in skillDefdata.buffself.Keys)
                {
                    buffdata = ConfigManager.intance.basebuffDataDic[buffid];

                    if (skillDefdata.buffself[buffid] == 0)
                        addBuffBydata(buffdata);
                    else
                    {
                        foreach (DBaseFightRole role in selfside)
                            role.addBuffBydata(buffdata);
                    }
                }
               
            }

        }

    }

    #region buff处理

    //private List<DBuffData>
    public void addBuffBydata(DBuffData buffdata)
    {
        //持续性buff 不能叠加
        if (buffdata.bufftype == EBuffType.OnGoing && hasBuff(buffdata.buffid))
            return;

        GameObject buffbody = new GameObject("buff(" + buffdata.buffname+")");

        buffdataList.Add(buffdata);

        foreach (EBaseAttr attrkey in buffdata.effectBaseAttr.Keys)
        {
            roledata.addBaseBuffAttr(attrkey, buffdata.effectBaseAttr[attrkey] * roledata.getOriginBaseAttrByType(attrkey));
        }

        foreach (ESubAttr attrkey in buffdata.effectSubAttr.Keys)
        {
            roledata.addSubBuffAttr(attrkey, buffdata.effectSubAttr[attrkey] * roledata.getOriginSubAttrByType(attrkey));
        }

        BuffInstance buffeff = buffbody.AddComponent<BuffInstance>();
        buffeff.setBuffData(buffdata, this);
        hpbarui.updateHp();

    }
    private bool hasBuff(int buffid)
    {
        foreach(DBuffData bdata in buffdataList)
        {
            if(bdata.buffid == buffid)
                return true;
        }

        return false;
    }
    public void removeBuffData(DBuffData buffdata)
    {
        if (buffdataList.Contains(buffdata))
        {
            buffdataList.Remove(buffdata);

            foreach (EBaseAttr attrkey in buffdata.effectBaseAttr.Keys)
            {
                if (buffdata.bufftype == EBuffType.OnGoing)
                roledata.addBaseBuffAttr(attrkey, -buffdata.effectBaseAttr[attrkey] * roledata.getOriginBaseAttrByType(attrkey));
            }

            foreach (ESubAttr attrkey in buffdata.effectSubAttr.Keys)
            {
                if(buffdata.bufftype == EBuffType.OnGoing)
                roledata.addSubBuffAttr(attrkey, -buffdata.effectSubAttr[attrkey] * roledata.getOriginSubAttrByType(attrkey));
            }
        }
    }

    //判断角色是否处于眩晕，冰冻等状态中,如果同时眩晕，冰冻，以值 较大的 作为当前状态，冰冻 比眩晕 值大，所以认为处于冰冻状态
    public  void checkRoleState()
    {
       // RoleFightState rolestate = RoleFightState.Patrol;

        List<int> buffsepType = new List<int>();
        foreach(DBuffData buffdata in buffdataList)
        {
            buffsepType.Add((int)buffdata.specialtype);
        }

        int maxstate = -1;
        for (int i = 0; i < buffsepType.Count; i++)
        {
            if (buffsepType[i] > maxstate)
                maxstate = buffsepType[i];
        }
        switch ((EBuffSpecialType)maxstate)
        {
            case EBuffSpecialType.None:
                fightstate = RoleFightState.Patrol;
                break;
            case EBuffSpecialType.Freeze:
                fightstate = RoleFightState.Freeze;
                break;
            case EBuffSpecialType.Dissy:
                fightstate = RoleFightState.Dissy;
                break;
            case EBuffSpecialType.ForbidSkill:
                break;
        }

    }
    #endregion
    // Use this for initialization
	void Start () {
	
	}


    private Vector3 rolelastpos;
	// Update is called once per frame
	void Update () {

        if (skillstep == ESkillStep.Selecting && Input.GetMouseButtonDown(0))
        {
          OnLockTarget();
        }
        if (agent != null)
        {
            if (agent.enabled && agent.remainingDistance > 0.1)
            {
                if (FightRoleManager._instance.isCollisionOther(this))
                {
                    gotoDestination(rolePosition);
                  
                }
                else
                {                  
                    animatorControl.changeToState(eAnimatorState.arun);
                }
                
            }
            else if (agent.enabled && agent.remainingDistance <= 0.1)
            {
               
                if(animatorControl.currentState == eAnimatorState.arun)
                animatorControl.resetToIdle();
            }
        }
        if ((animatorControl.currentState == eAnimatorState.arun && !agent.enabled) || (animatorControl.currentState == eAnimatorState.arun && agent.enabled && agent.remainingDistance <= 0.1))
            animatorControl.resetToIdle();

        checkUseSkill();
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
            Vector3 pos = rolePosition + roleModel.transform.forward * 0.5f + Vector3.up;
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
                Vector3 pos = rolePosition + roleModel.transform.forward + Vector3.up;
                effectasset.setEffectParam(casterdata.skilldata.moveEffUrl, pos, Quaternion.identity, EEffectType.Move);

                MoveEffect effectmove = moveEff.AddComponent<MoveEffect>();
                effectmove.SetTargetPos(firstfightEnemy, casterdata.clone());
                effectmove.transform.rotation = roleRotation;
            }
            else
            {
                casterdata.beatonRoles = FightRoleManager._instance.getHarmListByDist(side, rolePosition, casterdata.skilldata.harmDist,isAttackEnemy(casterdata.skilldata));
                foreach (GameObject enemy in casterdata.beatonRoles)
                {
                    GameObject moveEff = new GameObject(casterdata.skilldata.moveEffUrl);
                    EffectAsset effectasset = moveEff.AddComponent<EffectAsset>();
                    Vector3 pos = rolePosition + roleModel.transform.forward + Vector3.up;
                    effectasset.setEffectParam(casterdata.skilldata.moveEffUrl, pos, Quaternion.identity, EEffectType.Move);

                    MoveEffect effectmove = moveEff.AddComponent<MoveEffect>();
                    effectmove.SetTargetPos(enemy, casterdata.clone());
                    effectmove.transform.rotation = roleRotation;

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
            casterdata.beatonRoles = FightRoleManager._instance.getHarmListByDist(side, rolePosition, casterdata.skilldata.harmDist, isAttackEnemy(casterdata.skilldata));
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

            effectasset.setEffectParam(beatondata.beatonEffUrl, rolePosition + Vector3.up, Quaternion.identity, EEffectType.Beaton);
        }

        if (beatondata.eBeatonbackFly == EBeatonToBackFly.None)
        {
            if (animatorControl.currentState == eAnimatorState.await || animatorControl.currentState == eAnimatorState.beaten || animatorControl.currentState == eAnimatorState.fall)
                animatorControl.changeToState(beatondata.animatorBeatonClip);
            yield return null;
        }
        if (beatondata.eBeatonbackFly == EBeatonToBackFly.Back)
        {
            Vector3 backpos = rolePosition - roleModel.transform.forward * DefaultSkillParam.BeatonBackMaxDist;

            if (agent != null && agent.enabled) //被击飞的时候 不能同时被击退，怕掉到 建筑物 上面
            {
                //agent.enabled = true;
                NavMeshPath path = new NavMeshPath();
                agent.CalculatePath(backpos, path);
                if (path.corners.Length == 2)
                {
                    agent.enabled = false;
                    iTween.MoveTo(roleModel, iTween.Hash("position", backpos, "easeType", "easeOutQuart", "delay",0, "time", 0.3f, "oncomplete", "resetAgent"));
                    yield return new WaitForSeconds(0.3f);
                    agent.enabled = true;

                    if (animatorControl.currentState == eAnimatorState.await || animatorControl.currentState == eAnimatorState.beaten ||animatorControl.currentState == eAnimatorState.fall)
                    animatorControl.changeToState(beatondata.animatorBeatonClip);

                }
                else
                {
                    if (animatorControl.currentState == eAnimatorState.await || animatorControl.currentState == eAnimatorState.beaten || animatorControl.currentState == eAnimatorState.fall)
                        animatorControl.changeToState(beatondata.animatorBeatonClip);
                }
            }
            else
            {
                if (animatorControl.currentState == eAnimatorState.await || animatorControl.currentState == eAnimatorState.beaten || animatorControl.currentState == eAnimatorState.fall)
                    animatorControl.changeToState(beatondata.animatorBeatonClip);
            }
        }
        else if (beatondata.eBeatonbackFly == EBeatonToBackFly.Fly && agent.enabled)
        {
            agent.enabled = false;
            if (animatorControl.currentState == eAnimatorState.await || animatorControl.currentState == eAnimatorState.beaten ||animatorControl.currentState == eAnimatorState.fall)
                animatorControl.changeToState(beatondata.animatorBeatonClip);

            Vector3 originpos = rolePosition;
            Vector3 topos = rolePosition + Vector3.up * DefaultSkillParam.BeatonUpMaxDist;
            iTween.MoveTo(roleModel, iTween.Hash("position", topos, "easeType", "easeOutCirc", "delay", 0, "time", 0.3f));
            yield return new WaitForSeconds(0.33f);

            iTween.MoveTo(roleModel, iTween.Hash("position", originpos, "easeType", "easeInCubic", "delay", 0, "time", 0.3f));
            yield return new WaitForSeconds(0.35f);

            agent.enabled = true;

        }
    }


    public void showBeatonBySkill(SkillCasterData casterdata)
    {
            DRoleData castRoledata = casterdata.castRole.GetComponent<DBaseFightRole>().roledata;

        //攻击技能才能闪避，如果是 加buff 的 不能闪避
            if (casterdata.skilldata.isAttackSkill() && !FightCalculateTool.checkIsHitted(castRoledata, roledata))
            {
                //闪避了
                showDogeCritEffect(EEffectType.Dodge);
                return;
            }
            if (casterdata != null && casterdata.skilldata != null)
            {
                BeatonData beatdata;
                for (int i = 0; i < casterdata.skilldata.beatonDatas.Count; i++)
                {
                    beatdata = casterdata.skilldata.beatonDatas[i];

                    if(casterdata.skilldata.isAttackSkill())
                        StartCoroutine(beantonBlood(castRoledata, casterdata.skilldata, beatdata.beatonTime));
                    if (isDead)
                        break;
                    StartCoroutine(beatonToBackOrFly(beatdata));
                    if(i==0)
                        StartCoroutine(addBuffByBeaton(casterdata.skilldata, beatdata.beatonTime));
                }
                
                //animatorControl.changeToState(casterdata.skilldata.animatorBeatonClip);
            }
    }
    //被击buff
    IEnumerator addBuffByBeaton(DSkillBaseData baseskilldata,float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
         DSkillDefaultData skillDefdata = null;

         if (ConfigManager.intance.skillDefaultDic.ContainsKey(baseskilldata.id))
            skillDefdata = ConfigManager.intance.skillDefaultDic[baseskilldata.id];
         if (skillDefdata != null)
         {
             DBuffData buffdata;
             foreach (int buffid in skillDefdata.buffenemy)
             {
                 buffdata = ConfigManager.intance.basebuffDataDic[buffid];
                 if (buffdata != null)
                 {
                     addBuffBydata(buffdata);
                 }
             }

         }
         hpbarui.updateHp();
    }
    //被击伤害

    IEnumerator beantonBlood(DRoleData castRoleData, DSkillBaseData baseskilldata, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        int phyPlus = 0;
        int magPlus = 0;
        if (ConfigManager.intance.skillDefaultDic.ContainsKey(baseskilldata.id))
        {
            DSkillDefaultData defaultskill = ConfigManager.intance.skillDefaultDic[baseskilldata.id];
            if (defaultskill.attack_plus.ContainsKey((int)EBaseAttr.Phy_Attack))
            {
                phyPlus = (int)castRoleData.getBaseAttrByType(EBaseAttr.Phy_Attack) * defaultskill.attack_plus[(int)EBaseAttr.Phy_Attack]/100;
            }
            if (defaultskill.attack_plus.ContainsKey((int)EBaseAttr.Mag_Attack))
            {
                magPlus = (int)castRoleData.getBaseAttrByType(EBaseAttr.Mag_Attack) * defaultskill.attack_plus[(int)EBaseAttr.Mag_Attack]/100;
            }
        }
        int harm = FightCalculateTool.calculateHarm(castRoleData, roledata, phyPlus, magPlus);
        roledata.addBaseBuffAttr(EBaseAttr.Hp, -harm);

        hpbarui.updateHp();
        if (roledata.getBaseAttrByType(EBaseAttr.Hp) <=0 )
        {
            isDead = true;
            doDead();
        }
    }

    void doDead()
    {
        StopAllCoroutines();
        animatorControl.changeToState(eAnimatorState.die);
        gotoDestination(rolePosition);
        FightRoleManager._instance.roleDead(this);

        Invoke("destoryRole", 6);
    }

    void destoryRole()
    {
        Destroy(gameObject);
        Destroy(hpbarui.gameObject);

    }
    public void gotoPoint(Vector3 targetpos)
    {
        if(agent != null && agent.enabled)
        {
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(targetpos, path);
            if (path.corners.Length >= 2)
            {
                agent.enabled = true;
                agent.SetDestination(targetpos);
            }
        }
        
    }

    #region 显示 闪避，暴击 等特效

    
    void showDogeCritEffect(EEffectType type)
    {
        GameObject effect = new GameObject(type.ToString());
        effect.AddComponent<EffectAsset>().setEffectParam("", rolePosition + Vector3.up, roleRotation, type);
    }
    #endregion
    #region 角色大小 位置接口
    public float roleRadius
    {
        get{
            return bodyRadius;
        }
       
    }

    public float roleHeight
    {
        get
        {
            return bodyHeight;
        }
        
    }

    public Vector3 rolePosition
    {
        get
        {
            if (roleModel != null)
                return roleModel.transform.position;
            else
                return Vector3.zero;
        }
        
    }

    public Quaternion roleRotation
    {
        get
        {
            if (roleModel != null)
                return roleModel.transform.rotation;
            else
                return Quaternion.identity;
        }
        
    }

    #endregion

    #region 本次技能是攻击敌人还是己方
    private bool isAttackEnemy(DSkillBaseData skilldata)
    {
        if (fightstate == RoleFightState.Chaos)
            return false;
        else return skilldata.isAttackSkill();
    }

    #endregion

    #region 移动接口
    public void gotoDestination(Vector3 despoint)
    {
        StartCoroutine(gotoTargetPoint(despoint));
    }

    IEnumerator gotoTargetPoint(Vector3 despoint)
    {
        yield return null;
        NavMeshPath path = new NavMeshPath();
         agent.enabled = true;
         //agent.Stop();

         agent.CalculatePath(despoint, path);
         if (path.corners.Length >= 2)
         {
             rolelastpos = rolePosition;
             agent.SetDestination(despoint);
        }
    }
    #endregion

    #region 自动战斗

    private bool _isAutoFight = false;
    public bool autoFight
    {
        get{ return _isAutoFight;}

        set
        {
            _isAutoFight = value;
        }
    }

    void checkUseSkill()
    {
        if (_isAutoFight)
        {
            if (skillManager != null)
            {
                if (skillManager.getCanUseSkill() != null)
                {
                    useSkill(skillManager.getCanUseSkill());
                }
            }
        }
    }
    #endregion

}


