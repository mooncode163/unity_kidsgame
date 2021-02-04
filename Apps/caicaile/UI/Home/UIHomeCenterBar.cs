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

    void Awake()
    {
        btnAddLove.gameObject.SetActive(true);
        if (!Config.main.APP_FOR_KIDS)
        {
            btnLearn.gameObject.SetActive(false);
            // btnAddLove.gameObject.SetActive(false);
        }
        if (Common.appKeyName != GameRes.GAME_IdiomConnect)
        {
            // btnAddLove.gameObject.SetActive(false);
        }
        if (Common.appKeyName == GameRes.GAME_IdiomFlower)
        {
            btnAddLove.gameObject.SetActive(true);
        }
        if (Common.appKeyName == GameRes.GAME_PlacePoem)
        {
            btnAddLove.gameObject.SetActive(true);
        }

        if (!AppVersion.appCheckHasFinished)
        {
            btnAddLove.gameObject.SetActive(false);
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

    }




    public void OnClickBtnLearn()
    {

        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            navi.Push(LearnViewController.main);

        }
    }

    public void OnClickBtnAdVideo()
    {
        AdKitCommon.main.ShowAdVideo();
    }
    public void OnClickBtnAddLove()
    {

        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            navi.Push(LoveViewController.main);
        }
    }
}
