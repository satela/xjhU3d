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

    public GameObject positiveModel;//主动攻击方

    public GameObject negtiveModel;//被动方

    private DBaseFightRole[] selfRoles = new DBaseFightRole[3];

    private DBaseFightRole[] enemyRoles = new DBaseFightRole[3];

    private Vector3[] positivePos = new Vector3[] { new Vector3(253.7f, 101.4f, 123.68f), new Vector3(257.45f, 101.4f, 140.0f), new Vector3(239.2442f, 101.4f, 118.5f) };

    private Vector3[] negtivePos = new Vector3[] { new Vector3(289.5f, 101.6f, 119.1f), new Vector3(299.4f, 101.6f, 128.2f), new Vector3(292.59f, 101.6f, 102.5688f) };

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
        if(GUI.Button(new Rect(700,10,50,20), "己方1"))
        {
            addtype = 0;
            posindex = 0;
            gname = "己方1";
        }
        if (GUI.Button(new Rect(760, 10, 50, 20), "己方2"))
        {
            addtype = 0;
            posindex = 1;
            gname = "己方2";

        }
        if (GUI.Button(new Rect(810, 10, 50, 20), "己方3"))
        {
            addtype = 0;
            posindex = 2;
            gname = "己方3";

        }
        if (GUI.Button(new Rect(700, 40, 50, 20), "敌方1"))
        {
            addtype = 1;
            posindex = 0;
            gname = "敌方1";

        }
        if (GUI.Button(new Rect(760, 40, 50, 20), "敌方2"))
        {
            addtype = 1;
            posindex = 1;
            gname = "敌方2";

        }
        if (GUI.Button(new Rect(810, 40, 50, 20), "敌方3"))
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
            DBaseFightRole fightroledata = AddRole(gname,0);

            selfRoles[posindex] = fightroledata;
            selfRoles[posindex].gameObject.transform.position = positivePos[posindex];
            selfRoles[posindex].gameObject.transform.eulerAngles = new Vector3(0, 120, 0);
        }
        else if (addtype == 1)
        {
            if (enemyRoles[posindex] != null)
            {
                Destroy(enemyRoles[posindex].gameObject);
                enemyRoles[posindex] = null;
            }
            DBaseFightRole fightroledata = AddRole(gname,1);

            enemyRoles[posindex] = fightroledata;
            enemyRoles[posindex].gameObject.transform.position = negtivePos[posindex];
            enemyRoles[posindex].gameObject.transform.eulerAngles = new Vector3(0, -60, 0);

        }
    }

    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public DBaseFightRole AddRole(string gname, int side)
    {
        GameObject fightrole = new GameObject(gname);
        DBaseFightRole fightroledata = fightrole.AddComponent<DBaseFightRole>();
        if(side == 0)
            fightroledata.setSide(side,positiveModel);
        else
            fightroledata.setSide(side, negtiveModel);
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


    //寻找技能攻击对象 priority 优先攻击对象策略
    public GameObject findAttackEnemy(GameObject attacker,EAttackStragety priority = EAttackStragety.EAttackStragety_Nearest)
    {
        if(attacker != null)
        {
            int attackside = attacker.GetComponent<DBaseFightRole>().side;

            List<DBaseFightRole> tempEnemy = new List<DBaseFightRole>();
            foreach(DBaseFightRole fightrole in allRoles)
            {
                if(fightrole.side != attackside)
                {
                    tempEnemy.Add(fightrole);
                }
            }

            return getEnemy(attacker, tempEnemy, priority);
        }

        return null;
    }

    public GameObject getEnemy(GameObject attacker,List<DBaseFightRole> enemyList,EAttackStragety priority = EAttackStragety.EAttackStragety_Nearest)
    {
        GameObject resultEnemy = null;
        switch(priority)
        {
            case EAttackStragety.EAttackStragety_Nearest:
                float distance = 100;
                foreach(DBaseFightRole enemy in enemyList)
                {
                    float tempdist = Vector3.Distance(attacker.transform.position,enemy.transform.position);
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
}
