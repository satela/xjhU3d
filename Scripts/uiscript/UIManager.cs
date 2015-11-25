using UnityEngine;
using System.Collections;

public enum EWindowName
{
    Equipment,
    Inventory,
    Quest,
    ShopDrug,
    ShopWeapon,
    Status,
    ItemDesc,
    SkillUI
}
public class UIManager : MonoBehaviour{

    public static  UIManager _instance ;

    public delegate void OnOpenWindow(EWindowName windName);

    public  event OnOpenWindow openCall;

    public delegate void OnCloseDesc();

    public  event OnCloseDesc closeDescWindow;

   
   
    public void Awake()
    {
        _instance = this;        
    }

	// Use this for initialization


    public void ShowNewpanel(EWindowName name)
    {
        if (openCall != null)
            openCall(name);
    }

    public void CloseDescWnd()
    {
        if (closeDescWindow != null)
            closeDescWindow();
    }
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
