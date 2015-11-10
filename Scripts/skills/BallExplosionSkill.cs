using UnityEngine;
using System.Collections;

public class BallExplosionSkill : MonoBehaviour {

    public float attack = 0;

    private int _eff_radius = 1;
	// Use this for initialization
	void Start () {

        
	}
	 public int eff_radius
    {
         get { return _eff_radius;}
         set
         {
             _eff_radius = value;
             SphereCollider collid = gameObject.AddComponent<SphereCollider>();
             collid.radius = _eff_radius;
             collid.isTrigger = true;
         }
    }

	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == Tags.enemy)
        {
            other.gameObject.GetComponent<BabyWolfManager>().TakeDamage((int)attack);
            Debug.Log("enemy:" + other.gameObject.name);

        }
    }

}
