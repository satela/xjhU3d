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

        if (attack.isInBeHited)
            return;
        if (attack.state == PlayerFightState.ControlWalk)
        {
            if (move.state == PlayerState.Moving)
            {
                PlayAnim("Run");
            }
            else if (move.state == PlayerState.Idle)
            {
                PlayAnim("Idle");
            }
        }
        else if (attack.state == PlayerFightState.NormalAttack)
        {
            if (attack.attack_state == AttackState.Tracking)
            {
                PlayAnim("Run");
            }
        }
	}
	void PlayAnim(string aninName)
	{
		animation.CrossFade (aninName);
	}
}
