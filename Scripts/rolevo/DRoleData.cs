using UnityEngine;
using System.Collections;

public enum EBaseAttr
{
    Hp = 0,
    Phy_Attack,
    Mag_Attack,
    PhyDef,
    MagDef,
    Speed,

    Max
}

public enum ESubAttr
{
    Crit = 0, //暴击
    Dodge, //闪避
    Hit,  //命中
    Tough, //韧性

    Max
}
public class DRoleData  {

    public float[] roleBaseAttr = new float[(int)EBaseAttr.Max];
 
    public float[] roleSubAttr = new float[(int)ESubAttr.Max];

    public int max_hp = 100;//最大血量

    public float attack = 20;//基本伤害值

    public float attack_speed = 1;//普通攻击速度

    public float dodge = 0.1f;//闪避

    public float crit = 0.2f;//暴击

    public float hit = 0.5f;

    public float tough = 0.1f;



}
