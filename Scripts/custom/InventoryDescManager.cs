using UnityEngine;
using System.Collections;

public class InventoryDescManager : MonoBehaviour
{

    public static InventoryDescManager _instance;
    public UILabel deslbl;
    public GameObject equipBtn;

    public GameObject unequipBtn;

    private ItemInfo info;
    private TweenPosition tween;
    void Awake()
    {
        _instance = this;
       // deslbl = this.GetComponentInChildren<UILabel>();
        UIEventListener.Get(gameObject).onClick = OnClickClose;
        UIEventListener.Get(equipBtn).onClick = OnClickEquip;
        UIEventListener.Get(unequipBtn).onClick = OnClickUnEquip;
        UIManager.openCall += OnShowPanel;

        tween = this.GetComponent<TweenPosition>();
        this.gameObject.SetActive(false);
    }
	// Use this for initialization
	void Start () {

	}

    private void OnShowPanel(EWindowName closeWindName)
    {
        if(EWindowName.ItemDesc != closeWindName)
        Hide();
    }
    void OnClickClose(GameObject go)
    {
        UIManager._instance.CloseDescWnd();
        tween.PlayReverse();
        this.gameObject.SetActive(false);

    }
    public void Hide()
    {
        this.transform.localPosition = new Vector3(850, 0, 0);
        this.gameObject.SetActive(false);
    }
	// Update is called once per frame
	void Update () {
	
       

	}

    public void ShowItemDes(ItemInfo itemInfo)
    {
        if(!gameObject.activeSelf)
        {
            this.gameObject.SetActive(true);
            UIManager._instance.ShowNewpanel(EWindowName.ItemDesc);
            tween.PlayForward();
        }
       
       // transform.position = UICamera.currentCamera.ScreenToWorldPoint(Input.mousePosition);
       // ObjectInfo info = ObjectsInfo._instance.GetInfoById(itemInfo.baseInfo.id);

        this.info = itemInfo;
        string des = "";
        if (info == null)
            return;
        switch(info.baseInfo.type)
        {
            case ObjectType.Drug:
                des = GetDrugDes(info.baseInfo);
                break;
            case ObjectType.Equip:
                des = GetEquipmentDes(info.baseInfo);
                break;

        }
        if(itemInfo.storeType == ItemStoreType.BAG)
        {
            equipBtn.SetActive(true);
            unequipBtn.SetActive(false);
        }
        else
        {
            equipBtn.SetActive(false);
            unequipBtn.SetActive(true);
        }
        deslbl.text = des;
    }

    string GetDrugDes(ObjectInfo info)
    {
        string str = "";
        str += "名称：" + info.name + "\n";
        str += "+HP：" + info.hp + "\n";
        str += "+MP：" + info.mp + "\n";
        str += "出售价：" + info.price_sell + "\n";
        str += "购买价：" + info.price_buy + "\n";

        return str;
    }

    string GetEquipmentDes(ObjectInfo info)
    {
        string str = "";
        str += "名称：" + info.name + "\n";
        switch(info.dressType)
        {
            case DressType.HeadGear:
                str += "穿戴类型：头盔\n";
                break;
            case DressType.Armor:
                str += "穿戴类型：盔甲\n";
                break;
            case DressType.LeftHand:
                str += "穿戴类型：左手\n";
                break;
            case DressType.RightHand:
                str += "穿戴类型：右手\n";
                break;
            case DressType.Shoe:
                str += "穿戴类型：鞋子\n";
                break;
            case DressType.Accessory:
                str += "穿戴类型：饰品\n";
                break;

        }
        switch(info.appType)
        {
            case ApplicationType.Swordman:
                str += "适用职业：剑士\n";
                break;
            case ApplicationType.Magician:
                str += "适用职业：法师\n";
                break;
            case ApplicationType.Common:
                str += "适用职业：通用\n";
                break;
        }
        str += "伤害值：" + info.attack + "\n";
        str += "防御值：" + info.def + "\n";
        str += "速度值：" + info.speed + "\n";

        str += "出售价：" + info.price_sell + "\n";
        str += "购买价：" + info.price_buy + "\n";

        return str;
    }
    //穿戴物品
    void OnClickEquip(GameObject go)
    {
        if (info.baseInfo.appType == ApplicationType.Common || (int)info.baseInfo.appType == (int)PlayerStatus._instance.heroType)
        {
            if (PlayerStatus._instance.useItems(info.baseInfo.id, 1))
            {
                PlayerStatus._instance.equipItem(info.baseInfo);              
                Inventory._instance.updateItemView();
                OnClickClose(null);
            }
        }
        
    }
    //卸下装备
    void OnClickUnEquip(GameObject go)
    {
        PlayerStatus._instance.addItem(info.baseInfo.id);
        PlayerStatus._instance.equipinfos.Remove(info.baseInfo.dressType);
        EquipMentManager._instance.updateEquipView();
        OnClickClose(null);
    }
}
