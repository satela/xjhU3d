using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System.IO;


public class SkillConfiguration  {

    public static Dictionary<int, DSkillBaseData> skillsDic = new Dictionary<int, DSkillBaseData>();


    public static void LoadXml()
    {
        //创建xml文档
        if (!File.Exists(Application.dataPath + "/TextInfo/skillConfig.xml"))
        {
            return;
        }
        XmlDocument xml = new XmlDocument();
        XmlReaderSettings set = new XmlReaderSettings();
        set.IgnoreComments = true;//这个设置是忽略xml注释文档的影响。有时候注释会影响到xml的读取
        xml.Load(XmlReader.Create((Application.dataPath + "/TextInfo/skillConfig.xml"), set));
        //得到objects节点下的所有子节点
        XmlNodeList xmlNodeList = xml.SelectSingleNode("allskill").ChildNodes;
        DSkillBaseData skilldata;
        //遍历所有子节点
        foreach (XmlElement oneskill in xmlNodeList)
        {

            skilldata = new DSkillBaseData();
            skilldata.id = int.Parse(oneskill.GetAttribute("id"));
            skilldata.skillName = oneskill.GetAttribute("name");
            skilldata.skilltype = (ESkillType)(int.Parse(oneskill.GetAttribute("skilltype")));
            skilldata.minAttackDist = float.Parse(oneskill.GetAttribute("minAttackDist"));

            skilldata.animatorClip = (eAnimatorState)(int.Parse(oneskill.GetAttribute("animatorClip")));
            skilldata.fireEffUrl = oneskill.GetAttribute("fireEffUrl");
            skilldata.fireTime = float.Parse(oneskill.GetAttribute("fireTime"));
            skilldata.fireEffPos = (EFirePos)(int.Parse(oneskill.GetAttribute("fireEffPos")));
            skilldata.isUseMoveEff = oneskill.GetAttribute("isUseMoveEff") == "1";
            skilldata.moveEffUrl = oneskill.GetAttribute("moveEffUrl");
            skilldata.isSingleMove = oneskill.GetAttribute("isSingleMove") == "1";
            skilldata.moveBeginTime = float.Parse(oneskill.GetAttribute("moveBeginTime"));
            skilldata.isQunGong = oneskill.GetAttribute("isQunGong") == "1";
            skilldata.harmDist = float.Parse(oneskill.GetAttribute("harmDist"));
            skilldata.isNeedAppoint = oneskill.GetAttribute("isNeedAppoint") == "1";
            skilldata.explodeEffUrl = oneskill.GetAttribute("explodeEffUrl");

            if (oneskill.HasAttribute("isShakeCamera"))
                skilldata.isShakeCamera = oneskill.GetAttribute("isShakeCamera") == "1";
            if (oneskill.HasAttribute("shakeTime"))
                skilldata.shakeTime = float.Parse(oneskill.GetAttribute("shakeTime"));

            if (oneskill.HasAttribute("beatonData"))
                skilldata.parseBeatonStr(oneskill.GetAttribute("beatonData"));
           // skilldata.animatorBeatonClip = (eAnimatorState)(int.Parse(oneskill.GetAttribute("animatorBeatonClip")));
          //  skilldata.beatonEffUrl = oneskill.GetAttribute("beatonEffUrl");
          //  skilldata.beatonTime = float.Parse(oneskill.GetAttribute("beatonTime"));
          //  if (oneskill.HasAttribute("eBeatonbackFly"))
         //   skilldata.eBeatonbackFly = (EBeatonToBackFly)(int.Parse(oneskill.GetAttribute("eBeatonbackFly")));

            skillsDic.Add(skilldata.id, skilldata);
        }

    }

    public static void saveSkill(DSkillBaseData skilldata)
    {
        if (skillsDic.ContainsKey(skilldata.id))
        {
            skillsDic[skilldata.id] = skilldata;
        }
        else
            skillsDic.Add(skilldata.id, skilldata);

        CreateXML();
    }

     static void CreateXML()
    {
        string path = Application.dataPath + "/TextInfo/skillConfig.xml";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        if (!File.Exists(path))
        {
            //创建最上一层的节点。
            XmlDocument xml = new XmlDocument();
            //创建最上一层的节点。
            XmlElement root = xml.CreateElement("allskill");

            foreach (DSkillBaseData oneskill in skillsDic.Values)
            {
                XmlElement element = xml.CreateElement("skill");
                element.SetAttribute("id", oneskill.id.ToString());
                element.SetAttribute("name", oneskill.skillName);
                element.SetAttribute("minAttackDist", oneskill.minAttackDist.ToString());
                element.SetAttribute("skilltype", ((int)oneskill.skilltype).ToString());
                element.SetAttribute("animatorClip", ((int)oneskill.animatorClip).ToString());

                element.SetAttribute("fireEffUrl", oneskill.fireEffUrl);
                element.SetAttribute("fireTime", oneskill.fireTime.ToString());
                element.SetAttribute("fireEffPos", ((int)oneskill.fireEffPos).ToString());
                element.SetAttribute("isUseMoveEff", oneskill.isUseMoveEff ? "1":"0");
                element.SetAttribute("moveEffUrl", oneskill.moveEffUrl);
                element.SetAttribute("isSingleMove", oneskill.isSingleMove ? "1" : "0");
                element.SetAttribute("moveBeginTime", oneskill.moveBeginTime.ToString());
                element.SetAttribute("isQunGong", oneskill.isQunGong ? "1" : "0");
                element.SetAttribute("harmDist", oneskill.harmDist.ToString());
                element.SetAttribute("isNeedAppoint", oneskill.isNeedAppoint ? "1" : "0");
                element.SetAttribute("explodeEffUrl", oneskill.explodeEffUrl.ToString());

                element.SetAttribute("isShakeCamera", oneskill.isShakeCamera ? "1" : "0");
                element.SetAttribute("shakeTime", oneskill.shakeTime.ToString());

                element.SetAttribute("beatonData", oneskill.getBeatonStr());
                root.AppendChild(element);
            }
           

            xml.AppendChild(root);
            //最后保存文件
            xml.Save(path);
        }
    }
}
