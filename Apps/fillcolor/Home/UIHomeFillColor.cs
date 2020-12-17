using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIHomeFillColor : UIHomeBase
{
    float timeAction;
    bool isActionFinish; 
    public UIHomeSideBar uiHomeSideBar;
    public UIHomeCenterBar uiHomeCenterBar;
   
 

    //public ActionHomeBtn actionBtnLearn;
    public void Awake()
    {
        base.Awake();
        // TextureUtil.UpdateRawImageTexture(imageBg, AppRes.IMAGE_HOME_BG, true);
        AppSceneBase.main.UpdateWorldBg(AppRes.IMAGE_HOME_BG);
        string appname = Common.GetAppNameDisplay();
        TextName.text = appname;
        timeAction = 0.3f;
        isActionFinish = false;

        LoadPrefab();
 
    }

    // Use this for initialization
    public void Start()
    {
        base.Start();
        isActionFinish = false; 
        //   actionBtnLearn.RunAction();
        LayOut();
        OnUIDidFinish(2);
    }

    void LoadPrefab()
    {
        float x, y, z;
        // {
        //     GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/Home/GlitterParticles");
        //     obj = GameObject.Instantiate(obj);
        //     x = obj.transform.localPosition.x;
        //     y = obj.transform.localPosition.y;
        //     z = -1f;
        //     AppSceneBase.main.AddObjToMainWorld(obj);
        //     obj.transform.localPosition = new Vector3(x, y, z);
        // }
        // {
        //     GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/Home/StarsParticles");
        //     obj = GameObject.Instantiate(obj);
        //     x = obj.transform.localPosition.x;
        //     y = obj.transform.localPosition.y;
        //     z = -1f;
        //     AppSceneBase.main.AddObjToMainWorld(obj);
        //     obj.transform.localPosition = new Vector3(x, y, z);
        // }

    }
    // Update is called once per frame
    void Update()
    {
        UpdateBase();
        //   LayOut();
    }
  
   

    public override void LayOut()
    {
        base.LayOut();
        Vector2 sizeCanvas = this.frame.size;
        float x = 0, y = 0, w = 0, h = 0;
        // RectTransform rctranAppIcon = uiHomeAppCenter.transform as RectTransform; 

        // RectTransform rctranPlay = btnPlay.transform as RectTransform;
        // // //play
        // {


        //     x = 0;

        //     if (Device.isLandscape)
        //     {
        //         y = 0;
        //     }
        //     else
        //     {
        //         y = -rctranPlay.rect.size.y / 2;
        //     }
        //     rctranPlay.anchoredPosition = new Vector2(x, y);
        // }

        // Vector4 ptImageName = GetPosOfImageName();
        // //image name
        // {

        //     rctranImageName.sizeDelta = new Vector2(ptImageName.z, ptImageName.w);
        //     rctranImageName.anchoredPosition = new Vector2(ptImageName.x, ptImageName.y);
        // }


        uiHomeAppCenter.LayOut();

        // UpdateLayoutBtn();

        uiHomeCenterBar.LayOut();
        base.LayOut();

        //layoutBtn
        // {

        //     layoutBtn.enableHide = false;
        //     int child_count = layoutBtn.GetChildCount(false);
        //     layoutBtn.row = 1;
        //     layoutBtn.col = child_count;
        //     layoutBtn.LayOut();
        //     RectTransform rctran = layoutBtn.transform as RectTransform;

        //     float h_item = rctran.rect.size.y;
        //     float w_item = h_item;
        //     float oft = layoutBtn.space.x;
        //     h = (h_item + oft) * layoutBtn.row;
        //     w = (w_item + oft) * layoutBtn.col;
        //     rctran.sizeDelta = new Vector2(w, h);

        //     y = 0;
        //     x = 0;
        //     if (Device.isLandscape)
        //     {
        //         x = rctranPlay.anchoredPosition.x + rctranPlay.rect.size.x / 2 + w / 2 + 16;
        //         y = 0;
        //     }
        //     else
        //     {
        //         x = 0;
        //         y = rctranPlay.anchoredPosition.y - rctranPlay.rect.size.y / 2 - h / 2;
        //     }
        //     rctran.anchoredPosition = new Vector2(x, y);


        // }
    
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
}
