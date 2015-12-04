using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class testskill : MonoBehaviour {

    public float attack = 0;

    private int _eff_radius = 1;

    private List<GameObject> hitwolf = new List<GameObject>();
    // Use this for initialization
    void Start()
    {


    }
    public int eff_radius
    {
        get { return _eff_radius; }
        set
        {
            _eff_radius = value;
            SphereCollider collid = gameObject.AddComponent<SphereCollider>();
            collid.radius = _eff_radius;
            collid.isTrigger = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.player)
        {
            if (hitwolf.IndexOf(other.gameObject) < 0)
            {
               // other.gameObject.GetComponent<BabyWolfManager>().TakeDamage((int)attack);
                other.gameObject.rigidbody.AddExplosionForce(1000, transform.position, 10);
                hitwolf.Add(other.gameObject);
            }


            Debug.Log("enemy:" + other.gameObject.name);

        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("colliser name:" + collision.collider.gameObject.name);
    }



}
