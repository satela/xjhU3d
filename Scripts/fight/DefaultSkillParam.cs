using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DefaultSkillParam
{


    public const float PathFindingDist = 45;//技能寻路距离，当所有敌人超过这个距离时，不作自动寻路

    public const float BeatonBackMaxDist = 2;//被击退的默认距离

    public const float BeatonUpMaxDist = 3;//被击飞的默认高度


    public static string[] beatonBackFly = { "无", "击退", "击飞" };

    public static string[] skill_type = { "攻击", "给敌人加buff", "给己方加buff", "都加buff" };
    public static string[] near_fartype = { "近攻", "远攻" };
    public static string[] relatePos = { "中心", "前", "后", "上", "下", "左", "右" };

    private static Dictionary<eAnimatorState, string> m_actionName;

    public static Dictionary<eAnimatorState, string> ActionName
    {
        get
        {
            if (m_actionName == null)
            {
                m_actionName = new Dictionary<eAnimatorState, string>();
                m_actionName.Add(eAnimatorState.atk0, "普通攻击1");
                m_actionName.Add(eAnimatorState.atk1, "普通攻击2");
                m_actionName.Add(eAnimatorState.atk2, "普通攻击3");
                m_actionName.Add(eAnimatorState.skl0, "技能攻击1");
                m_actionName.Add(eAnimatorState.skl1, "技能攻击2");
                m_actionName.Add(eAnimatorState.skl2, "技能攻击3");
                m_actionName.Add(eAnimatorState.skl3, "技能攻击4");
                m_actionName.Add(eAnimatorState.skl4, "技能攻击5");
                m_actionName.Add(eAnimatorState.skl5, "技能攻击6");
                m_actionName.Add(eAnimatorState.skl6, "旋风斩");


                m_actionName.Add(eAnimatorState.beaten, "普通被击");

                m_actionName.Add(eAnimatorState.fall, "摔倒");
            }
            return m_actionName;
        }
    }

}
