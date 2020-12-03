using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameViewController : PopViewController
{

    public const string STR_KEYNAME_VIEWALERT_COMMENT = "STR_KEYNAME_VIEWALERT_COMMENT";
    public UIGameBase gameBasePrefab;
    public UIAdBanner uiAdBannerPrefab;
    public UIAdBanner uiAdBanner;
    static private bool isShowComment = false;
    static public string gameType = Common.appType;
    static public string dirRootPrefab = "AppCommon/Prefab/Game";
    string dirRootPrefabApp = "App/Prefab/Game";
    static private GameViewController _main = null;
    public static GameViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new GameViewController();
                _main.Init();
            }
            return _main;
        }
    }


    static private UIGameBase _gameBase = null;
    public UIGameBase gameBase
    {
        get
        {
            if (_gameBase == null)
            {
                return main.gameBasePrefab;
            }
            return _gameBase;
        }
    }

    void Init()
    {
        LoadGame();
        LoadUIAdBanner();
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        Debug.Log("UIGameController.ViewDidLoad");

        CreateUI();
    }


    public override void ViewDidUnLoad()
    {
        base.ViewDidUnLoad();
        Debug.Log("UIGameController.ViewDidUnLoad");
        AppSceneBase.main.ClearMainWorld();
        AdKitCommon.main.ShowAdBanner(false);
        DestroyUIAdBanner();
    }
    public override void LayOutView()
    {
        base.LayOutView();
        Debug.Log("GameViewController LayOutView");
        if ((gameBasePrefab != null) && (_gameBase != null))
        {
            UIViewController.ClonePrefabRectTransform(gameBasePrefab.gameObject, _gameBase.gameObject);
            _gameBase.LayOut();
        }

    }
    public void CreateUI()
    {

        GotoGame(gameType);
    }

    static public string GetGamePrefabName()
    {
        string name = gameType;
        //首字母大写
        // string strFirst = name.Substring(0, 1);
        // strFirst.ToUpper();
        // string strOther = name.Substring(1);
        // name = "Game" + strFirst + strOther;

        //Resources.Load 文件可以不区分大小写字母
        name = "UIGame" + name;

        return name;
    }
    void LoadGame()
    {
        GameObject obj = null;
        string name = gameType;
        string strPrefab = dirRootPrefabApp + "/" + GetGamePrefabName();
        obj = PrefabCache.main.Load(strPrefab);
        if (obj == null)
        {
            strPrefab = dirRootPrefab + "/" + GetGamePrefabName();
            if (!Common.isBlankString(GameManager.main.pathGamePrefab))
            {
                strPrefab = GameManager.main.pathGamePrefab;
            }
            Debug.Log("strPrefab=" + strPrefab);
            //Resources.Load 文件可以不区分大小写字母

            // if (!Device.isLandscape)
            // {
            //     // string strPrefab_shu = strPrefab + "_shu";
            //     // GameObject objShu = PrefabCache.main.Load(strPrefab_shu);
            //     // if (objShu != null)
            //     // {
            //     //     obj = objShu;
            //     // }
            //     // else
            //     {
            //         obj = PrefabCache.main.Load(strPrefab);
            //     }
            // }
            // else
            {
                obj = PrefabCache.main.Load(strPrefab);
            }
        }


        if (obj == null)
        {
            Debug.Log("LoadGame obj is null");
        }


        if (obj != null)
        {
            gameBasePrefab = obj.GetComponent<UIGameBase>();
        }

        //gameBasePrefab.LoadPrefab();
    }

    public bool EnableUIAdBanner()
    {
        if (!AdKitCommon.main.enableBanner)
        {
            return false;
        }
        if (Application.isEditor)
        {
            //编辑器
            return true;
        }

        if (Common.isMonoPlayer)
        {
            return false;
        }

        bool ret = false;

        if (Common.isiOS && !AppVersion.appCheckHasFinished)
        {
            //ios 审核中
            ret = true;
        }
        if (Common.isRemoveAd)
        {
            ret = false;
        }
        return ret;
    }

    void LoadUIAdBanner()
    {
        if (!EnableUIAdBanner())
        {
            return;
        }
        GameObject obj = PrefabCache.main.Load(UIAdBanner.PREFAB_UIAdBanner);
        if (obj != null)
        {
            uiAdBannerPrefab = obj.GetComponent<UIAdBanner>();
        }
    }

    void DestroyUIAdBanner()
    {
        if (!EnableUIAdBanner())
        {
            return;
        }
        if (uiAdBanner != null)
        {
            GameObject.Destroy(uiAdBanner.gameObject);
            uiAdBanner = null;
        }
    }

    public void OnRemoveAd()
    {
        if (uiAdBanner != null)
        {
            uiAdBanner.OnClickBtnClose();
        }
        DestroyUIAdBanner();
    }


    void GotoGame(string name)
    {
        Debug.Log("GotoGame:" + name);
        _gameBase = (UIGameBase)GameObject.Instantiate(gameBasePrefab);
        //_gameBase.mainCamera = mainCamera;
        // _gameBaseRun.mainCamera = mainCamera;
        _gameBase.Init();
        _gameBase.SetController(this);
        UIViewController.ClonePrefabRectTransform(gameBasePrefab.gameObject, _gameBase.gameObject);

        RectTransform rctranParent = objController.transform.GetComponent<RectTransform>();

        RectTransform rctran = _gameBase.GetComponent<RectTransform>();
        float x, y, w, h;
        float adBannerHeight = 160f;
        adBannerHeight = 0;
        //poivt (0.5,0.5)
        // w = rctranParent.rect.width;
        // h = rctranParent.rect.height-adBannerHeight;
        // x = 0;
        // y = adBannerHeight/2;
        // rctran.sizeDelta = new Vector2(w,h);
        // rctran.anchoredPosition = new Vector2(x,y);

        // Vector2 pt = rctran.offsetMin;
        // pt.y = adBannerHeight;
        // if (gameType == AppType.NONGCHANG)
        // {
        //     pt.y = 0;
        // }
        // rctran.offsetMin = pt;

        AppSceneBase.main.UpdateMainWorldRect(adBannerHeight);

        //显示横幅广告
        AdKitCommon.main.InitAdBanner();
        if (EnableUIAdBanner())
        {
            uiAdBanner = (UIAdBanner)GameObject.Instantiate(uiAdBannerPrefab);
            uiAdBanner.SetViewParent(AppSceneBase.main.canvasMain.gameObject);
            UIViewController.ClonePrefabRectTransform(uiAdBannerPrefab.gameObject, uiAdBanner.gameObject);
        }


        ShowUserComment();

    }


    void ShowUserComment()
    {
        if (Application.isEditor)
        {
            return;
        }
        if (!AppVersion.appCheckHasFinished)
        {
            return;
        }
        if (isShowComment)
        {
            return;
        }

        // if (Common.GetDayIndexOfUse() <= AppCommon.NO_USER_COMMENT_DAYS)
        // {
        //     return;
        // }

        string pkey = AppString.STR_KEY_COMMENT_VERSION + Common.GetAppVersion();
        bool isshowplay = Common.GetBool(pkey);
        if (isshowplay == true)
        {
            return;
        }

        int day_cur = Common.GetDayIndexOfUse();
        int day_last = PlayerPrefs.GetInt(AppString.STR_KEY_COMMENT_LAST_TIME);
        int day_step = Mathf.Abs(day_cur - day_last);



        //隔几天弹评论
        if (day_step >= AppCommon.USER_COMMENT_DAY_STEP)
        {
            string title = Language.main.GetString(AppString.STR_UIVIEWALERT_TITLE_USERCOMMENT);
            string msg = Language.main.GetString(AppString.STR_UIVIEWALERT_MSG_USERCOMMENT);
            string yes = Language.main.GetString(AppString.STR_UIVIEWALERT_YES_USERCOMMENT);
            string no = Language.main.GetString(AppString.STR_UIVIEWALERT_NO_USERCOMMENT);

            ViewAlertManager.main.ShowFull(title, msg, yes, no, true, STR_KEYNAME_VIEWALERT_COMMENT, OnUIViewAlertFinished);
        }

    }
    void OnUIViewAlertFinished(UIViewAlert alert, bool isYes)
    {

        if (STR_KEYNAME_VIEWALERT_COMMENT == alert.keyName)
        {
            if (isYes)
            {
                string url = AppVersion.main.strUrlComment;
                if (!Common.BlankString(url))
                {
                    isShowComment = true;
                    string pkey = AppString.STR_KEY_COMMENT_VERSION + Common.GetAppVersion();
                    Common.SetBool(pkey, true);
                    int day = Common.GetDayIndexOfUse();
                    PlayerPrefs.SetInt(AppString.STR_KEY_COMMENT_LAST_TIME, day);
                    Application.OpenURL(url);
                }
            }
        }

    }

}
