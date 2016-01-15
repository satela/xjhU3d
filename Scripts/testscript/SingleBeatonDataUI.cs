using UnityEngine;
using System.Collections;

public class SingleBeatonDataUI : MonoBehaviour {

    public GameObject beatonEff;

    public EBeatonToBackFly eBeatonbackFly = EBeatonToBackFly.None;

    public eAnimatorState animatorBeatonClip;//施放受击动作

    public UILabel actionNameTxt;

    public UILabel beatonEfftxt;

    public UILabel beatonBacktxt;

    public UIInput timeInput;

    public GameObject m_delete;

	// Use this for initialization
	void Start () {

        UIEventListener.Get(m_delete).onClick += OnDelteThis;
	}

    void OnDelteThis(GameObject go)
    {
        Destroy(gameObject);

        SendMessageUpwards("deleteBeatonUI",gameObject);
    }
    public void initdata(BeatonData datas)
    {
        eBeatonbackFly = datas.eBeatonbackFly;

        animatorBeatonClip = datas.animatorBeatonClip;

        timeInput.value = datas.beatonTime.ToString();

        if (!string.IsNullOrEmpty(datas.beatonEffUrl))
        {
            string asseturl = UrlManager.GetEffectUrl(datas.beatonEffUrl, EEffectType.Beaton);
            beatonEff = Resources.LoadAssetAtPath(asseturl, typeof(GameObject)) as GameObject;
        }
        else
            beatonEff = null;

        actionNameTxt.text = "受击动作：" + DefaultSkillParam.ActionName[animatorBeatonClip];

        beatonBacktxt.text = "击退击飞：" + DefaultSkillParam.beatonBackFly[(int)eBeatonbackFly];

        if (beatonEff != null)
            beatonEfftxt.text = "被击特效：" + beatonEff.name;
        else
            beatonEfftxt.text = "被击特效：无";
    }
	
    void Update()
    {
       // timeInput.value = datas.beatonTime.ToString();

        actionNameTxt.text = "受击动作：" + DefaultSkillParam.ActionName[animatorBeatonClip];

        beatonBacktxt.text = "击退击飞：" + DefaultSkillParam.beatonBackFly[(int)eBeatonbackFly];

        if (beatonEff != null)
            beatonEfftxt.text = "被击特效：" + beatonEff.name;
        else
            beatonEfftxt.text = "被击特效：无";
    
    }
    public BeatonData getData()
    {
        BeatonData data = new BeatonData();

        data.beatonTime = float.Parse(timeInput.value);
        data.animatorBeatonClip = animatorBeatonClip;
        if (beatonEff != null)
            data.beatonEffUrl = beatonEff.name;
        else
            data.beatonEffUrl = "";
        data.eBeatonbackFly = eBeatonbackFly;

        return data;
    }
}
