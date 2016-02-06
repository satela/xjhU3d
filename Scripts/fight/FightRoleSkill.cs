using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FightRoleSkill:MonoBehaviour  {

    private List<DSkillBaseData> skillList = new List<DSkillBaseData>();

    private DSkillBaseData lastUseSkill;

    private Dictionary<int, float> skillCdTime = new Dictionary<int, float>();

    private float normalCdTime = 0;//每次施放一次技能，至少冷却时间

    private float timespan = 0;

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

        DSkillBaseData tempdata = null;
        foreach (DSkillBaseData skilldata in skillList)
        {
            
           if (skillCdTime[skilldata.id] <= 0 && tempdata == null)
           {
               tempdata = skilldata;
           }
           else if (skillCdTime[skilldata.id] <= 0 && tempdata != null)
           {
               DSkillDefaultData skilln = ConfigManager.intance.skillDefaultDic[skilldata.id];
               DSkillDefaultData skilllast = ConfigManager.intance.skillDefaultDic[tempdata.id];
               if (skilln.needMp > skilllast.needMp)
                   tempdata = skilldata;
           }
        }

        return tempdata;
    }
}
