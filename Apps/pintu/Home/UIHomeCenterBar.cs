using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Moonma.IAP;
using Moonma.Share;
using UnityEngine;
using UnityEngine.UI;
using Moonma.SysImageLib;
public class UIHomeCenterBar : UIView
{

    public Button btnPlay;
    public Button btnLearn;
    public Button btnAdVideo;
    public Button btnAddLove;
    public Button btnPhoto;
    public Button btnCamera;
    public Button btnNetImage;
    void Awake()
    {


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
        if (!AppVersion.appCheckHasFinished)
        {
            btnPhoto.gameObject.SetActive(false);
            btnCamera.gameObject.SetActive(false);
            btnNetImage.gameObject.SetActive(false);
        }

        if (!Config.main.APP_FOR_KIDS)
        {
            btnLearn.gameObject.SetActive(false);
        }
        if (Common.appKeyName == GameRes.GAME_Word)
        {
            btnPhoto.gameObject.SetActive(false);
            btnCamera.gameObject.SetActive(false);
            btnNetImage.gameObject.SetActive(false);
        }
        // btnAddLove.gameObject.SetActive(AppVersion.appCheckHasFinished);
        btnAddLove.gameObject.SetActive(false);
    }
    // Use this for initialization
    void Start()
    {
        LayOut();

    }



    public override void LayOut()
    {
        base.LayOut();

        Vector2 sizeCanvas = this.frame.size;
        float x = 0, y = 0, w = 0, h = 0;

        LayOutGrid lygrid = this.GetComponent<LayOutGrid>();
        if (Device.isLandscape)
        {
            lygrid.row = 1;
            lygrid.col = lygrid.GetChildCount(false);


        }
        else
        {
            int count = lygrid.GetChildCount(false);
            lygrid.col = 2;
            lygrid.row = count / lygrid.col;
            if (count % lygrid.col > 0)
            {
                lygrid.row++;
            }

        }
        // if (!AppVersion.appCheckHasFinished)
        // {
        //     lygrid.row = 2;

        //     lygrid.col = 2;
        // }


        lygrid.LayOut();

        RectTransform rctran = this.GetComponent<RectTransform>();
        w = rctran.rect.width;
        h = lygrid.row * 256;
        rctran.sizeDelta = new Vector2(w, h);

    }





    public void OnClickBtnPlay()
    {
        //AudioPlay.main.PlayAudioClip(audioClipBtn); 
        UIGamePintu.imageSource = GamePintu.ImageSource.GAME_INNER;
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

    public void OnClickBtnPhoto()
    {

    if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            navi.Push(AdHomeViewController.main);
        }
        return ;

        SysImageLib.main.SetObjectInfo(this.gameObject.name, "OnSysImageLibDidOpenFinish");
        SysImageLib.main.OpenImage();
    }
    public void OnClickBtnCamera()
    {
        SysImageLib.main.SetObjectInfo(this.gameObject.name, "OnSysImageLibDidOpenFinish");
        SysImageLib.main.OpenCamera();
    }

    public void OnClickBtnNetImage()
    {
        UIGamePintu.imageSource = GamePintu.ImageSource.NET;
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            navi.Push(NetImageViewController.main);
        }
    }

    public void OnClickBtnLearn()
    {
        NaviViewController navi = this.controller.naviController;
        navi.Push(LearnProgressViewController.main);

    }
    public void OnClickBtnAddLove()
    {
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            navi.Push(LoveViewController.main);
        }
    }

    public void OnClickBtnAdVideo()
    {
        AdKitCommon.main.ShowAdVideo();
    }
    void OnSysImageLibDidOpenFinish(string file)
    {
        Texture2D tex = null;
        if (Common.isAndroid)
        {
            int w, h;
            // using (var javaClass = new AndroidJavaClass(SysImageLib.JAVA_CLASS))
            {

                //安卓系统解码
                // w = javaClass.CallStatic<int>("GetRGBDataWidth");
                // h = javaClass.CallStatic<int>("GetRGBDataHeight");
                // byte[] dataRGB = javaClass.CallStatic<byte[]>("GetRGBData");
                //   tex = LoadTexture.LoadFromRGBData(dataRGB, w, h);
            }

            //unity解码
            tex = LoadTexture.LoadFromFile(file);

        }
        else
        {
            tex = LoadTexture.LoadFromFile(file);
        }

        if (tex != null)
        {
            UIGamePintu.texGamePic = tex;
            UIGamePintu.imageSource = GamePintu.ImageSource.SYSTEM_IMAGE_LIB;
            if (this.controller != null)
            {
                NaviViewController navi = this.controller.naviController;
                navi.Push(GameViewController.main);
            }

        }
    }

}
