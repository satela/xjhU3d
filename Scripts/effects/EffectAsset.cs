using UnityEngine;
using System.Collections;

public enum EEffectType
{

    Normal = 0,//一般特性
    Attack,//攻击特效
    Beaton,//受击特效
    Move,//移动特效
    Explode,// 指定地点爆炸特效

    Dodge,//闪避特性
    Crit,//暴击特性

    Buff   //buff 特效
}


public class EffectAsset : MonoBehaviour {

	// Use this for initialization

    public GameObject effectInstance;
	void Start () {
	
        
	}

    public void setEffectParam(string prefaburl, Vector3 position, Quaternion rotations, EEffectType type = EEffectType.Normal)
    {
        string asseturl = UrlManager.GetEffectUrl(prefaburl,type);
        GameObject prefab = ResourceManager.loadAsset<GameObject>(asseturl);
        if (prefab != null)
        {
            effectInstance = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;

            //effect.transform.localScale = new Vector3(8, 8, 8);
            effectInstance.transform.rotation = rotations;
            transform.position = position;

            effectInstance.transform.parent = gameObject.transform;
            effectInstance.transform.localPosition = Vector3.zero;
            
        }
        if (type != EEffectType.Move)
            Invoke("destroyDelay", 5);
    }

    void destroyDelay()
    {
        Destroy(gameObject);
        Resources.UnloadUnusedAssets();
    }
	// Update is called once per frame
	void Update () {
	
	}
}
