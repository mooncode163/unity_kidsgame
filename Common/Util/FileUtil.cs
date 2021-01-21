using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;
using System.Collections.Generic;

public class FileUtil
{
    public const string JAVA_CLASS_FILEUTIL = "com.moonma.common.FileUtil";

    static public byte[] ReadDataAuto(string filePath)
    {
        byte[] data = FileUtil.ReadDataFromResources(filePath);
        if (data == null)
        {
            if (FileUtil.FileIsExistAsset(filePath))
            {
                data = FileUtil.ReadDataAsset(filePath);
            }

        }
        if (data == null)
        {
            if (FileUtil.FileIsExist(filePath))
            {
                data = FileUtil.ReadDataFromFile(filePath);
            }

        }
        return data;
    }


    static public string ReadStringAuto(string filePath)
    {
        byte[] data = ReadDataAuto(filePath);
        if (data == null)
        {
            return null;
        }
        string str = Encoding.UTF8.GetString(data);
        return str;
    }
    //filePath 为绝对路径
    static public byte[] ReadDataFromFile(string filePath)
    {

        //FileStream fs = new FileStream(filePath, FileMode.Open);
        //win10 访问 app 目录下文件需要加 FileAccess.Read 权限
        FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        if (fs != null)
        {
            int len = (int)fs.Length;
            if (len > 0)
            {
                byte[] data = new byte[len];
                fs.Read(data, 0, len);
                fs.Close();
                return data;
            }
            else
            {
                fs.Close();
            }
        }
        return null;
    }
    /* 
        ios下边的目录可以使用FIle.Read 直接读取，
        但是StreamAsseting目录文件流只能使用myStream = File.OpenRead(myPath); 来读。原因是FileMode.Open有读写权限 而StreamAsseting只有读的权限 
        所以 用FIle.Open会出错:Access to the path "/..Raw/4.page" is denied.
    */
    static public byte[] ReadDataAssetIos(string filePath)
    {
        FileStream fs = File.OpenRead(filePath);
        if (fs != null)
        {
            int len = (int)fs.Length;
            if (len > 0)
            {
                byte[] data = new byte[len];
                fs.Read(data, 0, len);
                fs.Close();
                return data;
            }
            else
            {
                fs.Close();
            }
        }
        return null;
    }

    static public string ReadStringFromFile(string filePath)
    {
        byte[] data = ReadDataFromFile(filePath);
        if (data == null)
        {
            return null;
        }
        string str = Encoding.UTF8.GetString(data);
        return str;
    }
    static public void WriteStringToFile(string filePath, string strcontent)
    {
        byte[] bytes = System.Text.Encoding.Default.GetBytes(strcontent);
        System.IO.File.WriteAllBytes(filePath, bytes);
    }

    static public bool isCloudRes()
    {
        if (Application.isEditor || GameManager.main.isLoadGameScreenShot)
        {
            return true;
        }
        if (CloudRes.main.enable)
        {
            return true;
        }
        return false;
    }

    //http://blog.csdn.net/ynnmnm/article/details/52253674
    /*
        我们常用的是以下四个路径：

    Application.dataPath 
    Application.streamingAssetsPath 
    Application.persistentDataPath 
    Application.temporaryCachePath 
    根据测试，详细情况如下：

    iOS:

    Application.dataPath            /var/containers/Bundle/Application/app sandbox/xxx.app/Data 
    Application.streamingAssetsPath /var/containers/Bundle/Application/app sandbox/test.app/Data/Raw 
    Application.temporaryCachePath /var/mobile/Containers/Data/Application/app sandbox/Library/Caches 
    Application.persistentDataPath  /var/mobile/Containers/Data/Application/app sandbox/Documents


     Android:

    Application.dataPath            /data/app/package name-1/base.apk 
    Application.streamingAssetsPath jar:file:///data/app/package name-1/base.apk!/assets 
    Application.temporaryCachePath /storage/emulated/0/android/data/com.moonma.learnword/cache 
    Application.persistentDataPath   /storage/emulated/0/Android/data/package name/files

     */
    //从streamingasset 读取，android为apk的asset目录下
    // file 为相对路径
    static public byte[] ReadDataAsset(string file)
    {

        //string fileDir = Application.dataPath + "/StreamingAssets";
        string fileDir = Application.streamingAssetsPath;
        string filePath = fileDir + "/" + file;

        // if (Application.isEditor || GameManager.main.isLoadGameScreenShot)
        if (isCloudRes())
        {
            if (file.Contains("GameRes/"))
            {
                Debug.Log("ReadDataAsset GameRes file =" + file);
                // filePath = FileUtil.GetFileDir(CloudRes.main.rootPathGameRes)+ "/" + file;
                filePath = file;
                Debug.Log("ReadDataAsset GameRes filePath =" + filePath);
                // filePath = Resource.dirResourceDataApp + "/" + file;
                // Debug.Log("filePath=" + filePath + " file=" + file);
                // if (!File.Exists(filePath))
                // {
                //     filePath = Resource.dirGameResCommon + "/" + file;
                //     filePath = filePath.Replace("GameRes/", Common.appKeyName + "/");
                //     Debug.Log("filePath common=" + filePath + " file=" + file);
                // }
                //直接读取resourcedata
                return ReadDataFromFile(filePath);
            }
        }

        if (Common.isAndroid)
        {
            byte[] data = null;
            using (var javaClass = new AndroidJavaClass(JAVA_CLASS_FILEUTIL))
            {

                data = javaClass.CallStatic<byte[]>("ReadDataAsset", file);
            }
            return data;
        }
        if (Common.isiOS)
        {
            //fileDir = Application.streamingAssetsPath;

        }

        if (Common.isiOS)
        {
            //StreamAsseting目录 ios真机只读 不可写
            return ReadDataAssetIos(filePath);
        }


        return ReadDataFromFile(filePath);
    }

    static public byte[] ReadRGBDataFromByte(byte[] data)
    {
        byte[] ret = null;
        if (Common.isAndroid)
        {
            using (var javaClass = new AndroidJavaClass(JAVA_CLASS_FILEUTIL))
            {

                ret = javaClass.CallStatic<byte[]>("ReadRGBDataFromByte", data);
            }

        }
        return ret;
    }

    static public byte[] ReadRGBDataAsset(string file)
    {

        //string fileDir = Application.dataPath + "/StreamingAssets";
        string fileDir = Application.streamingAssetsPath;
        if (Common.isAndroid)
        {
            byte[] data = null;
            using (var javaClass = new AndroidJavaClass(JAVA_CLASS_FILEUTIL))
            {

                data = javaClass.CallStatic<byte[]>("ReadRGBDataAsset", file);
            }
            return data;
        }
        if (Common.isiOS)
        {
            //fileDir = Application.streamingAssetsPath;

        }
        string filePath = fileDir + "/" + file;
        if (Common.isiOS)
        {
            //StreamAsseting目录 ios真机只读 不可写
            return ReadDataAssetIos(filePath);
        }
        return ReadDataFromFile(filePath);
    }

    static public byte[] ReadDataFromResources(string file)
    {
        TextAsset text = Resources.Load(GetFileBeforeExtWithOutDot(file)) as TextAsset;
        if (text == null)
        {
            return null;
        }
        byte[] data = text.bytes;
        return data;
    }

    static public string ReadStringFromRawFile(string file)
    {
        byte[] data = ReadDataFromFile(file);
        if (data == null)
        {
            return null;
        }
        string str = Encoding.UTF8.GetString(data);
        return str;
    }

    static public string ReadStringAsset(string file)
    {
        byte[] data = ReadDataAsset(file);
        if (data == null)
        {
            Debug.Log("ReadStringAsset data is null file=" + file);
            return null;
        }
        string str = Encoding.UTF8.GetString(data);
        return str;
    }
    static public string ReadStringFromResources(string file)
    {
        TextAsset text = Resources.Load(GetFileBeforeExtWithOutDot(file)) as TextAsset;
        if (text == null)
        {
            return null;
        }
        string str = text.text;
        // byte[] data = text.bytes;
        // if (data == null)
        // {
        //     return null;
        // }
        // string str = Encoding.UTF8.GetString(data);
        return str;
    }
    static public string ReadString2(string file)
    {
        string str = ReadStringAsset(file);
        if (str == null)
        {
            return null;
        }
        return str;
    }

    static public bool FileIsExist(string file)
    {
        if (File.Exists(file))
        {
            return true;
        }
        if (FileIsExistResource(file))
        {
            return true;
        }
        if (FileIsExistAsset(file))
        {
            return true;
        }

        return false;
    }

    static public bool FileIsExistResource(string file)
    {
        System.Object obj = Resources.Load(GetFileBeforeExtWithOutDot(file));
        if (obj == null)
        {
            return false;
        }
        return true;
    }

    //file 为相对路径
    static public bool FileIsExistAsset(string file)
    {

        string fileDir = Application.streamingAssetsPath;

        string filePath = fileDir + "/" + file;
        // if (Application.isEditor || GameManager.main.isLoadGameScreenShot)
        if (isCloudRes())
        {
            if (file.Contains("GameRes/"))
            {
                filePath = file;
                // filePath = Resource.dirResourceDataApp + "/" + file;
                // if (!File.Exists(filePath))
                // {
                //     filePath = Resource.dirGameResCommon + "/" + file;
                //     filePath = filePath.Replace("GameRes/", Common.appKeyName + "/");
                //     Debug.Log("Exists filePath common=" + filePath + " file=" + file);
                // }
                // 直接判断 CloudRes
                return File.Exists(filePath);

            }
        }


        if (Common.isAndroid)
        {

            bool ret = true;
            using (var javaClass = new AndroidJavaClass(JAVA_CLASS_FILEUTIL))
            {

                ret = javaClass.CallStatic<bool>("FileIsExistAsset", file);
            }



            return ret;
        }

        return File.Exists(filePath);
    }
    //文件名
    static public string GetFileName(string filepath)
    {
        string ret = filepath;
        int idx = filepath.LastIndexOf("/");
        if (idx >= 0)
        {
            string str = filepath.Substring(idx + 1);
            ret = str;
            idx = str.LastIndexOf(".");
            if (idx >= 0)
            {
                ret = str.Substring(0, idx);
            }
        }
        else
        {
            idx = filepath.LastIndexOf(".");
            if (idx >= 0)
            {
                ret = filepath.Substring(0, idx);
            }
        }
        return ret;
    }

    //文件后缀 如 png jpg ext 没有.
    static public string GetFileExt(string filepath)
    {
        string ret = "";
        int idx = filepath.LastIndexOf(".");
        if (idx >= 0)
        {
            ret = filepath.Substring(idx + 1);
        }
        return ret;
    }


    //上一级目录
    static public string GetLastDir(string filepath)
    {
        string ret = filepath;
        int idx = filepath.LastIndexOf("/");
        if (idx >= 0)
        {
            ret = filepath.Substring(0, idx);
        }

        return ret;
    }

    //除去文件后缀 
    static public string GetFileBeforeExt(string filepath)
    {
        string ret = filepath;
        int idx = filepath.LastIndexOf(".");
        if (idx >= 0)
        {
            ret = filepath.Substring(0, idx + 1);
        }
        return ret;
    }
    //除去文件后缀  并去除.
    static public string GetFileBeforeExtWithOutDot(string filepath)
    {
        string ret = filepath;
        int idx = filepath.LastIndexOf(".");
        if (idx >= 0)
        {
            ret = filepath.Substring(0, idx);
        }
        return ret;
    }

    //文件目录
    static public string GetFileDir(string filepath)
    {
        string ret = filepath;
        int idx = filepath.LastIndexOf("/");
        if (idx >= 0)
        {
            ret = filepath.Substring(0, idx);
        }
        return ret;
    }
    static public void CreateFile(string filepath)
    {
        if (!FileUtil.FileIsExist(filepath))
        {
            string dirSave = FileUtil.GetFileDir(filepath);
            FileUtil.CreateDir(dirSave);
            Debug.Log("create file jsonfile=" + filepath);
            System.IO.File.Create(filepath).Close();
        }


    }
    static public void CreateDir(string dir)
    {
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }
    static public bool DirIsExist(string dir)
    {
        return Directory.Exists(dir);

    }


    static public void DeleteDir(string dir)
    {

        //判断文件夹是否还存在
        if (!Directory.Exists(dir))
        {
            return;
        }

        //去除文件夹和子文件的只读属性
        //去除文件夹的只读属性
        System.IO.DirectoryInfo fileInfo = new DirectoryInfo(dir);
        fileInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;

        //去除文件的只读属性
        System.IO.File.SetAttributes(dir, System.IO.FileAttributes.Normal);

        //判断文件夹是否还存在
        // if (Directory.Exists(dir))
        {

            foreach (string f in Directory.GetFileSystemEntries(dir))
            {

                if (File.Exists(f))
                {
                    //如果有子文件删除文件
                    File.Delete(f);
                }
                else
                {
                    //循环递归删除子文件夹
                    DeleteDir(f);
                }

            }

            //删除空文件夹

            Directory.Delete(dir);

        }
    }
    static public void GetFileList(string dir, string ext, List<string> list)
    {
        System.IO.DirectoryInfo fileInfo = new DirectoryInfo(dir);
        //判断文件夹是否还存在
        if (Directory.Exists(dir))
        {
            foreach (string f in Directory.GetFileSystemEntries(dir))
            {
                if (File.Exists(f))
                {
                    if (GetFileExt(f) == ext)
                    {
                        if (list != null)
                        {
                            list.Add(f);
                        }
                    }
                }
                else
                {
                    //循环递归删除子文件夹
                    GetFileList(f, ext, list);
                }

            }

        }

    }

    static public void DeleteMetaFiles(string dir)
    {

        //去除文件夹和子文件的只读属性
        //去除文件夹的只读属性
        System.IO.DirectoryInfo fileInfo = new DirectoryInfo(dir);
        fileInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;

        //去除文件的只读属性
        System.IO.File.SetAttributes(dir, System.IO.FileAttributes.Normal);

        //判断文件夹是否还存在
        if (Directory.Exists(dir))
        {

            foreach (string f in Directory.GetFileSystemEntries(dir))
            {

                if (File.Exists(f))
                {
                    string ext = GetFileExt(f);
                    if (ext == "meta")
                    {
                        File.Delete(f);
                    }
                }
                else
                {
                    //循环递归删除子文件夹
                    DeleteMetaFiles(f);
                }

            }

        }

    }

    static public void CopyOneFile(string src, string dst, bool rewrite = true)
    {
        File.Copy(src, dst, rewrite);
    }
    static public void CopyFile(System.IO.DirectoryInfo path, string desPath)
    {
        string sourcePath = path.FullName;
        System.IO.FileInfo[] files = path.GetFiles();
        foreach (System.IO.FileInfo file in files)
        {
            string sourceFileFullName = file.FullName;
            string destFileFullName = sourceFileFullName.Replace(sourcePath, desPath);
            file.CopyTo(destFileFullName, true);
        }
    }
    static public void CopyDir(string sPath, string dPath)
    {
        string[] directories = System.IO.Directory.GetDirectories(sPath);
        if (!System.IO.Directory.Exists(dPath))
            System.IO.Directory.CreateDirectory(dPath);
        System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(sPath);
        System.IO.DirectoryInfo[] dirs = dir.GetDirectories();
        CopyFile(dir, dPath);
        if (dirs.Length > 0)
        {
            foreach (System.IO.DirectoryInfo temDirectoryInfo in dirs)
            {
                string sourceDirectoryFullName = temDirectoryInfo.FullName;
                string destDirectoryFullName = sourceDirectoryFullName.Replace(sPath, dPath);
                if (!System.IO.Directory.Exists(destDirectoryFullName))
                {
                    System.IO.Directory.CreateDirectory(destDirectoryFullName);
                }
                CopyFile(temDirectoryInfo, destDirectoryFullName);
                CopyDir(sourceDirectoryFullName, destDirectoryFullName);
            }
        }

    }
}