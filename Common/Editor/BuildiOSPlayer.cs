// #if UNITY_IOS

using UnityEngine;

using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using UnityEditor.iOS.Xcode;
using System.Collections.Generic;
using System.Text.RegularExpressions;
//Unity iOS 一键打包 https://www.jianshu.com/p/f347e00abb9c
//https://www.cnblogs.com/laugher/p/6951232.html

/*（2）检查IDFA的方法：
// https://www.jianshu.com/p/78d1fbc24e77
步骤：
1、打开终端cd到要检查的文件的根目录。

2、执行下列语句：grep -r advertisingIdentifier .   （别少了最后那个点号）。
*/

public static class BuildiOSPlayer
{
    ////该属性是在build完成后，被调用的callback

    // #if UNITY_2019_2_0
    [PostProcessBuildAttribute]// PostProcessBuild PostProcessBuildAttribute
                               // #else
                               // [PostProcessBuildAttribute]
                               // #endif
                               // 
    static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
    {
        Debug.Log("BuildiOSPlayer:" + pathToBuiltProject);

        //  EditProj(pathToBuiltProject); 
    }

    //添加lib方法
    static void AddLibToProject(PBXProject inst, string targetGuid, string lib)
    {
        string fileGuid = inst.AddFile("usr/lib/" + lib, "Frameworks/" + lib, PBXSourceTree.Sdk);
        inst.AddFileToBuild(targetGuid, fileGuid);
    }

    static void AddFileToProject(string projPath, PBXProject inst, string targetGuid, string filepath)
    {
        string fileGuid = inst.AddFile(filepath, filepath, PBXSourceTree.Source);
        inst.AddFileToBuild(targetGuid, fileGuid);
    }

    static void AddThirdLibAToProject(string projPath, PBXProject inst, string targetGuid, string filepath)
    {
        string fileGuid = inst.AddFile(filepath, filepath, PBXSourceTree.Source);
        inst.AddFileToBuild(targetGuid, fileGuid);
    }

    //递归遍历如下：将已知路径和列表数组作为参数传递，
    static public void ScanLibAFiles(string dir, List<string> list)
    {
        DirectoryInfo d = new DirectoryInfo(dir);
        FileInfo[] files = d.GetFiles();//文件
        DirectoryInfo[] directs = d.GetDirectories();//文件夹
        foreach (FileInfo f in files)
        {
            string ext = FileUtil.GetFileExt(f.Name);
            if (ext == "a")
            {
                list.Add(f.FullName);//添加文件名到列表中  
            }
        }
        //获取子文件夹内的文件列表，递归遍历  
        foreach (DirectoryInfo dd in directs)
        {
            string ext = FileUtil.GetFileExt(dd.FullName);
            if ((ext != "framework"))
            {
                ScanLibAFiles(dd.FullName, list);
            }
        }

    }

    static public void ScanFrameworkFiles(string dir, List<string> list)
    {
        DirectoryInfo d = new DirectoryInfo(dir);
        FileInfo[] files = d.GetFiles();//文件
        DirectoryInfo[] directs = d.GetDirectories();//文件夹 
        //获取子文件夹内的文件列表，递归遍历  
        foreach (DirectoryInfo dd in directs)
        {
            string ext = FileUtil.GetFileExt(dd.FullName);
            if ((ext == "framework"))
            {
                list.Add(dd.FullName);
            }
            else
            {
                ScanFrameworkFiles(dd.FullName, list);
            }
        }

    }

    static void AddThirdPartyLibToProject(string projPath, PBXProject inst, string targetGuid, string dirLib)
    {
        Debug.Log("AddThirdPartyLibToProject dirLib=" + dirLib);
        DirectoryInfo TheFolder = new DirectoryInfo(dirLib);
        //Libraries/Plugins 
        //Frameworks/Plugins 
        // //遍历文件

        List<string> listFileLibA = new List<string>();
        ScanLibAFiles(dirLib, listFileLibA);
        foreach (string fullpath in listFileLibA)
        {
            Debug.Log("liba fullpath=" + fullpath);
            int idx = BuildPlayer.dirRootProject.Length;
            string addfilepath = fullpath.Substring(idx + 1);
            Debug.Log("liba addfilepath=" + addfilepath);
            AddThirdLibAToProject(projPath, inst, targetGuid, addfilepath);
        }


        // List<string> listFileFramework = new List<string>();
        // ScanFrameworkFiles(dirLib, listFileFramework);
        // foreach (string fullpath in listFileFramework)
        // {
        //     Debug.Log("Framework fullpath=" + fullpath);
        //     int idx = BuildPlayer.dirRootProject.Length;
        //     string addfilepath = fullpath.Substring(idx + 1);
        //     Debug.Log("Framework addfilepath=" + addfilepath);
        //     AddFileToProject(projPath, inst, targetGuid, addfilepath);
        // }
    }

    static bool isOldEditor()
    {
        bool ret = false;

#if UNITY_2019_2_0 || UNITY_2019_2_21
            ret = true;
#endif

        return ret;
    }

    //  ProvisioningStyle = Manual;  Automatic
    public static void EditProj(string pathToBuiltProject)
    {
        Debug.Log("BuildiOSPlayer EditProj start:" + pathToBuiltProject);
        string projPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
        Debug.Log("BuildiOSPlayer EditProj:" + pathToBuiltProject);
        string strFile = FileUtil.ReadStringFromFile(projPath);
        strFile = strFile.Replace("ProvisioningStyle = Manual", "ProvisioningStyle = Automatic");
        strFile = strFile.Replace("iPhone Distribution", "iPhone Developer");


        FileUtil.WriteStringToFile(projPath, strFile);

        PBXProject pbxProj = new PBXProject();
        pbxProj.ReadFromFile(projPath);
        string unityVersion = Application.unityVersion;
        Debug.Log("unityVersion=" + unityVersion);
        string projectGuid = pbxProj.ProjectGuid();
        string targetGuid = "", unityFrameworkTargetGuid = "";
        bool isOldUnity = false;


#if UNITY_2019_2_0 || UNITY_2019_2_21

        isOldUnity = true;
        {
            targetGuid = pbxProj.TargetGuidByName("Unity-iPhone");
            unityFrameworkTargetGuid = targetGuid;
        }
#else
        {
            targetGuid = pbxProj.GetUnityMainTargetGuid();
            unityFrameworkTargetGuid = pbxProj.GetUnityFrameworkTargetGuid();
            // AddLibToProject(pbxProj, unityFrameworkTargetGuid, "libxml2.tbd");

            AddFileToProject(projPath, pbxProj, targetGuid, "Libraries/AdmobBtnClose.png");
            AddFileToProject(projPath, pbxProj, targetGuid, "Libraries/Plugins/iOS/UnifiedNativeAdView.xib");
        }
#endif



        //string debugConfig = pbxProj.BuildConfigByName(target, "Debug");
        //string releaseConfig = pbxProj.BuildConfigByName(target, "Release");
        //pbxProj.SetBuildProperty(targetGuid, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");
        //pbxProj.SetBuildPropertyForConfig(debugConfig, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");
        //pbxProj.SetBuildPropertyForConfig(releaseConfig, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");

        pbxProj.AddFrameworkToProject(unityFrameworkTargetGuid, "AdSupport.framework", false);
        pbxProj.AddFrameworkToProject(unityFrameworkTargetGuid, "StoreKit.framework", false);
        pbxProj.AddFrameworkToProject(unityFrameworkTargetGuid, "CoreLocation.framework", false);
        pbxProj.AddFrameworkToProject(unityFrameworkTargetGuid, "CoreTelephony.framework", false);
        pbxProj.AddFrameworkToProject(unityFrameworkTargetGuid, "WebKit.framework", false);

        //chsj
        // pbxProj.AddFrameworkToProject(targetGuid, "StoreKit.framework", false);
        pbxProj.AddFrameworkToProject(unityFrameworkTargetGuid, "MobileCoreServices.framework", false);
        // pbxProj.AddFrameworkToProject(targetGuid, "WebKit.framework", false);
        pbxProj.AddFrameworkToProject(unityFrameworkTargetGuid, "MediaPlayer.framework", false);
        pbxProj.AddFrameworkToProject(unityFrameworkTargetGuid, "CoreMedia.framework", false);
        // pbxProj.AddFrameworkToProject(targetGuid, "CoreLocation.framework", false);
        pbxProj.AddFrameworkToProject(unityFrameworkTargetGuid, "AVFoundation.framework", false);
        // pbxProj.AddFrameworkToProject(targetGuid, "CoreTelephony.framework", false);
        pbxProj.AddFrameworkToProject(unityFrameworkTargetGuid, "SystemConfiguration.framework", false);
        // pbxProj.AddFrameworkToProject(targetGuid, "AdSupport.framework", false);
        pbxProj.AddFrameworkToProject(unityFrameworkTargetGuid, "CoreMotion.framework", false);
        pbxProj.AddFrameworkToProject(unityFrameworkTargetGuid, "Accelerate.framework", false);
        pbxProj.AddFrameworkToProject(unityFrameworkTargetGuid, "SystemConfiguration.framework", false);
        pbxProj.AddFrameworkToProject(unityFrameworkTargetGuid, "ImageIO.framework", false);
        AddLibToProject(pbxProj, unityFrameworkTargetGuid, "libresolv.9.tbd");
        AddLibToProject(pbxProj, unityFrameworkTargetGuid, "libc++.tbd");
        // AddLibToProject(pbxProj, unityFrameworkTargetGuid, "libz.tbd");
        AddLibToProject(pbxProj, unityFrameworkTargetGuid, "libsqlite3.tbd");
        if (!isOldUnity)
        {
            AddFileToProject(projPath, pbxProj, targetGuid, "Frameworks/Plugins/iOS/ThirdParty/chsj/BUAdSDK.bundle");
        }

        //chsj


        //添加lib
        AddLibToProject(pbxProj, unityFrameworkTargetGuid, "libz.tbd");
        AddLibToProject(pbxProj, targetGuid, "libxml2.tbd");
        AddLibToProject(pbxProj, unityFrameworkTargetGuid, "libxml2.tbd");

        //多国语言
        AddFileToProject(projPath, pbxProj, targetGuid, "appname/en.lproj/InfoPlist.strings");
        AddFileToProject(projPath, pbxProj, targetGuid, "appname/zh-Hans.lproj/InfoPlist.strings");

        //pbxProj.AddFileToBuild(targetGuid, pbxProj.AddFile("usr/lib/libsqlite3.dylib", "Frameworks/libsqlite3.dylib", PBXSourceTree.Sdk));
        //pbxProj.AddFileToBuild(targetGuid, pbxProj.AddFile("usr/lib/libz.dylib", "Frameworks/libz.dylib", PBXSourceTree.Sdk));

        Debug.Log("isOldUnity=" + isOldUnity);
        if (!isOldUnity)
        {
            //unity 2019.3 bug
            //需要手动添加第三方库 1,所有的framework到Unity-iPhone和UnityFramework 2,libSocialQQ.a 到Unity-iPhone
            // AddThirdPartyLibToProject(projPath, pbxProj, targetGuid, pathToBuiltProject + "/Frameworks/Plugins");

            //gdt 启动闪退问题
            pbxProj.SetBuildProperty(targetGuid, "LIBRARY_SEARCH_PATHS", "$(inherited)");
            AddThirdPartyLibToProject(projPath, pbxProj, targetGuid, pathToBuiltProject + "/Libraries/Plugins");
            // AddFileToProject(projPath, pbxProj, targetGuid, "Libraries/Plugins/iOS/ThirdParty/gdt/lib/libGDTMobSDK.a");
            pbxProj.AddBuildProperty(targetGuid, "LIBRARY_SEARCH_PATHS", "$(PROJECT_DIR)/Libraries/Plugins/iOS/ThirdParty/gdt/lib");
        }




        // 添加flag
        pbxProj.AddBuildProperty(targetGuid, "OTHER_LDFLAGS", "-ObjC");


        //Unity采用XCode11编译后，在iOS 12.0机器上Crash问题 https://zhuanlan.zhihu.com/p/103076213
        //pbxProj.AddBuildProperty(targetGuid, "OTHER_CFLAGS", "-mno-outline");


        // 打开选项
        pbxProj.SetBuildProperty(targetGuid, "ENABLE_BITCBITCODE", "NO");
        pbxProj.SetBuildProperty(projectGuid, "ENABLE_BITCODE", "NO");

        pbxProj.SetBuildProperty(targetGuid, "CLANG_ENABLE_MODULES", "YES");
        pbxProj.SetBuildProperty(projectGuid, "CLANG_ENABLE_MODULES", "YES");




        //teamid 
        pbxProj.SetTeamId(targetGuid, "Y9ZUK2WTEE");

        #region 添加资源文件(中文路径 会导致 project.pbxproj 解析失败)
        // string frameworksPath = Application.dataPath + "/Frameworks";
        // string[] directories = Directory.GetDirectories(frameworksPath, "*", SearchOption.TopDirectoryOnly);
        // for (int i = 0; i < directories.Length; i++)
        // {
        //     string path = directories[i];

        //     string name = path.Replace(frameworksPath + "/", "");
        //     string destDirName = pathToBuiltProject + "/" + name;

        //     if (Directory.Exists(destDirName))
        //         Directory.Delete(destDirName, true);

        //     Debug.Log(path + " => " + destDirName);
        //     Utility.CopyDirectory(path, destDirName, new string[] { ".meta", ".framework", ".mm", ".c", ".m", ".h", ".xib", ".a", ".plist", ".org", "" }, false);

        //     foreach (string file in Directory.GetFiles(destDirName, "*.*", SearchOption.AllDirectories))
        //         pbxProj.AddFileToBuild(targetGuid, pbxProj.AddFile(file, file.Replace(pathToBuiltProject + "/", ""), PBXSourceTree.Source));
        // }
        #endregion

        pbxProj.WriteToFile(projPath);
    }

    static void EditInfoPlist(string filePath)
    {
        //         string path = filePath + "/Info.plist";

        //         PlistDocument plistDocument = new PlistDocument();
        //         plistDocument.ReadFromFile(path);

        //         PlistElementDict dict = plistDocument.root.AsDict();

        //         PlistElementArray array = dict.CreateArray("CFBundleURLTypes");
        //         PlistElementDict dict2 = array.AddDict();
        //         dict2.SetString("CFBundleURLName", PlayerSettings.bundleIdentifier);
        //         PlistElementArray array2 = dict2.CreateArray("CFBundleURLSchemes");
        //         array2.AddString(PlayerSettings.bundleIdentifier);

        //         dict2 = array.AddDict();
        //         dict2.SetString("CFBundleURLName", "weixin");
        //         array2 = dict2.CreateArray("CFBundleURLSchemes");
        //         array2.AddString(BabybusConst.WEIXIN_ID);

        //         dict2 = array.AddDict();
        //         dict2.SetString("CFBundleURLName", "");
        //         array2 = dict2.CreateArray("CFBundleURLSchemes");
        //         array2.AddString("QQ" + BabybusConst.QQ_ID.ToString("X"));

        //         dict2 = array.AddDict();
        //         dict2.SetString("CFBundleURLName", "");
        //         array2 = dict2.CreateArray("CFBundleURLSchemes");
        //         array2.AddString("tencent" + BabybusConst.QQ_ID);


        // #region quick action
        //         string[] quickActions = { "Poem", "Pet", "Movie", "Telephone" };
        //         string[] quickActionsIcon = { "PoemIcon", "PetIcon", "MovieIcon", "TelephoneIcon" };
        //         //string[] icons = { "UIApplicationShortcutIconTypeBookmark", "UIApplicationShortcutIconTypeLove", "UIApplicationShortcutIconTypeCaptureVideo", "UIApplicationShortcutIconTypeFavorite" };
        //         array = dict.CreateArray("UIApplicationShortcutItems");
        //         for(int i=0; i<quickActions.Length; ++i)
        //         {
        //             dict2 = array.AddDict();
        //             //dict2.SetString("UIApplicationShortcutItemIconType", icons[i]);
        //             dict2.SetString("UIApplicationShortcutItemIconFile", quickActionsIcon[i]);
        //             dict2.SetString("UIApplicationShortcutItemTitle", quickActions[i] + "Title");
        //             dict2.SetString("UIApplicationShortcutItemType", quickActions[i]);
        //             dict2.CreateDict("UIApplicationShortcutItemUserInfo");
        //             //dict2.SetString("UIApplicationShortcutItemSubtitle", quickActions[i]);
        //         }
        // #endregion

        //         dict.SetString("CFBundleIdentifier", PlayerSettings.bundleIdentifier);

        //         var assetInfos = Utility.DeserializeXmlFromFile<List<AssetInfo>>(Application.dataPath + "/Resources/配置/APP.xml");
        //         array = dict.CreateArray("LSApplicationQueriesSchemes");
        //         foreach (var assetInfo in assetInfos)
        //         {
        //             if (string.IsNullOrEmpty(assetInfo.bundleIdentifier4iOS))
        //                 array.AddString(assetInfo.extra);
        //             else
        //                 array.AddString(assetInfo.bundleIdentifier4iOS);
        //         }

        //         plistDocument.WriteToFile(path);
    }

    static void EditUnityAppController(string pathToBuiltProject)
    {
        // string unityAppControllerPath = pathToBuiltProject + "/Classes/UnityAppController.mm";
        // if (File.Exists(unityAppControllerPath))
        // {
        //     string headerCode = "#include \"../Libraries/Plugins/iOS/SDKPlatformIOS.h\"\n" +
        //                         "#import <AVFoundation/AVAudioSession.h>\n\n";
        //     string unityAppController = headerCode + File.ReadAllText(unityAppControllerPath);

        //     Match match = Regex.Match(unityAppController, @"- \(void\)startUnity:\(UIApplication\*\)application\s+\{[^}]+\}");
        //     if(match.Success)
        //     {
        //         string newCode = match.Groups[0].Value.Remove(match.Groups[0].Value.Length - 1);
        //         newCode += "\n" +
        //                    "    [[AVAudioSession sharedInstance] setCategory: AVAudioSessionCategoryPlayback error: nil];\n" +
        //                    "    [[AVAudioSession sharedInstance] setActive:YES error:nil];\n" +
        //                    "}\n\n" +
        //                    "- (void)application:(UIApplication*)application performActionForShortcutItem: (UIApplicationShortcutItem*)shortcutItem completionHandler: (void(^)(BOOL))completionHandler\n" +
        //                    "{\n" +
        //                    "    [[SDKPlatform share] performActionForShortcutItem:shortcutItem];\n" +
        //                    "}";
        //         unityAppController = unityAppController.Replace(match.Groups[0].Value, newCode);
        //     }

        //     File.WriteAllText(unityAppControllerPath, unityAppController);
        // }
    }
}

// #endif