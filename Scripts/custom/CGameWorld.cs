using UnityEngine;
using System.Collections;

public class CGameWorld : MonoBehaviour {

    private static CGameWorld _instance;

    public bool isLoadAssetBundle = false;
    public static CGameWorld instance
    {
        get { return _instance; }
    }
    public void Awake()
    {
        _instance = this;
        Application.targetFrameRate = 50;
    }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
