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
    Freeze,
    ForbidSkill
}
public class DBuffData
{

    public int buffid = 1;

    public EBuffType bufftype = EBuffType.Temperary;
    public float effect_time = 1f; //持续时间

    public GameObject effect_prefab;// buff 特效

    public Dictionary<EBaseAttr, float> effectBaseAttr = new Dictionary<EBaseAttr, float>(); // 对大属性的影响

    public Dictionary<ESubAttr, float> effectSubAttr = new Dictionary<ESubAttr, float>(); // 对小属性的影响

    public string effurl;//buff特效

    public EFirePos buffEffPos = EFirePos.Top;//buff特效 显示位置


}
