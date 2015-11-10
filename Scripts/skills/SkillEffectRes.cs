using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillEffectRes : MonoBehaviour {

    public GameObject[] skilleffects;

    public Dictionary<string,GameObject> skillDic = new Dictionary<string,GameObject>();

    public static SkillEffectRes _instance;

    public void Awake()
    {
        _instance = this;
    }


	// Use this for initialization
	void Start () {
	
        foreach(GameObject go in skilleffects)
            skillDic.Add(go.name,go);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
