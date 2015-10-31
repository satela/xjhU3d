using UnityEngine;
using System.Collections;

public class InventoryItemChild : UIDragDropItem {

    private UISprite sprite;
    private int originDepth = 0;
    private int itemId;
    void Awake()
    {
        sprite = this.GetComponent<UISprite>();
    }

    void Start()
    {
        base.Start();
        originDepth = sprite.depth;
        UIEventListener.Get(gameObject).onClick = OnHoverOverItem;
    }

    protected override void OnDragDropStart()
    {
        base.OnDragDropStart();
        sprite.depth = 100;
    }

    protected override void OnDragDropRelease(GameObject surface)
    {
        base.OnDragDropRelease(surface);
        if (surface != null)
        {
            if(surface.tag == Tags.InventoryItem)
            {
                InventoryItem citem = surface.transform.parent.GetComponent<InventoryItem>();
                if(citem.itemInfo != null)
                {
                    InventoryItem pitem = this.transform.parent.GetComponent<InventoryItem>();
                   // int itemid = pitem.itemid;
                  //  int itemnum = pitem.itemnum;
                  //  ItemInfo tempinfo = pitem.itemInfo;
                   // pitem.SetInfo(citem.itemInfo);
                    PlayerStatus._instance.switchTwoitems(pitem.itemInfo, citem.itemInfo);
                   // citem.SetId(itemid, itemnum);
                }
            }
            else if (surface.tag == Tags.InventoryItemGrid && surface != this.transform.parent.gameObject)
            {
                 InventoryItem citem = surface.GetComponent<InventoryItem>();
                 InventoryItem pitem = this.transform.parent.GetComponent<InventoryItem>();

                 PlayerStatus._instance.bagIteminfo.Remove(pitem.itemInfo.InventoryIndex);

                 pitem.itemInfo.InventoryIndex = citem.bagIndex;
                 PlayerStatus._instance.bagIteminfo.Add(citem.bagIndex, pitem.itemInfo);
                 pitem.clearInfo();
            }
            else if(surface.tag == Tags.ShortCutSkill)
            {
                InventoryItem pitem = this.transform.parent.GetComponent<InventoryItem>();
                if (pitem.itemInfo.baseInfo.type == ObjectType.Drug)
                    ShortCutSkillUI._instance.SetShortCutItem(surface, pitem.itemInfo.baseInfo);
            }
            else
            {
                this.transform.localPosition = Vector3.zero;
            }
        }
        else
        {
            this.transform.localPosition = Vector3.zero;
        }

        Inventory._instance.updateItemView();
        sprite.depth = originDepth;

        //print(surface.tag);

    }

    public void SetID(int id)
    {
        itemId = id;
        ObjectInfo iteminfo = ObjectsInfo._instance.GetInfoById(id);
        sprite.spriteName = iteminfo.icon;
    }

    public void SetIconName(string icon_name)
    {
        if (sprite.spriteName != icon_name)
        sprite.spriteName = icon_name;
    }

    public void OnHoverOverItem(GameObject go)
    {
        InventoryItem item = this.transform.parent.gameObject.GetComponent<InventoryItem>();
        InventoryDescManager._instance.ShowItemDes(item.itemInfo);

    }

    
	
}
