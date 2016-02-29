using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.GZip;
using System;

public class CreateAssetBundle : Editor
{
    public static List<string> nameArray = new List<string>();
    [MenuItem("BuildAssetbundle/BuildAll")]
    static void Build()
    {

        GetObjectNameToArray<string>("TextInfo/txt", "pattern");

        GetObjectNameToArray<string>("RPG/actors", "pattern");

        GetObjectNameToArray<string>("RPG/Effect", "pattern");


        AssetBundleBuild[] buildMap = new AssetBundleBuild[nameArray.Count];

        for (int i = 0; i < buildMap.Length; i++)
        {
            string ss = nameArray[i];
            ss = ss.Replace("/", "_");
            ss = ss.Replace("\\", "_");
            ss = ss.Replace(".", "_");

            buildMap[i].assetBundleName = ss;//打包的资源包名称 随便命名  
            string[] resourcesAssets = new string[1];//此资源包下面有多少文件  
            resourcesAssets[0] = nameArray[i];
            // resourcesAssets[1] = "Assets/Resources/Obj/Sphere.prefab";  
            buildMap[i].assetNames = resourcesAssets;
        }

        BuildPipeline.BuildAssetBundles(Application.dataPath + "/../Assetbundle", buildMap, BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.DeterministicAssetBundle, EditorUserBuildSettings.activeBuildTarget);

        createToZipFile();
        // BuildPipeline.BuildAssetBundles(Application.dataPath + "/../Assetbundle", BuildAssetBundleOptions.UncompressedAssetBundle);  
    }

    static void GetObjectNameToArray<T>(string path, string pattern)
    {
        string objPath = Application.dataPath + "/" + path;
        string[] directoryEntries;
        try
        {
            //返回指定的目录中文件和子目录的名称的数组或空数组  
            directoryEntries = System.IO.Directory.GetFileSystemEntries(objPath);

            for (int i = 0; i < directoryEntries.Length; i++)
            {
                string p = directoryEntries[i];
                //得到要求目录下的文件或者文件夹（一级的）//  
                string[] tempPaths = StringExtention.SplitWithString(p, "/Assets/" + path + "\\");

                //tempPaths 分割后的不可能为空,只要directoryEntries不为空//  
                if (tempPaths[1].EndsWith(".meta") || tempPaths[1].EndsWith(".cs"))
                    continue;
                string[] pathSplit = StringExtention.SplitWithString(tempPaths[1], ".");
                //文件  
                if (pathSplit.Length > 1)
                {
                    string[] assetpath = StringExtention.SplitWithString(p, Application.dataPath + "/");
                    string ss = "assets/" + assetpath[1];

                    nameArray.Add(ss);
                }
                //遍历子目录下 递归吧！  
                else
                {
                    GetObjectNameToArray<T>(path + "/" + pathSplit[0], "pattern");
                    continue;
                }
            }
        }
        catch (System.IO.DirectoryNotFoundException)
        {
            Debug.Log("The path encapsulated in the " + objPath + "Directory object does not exist.");
        }
    }
    public class StringExtention
    {

        public static string[] SplitWithString(string sourceString, string splitString)
        {
            string tempSourceString = sourceString;
            List<string> arrayList = new List<string>();
            string s = string.Empty;
            while (sourceString.IndexOf(splitString) > -1)  //分割  
            {
                s = sourceString.Substring(0, sourceString.IndexOf(splitString));
                sourceString = sourceString.Substring(sourceString.IndexOf(splitString) + splitString.Length);
                arrayList.Add(s);
            }
            arrayList.Add(sourceString);
            return arrayList.ToArray();
        }
    }

    [MenuItem("BuildAssetbundle/createFileHasCode")]
    static void createHasTable()
    {
        string[] directoryEntries;
        AssetBundle manifestBundle = AssetBundle.CreateFromFile(Application.dataPath
                                                                 + "/../Assetbundle/Assetbundle");
        if (manifestBundle != null)
        {
            AssetBundleManifest manifest = (AssetBundleManifest)manifestBundle.LoadAsset("AssetBundleManifest");
            string[] allbundle = manifest.GetAllAssetBundles();
            string fileinfo = "";
            foreach (string assetname in allbundle)
            {
                Hash128 has = manifest.GetAssetBundleHash(assetname);
                fileinfo += assetname + "," + has.ToString() + "\n";
                // Debug.Log(has.GetHashCode().ToString());  

            }

            FileStream fs = new FileStream(Application.dataPath + "/../Assetbundle/assetBundleVersion.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            //开始写入  
            sw.Write(fileinfo);
            //清空缓冲区  
            sw.Flush();
            //关闭流  
            sw.Close();
            fs.Close();

        }

    }

    private static void createToZipFile()
    {
        string objPath = Application.dataPath + "/../Assetbundle";
        string outPath = Application.dataPath + "/../AssetbundleZip/";

        string[] directoryEntries;
        try
        {
            //返回指定的目录中文件和子目录的名称的数组或空数组  
            directoryEntries = System.IO.Directory.GetFileSystemEntries(objPath);

            for (int i = 0; i < directoryEntries.Length; i++)
            {
                string p = directoryEntries[i];
                //得到要求目录下的文件或者文件夹（一级的）//  
                string[] tempPaths = StringExtention.SplitWithString(p, objPath + "\\");

                if (tempPaths[1] == "Assetbundle.manifest" || tempPaths[1].Split('.').Length < 2)
                 CreateZipFile(p, outPath + tempPaths[1] + ".zip");
            }
               
        }
        catch (Exception e)
        {
            Debug.Log("compress error:" + e.Message);
        }
    }
    private static void CreateZipFile(string filesPath, string zipFilePath)
    {

        if (!Directory.Exists(filesPath))
        {
           // Console.WriteLine("Cannot find directory '{0}'", filesPath);
           // return;
        }

        try
        {
            string[] filenames = Directory.GetFiles(filesPath);
            using (ZipOutputStream s = new ZipOutputStream(File.Create(zipFilePath)))
            {

                s.SetLevel(9); // 压缩级别 0-9
                //s.Password = "123"; //Zip压缩文件密码
                byte[] buffer = new byte[4096]; //缓冲区大小
                foreach (string file in filenames)
                {
                    ZipEntry entry = new ZipEntry(Path.GetFileName(file));
                    entry.DateTime = DateTime.Now;
                    s.PutNextEntry(entry);
                    using (FileStream fs = File.OpenRead(file))
                    {
                        int sourceBytes;
                        do
                        {
                            sourceBytes = fs.Read(buffer, 0, buffer.Length);
                            s.Write(buffer, 0, sourceBytes);
                        } while (sourceBytes > 0);
                    }
                }
                s.Finish();
                s.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception during processing {0}", ex);
        }
    }
}