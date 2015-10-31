using UnityEngine;
using System.Collections;

public class ExpBarManager : MonoBehaviour {
    public static ExpBarManager _instance;

    public UISlider progressbar;

    public UILabel explbl;
    public void Awake()
    {
        _instance = this;
    }


    public void SetExpValue(int cur_exp,int needexp)
    {
        progressbar.value = cur_exp / (float)needexp;
        explbl.text = cur_exp + "/" + needexp;

    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

     //   SetExpValue(0, 100);
	}
}
