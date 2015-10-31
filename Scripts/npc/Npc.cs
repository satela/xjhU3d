using UnityEngine;
using System.Collections;

public class Npc : MonoBehaviour {

    void OnMouseEnter()
    {
        CursorManager.instance.SetNpcTalk();
    }

    public void OnMouseExit()
    {
        CursorManager.instance.SetNormal();
    }

}
