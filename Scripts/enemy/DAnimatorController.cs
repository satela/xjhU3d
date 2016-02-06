using UnityEngine;
using System.Collections;
using System;
public class DAnimatorController : MonoBehaviour {

    private Animator animator;

    private eAnimatorState lastAnimatorState = eAnimatorState.await;

    public eAnimatorState currentState
    {
        get {
            if (animator != null)
            {
                AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(0);
                int tagHash = stateinfo.tagHash;
                return (eAnimatorState)DAnimatorState.GetState(tagHash);
            }
            else
                return eAnimatorState.await;
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
        if (animator == null)
            return false;
        AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(0);

        int tagHash = stateinfo.tagHash;
        eAnimatorState m_curAnimatorState = (eAnimatorState)DAnimatorState.GetState(tagHash);
        if (m_curAnimatorState == eState && eState == eAnimatorState.arun)
            return true;

        if ((int)eState < (int)m_curAnimatorState)
            return false;

        if (eState != eAnimatorState.await)
        {
            StopAllCoroutines();
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
        try
        {
            animator.Play(statehash, 0, 0);
            animator.SetInteger(DAnimatorState.state, (int)state);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
            yield return null;

            AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(0);

            if (state == eAnimatorState.die)
            {
                animator.SetInteger(DAnimatorState.state, -1);
            }
            else
            {
                while (stateinfo.normalizedTime < 1)
                {
                    yield return null;
                    stateinfo = animator.GetCurrentAnimatorStateInfo(0);
                }
                if (currentState != eAnimatorState.arun)
                    resetToIdle();
            }                
      
    }

    public void resetToIdle()
    {
        int statehash = DAnimatorState.GetHash(eAnimatorState.await);
       // animator.Play(statehash, 0, 0);
        if(animator != null)
        animator.SetInteger(DAnimatorState.state, (int)eAnimatorState.await);
    }

    public void setAnimatorSpeed(float values)
    {

        animator.speed = values;
        
    }

	// Update is called once per frame
	void Update () {
	
	}
}
