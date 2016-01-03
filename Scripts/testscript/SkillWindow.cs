using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class SkillWindow : MonoBehaviour {

    Rect mainSkillWndRect = new Rect(10, 350,350,200);

    public string skillid = "1001";

    public string skillName = "霹雳掌";

    private string[] skill_type = { "普通攻击", "技能攻击" };
    private string[] near_fartype = { "近攻", "远攻" };
    private string[] relatePos = { "中心", "前", "后", "上", "下", "左", "右" };

    public ESkillDist near_farAtk = ESkillDist.NearAttack;
    public float minAttackDist = 2; // 最小施放距离 远攻 单体攻击时有效

    public ESkillType skilltype = ESkillType.NormalAttack;

    public eAnimatorState animatorClip = eAnimatorState.atk0;//施放动作

    public string fireEffUrl; //施放特效
    public float fireTime = 0;//特效施放 时间，从开始 施放技能 计算时间

    public EFirePos fireEffPos;

    public bool isUseMoveEff;//是否使用移动特效，比如朝着敌人 飞去

    public string moveEffUrl;//移动特效

    public bool isSingleMove = true;//是否只有一个移动特效，单体攻击技能最多只有一个，群攻 可能一个，可能多个，为 true 时，有几个被击 怪物则有几个移动特效


    public float moveBeginTime = 0;//移动特效 开始移动时间

    public bool isQunGong = false;//是否群攻技能

    public float harmDist = 1; //群攻 伤害 有效距离，即离 施放点 多远的怪物才会受到伤害

    public bool isNeedAppoint = false;//是否需要指点施放地点 不指定则使用 施放者位置

    public eAnimatorState animatorBeatonClip = eAnimatorState.beaten;//施放受击动作

    public string beatonEffUrl;// 受击特效

    public float beatonTime = 0.5f;//受击时间 没有移动攻击特效 时有效，否则 当移动攻击特效 达到受击者 时发生受击事件

    public UIInput skillIDText;

    public UIInput skillNameText;

    public UILabel skillTypeTxt;

    public UILabel skillDistTypeTxt;

    public UILabel skillAttackActionTxt;

    public UILabel skillAttackEffectTxt;

    public UILabel skillAttackEffectPosTxt;

    public UIInput skillAttackEffectTimeTxt;

    public UIToggle isUseMoveEffTgle;

    public UIToggle isSingleMoveEffTgle;

    public UILabel moveEffUrlTxt;

    public UIInput moveEffStartTimeInput;

    public UIToggle isAttackAllTgle;

    public UIInput harmDisTxt;

    public UIToggle isNeedAppointTgle;

    public UILabel beatonActionTxt;

    public UIInput beatonTimeInput;

    public UILabel beantonEffectTxt;

    public GameObject attackEffPrefab;

    public GameObject moveEffPrefab;

    public GameObject beatonEffPrefab;

    public GameObject save_btn;
    public GameObject look_btn;

    private Dictionary<eAnimatorState, string> m_actionName = new Dictionary<eAnimatorState, string>();
	// Use this for initialization
	void Start () {

        m_actionName.Add(eAnimatorState.atk0, "普通攻击1");
        m_actionName.Add(eAnimatorState.atk1, "普通攻击2");
        m_actionName.Add(eAnimatorState.atk2, "普通攻击3");
        m_actionName.Add(eAnimatorState.skl0, "技能攻击1");
        m_actionName.Add(eAnimatorState.skl1, "技能攻击2");
        m_actionName.Add(eAnimatorState.skl2, "技能攻击3");
        m_actionName.Add(eAnimatorState.skl3, "技能攻击4");
        m_actionName.Add(eAnimatorState.skl4, "技能攻击5");
        m_actionName.Add(eAnimatorState.skl5, "技能攻击6");
        m_actionName.Add(eAnimatorState.skl6, "旋风斩");


        m_actionName.Add(eAnimatorState.beaten, "普通被击");

        m_actionName.Add(eAnimatorState.fall, "摔倒");

        animatorClip = eAnimatorState.atk0;
        animatorBeatonClip = eAnimatorState.beaten;
        UIEventListener.Get(save_btn).onClick += OnClickSave;
        UIEventListener.Get(look_btn).onClick += OnClickLook;

	}

	// Update is called once per frame
	void Update () {

        skillTypeTxt.text = "技能类型：" + skill_type[(int)skilltype];
        skillDistTypeTxt.text = "攻击距离:" + near_fartype[(int)near_farAtk];

        skillAttackActionTxt.text = "施放动作：" + m_actionName[animatorClip];
        if (attackEffPrefab != null)
            skillAttackEffectTxt.text = "施放特效：" + attackEffPrefab.name;
        else
            skillAttackEffectTxt.text = "施放特效：无";

        skillAttackEffectPosTxt.text = "施放位置：" + relatePos[(int)fireEffPos];

        if (moveEffPrefab != null)
            moveEffUrlTxt.text = "移动特效：" + moveEffPrefab.name;
        else
            moveEffUrlTxt.text = "移动特效：无";

        beatonActionTxt.text = "受击动作：" + m_actionName[animatorBeatonClip];


        if (beatonEffPrefab != null)
            beantonEffectTxt.text = "被击特效：" + beatonEffPrefab.name;
        else
            beantonEffectTxt.text = "被击特效：无";
        
	}

    void OnClickSave(GameObject go)
    {

    }

    void OnClickLook(GameObject go)
    {
        DSkillBaseData skilldata = new DSkillBaseData();
        skilldata.skillName = skillNameText.value;
        skilldata.id = int.Parse(skillIDText.value);
        skilldata.animatorClip = animatorClip;
        skilldata.near_farAtk = near_farAtk;

        if(attackEffPrefab != null)
            skilldata.fireEffUrl = attackEffPrefab.name;

        skilldata.fireTime = float.Parse(skillAttackEffectTimeTxt.value);
        skilldata.fireEffPos = fireEffPos;

        skilldata.isUseMoveEff = isUseMoveEffTgle.value;
        if(moveEffPrefab != null)
            skilldata.moveEffUrl = moveEffPrefab.name;
        skilldata.moveBeginTime = float.Parse(moveEffStartTimeInput.value);
        skilldata.isSingleMove = isSingleMoveEffTgle.value;

        skilldata.isQunGong = isAttackAllTgle.value;

        skilldata.harmDist = float.Parse(harmDisTxt.value);

        skilldata.isNeedAppoint = isNeedAppointTgle.value;
        skilldata.animatorBeatonClip = animatorBeatonClip;

        if (beatonEffPrefab != null)
            skilldata.beatonEffUrl = beatonEffPrefab.name;
        skilldata.beatonTime = float.Parse(beatonTimeInput.value);


       // skilldata.minAttackDist
    }
}
