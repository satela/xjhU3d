using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DefaultRoleData  {

    public float[] roleBaseAttr = new float[(int)EBaseAttr.Max]; //人物出生主属性

    public float[] roleSubAttr = new float[(int)ESubAttr.Max];//人物出生小属性
  

    public string rolename;

    public int roleId;//Role 表中 Id
    public int job;
    public int type; //主角，副将，怪物，boss

    public string modelUrl;// 模型prefab

    public List<int> skillIdList;

    public void paresData(string datastr)
    {
        string[] propArray = datastr.Split(',');

        roleId = int.Parse(propArray[0]);
        type = int.Parse(propArray[1]);
        job = int.Parse(propArray[2]);
        rolename = propArray[3];
        modelUrl = propArray[4];
        for (int i = 5; i < 12; i++)
        {
            roleBaseAttr[i - 5] = float.Parse(propArray[i]);
        }

        for (int i = 12; i < 16 ; i++)
        {
            roleSubAttr[i - 12] = float.Parse(propArray[i]);
        }

        string[] tempskill = propArray[16].Split('|');

        skillIdList = new List<int>();
        for (int i = 0; i < tempskill.Length; i++)
        {
            skillIdList.Add(int.Parse(tempskill[i]));
        }

    }
}
