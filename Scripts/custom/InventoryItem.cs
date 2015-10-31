using UnityEngine;
using System.Collections;

public class InventoryItem : MonoBehaviour {

   // public int itemid = 0;
   // public int itemnum = 0;
    private UILabel numlbl;
  //  private ObjectInfo info = null;

    public GameObject inventoryitem;
    private GameObject itemchild;

    public ItemInfo itemInfo;
    public int bagIndex = 0;

    public void Awake()
    {
        numlbl = this.GetComponentInChildren<UILabel>();

    }

    
	// Use this for initialization
	void Start () {

       
	}
	
	// Update is called once per frame
	void Update () {
	
	}



    public void SetInfo(ItemInfo info)
    {
        clearInfo();
        itemInfo = info;
       // info = ObjectsInfo._instance.GetInfoById(id);
        itemchild = NGUITools.AddChild(gameObject, inventoryitem);
        itemchild.transform.localPosition = Vector3.zero;
        InventoryItemChild item = itemchild.GetComponent<InventoryItemChild>();
        item.SetIconName(itemInfo.baseInfo.icon);
       // itemid = id;
        numlbl.enabled = true;
      //  itemnum = num;
        numlbl.text = itemInfo.iteNum.ToString();
    }

    //清空格子存储信息
    public void clearInfo()
    {
        itemInfo = null;
        numlbl.enabled = false;
        if(itemchild != null)
        Destroy(itemchild);
        itemchild = null;
    }
    public void PlusNumber(int num=1)
    {
        itemInfo.iteNum += num;
        numlbl.text = itemInfo.iteNum.ToString();
    }

    //减去数量的，用于装备穿戴
    public bool Minus(int num = 1)
    {
        if (itemInfo.iteNum >= num)
        {
            itemInfo.iteNum -= num;
            if (itemInfo.iteNum == 0)
            {
                clearInfo();
                Destroy(itemchild);
            }
            else
            {
                numlbl.text = itemInfo.iteNum.ToString();
            }
            return true;
            
        }

        return false;
        
    }
}
