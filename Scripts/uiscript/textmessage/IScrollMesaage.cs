using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IScrollMesaage : MonoBehaviour {

    private static IScrollMesaage _instance;

    public GameObject textprefab;

    private List<IFlowtext> allmessage;
    public void Awake()
    {
        _instance = this;
        allmessage = new List<IFlowtext>();
    }

    public static IScrollMesaage instance
    {
        get { return _instance; }
    }

    public void pushMessage(string message)
    {

        if(textprefab != null)
        {
            IFlowtext text = NGUITools.AddChild(gameObject, textprefab).GetComponent<IFlowtext>();
            //text.transform.localPosition = new Vector3(0, 50 * allmessage.Count, 0);
            text.setText(message);
           // allmessage.Add(text);
           // iTween.MoveTo(text,iTween.Hash(""))
        }
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
