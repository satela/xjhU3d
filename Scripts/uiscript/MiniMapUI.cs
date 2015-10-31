using UnityEngine;
using System.Collections;

public class MiniMapUI : MonoBehaviour {

    public GameObject zoomin;
    public GameObject zoomout;

    private Camera minimapCamera;
    public void Awake()
    {
        UIEventListener.Get(zoomin).onClick += OnZoomIn;
        UIEventListener.Get(zoomout).onClick += OnZoomOut;
        minimapCamera = GameObject.FindGameObjectWithTag(Tags.MiniMapCamera).camera;

    }

    public void OnZoomIn(GameObject go)
    {
        if (minimapCamera.orthographicSize > 0)
        minimapCamera.orthographicSize--;
    }

    public void OnZoomOut(GameObject go)
    {
        if (minimapCamera.orthographicSize < 20)
        minimapCamera.orthographicSize++ ;

    }


}
