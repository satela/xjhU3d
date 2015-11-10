using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShortCutSkillUI : MonoBehaviour {

	// Use this for initialization

    public static ShortCutSkillUI _instance;

    public UISprite[] iconList = new UISprite[6];

    public Dictionary<GameObject,int> skillShortCuts = new Dictionary<GameObject,int>();

    public ArrayList skillInfos = new ArrayList();

    private PlayerAttack plattack;
    public void Awake()
    {
        _instance = this;
        for(int i=0;i < iconList.Length;i++)
        {
            skillShortCuts.Add(iconList[i].parent.gameObject,i);
            iconList[i].enabled = false;
            skillInfos.Add(i);
        }
    }

    void Start()
    {
        plattack = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerAttack>();
    }
    void Update()
    {
        object useInfo;
        int index = -1;
        if(Input.GetKeyDown(KeyCode.Q))
        {
            index = 0;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            index = 1;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            index = 2;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            index =3;
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            index = 4;
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            index = 5;
        }

        if (index >=0 && skillInfos[index] != null)
        {
            if(skillInfos[index] is SkillInfo)
            {
                OnUseSkill(skillInfos[index] as SkillInfo);
            }
            else if (skillInfos[index] is ObjectInfo)
            {
                if(OnUseItem(skillInfos[index] as ObjectInfo))
                {
                    iconList[index].enabled = false;
                    skillInfos[index] = null;
                }
            }
        }
    }

    private void OnUseSkill(SkillInfo info)
    {
        if ((int)info.applicableRole == (int)PlayerStatus._instance.heroType)
        {
            if (PlayerStatus._instance.useMp(info.mp))
            {
                plattack.useSkill(info);
            }
        }
        
    }

    private bool OnUseItem(ObjectInfo info)
    {
        if(PlayerStatus._instance.useItems(info.id,1))
        {
            PlayerStatus._instance.useDrugEff(info.hp, info.mp);
            return true;

        }

        return false;
    }

    public void SetShortCutSkill(GameObject skillshortcut,SkillInfo info)
    {
        int index = 0;
        if(skillShortCuts.TryGetValue(skillshortcut,out index))
        {
            iconList[index].spriteName = info.icon;
            skillInfos[index] = info;
            iconList[index].enabled = true;
        }

    }
    public void SetShortCutItem(GameObject skillshortcut, ObjectInfo info)
    {
        int index = 0;
        if (skillShortCuts.TryGetValue(skillshortcut, out index))
        {
            iconList[index].spriteName = info.icon;
            skillInfos[index] = info;
            iconList[index].enabled = true;
        }
    }
	
}
