using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectsInfo : MonoBehaviour {

    public static ObjectsInfo _instance;

    public TextAsset objectsInfoText;
    private Dictionary<int, ObjectInfo> objectDic = new Dictionary<int, ObjectInfo>();
	// Use this for initialization
	void Awake () {

        _instance = this;
        ReadInfo();
        print(objectDic.Keys.Count);
	}

    public ObjectInfo GetInfoById(int id)
    {
        ObjectInfo info = null;
        objectDic.TryGetValue(id, out info);

        return info;
    }
	void ReadInfo()
    {
        string text = objectsInfoText.text;
        string[] strArray = text.Split('\n');

        ObjectInfo obj;
        foreach(string str in strArray)
        {
            obj = new ObjectInfo();
            string[] propArray = str.Split(',');
            obj.id = int.Parse(propArray[0]);
            obj.name = propArray[1];
            obj.icon = propArray[2];
            string str_type = propArray[3];
            //ObjectType type = ObjectType.Drug;
            switch(str_type)
            {
                case "Drug":
                    obj.type = ObjectType.Drug;
                    obj.hp = int.Parse(propArray[4]);
                    obj.mp = int.Parse(propArray[5]);
                    obj.price_sell = int.Parse(propArray[6]);
                    obj.price_buy = int.Parse(propArray[7]);
                    break;
                case "Equip":
                    obj.type = ObjectType.Equip;
                    obj.attack = int.Parse(propArray[4]);
                    obj.def = int.Parse(propArray[5]);
                    obj.speed = int.Parse(propArray[6]);

                    obj.price_sell = int.Parse(propArray[9]);
                    obj.price_buy = int.Parse(propArray[10]);

                    string str_dresstype = propArray[7];
                    switch(str_dresstype)
                    {
                        case "Headgear":
                            obj.dressType = DressType.HeadGear;
                            break;
                        case "Armor":
                            obj.dressType = DressType.Armor;
                            break;
                        case "LeftHand":
                            obj.dressType = DressType.LeftHand;;
                            break;
                        case "RightHand":
                            obj.dressType = DressType.RightHand; ;
                            break;
                        case "Accessory":
                            obj.dressType = DressType.Accessory; ;
                            break;
                        case "Shoe":
                            obj.dressType = DressType.Shoe; ;
                            break;
                    }
                    string str_apptype = propArray[8];
                    switch (str_apptype)
                    {
                        case "Swordman":
                            obj.appType = ApplicationType.Swordman;
                            break;
                        case "Magician":
                            obj.appType = ApplicationType.Magician;
                            break;
                        case "Common": 
                            obj.appType = ApplicationType.Common;
                            break;
                    }

                    break;
                case "Mat":
                    obj.type = ObjectType.Mat;
                    break;
            }

            objectDic.Add(obj.id, obj);
        }
    }
	
   public List<ObjectInfo> getWeaponList()
    {
        List<ObjectInfo> list = new List<ObjectInfo>();

       foreach(ObjectInfo info in objectDic.Values)
       {
           if(info.type == ObjectType.Equip)
           {
               list.Add(info);
           }
       }
       return list;
    }
   
}
public enum ObjectType
{
    Drug,
    Equip,
    Mat
}

public enum DressType
{
    HeadGear,
    Armor,
    RightHand,
    LeftHand,
    Shoe,
    Accessory
}

public enum ApplicationType
{
    Swordman,
    Magician,
    Common
}
public class ObjectInfo
{
    public int id;
    public string name;
    public string icon;
    public ObjectType type;
    public int hp;
    public int mp;
    public int price_sell;
    public int price_buy;

    public int attack;
    public int def;
    public int speed;

    public DressType dressType;
    public ApplicationType appType;

}