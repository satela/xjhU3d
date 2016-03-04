using UnityEngine;
using System.Collections;

public class FollowFighter : MonoBehaviour {

    private DBaseFightRole followTarget;

    private DBaseFightRole fightrole;

    public bool isWalkingToTarget = false;
	// Use this for initialization
	void Start () {

        fightrole = gameObject.GetComponent<DBaseFightRole>();
	}
	
	// Update is called once per frame
	void Update () {

        if (followTarget != null)
        {
            float distance = Vector3.Distance(fightrole.rolePosition, followTarget.rolePosition);
            if (!fightrole.isCasteringSkill && distance > DefaultSkillParam.maxFollowDistance && !isWalkingToTarget)
            {
                isWalkingToTarget = true;
                fightrole.gotoDestination(followTarget.rolePosition);
            }
            else if (distance < DefaultSkillParam.minFollowDistance && isWalkingToTarget)
            {
                isWalkingToTarget = false;
                fightrole.gotoDestination(fightrole.rolePosition);
            }
        }
	}

    public void setFollowTarget(DBaseFightRole target)
    {
        followTarget = target;
    }
}
