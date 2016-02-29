using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class ResUpdate : MonoBehaviour
{
    public static readonly string VERSION_FILE = "assetBundleVersion.txt";
    public static readonly string AASETBUNDLE_FILE = "Assetbundle";
    public static readonly string AASETBUNDLE_MANIFEST_FILE = "Assetbundle.manifest";


    public static string LOCAL_RES_URL;
    public static readonly string SERVER_RES_URL = "http://192.168.1.13/dy_u3d_5guo/res/";//"file:///C:/Res/";
    public static  string LOCAL_RES_PATH;

    private Dictionary<string,string> LocalResVersion;
    private Dictionary<string, string> ServerResVersion;
    private List<string> NeedDownFiles;
    private bool NeedUpdateLocalVersionFile = false;

    public UISlider downloadSlider;
    private int allNeedUpdateFile = 0;

    public void Awake()
    {
        
        LOCAL_RES_URL = "file://" + (Application.persistentDataPath + "/").Replace('/','\\');
        LOCAL_RES_PATH = Application.persistentDataPath + "/";
    }


    void Start()
    {
        //初始化    
        LocalResVersion = new Dictionary<string, string>();
        ServerResVersion = new Dictionary<string, string>();
        NeedDownFiles = new List<string>();

        //加载本地version配置    
        StartCoroutine(DownLoad(LOCAL_RES_URL + VERSION_FILE, delegate(WWW localVersion)
        {
            //保存本地的version    
            ParseVersionFile(localVersion.text, LocalResVersion);
            //加载服务端version配置    
            StartCoroutine(this.DownLoad(SERVER_RES_URL + VERSION_FILE, delegate(WWW serverVersion)
            {
                //保存服务端version    
                ParseVersionFile(serverVersion.text, ServerResVersion);
                //计算出需要重新加载的资源    
                CompareVersion();

                allNeedUpdateFile = NeedDownFiles.Count;
                //加载需要更新的资源    
                DownLoadRes();
            }));

        }));
    }

    //依次加载需要更新的资源    
    private void DownLoadRes()
    {
        if (NeedDownFiles.Count == 0)
        {
            UpdateLocalVersionFile();
            this.gameObject.SetActive(false);
            FightRoleManager._instance.initConfiguretion();
            return;
        }

        string file = NeedDownFiles[0];
        NeedDownFiles.RemoveAt(0);

        if (downloadSlider != null)
            downloadSlider.value = (float)(allNeedUpdateFile - NeedDownFiles.Count) / allNeedUpdateFile;
        StartCoroutine(this.DownLoad(SERVER_RES_URL + file + ".zip", delegate(WWW w)
        {
            //将下载的资源替换本地就的资源    
            //ReplaceLocalRes(file, w.bytes);
            string errMsg = IcZipPack.Unpack(w.bytes, LOCAL_RES_PATH);
            if (errMsg != null)
            {
                Debug.Log("un pack error");
            }
            else
            {
                Debug.Log("un pack sucess");
            }
            DownLoadRes();
        }));
    }

    private void ReplaceLocalRes(string fileName, byte[] data)
    {
        string filePath = LOCAL_RES_PATH + fileName;
        FileStream stream = new FileStream(LOCAL_RES_PATH + fileName, FileMode.Create);
        stream.Write(data, 0, data.Length);
        stream.Flush();
        stream.Close();
    }

    //显示资源    
    private IEnumerator Show()
    {
        WWW asset = new WWW(LOCAL_RES_URL + "cube.assetbundle");
        yield return asset;
        AssetBundle bundle = asset.assetBundle;
        //Instantiate(bundle.Load("Cube"));
        bundle.Unload(false);
    }

    //更新本地的version配置    
    private void UpdateLocalVersionFile()
    {
        if (NeedUpdateLocalVersionFile)
        {
            StringBuilder versions = new StringBuilder();
            foreach (var item in ServerResVersion)
            {
                versions.Append(item.Key).Append(",").Append(item.Value).Append("\n");
            }

            FileStream stream = new FileStream(LOCAL_RES_PATH + VERSION_FILE, FileMode.Create);
            byte[] data = Encoding.UTF8.GetBytes(versions.ToString());
            stream.Write(data, 0, data.Length);
            stream.Flush();
            stream.Close();          
        }
        //加载显示对象    
       // StartCoroutine(Show());
    }

    private void CompareVersion()
    {
        foreach (var version in ServerResVersion)
        {
            string fileName = version.Key;
            string serverMd5 = version.Value;
            //新增的资源    
            if (!LocalResVersion.ContainsKey(fileName))
            {
                NeedDownFiles.Add(fileName);
            }
            else
            {
                //需要替换的资源    
                string localMd5;
                LocalResVersion.TryGetValue(fileName, out localMd5);
                if (!serverMd5.Equals(localMd5))
                {
                    NeedDownFiles.Add(fileName);
                }
            }
        }
        //本次有更新，同时更新本地的version.txt    
        NeedUpdateLocalVersionFile = true;// NeedDownFiles.Count > 0;
        NeedDownFiles.Add("skillConfig");
        if (NeedDownFiles.Count > 0)
        {
            NeedDownFiles.Add(AASETBUNDLE_FILE);
            NeedDownFiles.Add(AASETBUNDLE_MANIFEST_FILE);

        }
    }

    private void ParseVersionFile(string content, Dictionary<string,string> dict)
    {
        if (content == null || content.Length == 0)
        {
            return;
        }
        string[] items = content.Split(new char[] { '\n' });
        foreach (string item in items)
        {
            string[] info = item.Split(new char[] { ',' });
            if (info != null && info.Length == 2)
            {
                dict.Add(info[0], info[1]);
            }
        }

    }

    private IEnumerator DownLoad(string url, HandleFinishDownload finishFun)
    {
        WWW www = new WWW(url);
        yield return www;
        if (finishFun != null)
        {
            finishFun(www);
        }
        www.Dispose();
    }

    public delegate void HandleFinishDownload(WWW www);
}