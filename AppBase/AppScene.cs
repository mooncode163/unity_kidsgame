using System.Collections;
using System.Collections.Generic;
using Moonma.AdKit.AdVideo;
using UnityEngine;
using UnityEngine.UI;
public class AppScene : AppSceneBase
{
    public override void RunApp()
    {
        base.RunApp();

        InitAd();

        if (Common.isMonoPlayer)//isPC
        {
            SetRootViewController(ScreenShotViewController.main);
        }
        else
        {
            Init();
        }

    }

    public async void Init()
    {
        string tmp = await CloudRes.main.CheckNeedShow();
        if (CloudRes.main.isNeedShow)
        {
            Debug.Log("CheckVersion AppScene CloudRes resVersionWeb=" + CloudRes.main.resVersionWeb + " resVersionLocal=" + CloudRes.main.resVersionLocal);
            SetRootViewController(CloudResViewController.main);
        }
        else
        {

            SetRootViewController(MainViewController.main);
        }
    }


    bool EnableAdVideo()
    {
        if (Common.noad)
        {
            return false;
        }

        return true;
    }
    void InitAd()
    {
        if (EnableAdVideo())
        {
            //提前加载unity广告, 解决unity视频第一次不显示的问题
            AdVideo.SetType(AdVideo.ADVIDEO_TYPE_REWARD);
            {
                AdVideo.PreLoad(Source.UNITY);
                AdVideo.PreLoad(Source.MOBVISTA);
                AdVideo.PreLoad(Source.VUNGLE);
                AdVideo.PreLoad(Source.ADMOB);
            }


        }
        // AdKitCommon.main.InitAdBanner();
        bool isshow_adinsert = true;

        AdKitCommon.main.InitAdInsert();
        //AdKitCommon.main.InitAdVideo();

        if (Common.isAndroid)
        {
            if (Config.main.channel == Source.HUAWEI)
            {
                isshow_adinsert = false;
            }
        }
        // 显示开机插屏
        // if (isshow_adinsert)
        // {
        //     AdKitCommon.main.ShowAdInsert(100);
        // }


    }
}
