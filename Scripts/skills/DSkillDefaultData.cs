using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MainSkillType //该角色优先使用技能类型，比如该角色主要用来攻击，主要用来控制对方（眩晕对方，降低对方攻击等），主要用来恢复己方状态（比如加血，加攻击，去除不利buff)
{
    MainSkillType_None = -1,
    MainSkillType_Attack,
    MainSkillType_Control,
    MainSkillType_Recovery
}

public class DSkillDefaultData  {


    public int id;
    public string skillname;

    public string description = "";

    public string icon = "";

    public Dictionary<int, int> buffself = new Dictionary<int, int>();

    public List<int> buffenemy = new List<int>();

    public float cdTime = 1;// 冷却时间

    public float needMp = 10;// 消耗怒气

    public Dictionary<int, int> attack_plus;

    public MainSkillType mianskillType;

    public void paresData(string datastr)
    {
        string[] propArray = datastr.Split(',');

        id = int.Parse(propArray[0]);
        skillname = propArray[1];

        description = propArray[2];
        icon = propArray[3];

        string[] buffdata = propArray[4].Split(';');
        string[] tempdata;
        for (int i = 0; i < buffdata.Length; i++)
        {
            tempdata = buffdata[i].Split('|');
            if (tempdata.Length > 1)
            {
                buffself.Add(int.Parse(tempdata[0]),int.Parse(tempdata[1]));
            }
        }

        buffdata = propArray[5].Split(';');

        for (int i = 0; i < buffdata.Length; i++)
        {
            if (buffdata[i] != "0")
                buffenemy.Add(int.Parse(buffdata[i]));
            
        }
        cdTime = float.Parse(propArray[6]);

        needMp = float.Parse(propArray[7]);

        tempdata = propArray[8].Split('|');
        attack_plus = new Dictionary<int, int>();
        if (tempdata.Length > 1)
        {
            attack_plus.Add(int.Parse(tempdata[0]), int.Parse(tempdata[1]));
        }

        mianskillType = (MainSkillType)(int.Parse(propArray[9]));
    }
}
