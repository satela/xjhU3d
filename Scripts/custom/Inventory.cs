using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

    public static Inventory _instance;
    private TweenPosition tween;

    public List<InventoryItem> itemlist = new List<InventoryItem>();
    private int coinCount = 1000; //金币数量

    public UILabel coinlbl;
    public GameObject inventoryitem;
    public GameObject closebtn;

    void Awake()
    {
        _instance = this;
        tween = this.GetComponent<TweenPosition>();
        gameObject.SetActive(false);
        for(int i=00;i < itemlist.Count;i++)
        {
            itemlist[i].bagIndex = i;
        }
    }
	// Use this for initialization
	void Start () {
	
         UIManager._instance.openCall += OnShowPanel;

         UIManager._instance.closeDescWindow += OnCloseDesc;

         UIEventListener.Get(closebtn).onClick += CloseWindow;

	}

    public void OnEnable()
    {
        updateItemView();
    }

    public void updateItemView()
    {
        ItemInfo info;
        for (int i = 0; i < itemlist.Count;i++ )
        {
            InventoryItem itemgrid = itemlist[i].GetComponent<InventoryItem>();
            itemgrid.clearInfo();
        }
            foreach (int key in PlayerStatus._instance.bagIteminfo.Keys)
            {
                PlayerStatus._instance.bagIteminfo.TryGetValue(key, out info);
                itemlist[key].SetInfo(info);
            }

    }

    void CloseWindow(GameObject go)
    {
        Hide();
        InventoryDescManager._instance.Hide();
    }

    private void OnShowPanel(EWindowName closeWindName)
    {
        if (closeWindName == EWindowName.Inventory)
            return;
        if (closeWindName != EWindowName.ItemDesc)
            Hide();
        else if (isShow)
        {
            this.transform.localPosition = new Vector3(-200, this.transform.localPosition.y, this.transform.localPosition.z);
        }
    }

    private void OnCloseDesc()
    {
        if(isShow)
        this.transform.localPosition = new Vector3(0, this.transform.localPosition.y, this.transform.localPosition.z);

    }
	
	
    void updateCoins()
    {
        coinlbl.text = PlayerStatus._instance.coin.ToString();
    }
	// Update is called once per frame
	void Update () {
	
        if(Input.GetKeyDown(KeyCode.X))
        {
           PlayerStatus._instance.addItem((Random.Range(2001, 2023)));
           updateItemView();
        }
	}

    public bool useItem(int id,int num=1)
    {
        InventoryItem grid = null;
        
        foreach (InventoryItem tempgrid in itemlist)
        {
            if (tempgrid.itemInfo.baseInfo.id == id)
            {
                grid = tempgrid;
                break;
            }
        }
        if (grid != null)
        {
            grid.Minus(num);
            return true;
        }

        return false;
    }

    private bool isShow = false;
     void Show()
    {
        isShow = true;
        gameObject.SetActive(true);
        tween.PlayForward();
        updateItemView();
    }
     void Hide()
    {
        isShow = false;
        tween.PlayReverse();
    }
    private void OnTweenFinished()
    {
        if(isShow == false)
            gameObject.SetActive(false);

    }
    public void TransformState()
    {
        if (isShow == false)
            Show();
        else
            Hide();
    }
}
