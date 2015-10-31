using UnityEngine;
using System.Collections;

public class ShopDrugNpc : Npc {


    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && UICamera.hoveredObject == null)
        {
            UIManager._instance.ShowNewpanel(EWindowName.ShopDrug);
            ShopDrug._instance.TransformState();

        }
    }
}
