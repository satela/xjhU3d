using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillUIManager : MonoBehaviour {

    public static SkillUIManager _instance;
    private bool isShow = false;
    private TweenPosition tween;
    public GameObject closebtn;
    public UIGrid skillgrid;
    public GameObject skillitemPrefab;

    public List<SkillInfo> magicianSkills = new List<SkillInfo>();
    public List<SkillInfo> swordSkills = new List<SkillInfo>();

    private SkillItemUI[] skillitems;
    // Use this for initialization
	void Awake () {

        _instance = this;
        tween = this.GetComponent<TweenPosition>();
        UIManager.openCall += OnShowPanel;
        UIEventListener.Get(closebtn).onClick += OnClickClose;
	}

   
    private void initSkills()
    {
        List<SkillInfo> skills = SkillsInfo._instance.getHeroSkill(PlayerStatus._instance.heroType);
        skillitems = new SkillItemUI[skills.Count];
        for(int i=0; i < skills.Count;i++)
        {
            SkillItemUI skillitem = NGUITools.AddChild(skillgrid.gameObject, skillitemPrefab).GetComponent<SkillItemUI>();
            skillitem.setSkillInfo(skills[i]);
            skillitems[i] = skillitem;
            skillitem.UpdateEnabled();
        }

        skillgrid.Reposition();
    }
    void OnClickClose(GameObject go)
    {
        Hide();
    }
    void Start()
    {
        UIManager.openCall += OnShowPanel;
        initSkills();

    }

    void UpdateSkills()
    {
        foreach(SkillItemUI skillitem in skillitems)
        {
            skillitem.UpdateEnabled();
        }
    }
    private void OnShowPanel(EWindowName closeWindName)
    {
        if (closeWindName != EWindowName.SkillUI)
            Hide();

    }
	// Update is called once per frame
	void Update () {
	
	}

    public void TransformState()
    {
        if (isShow == false)
            Show();
        else
            Hide();
    }
    void Show()
    {
        isShow = true;
        gameObject.SetActive(true);
        tween.PlayForward();
        UpdateSkills();
    }
    void Hide()
    {
        isShow = false;
        tween.PlayReverse();
        gameObject.SetActive(false);
    }

    private void OnTweenFinished()
    {
        if (isShow == false)
            gameObject.SetActive(false);

    }
}
