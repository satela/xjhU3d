using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Excel;
using System.Data;
using System.Collections.Generic;

public class EExportExcel : MonoBehaviour {

    [MenuItem("EExportExcel/ExportAll")]
    static void Build()
    {
        string[] directoryEntries;

        directoryEntries = System.IO.Directory.GetFileSystemEntries(Application.dataPath + "/TextInfo/excel");

        for (int m = 0; m < directoryEntries.Length; m++)
        {
            if (directoryEntries[m].Substring(directoryEntries[m].Length - 4) == "xlsx")
            {
                FileStream stream = File.Open(directoryEntries[m], FileMode.Open, FileAccess.Read);
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                DataSet result = excelReader.AsDataSet();

                int columns = result.Tables[0].Columns.Count;
                int rows = result.Tables[0].Rows.Count;
                StringBuilder contentinfo = new StringBuilder();
                for (int i = 0; i < rows; i++)
                {
                    string rowstr = "";
                    for (int j = 0; j < columns; j++)
                    {
                        rowstr += result.Tables[0].Rows[i][j].ToString() + ",";
                       // Debug.Log(nvalue);
                    }
                    contentinfo.AppendLine(rowstr);
                }	


                string[] pathSplit = StringExtention.SplitWithString(directoryEntries[m],Application.dataPath + "/TextInfo/excel\\");
                string filename = pathSplit[1].Split('.')[0];

                CreateFile(Application.dataPath + "/TextInfo/txt", filename, contentinfo.ToString());
            }
            
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
    static void CreateFile(string path, string name, string info)
    {
        //文件流信息
        StreamWriter sw;
        FileInfo t = new FileInfo(path + "//" + name + ".txt");
        if (!t.Exists)
        {
            //如果此文件不存在则创建
            sw = t.CreateText();
        }
        else
        {
            //如果此文件存在则打开
            File.Delete(path + "//" + name + ".txt");
            sw = t.CreateText();
           // sw = t.AppendText();
        }
        //以行的形式写入信息
        sw.WriteLine(info);
        //关闭流
        sw.Close();
        //销毁流
        sw.Dispose();
    }  
 
}
