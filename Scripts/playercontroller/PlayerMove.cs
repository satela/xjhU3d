using UnityEngine;
using System.Collections;

public enum PlayerState
{
	Moving,
	Idle

}
public class PlayerMove : MonoBehaviour {

	public float speed = 0.01f;
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
            float horizone = Input.GetAxis("Mouse X");
            float vertical = Input.GetAxis("Mouse Y");

            if(Input.GetKey(KeyCode.A))
            {
                Vector3 cameraleft = new Vector3(-Camera.main.transform.forward.z, 0, Camera.main.transform.forward.x);


                Quaternion rotation = Quaternion.LookRotation(cameraleft);


                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5 * Time.deltaTime);
                agent.SetDestination(transform.position + transform.forward * speed);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                Vector3 cameraright = new Vector3(Camera.main.transform.forward.z, 0, -Camera.main.transform.forward.x);


                Quaternion rotation = Quaternion.LookRotation(cameraright);

                transform.rotation = Quaternion.Slerp(transform.rotation, rotation,5 * Time.deltaTime);
                agent.SetDestination(transform.position + transform.forward * speed);

            }

            if (Input.GetKey(KeyCode.W))
            {
                Vector3 cameraforwar = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);


                Quaternion rotation = Quaternion.LookRotation(cameraforwar);

                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5 * Time.deltaTime);
                agent.SetDestination(transform.position + transform.forward * speed);

            }
            else if (Input.GetKey(KeyCode.S))
            {
                Vector3 cameraback = new Vector3(-Camera.main.transform.forward.x, 0, -Camera.main.transform.forward.z);


                Quaternion rotation = Quaternion.LookRotation(cameraback);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5 * Time.deltaTime);

                agent.SetDestination(transform.position + transform.forward * speed);

            }

            
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
