using UnityEngine;
using System.Collections;

public class EquipItem : MonoBehaviour {

    public UISprite icon;

    public ItemInfo equipinfo;

    public void Start()
    {
        UIEventListener.Get(gameObject).onClick += OnShowDesc;
    }

    
    private void OnShowDesc(GameObject go)
    {
        if(equipinfo != null)
            InventoryDescManager._instance.ShowItemDes(equipinfo);
    }

    public void clear()
    {
        equipinfo = null;
        icon.spriteName = null;
    }
    public void SetInfo(ItemInfo info)
    {
        equipinfo = info;

        icon.spriteName = info.baseInfo.icon;
    }
}
