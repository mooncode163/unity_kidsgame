using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Moonma.IAP;
using Moonma.Share;
public class UIHomeTopBar : UIView
{

    public const string STR_KEYNAME_VIEWALERT_NOAD = "STR_KEYNAME_VIEWALERT_NOAD";
    public const string STR_KEYNAME_VIEWALERT_LOADING = "STR_KEYNAME_VIEWALERT_LOADING";


    public Button btnNoAd;
    public Button btnRestoreIAP;
    public Button btnShare;
    public Button btnSetting;
    public Button btnMore;
    public Button btnAdVideo;
    public Button btnMusic;
    public Button btnSound;
    public Button btnLearn;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (btnShare != null)
        {
            btnShare.gameObject.SetActive(Config.main.isHaveShare);
        }
        if (btnNoAd != null)
        {
            btnNoAd.gameObject.SetActive(Config.main.isHaveRemoveAd);
        }
        if (btnMore != null)
        {
            if (!AppVersion.appCheckHasFinished)
            {
                btnMore.gameObject.SetActive(false);
            }
            if (Common.isAndroid)
            {
                if ((Config.main.channel == Source.HUAWEI) || (Config.main.channel == Source.GP))
                {
                    //华为市场不显示
                    btnMore.gameObject.SetActive(false);
                }
            }
            if (Common.isWinUWP)
            {
                btnMore.gameObject.SetActive(false);
            }
            if (Application.isEditor)
            {
                btnMore.gameObject.SetActive(true);
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

        UIHomeBase.UpdateBtnMusic(btnMusic);
        UpdateBtnSound();
    }
    // Use this for initialization
    void Start()
    {
        LayOut();
    }

    // Update is called once per frame
    void Update()
    {

    }

   public override void LayOut()
    {
        base.LayOut();
        Vector2 sizeCanvas = this.frame.size;
        float x = 0, y = 0;

    }
    void ShowShare()
    {
        ShareViewController.main.callBackClick = OnUIShareDidClick;
        ShareViewController.main.Show(null, null);
    }




    public void UpdateBtnSound()
    {
        UIHomeBase.UpdateBtnSound(btnSound);
    }

    public void OnUIShareDidClick(ItemInfo item)
    {
        string title = Language.main.GetString("UIMAIN_SHARE_TITLE");
        string detail = Language.main.GetString("UIMAIN_SHARE_DETAIL");
        string url = Config.main.shareAppUrl;
        Share.main.ShareWeb(item.source, title, detail, url);
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

    }


    public void OnUIParentGateDidCloseShop(UIParentGate ui, bool isLongPress)
    {
        if (isLongPress)
        {
            ShowShop();
        }
    }

    public void OnUIParentGateDidCloseNoAd(UIParentGate ui, bool isLongPress)
    {
        if (isLongPress)
        {
            DoBtnNoAdAlert();
        }
    }

    public void OnUIParentGateDidCloseRestoreIAP(UIParentGate ui, bool isLongPress)
    {
        if (isLongPress)
        {
            DoBtnRestoreIAP();
        }
    }
    public void OnUIParentGateDidCloseShare(UIParentGate ui, bool isLongPress)
    {
        if (isLongPress)
        {
            DoBtnShare();
        }
    }
    public void IAPCallBack(string str)
    {
        Debug.Log("IAPCallBack::" + str);
        IAP.main.IAPCallBackBase(str);
    }



    void ShowPay()
    {

    }

    void ShowShop()
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
        UIHomeBase.UpdateBtnMusic(btnMusic);
    }

    public void OnClickBtnSound()
    {
        bool ret = Common.GetBool(AppString.KEY_ENABLE_PLAYSOUND);
        bool value = !ret;
        Common.SetBool(AppString.KEY_ENABLE_PLAYSOUND, value);
        UpdateBtnSound();
    }
    public void OnClickBtnAdVideo()
    {
        AdKitCommon.main.ShowAdVideo();
    }


    public void OnClickBtnSetting()
    {
        SettingViewController.main.Show(AppSceneBase.main.rootViewController, null);
    }


    public void OnClickBtnMore()
    {
        // if (audioSource == null)
        // {
        //     //AudioPlayer对象在场景切换后可能从当前scene移除了
        //     GameObject audioPlayer = GameObject.Find("AudioPlayer");
        //     audioSource = audioPlayer.GetComponent<AudioSource>();
        // }
        // audioSource.PlayOneShot(audioClipBtnPlay);

        MoreViewController.main.Show(AppSceneBase.main.rootViewController, null);

    }

    public void OnClickBtnLearn()
    {
        // LearnViewController.main.Show(AppSceneBase.main.rootViewController, null);

    }


    public void OnClickBtnNoADIAP()
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
        // viewAlert.ShowBtnNo(false);
        // viewAlert.keyName = STR_KEYNAME_VIEWALERT_LOADING;
        // viewAlert.callback = OnUIViewAlertFinished;
        // string title = Language.main.GetString(AppString.STR_UIVIEWALERT_TITLE_SHOP_START_BUY);
        // string msg = Language.main.GetString(AppString.STR_UIVIEWALERT_MSG_SHOP_START_BUY);
        // string yes = Language.main.GetString(AppString.STR_UIVIEWALERT_YES_SHOP_START_BUY);
        // string no = Language.main.GetString(AppString.STR_UIVIEWALERT_YES_SHOP_START_BUY);
        // viewAlert.SetText(title, msg, yes, no);
        // viewAlert.Show();
        IAP.main.StartBuy(IAP.productIdNoAD, false);


    }

    public void OnClickBtnRestoreIAP()
    {
        if (Config.main.APP_FOR_KIDS && !Application.isEditor)
        {
            ParentGateViewController.main.Show(null, null);
            ParentGateViewController.main.ui.callbackClose = OnUIParentGateDidCloseRestoreIAP;
        }
        else
        {
            DoBtnRestoreIAP();
        }

    }

    //恢复内购
    public void DoBtnRestoreIAP()
    {
        IAP.main.SetObjectInfo(this.gameObject.name, "IAPCallBack");
        // viewAlert.ShowBtnNo(false);
        // viewAlert.keyName = STR_KEYNAME_VIEWALERT_LOADING;
        // viewAlert.callback = OnUIViewAlertFinished;
        // string title = Language.main.GetString(AppString.STR_UIVIEWALERT_TITLE_SHOP_RESTORE_BUY);
        // string msg = Language.main.GetString(AppString.STR_UIVIEWALERT_MSG_SHOP_RESTORE_BUY);
        // string yes = Language.main.GetString(AppString.STR_UIVIEWALERT_YES_SHOP_RESTORE_BUY);
        // string no = Language.main.GetString(AppString.STR_UIVIEWALERT_YES_SHOP_RESTORE_BUY);
        // viewAlert.SetText(title, msg, yes, no);
        // viewAlert.Show();
        IAP.main.RestoreBuy(IAP.productIdNoAD);


    }


    public void OnClickBtnShare()
    {
        if (Config.main.APP_FOR_KIDS)
        {
            ParentGateViewController.main.Show(null, null);
            ParentGateViewController.main.ui.callbackClose = OnUIParentGateDidCloseShare;
        }
        else
        {
            DoBtnShare();
        }

    }

    public void DoBtnShare()
    {
        ShowShare();
    }


}
