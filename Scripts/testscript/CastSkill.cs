using UnityEngine;
using System.Collections;

public class CastSkill : MonoBehaviour {

    public GameObject effectprefab;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
       if(Input.GetMouseButtonDown(0))
        {
            OnLockMultiTarget();
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
