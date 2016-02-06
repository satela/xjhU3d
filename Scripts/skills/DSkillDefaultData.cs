using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DSkillDefaultData  {


    public int id;
    public string skillname;

    public string description = "";

    public string icon = "";

    public Dictionary<int, int> buffself = new Dictionary<int, int>();

    public List<int> buffenemy = new List<int>();

    public float cdTime = 1;// 冷却时间

    public float needMp = 10;// 消耗怒气

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

    }
}
