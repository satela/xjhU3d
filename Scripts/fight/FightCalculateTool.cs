using UnityEngine;
using System.Collections;

/* 战斗计算类，主要用于计算战斗中是否被击中，伤害值*/
public class FightCalculateTool {

    //true 被击中 false 则闪避
	public static bool checkIsHitted(DRoleData fightrole,DRoleData beatonrole)
    {
        float hitrate = fightrole.getSubAttrByType(ESubAttr.Hit) / (fightrole.getSubAttrByType(ESubAttr.Hit) + beatonrole.getSubAttrByType(ESubAttr.Dodge));

        float rand = Random.Range(0, 1f);

        if (rand <= hitrate)
            return true;
        else
            return false;
    }

    // phyplus 物理攻击加成，magplus 法术攻击加成
    public static int calculateHarm(DRoleData fightrole, DRoleData beatonrole,int phyplus = 0,int magplus = 0)
    {
        float phyHarm = phyplus + fightrole.getBaseAttrByType(EBaseAttr.Phy_Attack) - beatonrole.getBaseAttrByType(EBaseAttr.PhyDef);
        float magHarm = magplus + fightrole.getBaseAttrByType(EBaseAttr.Mag_Attack) - beatonrole.getBaseAttrByType(EBaseAttr.MagDef);

        float total = phyHarm + magHarm;
        if (total <= 0)
            total = 1;
        return (int)(total*0.5f);      

    }
}
