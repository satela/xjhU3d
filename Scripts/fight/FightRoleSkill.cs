using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class FightRoleSkill:MonoBehaviour  {

    private List<DSkillBaseData> skillList = new List<DSkillBaseData>();

    private DSkillBaseData lastUseSkill;

    private Dictionary<int, float> skillCdTime = new Dictionary<int, float>();

    private float normalCdTime = 0;//每次施放一次技能，至少冷却时间

    private float timespan = 0;

    public MainSkillType useSkillPriority = MainSkillType.MainSkillType_None;

   // private MainSkillType[] controlPriority = []
    void Start()
    {
       // InvokeRepeating("updatecdTime", 1, -1);
    }

    void Update()
    {
        timespan += Time.deltaTime;
        if (timespan >= 1)
        {
            updatecdTime();
            timespan = 0;
        }
    }

    
    public void initSkill(List<int> skills)
    {
        for (int i = 0; i < skills.Count; i++)
        {
            if (SkillConfiguration.skillsDic.ContainsKey(skills[i]))
            {
                skillList.Add(SkillConfiguration.skillsDic[skills[i]]);
                skillCdTime.Add(skills[i], 0);
            }
        }
    }
    void updatecdTime()
    {
        if (skillCdTime == null)
            return;
        Dictionary<int, float> tempcd = new Dictionary<int, float>();
        foreach(int keys in skillCdTime.Keys)
        {
            if (skillCdTime[keys] > 0)
            {
                tempcd.Add(keys,skillCdTime[keys] - 1);
            }
        }
        foreach (int keys in tempcd.Keys)
        {
            skillCdTime[keys] = tempcd[keys];
        }
        if(normalCdTime > 0)
            normalCdTime -= 1;
    }
    public bool useSkill(DSkillBaseData useskilldata)
    {
        foreach(DSkillBaseData skilldata in skillList)
        {
            if(useskilldata.id == skilldata.id)
            {
                //int index = skillList.IndexOf(skilldata);
                if (skillCdTime[skilldata.id] <= 0)
                {
                    if (ConfigManager.intance.skillDefaultDic.ContainsKey(useskilldata.id))
                        skillCdTime[skilldata.id] = ConfigManager.intance.skillDefaultDic[useskilldata.id].cdTime;
                    lastUseSkill = useskilldata;
                    normalCdTime = Random.Range(2,DefaultSkillParam.skillCdTime);
                    return true;
                }
            }
        }

        return false;
    }

    public float getCDTime(DSkillBaseData skilldata)
    {
        if (skillCdTime.ContainsKey(skilldata.id))
            return skillCdTime[skilldata.id];
        else
            return 0;
    }
    public float getCDTime(int skillid)
    {
        if (skillCdTime.ContainsKey(skillid))
            return skillCdTime[skillid];
        else
            return 0;
    }

    public DSkillBaseData getCanUseSkill()
    {
        if (normalCdTime > 0)
            return null;

        List<DSkillBaseData> canuseSkills = new List<DSkillBaseData>();
        foreach (DSkillBaseData skilldata in skillList)
        {
            
           if (skillCdTime[skilldata.id] <= 0)
           {
               canuseSkills.Add(skilldata);
           }
           /*else if (skillCdTime[skilldata.id] <= 0 && tempdata != null)
           {
               DSkillDefaultData skilln = ConfigManager.intance.skillDefaultDic[skilldata.id];
               DSkillDefaultData skilllast = ConfigManager.intance.skillDefaultDic[tempdata.id];
               if (skilln.needMp > skilllast.needMp)
                   tempdata = skilldata;
           }*/
        }
        if (canuseSkills.Count > 0)
            return getBestSkillByType(canuseSkills, useSkillPriority);
        return null;
    }

    private DSkillBaseData getBestSkillByType(List<DSkillBaseData> allSkills,MainSkillType type)
    {
        DSkillBaseData tempdata = null;
        if (type == MainSkillType.MainSkillType_None)
        {
            tempdata = allSkills[0];
            for (int i = 1; i < allSkills.Count; i++)
            {
                DSkillDefaultData skilln = ConfigManager.intance.skillDefaultDic[allSkills[i].id];
                DSkillDefaultData skilllast = ConfigManager.intance.skillDefaultDic[tempdata.id];
                if (skilln.needMp > skilllast.needMp)
                    tempdata = allSkills[i];
            }
            return tempdata;
        }
        else if (type == MainSkillType.MainSkillType_Attack)
        {
           // 寻找类型为攻击的技能，并且是攻击加成最多的，如果没有则返回需要怒气最高的技能
            int attackplus = 0;
            for (int i = 0; i < allSkills.Count; i++)
            {
                DSkillDefaultData skilln = ConfigManager.intance.skillDefaultDic[allSkills[i].id];
                DSkillDefaultData skilllast;
                if(tempdata != null)
                     skilllast = ConfigManager.intance.skillDefaultDic[tempdata.id];
                if (tempdata == null && skilln.mianskillType == type)
                {
                    tempdata = allSkills[i];
                    foreach(int plus in skilln.attack_plus.Values)
                        attackplus += plus;
                }
                else if (tempdata != null && skilln.mianskillType == type)
                {
                    int templus = 0;
                    foreach (int plus in skilln.attack_plus.Values)
                        templus += plus;
                    if (templus > attackplus)
                    {
                        tempdata = allSkills[i];
                        attackplus = templus;

                    }
                }
            }
            if (tempdata != null)
                return tempdata;
            else
                return getBestSkillByType(allSkills, MainSkillType.MainSkillType_None);
        }

        else if (type == MainSkillType.MainSkillType_Control || type == MainSkillType.MainSkillType_Recovery)
        {
            for (int i = 0; i < allSkills.Count; i++)
            {
                DSkillDefaultData skilln = ConfigManager.intance.skillDefaultDic[allSkills[i].id];
                if (skilln.mianskillType == type)
                {
                    return allSkills[i];
                }
            }

            return getBestSkillByType(allSkills, MainSkillType.MainSkillType_None);
        }

        return getBestSkillByType(allSkills, MainSkillType.MainSkillType_None);
    }
}
