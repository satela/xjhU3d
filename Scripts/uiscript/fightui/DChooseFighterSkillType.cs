using UnityEngine;
using System.Collections;

public class DChooseFighterSkillType : MonoBehaviour {

    public UILabel pname;

    public UIToggle m_attack;

    public UIToggle m_control;

    public UIToggle m_recovery;

    public static DChooseFighterSkillType instance;

    public DBaseFightRole cur_role;
    private FightRoleSkill roleskill;

    public void Awake()
    {
        instance = this;

        m_attack.value = false;
        m_control.value = false;
        m_recovery.value = false;

        UIEventListener.Get(m_attack.gameObject).onClick += onClickAttack;
        UIEventListener.Get(m_control.gameObject).onClick += onClickControl;
        UIEventListener.Get(m_recovery.gameObject).onClick += onClickRecovery;

    }

    void onClickAttack(GameObject go)
    {
        if (m_attack.value && cur_role != null)
        {
            m_control.value = false;
            m_recovery.value = false;
            if (roleskill != null)
                roleskill.useSkillPriority = MainSkillType.MainSkillType_Attack;
        }
    }

    void onClickControl(GameObject go)
    {
        if (m_control.value && cur_role != null)
        {
            m_attack.value = false;
            m_recovery.value = false;
            if (roleskill != null)
                roleskill.useSkillPriority = MainSkillType.MainSkillType_Control;
        }
    }

    void onClickRecovery(GameObject go)
    {
        if (m_recovery.value && cur_role != null)
        {
            m_control.value = false;
            m_attack.value = false;
            if (roleskill != null)
                roleskill.useSkillPriority = MainSkillType.MainSkillType_Recovery;
        }
    }
	// Use this for initialization
	void Start () {
	

	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitinfo;

            // LayerMask mask = 1 << LayerMask.NameToLayer("GroundLayer");

            bool isCollider = Physics.Raycast(ray, out hitinfo);

            if (isCollider)
            {
                if (hitinfo.collider.transform.parent.GetComponent<DBaseFightRole>() != null)
                {
                    cur_role = hitinfo.collider.transform.parent.GetComponent<DBaseFightRole>();
                    pname.text = cur_role.name;
                    roleskill = hitinfo.collider.transform.parent.GetComponent<FightRoleSkill>();
                };
            }
        }
	}
}
