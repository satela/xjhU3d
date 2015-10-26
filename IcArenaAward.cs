using UnityEngine;
using System.Collections;

public class IcArenaAward : MonoBehaviour 
{
    [SerializeField] UILabel m_rank = null;             // 名次
    [SerializeField] GameObject m_gem = null;           // 名字
    [SerializeField] GameObject m_money = null;         // 等级
    [SerializeField] GameObject m_rongyu = null;        // 荣誉

    [SerializeField] GameObject m_slots = null;
    [SerializeField] GameObject m_templateSlot = null;
    public string StringRank{get;set;}
    public void UpdateInfo(stPvpAwardInfo pData, bool bMy = false)
    {
        Clear();
        if (null == pData)
            return;

        if (pData.m_iRank.Min == pData.m_iRank.Max)
            StringRank = string.Format(Language.Get("FormatPvpRank"), pData.m_iRank.Min);//"第{0}名"
        else if (pData.m_iRank.Max < 1)
            StringRank = string.Format(Language.Get("FormatPvpRankOut"), pData.m_iRank.Min);//"第{0}名以外(包括)"
        else
            StringRank = string.Format(Language.Get("FormatPvpRankBetween"), pData.m_iRank.Min, pData.m_iRank.Max);//"第{0}到第{1}名"

        if (!bMy)       // 胜利
            m_rank.text = StringRank;

        int iBeginX = -240;
        UILabel pLable = null;
        if (pData.m_iVipMoney > 0)
        {
            m_gem.SetActive(true);
            pLable = m_gem.GetComponentInChildren<UILabel>();
            if (null != pLable)
                pLable.text = pData.m_iVipMoney.ToString("#,###");

            iBeginX += 120;
        }
        if (pData.m_iMoney > 0)
        {
            m_money.SetActive(true);
            pLable = m_money.GetComponentInChildren<UILabel>();
            if (null != pLable)
                pLable.text = pData.m_iMoney.ToString("#,###");

            m_money.transform.localPosition = new Vector3(iBeginX, 0, 0);
            iBeginX += 120;
        }
        if (pData.m_iPvpMoney > 0)
        {
            m_rongyu.SetActive(true);
            pLable = m_rongyu.GetComponentInChildren<UILabel>();
            if (null != pLable)
                pLable.text = pData.m_iPvpMoney.ToString("#,###");

            m_rongyu.transform.localPosition = new Vector3(iBeginX, 0, 0);

            iBeginX += 130;
        }

        m_slots.transform.localPosition = new Vector3(iBeginX, 0, 0);
        iBeginX = 0;
        stItemInfo pInfo = null;
        for (int i = 0; i < CItemDefine.MAX_PVP_AWARD_ITEM_NUM; ++i )
        {
            pInfo = pData.m_Item[i];
            if (pInfo.m_iItemId > 0 && pInfo.m_iItemNum > 0)
            {
                CIsItem pItem = CGameWorld.GetItemManager().Search(pInfo.m_iItemId);
                if (null != pItem)
                {
                    GameObject go = NGUITools.AddChild(m_slots, m_templateSlot);
                    if (null != go)
                    {
                        go.transform.localPosition = new Vector3(iBeginX, 0, 0);
                        IcSlot pSlot = go.GetComponent<IcSlot>();
                        if (null != pSlot)
                        {
                            pSlot.gameObject.SetActive(true);
                            pSlot.ServedObj = pItem;
                            pSlot.Num = pInfo.m_iItemNum;

                            iBeginX += 130;
                        }
                    }
                }
            }
        }
    }

    void Clear()
    {
        m_gem.SetActive(false);
        m_money.SetActive(false);
        m_rongyu.SetActive(false);
        m_rank.text = "";
        CUtil.DeleteAllChildren(m_slots);
    }
}
