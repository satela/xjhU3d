using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponpShopUI : MonoBehaviour {

    public static WeaponpShopUI _instance;
    private TweenPosition tween;
    private bool isShow = false;
    public GameObject closebtn;

    public UIGrid grid;
    public GameObject weaponItemPrefab;

    public GameObject buyPanel;
    public GameObject okbtn;
    public UIInput inputNum;

    private ObjectInfo curBuyWeaponInfo;
    public void Awake()
    {
        _instance = this;
    }


    // Use this for initialization
    void Start()
    {
        tween = this.GetComponent<TweenPosition>();
        UIEventListener.Get(closebtn).onClick += OnClickClose;
        UIEventListener.Get(okbtn).onClick += OnClickOkBuy;     

        gameObject.SetActive(false);
        UIManager.openCall += OnShowPanel;
        buyPanel.SetActive(false);
        showWeaponList();
    }

    private void OnShowPanel(EWindowName closeWindName)
    {
        if (closeWindName == EWindowName.ShopWeapon)
            return;
        Hide();
    }

    void OnClickClose(GameObject go)
    {
        Hide();
    }

    // Update is called once per frame
    private void showWeaponList()
    {
        List<ObjectInfo> list = ObjectsInfo._instance.getWeaponList();

        for(int i=0;i < list.Count;i++)
        {
            ShopWeaponItem weaponitem = NGUITools.AddChild(grid.gameObject, weaponItemPrefab).GetComponent<ShopWeaponItem>();
            weaponitem.setInfo(list[i]);

        }
        grid.Reposition();
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
        grid.Reposition();

        // UpdateView();
    }
    void Hide()
    {
        isShow = false;
        tween.PlayReverse();
    }

    private void OnTweenFinished()
    {
        if (isShow == false)
            gameObject.SetActive(false);

    }

    private void OnClickOkBuy(GameObject go)
    {
        int count = int.Parse(inputNum.value);
        if(count > 0)
        {
            int price = curBuyWeaponInfo.price_sell * count;
            if(PlayerStatus._instance.getCoin(price))
            {
                PlayerStatus._instance.addItem(curBuyWeaponInfo.id,count);
                inputNum.value = "0";
            }
            
        }
    }
    public void buyWeapon(ObjectInfo info)
    {
        buyPanel.SetActive(true);
        curBuyWeaponInfo = info;
        inputNum.value = "0";
    }

    void OnClick()
    {
        buyPanel.SetActive(false);

    }
}
