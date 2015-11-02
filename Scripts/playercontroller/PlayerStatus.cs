using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum ERoleAttType
{
    ATTACK = 0,
    DEFENSE,
    SPEED
}

public enum EHeroType
{
    Swordman,
    Magician,
    Common

}
public class PlayerStatus : MonoBehaviour {

	public int grade = 1;//角色等级
    public int cur_exp = 0;//当前经验

	public int hp = 100;
	public int mp = 100;

    public float hp_remain = 100;
    public float mp_remain = 100;

	public int coin = 100;

    public int attack = 20;
    public int attack_plus = 0;
    public int def = 20;
    public int def_plus = 0;
    public int speed = 20;
    public int speed_plus = 0;
    public int point_remain = 0;//剩余的点数

    //主角闪避概率
    public float dodgeRate = 0.25f;
	// Use this for initialization

    public EHeroType heroType;

    public Dictionary<int, ItemInfo> bagIteminfo = new Dictionary<int, ItemInfo>();
    public Dictionary<DressType, ItemInfo> equipinfos = new Dictionary<DressType, ItemInfo>();

    public const int BAG_CAPACITY = 20;

    public string nickname = "风流少侠";
    public static PlayerStatus _instance;

    public void Awake()
    {
        _instance = this;

    }


	void Start () {
	
        getExp(0);
	}
	


    public void GetCoin(int count)
    {
        coin += count;
    }

    public bool getGetPoint(int points=1)
    {
        if (point_remain >= points)
        {
            point_remain -= points;
            return true;
        }
        return false;

    }

    //取款函数
    public bool getCoin(int coints = 1)
    {
        if (coin >= coints)
        {
            coin -= coints;
            return true;
        }
        return false;

    }
    public void DistributeRemainPoint(ERoleAttType type)
    {
        if(getGetPoint())
        {
            switch(type)
            {
                case ERoleAttType.ATTACK:
                    attack_plus += 1;
                    break;
                case ERoleAttType.DEFENSE:
                    def_plus += 1;
                    break;
                case ERoleAttType.SPEED:
                    speed_plus += 1;
                    break;

            }
        }

    }

    public bool addItem(int id,int num = 1)
    {
        foreach(ItemInfo info in bagIteminfo.Values)
        {
            if(info.baseInfo.id == id)
            {
                info.iteNum += num;
                return true;
            }
        }
        ItemInfo tempinfo;
        for (int i = 0; i < BAG_CAPACITY;i++)
        {
             bagIteminfo.TryGetValue(i,out tempinfo);
            if(tempinfo == null)
            {
                tempinfo = new ItemInfo();
                tempinfo.storeType = ItemStoreType.BAG;
                tempinfo.iteNum = num;
                tempinfo.InventoryIndex = i;           
                tempinfo.baseInfo = ObjectsInfo._instance.GetInfoById(id);
                bagIteminfo.Add(i, tempinfo);
                return true;
            }
        }
            return false;
    }

    public bool useItems(int id,int num = 1)
    {
        foreach (ItemInfo info in bagIteminfo.Values)
        {
            if (info.baseInfo.id == id)
            {
                if (info.iteNum >= num)
                {
                    info.iteNum -= num;
                    if (info.iteNum == 0)
                    {
                        bagIteminfo.Remove(info.InventoryIndex);
                    }
                    return true;
                }
                else
                    return false;
            }
        }

        return false;
    }
    public void switchTwoitems(ItemInfo infoFrom,ItemInfo infoTo)
    {
        bagIteminfo.Remove(infoFrom.InventoryIndex);

        bagIteminfo.Remove(infoTo.InventoryIndex);

       
        bagIteminfo.Add(infoFrom.InventoryIndex, infoTo);

        bagIteminfo.Add(infoTo.InventoryIndex, infoFrom);
        int temp = infoTo.InventoryIndex;
        infoTo.InventoryIndex = infoFrom.InventoryIndex;
        infoFrom.InventoryIndex = temp;

    }

    public void equipItem(ObjectInfo info)
    {
        ItemInfo equip = new ItemInfo();
        equip.baseInfo = info;
        equip.storeType = ItemStoreType.EQUIP;
        if (equipinfos.ContainsKey(info.dressType))
        {
            ItemInfo tempinfo;
            equipinfos.TryGetValue(info.dressType,out tempinfo);
            addItem(tempinfo.baseInfo.id);
            equipinfos.Remove(info.dressType);
        }
        equipinfos.Add(equip.baseInfo.dressType, equip);

    }

    public void useDrugEff(int hp,int mp)
    {
        hp_remain += hp;
        mp_remain += mp;
        if (hp_remain > this.hp)
            hp_remain = this.hp;
        if (mp_remain > this.mp)
            mp_remain = this.mp;


    }

    public void getExp(int value)
    {
        this.cur_exp += value;
        int total_exp = 100 + grade * 30;

        while (this.cur_exp >= total_exp)      
        {
            this.grade += 1;
            this.cur_exp -= total_exp;
            total_exp = 100 + grade * 30;
        }
        ExpBarManager._instance.SetExpValue(this.cur_exp, total_exp);
    }

    //主角最终攻击力
    public int finalAttack
    {
        get
        {
            int attcksum = attack + attack_plus;
            foreach(ItemInfo equip in equipinfos.Values)
            {
                if(equip != null )
                {
                    attcksum += equip.baseInfo.attack;
                }
            }
            return attcksum;
        }

    }

    //主角最终防御力
    public int finalDef
    {
        get
        {
            int defsum = def + def_plus;
            foreach (ItemInfo equip in equipinfos.Values)
            {
                if (equip != null)
                {
                    defsum += equip.baseInfo.def;
                }
            }
            return defsum;
        }

    }
}

public enum ItemStoreType
{
    BAG,
    EQUIP,
    MAX
}
public class ItemInfo
{
    public ObjectInfo baseInfo = null;
    public int iteNum = 0;
    public int InventoryIndex = 0;

    public ItemStoreType storeType;

}

