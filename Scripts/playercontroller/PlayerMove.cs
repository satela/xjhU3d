using UnityEngine;
using System.Collections;

public enum PlayerState
{
	Moving,
	Idle

}
public class PlayerMove : MonoBehaviour {

	public float speed = 0.04f;
    private float startSpeed = 0;
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

    void OnEnable()
    {

        EasyJoystick.On_JoystickMove += OnJoystickMove;

        EasyJoystick.On_JoystickMoveEnd += OnJoystickMoveEnd;


    }  

    void OnJoystickMoveEnd(MovingJoystick move)  

    {  

        //停止时，角色恢复idle  

        if (move.joystickName == "MoveJoyStick")  

        {

            state = PlayerState.Idle;

        }  

    }  

  

  

    //移动摇杆中  

    void OnJoystickMove(MovingJoystick move)  

    {  

        if (move.joystickName != "MoveJoyStick")  

        {  

            return;  

        }  

          

        //获取摇杆中心偏移的坐标  

        float joyPositionX = move.joystickAxis.x;  

        float joyPositionY = move.joystickAxis.y;  

  

  

        if (joyPositionY != 0 || joyPositionX != 0)  

        {  

            //设置角色的朝向（朝向当前坐标+摇杆偏移量）  

            Vector3 mvoedist = new Vector3(joyPositionX, 0, joyPositionY).normalized;

            Vector3 forward = Camera.main.transform.TransformDirection(mvoedist);
          // mvoedist.R
           // Quaternion.
            agent.SetDestination(transform.position + forward * 2);

            Debug.Log("move x:" + joyPositionX + "," + joyPositionY);
           //state = PlayerState.Moving;

          
        }  

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

            if(Input.GetKey(KeyCode.A))
            {
                Vector3 cameraleft = new Vector3(-Camera.main.transform.forward.z, 0, Camera.main.transform.forward.x);
                Quaternion rotation = Quaternion.LookRotation(cameraleft);

                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5 * Time.deltaTime);

                startSpeed = Mathf.Lerp(startSpeed, speed, Time.deltaTime);
                agent.SetDestination(transform.position + transform.forward * startSpeed);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                Vector3 cameraright = new Vector3(Camera.main.transform.forward.z, 0, -Camera.main.transform.forward.x);


                Quaternion rotation = Quaternion.LookRotation(cameraright);

                transform.rotation = Quaternion.Slerp(transform.rotation, rotation,5 * Time.deltaTime);
                startSpeed = Mathf.Lerp(startSpeed, speed, Time.deltaTime);
                agent.SetDestination(transform.position + transform.forward * startSpeed);
            }

            if (Input.GetKey(KeyCode.W))
            {
                Vector3 cameraforwar = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);


                Quaternion rotation = Quaternion.LookRotation(cameraforwar);

                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5 * Time.deltaTime);
                startSpeed = Mathf.Lerp(startSpeed, speed, Time.deltaTime);
                agent.SetDestination(transform.position + transform.forward * startSpeed);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                Vector3 cameraback = new Vector3(-Camera.main.transform.forward.x, 0, -Camera.main.transform.forward.z);


                Quaternion rotation = Quaternion.LookRotation(cameraback);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5 * Time.deltaTime);

                startSpeed = Mathf.Lerp(startSpeed, speed, Time.deltaTime);
                agent.SetDestination(transform.position + transform.forward * startSpeed);
            }
            
            if (agent.remainingDistance > 0)
            {
                state = PlayerState.Moving;
            }
            else
            {
                state = PlayerState.Idle;
                startSpeed = 0;

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
