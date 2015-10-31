using UnityEngine;
using System.Collections;

public class SkillItemUI : MonoBehaviour {

    public SkillInfo skillinfo;

    public UISprite icon;
    public UILabel namelbl;
    public UILabel typelbl;
    public UILabel desclbl;
    public UILabel mplbl;

    public UISprite mask;
        // Use this for initialization
	void Start () {
	
	}
	
    public void setSkillInfo(SkillInfo info)
    {
        skillinfo = info;

        icon.spriteName = skillinfo.icon;
        namelbl.text = skillinfo.skillname;
        switch(skillinfo.applyType)
        {
            case ApplyType.Buff:
                typelbl.text = "增强";
                break;
            case ApplyType.Passive:
                typelbl.text = "增益";
                break;
            case ApplyType.SingleTarget:
                typelbl.text = "单个目标";
                break;
            case ApplyType.MultiTarget:
                typelbl.text = "多个目标";
                break;
        }

        desclbl.text = skillinfo.des;
        mplbl.text = skillinfo.mp + "MP";
    }

    public void UpdateEnabled()
    {
        mask.gameObject.SetActive(PlayerStatus._instance.grade < skillinfo.level);
        icon.GetComponent<SkillItemIcon>().enabled = PlayerStatus._instance.grade >= skillinfo.level;
    }
	
}
