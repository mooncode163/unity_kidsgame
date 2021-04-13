using System.Collections;
using System.Collections.Generic;
using LitJson;
using Moonma.Share;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameBase : UIView
{

    public const int GAME_AD_INSERT_SHOW_STEP = 1;
    public const string STR_KEYNAME_VIEWALERT_USER_GUIDE = "keyname_viewalert_user_guide";
    public const string STR_KEYNAME_VIEWALERT_GAME_FINISH = "keyname_viewalert_game_finish";
    public const string STR_KEYNAME_VIEWALERT_GOLD = "keyname_viewalert_gold";
    public const string STR_KEYNAME_VIEWALERT_ADVIDEO_FAIL = "keyname_viewalert_advideo_fail";
    //public AudioClip audioClipBtn;
    public Button btnShare;
    public Button btnMusic;
    //static public List<object> listGuanka;
    static public Language languageGame;
    //static public int heightAdBanner;
    //static public float heightAdBannerWorld;
    public HttpRequest httpReqLanguage;
    private int _gameMode;


    public OnInitUIFinishDelegate callbackGameInitUIFinish { get; set; }

    //public Camera mainCamera;


    static public int gameMode;//已经通关 


    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnClickBtnBack();
        }
    }
    public void Init()
    {
        Debug.Log("UIGameBase Init");

        if (btnShare != null)
        {
            btnShare.gameObject.SetActive(Config.main.isHaveShare);
        }
    }




    void ShowShare()
    {
        Debug.Log("gamebase showshare");
        ShareViewController.main.callBackClick = OnUIShareDidClick;
        ShareViewController.main.Show(null, null);
    }
    public void ShowAdVideoFailAlert()
    {
        string title = Language.main.GetString("STR_UIVIEWALERT_TITLE_ADVIDEO_FAIL");
        string msg = Language.main.GetString("STR_UIVIEWALERT_MSG_ADVIDEO_FAIL");
        string yes = Language.main.GetString("STR_UIVIEWALERT_YES_ADVIDEO_FAIL");
        string no = "no";
        ViewAlertManager.main.ShowFull(title, msg, yes, no, false, STR_KEYNAME_VIEWALERT_ADVIDEO_FAIL, null);

    }


    public void ShowParentGate(OnUIParentGateDidCloseDelegate callbackClose)
    {
        ParentGateViewController.main.Show(null, null);
        ParentGateViewController.main.ui.callbackClose = callbackClose;
    }

    public void ShowAdInsert(int step, bool isAlwasy)
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
            // if ((LevelManager.main.gameLevel != 0) && ((LevelManager.main.gameLevel % _step) == 0))
            if ((LevelManager.main.gameLevel % _step) == 0)
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

    public virtual void OnUIShareDidClick(ItemInfo item)
    {
        string title = Language.main.GetString("UIGAME_SHARE_TITLE");
        string detail = Language.main.GetString("UIGAME_SHARE_DETAIL");
        string url = Config.main.shareAppUrl;
        Share.main.ShareWeb(item.source, title, detail, url);
    }

    // public virtual int GetPlaceTotal()
    // {
    //     return 0;
    // }  


    public virtual void UpdateGuankaLevel(int level)
    {
        UpdateLanguage();
        AdKitCommon.main.callbackFinish = OnAdKitFinish;
        AppSceneBase.main.ClearMainWorld();
    }
    public virtual void UpdatePlaceLevel(int level)
    {
    }

    public virtual void PreLoadDataForWeb()
    {
    }

    public void LayoutChildBase()
    {
        // if (objSpriteBg != null)
        // {
        //     SpriteRenderer render = objSpriteBg.GetComponent<SpriteRenderer>();
        //     Vector2 worldsize = Common.GetWorldSize(AppSceneBase.main.mainCamera);
        //     float w = render.size.x;//rectTransform.rect.width;
        //     float h = render.size.y;//rectTransform.rect.height;
        //     float scalex = worldsize.x / w;
        //     float scaley = worldsize.y / h;
        //     float scale = Mathf.Max(scalex, scaley);
        //     objSpriteBg.transform.localScale = new Vector3(scale, scale, 1.0f);

        // }
        AppSceneBase.main.LayOut();
    }

    public void UpdateLanguage()
    {
        ItemInfo info = LevelManager.main.GetPlaceItemInfo(LevelManager.main.placeLevel);
        if(info==null){
            return;
        }
        string strlan = CloudRes.main.rootPathGameRes +"/language/" + info.language + ".csv";
        languageGame = new Language();
        languageGame.Init(strlan);
        languageGame.SetLanguage(Language.main.GetLanguage());


    }
    public void UpdateBtnMusic()
    { 
        UIHomeBase.UpdateBtnMusic(btnMusic);
    }

    public void OnGameWinBase()
    {
        ShowAdInsert(GAME_AD_INSERT_SHOW_STEP, false);
    }

    public void OnGameWinBase2(bool isAlwasyShowAd)
    {
        ShowAdInsert(GAME_AD_INSERT_SHOW_STEP, isAlwasyShowAd);
    }
    public void OnGameFailBase()
    {
    }
    public void OnClickBtnMusic()
    {
        bool ret = Common.GetBool(AppString.STR_KEY_BACKGROUND_MUSIC);
        bool value = !ret;
        Common.SetBool(AppString.STR_KEY_BACKGROUND_MUSIC, value);
        if (value)
        {
            MusicBgPlay.main.PlayMusicBg();
        }
        else
        {
            MusicBgPlay.main.Stop();
        }
        UpdateBtnMusic();
    }
    public void OnClickBtnShare()
    {
        ShowShare();
    }


    public virtual void OnClickBtnBack()
    {
        // PopViewController pop = (PopViewController)this.controller;
        // if (pop != null)
        // {
        //     pop.Close();
        // }
        Debug.Log("GameBase:OnClickBtnBack");
        NaviViewController navi = this.controller.naviController;
        if (navi != null)
        {
            navi.Pop();
        }
        // ShowAdInsert(GAME_AD_INSERT_SHOW_STEP);
    }


    public void OnAdKitFinish(AdKitCommon.AdType type, AdKitCommon.AdStatus status, string str)
    {
        OnGameAdKitFinish(type, status, str);
    }

    public virtual void OnGameAdKitFinish(AdKitCommon.AdType type, AdKitCommon.AdStatus status, string str)
    {
        if (type == AdKitCommon.AdType.BANNER)
        {
            if (status == AdKitCommon.AdStatus.SUCCESFULL)
            {
                int w = 0;
                int h = 0;
                int idx = str.IndexOf(":");
                string strW = str.Substring(0, idx);
                int.TryParse(strW, out w);
                string strH = str.Substring(idx + 1);
                int.TryParse(strH, out h);
                Debug.Log("OnGameAdKitFinish AdBannerDidReceiveAd::w=" + w + " h=" + h);

                Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
                GameManager.main.heightAdScreen = h + Device.heightSystemHomeBar;
                GameManager.main.heightAdWorld = Common.ScreenToWorldHeight(mainCam, h);
                GameManager.main.heightAdCanvas = Common.ScreenToCanvasHeigt(sizeCanvas, h);
            }

            if (status == AdKitCommon.AdStatus.FAIL)
            {

            }

            LayOut();
        }
    }

    public void PlaySoundFromResource(string file)
    {
        GameObject audioPlayer = GameObject.Find("AudioPlayer");
        if (audioPlayer != null)
        {
            AudioSource audioSource = audioPlayer.GetComponent<AudioSource>();
            AudioClip audioClip = AudioCache.main.Load(file);
            audioSource.PlayOneShot(audioClip);
        }
    }


}
