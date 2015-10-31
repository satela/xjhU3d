using UnityEngine;
using System.Collections;

public class HudTextParent : MonoBehaviour {

    public static HudTextParent _instacne;

    public GameObject hudtextPrefab;

    public void Awake()
    {
        _instacne = this;
    }

    public GameObject createHudText()
    {
       GameObject hudtextGo = NGUITools.AddChild(gameObject, hudtextPrefab);

       return hudtextGo;

    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
