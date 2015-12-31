using UnityEngine;
using System.Collections;

public class CastSkill : MonoBehaviour {

    public GameObject effectprefab;

    public DAnimatorController anicontroller;

    public GameObject beatenEffectPfb;//受击特效

    public GameObject hitEffectPfb;//攻击特效

    public string hitprefaburl;
    public DAnimatorController enemy;
	// Use this for initialization
	void Start () {
	
	}

    void OnGUI()
    {

        if (GUILayout.Button("普通攻击"))
        {
            //if (anicontroller.doNormalAttack())
            {
               // GameObject eff = GameObject.Instantiate(startEffectPfb, anicontroller.gameObject.transform.position + Vector3.up * 0.5f, anicontroller.gameObject.transform.rotation) as GameObject;
               // eff.transform.localScale = anicontroller.gameObject.transform.localScale;

                 Object gos =  Resources.LoadAssetAtPath("Assets" + "/RPG/Effect/effects/airen/airen_atk1.prefab",typeof(GameObject));

                 GameObject hiteff = GameObject.Instantiate(gos, anicontroller.gameObject.transform.position + Vector3.up * 0.5f, anicontroller.gameObject.transform.rotation) as GameObject;

                hiteff.transform.localScale = anicontroller.gameObject.transform.localScale;

                //StartCoroutine(behitted());
            }
        }
    }

    IEnumerator behitted()
    {
        yield return new WaitForSeconds(0.5f);
        if (enemy != null)
        {
            enemy.doBeHitted();
            for (int i = 0; i < 3; i++)
            {
                GameObject eff = GameObject.Instantiate(beatenEffectPfb, enemy.gameObject.transform.position + Vector3.up * 0.5f, enemy.gameObject.transform.rotation) as GameObject;
                eff.transform.localScale = enemy.gameObject.transform.localScale;
            }
            
        }

    }
	// Update is called once per frame
	void Update () {
	
       if(Input.GetMouseButtonDown(0))
        {
           // OnLockMultiTarget();
        }
        
	}

    void OnLockMultiTarget()
    {
       // CursorManager.instance.SetNormal();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitinfo;
        bool isCollider = Physics.Raycast(ray, out hitinfo);
        if (isCollider && hitinfo.collider.tag == Tags.ground)
        {
            // Quaternion rotation = Quaternion.LookRotation(transform.position - hitinfo.point);
            //  transform.rotation = rotation;
            //transform.LookAt(hitinfo.point);
            //animation.CrossFade(cur_skill.animation_name);
           // yield return new WaitForSeconds(cur_skill.animation_time);
            //state = PlayerFightState.ControlWalk;

           // GameObject effectprefab = null;

            GameObject players = GameObject.FindGameObjectWithTag(Tags.player);
            Vector3 forcedr = (players.transform.position + Vector3.up*3f) - hitinfo.point;

           // players.GetComponent<PlayerDir>().hitover(forcedr.normalized);
             //   GameObject skill_eff = GameObject.Instantiate(effectprefab, hitinfo.point + Vector3.up * 2f, Quaternion.identity) as GameObject;
             //   testskill skillmanager = skill_eff.AddComponent<testskill>();
               // skillmanager.attack = PlayerStatus._instance.finalAttack;
             //   skillmanager.eff_radius = 8;
            
        }
        
    }

    

}
