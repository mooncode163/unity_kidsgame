using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DG.Tweening;
using LitJson;
using Moonma.IAP;
using Moonma.Share;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ToolEditor : Editor
{


    //资源存放路径
    // static string assetsDir = Application.dataPath + "/AssetBundleBuilder/Res";
    static string assetsDir = Application.streamingAssetsPath + "/GameRes/image";


    //打包后存放路径
    const string assetBundlesPath = "AssetBundles";
    public const string KEY_MENU_GameObject = "Moonma/Tool";
    public const string UI_ROOT_DEFAULT = "/Script/Common/Resources/Common/UI";

    void Awake()
    {

    }
    // Use this for initialization
    void Start()
    {

    }

   

    [MenuItem(KEY_MENU_GameObject + "/MakeConfigPrefabImage", false, 4)]
    static public void MakeConfigPrefabAndImage()
    {
        MakeConfigPrefab();
        MakeConfigImage();

    }

    [MenuItem(KEY_MENU_GameObject + "/MakeConfigPrefab", false, 4)]
    static public void MakeConfigPrefab()
    {

        // LevelParseLearnWord.main.ConvertChinesWordFileToUnicode();

        string dir1 = Resource.dirResource + "/App/Prefab";
        string dir2 = Resource.dirResource + "/AppCommon/Prefab";
        ScanPrefabFile(dir1, Resource.dirResource + "/ConfigData/Prefab/ConfigPrefabApp.json");
        ScanPrefabFile(dir2, Resource.dirResource + "/ConfigData/Prefab/ConfigPrefabAppCommon.json");

        string dir = Resource.dirScript + "/Common/Resources/Common/Prefab";
        ScanPrefabFile(dir, dir + "/ConfigPrefab.json");

        Debug.Log("MakeConfigPrefab end");
    }


    static public void ScanPrefabFile(string dir, string jsonfile)
    {
        List<string> listFile = new List<string>();
        FileUtil.GetFileList(dir, "prefab", listFile);
        Hashtable data = new Hashtable();
        foreach (string strfile in listFile)
        {
            string name = strfile.Replace(Resource.dirResource + "/", "");
            name = name.Replace(Resource.dirScript + "/Common/Resources/", "");
            name = name.Replace("\\", "/");
            string key = FileUtil.GetFileName(name);
            // Debug.Log("name =" + name + " key=" + key);
            data[key] = name;
        }

        //save guanka json
        {
            StringBuilder sb = new StringBuilder();
            JsonWriter jr = new JsonWriter(sb);
            jr.PrettyPrint = true;//设置为格式化模式，LitJson称其为PrettyPrint（美观的打印），在 Newtonsoft.Json里面则是 Formatting.Indented（锯齿状格式）
            jr.IndentValue = 0;//缩进空格个数 
            JsonMapper.ToJson(data, jr);
            string strJson = sb.ToString();
            //Debug.Log(strJson);

            byte[] bytes = Encoding.UTF8.GetBytes(strJson);
            FileUtil.CreateFile(jsonfile);
            System.IO.File.WriteAllBytes(jsonfile, bytes);
        }

    }

    [MenuItem(KEY_MENU_GameObject + "/MakeConfigImage", false, 4)]
    static void MakeConfigImage()
    {
        string dir1 = Resource.dirResource + "/App/UI";
        string filepathJson = Resource.dirResource + "/ConfigData/Image/" + "ImageResApp.json";
        ScanImageFile(dir1, filepathJson);
        string dir2 = Resource.dirResource + "/AppCommon/UI";
        filepathJson = Resource.dirResource + "/ConfigData/Image/" + "ImageResAppCommon.json";
        ScanImageFile(dir2, filepathJson);

        string dir = Application.dataPath + UI_ROOT_DEFAULT;
        filepathJson = dir + "/ImageResDefault.json";
        ScanImageFile(dir, filepathJson);


        //    imageResCommon.Init(Common.RES_CONFIG_DATA_COMMON + "/Image/ImageRes.json");
        Debug.Log("MakeConfigImage end");
    }


    static public void ScanImageFile(string dir, string filepathJson)
    {
        List<string> listFile = new List<string>();
        FileUtil.GetFileList(dir, "png", listFile);
        FileUtil.GetFileList(dir, "jpg", listFile);

        Hashtable data = new Hashtable();
        foreach (string strfile in listFile)
        {
            string name = strfile.Replace(Resource.dirResource + "/", "");
            name = name.Replace(Application.dataPath + "/Script/Common/Resources" + "/", "");

            name = name.Replace("\\", "/");
            string key = FileUtil.GetFileName(name);
            // data[key] = name;
            Hashtable dataImge = new Hashtable();
            dataImge["path"] = name;
            // dataImge["border"] = "";
            string strborder = ImageRes.main.GetImageBoardString(name);
            if (!Common.BlankString(strborder))
            {
                dataImge[ImageResInternal.KEY_BOARD] = strborder;
            }
            data[key] = dataImge;
        }

        //save guanka json
        {
            StringBuilder sb = new StringBuilder();
            JsonWriter jr = new JsonWriter(sb);
            jr.PrettyPrint = true;//设置为格式化模式，LitJson称其为PrettyPrint（美观的打印），在 Newtonsoft.Json里面则是 Formatting.Indented（锯齿状格式）
            jr.IndentValue = 4;//缩进空格个数 
            JsonMapper.ToJson(data, jr);
            string strJson = sb.ToString();
            //Debug.Log(strJson); 
            FileUtil.CreateFile(filepathJson);
            byte[] bytes = Encoding.UTF8.GetBytes(strJson);
            System.IO.File.WriteAllBytes(filepathJson, bytes);
        }
    }
    //  https://blog.csdn.net/ithot/article/details/75128535
    [MenuItem(KEY_MENU_GameObject + "/Build AssetBundles", false, 4)]
    static void BuildAllAssetBundles()
    {
        string dir = "AssetBundles";
        if (Directory.Exists(dir) == false)
        {
            Directory.CreateDirectory(dir);
        }
        //BuildTarget 选择build出来的AB包要使用的平台
        BuildPipeline.BuildAssetBundles(dir, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);

        // AssetBundle ab = AssetBundle.LoadFromFile("AssetBundles/scene/wall.jy");
        // GameObject obj = ab.LoadAsset<GameObject>("wall");
        // Instantiate(obj);
    }



    [MenuItem(KEY_MENU_GameObject + "/AutoBuildAll")]
    static void AutoBuildAll()
    {
        //清除所有的AssetBundleName
        ClearAssetBundlesName();
        //设置指定路径下所有需要打包的assetbundlename
        SetAssetBundlesName(assetsDir);
        //打包所有需要打包的asset
        string dir = assetBundlesPath;
        if (Directory.Exists(dir) == false)
        {
            Directory.CreateDirectory(dir);
        }
        BuildPipeline.BuildAssetBundles(assetBundlesPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }

    [MenuItem("AssetBundle/BuildWithName")]
    static void BuildAllAssetBundle()
    {
        BuildPipeline.BuildAssetBundles(assetBundlesPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }



    /// <summary>
    /// 清除所有的AssetBundleName，由于打包方法会将所有设置过AssetBundleName的资源打包，所以自动打包前需要清理
    /// </summary>
    static void ClearAssetBundlesName()
    {
        //获取所有的AssetBundle名称
        string[] abNames = AssetDatabase.GetAllAssetBundleNames();

        //强制删除所有AssetBundle名称
        for (int i = 0; i < abNames.Length; i++)
        {
            AssetDatabase.RemoveAssetBundleName(abNames[i], true);
        }
    }

    /// <summary>
    /// 设置所有在指定路径下的AssetBundleName
    /// </summary>
    static void SetAssetBundlesName(string _assetsPath)
    {
        //先获取指定路径下的所有Asset，包括子文件夹下的资源
        DirectoryInfo dir = new DirectoryInfo(_assetsPath);
        FileSystemInfo[] files = dir.GetFileSystemInfos(); //GetFileSystemInfos方法可以获取到指定目录下的所有文件以及子文件夹

        for (int i = 0; i < files.Length; i++)
        {
            if (files[i] is DirectoryInfo)  //如果是文件夹则递归处理
            {
                SetAssetBundlesName(files[i].FullName);
            }
            else if (!files[i].Name.EndsWith(".meta")) //如果是文件的话，则设置AssetBundleName，并排除掉.meta文件
            {
                SetABName(files[i].FullName);     //逐个设置AssetBundleName
            }
        }

    }

    /// <summary>
    /// 设置单个AssetBundle的Name
    /// </summary>
    /// <param name="filePath"></param>
    static void SetABName(string assetPath)
    {
        string importerPath = "Assets" + assetPath.Substring(Application.dataPath.Length);  //这个路径必须是以Assets开始的路径
        AssetImporter assetImporter = AssetImporter.GetAtPath(importerPath);  //得到Asset

        string tempName = assetPath.Substring(assetPath.LastIndexOf(@"\") + 1);
        string assetName = tempName.Remove(tempName.LastIndexOf(".")); //获取asset的文件名称
        assetImporter.assetBundleName = assetName;    //最终设置assetBundleName
    }

    // 把"Assets/Resources/mats" 和“"Assets/Resources"下的图片文件全部编译成assetBundle
    static void ExportAssetBundles()
    {
        string[] Folder = { "Resources/", "Resources/mats/" };
        foreach (string path in Folder)
        {
            string[] fileEntries = Directory.GetFiles(Application.dataPath + "/" + path);
            foreach (string filePath in fileEntries)
            {
                string fileName = filePath;
                int index = fileName.LastIndexOf("/");
                fileName = fileName.Substring(index + 1);
                Debug.Log("fileName:" + fileName);
                string localPath = "Assets/" + path;
                if (index > 0)
                    localPath += fileName;
                Debug.Log("resource localPath:" + localPath);
                Object t = AssetDatabase.LoadMainAssetAtPath(localPath);
                if (t != null)
                {
                    Debug.Log(t.name);
                    string bundlePath = "Assets/" + path + t.name + ".unity3d";
                    Debug.Log("Building bundle at: " + bundlePath);
                    //BuildPipeline.BuildAssetBundle(t, null, bundlePath, BuildAssetBundleOptions.CompleteAssets);
                    // BuildPipeline.BuildAssetBundle(t, null, bundlePath, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);
                    BuildPipeline.BuildAssetBundles(bundlePath, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);
                }
            }
        }
    }

}
