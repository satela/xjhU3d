using UnityEngine;
using System.Collections;

public class HeadStatusUI : MonoBehaviour {


    public UILabel playername;
    public UISlider hpBar;
    public UISlider mpBar;

    public UILabel hplbl;
    public UILabel mplbl;

    public static HeadStatusUI _instance;

    public void Awake()
    {
        _instance = this;
    }

    

    public void UpdateShowUI()
    {
        if (PlayerStatus._instance != null)
        {
            playername.text = "Lv." + PlayerStatus._instance.grade + PlayerStatus._instance.nickname;
            hpBar.value = PlayerStatus._instance.hp_remain / PlayerStatus._instance.hp;
            mpBar.value = PlayerStatus._instance.mp_remain / PlayerStatus._instance.mp;

            hplbl.text = PlayerStatus._instance.hp_remain + "/" + PlayerStatus._instance.hp;
            mplbl.text = PlayerStatus._instance.mp_remain + "/" + PlayerStatus._instance.mp;
        }       

    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        UpdateShowUI();
	}
}
