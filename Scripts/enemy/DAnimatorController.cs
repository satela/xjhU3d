using UnityEngine;
using System.Collections;

public class DAnimatorController : MonoBehaviour {

    private Animator animator;

    private eAnimatorState lastAnimatorState = eAnimatorState.await;

    public eAnimatorState currentState
    {
        get { 
            AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(0);
            int tagHash = stateinfo.tagHash;
            return (eAnimatorState)DAnimatorState.GetState(tagHash);
        }
    }
	// Use this for initialization
	void Start () {

        animator = this.GetComponent<Animator>();
	}


    public bool doBeHitted()
    {
        AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(0);

        int tagHash = stateinfo.tagHash;
        eAnimatorState m_curAnimatorState = (eAnimatorState)DAnimatorState.GetState(tagHash);

        if (m_curAnimatorState == eAnimatorState.await)
        {
            StartCoroutine(changeState(eAnimatorState.fall));
            return true;
        }
        return false;
    }

    public bool changeToState(eAnimatorState eState)
    {
        AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(0);

        int tagHash = stateinfo.tagHash;
        eAnimatorState m_curAnimatorState = (eAnimatorState)DAnimatorState.GetState(tagHash);
        if (eState != eAnimatorState.await && m_curAnimatorState == eAnimatorState.await)
        {
            StartCoroutine(changeState(eState));
            return true;
        }
        else if (eState == eAnimatorState.await)
        {
            resetToIdle();
            return true;

        }
        else
            return false;
    }
    IEnumerator changeState(eAnimatorState state)
    {
        int statehash = DAnimatorState.GetHash(state);
        animator.Play(statehash, 0, 0);
        animator.SetInteger(DAnimatorState.state, (int)state);
        yield return null;

        AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(0);
        while (stateinfo.normalizedTime < 1)
        {
            yield return null;
            stateinfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        resetToIdle();
    }

    void resetToIdle()
    {
        int statehash = DAnimatorState.GetHash(eAnimatorState.await);
       // animator.Play(statehash, 0, 0);
        animator.SetInteger(DAnimatorState.state, (int)eAnimatorState.await);
    }
	// Update is called once per frame
	void Update () {
	
	}
}
