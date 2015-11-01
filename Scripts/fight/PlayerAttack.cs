using UnityEngine;
using System.Collections;

public enum PlayerFightState
{
    ControlWalk,
    NormalAttack,
    SkillAttack,
    Death
}

public enum AttackState //攻击时状态
{
    Moving,
    Idle,
    Attack
}
public class PlayerAttack : MonoBehaviour {

    [HideInInspector]
    public PlayerFightState state = PlayerFightState.ControlWalk;
    public AttackState attack_state = AttackState.Idle;
    public string animName_normalattack;//普通攻击动画名
    public string animName_Idle;
    private string animName_now;
    public string animName_Move;
    public string animName_Death;
    public string animName_Hited;

    public float time_normalattack;//普通攻击动画时间
    private float timer = 0;
    public float min_attackDistance = 5;//默认攻击的最小距离
    public float normalattack_rate = 1;

    public float Min_followDist = 3.5f;//攻击目标跑开，重新跟踪需要到达的最小距离
    //public float normal_attack = 34;//普通攻击伤害值

    private Transform target_normalattack;

    private PlayerMove playermove;

    private bool showEffect = false;

    public GameObject attack_effect;
	// Use this for initialization
	void Start () {

        playermove = this.GetComponent<PlayerMove>();
        animName_now = animName_normalattack;

        initHudText();
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitinfo;
            bool isCollider = Physics.Raycast(ray, out hitinfo);
            if(isCollider && hitinfo.collider.tag == Tags.enemy)
            {
                target_normalattack = hitinfo.collider.transform;
                state = PlayerFightState.NormalAttack;//进入普通攻击模式
                showEffect = false;
                timer = 0;
                playermove.stopFollowing();

            }
            else
            {
                state = PlayerFightState.ControlWalk;
                target_normalattack = null;
            }
        }

        if(state == PlayerFightState.NormalAttack)
        {
            if (target_normalattack == null)
            {
                state = PlayerFightState.ControlWalk;
                playermove.stopFollowing();
                return;
            }
            if (isInBeHited)
                return;

            float distance = Vector3.Distance(transform.position, target_normalattack.position);
            if (attack_state !=AttackState.Moving && distance <= min_attackDistance)
            {
                //转向目标 时应该播放原地踏步动画
               // Vector3 difvec = new Vector3(target_normalattack.position.x, 0, target_normalattack.position.z) - new Vector3(transform.position.x, 0, transform.position.z);
               // float angle = Vector3.Angle(transform.forward, difvec);


               // if (Mathf.Abs(angle) > 0 && animName_now == animName_Idle)
               //     animName_now = animName_Move;
               // else if (animName_now != animName_normalattack)
               //     animName_now = animName_Idle;

                transform.LookAt(target_normalattack.position);

                attack_state = AttackState.Attack;
                timer += Time.deltaTime;
                animation.CrossFade(animName_now);
                if(timer >= time_normalattack)
                {
                    animName_now = animName_Idle;
                    if (!showEffect)
                    {
                        showEffect = true;
                        GameObject.Instantiate(attack_effect, target_normalattack.position, Quaternion.identity);
                        target_normalattack.GetComponent<BabyWolfManager>().TakeDamage(PlayerStatus._instance.finalAttack);
                    }
                    //播放特效
                }

                if(timer >= 1f/normalattack_rate)
                {
                    showEffect = false;
                    timer = 0;
                    animName_now = animName_normalattack; 
                }
            }
            else if (distance > Min_followDist)
            {

                playermove.MoveToTargetRadius(target_normalattack.position);
                attack_state = AttackState.Moving;
                timer = 0;
               
            }
            else if(attack_state == AttackState.Moving && distance <= Min_followDist)
            {
                attack_state = AttackState.Attack;
                playermove.stopFollowing();
               // animation.CrossFade(animName_Idle);
              //  animName_now = animName_Idle;

            }
        }
        else if(state == PlayerFightState.Death)
        {
            animation.CrossFade(animName_Death);
        }
	
	}

    public GameObject hudtextFollow;//显示飘血的文字
    private GameObject hudtextGo;
    private HUDText hudtext;
    private UIFollowTarget followtarget;
    public AudioClip sound_miss;

    void initHudText()
    {

        hudtextGo = HudTextParent._instacne.createHudText();
        hudtext = hudtextGo.GetComponent<HUDText>();
        followtarget = hudtext.GetComponent<UIFollowTarget>();
        followtarget.target = hudtextFollow.transform;
        followtarget.gameCamera = Camera.main;
        StartCoroutine(setUIcamera());
    }
    IEnumerator setUIcamera()
    {
        yield return new WaitForSeconds(2);

        followtarget.uiCamera = UICamera.currentCamera;

    }
    public void takeDamage(int harm)
    {
        if (state == PlayerFightState.Death) return;
        int def = PlayerStatus._instance.finalDef;

        int tempharm =  (int) harm * ((200 - def) / 200);
        if (tempharm < 1)
            tempharm = 1;

        float value = Random.Range(0, 1f);
        if(value < PlayerStatus._instance.dodgeRate)
        {
            AudioSource.PlayClipAtPoint(sound_miss, transform.position);
            hudtext.Add("Miss", Color.cyan, 0f);
        }
        else
        {
            hudtext.Add("-" + tempharm, Color.red, 1);
            PlayerStatus._instance.hp -= tempharm;

            if(PlayerStatus._instance.hp <= 0)
            {
                state = PlayerFightState.Death;
                Destroy(this.gameObject, 2);
            }
            else
            {
                //aniState = WolfAnimateState.Hitted;
                StartCoroutine(showHittedAnimation());
            }

        }
    }

    public bool isInBeHited = false;
    IEnumerator showHittedAnimation()
    {
        isInBeHited = true;

        yield return new WaitForSeconds(0.2f);
        animation.CrossFade(animName_Hited);
        //cur_animName = animName_Hited;
        yield return new WaitForSeconds(0.567f);
        animation.CrossFade(animName_Idle);
        isInBeHited = false;

    }
}
