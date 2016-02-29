using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.GZip;

public static class IcZipPack
{
    public static void Pack(string srcFile, string dstZip)
    {
        if (srcFile[srcFile.Length - 1] != Path.DirectorySeparatorChar)
            srcFile += Path.DirectorySeparatorChar;
        ZipOutputStream s = new ZipOutputStream(File.Create(dstZip));
        s.SetLevel(6); // 0 - store only to 9 - means best compression
        zip(srcFile, s, srcFile);
        s.Finish();
        s.Close();
    }

    public static string Unpack(string TargetFile, string fileDir)
    {
        string rootFile = " ";
        try
        {
            //读取压缩文件(zip文件)，准备解压缩
            ZipInputStream s = new ZipInputStream(File.OpenRead(TargetFile.Trim()));
            ZipEntry theEntry;
            string path = fileDir;
            //解压出来的文件保存的路径

            string rootDir = " ";
            //根目录下的第一个子文件夹的名称
            while ((theEntry = s.GetNextEntry()) != null)
            {
				string entryName = theEntry.Name.Replace("\\", "/");
				rootDir = Path.GetDirectoryName(entryName);
				
				//得到根目录下的第一级子文件夹的名称
				int index = rootDir.IndexOf('/');
				if (index >= 0)
				{
					rootDir = rootDir.Substring(0, index + 1);
				}
				string dir = Path.GetDirectoryName(entryName);
                //根目录下的第一级子文件夹的下的文件夹的名称
				string fileName = Path.GetFileName(entryName);
				//根目录下的文件名称
                if (dir != " ")
                //创建根目录下的子文件夹,不限制级别
                {
                	path = string.Format("{0}/{1}", fileDir, dir);
                    if (!Directory.Exists(path))
                    {
                        //在指定的路径创建文件夹
                        Directory.CreateDirectory(path);
                    }
                }
                else if (dir == " " && fileName != "")
                //根目录下的文件
                {
                    path = fileDir;
                    rootFile = fileName;
                }
                else if (dir != " " && fileName != "")
                //根目录下的第一级子文件夹下的文件
                {
					if (dir.IndexOf('/') > 0)
					//指定文件保存的路径
                    {
                        path = string.Format("{0}/{1}", fileDir, dir);
                    }
                }

                if (dir == rootDir)
                //判断是不是需要保存在根目录下的文件
                {
                    path = string.Format("{0}/{1}", fileDir, rootDir);
                }

                //以下为解压缩zip文件的基本步骤
                //基本思路就是遍历压缩文件里的所有文件，创建一个相同的文件。
                if (fileName != String.Empty)
                {
					string filePath = string.Format("{0}/{1}", path, fileName);
                    FileStream streamWriter = File.Create(filePath);

                    int size = 2048;
                    byte[] data = new byte[2048];
                    while (true)
                    {
                        size = s.Read(data, 0, data.Length);
                        if (size > 0)
                        {
                            streamWriter.Write(data, 0, size);
                        }
                        else
                        {
                            break;
                        }
                    }

                    streamWriter.Close();
                }
            }
            s.Close();

            return rootFile;
        }
        catch (Exception ex)
        {
            return "1; " + ex.Message;
        }
    }
	
	public static string Unpack(byte[] srcBytes, string fileDir)
	{
        string err = null;
		string rootFile = " ";
        try
        {
			MemoryStream ms = new MemoryStream(srcBytes);
            //读取压缩文件(zip文件)，准备解压缩
            ZipInputStream s = new ZipInputStream(ms);
            ZipEntry theEntry;
            string path = fileDir;
            //解压出来的文件保存的路径

            string rootDir = " ";
            //根目录下的第一个子文件夹的名称
            while ((theEntry = s.GetNextEntry()) != null)
            {
				string entryName = theEntry.Name.Replace("\\", "/");
				rootDir = Path.GetDirectoryName(entryName);

                //得到根目录下的第一级子文件夹的名称
				int index = rootDir.IndexOf('/');
				if (index >= 0)
				{
					rootDir = rootDir.Substring(0, index + 1);
				}
				string dir = Path.GetDirectoryName(entryName);
                //根目录下的第一级子文件夹的下的文件夹的名称
				string fileName = Path.GetFileName(entryName);
				//根目录下的文件名称
                if (dir != " ")
                //创建根目录下的子文件夹,不限制级别
                {
                	path = string.Format("{0}/{1}", fileDir, dir);
                    if (!Directory.Exists(path))
                    {
                        //在指定的路径创建文件夹
                        Directory.CreateDirectory(path);
                    }
                }
                else if (dir == " " && fileName != "")
                //根目录下的文件
                {
                    path = fileDir;
                    rootFile = fileName;
                }
                else if (dir != " " && fileName != "")
                //根目录下的第一级子文件夹下的文件
                {
					if (dir.IndexOf('/') > 0)
					//指定文件保存的路径
                    {
                        path = string.Format("{0}/{1}", fileDir, dir);
                    }
                }

                if (dir == rootDir)
                //判断是不是需要保存在根目录下的文件
                {
                    path = string.Format("{0}/{1}", fileDir, rootDir);
                }

                //以下为解压缩zip文件的基本步骤
                //基本思路就是遍历压缩文件里的所有文件，创建一个相同的文件。
                if (fileName != String.Empty)
                {
					string filePath = string.Format("{0}/{1}", path, fileName);
                    FileStream streamWriter = File.Create(filePath);

                    int size = 2048;
                    byte[] data = new byte[2048];
                    while (true)
                    {
                        size = s.Read(data, 0, data.Length);
                        if (size > 0)
                        {
                            streamWriter.Write(data, 0, size);
                        }
                        else
                        {
                            break;
                        }
                    }

                    streamWriter.Close();
                }
            }
            s.Close();

            //return rootFile;
            return err;
        }
        catch (Exception ex)
        {
            //return "1; " + ex.Message;
            err = ex.Message;
            return err;
        }
	}

    private static void zip(string strFile, ZipOutputStream s, string staticFile)
    {
        if (strFile[strFile.Length - 1] != Path.DirectorySeparatorChar) strFile += Path.DirectorySeparatorChar;
        Crc32 crc = new Crc32();
        string[] filenames = Directory.GetFileSystemEntries(strFile);
        foreach (string file in filenames)
        {
            if (Directory.Exists(file))
            {
                zip(file, s, staticFile);
            }

            else // 否则直接压缩文件
            {
                //打开压缩文件
                FileStream fs = File.OpenRead(file);

                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                string tempfile = file.Substring(staticFile.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                ZipEntry entry = new ZipEntry(tempfile);

                entry.DateTime = DateTime.Now;
                entry.Size = fs.Length;
                fs.Close();
                crc.Reset();
                crc.Update(buffer);
                entry.Crc = crc.Value;
                s.PutNextEntry(entry);

                s.Write(buffer, 0, buffer.Length);
            }
        }
    }

    public static void CopyDir(string src, string dst)
    {
        try
        {
            // 检查目标目录是否以目录分割字符结束如果不是则添加
            if(dst[dst.Length - 1] != System.IO.Path.DirectorySeparatorChar)
            {
                dst += System.IO.Path.DirectorySeparatorChar;
            }

            // 判断目标目录是否存在如果不存在则新建
            if (!System.IO.Directory.Exists(dst))
            {
                System.IO.Directory.CreateDirectory(dst);
            }

            // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
            // 如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
            // string[] fileList = Directory.GetFiles（srcPath）；
            string[] fileList = System.IO.Directory.GetFileSystemEntries(src);
            // 遍历所有的文件和目录
            foreach (string file in fileList)
            {
            // 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                if (System.IO.Directory.Exists(file))
                {
					string dirName = System.IO.Path.GetFileName(file);
					if(dirName != ".svn")
						CopyDir(file, dst + dirName);
				}
				// 否则直接Copy文件
                else
                {
                    System.IO.File.Copy(file, dst + System.IO.Path.GetFileName(file), true);
                }
            }
        }
        catch (Exception e)
        {
            throw;
        }
    }
}
