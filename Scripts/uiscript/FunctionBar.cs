using UnityEngine;
using System.Collections;

public class FunctionBar : MonoBehaviour {

    [SerializeField]
    GameObject m_bagButton;

    [SerializeField]
    GameObject m_equipButton;

    [SerializeField]
    GameObject m_skillButton;

    [SerializeField]
    GameObject m_statusButton;

    [SerializeField]
    GameObject m_setingButton;

	// Use this for initialization
	void Start () {

        UIEventListener.Get(m_bagButton).onClick += onClickBagButton;
        UIEventListener.Get(m_equipButton).onClick += onClickEquipButton;
        UIEventListener.Get(m_skillButton).onClick += onClickSkillButton;
        UIEventListener.Get(m_statusButton).onClick += onClickStatusButton;
        UIEventListener.Get(m_setingButton).onClick += onClickSettingButton;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void onClickBagButton(GameObject go)
    {
        UIManager._instance.ShowNewpanel(EWindowName.Inventory);
        Inventory._instance.TransformState();
    }

    void onClickEquipButton(GameObject go)
    {
        UIManager._instance.ShowNewpanel(EWindowName.Equipment);
        EquipMentManager._instance.TransformState();

    }

    void onClickSkillButton(GameObject go)
    {
        UIManager._instance.ShowNewpanel(EWindowName.SkillUI);
        SkillUIManager._instance.TransformState();

    }

    void onClickStatusButton(GameObject go)
    {
        UIManager._instance.ShowNewpanel(EWindowName.Equipment);
        StatusManager._instance.TransformState();

    }

    void onClickSettingButton(GameObject go)
    {

    }
}
