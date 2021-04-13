using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEditor.Build.Reporting;

//Unity命令行模式，也能「日志实时输出」
//https://www.jianshu.com/p/bd97cb8042a9

//http://www.xuanyusong.com/archives/2418
public class BuildPlayer
{
    public const string KEY_MENU_ROOT = "Moonma/Build";
    //得到工程中所有场景名称
    static string[] SCENES = FindEnabledEditorScenes();
    //一系列批量build的操作
    static public string dirRootProject;

    [MenuItem(KEY_MENU_ROOT + "/Tool/TestCompressFiles")]
    static void CompressFiles()
    {
        string[] fileName = new string[] {
            Application.dataPath + @"/ZipTest/zip01.txt",
            Application.dataPath + @"/ZipTest/zip02.txt"
        };
        string outputFilePath = Application.dataPath + @"/ZipTest/ZipTest.zip";
        ZipTool.ZipFile(fileName, outputFilePath, 9);
        AssetDatabase.Refresh();
    }

    [MenuItem(KEY_MENU_ROOT + "/Tool/TestUnCompressFiles")]
    static void UnCompressFiles()
    {
        string zipPath = Application.dataPath + @"/ZipTest/ZipTest.zip";
        string outPath = Application.dataPath + @"/ZipTest/UncCompress/";
        ZipTool.UnZipFile(zipPath, outPath);
        AssetDatabase.Refresh();
    }

    [MenuItem(KEY_MENU_ROOT + "/Export Android")]
    static void PerformAndroidBuild()
    {
        Debug.Log("PerformAndroidBuild android start");
        BulidTarget("UC", "Android");
        Debug.Log("PerformAndroidBuild android end");
    }

    [MenuItem(KEY_MENU_ROOT + "/Export iPhone")]
    static void PerformiPhoneBuild()
    {
        Debug.Log("PerformiPhoneBuild ios start");
#if UNITY_IOS
        Debug.Log("UNITY_IOS PerformiPhoneBuild start");
#endif

        BulidTarget("QQ", "IOS");
        Debug.Log("PerformiPhoneBuild ios end");
    }


    [MenuItem(KEY_MENU_ROOT + "/Screenshot")]
    static void ScreenshotBuild()
    {
        Debug.Log("ScreenshotBuild start");

        BulidTarget("QQ", "Screenshot");
        Debug.Log("ScreenshotBuild end");
    }

    [MenuItem(KEY_MENU_ROOT + "/Export Android & iOS")]
    static void PerformAndroidAndiOSBuild()
    {
        Debug.Log("PerformAndroidAndiOSBuild start ");
        PerformAndroidBuild();
        PerformiPhoneBuild();

        Debug.Log("PerformAndroidAndiOSBuild end ");
    }
    static void ConverIcon()
    {

        if (Directory.Exists(ImageConvert.main.GetRootDirSaveIcon(false)))
        {
            return;
        }
        ImageConvert.main.OnConvertIcon();
    }

    //这里封装了一个简单的通用方法。
    static void BulidTarget(string name, string target)
    {
        ConverIcon();

        // ToolEditor.MakeConfigPrefabAndImage();

        string app_name = name;
        string target_dir = Application.dataPath + "/OutPut";
        string target_name = name + ".apk";
        BuildTargetGroup targetGroup = BuildTargetGroup.Android;
        BuildTarget buildTarget = BuildTarget.Android;
        string applicationPath = Application.dataPath.Replace("/Assets", "");

        if (target == "Android")
        {
            // F:\sourcecode\unity\product\kidsgame\kidsgameUnity
            String dir = applicationPath;
            dir = FileUtil.GetLastDir(dir);
            target_dir = dir + "/ProjectOutPut/Unity/" + Common.appType + "/" + Common.appKeyName;
            target_name = app_name + ".apk";
            target_name = "Android";
            targetGroup = BuildTargetGroup.Android;
        }
        if (target == "IOS")
        {
            //target_dir = applicationPath + "/OutPut/iOS";
            // target_dir = FileUtil.GetLastDir(applicationPath) + "/project_ios";
            target_dir = Resource.dirProjectIos;
            // target_name = app_name;
            target_name = PlayerSettings.productName + "_device" + "_" + Common.appType + "_" + Common.appKeyName;// "iOS";
            targetGroup = BuildTargetGroup.iOS;
            buildTarget = BuildTarget.iOS;

        }

        if (target == "Screenshot")
        {
            //target_dir = applicationPath + "/OutPut/iOS";
            target_dir = Resource.dirProduct + "/bin";
            FileUtil.DeleteDirContent(target_dir);
            // target_name = app_name;
            target_name = PlayerSettings.productName;
            targetGroup = BuildTargetGroup.Standalone;
            buildTarget = BuildTarget.StandaloneOSX;
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                buildTarget = BuildTarget.StandaloneWindows;
            }


        }

        dirRootProject = target_dir + "/" + target_name;

        // EditorXcode(target_dir + "/" + target_name);
        // return;

        //每次build删除之前的残留
        if (Directory.Exists(dirRootProject))
        {
            Directory.Delete(dirRootProject, true);
        }
        else
        {
            // Directory.Delete(path);           //这种删除如果当目录内的内容不为空时会报错
            //Directory.Delete(path,true);      //第二个参数代表如果内容不为空是否也要删除，这样就不会报错了

            Directory.CreateDirectory(target_dir);
        }
        //==================这里是比较重要的东西=======================

        //PlayerSettings.applicationIdentifier = "com.moonma.kidsgame";
        PlayerSettings.bundleVersion = "0.0.1";
        PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, "QQ");


        //==================这里是比较重要的东西=======================

        //开始Build场景，等待吧～
        GenericBuild(SCENES, target_dir + "/" + target_name, targetGroup, buildTarget, BuildOptions.None);

    }

    private static string[] FindEnabledEditorScenes()
    {
        List<string> EditorScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            EditorScenes.Add(scene.path);
        }
        return EditorScenes.ToArray();
    }


    static void CopyGameRes()
    {
        Debug.Log("BuildiOSPlayer copy gameres start ");
        string src = Resource.dirResourceDataGameRes;
        string dst = Resource.dirProjectXcode + "/Data/Raw/GameRes";
        FileUtil.DeleteDir(dst);
        FileUtil.CopyDir(src, dst);
        Debug.Log("BuildiOSPlayer copy gameres end ");
    }
    static void EditorXcode(string target_dir)
    {
        // #if UNITY_IOS
        Debug.Log("BuildiOSPlayer start ");
        BuildiOSPlayer.EditProj(target_dir);
        Debug.Log("BuildiOSPlayer end ");
        // #endif
        // copy gameres
        CopyGameRes();
    }

    //https://docs.unity3d.com/ScriptReference/BuildPipeline.BuildPlayer.html

    static void GenericBuild(string[] scenes, string target_dir, BuildTargetGroup targetGroup, BuildTarget build_target, BuildOptions build_options)
    {

        if (Directory.Exists(target_dir))
        {
            FileUtil.DeleteDir(target_dir);
        }
        else
        {
            Directory.CreateDirectory(target_dir);
        }

        EditorUserBuildSettings.SwitchActiveBuildTarget(targetGroup, build_target);
        // string res =
        BuildReport report = BuildPipeline.BuildPlayer(scenes, target_dir, build_target, build_options);
        // BuildSummary summary;
        BuildSummary summary = report.summary;
        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes build_target=" + build_target);
            if (build_target == BuildTarget.iOS)
            {
                // #if UNITY_IOS
                Debug.Log("BuildiOSPlayer start ");
                BuildiOSPlayer.EditProj(target_dir);
                Debug.Log("BuildiOSPlayer end ");
                // #endif
                // copy gameres
                CopyGameRes();
            }


            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                if (build_target == BuildTarget.iOS)
                {
                    FileUtil.DeleteMetaFiles(dirRootProject + "/Frameworks/Plugins");
                    FileUtil.DeleteMetaFiles(dirRootProject + "/Libraries/Plugins");
                    //RunShell阿里云上异常
                    RunShell.RunProcessCommand("explorer.exe", dirRootProject);
                }
            }
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
        // if (res.Length > 0)
        // {
        //     throw new Exception("BuildPlayer failure: " + res);
        // }
    }

}