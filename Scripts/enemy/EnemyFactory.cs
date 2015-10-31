using UnityEngine;
using System.Collections;

public class EnemyFactory : MonoBehaviour {

    public GameObject enemyprefab;

    public static EnemyFactory _instance;

    public const int MAX_NUM = 0;
    public int currentNum = 0;

    public float time = 3;
    private float timer = 0;

	// Use this for initialization
	void Start () {

        _instance = this;
        createEnemy();
	}
	
	// Update is called once per frame
	void Update () {
	
        if(currentNum < MAX_NUM)
        {
            timer += Time.deltaTime;
            if(timer > time)
            {
                GameObject wolfbaby =  GameObject.Instantiate(enemyprefab) as GameObject;
                Vector3 pos = wolfbaby.transform.position;
                pos.x += Random.Range(-5, 5);
                pos.z += Random.Range(-5, 5);
                timer = 0;
                currentNum++;
            }
        }
	}

    public void createEnemy()
    {
        GameObject.Instantiate(enemyprefab);

    }
}
