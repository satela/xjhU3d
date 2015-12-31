using UnityEngine;
using System.Collections;

public class SkillCaster : MonoBehaviour {

    public SkillCasterData skilldata;


    public NavMeshAgent casterAgent;
    //private 
	// Use this for initialization
	void Start () {

        if (skilldata != null && skilldata.castRole != null)
        {
            casterAgent = skilldata.castRole.GetComponent<NavMeshAgent>();
            if (casterAgent == null)
            {
                Debug.Log("caster has no meshagent");
                Destroy(gameObject);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	

	}
}
