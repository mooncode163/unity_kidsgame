using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;
using Moonma.IAP;
using Moonma.Share;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHomeBase : UIView, IPopViewControllerDelegate
{
    public const string STR_KEYNAME_VIEWALERT_NOAD = "STR_KEYNAME_VIEWALERT_NOAD";
    public const string STR_KEYNAME_VIEWALERT_UPDATE_VERSION = "STR_KEYNAME_VIEWALERT_UPDATE_VERSION";
    public const string STR_KEYNAME_VIEWALERT_EXIT_APP = "STR_KEYNAME_VIEWALERT_EXIT_APP";

    public UIHomeAppCenter uiHomeAppCenter;
    public RawImage imageBg;
    public Image imageBgName;
    public UIText TextName;
    //public GameObject objLayoutTopBar;
    //public Button btnPlay;
    public UIImageText uiPlay;
    // public Button btnSetting;
    // public Button btnMore;
    // public Button btnShare;
    // public Button btnNoAd;
    public Button btnMusic;
    public Button btnSound;
    public Button btnAdVideo;
    public float topBarHeight = 160; 
    public void Init()
    {
        Debug.Log("UIMainBase Init");
        //提前加载
        LevelManager.main.ParsePlaceList();
        if (uiHomeAppCenter != null)
        {
            uiHomeAppCenter.gameObject.SetActive(true);
            if (!AppVersion.appCheckHasFinished)
            {
                uiHomeAppCenter.gameObject.SetActive(false);
            }
            if (Common.isAndroid)
            {
                if ((Config.main.channel == Source.GP))//|| (Config.main.channel == Source.HUAWEI)
                {
                    //GP市场不显示
                    uiHomeAppCenter.gameObject.SetActive(false);
                }
            }
        }
        if (btnAdVideo != null)
        {
            btnAdVideo.gameObject.SetActive(true);
            if ((Common.noad) || (!AppVersion.appCheckHasFinished))
            {
                btnAdVideo.gameObject.SetActive(false);
            }
            if (Common.isAndroid)
            {
                if (Config.main.channel == Source.GP)
                {
                    //GP市场不显示
                    btnAdVideo.gameObject.SetActive(false);
                }
            }
        }

        // CheckAppVersion();

    }

    public void Awake()
    {
        Debug.Log(" UIHomeBase Awake");
        UpdateBtns();
    }

    public void Start()
    {
        Common.UnityStartUpFinish();
    }

    public void UpdateBtns()
    {
        UpdateBtnMusic(btnMusic);
        UpdateBtnSound(btnSound);
    }


    // Update is called once per frame
    public void UpdateBase()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnAppExit();
        }

    }
  

    public override void LayOut()
    {
        base.LayOut();
        float x = 0, y = 0, w = 0, h = 0;
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        if(imageBg!=null)
        {
            RectTransform rctran = imageBg.GetComponent<RectTransform>();
            if (imageBg.texture != null)
            {
                w = imageBg.texture.width;//rectTransform.rect.width;
                h = imageBg.texture.height;//rectTransform.rect.height;
            }

            print("imageBg size:w=" + w + " h=" + h);
            if (w != 0)
            {
                float scalex = sizeCanvas.x / w;
                float scaley = sizeCanvas.y / h;
                float scale = Mathf.Max(scalex, scaley);
                imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);
                //屏幕坐标 现在在屏幕中央
                imageBg.transform.position = new Vector2(Screen.width / 2, Screen.height / 2);
            }

        }

        if (uiHomeAppCenter != null)
        {
            uiHomeAppCenter.LayOut();
        }

    }



    void OnAppExit()
    {
        // if (!MainScene.isInMainUI)
        // {
        //     return;
        // }
        string title = Language.main.GetString(AppString.STR_UIVIEWALERT_TITLE_APP_EXIT);
        string msg = Language.main.GetString(AppString.STR_UIVIEWALERT_MSG_APP_EXIT);
        string yes = Language.main.GetString(AppString.STR_UIVIEWALERT_YES);
        string no = Language.main.GetString(AppString.STR_UIVIEWALERT_NO);
        ViewAlertManager.main.ShowFull(title, msg, yes, no, true, STR_KEYNAME_VIEWALERT_EXIT_APP, OnUIViewAlertFinished);

    }

    void OnAppVersionFinished(AppVersion app)
    {
        //  Debug.Log("OnAppVersionFinished 0");

        if (Common.isAndroid)
        {
            if (Config.main.channel == Source.XIAOMI)
            {
                if (Common.GetDayIndexOfUse() <= AppCommon.NO_APPVERSION_UPDATE_DAYS)
                {
                    return;
                }
            }
        }

        if (AppVersion.main.appNeedUpdate)
        {
            string title = Language.main.GetString(AppString.STR_UIVIEWALERT_TITLE_UPDATE_VERSION);
            string msg = AppVersion.main.strUpdateNote;
            string yes = Language.main.GetString(AppString.STR_UIVIEWALERT_YES);
            string no = Language.main.GetString(AppString.STR_UIVIEWALERT_NO);
            ViewAlertManager.main.ShowFull(title, msg, yes, no, false, STR_KEYNAME_VIEWALERT_UPDATE_VERSION, OnUIViewAlertFinished);
        }
    }
    public void OnUIParentGateDidCloseNoAd(UIParentGate ui, bool isLongPress)
    {
        if (isLongPress)
        {
            DoBtnNoAdAlert();
        }
    }

    void OnUIViewAlertFinished(UIViewAlert alert, bool isYes)
    {
        if (STR_KEYNAME_VIEWALERT_NOAD == alert.keyName)
        {
            if (isYes)
            {
                DoBtnNoADIAP();

            }
        }

        if (STR_KEYNAME_VIEWALERT_EXIT_APP == alert.keyName)
        {
            Debug.Log("OnUIViewAlertFinished 1");
            if (isYes)
            {
                Application.Quit();
            }

        }
        Debug.Log("OnUIViewAlertFinished 2");
        if (STR_KEYNAME_VIEWALERT_UPDATE_VERSION == alert.keyName)
        {
            Debug.Log("OnUIViewAlertFinished 3");
            if (isYes)
            {

                if (AppVersion.main.appNeedUpdate)
                {
                    string url = AppVersion.main.strUrlAppstore;
                    if (!Common.BlankString(url))
                    {
                        Application.OpenURL(url);
                    }
                }

            }
        }
    }

    public void IAPCallBack(string str)
    {
        Debug.Log("IAPCallBack::" + str);
        IAP.main.IAPCallBackBase(str);
    }


    static public void UpdateBtnMusic(Button btn)
    {
        bool ret = Common.GetBool(AppString.STR_KEY_BACKGROUND_MUSIC);
        if (btn != null)
        {
            UIImage imageBg = btn.gameObject.GetComponent<UIButton>().imageBg;
            imageBg.UpdateImage(ret ? ImageRes.main.GetImage("IMAGE_BtnBg") : ImageRes.main.GetImage("IMAGE_BtnBgGrey"),imageBg.keyImage);

            UIImage imageIcon = btn.gameObject.GetComponent<UIButton>().imageIcon;
            imageIcon.UpdateImage(ret ? ImageRes.main.GetImage("IMAGE_BtnMusicIcon") : ImageRes.main.GetImage("IMAGE_BtnMusicIcon"),imageBg.keyImage);

        }
    }
    static public void UpdateBtnSound(Button btn)
    {
        bool ret = Common.GetBool(AppString.KEY_ENABLE_PLAYSOUND);
        Debug.Log(" UIHomeBase UpdateBtnSound ret=" + ret);
        if (btn != null)
        {
            UIImage imageBg = btn.gameObject.GetComponent<UIButton>().imageBg;
            imageBg.UpdateImage(ret ? ImageRes.main.GetImage("IMAGE_BtnBg") : ImageRes.main.GetImage("IMAGE_BtnBgGrey"),imageBg.keyImage);

            string pic = ret ? ImageRes.main.GetImage("IMAGE_BtnSoundIcon") : ImageRes.main.GetImage("IMAGE_BtnSoundIcon");
            Debug.Log(" UIHomeBase UpdateBtnSound  imageIcon.UpdateImage pic=" + pic);
            UIImage imageIcon = btn.gameObject.GetComponent<UIButton>().imageIcon;
            imageIcon.UpdateImage(pic,imageBg.keyImage);
        }
        else
        {
            Debug.Log(" UIHomeBase btn is null ");
        }
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
        UpdateBtnMusic(btnMusic);
    }
    public void OnClickBtnSound()
    {
        bool ret = Common.GetBool(AppString.KEY_ENABLE_PLAYSOUND);
        bool value = !ret;
        Common.SetBool(AppString.KEY_ENABLE_PLAYSOUND, value);

        UpdateBtnSound(btnSound);
    }

    public void OnClickBtnMore()
    {
        MoreViewController.main.Show(null, null);
    }
    public void OnClickBtnSetting()
    {

        // TextureUtil.UpdateImageTexture(imageBg, AppRes.IMAGE_SETTING_BG, true);//IMAGE_SETTING_BG 导致PlayBtnSound声音播放不完整 延迟执行
        // Invoke("DoClickBtnSetting", 0.1f);
        DoClickBtnSetting();
    }

    public void DoClickBtnSetting()
    {
        SettingViewController.main.Show(null, this);
    }

    public void OnClickBtnShare()
    {
        ShareViewController.main.callBackClick = OnUIShareDidClick;
        ShareViewController.main.Show(null, null);
    }

    public void OnClickBtnAdVideo()
    {
        AdKitCommon.main.ShowAdVideo();
    }

    public void OnClickBtnNoAd()
    {
        if (Config.main.APP_FOR_KIDS)
        {
            ParentGateViewController.main.Show(null, null);
            ParentGateViewController.main.ui.callbackClose = OnUIParentGateDidCloseNoAd;
        }
        else
        {
            DoBtnNoAdAlert();
        }
    }


    public void DoBtnNoAdAlert()
    {
        string title = Language.main.GetString("STR_UIVIEWALERT_TITLE_NOAD");
        string msg = Language.main.GetString("STR_UIVIEWALERT_MSG_NOAD");
        string yes = Language.main.GetString("STR_UIVIEWALERT_YES_NOAD");
        string no = Language.main.GetString("STR_UIVIEWALERT_NO_NOAD");

        ViewAlertManager.main.ShowFull(title, msg, yes, no, true, STR_KEYNAME_VIEWALERT_NOAD, OnUIViewAlertFinished);
    }
    public void DoBtnNoADIAP()
    {
        IAP.main.SetObjectInfo(this.gameObject.name, "IAPCallBack");
        IAP.main.StartBuy(IAP.productIdNoAD, false);
    }

    public void OnUIShareDidClick(ItemInfo item)
    {
        string title = Language.main.GetString("UIMAIN_SHARE_TITLE");
        string detail = Language.main.GetString("UIMAIN_SHARE_DETAIL");
        string url = Config.main.shareAppUrl;
        Share.main.ShareWeb(item.source, title, detail, url);
    }

    public void OnPopViewControllerDidClose(PopViewController controller)
    {
        UpdateBtnMusic(btnMusic);
        UpdateBtnSound(btnSound);
    }
}

