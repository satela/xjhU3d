using UnityEngine;
using System.Collections;

public enum EBaseAttr
{
    Hp = 0,
    Phy_Attack,
    Mag_Attack,
    PhyDef,
    MagDef,
    Speed,
    Mp,
    Max
}

public enum ESubAttr
{
    Crit = 0, //暴击
    Dodge, //闪避
    Hit,  //命中
    Tough, //韧性

    Max
}
public class DRoleData  {

    public float[] roleBaseAttr = new float[(int)EBaseAttr.Max]; //人物基本主属性

    public float[] roleSubAttr = new float[(int)ESubAttr.Max];//人物基本小属性

    public float[] roleBufBasefAttr = new float[(int)EBaseAttr.Max];//人物附加主属性，战斗buff加成

    public float[] roleBufSubfAttr = new float[(int)ESubAttr.Max];//人物附加小属性，战斗buff加成

    public int cur_hp = 100;//最大血量

    public float cur_mp = 60;// 怒气

    public DRoleData()
    {
        float[] temp = { 100, 40, 20, 20, 10, 2, 60 };
        for (int i = 0; i < temp.Length; i++)
            setBaseAttrByType((EBaseAttr)i, temp[i]);

        float[] tempsub = {100,5,30,20};
        for (int i = 0; i < tempsub.Length; i++)
            setSubAttrByType((ESubAttr)i, tempsub[i]);

    }

    public float getOriginBaseAttrByType(EBaseAttr attrType)
    {
        if ((int)attrType < 0 || (int)attrType >= (int)EBaseAttr.Max)
        {
            return 0;
        }

        else
            return roleBaseAttr[(int)attrType];
    }

    public float getOriginSubAttrByType(ESubAttr attrType)
    {
        if ((int)attrType < 0 || (int)attrType >= (int)ESubAttr.Max)
        {
            return 0;
        }

        else
            return roleSubAttr[(int)attrType];
    }

    public float getBaseAttrByType(EBaseAttr attrType)
    {
        if ((int)attrType < 0 || (int)attrType >= (int)EBaseAttr.Max)
        {
            return 0;
        }

        else
        {
            if (attrType == EBaseAttr.Hp)
                return cur_hp;
            else if (attrType == EBaseAttr.Mp)
                return cur_mp;
            return roleBaseAttr[(int)attrType] + roleBufBasefAttr[(int)attrType];
        }
    }

    public float getSubAttrByType(ESubAttr attrType)
    {
        if((int)attrType < 0 ||(int)attrType >= (int)ESubAttr.Max )
        {
            return 0;
        }

        else
            return roleSubAttr[(int)attrType] + roleBufSubfAttr[(int)attrType];
    }

    public void setBaseAttrByType(EBaseAttr attrType,float value)
    {
        if ((int)attrType < 0 || (int)attrType >= (int)EBaseAttr.Max)
        {
            return;
        }

        else
            roleBaseAttr[(int)attrType] = value ;
    }

    public void setSubAttrByType(ESubAttr attrType, float value)
    {
        if ((int)attrType < 0 || (int)attrType >= (int)ESubAttr.Max)
        {
            return;
        }

        else
            roleSubAttr[(int)attrType] = value;
    }

    public void addBaseBuffAttr(EBaseAttr attrType,float value)
    {
        if ((int)attrType < 0 || (int)attrType >= (int)EBaseAttr.Max)
        {
            return;
        }

        else
        {
            if (attrType == EBaseAttr.Hp)
            {
                cur_hp += (int)value;
                if (cur_hp > roleBaseAttr[(int)EBaseAttr.Hp])
                    cur_hp = (int)roleBaseAttr[(int)EBaseAttr.Hp];
                else if (cur_hp < 0)
                    cur_hp = 0;
            }
            else if (attrType == EBaseAttr.Mp)
                cur_mp += value;
            roleBufBasefAttr[(int)attrType] += value;
        }
    }

    public void addSubBuffAttr(ESubAttr attrType, float value)
    {
        if ((int)attrType < 0 || (int)attrType >= (int)ESubAttr.Max)
        {
            return;
        }

        else
            roleBufSubfAttr[(int)attrType] += value;
    }
}
