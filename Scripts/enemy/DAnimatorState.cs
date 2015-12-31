using UnityEngine;
using System.Collections.Generic;

public enum eAnimatorState
{
    none = -1,//invalid value

    wait = 0,
    await = 1,

    run = 10,
    arun = 11,

    atk0 = 20,
    atk1 = 21,
    atk2 = 22,
    atk3 = 23,

    skl0 = 30,
    skl1 = 31,
    skl2 = 32,
    skl3 = 33,
    skl4 = 34,
    skl5 = 35,

    skl6 = 36,//旋风斩

    beaten = 40,
    parry = 41,

    fall_s = 50,
    fall = 51,
    fall_e = 52,

    up = 60,
    roll = 70,
    appear = 71,
    climb = 72,
    climbEnd = 73,

    die = 90,

    global = 100,//全局状态,并无对应的animation, 并不需要计算hash等
    //inTransition = 101,//状态转换中,并无对应的animation, 并不需要计算hash等
}

public class DAnimatorState
{
    //const define
    public const int ChangeState_OK = 1;
    public const int ChangeState_Failed = -1;
    public const int ChangeState_InTransition = 0;//动画和状态机还没有同步,应该处于transition阶段

    //control state param(not curve param)
    public static int state;
    public static int dead;

    //tag hash
    public static int wait;
    public static int Await;

    public static int run;
    public static int arun;

    public static int atk0;
    public static int atk1;
    public static int atk2;
    public static int atk3;
    public static int atk4;

    public static int skl0;
    public static int skl1;
    public static int skl2;
    public static int skl3;
    public static int skl4;
    public static int skl5;
    public static int skl6;
    public static int beaten;
    public static int parry;

    public static int fall_s;
    public static int fall;
    public static int fall_e;

    public static int up;
    public static int roll;
    public static int appear;

    public static int climb;
    public static int climbEnd;

    public static int die;


    const int STATE_COUNT = 128;
    private static int[] stateToHash = new int[STATE_COUNT];
    private static Dictionary<int, int> hashToState = new Dictionary<int, int>();
    //Transition

    static DAnimatorState()
    {
        state = Animator.StringToHash("roleAction");
        dead = Animator.StringToHash("dead");

        wait = Animator.StringToHash("wait");
        stateToHash[(int)eAnimatorState.wait] = wait;
        hashToState.Add(wait, (int)eAnimatorState.wait);

        Await = Animator.StringToHash("await");
        stateToHash[(int)eAnimatorState.await] = Await;
        hashToState.Add(Await, (int)eAnimatorState.await);

        run = Animator.StringToHash("run");
        stateToHash[(int)eAnimatorState.run] = run;
        hashToState.Add(run, (int)eAnimatorState.run);

        arun = Animator.StringToHash("arun");
        stateToHash[(int)eAnimatorState.arun] = arun;
        hashToState.Add(arun, (int)eAnimatorState.arun);

        atk0 = Animator.StringToHash("atk0");
        stateToHash[(int)eAnimatorState.atk0] = atk0;
        hashToState.Add(atk0, (int)eAnimatorState.atk0);

        atk1 = Animator.StringToHash("atk1");
        stateToHash[(int)eAnimatorState.atk1] = atk1;
        hashToState.Add(atk1, (int)eAnimatorState.atk1);

        atk2 = Animator.StringToHash("atk2");
        stateToHash[(int)eAnimatorState.atk2] = atk2;
        hashToState.Add(atk2, (int)eAnimatorState.atk2);

        atk3 = Animator.StringToHash("atk3");
        stateToHash[(int)eAnimatorState.atk3] = atk3;
        hashToState.Add(atk3, (int)eAnimatorState.atk3);

        skl0 = Animator.StringToHash("skl0");
        stateToHash[(int)eAnimatorState.skl0] = skl0;
        hashToState.Add(skl0, (int)eAnimatorState.skl0);

        skl1 = Animator.StringToHash("skl1");
        stateToHash[(int)eAnimatorState.skl1] = skl1;
        hashToState.Add(skl1, (int)eAnimatorState.skl1);

        skl2 = Animator.StringToHash("skl2");
        stateToHash[(int)eAnimatorState.skl2] = skl2;
        hashToState.Add(skl2, (int)eAnimatorState.skl2);

        skl3 = Animator.StringToHash("skl3");
        stateToHash[(int)eAnimatorState.skl3] = skl3;
        hashToState.Add(skl3, (int)eAnimatorState.skl3);

        skl4 = Animator.StringToHash("skl4");
        stateToHash[(int)eAnimatorState.skl4] = skl4;
        hashToState.Add(skl4, (int)eAnimatorState.skl4);

        skl5 = Animator.StringToHash("skl5");
        stateToHash[(int)eAnimatorState.skl5] = skl5;
        hashToState.Add(skl5, (int)eAnimatorState.skl5);

        skl6 = Animator.StringToHash("skl6");
        stateToHash[(int)eAnimatorState.skl6] = skl6;
        hashToState.Add(skl6, (int)eAnimatorState.skl6);

        beaten = Animator.StringToHash("beaten");
        stateToHash[(int)eAnimatorState.beaten] = beaten;
        hashToState.Add(beaten, (int)eAnimatorState.beaten);

        parry = Animator.StringToHash("parry");
        stateToHash[(int)eAnimatorState.parry] = parry;
        hashToState.Add(parry, (int)eAnimatorState.parry);

        fall_s = Animator.StringToHash("fall_s");
        stateToHash[(int)eAnimatorState.fall_s] = fall_s;
        hashToState.Add(fall_s, (int)eAnimatorState.fall_s);

        fall = Animator.StringToHash("fall");
        stateToHash[(int)eAnimatorState.fall] = fall;
        hashToState.Add(fall, (int)eAnimatorState.fall);

        fall_e = Animator.StringToHash("fall_e");
        stateToHash[(int)eAnimatorState.fall_e] = fall_e;
        hashToState.Add(fall_e, (int)eAnimatorState.fall_e);

        up = Animator.StringToHash("up");
        stateToHash[(int)eAnimatorState.up] = up;
        hashToState.Add(up, (int)eAnimatorState.up);

        roll = Animator.StringToHash("roll");
        stateToHash[(int)eAnimatorState.roll] = roll;
        hashToState.Add(roll, (int)eAnimatorState.roll);

        appear = Animator.StringToHash("appear");
        stateToHash[(int)eAnimatorState.appear] = appear;
        hashToState.Add(appear, (int)eAnimatorState.appear);

        climb = Animator.StringToHash("climb");
        stateToHash[(int)eAnimatorState.climb] = climb;
        hashToState.Add(climb, (int)eAnimatorState.climb);

        climbEnd = Animator.StringToHash("climbEnd");
        stateToHash[(int)eAnimatorState.climbEnd] = climbEnd;
        hashToState.Add(climbEnd, (int)eAnimatorState.climbEnd);

        die = Animator.StringToHash("die");
        stateToHash[(int)eAnimatorState.die] = die;
        hashToState.Add(die, (int)eAnimatorState.die);
    }

    public static int GetHash(eAnimatorState eState)
    {
        return stateToHash[(int)eState];
    }

    public static int GetState(int hash)
    {
        int state;
        bool suc = hashToState.TryGetValue(hash, out state);
        return suc ? state : 0;
    }

    public static bool IsAtkState(eAnimatorState eState)
    {
        if (eState == eAnimatorState.atk0
            || eState == eAnimatorState.atk1
            || eState == eAnimatorState.atk2
            || eState == eAnimatorState.atk3)
            return true;

        return false;
    }

    public static bool IsSklState(eAnimatorState eState)
    {
        if (eState == eAnimatorState.skl0
            || eState == eAnimatorState.skl1
            || eState == eAnimatorState.skl2
            || eState == eAnimatorState.skl3
            || eState == eAnimatorState.skl4
            || eState == eAnimatorState.skl5
            || eState == eAnimatorState.skl6)
            return true;

        return false;
    }

    public static bool IsFallState(eAnimatorState eState)
    {
        if (eState == eAnimatorState.fall_s
            || eState == eAnimatorState.fall
            || eState == eAnimatorState.fall_e)
            return true;

        return false;
    }

    public static bool IsLoopState(eAnimatorState eState)
    {
        if (eState == eAnimatorState.await
            || eState == eAnimatorState.wait
            || eState == eAnimatorState.run
            || eState == eAnimatorState.arun
            || eState == eAnimatorState.die
            || eState == eAnimatorState.climb
            || eState == eAnimatorState.skl6)
            return true;

        return false;
    }

    public static bool IsHitEndState(eAnimatorState eState)
    {
        if (eState == eAnimatorState.beaten
            || eState == eAnimatorState.parry
            || eState == eAnimatorState.up)
            return true;

        return false;
    }
}

