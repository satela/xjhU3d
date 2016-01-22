using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillButtonManager : MonoBehaviour {

    public GameObject[] m_allSkillsBtn;

    public UILabel[] m_allSkillsTxt;


    private List<DSkillBaseData> m_skilldataList = new List<DSkillBaseData>();

	// Use this for initialization
	void Start () {

        for (int i = 0; i < m_allSkillsBtn.Length; i++)
        {
            UIEventListener.Get(m_allSkillsBtn[i]).onClick += fireSkill;
        }
        setSkills(new int[]{10012,10020,10021,10022});
	}

    void fireSkill(GameObject go)
    {
        for (int i = 0; i < m_allSkillsBtn.Length; i++)
        {
            if (go == m_allSkillsBtn[i])
            {
                FightRoleManager._instance.getTestAttacker().useSkill(m_skilldataList[i]);
                return;
            }
        }
    }
	// Update is called once per frame
	void setSkills (List<DSkillBaseData> skills) {
	

	}

    void setSkills(int[] skillid)
    {
        m_skilldataList.Clear();
        DSkillBaseData dataskill;
        for(int i=0;i < skillid.Length;i++)
        {
            if (SkillConfiguration.skillsDic.TryGetValue(skillid[i], out dataskill))
            {
                m_skilldataList.Add(dataskill);
                if(m_allSkillsTxt[i] != null)
                    m_allSkillsTxt[i].text = dataskill.skillName;
           }

        }

    }
}
