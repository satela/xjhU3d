using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//寻找攻击对象策略，找最近的，最远的，主角，血最多的，血最少的，攻击力最强的，攻击力最弱的，怒气最多的，怒气最少的，等...
public enum EAttackStragety
{
    EAttackStragety_Nearest = 0,
    EAttackStragety_Fast,
    EAttackStragety_MainRole,
    EAttackStragety_MaxHp,
    EAttackStragety_MinHp,
    EAttackStragety_MaxAttack,
    EAttackStragety_MinAttack,
    EAttackStragety_MaxMp,
    EAttackStragety_MinMp,

    max

}
public class FightRoleManager : MonoBehaviour {

    public static FightRoleManager _instance;

    private List<DBaseFightRole> allRoles;

    public int positiveModel;//主动攻击方

    public int negtiveModel;//被动方

    private DBaseFightRole[] selfRoles = new DBaseFightRole[3];

    private DBaseFightRole[] enemyRoles = new DBaseFightRole[3];

    private Vector3[] positivePos = new Vector3[] { new Vector3(-9.035374f, 29f, -65.56f), new Vector3(-6.8f, 29f, -65.56f), new Vector3(-6.8f, 29f, -69f) };

    private Vector3[] negtivePos = new Vector3[] { new Vector3(-8.9f, 29f, -60f), new Vector3(-7f, 29f, -60f), new Vector3(-10f, 29f, -61.5f) };

   // public
    public void Awake()
    {
        _instance = this;
        allRoles = new List<DBaseFightRole>();
    }

    public void OnGUI()
    {
        int addtype = -1;
        int posindex = -1;
        string gname = "";
        if(GUI.Button(new Rect(800,10,50,20), "己方1"))
        {
            addtype = 0;
            posindex = 0;
            gname = "己方1";
        }
        if (GUI.Button(new Rect(860, 10, 50, 20), "己方2"))
        {
            addtype = 0;
            posindex = 1;
            gname = "己方2";

        }
        if (GUI.Button(new Rect(910, 10, 50, 20), "己方3"))
        {
            addtype = 0;
            posindex = 2;
            gname = "己方3";

        }
        if (GUI.Button(new Rect(800, 40, 50, 20), "敌方1"))
        {
            addtype = 1;
            posindex = 0;
            gname = "敌方1";

        }
        if (GUI.Button(new Rect(860, 40, 50, 20), "敌方2"))
        {
            addtype = 1;
            posindex = 1;
            gname = "敌方2";

        }
        if (GUI.Button(new Rect(910, 40, 50, 20), "敌方3"))
        {
            addtype = 1;
            posindex = 2;
            gname = "敌方3";

        }

        if (addtype == 0)
        {
            if (selfRoles[posindex] != null)
            {
                Destroy(selfRoles[posindex].gameObject);
                selfRoles[posindex] = null;
            }
            DBaseFightRole fightroledata = AddRole(gname, 0, positivePos[posindex], new Vector3(0, -60, 0));

            selfRoles[posindex] = fightroledata;
           // selfRoles[posindex].gameObject.transform.position = positivePos[posindex];
           // selfRoles[posindex].gameObject.transform.eulerAngles = new Vector3(0, -60, 0);
        }
        else if (addtype == 1)
        {
            if (enemyRoles[posindex] != null)
            {
                Destroy(enemyRoles[posindex].gameObject);
                enemyRoles[posindex] = null;
            }
            DBaseFightRole fightroledata = AddRole(gname, 1, negtivePos[posindex], new Vector3(0, 120, 0));

            enemyRoles[posindex] = fightroledata;
           // enemyRoles[posindex].gameObject.transform.position = negtivePos[posindex];
           // enemyRoles[posindex].gameObject.transform.eulerAngles = new Vector3(0, 120, 0);

        }
    }

    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public DBaseFightRole AddRole(string gname, int side,Vector3 pos,Vector3 rotate)
    {
        GameObject fightrole = new GameObject(gname);
        DBaseFightRole fightroledata = fightrole.AddComponent<DBaseFightRole>();
        if(side == 0)
            fightroledata.setSide(side, positiveModel, pos, rotate);
        else
            fightroledata.setSide(side, negtiveModel, pos, rotate);
        allRoles.Add(fightroledata);

        return fightroledata;
    }

    public void casterSkill(DSkillBaseData baseskilldata)
    {
        DBaseFightRole attackrole = selfRoles[0];
        if(attackrole != null)
        {
            attackrole.useSkill(baseskilldata);
        }

    }


    //寻找技能攻击对象 priority 优先攻击对象策略,isAttackEnemy 是攻击敌人，还是攻击己方，比如一些 加血，加防的技能攻击对象是己方，再比如混乱状态下 会攻击己方
    public GameObject findAttackEnemy(DBaseFightRole attacker, EAttackStragety priority = EAttackStragety.EAttackStragety_Nearest,bool isAttackEnemy = true)
    {
        if(attacker != null)
        {
            int attackside = attacker.side;

            List<DBaseFightRole> tempEnemy = new List<DBaseFightRole>();
            foreach(DBaseFightRole fightrole in allRoles)
            {
                if (isAttackEnemy && fightrole.side != attackside)
                {
                    tempEnemy.Add(fightrole);
                }
                else if (!isAttackEnemy && fightrole.side == attackside && fightrole != attacker)
                    tempEnemy.Add(fightrole);
            }

            return getEnemy(attacker, tempEnemy, priority);
        }

        return null;
    }

    public GameObject getEnemy(DBaseFightRole attacker,List<DBaseFightRole> enemyList,EAttackStragety priority = EAttackStragety.EAttackStragety_Nearest)
    {
        GameObject resultEnemy = null;
        switch(priority)
        {
            case EAttackStragety.EAttackStragety_Nearest:
                float distance = 100;
                foreach(DBaseFightRole enemy in enemyList)
                {
                    float tempdist = Vector3.Distance(attacker.rolePosition, enemy.rolePosition);
                    if(tempdist < distance)
                    {
                        resultEnemy = enemy.gameObject;
                        distance = tempdist;
                    }
                }
                break;
            case EAttackStragety.EAttackStragety_MainRole:
                foreach (DBaseFightRole enemy in enemyList)
                {
                    if(enemy.isMainRole)
                    {
                        resultEnemy = enemy.gameObject;
                        break;
                    }
                }
                break;
        }

        return resultEnemy;
    }

    public List<DBaseFightRole> getRolesBySide(int side)
    {
        List<DBaseFightRole> tempEnemy = new List<DBaseFightRole>();
        foreach (DBaseFightRole fightrole in allRoles)
        {
            if (fightrole.side == side)
            {
                tempEnemy.Add(fightrole);
            }
        }
        return tempEnemy;
    }
    public DBaseFightRole getTestAttacker()
    {
        if (selfRoles[0] != null)
            return selfRoles[0];
        //if (enemyRoles[0] != null)
        //    return enemyRoles[0];
        return null;
    }

    public DBaseFightRole getEnemuAttacker()
    {     
        if (enemyRoles[0] != null)
            return enemyRoles[0];
        return null;
    }

    public void setAutoFight()
    {
        foreach (DBaseFightRole role in allRoles)
            role.autoFight = !role.autoFight;
    }
    public float getFightRoleDistance(DBaseFightRole role1, DBaseFightRole role2)
    {
        return Vector3.Distance(role1.rolePosition, role2.rolePosition);
    }

    public List<GameObject> getHarmListByDist(int side, Vector3 centerpos, float distance, bool isAttackEnemy = true)
    {

        List<GameObject> harmlist = new List<GameObject>();
        List<DBaseFightRole> tempEnemy = new List<DBaseFightRole>();

        foreach (DBaseFightRole fightrole in allRoles)
        {           
            if (isAttackEnemy && fightrole.side != side)
            {
                tempEnemy.Add(fightrole);
            }
            else if (!isAttackEnemy && fightrole.side == side)
                tempEnemy.Add(fightrole);
        }



        foreach (DBaseFightRole fightrole in tempEnemy)
        {
            if (Vector3.Distance(centerpos, fightrole.rolePosition) - fightrole.roleRadius<= distance)
                harmlist.Add(fightrole.gameObject);
        }
        return harmlist;
    }

    public void roleDead(DBaseFightRole fightrole)
    {
        if (allRoles.Contains(fightrole))
            allRoles.Remove(fightrole);
    }

    #region 检测碰撞

    public bool isCollisionOther(DBaseFightRole role)
    {
        float size1 = role.agent.radius;
        float dist;
        bool iscollision = false;
        foreach (DBaseFightRole other in allRoles)
        {
            if (role != other)
            {
                dist = Vector3.Distance(role.rolePosition, other.rolePosition);

                Vector3 nextpos = role.rolePosition + (role.agent.destination - role.rolePosition).normalized * 0.1f;
                float nextdist = Vector3.Distance(nextpos, other.rolePosition);

                if (dist <= 2*size1 + other.agent.radius)
                {
                   // iscollision = true;

                    float destdit = Vector3.Distance(role.agent.destination, other.rolePosition);
                    if (destdit < size1 + other.agent.radius + 0.2)
                        iscollision = true;
                               
                }
            }
        }

        return iscollision;
    }
    #endregion

    #region 获取某个战斗单位一定距离范围内 可攻击的 点，防止几个 角色挤到一起
    public Vector3 getAttackPointByDist(DBaseFightRole attacker, DBaseFightRole target,float dist)
    {
        Vector3 vecToAttacker =  attacker.rolePosition -target.rolePosition;

        Vector3 normalDirect =  vecToAttacker.normalized * dist;

        Vector3 tempDirect;
        bool isOccupied = false;
        for (int i = 0; i < 6; i++)
        {
            tempDirect = target.rolePosition + Quaternion.Euler(0, 30 * i, 0)*normalDirect;
            isOccupied = false;

            foreach (DBaseFightRole other in allRoles)
            {
                if (other != attacker && other != target)
                {
                    float tempdist = Vector3.Distance(other.rolePosition, tempDirect);
                    if (tempdist < 3*(attacker.roleRadius + other.roleRadius))
                    {
                        isOccupied = true;
                        break;

                    }
                }
            }
            if (!isOccupied)
            {
                NavMeshPath path = new NavMeshPath();
                attacker.agent.CalculatePath(tempDirect, path);
                if (path.corners.Length >= 2)
                {
                    return tempDirect;
                }
            }

            tempDirect = target.rolePosition + Quaternion.Euler(0, -30 * i, 0) * normalDirect;
            isOccupied = false;

            foreach (DBaseFightRole other in allRoles)
            {
                if (other != attacker && other != target)
                {
                    float tempdist = Vector3.Distance(other.rolePosition, tempDirect);
                    if (tempdist < 3 * (attacker.roleRadius + other.roleRadius))
                    {
                        isOccupied = true;
                        break;

                    }
                }
            }
            if (!isOccupied)
            {
                NavMeshPath path = new NavMeshPath();
                attacker.agent.CalculatePath(tempDirect, path);
                if (path.corners.Length >= 2)
                {
                    return tempDirect;
                }
            }

        }
        return target.rolePosition;

    }
    #endregion
}
