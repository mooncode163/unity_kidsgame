using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Moonma.IAP;
using Moonma.Share;
using UnityEngine;
using UnityEngine.UI;

public class UIHomeCenterBar : UIView
{
    public Button btnHistory;
    public Button btnFreeDraw;
    public Button btnAdVideo;
    public Button btnAddLove;


    public UIViewController controllerHome;
    void Awake()
    {
        controllerHome = HomeViewController.main;

        this.controller = controllerHome;

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
        if (btnAddLove != null)
        {
            btnAddLove.gameObject.SetActive(AppVersion.appCheckHasFinished);
        }
    }
    // Use this for initialization
    void Start()
    {
        LayOut();

    }



    public override void LayOut()
    {
        base.LayOut();
        LayOutGrid ly = this.GetComponent<LayOutGrid>();
        if (Device.isLandscape)
        {
            ly.row = 1;
            ly.col = ly.GetChildCount();
        }
        else
        {

            ly.col = 2;
            ly.row = (int)Mathf.Ceil(ly.GetChildCount() * 1.0f / ly.col);
        }

        RectTransform rctran = this.GetComponent<RectTransform>();
        float w, h;
        w = rctran.sizeDelta.x;
        h = ly.row * (btnHistory.GetComponent<RectTransform>().rect.width + ly.space.y);
        rctran.sizeDelta = new Vector2(w, h);

        ly.LayOut();
    }

    public void GotoGame(int mode)
    {
        UIGamePaint.gameMode = mode;
        UIViewController controller = null;

        if (UIGamePaint.gameMode == UIGamePaint.GAME_MODE_FREE_DRAW)
        {
            controller = GameViewController.main;
        }
        else
        {

            int total = LevelManager.main.placeTotal;
            if (total > 1)
            {
                controller = PlaceViewController.main;
            }
            else
            {
                controller = GuankaViewController.main;
            }
        }

        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            navi.source = AppRes.SOURCE_NAVI_GUANKA;
            navi.Push(controller);

        }


    }


    public void OnClickBtnPlay()
    {

        Debug.Log("OnClickBtnPlay");
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            int total = LevelManager.main.placeTotal;
            if (total > 1)
            {
                navi.Push(PlaceViewController.main);
            }
            else
            {
                navi.Push(GuankaViewController.main);
            }
        }
    }

    public void OnClickBtnLearn()
    {

        if (controllerHome != null)
        {
            NaviViewController navi = controllerHome.naviController;
            //  navi.Push(LearnViewController.main);

        }
    }

    public void OnClickBtnAdVideo()
    {
        AdKitCommon.main.ShowAdVideo();
    }
    public void OnClickBtnAddLove()
    {
        // if (controllerHome != null)
        // {
        //     NaviViewController navi = controllerHome.naviController;
        //     navi.Push(LoveViewController.main);
        // }
    }

    public void OnClickBtnHistory()
    {
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            navi.source = AppRes.SOURCE_NAVI_HISTORY;
            navi.Push(HistoryViewController.main);
        }
    }

    public void OnClickBtnFreeDraw()
    {
        Invoke("DoClickBtnFreeDraw", 0.1f);
    }

    public void DoClickBtnFreeDraw()
    {
        LevelManager.main.placeLevel = 0;
        //必须在placeLevel设置之后再设置gameLevel
        LevelManager.main.gameLevel = 0;
        GotoGame(UIGamePaint.GAME_MODE_FREE_DRAW);
    }
}
