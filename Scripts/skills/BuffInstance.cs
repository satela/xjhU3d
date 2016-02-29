using UnityEngine;
using System.Collections;

public class BuffInstance : MonoBehaviour {

    private DBuffData data;

    private DBaseFightRole effRole;

    private GameObject effectInstance;

    private float agentradius;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (data != null && effRole != null)
        {
            Vector3 firepos = Vector3.zero;
            if (data.buffEffPos == EFirePos.Center)
            {
                firepos = effRole.rolePosition;
            }
            else if (data.buffEffPos == EFirePos.Forward)
            {
                firepos = effRole.rolePosition + effRole.roleModel.transform.forward * effRole.roleRadius;
            }
            else if (data.buffEffPos == EFirePos.Back)
            {
                firepos = effRole.rolePosition - effRole.roleModel.transform.forward * effRole.roleRadius;
            }
            else if (data.buffEffPos == EFirePos.Top)
            {
                firepos = effRole.rolePosition + effRole.roleModel.transform.up * effRole.roleHeight / 2;
            }
            else if (data.buffEffPos == EFirePos.Bottom)
            {
                firepos = effRole.rolePosition - effRole.roleModel.transform.up * effRole.roleHeight / 2 + Vector3.up * 0.2f;
            }
            else if (data.buffEffPos == EFirePos.Left)
            {
                firepos = effRole.rolePosition - effRole.roleModel.transform.right * effRole.roleRadius;
            }
            else if (data.buffEffPos == EFirePos.Right)
            {
                firepos = effRole.rolePosition + effRole.roleModel.transform.right * effRole.roleRadius;
            }

            transform.position = firepos + Vector3.up;
            transform.rotation = effRole.roleRotation;
        }
        else
        {
            destroyEff();

        }
       
	}

    public void setBuffData(DBuffData buffdata,DBaseFightRole effectrole)
    {
        data = buffdata;

        effRole = effectrole;

        string asseturl = UrlManager.GetEffectUrl(buffdata.effurl,EEffectType.Buff);
        GameObject prefab = ResourceManager.loadAsset<GameObject>(asseturl);
        if (prefab != null)
        {
            effectInstance = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;

            effectInstance.transform.parent = transform;
            effectInstance.transform.localPosition = Vector3.zero;

            if (buffdata.bufftype == EBuffType.Temperary)
                Invoke("destroyEff", 5);
            else
                Invoke("destroyEff", buffdata.duration);
        }

    }

    void destroyEff()
    {
        Destroy(gameObject);
        if (effRole != null)
            effRole.removeBuffData(data);
    }
}
