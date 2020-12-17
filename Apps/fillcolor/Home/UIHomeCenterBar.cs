using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Moonma.IAP;
using Moonma.Share;
using UnityEngine;
using UnityEngine.UI;

public class UIHomeCenterBar : UIView
{

    public Button btnLearn;
    public Button btnAdVideo;
    public Button btnAddLove;

    public LayOutGrid layOutGrid;
    public UIViewController controllerHome;
    void Awake()
    {
        controllerHome = HomeViewController.main;

        btnAddLove.gameObject.SetActive(false);
        // if (!Config.main.APP_FOR_KIDS)
        {
            btnLearn.gameObject.SetActive(false);
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

    }
    // Use this for initialization
    void Start()
    {
        LayOut();

    }

    public override void LayOut()
    {
        base.LayOut();

        layOutGrid.enableHide = false;
        int child_count = layOutGrid.GetChildCount(false);
        if (Device.isLandscape)
        {

            layOutGrid.row = 1;
            layOutGrid.col = child_count;
        }
        else
        {

            layOutGrid.col = 2;
            layOutGrid.row = child_count / layOutGrid.col;
            if (child_count%layOutGrid.col!=0)
            {
                layOutGrid.row++;
            }
        }
        layOutGrid.LayOut();
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

}
