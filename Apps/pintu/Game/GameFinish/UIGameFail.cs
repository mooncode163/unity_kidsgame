using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIGameFail : UIGameWinBase
{
    public const string KEYNAME_VIEWALERT_VIDEO_FAIL = "KEYNAME_VIEWALERT_VIDEO_FAIL";
    // public UIImage imageBg;
    // public UIText textTitle;
    public UIText textDetail;
    public Button btnRevive;//复活
    public Button btnRestart; //从第一关开始

    public void Awake()
    {
        base.Awake();
        // textDetail.color = new Color32(192, 90, 59, 255);

        if (!AppVersion.appCheckHasFinished)
        {
            btnRevive.gameObject.SetActive(false);
        }
        LayOut();
    }
    // Use this for initialization
    public void Start()
    {
        base.Start();
        if (!AppVersion.appCheckHasFinished)
        {
            textDetail.text = Language.main.GetString("STR_GameFail_Detail_for_check");
        }
        else
        {
            textDetail.text = Language.main.GetString("STR_GameFail_Detail");
        }

        LayOut();
    }

    public override void LayOut()
    {
        base.LayOut();
        float x = 0, y = 0, w = 0, h = 0;
        float ratio = 0.8f;
        if (Device.isLandscape)
        {
            ratio = 0.7f;
        }

        RectTransform rctranRoot = this.GetComponent<RectTransform>();
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        {

            w = sizeCanvas.x * ratio;
            h = sizeCanvas.y * ratio;//rctran.rect.size.y * w / rctran.rect.size.x;
            rctranRoot.sizeDelta = new Vector2(w, h);

        }
        base.LayOut();
    }

    public void OnClickBtnRevive()
    {
        if (!AppVersion.appCheckHasFinished)
        {
            Close();
            GameManager.main.GotoPlayAgain();
            return;
        }

        AdKitCommon.main.callbackAdVideoFinish = OnAdKitAdVideoFinish;
        AdKitCommon.main.ShowAdVideo();


    }
    public void OnClickBtnRestart()
    {
        Close();
        LevelManager.main.placeLevel = 0;
        LevelManager.main.gameLevel = 0;
        LevelManager.main.gameLevelFinish = -1;

        if (Application.isEditor)
        {
            LevelManager.main.gameLevel = 1;
            LevelManager.main.gameLevelFinish = 2;
        }
        GameManager.main.GotoPlayAgain();
    }
    void ShowVideoFail()
    {
        string title = Language.main.GetString("STR_UIVIEWALERT_TITLE_VIDEO_FAIL");
        string msg = Language.main.GetString("STR_UIVIEWALERT_MSG_VIDEO_FAIL");
        string yes = Language.main.GetString(("STR_UIVIEWALERT_YES_VIDEO_FAIL"));
        string no = Language.main.GetString("STR_UIVIEWALERT_NO_VIDEO_FAIL");
        ViewAlertManager.main.ShowFull(title, msg, yes, no, true, KEYNAME_VIEWALERT_VIDEO_FAIL, OnUIViewAlertFinished);
    }
    void OnUIViewAlertFinished(UIViewAlert alert, bool isYes)
    {

        if (KEYNAME_VIEWALERT_VIDEO_FAIL == alert.keyName)
        {
            if (isYes)
            {
                OnClickBtnRevive();
            }
        }
    }

    public void OnAdKitAdVideoFinish(AdKitCommon.AdType type, AdKitCommon.AdStatus status, string str)
    {
        if (type == AdKitCommon.AdType.VIDEO)
        {
            if (status == AdKitCommon.AdStatus.SUCCESFULL)
            {
                Close();
                GameManager.main.GotoPlayAgain();
            }
            if (status == AdKitCommon.AdStatus.FAIL)
            {
                ShowVideoFail();
            }

        }
    }
}
