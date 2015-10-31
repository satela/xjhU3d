using UnityEngine;
using System.Collections;

public enum PlayerState
{
	Moving,
	Idle

}
public class PlayerMove : MonoBehaviour {

	public float speed = 4;
	private PlayerDir dir;
	private CharacterController controller;
	public PlayerState state = PlayerState.Idle;
	public bool isMoving = false;

    private PlayerAttack attack;
    private NavMeshAgent agent;

	// Use this for initialization
	void Start () {
	
		dir = this.GetComponent<PlayerDir> ();
		controller = this.GetComponent<CharacterController> ();
        attack = this.GetComponent<PlayerAttack>();
        agent = this.GetComponent<NavMeshAgent>();

	}
	
	// Update is called once per frame
	void Update () {

      /*  if (attack.state == PlayerFightState.ControlWalk)
        {
            float distance = Vector3.Distance(dir.targetPosition, transform.position);
            if (distance > 0.1f)
            {
                isMoving = true;
                controller.SimpleMove(transform.forward * speed);
                state = PlayerState.Moving;
            }
            else
            {
                isMoving = false;
                state = PlayerState.Idle;
            }
        }*/
        if (attack.state == PlayerFightState.ControlWalk)
        {
            if (agent.remainingDistance > 0)
            {
                state = PlayerState.Moving;
            }
            else
            {
                state = PlayerState.Idle;

            }
        }
                
	}

    //跟踪到目标点 半径范围内
    public void MoveToTargetRadius(Vector3 destPoint)
    {
        //transform.LookAt(destPoint);
        //controller.SimpleMove(transform.forward * speed);

        agent.SetDestination(destPoint);
              
    }
    public void stopFollowing()
    {
        //agent.Stop();
        //agent.ResetPath();
        agent.SetDestination(transform.position);
    }
}
