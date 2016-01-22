using UnityEngine;
using System.Collections;

public class HpBarControl : MonoBehaviour {

    public UIProgressBar hpbar;
    public float hpbarScale = 5f;
    public Transform hpbarpos;

    private DBaseFightRole fightrole;

    void Awake()
    {
        fightrole = gameObject.GetComponent<DBaseFightRole>();
    }
	// Use this for initialization
	void Start () {
	   
        hpbar = RoleBarUIManager._instance.createHp();

        GameObject hpBarpt = new GameObject(fightrole.name + "hpBar");

        hpBarpt.transform.parent = fightrole.roleModel.transform;

        hpBarpt.transform.localPosition = new Vector3(0, fightrole.roleHeight, 0);

        hpbarpos = hpBarpt.transform;

        hpbar.transform.localScale = Vector3.one * 1.5f;
	}

    public void updateHp()
    {
        float rate = fightrole.roledata.getBaseAttrByType(EBaseAttr.Hp) / fightrole.roledata.getOriginBaseAttrByType(EBaseAttr.Hp);

        hpbar.value = rate;

    }
	// Update is called once per frame
	void Update () {

        updateHpBarPos();
	}

    void updateHpBarPos()
    {

        float newFomat = hpbarScale / Vector3.Distance(hpbarpos.position, Camera.main.transform.position);
        hpbar.transform.position = hpbarpos.position;// WorldToUI(hpbarpos.position);
        //计算出血条的缩放比例   
       // hpbar.transform.localScale = Vector3.one * newFomat;
        hpbar.transform.rotation = Camera.main.transform.rotation;
       // hpbar.value = (float)hp / max_hp;

    }
    public static Vector3 WorldToUI(Vector3 point)
    {
        Vector3 pt = Camera.main.WorldToScreenPoint(point);
        //我发现有时候UICamera.currentCamera 有时候currentCamera会取错，取的时候注意一下啊。  
        Vector3 ff = UICamera.currentCamera.ScreenToWorldPoint(pt);
        //UI的话Z轴 等于0   
        ff.z = 0;
        return ff;
    }

    public void OnDestroy()
    {
        if(hpbar != null)
            Destroy(hpbar.gameObject);
    }  

    
}
