using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Moonma.AdKit.AdBanner;
using Moonma.AdKit.AdInsert;
using Moonma.AdKit.AdVideo;
using Moonma.AdKit.AdConfig;
using Moonma.AdKit.AdNative;

/*
admob ios sdk
https://developers.google.com/admob/ios/quick-start

https://developers.google.com/admob/ios/download

*/

public delegate void OnAdKitFinishDelegate(AdKitCommon.AdType type, AdKitCommon.AdStatus status, string str);
public class AdKitCommon : MonoBehaviour
{
    public const int INSERT_NOAD_DAY = 1;
    public enum AdType
    {
        BANNER = 0,
        INSERT,
        NATIVE,
        VIDEO
    }
    public enum AdStatus
    {
        FAIL = 0,
        SUCCESFULL,
        START,
        CLOSE
    }
    public static AdKitCommon main;
    public bool enableBanner = true;
    bool isAdVideoFinish;
    public float heightAdWorld;
    public float heightAdScreen;
    public float heightAdCanvas;
    public int adinsertNoadDay;
    public OnAdKitFinishDelegate callbackFinish { get; set; }
    public OnAdKitFinishDelegate callbackAdVideoFinish { get; set; }

    public async void GetIPInfo()
    {
        // return;
        Debug.Log("IPInfo GetIPInfo start");
        int ret = 0;
         Debug.Log("IPInfo GetIPInfo   0");
        await IPInfo.main.GetIpInfoAsync();
         Debug.Log("IPInfo GetIPInfo   1");
        if (IPInfo.main.IsHuaweiAppStoreCheck())
        {
            ret = INSERT_NOAD_DAY;
        }
         Debug.Log("IPInfo GetIPInfo   2");
        adinsertNoadDay = ret;

        Debug.Log("IPInfo GetIPInfo adinsertNoadDay =" + adinsertNoadDay);
    }


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (main == null)
        {
            main = this;
        }
        isAdVideoFinish = false;
        // GetIPInfo();
        // enableBanner = true;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void InitAdBanner()
    {
        if (!enableBanner)
        {
            return;
        }
        if (Common.noad)
        {
            return;
        }
        bool isShowAdBanner = true;
        if (Common.isiOS)
        {
            if (!AppVersion.appCheckHasFinished)
            {
                //ios app审核不显示banner
                isShowAdBanner = false;
            }
        }

        if (Common.isAndroid)
        {

            if (!AppVersion.appCheckHasFinished)
            {
                //xiaomi app审核不显示banner
                isShowAdBanner = false;
            }
        }

        if (isShowAdBanner)
        {
            AdBanner.SetScreenSize(Screen.width, Screen.height);
            AdBanner.SetScreenOffset(0, Device.heightSystemHomeBar);
            {
                int type = AdConfigParser.SOURCE_TYPE_BANNER;
                string source = AdConfig.main.GetAdSource(type);
                AdBanner.InitAd(source);
                AdBanner.ShowAd(true);
            }
        }




    }

    public void AdBannerSetScreenOffsetY(float y)
    {
        AdBanner.SetScreenOffset(0, (int)y);
    }

    public void InitAdInsert()
    {
        if (Common.noad)
        {
            return;
        }
        GetIPInfo();
        if (Config.main.channel == Source.HUAWEI)
        {
            // 华为不能  应用频繁弹窗恶意广告
            if (Common.GetDayIndexOfUse() <= adinsertNoadDay)
            {
                return;
            }
        }
        bool isShowAdInsert = false;
        if (AppVersion.appCheckHasFinished)
        {
            isShowAdInsert = true;
        }
        if (isShowAdInsert)
        {
            AdInsert.SetObjectInfo(this.gameObject.name);
            int type = AdConfigParser.SOURCE_TYPE_INSERT;
            string source = AdConfig.main.GetAdSource(type);
            AdInsert.InitAd(source);
        }

    }

    public void InitAdVideo()
    {
        if (Common.noad)
        {
            return;
        }

        if (AppVersion.appCheckHasFinished)
        {
            AdVideo.SetType(AdVideo.ADVIDEO_TYPE_REWARD);
            int type = AdConfigParser.SOURCE_TYPE_VIDEO;
            string source = AdConfig.main.GetAdSource(type);
            Debug.Log("InitAdVideo AdVideo.InitAd =" + source);
            AdVideo.InitAd(source);
        }
    }

    public void ShowAdBanner(bool isShow)
    {
        if (!enableBanner)
        {
            return;
        }
        AdBanner.ShowAd(isShow);
    }
    public void ShowAdVideo()
    {
        if (Common.noad)
        {
            return;
        }
        if (!AppVersion.appCheckHasFinished)
        {
            return;
        }
        //show 之前重新设置广告
        InitAdVideo();
        AdVideo.ShowAd();
    }



    public void ShowAdInsertWithStep(int step, bool isAlwasy)
    {
        int _step = step;
        if (_step <= 0)
        {
            _step = 1;
        }
        GameManager.main.isShowGameAdInsert = false;
        bool isshow = false;
        if (isAlwasy)
        {
            isshow = true;
        }
        else
        {
            if ((LevelManager.main.gameLevel != 0) && ((LevelManager.main.gameLevel % _step) == 0))
            //if ((LevelManager.main.gameLevel % _step) == 0)
            {
                isshow = true;
            }
        }
        if (isshow)
        {
            AdKitCommon.main.InitAdInsert();
            AdKitCommon.main.ShowAdInsert(100);
            GameManager.main.isShowGameAdInsert = true;
        }
    }


    public void ShowAdInsert(int rate)
    {

        if (!AppVersion.appCheckHasFinished)
        {
            return;
        }

        if (Common.noad)
        {
            return;
        }

        if (Common.isAndroid)
        {
            if (Common.GetDayIndexOfUse() <= Config.main.NO_AD_DAY)
            {
                return;
            }

            if (Config.main.channel == Source.HUAWEI)
            {
                // 华为不能  应用频繁弹窗恶意广告
                if (Common.GetDayIndexOfUse() <= adinsertNoadDay)
                {
                    return;
                }
            }
        }


        int randvalue = Random.Range(0, 100);
        if (randvalue > rate)
        {
            return;
        }
        //show 之前重新设置广告
        //InitAdInsert();
        AdInsert.ShowAd();
    }

    //原生开机广告
    public void ShowAdNativeSplash(string source)
    {
        if (!AppVersion.appCheckHasFinished)
        {
            return;
        }

        if (Common.noad)
        {
            return;
        }

        if (Common.isAndroid)
        {
            if (Common.GetDayIndexOfUse() <= Config.main.NO_AD_DAY)
            {
                return;
            }
        }

        AdNative.ShowSplash(source);

    }

    //c++调用c#的回调
    public void AdBannerCallbackUnity(string source, string method, int w, int h)
    {
        Debug.Log("AdBannerCallbackUnity method=" + method + "  w=" + w + " h=" + h);
        if ("AdDidFinish" == method)
        {
            AdBannerDidReceiveAd(w + ":" + h);
        }
        if ("AdDidFail" == method)
        {
            AdBannerDidReceiveAdFail(source);
        }
    }
    public void AdBannerDidReceiveAd(string str)
    {

        int w = 0;
        int h = 0;
        int idx = str.IndexOf(":");
        string strW = str.Substring(0, idx);
        int.TryParse(strW, out w);
        string strH = str.Substring(idx + 1);
        int.TryParse(strH, out h);
        Debug.Log("AdBannerDidReceiveAd::w=" + w + " h=" + h);

        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        AdKitCommon.main.heightAdScreen = h + Device.heightSystemHomeBar;
        AdKitCommon.main.heightAdWorld = Common.ScreenToWorldHeight(AppSceneBase.main.mainCamera, h);
        AdKitCommon.main.heightAdCanvas = Common.ScreenToCanvasHeigt(sizeCanvas, h);
        // if (gameBaseRun != null)
        // {
        //     gameBaseRun.AdBannerDidReceiveAd(w, h);
        // }
        if (callbackFinish != null)
        {
            callbackFinish(AdType.BANNER, AdStatus.SUCCESFULL, str);
        }
        AppSceneBase.main.LayoutChild();

    }
    public void AdBannerDidReceiveAdFail(string adsource)
    {


        int type = AdConfigParser.SOURCE_TYPE_BANNER;
        AdInfo info = AdConfig.main.GetNextPriority(type);
        if (info != null)
        {
            AdBanner.InitAd(info.source);
            AdBanner.ShowAd(true);
        }
        else
        {
            if (callbackFinish != null)
            {
                callbackFinish(AdType.BANNER, AdStatus.FAIL, null);
            }
        }
    }

    public void AdBannerDidClick(string adsource)
    {
        Debug.Log("AdBannerDidClick adsource=" + adsource);
        if (adsource == Source.MOBVISTA)
        {
            //点击MobVista banner 弹出视频
            // AdKitCommon.main.ShowAdVideo();
            AdKitCommon.main.ShowAdInsert(100);
        }
    }
    //c++调用c#的回调
    public void AdInsertCallbackUnity(string source, string method)
    {

        if ("AdDidFinish" == method)
        {
            AdInsertWillShow(source);
        }
        if ("AdDidFail" == method)
        {
            AdInsertDidFail(source);
        }
        if ("AdDidClose" == method)
        {
            AdInsertDidClose(source);
        }

    }

    public void AdInsertWillShow(string str)
    {
        //Debug.Log(str);
        // PauseGame(true);

        if (callbackFinish != null)
        {
            callbackFinish(AdType.INSERT, AdStatus.START, null);
        }
    }
    public void AdInsertDidClose(string str)
    {
        //s Debug.Log(str);
        // PauseGame(false);
        if (callbackFinish != null)
        {
            callbackFinish(AdType.INSERT, AdStatus.CLOSE, null);
        }
    }

    public void AdInsertDidFail(string adsource)
    {

        int type = AdConfigParser.SOURCE_TYPE_INSERT;
        AdInfo info = AdConfig.main.GetNextPriority(type);
        if (info != null)
        {
            AdInsert.InitAd(info.source);
            AdInsert.ShowAd();
        }
        else
        {
            if (callbackFinish != null)
            {
                callbackFinish(AdType.INSERT, AdStatus.FAIL, null);
            }
        }

    }

    //AdNative CallBack
    public void AdNativeDidFail(string adsource)
    {
        Debug.Log("AdNativeDidFail adsource=" + adsource);
        if (callbackFinish != null)
        {
            callbackFinish(AdType.NATIVE, AdStatus.FAIL, adsource);
        }
    }
    public void AdNativeDidLoad(string str)
    {
    }


    //c++调用c#的回调
    public void AdVideoCallbackUnity(string source, string method)
    {

        if ("AdDidFinish" == method)
        {
            AdVideoDidFinish(source);
        }
        if ("AdDidFail" == method)
        {
            AdVideoDidFail(source);
        }
        if ("AdDidClose" == method)
        {
            // AdInsertDidClose(source);
        }

    }


    public void AdVideoDidFail(string str)
    {
        int type = AdConfigParser.SOURCE_TYPE_VIDEO;
        AdInfo info = AdConfig.main.GetNextPriority(type);
        if (info != null)
        {
            AdVideo.InitAd(info.source);
            AdVideo.ShowAd();
        }
        else
        {
            if (callbackAdVideoFinish != null)
            {
                callbackAdVideoFinish(AdType.VIDEO, AdStatus.FAIL, null);
            }
        }
    }

    public void AdVideoDidStart(string str)
    {
        AudioPlay.main.Pause();
        if (callbackAdVideoFinish != null)
        {
            callbackAdVideoFinish(AdType.VIDEO, AdStatus.START, null);
        }

    }

    //PlayerPrefs.GetInt can only be called from the main thread
    IEnumerator AdVideoDidFinishMainThread(string str)
    {
        yield return null;
        bool ret = Common.GetBool(AppString.STR_KEY_BACKGROUND_MUSIC);
        if (ret)
        {
            MusicBgPlay.main.PlayMusicBg();
        }

        if (callbackAdVideoFinish != null)
        {
            callbackAdVideoFinish(AdType.VIDEO, AdStatus.SUCCESFULL, str);
        }


    }


    void DoAdVideoDidFinish()
    {
        //win10 微軟視頻廣告 播放結束調用需要在main ui thread 否則會crash
        string str = "advideo";
        bool ret = Common.GetBool(AppString.STR_KEY_BACKGROUND_MUSIC);
        if (ret)
        {
            MusicBgPlay.main.PlayMusicBg();
        }

        if (callbackAdVideoFinish != null)
        {
            callbackAdVideoFinish(AdType.VIDEO, AdStatus.SUCCESFULL, str);
        }
    }
    //Unity多线程（Thread）和主线程（MainThread）交互使用类——Loom工具分享 http://dsqiu.iteye.com/blog/2028503 
    public void AdVideoDidFinish(string str)
    {
        //  isAdVideoFinish = true;
        //PlayerPrefs.GetInt can only be called from the main thread
        // StartCoroutine(AdVideoDidFinishMainThread(str));
        //Invoke("DoAdVideoDidFinish", 0.2f);

        Loom.QueueOnMainThread(() =>
        {
            DoAdVideoDidFinish();

        });
    }

}
