using UnityEngine;
using System.Collections;

public class RoleBarUIManager : MonoBehaviour {

    public GameObject hpBarPrefab;

    public static RoleBarUIManager _instance;

    public void Awake()
    {
        _instance = this;
    }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}  

    public UIProgressBar createHp()
    {
        GameObject go = Instantiate(hpBarPrefab) as GameObject;
        go.transform.parent = gameObject.transform;
        go.transform.position = Vector3.zero;
        return go.GetComponent<UIProgressBar>();
    }

}
