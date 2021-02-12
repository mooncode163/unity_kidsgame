using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Moonma.IAP;
using Moonma.Share;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShotEditor : Editor
{
    public const string KEY_MENU_GameObject_UI = "ScreenShot";
    void Awake()
    {

    }
    // Use this for initialization
    void Start()
    {

    }

    [MenuItem(KEY_MENU_GameObject_UI + "/ConverIcon", false, 4)]
    static void OnConverIcon()
    {
        ImageConvert.main.OnConvertIcon();
    }


    [MenuItem(KEY_MENU_GameObject_UI + "/ConvertBg", false, 4)]
    static void OnConvertBg()
    {
        ImageConvert.main.OnConvertBg();
    }
    [MenuItem(KEY_MENU_GameObject_UI + "/ConvertScreenShot", false, 4)]
    static void OnConvertScreenShot()
    {
        ImageConvert.main.OnConvertScreenShot();
    }

    [MenuItem(KEY_MENU_GameObject_UI + "/DeleteAllScreenShot", false, 4)]
    static void OnDeleteAllScreenShot()
    {
        string dir = AppsConfig.ROOT_DIR_PC + "/ProjectOutPut/" + Common.appType + "/" + Common.appKeyName+"/screenshot";
        FileUtil.DeleteDir(dir);
    }

    [MenuItem(KEY_MENU_GameObject_UI + "/CopyRight", false, 4)]
    static void OnCopyRight()
    {

    }
}
