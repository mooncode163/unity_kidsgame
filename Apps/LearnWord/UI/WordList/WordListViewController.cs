using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moonma.AdKit.AdInsert;

public class WordListViewController : UIViewController
{
    public const int RUN_COUNT_SHOW_AD = 2;
    UIWordList uiPrefab;
    UIWordList ui;

    public static bool isAdVideoHasFinish = false;
    public static int runCount = 0;

    static private WordListViewController _main = null;
    public static WordListViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new WordListViewController();
                _main.Init();
            }
            return _main;
        }
    }

    void Init()
    {
        string strPrefab = "App/Prefab/WordList/UIWordList";
        GameObject obj = PrefabCache.main.Load(strPrefab);
        uiPrefab = obj.GetComponent<UIWordList>();
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        CreateUI();
        Debug.Log("HomeViewCon)troller ViewDidLoad");
  
         // 开机广告
        //if ((!isAdVideoHasFinish) && (runCount >= RUN_COUNT_SHOW_AD) && (!GameManager.main.isShowGameAdInsert))
        if (runCount == 0)
        {
            //至少在home界面显示一次视频广告
            //AdKitCommon.main.callbackAdVideoFinish = OnAdKitAdVideoFinish;
            //   if (uiHome != null)
            // {
            //     uiHome.OnClickBtnAdVideo();
            // }


            AdKitCommon.main.callbackFinish = OnAdKitCallBack;
            if (Common.isiOS)
            {
                //原生开机插屏
                AdKitCommon.main.ShowAdNativeSplash(Source.ADMOB);
            }
            else
            {
                //至少在home界面显示一次开机插屏  
                ShowAdInsert();

            }

        } 


        runCount++;

        GameManager.main.ShowPrivacy();
    }
    public override void ViewDidUnLoad()
    {
        base.ViewDidUnLoad();
        Debug.Log("UIWordList ViewDidUnLoad");
    }
    public override void LayOutView()
    {
        base.LayOutView();
        Debug.Log("UIWordList LayOutView ");
        if (ui != null)
        {
            ui.LayOut();
        }
    }

    public void CreateUI()
    {
        if (this.naviController != null)
        {
            this.naviController.HideNavibar(true);
        }
        ui = (UIWordList)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
        // ui.Init();
    }
 
    
    void ShowAdInsert()
    {
        if (Config.main.channel == Source.HUAWEI)
        {
            // return;
        }
        string source = Source.GDT;//GDT
        if (Common.isiOS)
        {
            // source = Source.CHSJ;
        }
        AdInsert.InitAd(source);
        AdKitCommon.main.ShowAdInsert(100);
    }
    public void OnAdKitCallBack(AdKitCommon.AdType type, AdKitCommon.AdStatus status, string str)
    {
        if (type == AdKitCommon.AdType.NATIVE)
        {
            if (status == AdKitCommon.AdStatus.FAIL)
            {
                ShowAdInsert();
            }
        }
    }
    public void OnAdKitAdVideoFinish(AdKitCommon.AdType type, AdKitCommon.AdStatus status, string str)
    {
        //if (type == AdKitCommon.AdType.VIDEO)
        {
            if (status == AdKitCommon.AdStatus.SUCCESFULL)
            {
                isAdVideoHasFinish = true;
            }
        }
    }
}
