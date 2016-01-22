using UnityEngine;
using System.Collections;

public enum PlayerFightState
{
    ControlWalk,
    NormalAttack,
    SkillAttack,
    Death
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

        if(isLockingTarget == false && Input.GetMouseButtonDown(0) && state != PlayerFightState.Death)
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
            if (attack_state !=AttackState.Tracking && distance <= min_attackDistance)
            {
                //转向目标 时应该播放原地踏步动画
                Quaternion rotation = Quaternion.LookRotation(target_normalattack.position - transform.position);

                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5 * Time.deltaTime);

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
                attack_state = AttackState.Tracking;
                timer = 0;
               
            }
            else if (attack_state == AttackState.Tracking && distance <= Min_followDist)
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
	
        if(isLockingTarget && Input.GetMouseButtonDown(0))
        {
            OnLockTarget();
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

        int tempharm =  (int)( harm * ((200f - def) / 200f));
        if (tempharm < 1)
            tempharm = 1;

        float value = Random.Range(0, 1f);
        if(value < PlayerStatus._instance.dodgeRate)
        {
            //AudioSource.PlayClipAtPoint(sound_miss, transform.position);
           // hudtext.Add("Miss", Color.cyan, 0f);
        }
        else
        {
            //hudtext.Add("-" + tempharm, Color.red, 1);
            PlayerStatus._instance.hp_remain -= tempharm;

            HeadStatusUI._instance.UpdateShowUI();
            if (PlayerStatus._instance.hp_remain <= 0)
            {
                state = PlayerFightState.Death;
               // Destroy(this.gameObject, 2);
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

    private SkillInfo cur_skill;
    public void useSkill(SkillInfo skill)
    {
        playermove.stopFollowing();
        animation.CrossFade(animName_Idle);
        switch(skill.applyType)
        {
            case ApplyType.Passive:
                StartCoroutine(OnPassiveSkillUse(skill));
                break;
            case ApplyType.Buff:
                StartCoroutine(OnBuffSkillUse(skill));
                break;
            case ApplyType.SingleTarget:
                OnSingleTargetSkill(skill);
                break;
            case ApplyType.MultiTarget:
                onMultiTargetSkillUse(skill);
                break;
        }

    }

    IEnumerator OnPassiveSkillUse(SkillInfo skill)
    {
        state = PlayerFightState.SkillAttack;
        animation.CrossFade(skill.animation_name);
        yield return new WaitForSeconds(skill.animation_time);
        state = PlayerFightState.ControlWalk;

        int addhp = 0; int addmp = 0;
        if (skill.applyProperty == ApplyProperty.HP)
            addhp = skill.applyValue;
        else if (skill.applyProperty == ApplyProperty.MP)
            addmp = skill.applyValue;
        PlayerStatus._instance.useDrugEff(addhp, addmp);

        GameObject effectprefab = null;
        if(SkillEffectRes._instance.skillDic.TryGetValue(skill.efx_name,out effectprefab))
        {
            GameObject.Instantiate(effectprefab, transform.position, Quaternion.identity);
        }

    }
    IEnumerator OnBuffSkillUse(SkillInfo skill)
    {
        state = PlayerFightState.SkillAttack;
        animation.CrossFade(skill.animation_name);
        yield return new WaitForSeconds(skill.animation_time);
        state = PlayerFightState.ControlWalk;
        
        switch(skill.applyProperty)
        {
            case ApplyProperty.Attack:
                PlayerStatus._instance.attack *= (skill.applyValue / 100f);
                break;
            case ApplyProperty.AttackSpeed:
                normalattack_rate *= (skill.applyValue / 100f);
                break;
            case ApplyProperty.Def:
                PlayerStatus._instance.def *= (skill.applyValue / 100f);
                break;
            case ApplyProperty.Speed:
                break;
        }

        GameObject effectprefab = null;
        if (SkillEffectRes._instance.skillDic.TryGetValue(skill.efx_name, out effectprefab))
        {
            GameObject.Instantiate(effectprefab, transform.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(skill.applyTime);

        switch (skill.applyProperty)
        {
            case ApplyProperty.Attack:
                PlayerStatus._instance.attack /= (skill.applyValue / 100f);
                break;
            case ApplyProperty.AttackSpeed:
                normalattack_rate /= (skill.applyValue / 100f);
                break;
            case ApplyProperty.Def:
                PlayerStatus._instance.def /= (skill.applyValue / 100f);
                break;
            case ApplyProperty.Speed:
                break;
        }
    }

    public bool isLockingTarget = false;//是否在选择目标
    //准备选择目标
    void OnSingleTargetSkill(SkillInfo skill)
    {
        CursorManager.instance.SetLockTarget();
        state = PlayerFightState.SkillAttack;
        isLockingTarget = true;
        cur_skill = skill;

    }

    //选择目标完成，开始技能释放
    void OnLockTarget()
    {
        switch(cur_skill.applyType)
        {
            case ApplyType.SingleTarget:
                StartCoroutine( OnLockSingleTarget());
                break;
            case ApplyType.MultiTarget:
                StartCoroutine(OnLockMultiTarget());
                break;
        }

    }

    IEnumerator OnLockSingleTarget()
    { 
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitinfo;
            bool isCollider = Physics.Raycast(ray, out hitinfo);
            if(isCollider && hitinfo.collider.tag == Tags.enemy)
            {
                transform.LookAt(hitinfo.point);
                animation.CrossFade(cur_skill.animation_name);
                yield return new WaitForSeconds(cur_skill.animation_time);
                state = PlayerFightState.ControlWalk;
                isLockingTarget = false;

                GameObject effectprefab = null;
                if (SkillEffectRes._instance.skillDic.TryGetValue(cur_skill.efx_name, out effectprefab))
                {
                    GameObject.Instantiate(effectprefab, hitinfo.collider.transform.position, Quaternion.identity);
                }

               // PlayerStatus._instance.attack *= cur_skill.applyValue / 100f;
                hitinfo.collider.GetComponent<BabyWolfManager>().TakeDamage((int)(PlayerStatus._instance.finalAttack*(cur_skill.applyValue/100f)));
            }
            else
            {
                state = PlayerFightState.NormalAttack;
            }
            CursorManager.instance.SetNormal();
    }

    void onMultiTargetSkillUse(SkillInfo skill)
    {
        CursorManager.instance.SetLockTarget();
        state = PlayerFightState.SkillAttack;
        isLockingTarget = true;
        cur_skill = skill;
    }

    IEnumerator OnLockMultiTarget()
    {
            CursorManager.instance.SetNormal();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitinfo;

            LayerMask mask = 1 << LayerMask.NameToLayer("GroundLayer");

            bool isCollider = Physics.Raycast(ray, out hitinfo,1000, mask);
            if (isCollider && hitinfo.collider.tag == Tags.ground)
            {
               // Quaternion rotation = Quaternion.LookRotation(transform.position - hitinfo.point);
              //  transform.rotation = rotation;
                transform.LookAt(hitinfo.point);
                animation.CrossFade(cur_skill.animation_name);
                yield return new WaitForSeconds(cur_skill.animation_time);
                state = PlayerFightState.ControlWalk;
                isLockingTarget = false;

                GameObject effectprefab = null;
                if (SkillEffectRes._instance.skillDic.TryGetValue(cur_skill.efx_name, out effectprefab))
                {
                    GameObject skill_eff =  GameObject.Instantiate(effectprefab, hitinfo.point + Vector3.up*0.5f, Quaternion.identity) as GameObject;
                    BallExplosionSkill skillmanager = skill_eff.AddComponent<BallExplosionSkill>();
                    skillmanager.attack = PlayerStatus._instance.finalAttack;
                    skillmanager.eff_radius = 2;
                }
            }
            else
                state = PlayerFightState.ControlWalk;
    }
}
