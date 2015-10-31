using UnityEngine;
using System.Collections;

public class WeaponpShopNpc : Npc
{

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && UICamera.hoveredObject == null)
        {
            UIManager._instance.ShowNewpanel(EWindowName.ShopWeapon);
            WeaponpShopUI._instance.TransformState();

        }
    }
}
