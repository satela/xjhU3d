using UnityEngine;
using System.Collections;

public class CastSkill : MonoBehaviour {

    public GameObject effectprefab;

    public DAnimatorController anicontroller;

    public GameObject beatenEffectPfb;//受击特效

    public GameObject hitEffectPfb;//攻击特效

    public string hitprefaburl;
    public DAnimatorController enemy;

    public NavMeshAgent cur_Role;
	// Use this for initialization
	void Start () {
	
	}

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitinfo;

           // LayerMask mask = 1 << LayerMask.NameToLayer("GroundLayer");

            bool isCollider = Physics.Raycast(ray, out hitinfo);
            if (isCollider && hitinfo.collider.tag == Tags.player)
            {
               // cur_Role.gotoPoint(hitinfo.point);
                cur_Role = hitinfo.collider.GetComponent<NavMeshAgent>();
            }
            if (isCollider && hitinfo.collider.tag == Tags.ground && cur_Role != null && cur_Role.enabled)
            {
                NavMeshPath path = new NavMeshPath();
                cur_Role.CalculatePath(hitinfo.point, path);
                if (path.corners.Length >= 2)
                {
                    cur_Role.SetDestination(hitinfo.point);
                }
                cur_Role = null;
            }
        }
    }
    void OnGUI()
    {

       
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
