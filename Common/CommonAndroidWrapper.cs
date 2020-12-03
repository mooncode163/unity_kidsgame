using UnityEngine;
using System.Collections;
using System.IO;

internal class CommonAndroidWrapper : CommonBasePlatformWrapper
{
    public const string JAVA_CLASS_UTILS = "com.moonma.common.CommonUtils";
    public const string JAVA_CLASS_SCREEN_UTILS = "com.moonma.common.ScreenUtil";
    public const string JAVA_CLASS_UNITY = "com.moonma.unity.MainActivity";
    public override string GetAppName()
    {
        string ret;
        using (var javaClass = new AndroidJavaClass(JAVA_CLASS_UTILS))
        {
            ret = javaClass.CallStatic<string>("getAppName");
        }
        return ret;
    }
    public override string GetCachePath()
    {
        string ret;
        using (var javaClass = new AndroidJavaClass(JAVA_CLASS_UTILS))
        {
            //getDownloadPath getSDCardPath
            ret = javaClass.CallStatic<string>("getCachePath");
            //新建AppCache文件夹，保证有些android设备不会crash
            ret = ret + "/AppCache";
            //创建文件夹
            Directory.CreateDirectory(ret);
        }
        return ret;
    }
    public override string GetAppPackage()
    {
        string ret;
        using (var javaClass = new AndroidJavaClass(JAVA_CLASS_UTILS))
        {
            ret = javaClass.CallStatic<string>("getPackage");
        }
        return ret;
    }

    public override string GetAppVersion()
    {
        string ret;
        using (var javaClass = new AndroidJavaClass(JAVA_CLASS_UTILS))
        {
            ret = javaClass.CallStatic<string>("getAppVersion");
        }
        return ret;
    }

    public override string GetChannelName()
    {
        string ret;
        using (var javaClass = new AndroidJavaClass(JAVA_CLASS_UTILS))
        {
            ret = javaClass.CallStatic<string>("getChannelName");
        }
        return ret;
    }

    public override void UnityStartUpFinish()
    {
        using (var javaClass = new AndroidJavaClass(JAVA_CLASS_UNITY))
        {
            javaClass.CallStatic("unityStartUpFinish");
        }
    }

    public override void SetIpInChina(bool isin)
    {
        using (var javaClass = new AndroidJavaClass(JAVA_CLASS_UTILS))
        {
            javaClass.CallStatic("SetIpInChina", isin);
        }

    }
    public override void EnableAdSplash()
    {

    }

    public override void SetOrientation(int orientaion)
    {

        using (var javaClass = new AndroidJavaClass(JAVA_CLASS_UTILS))
        {
            javaClass.CallStatic("setOrientation", orientaion);
        }

    }

    public override int getHeightSystemTopBar()
    {
        int ret = 0;
        using (var javaClass = new AndroidJavaClass(JAVA_CLASS_SCREEN_UTILS))
        {
            ret = javaClass.CallStatic<int>("getTopBarHeight");
        }
        return ret;
    }
    public override int getHeightSystemHomeBar()
    {
        int ret = 0;
        using (var javaClass = new AndroidJavaClass(JAVA_CLASS_SCREEN_UTILS))
        {
            ret = javaClass.CallStatic<int>("getBottomNavigationBarHeight");
        }
        return ret;
    }


}


