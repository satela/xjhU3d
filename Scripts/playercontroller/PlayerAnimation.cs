using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {

	private PlayerMove move;
    private PlayerAttack attack;
	// Use this for initialization
	void Start () {
	
		move = this.GetComponent<PlayerMove> ();
        attack = this.GetComponent<PlayerAttack>();

	}
	
	// Update is called once per frame
	void LateUpdate () {

        if (attack.state == PlayerFightState.ControlWalk)
        {
            if (move.state == PlayerState.Moving)
            {
                PlayAnim("Run");
            }
            else if (move.state == PlayerState.Idle)
            {
                PlayAnim("Idle");
                Debug.Log("idle");
            }
        }
        else if (attack.state == PlayerFightState.NormalAttack)
        {
            if(attack.attack_state == AttackState.Moving)
            {
                PlayAnim("Run");
                Debug.Log("follow tarhet");
            }
        }
	}
	void PlayAnim(string aninName)
	{
		animation.CrossFade (aninName);
	}
}
