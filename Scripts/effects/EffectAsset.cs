using UnityEngine;
using System.Collections;

public enum EEffectType
{

    Normal = 0,//一般特性
    Attack,//攻击特效
    Beaton,//受击特效
    Move,//移动特效
}
public class EffectAsset : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
    public void setEffectParam(string prefaburl,Vector3 position,EEffectType type = EEffectType.Normal)
    {
        

    }
	// Update is called once per frame
	void Update () {
	
	}
}
