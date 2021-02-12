using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIHomePaint : UIHomeBase
{
    float timeAction;
    bool isActionFinish;

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
        LevelManager.main.ParseGuanka();
        // actionBtnLearn.gameObject.SetActive(Config.main.APP_FOR_KIDS);


        // actionBtnLearn.ptNormal = layoutBtn.GetItemPostion(0, 0);
        LoadPrefab();


    }

    // Use this for initialization
    public void Start()
    {
        base.Start();
        isActionFinish = false;
        // RunActionImageName();
        //   actionBtnLearn.RunAction();
        LayOut();
        Invoke("LayOut", 0.1f);
        OnUIDidFinish();

    }

    void LoadPrefab()
    {
        float x, y, z;


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
        // RectTransform rctranPlay = btnPlay.transform as RectTransform;
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

            // y = (sizeCanvas.y / 2 + (rctranPlay.anchoredPosition.y + rctranPlay.rect.height / 2)) / 2;

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
                      Invoke("OnUIDidFinish", 1f);
                      isActionFinish = true;
                      Invoke("LayOut", 0.2f);
                  });
    }


    public override void LayOut()
    {
        base.LayOut();

        // centerBar.LayOut();


        base.LayOut();
    }



}
