using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillCasterData  {

    public GameObject castRole;//技能释放者

    public List<GameObject> beatonRoles;//被攻击列表

    public DSkillBaseData skilldata;


    public SkillCasterData clone()
    {
        SkillCasterData skillcastdata = new SkillCasterData();
        skillcastdata.castRole = castRole;
        skillcastdata.beatonRoles = beatonRoles;

        skillcastdata.skilldata = skilldata;

        return skillcastdata;
    }
}
