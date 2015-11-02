using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SkillsInfo : MonoBehaviour
{

    // Use this for initialization

    public TextAsset skilltext;

    public static SkillsInfo _instance;


    private Dictionary<int, SkillInfo> skillInfoDic = new Dictionary<int, SkillInfo>();
    void Awake()
    {

        _instance = this;
        initSkillInfoDic();
    }

    public List<SkillInfo> getHeroSkill(EHeroType herotype)
    {
        List<SkillInfo> list = new List<SkillInfo>();

        if(herotype == EHeroType.Magician)
        {
            foreach(SkillInfo info in skillInfoDic.Values)
            {
                if (info.applicableRole == ApplicableRole.Magician)
                    list.Add(info);
            }
        }
        else if (herotype == EHeroType.Swordman)
        {
            foreach (SkillInfo info in skillInfoDic.Values)
            {
                if (info.applicableRole == ApplicableRole.Swordman)
                    list.Add(info);
            }
        }
        return list;

    }

    // Update is called once per frame
    void initSkillInfoDic()
    {

        string text = skilltext.text;
        string[] strArray = text.Split('\n');

        SkillInfo obj;
        foreach (string str in strArray)
        {
            obj = new SkillInfo();
            string[] propArray = str.Split(',');
            obj.id = int.Parse(propArray[0]);
            obj.skillname = propArray[1];
            obj.icon = propArray[2];
            obj.des = propArray[3];
            switch (propArray[4])
            {
                case "Passvie":
                    obj.applyType = ApplyType.Passive;
                    break;
                case "Buff":
                    obj.applyType = ApplyType.Buff;
                    break;
                case "SingleTarget":
                    obj.applyType = ApplyType.SingleTarget;
                    break;
                case "MultiTarget":
                    obj.applyType = ApplyType.MultiTarget;
                    break;

            }
            switch (propArray[5])
            {
                case "Attack":
                    obj.applyProperty = ApplyProperty.Attack;
                    break;
                case "Def":
                    obj.applyProperty = ApplyProperty.Def;
                    break;
                case "HP":
                    obj.applyProperty = ApplyProperty.HP;
                    break;
                case "MP":
                    obj.applyProperty = ApplyProperty.MP;
                    break;
                case "Speed":
                    obj.applyProperty = ApplyProperty.Speed;
                    break;
                case "AttackSpeed":
                    obj.applyProperty = ApplyProperty.AttackSpeed;
                    break;

            }

            obj.applyValue = int.Parse(propArray[6]);
            obj.applyTime = int.Parse(propArray[7]);
            obj.mp = int.Parse(propArray[8]);
            obj.coldTime = int.Parse(propArray[9]);
            obj.applicableRole = propArray[10] == "Swordman" ? ApplicableRole.Swordman : ApplicableRole.Magician;

            obj.level = int.Parse(propArray[11]);
            switch (propArray[12])
            {
                case "Self":
                    obj.releaseType = ReleaseType.Self;
                    break;
                case "Enemy":
                    obj.releaseType = ReleaseType.Enemy;
                    break;
                case "Position":
                    obj.releaseType = ReleaseType.Postion;
                    break;

            }
            obj.distance = float.Parse(propArray[13]);
            obj.efx_name = propArray[14];
            obj.animation_name = propArray[15];
            obj.animation_time = float.Parse(propArray[16]);
            skillInfoDic.Add(obj.id, obj);

        }
    }
}


//适用juese
public enum ApplicableRole
{
    Swordman,
    Magician
}

public enum ApplyType
{
    Passive,
    Buff,
    SingleTarget,
    MultiTarget
}

public enum ApplyProperty
{
    Attack,
    Def,
    Speed,
    AttackSpeed,
    HP,
    MP
}

public enum ReleaseType
{
    Self,
    Enemy,
    Postion
}

public class SkillInfo
{
    public int id;
    public string skillname;
    public string icon;
    public string des;
    public ApplyType applyType;
    public ApplyProperty applyProperty;

    public int applyValue;
    public int applyTime;
    public int mp;
    public int coldTime;
    public ApplicableRole applicableRole;
    public int level;
    public ReleaseType releaseType;
    public float distance;
    public string efx_name;
    public string animation_name;
    public float animation_time = 0;

}