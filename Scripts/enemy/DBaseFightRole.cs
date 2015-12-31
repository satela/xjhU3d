using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    public GameObject roleModel;
    public void setSide(int fside,GameObject modelprefab)
    {
        side = fside;
        roleModel = GameObject.Instantiate(modelprefab) as GameObject;
        roleModel.transform.parent = transform;
        agent = roleModel.GetComponent<NavMeshAgent>();

        roleModel.transform.localPosition = Vector3.zero;
        roleModel.transform.localScale = new Vector3(8, 8, 8);
        animatorControl = roleModel.AddComponent<DAnimatorController>();

        StartCoroutine(resetModelPos());
        roledata = new DRoleData();

    }

    IEnumerator resetModelPos()
    {
        yield return new WaitForSeconds(1f);      
        roleModel.transform.localPosition = Vector3.zero;
        animatorControl.changeToState(eAnimatorState.await);
        agent.SetDestination(transform.position);

    }
    public bool casterSkill(SkillCasterData skillcastdata)
    {

        if (isCasteringSkill)
            return false;

        if (skillcastdata != null && skillcastdata.skilldata != null)
        {
            isCasteringSkill = true;
            casterdata = skillcastdata;
            StartCoroutine(gotoEnemy());
        }

        return true;
    }

    IEnumerator gotoEnemy()
    {
        if (casterdata.skilldata.near_farAtk == ESkillDist.NearAttack)
        {
            if (casterdata.beatonRoles.Length > 0)
            {
                GameObject enemy = casterdata.beatonRoles[0];
                if (enemy != null)
                {
                    float dist = Vector3.Distance(transform.position, enemy.transform.position);
                    if (dist <= NEAR_ATTACK_DISTANCE)
                    {
                        transform.LookAt(enemy.transform, Vector3.up);
                        beginAttack();
                    }

                    else
                    {
                        agent.SetDestination(enemy.transform.position);
                        yield return null;
                        while (dist > NEAR_ATTACK_DISTANCE)
                        {
                            yield return null;
                            dist = Vector3.Distance(transform.position, enemy.transform.position);
                        }
                        beginAttack();
                    }
                }
            }
        }
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
	
       
	}
}
