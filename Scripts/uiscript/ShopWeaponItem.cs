using UnityEngine;
using System.Collections;

public class ShopWeaponItem : MonoBehaviour {

    public UISprite icon;
    public UILabel itemname;
    public UILabel des;
    public UILabel sellprice;

    private ObjectInfo weaponInfo;
    public GameObject buytbtn;

    void Start()
    {
        UIEventListener.Get(buytbtn).onClick += OnBuyWeapon;
    }

    private void OnBuyWeapon(GameObject go)
    {
        WeaponpShopUI._instance.buyWeapon(weaponInfo);
    }
    public void setInfo(ObjectInfo info)
    {
        weaponInfo = info;
        icon.spriteName = weaponInfo.icon;
        itemname.text = weaponInfo.name;

        if(weaponInfo.attack > 0)
        {
            des.text = "+伤害：" + weaponInfo.attack;
        }
        else if (weaponInfo.def > 0)
        {
            des.text = "+防御：" + weaponInfo.def;
        }
        else if (weaponInfo.speed > 0)
        {
            des.text = "+速度：" + weaponInfo.speed;
        }
        sellprice.text = weaponInfo.price_sell.ToString();


    }
}
