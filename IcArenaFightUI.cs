using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

public enum Enum_VsMapType
{
    EVT_JinJiChang,         // 竞技场
    EVT_LuanZhan,           // 公会乱战
}

public class IcArenaFightUI: MonoBehaviour 
{
	IcRole m_fightRole = null;
	float BossRowHpValue{ get; set; }
	int BossRowCount{ get; set;	}
	
	void OnEnable()
	{
		m_selectIndex = FIRST_ROLE_INDEX;
		for(int i = 0; i< CRoleDefine.MAX_ZHEN_SERVANT; i++)
		{
			m_pServantAlive[i] = true;
			m_pTarServantAlive[i] = true;
		}
	}

    Enum_VsMapType m_vsMapTyep = Enum_VsMapType.EVT_JinJiChang;
	void Start()
	{
        m_vsMapTyep = CGameWorld.GetUIDataManager().VSMapType;

		IcCommonUIManager.Instance.onRole += UpdateRole;
		
		//if(m_myServantObj[FIRST_ROLE_INDEX] != null)
		//	UpdateRole(FIRST_ROLE_INDEX, m_myServantObj[FIRST_ROLE_INDEX].gameObject);

        m_fightRole = CGameWorld.FightRole();
        if (m_fightRole == null)
            return;

        OnResurreEvent();
		IcJoystick.instance.gameObject.SetActive(false);
        m_challengeEnd.SetActive(false);
        if (null != m_returnCity)
            UIEventListener.Get(m_returnCity).onClick = ReturnCity;
	}

	void OnDestroy()
	{
		IcJoystick.instance.gameObject.SetActive(true);
		IcCommonUIManager.Instance.onRole -= UpdateRole;
	}
	
#region 是否用作特殊用途	
	bool IsNormalMap
	{
		get
		{
//			CBaseMap map = CGameWorld.GetMap();
//			if(map != null)
//			{
//                return map.IsNormalClientDungeonMap();
//			}
//			
//			return false;
			
			return true;
		}		
	}
	#endregion
	
#region 初始化
    IcRole[] m_pMyServant = null;      // 我的角色
	IcRole[] m_pTarServant = null;      // 对方角色

    void ClearEvent()
    {
        m_pMyServant = new IcRole[CRoleDefine.MAX_ZHEN_SERVANT];
        m_pTarServant = new IcRole[CRoleDefine.MAX_ZHEN_SERVANT];

        for (int i = 0; i < CRoleDefine.MAX_ZHEN_SERVANT; i++)
        {
            if (m_myServantObj[i] != null)
            {
                m_myServantObj[i].gameObject.SetActive(false);
                m_myServantObj[i].SetClickEffect(false);
            }
            if (m_tarServantObj[i] != null)
            {
                m_tarServantObj[i].gameObject.SetActive(false);
                m_tarServantObj[i].SetClickEffect(false);
            }
        }
    }

	public void OnResurreEvent()
	{
        ClearEvent();

        if (CGameWorld.GetMap().IsChallengeMap())
        {
            m_pMyServant = CGameWorld.GetChallengeManage().m_pMyServant;
            m_pTarServant = CGameWorld.GetChallengeManage().m_pTarServant;
        }
        else
        {
            m_pMyServant = CGameWorld.MainRole().m_zhenServants;
            m_pTarServant = CGameWorld.GetArenaDataManage().m_pTarServant;
        }

        for (int i = 0; i < CRoleDefine.MAX_ZHEN_SERVANT; i++)
        {
            if(m_pMyServant[i] != null && m_myServantObj[i] != null)
			{
                m_myServantObj[i].gameObject.SetActive(true);
                m_myServantObj[i].ResurreEvent(m_pMyServant[i]);
			}

            if (m_pTarServant[i] != null && m_tarServantObj[i] != null)
            {
                m_tarServantObj[i].gameObject.SetActive(true);
                m_tarServantObj[i].ResurreEvent(m_pTarServant[i]);
            }
        }
	}
	
#endregion

	bool m_isEntityLoaded = false;
	void Update()
	{
		UpdateRoleInfo();
		
		if(CGameWorld.GetMap()!= null)
		{
			if(CGameWorld.GetMap().IsLoaded())
			{
				if(!m_isEntityLoaded)
					m_isEntityLoaded = true;
			}
		}
	}
	
	//主将副将不互换
	public IcRole CurrentRole {get;set;}
	
	bool m_isClick = false;
	public bool IsClick(IcRole role)
	{
		if(role.GetTypeId() == CGameWorld.FightRole().GetTypeId())
			return !m_isClick;
		return m_isClick;
	}
	
#region 界面重新布局 新做
    [SerializeField] IcFightUIServant[] m_myServantObj = new IcFightUIServant[CRoleDefine.MAX_ZHEN_SERVANT];		//主角的3个副将
    bool[] m_pServantAlive = new bool[CRoleDefine.MAX_ZHEN_SERVANT];

    [SerializeField] IcFightUIServant[] m_tarServantObj = new IcFightUIServant[CRoleDefine.MAX_ZHEN_SERVANT];       // 对方
    bool[] m_pTarServantAlive = new bool[CRoleDefine.MAX_ZHEN_SERVANT];
	public void TriggerServantButton(int selIndex)
	{
		if(selIndex < 0 || selIndex > CRoleDefine.MAX_ZHEN_SERVANT -1)
			return;
		
		UpdateRoleInfo();
        UpdateRole(selIndex, m_myServantObj[selIndex].gameObject);
	}
	
	void SetFightServantCD(int selIndex)
	{
		if(selIndex < 0 || selIndex > CRoleDefine.MAX_ZHEN_SERVANT -1)
			return;

        if (m_myServantObj[selIndex] != null)
            m_myServantObj[selIndex].IsShowcD();
	}

    void ClickHeroForSwitch(int servantIndex)
    {
        for (int i = 0; i < CRoleDefine.MAX_ZHEN_SERVANT; ++i)
            m_myServantObj[i].SetClickEffect(i == servantIndex);
    }
	
	const int FIRST_ROLE_INDEX = 0;
	int m_selectIndex = FIRST_ROLE_INDEX;
    void UpdateRoleInfo()
    {
        IcRole pRole = null;
        for(int i = 0; i < CRoleDefine.MAX_ZHEN_SERVANT; i++)
        {
            // 我方
            pRole = m_pMyServant[i];
            if (pRole != null)
            {
                if (pRole.IsAlive())
                {
					m_pServantAlive[i] = true;
                    m_myServantObj[i].UpdateInfo(i, pRole, false);
                }
                else
                {
                    if (m_pServantAlive[i])
                    {
                        m_pServantAlive[i] = false;
                        m_myServantObj[i].UpdateInfo(i, pRole, true);

                        if (m_selectIndex != FIRST_ROLE_INDEX && m_selectIndex == i)
                            UpdateRole(FIRST_ROLE_INDEX, m_myServantObj[FIRST_ROLE_INDEX].gameObject);
                    }
                }
            }

            // 对方
            pRole = m_pTarServant[i];
            if (pRole != null)
            {
                if (pRole.IsAlive())
                {
                    m_tarServantObj[i].UpdateInfo(i, pRole, false);
                }
                else
                {
                    if (m_pTarServantAlive[i])
                    {
                        m_pTarServantAlive[i] = false;
                        m_tarServantObj[i].UpdateInfo(i, pRole, true);
                    }
                }
            }
		}
	}
	
	void UpdateRole(int fightServantIndex, GameObject go)
	{	
		if(m_selectIndex == fightServantIndex)
			return;
		
		m_selectIndex = fightServantIndex;
		
		IcRole role = CGameWorld.MainRole().GetFightServant(m_selectIndex);
		
		if(role == null || !role.IsAlive())
			return;
		
		CurrentRole = role;

        ClickHeroForSwitch(fightServantIndex);
		SetFightServantCD(fightServantIndex);
		if(CGameWorld.IsGameOnline() && CGameWorld.GetMap() != null && !CGameWorld.GetMap().IsClientDungeonMap())
		{
			CGameWorld.GetRoleManager().SendChangeFightServant(CurrentRole.m_iDataId);
		}
		else 
		{
			IcRole oldFightRole = CGameWorld.FightRole();
            if (IsNormalMap)
            {
                CurrentRole.SetAutoFight(false);
                CurrentRole.ClearFollowTarget();
            }

			if(CGameWorld.GetAutoSystem().GetData(oldFightRole) != null)
				CGameWorld.GetAutoSystem().SetCurrentServant(CurrentRole);

            bool bDisableOld = !IsNormalMap;
            CGameWorld.SetFightRole(CurrentRole, bDisableOld);
            if (IsNormalMap)
            {
                oldFightRole.SetAutoFight(true);
                oldFightRole.SetFollowTarget(CGameWorld.FightRole());
            }
		}
        CGameWorld.GetGameHandler().m_bSwitchHeroByUI = true;
	}
	
#endregion
	
#region 竞技UI
	// 3\2\1\ fight
	[SerializeField] GameObject m_tipParent;
	
	[SerializeField] UISprite m_tip;
	bool TipEnable{ set{if(m_tip != null) m_tip.enabled = value;} }
	string TipStr{ set{if(m_tip != null) {m_tip.spriteName = value;  m_tip.MakePixelPerfect();} } }
	
	public void SetPvpPreparing(float time)
	{
		CGameWorld.GetCoroutineManager().StartCoroutine(PvpPrepareCorount(time));
	}
	
	IEnumerator PvpPrepareCorount(float time)
	{
		TipEnable = true;

        m_timeText = CUtil.GetFromTimeNum((int)IcArenaDataManage.LOSE_TIME);
		CountDown = m_timeText;
        
		float souTime = 0;
		float desTime = time;
		while(souTime <= desTime)
		{
			TipStr = string.Format("arena_{0}", Math.Floor(desTime - souTime));
			if(m_tipParent != null && m_tip != null)
			{
				iTween.ScaleFrom(m_tipParent, iTween.Hash("scale", new Vector3(4, 4, 1), "time", 0.2f, "easetype", iTween.EaseType.linear));
			}
			
			souTime += 1.0f;
			yield return new  WaitForSeconds(1.0f);
		}
		
		TipEnable = false;
	}
	
	void OnAlphaUpdate(float val)
	{
		if(m_tip != null)
			m_tip.alpha = val;
	}
	
	// 倒计时
	[SerializeField] UILabel m_countDownText;
	string CountDown{ set{ if(null != m_countDownText) m_countDownText.text = value; } }
	string m_timeText = string.Empty;
	float m_countDown = 0;	//倒计时	
	public void SetCountDown(float time)
	{
        m_isStop = false;
		CGameWorld.GetCoroutineManager().StartCoroutine(CountDownCorount(time));
	}
	
	bool m_isStop = false;
	IEnumerator CountDownCorount(float time)
	{
		m_countDown = time;
		
		while(m_countDown > 0  && !m_isStop)
		{
			yield return null;
			
			//m_timeText = IcCommonUIManager.Instance.GetConverCountDown((int)m_countDown, 1);
            m_timeText = CUtil.GetFromTimeNum((int)m_countDown);
			m_countDown -= Time.deltaTime;
			
			CountDown = m_timeText;
		}
		
		if(m_countDown <= 0)
		{
			m_timeText = "00:00";
			CountDown = m_timeText;
		}
	}
	
	// 停止
	public void StopCorout()
	{
		StopAllCoroutines();
		m_isStop = true;
	}

    void ReturnToCity()
    {
        CGameWorld.GetSceneManager().ReturnToCity();
    }
#endregion

    #region  单挑方面逻辑处理
    public void SetFigntRole()
    {
        int iNowFightIndex = CGameWorld.GetChallengeManage().m_iNowFightIndex;
        for (int i = 0; i < CRoleDefine.MAX_ZHEN_SERVANT; ++i)
        {
            if (null != m_myServantObj[i])
                m_myServantObj[i].SetClickEffect(i == iNowFightIndex);

            if (null != m_tarServantObj[i])
                m_tarServantObj[i].SetClickEffect(i == iNowFightIndex);
        }
    }


    [SerializeField] GameObject m_challengeEnd = null;
    [SerializeField] GameObject m_returnCity = null;
    [SerializeField] UILabel m_lableShengDian = null;
    [SerializeField] UILabel m_lableShengWang = null;
    [SerializeField] GameObject m_winObj = null;
    [SerializeField] GameObject m_failObj = null;
    [SerializeField] IcChallengeServant[] m_servants = new IcChallengeServant[CRoleDefine.CONST_CHALLENGE_ROLE_NUM];

    // 刷新选择界面
    public void UpdateChallengeInfo(int iAddShengWang, int iAddShengDian)
    {
        ClearServant();

        m_challengeEnd.SetActive(true);
        m_lableShengWang.text = string.Format(Language.Get("FormatGetShengWang"), iAddShengWang); //获得的声望：{0}
        m_lableShengDian.text = string.Format(Language.Get("FormatGetShengDian"), iAddShengDian); //获得的胜点：{0}

        IcChallengeDataMange pManage = CGameWorld.GetChallengeManage();
        if (null == pManage)
            return;

        if (pManage.GetFightWinNum() > 1)
            m_winObj.SetActive(true);
        else
            m_failObj.SetActive(true);

        for (int i = 0; i < CRoleDefine.CONST_CHALLENGE_ROLE_NUM; ++i)
        {
            m_servants[i].UpdateFightEndInfo(pManage.m_pMyServant[i], pManage.m_pTarServant[i], pManage.IsFightWin(i));
        }
    }

    void ClearServant()
    {
        m_lableShengDian.text = string.Empty;
        m_lableShengWang.text = string.Empty;
        m_winObj.SetActive(false);
        m_failObj.SetActive(false);
    }

    void ReturnCity(GameObject go)
    {
        CGameWorld.GetChallengeManage().ClearAll();
        CGameWorld.GetSceneManager().ReturnToCity();
    }
    #endregion
}
