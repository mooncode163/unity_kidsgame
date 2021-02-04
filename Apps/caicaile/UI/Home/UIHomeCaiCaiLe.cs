using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIHomeCaiCaiLe : UIHomeBase
{
    float timeAction;
    bool isActionFinish;
    public LayOutGrid layoutBtnSide;
    public LayOutGrid layoutBtn;
    public AnimateButton btnPlay;
    public GameObject objLogo;
    //public ActionHomeBtn actionBtnLearn;
    public void Awake()
    {
        base.Awake();
        // TextureUtil.UpdateRawImageTexture(imageBg, AppRes.IMAGE_HOME_BG, true);
        AppSceneBase.main.UpdateWorldBg(AppRes.IMAGE_HOME_BG);
        string appname = Common.GetAppNameDisplay();
        TextName.text = appname;
        TextName.color = GameRes.main.colorTitle;
        timeAction = 0.3f;
        isActionFinish = false;

        // actionBtnLearn.gameObject.SetActive(Config.main.APP_FOR_KIDS);


        // actionBtnLearn.ptNormal = layoutBtn.GetItemPostion(0, 0);
        LoadPrefab();


        if (Config.main.APP_FOR_KIDS)
        {
            objLogo.gameObject.SetActive(false);
        }
        else
        {
            objLogo.gameObject.SetActive(false);
            //imageBgName.gameObject.SetActive(false);
        }


    }

    // Use this for initialization
    public void Start()
    {
        base.Start();
        isActionFinish = false;
        RunActionImageName();
        
        UpdateLayoutBtn();
        //   actionBtnLearn.RunAction();
        LayOut();


    }

    void LoadPrefab()
    {
        float x, y, z;
        {
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/Home/GlitterParticles");
            obj = GameObject.Instantiate(obj);
            x = obj.transform.localPosition.x;
            y = obj.transform.localPosition.y;
            z = -1f;
            AppSceneBase.main.AddObjToMainWorld(obj);
            obj.transform.localPosition = new Vector3(x, y, z);
        }
        {
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/Home/StarsParticles");
            obj = GameObject.Instantiate(obj);
            x = obj.transform.localPosition.x;
            y = obj.transform.localPosition.y;
            z = -1f;
            AppSceneBase.main.AddObjToMainWorld(obj);
            obj.transform.localPosition = new Vector3(x, y, z);
        }

    }
    // Update is called once per frame
    void Update()
    {
        UpdateBase();
    }

    public Vector4 GetPosOfImageName()
    {
        Vector2 sizeCanvas = this.frame.size;
        float x = 0, y = 0, w = 0, h = 0;
        RectTransform rctranPlay = btnPlay.transform as RectTransform;
        //image name
        {
            RectTransform rctran = imageBgName.GetComponent<RectTransform>();

            int fontSize = TextName.fontSize;
            int r = fontSize / 2;
            w = Common.GetStringLength(TextName.text, AppString.STR_FONT_NAME, fontSize) + r * 2;
            h = fontSize * 1.5f;
            if (!Device.isLandscape)
            {
                h = fontSize * 2;
                if ((w + r * 2) > sizeCanvas.x)
                {
                    //显示成两行文字
                    w = w / 2 + r * 2;
                    h = h * 2;
                    // RectTransform rctranText = TextName.GetComponent<RectTransform>();
                    // float w_text = rctranText.sizeDelta.x;
                    // rctranText.sizeDelta = new Vector2(w_text, h);
                }
            }


            x = 0;

            y = (sizeCanvas.y / 2 + (rctranPlay.anchoredPosition.y + rctranPlay.rect.height / 2)) / 2;

        }
        return new Vector4(x, y, w, h);
    }

    void RunActionImageName()
    {
        //动画：https://blog.csdn.net/agsgh/article/details/79447090
        //iTween.ScaleTo(info.obj, new Vector3(0f, 0f, 0f), 1.5f);
        float duration = timeAction;
        Vector4 ptNormal = GetPosOfImageName();
        RectTransform rctran = imageBgName.GetComponent<RectTransform>();
        Vector2 sizeCanvas = this.frame.size;
        float x, y;
        x = 0;
        y = sizeCanvas.y / 2 + rctran.rect.height;
        rctran.anchoredPosition = new Vector2(x, y);

        Vector2 toPos = new Vector2(ptNormal.x, ptNormal.y);
        rctran.DOLocalMove(toPos, duration).OnComplete(() =>
                  {
                      OnUIDidFinish(1f);
                      isActionFinish = true;
                      Invoke("LayOut", 0.2f);
                  });
    }


    public void UpdateLayoutBtn()
    {
        float w_item = 160;
        float h_item = 160;
        float oft = 8;
        float x, y, w, h;
        Vector2 sizeCanvas = this.frame.size;

        layoutBtnSide.enableHide = false;

        int child_count = layoutBtnSide.GetChildCount(false);
        if (Device.isLandscape)
        {
            layoutBtnSide.row = 1;
            layoutBtnSide.col = child_count;
        }
        else
        {
            layoutBtnSide.row = child_count;
            layoutBtnSide.col = 1;
        }

        RectTransform rctran = layoutBtnSide.GetComponent<RectTransform>();
        rctran.sizeDelta = new Vector2((w_item + oft) * layoutBtnSide.col, (h_item + oft) * layoutBtnSide.row);
        layoutBtnSide.LayOut();

        GridLayoutGroup gridLayout = uiHomeAppCenter.GetComponent<GridLayoutGroup>();
        Vector2 cellSize = gridLayout.cellSize;
        w = rctran.rect.size.x;
        h = rctran.rect.size.y;
        if (Device.isLandscape)
        {
            x = 0;
            y = -sizeCanvas.y / 2 + h / 2;
        }
        else
        {
            x = -sizeCanvas.x / 2 + w / 2;
            y = -sizeCanvas.y / 2 + cellSize.y + h / 2;
        }

        //Debug.Log("layout y1=" + y1 + " y2=" + y2 + " y=" + y + " rctranAppIcon.rect.size.y=" + rctranAppIcon.rect.size.y);


        rctran.anchoredPosition = new Vector2(x, y);

    }

    public override void LayOut()
    {
        base.LayOut();
        Vector2 sizeCanvas = this.frame.size;
        float x = 0, y = 0, w = 0, h = 0;
        RectTransform rctranAppIcon = uiHomeAppCenter.transform as RectTransform;
        RectTransform rctranImageName = imageBgName.GetComponent<RectTransform>();

        RectTransform rctranPlay = btnPlay.transform as RectTransform;
        //play
        {


            x = 0;

            if (Device.isLandscape)
            {
                y = 0;
            }
            else
            {
                y = -rctranPlay.rect.size.y / 2;
            }
            rctranPlay.anchoredPosition = new Vector2(x, y);
        }

        Vector4 ptImageName = GetPosOfImageName();
        //image name
        {

            rctranImageName.sizeDelta = new Vector2(ptImageName.z, ptImageName.w);
            rctranImageName.anchoredPosition = new Vector2(ptImageName.x, ptImageName.y);
        }




        UpdateLayoutBtn();

        //layoutBtn
        {

            layoutBtn.enableHide = false;
            int child_count = layoutBtn.GetChildCount(false);
            layoutBtn.row = 1;
            layoutBtn.col = child_count;
            layoutBtn.LayOut();
            RectTransform rctran = layoutBtn.transform as RectTransform;

            float h_item = rctran.rect.size.y;
            float w_item = h_item;
            float oft = layoutBtn.space.x;
            h = (h_item + oft) * layoutBtn.row;
            w = (w_item + oft) * layoutBtn.col;
            rctran.sizeDelta = new Vector2(w, h);


            if (Device.isLandscape)
            {
                x = rctranPlay.anchoredPosition.x + rctranPlay.rect.size.x / 2 + w / 2 + 16;
                y = 0;
            }
            else
            {
                x = 0;
                y = rctranPlay.anchoredPosition.y - rctranPlay.rect.size.y / 2 - h / 2;
            }
            rctran.anchoredPosition = new Vector2(x, y);


        }

    }


    public void OnClickBtnPlay()
    {

        Debug.Log("OnClickBtnPlay");
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            int total = LevelManager.main.placeTotal;
            if (Common.appKeyName == GameRes.GAME_IdiomFlower)
            {
                total = 2;
            }
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
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            navi.Push(LearnViewController.main);
        }
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
