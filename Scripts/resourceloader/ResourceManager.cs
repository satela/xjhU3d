using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ResourceManager {

    public static AssetBundle manifestBundle;

    public static Dictionary<string, UnityEngine.Object> tempResourceDic = new Dictionary<string, Object>();
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static T loadAsset<T>(string path) where T : UnityEngine.Object
    {
        if (CGameWorld.instance.isLoadAssetBundle)
        {
            if (tempResourceDic.ContainsKey(path))
                return tempResourceDic[path] as T;

            string fullpath = path;
            T asset = default(T);

            string[] filepaht = path.Split('.')[0].Split('/');
            string filename = filepaht[filepaht.Length - 1];

            path = path.ToLower();
            path = path.Replace('/', '_');
            path = path.Replace('.', '_');

            if (manifestBundle == null)
                manifestBundle = AssetBundle.CreateFromFile(Application.persistentDataPath
                                                                     + "/Assetbundle");
            if (manifestBundle != null)
            {
                AssetBundleManifest manifest = (AssetBundleManifest)manifestBundle.LoadAsset("AssetBundleManifest");

                //获取依赖文件列表;
                string[] cubedepends = manifest.GetAllDependencies(path);
                AssetBundle[] dependsAssetbundle = new AssetBundle[cubedepends.Length];

                for (int index = 0; index < cubedepends.Length; index++)
                {
                    //加载所有的依赖文件;
                    dependsAssetbundle[index] = AssetBundle.CreateFromFile(Application.persistentDataPath
                                                                         + "/"
                                                                         + cubedepends[index]);
                    if (dependsAssetbundle[index] == null)
                    {
                        Debug.Log("error:" + cubedepends[index]);
                    }

                }

                //加载我们需要的文件;"
                AssetBundle fileBundle = AssetBundle.CreateFromFile(Application.persistentDataPath
                                                                  + "/" + path);
                asset = fileBundle.LoadAsset(filename) as T;
                fileBundle.Unload(false);

                for (int index = 0; index < cubedepends.Length; index++)
                {
                    if (dependsAssetbundle[index] != null)
                    dependsAssetbundle[index].Unload(false);
                }
                tempResourceDic.Add(fullpath, asset);
                return asset;
            }
        }
        else
        {
#if UNITY_EDITOR
            return UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(T)) as T;
#endif
        }

         return null;
    }
}
