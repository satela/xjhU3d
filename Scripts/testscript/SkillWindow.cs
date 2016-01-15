using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class SkillWindow : MonoBehaviour {

    Rect mainSkillWndRect = new Rect(10, 350,350,200);



    //private string[] skill_type = { "攻击", "给敌人加buff","给己方加buff", "都加buff"};
    //private string[] near_fartype = { "近攻", "远攻" };
    //private string[] relatePos = { "中心", "前", "后", "上", "下", "左", "右" };

    //private string[] beatonBackFly = { "无", "击退", "击飞" };

  

    public ESkillType skilltype = ESkillType.Attack;

    public eAnimatorState animatorClip = eAnimatorState.atk0;//施放动作

 
    public EFirePos fireEffPos;

   // public eAnimatorState animatorBeatonClip = eAnimatorState.beaten;//施放受击动作

   // public EBeatonToBackFly beatonBackOrFly = EBeatonToBackFly.None;

    public UIInput skillIDText;

    public UIInput skillNameText;

    public UILabel skillTypeTxt;

    public UIInput skillMinDistTxt;

    public UILabel skillAttackActionTxt;

    public UILabel skillAttackEffectTxt;

    public UILabel skillAttackEffectPosTxt;

    public UIInput skillAttackEffectTimeTxt;

    public UIToggle isUseMoveEffTgle;

    public UIToggle isSingleMoveEffTgle;

    public UILabel moveEffUrlTxt;

    public UILabel explodeEffUrlTxt;


    public UIInput moveEffStartTimeInput;

    public UIToggle isAttackAllTgle;

    public UIInput harmDisTxt;

    public UIToggle isNeedAppointTgle;

    public UIToggle isShakeCameraTgle;

    public UIInput shakeTimeInput;
    //public UILabel beatonActionTxt;

    //public UIInput beatonTimeInput;

    //public UILabel beantonEffectTxt;

   // public UILabel beantonBackFlyTxt;


    public GameObject attackEffPrefab;

    public GameObject moveEffPrefab; //boss04_skl2_m

    public GameObject explodeEffPrefab;

   // public GameObject beatonEffPrefab;

    public GameObject beantEdit_btn;

    public GameObject save_btn;
    public GameObject look_btn;

    public BeatonEditorMag beatonPanelMg;

   // private Dictionary<eAnimatorState, string> m_actionName = new Dictionary<eAnimatorState, string>();

    public void Awake()
    {
        SkillConfiguration.LoadXml();
    }


	// Use this for initialization
	void Start () {       

        animatorClip = eAnimatorState.atk0;
       // animatorBeatonClip = eAnimatorState.beaten;

        skillIDText.value = "1001";
        skillAttackEffectTimeTxt.value = "0.2";
        moveEffStartTimeInput.value = "0.2";
        harmDisTxt.value = "0.2";
        //beatonTimeInput.value = "0.2";
        skillMinDistTxt.value = "1";
        shakeTimeInput.value = "0.8";
        UIEventListener.Get(save_btn).onClick += OnClickSave;
        UIEventListener.Get(look_btn).onClick += OnClickLook;
        UIEventListener.Get(beantEdit_btn).onClick += OnEditBeaton;

	}

    void OnEditBeaton(GameObject go)
    {
        beatonPanelMg.gameObject.SetActive(true);
        beatonPanelMg.initView(skilldata.beatonDatas);
    }
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (SkillConfiguration.skillsDic.TryGetValue(int.Parse(skillIDText.value),out skilldata))
            {
                skillNameText.value = skilldata.skillName;
                skilltype = skilldata.skilltype;
              //  beatonBackOrFly = skilldata.eBeatonbackFly;
                animatorClip = skilldata.animatorClip;
                skillAttackEffectTimeTxt.value = skilldata.fireTime.ToString();
                moveEffStartTimeInput.value = skilldata.moveBeginTime.ToString();
                harmDisTxt.value = skilldata.harmDist.ToString();
              //  beatonTimeInput.value = skilldata.beatonTime.ToString();
                skillMinDistTxt.value = skilldata.minAttackDist.ToString();

                animatorClip = skilldata.animatorClip;
                fireEffPos = skilldata.fireEffPos;

                isUseMoveEffTgle.value = skilldata.isUseMoveEff;
                isSingleMoveEffTgle.value = skilldata.isSingleMove;

                isNeedAppointTgle.value = skilldata.isNeedAppoint;
                isAttackAllTgle.value = skilldata.isQunGong;

                isShakeCameraTgle.value = skilldata.isShakeCamera;
                shakeTimeInput.value = skilldata.shakeTime.ToString();
              //  animatorBeatonClip = skilldata.animatorBeatonClip;

                if (!string.IsNullOrEmpty(skilldata.fireEffUrl))
                {
                    string asseturl = UrlManager.GetEffectUrl(skilldata.fireEffUrl, EEffectType.Attack);
                    attackEffPrefab = Resources.LoadAssetAtPath(asseturl, typeof(GameObject)) as GameObject;

                }
                else
                    attackEffPrefab = null;
                if (!string.IsNullOrEmpty(skilldata.moveEffUrl))
                {
                    string asseturl = UrlManager.GetEffectUrl(skilldata.moveEffUrl, EEffectType.Move);
                    moveEffPrefab = Resources.LoadAssetAtPath(asseturl, typeof(GameObject)) as GameObject;
                }
                else
                    moveEffPrefab = null;
                if (!string.IsNullOrEmpty(skilldata.explodeEffUrl))
                {
                    string asseturl = UrlManager.GetEffectUrl(skilldata.explodeEffUrl, EEffectType.Explode);
                    explodeEffPrefab = Resources.LoadAssetAtPath(asseturl, typeof(GameObject)) as GameObject;
                }
                else
                    explodeEffPrefab = null;
                //if (!string.IsNullOrEmpty(skilldata.beatonEffUrl))
                //{
                //    string asseturl = UrlManager.GetEffectUrl(skilldata.beatonEffUrl, EEffectType.Beaton);
                //    beatonEffPrefab = Resources.LoadAssetAtPath(asseturl, typeof(GameObject)) as GameObject;
                //}
                //else
                //    beatonEffPrefab = null;
            }
        }
        skillTypeTxt.text = "技能类型：" + DefaultSkillParam.skill_type[(int)skilltype];
        //skillDistTypeTxt.text = "攻击距离:" + near_fartype[(int)near_farAtk];

        skillAttackActionTxt.text = "施放动作：" + DefaultSkillParam.ActionName[animatorClip];
        if (attackEffPrefab != null)
            skillAttackEffectTxt.text = "施放特效：" + attackEffPrefab.name;
        else
            skillAttackEffectTxt.text = "施放特效：无";

        skillAttackEffectPosTxt.text = "施放位置：" + DefaultSkillParam.relatePos[(int)fireEffPos];

        if (moveEffPrefab != null)
            moveEffUrlTxt.text = "移动特效：" + moveEffPrefab.name;
        else
            moveEffUrlTxt.text = "移动特效：无";

        if (explodeEffPrefab != null)
            explodeEffUrlTxt.text = "爆炸特效：" + explodeEffPrefab.name;
        else
            explodeEffUrlTxt.text = "移动特效：无";
        
	}

    void OnClickSave(GameObject go)
    {
        setSkilldata();
        SkillConfiguration.saveSkill(skilldata);
    }

    DSkillBaseData skilldata = new DSkillBaseData();
    void OnClickLook(GameObject go)
    {

        setSkilldata();
        DBaseFightRole role = FightRoleManager._instance.getTestAttacker();
        if (role != null)
        {
            role.useSkill(skilldata);
        }
       // skilldata.minAttackDist
    }

    void setSkilldata()
    {
        skilldata = new DSkillBaseData();
        skilldata.skillName = skillNameText.value;
        skilldata.id = int.Parse(skillIDText.value);
        skilldata.animatorClip = animatorClip;
        // skilldata.near_farAtk = near_farAtk;
        skilldata.minAttackDist = float.Parse(skillMinDistTxt.value);

        if (attackEffPrefab != null)
            skilldata.fireEffUrl = attackEffPrefab.name;

        skilldata.fireTime = float.Parse(skillAttackEffectTimeTxt.value);
        skilldata.fireEffPos = fireEffPos;

        skilldata.isUseMoveEff = isUseMoveEffTgle.value;
        if (moveEffPrefab != null)
            skilldata.moveEffUrl = moveEffPrefab.name;
        skilldata.moveBeginTime = float.Parse(moveEffStartTimeInput.value);
        skilldata.isSingleMove = !isSingleMoveEffTgle.value;

        skilldata.isQunGong = isAttackAllTgle.value;

        skilldata.harmDist = float.Parse(harmDisTxt.value);

        skilldata.isNeedAppoint = isNeedAppointTgle.value;
        if (explodeEffPrefab != null)
            skilldata.explodeEffUrl = explodeEffPrefab.name;

        skilldata.isShakeCamera = isShakeCameraTgle.value;
        skilldata.shakeTime = float.Parse(shakeTimeInput.value);
        skilldata.beatonDatas = beatonPanelMg.getbeatonData();
        //skilldata.animatorBeatonClip = animatorBeatonClip;

        //if (beatonEffPrefab != null)
        //    skilldata.beatonEffUrl = beatonEffPrefab.name;
        //skilldata.beatonTime = float.Parse(beatonTimeInput.value);
        //skilldata.eBeatonbackFly = beatonBackOrFly;
    }
}
