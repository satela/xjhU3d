using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	private Transform player;
	private Vector3 offsetposition;
	public float distance = 0;
	public float scrollSpeed = 10;

	public float rotateSpeed = 2;
	private bool isRotating = false;

    private ShakeCamera shakecamera;
	// Use this for initialization
	void Start () {

        shakecamera = this.GetComponent<ShakeCamera>();
		//player = GameObject.FindGameObjectWithTag (Tags.player).transform;
		//transform.LookAt (player.position);
		//offsetposition = transform.position - player.position;
	}

    public void setplayer(DBaseFightRole playrole)
    {
        player = playrole.roleModel.transform;
        transform.LookAt(player.position);
        offsetposition = transform.position - player.position;

        playrole.roleModel.AddComponent<PlayerMove>();
        playrole.roleModel.AddComponent<PlayerDir>();
    }
	// Update is called once per frame
	void LateUpdate () {
        if (player != null)
        {
            if (!shakecamera.isInShake)
                transform.position = offsetposition + player.position;
            RotateView();

            ScrollView();
        }

	
	}

	void ScrollView()
	{
		distance = offsetposition.magnitude;
		distance += Input.GetAxis ("Mouse ScrollWheel") * scrollSpeed;
		distance = Mathf.Clamp(distance,2,18);
		offsetposition = offsetposition.normalized * distance;
	}
	void RotateView()
	{
		if (Input.GetMouseButtonDown (1)) 
		{
			isRotating = true;
		}
		if (Input.GetMouseButtonUp (1)) 
		{
			isRotating = false;
		}
		if (isRotating) 
		{
			transform.RotateAround(player.position,Vector3.up, rotateSpeed*Input.GetAxis("Mouse X"));

			Vector3 lastposition = transform.position;
			Vector3 lastanges = transform.eulerAngles;
			transform.RotateAround(player.position,-transform.right, rotateSpeed*Input.GetAxis("Mouse Y"));

			float x = transform.eulerAngles.x;
			if(x < 10 || x > 80)
			{
				transform.eulerAngles = lastanges;
				transform.position = lastposition;

			}
			//x = Mathf.Clamp(x,10,80);
			//transform.eulerAngles = new Vector3(x,transform.eulerAngles.y,transform.eulerAngles.z);
			offsetposition = transform.position - player.position;
		}
		//Input.GetAxis ("Mouse X");
		//Input.GetAxis ("Mouse Y");
	}
}
