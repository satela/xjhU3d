using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//技能基本配置

public enum ESkillType
{
    None = -1,
    Attack, //攻击
    BuffEnemy,//给敌人加buff
    BuffSelf,//给己方加buff
    BuffAll, //都加buff
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

public enum EBeatonToBackFly
{
    None,
    Back,
    Fly
}
public class DSkillBaseData  {

    public int id = 0;

  //  public string icon;

  //  public string des;

    public string skillName = "霹雳掌";

  //  public ESkillDist near_farAtk;
    public float minAttackDist = 2; // 最小施放距离 即当有敌人和释放者 距离小于该距离时可以施放技能

    public ESkillType skilltype = 0;

    public eAnimatorState animatorClip;//施放动作

    public string fireEffUrl; //施放特效
    public float fireTime = 0;//特效施放 时间，从开始 施放技能 计算时间

    public EFirePos fireEffPos;

    public bool isUseMoveEff;//是否使用移动特效，比如朝着敌人 飞去，如果需要指定施放地点，也可以朝施放地点飞去

    public string moveEffUrl;//移动特效

    public bool isSingleMove = true;//是否只有一个移动特效，单体攻击技能最多只有一个，群攻 可能一个，可能多个，为 true 时，有几个被击 怪物则有几个移动特效


    public float moveBeginTime = 0;//移动特效 开始移动时间

    public bool isQunGong = false;//是否群攻技能

    public float harmDist = 1; //群攻 伤害 有效距离，即离 施放点 多远的怪物才会受到伤害

    public bool isNeedAppoint = false;//是否需要指点施放地点 不指定则使用 施放者位置

    public string explodeEffUrl = "";//如果指定施放地点，一般在施放地点产生爆炸特效


    public bool isShakeCamera = true;//施放震屏
    public float shakeTime = 0.6f;

    public List<BeatonData> beatonDatas = new List<BeatonData>();

    public string getBeatonStr()
    {
        string xmlstr = "";
        for (int i = 0; i < beatonDatas.Count; i++)
        {
            xmlstr += beatonDatas[i].ToString();
            if (i != beatonDatas.Count - 1)
                xmlstr += "|";
        }

        return xmlstr;
    }

    public void parseBeatonStr(string xmlstr)
    {
        if (beatonDatas != null)
            beatonDatas.Clear();
        string[] beatonliststr = xmlstr.Split('|');

        BeatonData beatdata;
        string[] tempstr;
        for (int i = 0; i < beatonliststr.Length; i++)
        {
            tempstr = beatonliststr[i].Split(',');
            if(tempstr.Length == 4)
            {
                beatdata = new BeatonData();
                beatdata.animatorBeatonClip = (eAnimatorState)(int.Parse(tempstr[0]));
                beatdata.beatonEffUrl = tempstr[1];
                beatdata.beatonTime = float.Parse(tempstr[2]);
                beatdata.eBeatonbackFly = (EBeatonToBackFly)(int.Parse(tempstr[3]));
                beatonDatas.Add(beatdata);
            }
           
        }
    }
}

public class BeatonData
{
    public EBeatonToBackFly eBeatonbackFly = EBeatonToBackFly.None;

    public eAnimatorState animatorBeatonClip = eAnimatorState.beaten;//施放受击动作

    public string beatonEffUrl;// 受击特效

    public float beatonTime = 0.5f;//受击时间 没有移动攻击特效 时有效，否则 当移动攻击特效 达到受击者 时发生受击事件

    public override string ToString()
    {
        return ((int)animatorBeatonClip).ToString() + "," + beatonEffUrl + "," + beatonTime.ToString() + "," + ((int)eBeatonbackFly).ToString();
        
    }
}