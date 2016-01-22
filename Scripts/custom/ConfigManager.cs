using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConfigManager {

    private static ConfigManager _instance;

    public static ConfigManager intance
    {
        get { 
            
            if (_instance != null) return _instance;
            else
            {
                _instance = new ConfigManager();
                return _instance;
            }
        
        }
    }

    public void init()
    {
        initSkillConfig();
        initBuffConfig();
    }

    #region 技能基本配置

    public Dictionary<int, DSkillDefaultData> skillDefaultDic;
    void initSkillConfig()
    {
        if (skillDefaultDic == null)
            skillDefaultDic = new Dictionary<int, DSkillDefaultData>();
        skillDefaultDic.Clear();

        TextAsset txtasset = Resources.LoadAssetAtPath("Assets/TextInfo/txt/Skill.txt", typeof(TextAsset)) as TextAsset;

        string text = txtasset.text;
        string[] strArray = text.Split('\n');

        DSkillDefaultData skilldata;
        for (int i = 1; i < strArray.Length;i++ )
        {
            if(!string.IsNullOrEmpty(strArray[i]))
            {
                skilldata = new DSkillDefaultData();
                skilldata.paresData(strArray[i]);
                skillDefaultDic.Add(skilldata.id, skilldata);
            }           

        }
    }

    #endregion

    #region buff基本配置

    public Dictionary<int, DBuffData> basebuffDataDic;
    void initBuffConfig()
    {
        if (basebuffDataDic == null)
            basebuffDataDic = new Dictionary<int, DBuffData>();
        basebuffDataDic.Clear();

        TextAsset txtasset = Resources.LoadAssetAtPath("Assets/TextInfo/txt/SkillBuff.txt", typeof(TextAsset)) as TextAsset;

        string text = txtasset.text;
        string[] strArray = text.Split('\n');

        DBuffData buffdata;
        for (int i = 1; i < strArray.Length; i++)
        {
            if (!string.IsNullOrEmpty(strArray[i]))
            {
                buffdata = new DBuffData();
                buffdata.paresData(strArray[i]);
                basebuffDataDic.Add(buffdata.buffid, buffdata);
            }

        }
    }

    #endregion
}
