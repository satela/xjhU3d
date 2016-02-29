using UnityEngine;
using System.Collections;

public class IFlowtext : MonoBehaviour {

    public UILabel text;
	// Use this for initialization
	void Start () {
	
	}

    public void setText(string msg)
    {
        text.text = msg;
        Vector3 targetpos = new Vector3(transform.localPosition.x, transform.localPosition.y + 30, transform.localPosition.z);
        iTween.MoveTo(gameObject, iTween.Hash("position", targetpos,  "delay", 1, "time", 2f, "oncomplete", "destroyite"));

    }

    void destroyite()
    {
        Destroy(gameObject);
    }
	// Update is called once per frame
	void Update () {
	
	}
}
