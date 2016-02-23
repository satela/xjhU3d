using UnityEngine;
using System.Collections;

public class PlayerDir : MonoBehaviour {

	public GameObject effect_click_prefab;
	public bool isMoving = false;//表示鼠标是否按下
	public Vector3 targetPosition = Vector3.zero;
	private PlayerMove playermvoe;

    private NavMeshAgent agent;
    private DBaseFightRole playerattack;
	// Use this for initialization
	void Start () {
	
		targetPosition = transform.position;
		playermvoe = this.GetComponent<PlayerMove> ();
        agent = this.GetComponent<NavMeshAgent>();
        playerattack = this.transform.parent.GetComponent<DBaseFightRole>();

	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0) && UICamera.hoveredObject == null && playerattack.skillstep != ESkillStep.Selecting)
		{

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitinfo;
			bool isCollider = Physics.Raycast(ray,out hitinfo,1<<7);
			if(isCollider && hitinfo.collider.tag == Tags.ground)
			{
				//isMoving = true;
				ShowClickEffect(hitinfo.point);
				//lookAtTarget(hitinfo.point);
                playerattack.gotoDestination(hitinfo.point);
			}
		}
       /* if(agent.remainingDistance > 0)
        {
            isMoving = true;
           // Debug.Log("is moveing");
        }
        else
        {
            isMoving = false;

        }*/
	/*	if (Input.GetMouseButtonUp(0)) 
		{
			isMoving = false;
		}
		if (isMoving) {
						Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
						RaycastHit hitinfo;
						bool isCollider = Physics.Raycast (ray, out hitinfo);
						if (isCollider && hitinfo.collider.tag == Tags.ground) {
								lookAtTarget (hitinfo.point);
						}

				} 
		else if (playermvoe.isMoving) {

			lookAtTarget(targetPosition);
		}*/
	}

	void ShowClickEffect(Vector3 hitPoint)
	{
		hitPoint = new Vector3 (hitPoint.x, hitPoint.y + 0.1f, hitPoint.z);
		//GameObject.Instantiate (effect_click_prefab, hitPoint, Quaternion.identity);
	}

	void lookAtTarget(Vector3 hitPoint)
	{
		targetPosition = hitPoint;
		targetPosition = new Vector3(targetPosition.x,transform.position.y,targetPosition.z);
		this.transform.LookAt(targetPosition);
	}

}
