using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EBuffType
{
    None = -1,
    Temperary, //即时生效buff，比如直接加血，加怒气
    OnGoing
}

//buff 其他效果，眩晕，冰冻，封住技能（无法技能攻击，只能普攻）等...
public enum EBuffSpecialType
{
    None = -1,
    Dissy,
    Chaos,//混乱
    Freeze,
    ForbidSkill
}
public class DBuffData
{

    public int buffid = 1;

    public string buffname = "";
    public string buffdesc = "";

    public EBuffType bufftype = EBuffType.Temperary;

    public EBuffSpecialType specialtype = EBuffSpecialType.None;

    public float duration = 1f; //持续时间

    public Dictionary<EBaseAttr, float> effectBaseAttr = new Dictionary<EBaseAttr, float>(); // 对大属性的影响

    public Dictionary<ESubAttr, float> effectSubAttr = new Dictionary<ESubAttr, float>(); // 对小属性的影响

    public string effurl;//buff特效

    public EFirePos buffEffPos = EFirePos.Top;//buff特效 显示位置

    public void paresData(string datastr)
    {
        string[] propArray = datastr.Split(',');

        buffid = int.Parse(propArray[0]);
        buffname = propArray[1];

        buffdesc = propArray[2];
        bufftype = (EBuffType)int.Parse(propArray[3]);

        string[] buffdata = propArray[4].Split(';');
        string[] tempdata;
        for (int i = 0; i < buffdata.Length; i++)
        {
            tempdata = buffdata[i].Split('|');
            if (tempdata.Length > 1)
            {
                effectBaseAttr.Add((EBaseAttr)int.Parse(tempdata[0]), float.Parse(tempdata[1])/100f);
            }
        }

        buffdata = propArray[5].Split(';');

        for (int i = 0; i < buffdata.Length; i++)
        {

            tempdata = buffdata[i].Split('|');
            if (tempdata.Length > 1)
            {
                effectSubAttr.Add((ESubAttr)int.Parse(tempdata[0]), float.Parse(tempdata[1]) / 100f);
            }

        }
        specialtype = (EBuffSpecialType)int.Parse(propArray[6]);
        duration = int.Parse(propArray[7]);
        buffEffPos = (EFirePos)int.Parse(propArray[8]);
        effurl = propArray[9];

    }
}
