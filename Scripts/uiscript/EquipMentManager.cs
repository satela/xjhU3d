using UnityEngine;
using System.Collections;

public class EquipMentManager : MonoBehaviour {

    public static EquipMentManager _instance;

    public GameObject closebtn;
    private bool isShow = false;
    private TweenPosition tween;

   /* public GameObject headgear;
    public GameObject armor;
    public GameObject rightHand;
    public GameObject leftHand;
    public GameObject shoe;
    public GameObject accessory;*/

    public GameObject[] allEquipSlots = new GameObject[6];
    public GameObject EquipItemPrefab;
    public void Awake()
    {
        _instance = this;

      //  allEquipSlots = {}
    }



	// Use this for initialization
	void Start () {

        tween = this.GetComponent<TweenPosition>();

        UIEventListener.Get(closebtn).onClick += CloseWindow;
        UIManager._instance.openCall += OnShowPanel;
        UIManager._instance.closeDescWindow += OnCloseDesc;

        gameObject.SetActive(false);

	}

    private void OnCloseDesc()
    {
        if (isShow)
            this.transform.localPosition = new Vector3(0, this.transform.localPosition.y, this.transform.localPosition.z);

    }
    void CloseWindow(GameObject go)
    {
        Hide();
    }
    private void OnShowPanel(EWindowName closeWindName)
    {
        if (closeWindName == EWindowName.Equipment)
            return;

        if (closeWindName != EWindowName.ItemDesc)
            Hide();
        else if(isShow)
        {
            // Bounds boud = this.renderer.bounds;
            this.transform.localPosition = new Vector3(-200, this.transform.localPosition.y, this.transform.localPosition.z);
        }
    }
	
	
	// Update is called once per frame
	void Update () {
	
	}

    public void TransformState()
    {
        if (isShow == false)
            Show();
        else
            Hide();
    }
    void Show()
    {
        isShow = true;
        gameObject.SetActive(true);
        tween.PlayForward();

        updateEquipView();
    }
    void Hide()
    {
        isShow = false;
        tween.PlayReverse();
        InventoryDescManager._instance.Hide();
    }

    private void OnTweenFinished()
    {
        if (isShow == false)
            gameObject.SetActive(false);

    }

    public void updateEquipView()
    {
        for (int i = 0; i < allEquipSlots.Length;i++ )
        {
            EquipItem equip = allEquipSlots[i].GetComponentInChildren<EquipItem>();
            if (equip != null)
                equip.clear();
        }
            foreach (ItemInfo equipinfo in PlayerStatus._instance.equipinfos.Values)
                dressEquip(equipinfo);

    }
    //穿戴一个装备
    private bool dressEquip(ItemInfo info)
    {
        bool isSucess = false;

      

        if (info.baseInfo.type != ObjectType.Equip)
            return false;
        if (info.baseInfo.appType != ApplicationType.Common)
        {
            if ((int)info.baseInfo.appType != (int)PlayerStatus._instance.heroType)
                return false;
        }
        GameObject parent = allEquipSlots[(int)info.baseInfo.dressType];

        EquipItem equipitem = parent.GetComponentInChildren<EquipItem>();
        if(equipitem != null)
        {
            equipitem.SetInfo(info);
        }
        else
        {
            GameObject itemGo = NGUITools.AddChild(parent, EquipItemPrefab);
            itemGo.transform.localPosition = Vector3.zero;
            itemGo.GetComponent<EquipItem>().SetInfo(info);

        }


        return isSucess;

    }
}
