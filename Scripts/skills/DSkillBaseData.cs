using UnityEngine;
using System.Collections;

//技能基本配置

public enum ESkillType
{
    None = -1,
    NormalAttack, //普通攻击
    SkillAttack,
    Max
}

public enum ESkillDist
{
    None = -1,
    NearAttack, //近攻
    FarAttack, //远攻
    Max
}

//施放特效位置
public enum EFirePos
{
    None = -1,
    Center,
    Forward,
    Back,
    Top,
    Bottom,
    Left,
    Right,

    Max
}
public class DSkillBaseData  {

    public int id = 0;

    public string icon;

    public string des;

    public string skillName = "霹雳掌";

    public ESkillDist near_farAtk;
    public float minAttackDist = 2; // 最小施放距离 远攻 单体攻击时有效

    public ESkillType skilltype = 0;

    public eAnimatorState animatorClip;//施放动作

    public string fireEffUrl; //施放特效
    public float fireTime = 0;//特效施放 时间，从开始 施放技能 计算时间

    public EFirePos fireEffPos;

    public bool isUseMoveEff;//是否使用移动特效，比如朝着敌人 飞去

    public string moveEffUrl;//移动特效

    public bool isSingleMove = true;//是否只有一个移动特效，单体攻击技能最多只有一个，群攻 可能一个，可能多个，为 true 时，有几个被击 怪物则有几个移动特效


    public float moveBeginTime = 0;//移动特效 开始移动时间

    public bool isQunGong = false;//是否群攻技能

    public float harmDist = 1; //群攻 伤害 有效距离，即离 施放点 多远的怪物才会受到伤害

    public bool isNeedAppoint = false;//是否需要指点施放地点 不指定则使用 施放者位置

    public  eAnimatorState animatorBeatonClip;//施放受击动作

    public string beatonEffUrl;// 受击特效

    public float beatonTime = 0.5f;//受击时间 没有移动攻击特效 时有效，否则 当移动攻击特效 达到受击者 时发生受击事件







}
