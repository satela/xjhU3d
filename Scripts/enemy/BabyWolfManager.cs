﻿using UnityEngine;
using System.Collections;


public enum WolfAnimateState
{
    Idle,
    Walk,
    Hitted,
    Attack,
    Death
}
public class BabyWolfManager : MonoBehaviour {

    public WolfAnimateState aniState = WolfAnimateState.Idle;

    public string animName_death;

    private string cur_animName;

    public string animName_Idle;
    public string animName_Walk;

    public string animName_Hited;
    public string animName_normalAttack;//普通攻击
    public string animName_crazyAttack;//疯狂攻击
    public int attack = 10;//伤害值

    public int kill_exp = 20;//杀死 可以获得的经验

    [HideInInspector]
    public string animate_attack_now;
    public float time_normalattack;

    public float time_crazyattack;

    public float time_hittedAnimation;


   // [HideInInspector]
    public int attac_rate = 4;//攻击速率每秒
    public float crazy_attack_rate;

    private float time = 2;
    private float timer = 0;
    public float speed = 1;
    private CharacterController cc;
    private NavMeshAgent agent;

    public float miss_rate = 0.2f;

    public int hp = 1000;
    public int max_hp = 1000;

    private Color normal;
    private float red_time = 1;//显示被击中的时间
    private float attack_timer = 0;//攻击计时器
    private bool is_attacked = false;

    public AudioClip sound_miss;

    public GameObject hudtextFollow;//显示飘血的文字
    private GameObject hudtextGo;

    private HUDText hudtext;
    private UIFollowTarget followtarget;

    public Renderer bodyrender;

    public Transform target;
    public float attack_minDistance = 2;
    public float attack_maxDistance = 5;

    public UIProgressBar hpbar;
    public float hpbarScale;
    public Transform hpbarpos;

    private bool isInHittedAnimation = false;
	// Use this for initialization
	void Start () {

        cc = this.GetComponent<CharacterController>();
        agent = this.GetComponent<NavMeshAgent>();
        normal = bodyrender.material.color;

        hudtextGo = HudTextParent._instacne.createHudText();
        hudtext = hudtextGo.GetComponent<HUDText>();
        followtarget = hudtext.GetComponent<UIFollowTarget>();
        followtarget.target = hudtextFollow.transform;
        followtarget.gameCamera = Camera.main;
        cur_animName = animName_Idle;
        animate_attack_now = animName_Idle;

        initHpBar();
        StartCoroutine(setUIcamera());
	}
	
    IEnumerator setUIcamera()
    {
        yield return new WaitForSeconds(2);

        followtarget.uiCamera = UICamera.currentCamera;

    }

    void initHpBar()
    {
        hpbar = RoleBarUIManager._instance.createHp();

    }
	// Update is called once per frame
	void Update () {

        if (isInBeHitted)
            return;
        if(aniState == WolfAnimateState.Death)
        {
            GetComponent<Animation>().CrossFade(animName_death);
        }
        else if(aniState == WolfAnimateState.Attack)
        {
            //TODO
            if(isInHittedAnimation == false)
             AutoAttack();
        }        
        else
        {
            //巡逻
           /* animation.CrossFade(cur_animName);
            if(cur_animName == animName_Walk)
            {
                agent.SetDestination(transform.position + transform.forward * (1 + 2 * Random.Range(0, 1)));
                //cc.SimpleMove(transform.forward * speed);
            }*/

            if (agent.enabled && agent.remainingDistance > 0.2)
                GetComponent<Animation>().CrossFade(animName_Walk);
            else
                GetComponent<Animation>().CrossFade(animName_Idle);

            timer += Time.deltaTime;
            if(timer >= time)
            {
                timer = 0;
                RandomState();
            }

        }

        

        if(Input.GetMouseButtonDown(1))
        {
           // aniState = WolfAnimateState.Attack;
            //target = GameObject.FindGameObjectWithTag(Tags.player).transform;
        }
        updateHpBarPos();
	}
    IEnumerator showHittedAnimation()
    {
        isInHittedAnimation = true;
        GetComponent<Animation>().CrossFade(animName_Hited);
        //cur_animName = animName_Hited;
        yield return new WaitForSeconds(time_hittedAnimation);
        GetComponent<Animation>().CrossFade(animName_Idle);
        isInHittedAnimation = false;

    }
    void RandomState()
    {
        int value = Random.Range(0, 2);
        if (value == 1)
        {
            if (cur_animName != animName_Walk)
            {
                cur_animName = animName_Walk;
                transform.Rotate(transform.up * Random.Range(0,360));
                if (agent.enabled)
                agent.SetDestination(transform.position + transform.forward * (agent.speed + 2 * Random.Range(1, 3)));
            }

        }
        else
        {
            stopMove();
            cur_animName = animName_Idle;
            GetComponent<Animation>().CrossFade(animName_Idle);
        }

    }

    //受到伤害
    public void TakeDamage(int attack)
    {
        if (aniState == WolfAnimateState.Death)
            return;
        float value = Random.Range(0f, 1);

        target = GameObject.FindGameObjectWithTag(Tags.player).transform;
       // transform.LookAt(target);

        if(value < miss_rate)
        {
            //AudioSource.PlayClipAtPoint(sound_miss, transform.position);
            hudtext.Add("Miss", Color.cyan,0f);
            aniState = WolfAnimateState.Attack;
        }
        else
        {
            this.hp -= attack;
            hudtext.Add("-" + attack, Color.red, 0f);
            StartCoroutine(showBodyRed());
            if(this.hp <= 0)
            {
                aniState = WolfAnimateState.Death;
                EnemyFactory._instance.createEnemy();
                Destroy(this.gameObject, 2);
            }
            else
            {
                StartCoroutine(showHittedAnimation());
                aniState = WolfAnimateState.Attack;

            }
        }
    }

    IEnumerator showBodyRed()
    {
        bodyrender.material.color = Color.red;
        yield return new WaitForSeconds(1f);

        bodyrender.material.color = normal;


    }

    public void OnDestroy()
    {
        GameObject.Destroy(hudtextGo);
        if(hpbar != null)
        Destroy(hpbar.gameObject);
        PlayerStatus._instance.getExp(kill_exp);
        BarNpc._instance.OnSkillWolf();
    }

    public void AutoAttack()
    {
        if(target != null)
        {

            float distacne = Vector3.Distance(target.position, transform.position);
            if(distacne > attack_maxDistance)//停止自动攻击
            {
                target = null;
                aniState = WolfAnimateState.Idle;
            }
            else if(distacne <= attack_minDistance)//自动攻击
            {
                Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5 * Time.deltaTime);
                stopMove();
                if(attack_timer == 0)
                {
                    RandomAttack();
                    //attack_timer = 0;
                    GetComponent<Animation>().CrossFade(animate_attack_now);
                }

                attack_timer += Time.deltaTime;

                if(animate_attack_now == animName_normalAttack)
                {
                    if(attack_timer > time_normalattack)
                    {
                        //产生伤害 TODO
                        animate_attack_now = animName_Idle;
                        GetComponent<Animation>().CrossFade(animate_attack_now);

                    }
                }
                else if(animate_attack_now == animName_crazyAttack)
                {
                    if (attack_timer > time_crazyattack)
                    {
                        //产生伤害 TODO
                        animate_attack_now = animName_Idle;
                        GetComponent<Animation>().CrossFade(animate_attack_now);

                    }
                }
                if(attack_timer > attac_rate)
                {
                    //再次攻击
                    RandomAttack();
                    attack_timer = 0;
                    attack_timer += Time.deltaTime;
                    GetComponent<Animation>().CrossFade(animate_attack_now);
                }
            }
            else//朝着主角移动
            {
                //transform.LookAt(target);
                Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5 * Time.deltaTime);
                if (agent.enabled)
                agent.SetDestination(transform.position + transform.forward * (2 + 2 * Random.Range(0, 1)));

                //cc.SimpleMove(transform.forward * speed);
                GetComponent<Animation>().CrossFade(animName_Walk);
                attack_timer = 0;
            }
        }
        else
        {
            aniState = WolfAnimateState.Idle;
            GetComponent<Animation>().CrossFade(animName_Idle);
            attack_timer = 0;
        }

    }
    void RandomAttack()
    {
         float value = Random.Range(0, 1f);
         if (value < crazy_attack_rate)
         {
             animate_attack_now = animName_crazyAttack;
         }
         else
             animate_attack_now = animName_normalAttack;

        target.GetComponent<PlayerAttack>().takeDamage(this.attack);

    }

    void stopMove()
    {
        if (agent.enabled)
        agent.SetDestination(transform.position);
    }
    void updateHpBarPos()
    {

        float newFomat = hpbarScale / Vector3.Distance(hpbarpos.position, Camera.main.transform.position);
        hpbar.transform.position = WorldToUI(hpbarpos.position);
        //计算出血条的缩放比例   
        hpbar.transform.localScale = Vector3.one * newFomat;
        hpbar.value = (float)hp / max_hp;
        
    }
    public static Vector3 WorldToUI(Vector3 point)
    {
        Vector3 pt = Camera.main.WorldToScreenPoint(point);
        //我发现有时候UICamera.currentCamera 有时候currentCamera会取错，取的时候注意一下啊。  
        Vector3 ff = UICamera.currentCamera.ScreenToWorldPoint(pt);
        //UI的话Z轴 等于0   
        ff.z = 0;
        return ff;
    }  
    public void OnMouseEnter()
    {
        if(PlayerStatus._instance.gameObject.GetComponent<PlayerAttack>().isLockingTarget == false)
        CursorManager.instance.SetAttack();
    }

    public void OnMouseExit()
    {
        if (PlayerStatus._instance.gameObject.GetComponent<PlayerAttack>().isLockingTarget == false)
        CursorManager.instance.SetNormal();

    }

    #region 击飞效果

    bool isInBeHitted = false;
    bool isgrounded = false;
    public void hitover(Vector3 hitforce)
    {
        isInBeHitted = true;
        agent.enabled = false;
        isgrounded = false;
        hitforce = new Vector3(hitforce.x * 0.2f, hitforce.y*0.6f, hitforce.z * 0.2f);
        StartCoroutine(hitfly(hitforce));
       // animation.CrossFade(animName_death);
       // animation[animName_death].speed = 2;
    }
    IEnumerator hitfly(Vector3 hitforce)
    {
        Vector3 movedir = hitforce;
        while (!isgrounded)
        {
            transform.position += movedir;
            if (transform.localEulerAngles.x > -89)
            {
                float angles = Mathf.Lerp(transform.localEulerAngles.x, -90,Time.deltaTime);
                transform.localEulerAngles = new Vector3(angles,transform.localEulerAngles.y, transform.localEulerAngles.z);
            }
            yield return null;

            movedir -= Vector3.up * 0.04f;

        }


    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.ground)
        {
            isgrounded = true;
             transform.position += Vector3.up;
            //animation.CrossFade(animName_death);

            StartCoroutine(resetpos());
        }
    }

    IEnumerator resetpos()
    {
        yield return new WaitForSeconds(1);
        agent.enabled = true;
        isInBeHitted = false;
        agent.SetDestination(transform.position + transform.forward * 0.1f);
    }
    #endregion
}
