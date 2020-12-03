using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using LitJson;
public delegate void OnAppVersionFinishedDelegate(AppVersion app);
public delegate void OnAppVersionUpdateDelegate(AppVersion app);

public class AppVersion
{
    //包名	商店
    //com.android.vending	Google Play
    //com.tencent.android.qqdownloader	应用宝
    //com.qihoo.appstore	360手机助手
    //com.baidu.appsearch	百度手机助
    //com.xiaomi.market	小米应用商店
    //com.wandoujia.phoenix2	豌豆荚
    //com.huawei.appmarket	华为应用市场
    //com.taobao.appcenter	淘宝手机助手
    //com.hiapk.marketpho	安卓市场
    public const string PACKAGE_APPSTORE_HUAWEI = "com.huawei.appmarket";
    public const string PACKAGE_APPSTORE_XIAOMI = "com.xiaomi.market";
    public const string PACKAGE_APPSTORE_GP = "com.android.vending";
    public const string PACKAGE_APPSTORE_TAPTAP = "com.taptap.market";


    public const string STRING_KEY_APP_CHECK_FINISHED = "app_check_finished";
    static private AppVersion _main = null;
    public static AppVersion main
    {
        get
        {
            if (_main == null)
            {
                _main = new AppVersion();
                _main.Init();
            }
            return _main;
        }
    }

    AppVersionBase appVersionBase;




    public static bool appCheckForXiaomi = false;//xiao app审核中
    static public bool appCheckHasFinished
    //app审核完成
    {
        get
        {

            if (Application.isEditor)
            {
                //编辑器
                return true;
            }
            if (Common.isMonoPlayer)//isPC
            {
                return false;
            }
            bool ret = Common.Int2Bool(PlayerPrefs.GetInt(STRING_KEY_APP_CHECK_FINISHED));

            if (Common.isAndroid)
            {
                if (Config.main.channel == Source.TAPTAP)
                {
                    return true;
                }
                if (Config.main.channel == Source.HUAWEI)
                {
                    // if ((Common.GetAppVersion() == "1.0.0") || (Common.GetAppVersion() == "1.0.1") || (Common.GetAppVersion() == "1.0.2"))
                    // {
                    //     return false;
                    // }
                    // if (!ret)
                    // {
                    //     if (Common.GetDayIndexOfUse() <= 1)
                    //     {
                    //         return false;
                    //     }
                    // }

                    // return true;
                }
                // if (!IPInfo.isInChina)
                // {
                //     //android 国外 直接当作 审核通过
                //   //  return true;
                // }
            }

            if (ret)
            {
                Debug.Log("appCheckHasFinished:ret=true");
            }
            else
            {

                Debug.Log("appCheckHasFinished:ret=false");
            }
            return ret;
        }
    }

    static public bool appForPad//
    {
        get
        {
            string str = Common.GetAppPackage();
            bool ret = false;
            if (str.Contains(".pad") || str.Contains(".ipad"))
            {
                ret = true;
            }
            if (Common.isWinUWP)
            {
                if (Common.GetAppName().ToLower().Contains("hd"))
                {
                    ret = true;
                }

            }

            return ret;
        }
    }

    public bool appNeedUpdate//
    {
        get
        {
            bool ret = false;
            if (appVersionBase != null)
            {
                ret = appVersionBase.appNeedUpdate;
            }
            return ret;
        }
    }

    public string strUpdateNote
    {
        get
        {
            string ret = "";
            if (appVersionBase != null)
            {
                ret = appVersionBase.strUpdateNote;
            }
            return ret;
        }
    }

    public string strUrlComment
    {
        get
        {
            string ret = "";
            if (appVersionBase != null)
            {
                ret = appVersionBase.strUrlComment;
            }
            return ret;
        }
    }
    public string strUrlAppstore
    {
        get
        {
            string ret = "";
            if (appVersionBase != null)
            {
                ret = appVersionBase.strUrlAppstore;
            }
            return ret;
        }
    }




    public OnUICommentDidClickDelegate callBackCommentClick { get; set; }

    public OnAppVersionFinishedDelegate callbackFinished { get; set; }



    void Init()
    {
        Debug.Log("AppVersion Init");
        // appNeedUpdate = false;
        // isFirstCreat = false;
        // appCheckForAppstore = false;

    }


    public string GetUrlOfComment(string source)
    {
        string url = "";
        string strappid = Config.main.appId;
        switch (source)
        {
            case Source.APPSTORE:
                {
                    url = "https://itunes.apple.com/cn/app/id" + strappid;
                    if (!IPInfo.isInChina)
                    {
                        url = "https://itunes.apple.com/us/app/id" + strappid;
                    }
                }
                break;
            case Source.TAPTAP:
                {
                    url = "https://www.taptap.com/app/" + strappid + "/review";
                }
                break;
            case Source.XIAOMI:
                {
                    url = "http://app.xiaomi.com/details?id=" + Common.GetAppPackage();
                }
                break;
            case Source.HUAWEI:
                {
                    //http://appstore.huawei.com/app/C100270155
                    url = "http://appstore.huawei.com/app/C" + strappid;
                }
                break;


        }
        return url;
    }


    public void OnUICommentDidClick(ItemInfo item)
    {
        if (callBackCommentClick != null)
        {
            callBackCommentClick(item);
        }
    }

    public void OnComment()
    {
        //if (Common.isAndroid)
        if (Config.main.listAppStore.Count > 1)
        {
            CommentViewController.main.callBackClick = OnUICommentDidClick;
            CommentViewController.main.Show(null, null);
            return;
        }
        ItemInfo info = Config.main.listAppStore[0];
        DoComment(info);
    }
    public void GotoComment()
    {
        string appstorePackage = "";
        string appstore = Source.APPSTORE;
        if (Common.isAndroid)
        {
            if (Config.main.channel == Source.TAPTAP)
            {
                appstore = Source.TAPTAP;
                appstorePackage = AppVersion.PACKAGE_APPSTORE_TAPTAP;
            }

            if (Config.main.channel == Source.XIAOMI)
            {
                appstore = Source.XIAOMI;
                appstorePackage = AppVersion.PACKAGE_APPSTORE_XIAOMI;
            }
            if (Config.main.channel == Source.HUAWEI)
            {
                appstore = Source.HUAWEI;
                appstorePackage = AppVersion.PACKAGE_APPSTORE_HUAWEI;
            }

        }

        GotoToAppstoreApp(appstore, Common.GetAppPackage(), appstorePackage, GetUrlOfComment(appstore));
    }

    //https://blog.csdn.net/pz789as/article/details/78223517
    //跳转到appstore写评论
    public void GotoToAppstoreApp(string appstore, string appPackage, string marketPkg, string url)
    {
        Debug.Log("GotoToAppstoreApp appstore=" + appstore + " appPackage=" + appPackage + " marketPkg=" + marketPkg + " url" + url);
        if (Common.isAndroid)
        {
            if (appstore != Source.TAPTAP)
            {
                AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
                AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
                intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_VIEW"));
                AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
                AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "market://details?id=" + appPackage);
                intentObject.Call<AndroidJavaObject>("setData", uriObject);
                intentObject.Call<AndroidJavaObject>("setPackage", marketPkg);
                AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
                currentActivity.Call("startActivity", intentObject);

                return;
            }

        }


        if (!Common.BlankString(url))
        {
            Application.OpenURL(url);
        }

    }
    public void DoComment(ItemInfo info)
    {
        string strappid = Config.main.GetAppIdOfStore(info.source);
        string strUrlComment = "";
        string appstorePackage = "";
        switch (info.source)
        {
            case Source.APPSTORE:
                {
                    strUrlComment = "https://itunes.apple.com/cn/app/id" + strappid;
                    if (!IPInfo.isInChina)
                    {
                        strUrlComment = "https://itunes.apple.com/us/app/id" + strappid;
                    }
                }
                break;
            case Source.TAPTAP:
                {
                    strUrlComment = "https://www.taptap.com/app/" + strappid + "/review";
                    appstorePackage = PACKAGE_APPSTORE_TAPTAP;
                }
                break;
            case Source.XIAOMI:
                {
                    strUrlComment = "http://app.xiaomi.com/details?id=" + Common.GetAppPackage();
                    appstorePackage = PACKAGE_APPSTORE_XIAOMI;
                }
                break;
            case Source.HUAWEI:
                {
                    //http://appstore.huawei.com/app/C100270155
                    strUrlComment = "http://appstore.huawei.com/app/C" + strappid;
                    appstorePackage = PACKAGE_APPSTORE_HUAWEI;
                }
                break;


        }



        string url = strUrlComment;
        if (!Common.BlankString(url))
        {
            OnUICommentDidClick(null);
            Debug.Log("strUrlComment::" + url);
        }
        else
        {
            Debug.Log("strUrlComment is Empty");
        }
        GotoToAppstoreApp(info.source, Common.GetAppPackage(), appstorePackage, url);

    }
    public void StartParseVersion()
    {
        //   startParserVersionXiaomi();
        //   return;
        //android
        if (Common.isAndroid)
        {
            switch (Config.main.channel)
            {
                case Source.XIAOMI:
                    {
                        appVersionBase = new AppVersionXiaomi();
                        break;
                    }
                case Source.TAPTAP:
                    {
                        appVersionBase = new AppVersionTaptap();
                        break;
                    }
                case Source.HUAWEI:
                    {
                        appVersionBase = AppVersionHuawei.main;
                        break;
                    }
                case Source.GP:
                    {
                        appVersionBase = new AppVersionGP();
                        break;
                    }
                default:
                    {
                        appVersionBase = new AppVersionBase();
                    }
                    break;

            }

        }
        else if (Common.isiOS)
        {
            appVersionBase = new AppVersionAppstore();
        }

        else if (Common.isWinUWP)
        {
            appVersionBase = new AppVersionWin();
        }
        else
        {
            appVersionBase = new AppVersionBase();
        }

        //appVersionBase = new AppVersionXiaomi();

        appVersionBase.callbackFinished = OnAppVersionBaseFinished;
        appVersionBase.Init();
        appVersionBase.StartParseVersion();
    }



    public void OnAppVersionBaseFinished(AppVersionBase app)
    {
        if (this.callbackFinished != null)
        {
            this.callbackFinished(this);
        }
    }




}
