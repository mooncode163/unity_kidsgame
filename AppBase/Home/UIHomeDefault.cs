using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIHomeDefault : UIHomeBase
{
    public Button btnPlay;
    float timeAction;
    bool isActionFinish;
    void Awake()
    {
        base.Awake();
       // TextureUtil.UpdateRawImageTexture(imageBg, AppRes.IMAGE_HOME_BG, true);
        string appname = Common.GetAppNameDisplay();
        TextName.text = appname;
        timeAction = 0.3f;
        isActionFinish = false;
    }

    // Use this for initialization
    void Start()
    {
        base.Start();
        isActionFinish = false;
        LayOut();
        OnUIDidFinish();
        RunActionImageName();
        RunActionBtnPlay();
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
            y = (sizeCanvas.y / 2 - topBarHeight) / 2;
        }
        return new Vector4(x, y, w, h);
    }
    public Vector2 GetPosOfBtnPlay()
    {
        Vector2 sizeCanvas = this.frame.size;
        float x = 0, y = 0;
        RectTransform rctranName = imageBgName.GetComponent<RectTransform>();

        Vector4 ptName = GetPosOfImageName();
        x = 0;
        y = (-sizeCanvas.y / 2 + (ptName.y - rctranName.rect.height / 2)) / 2;
        return new Vector2(x, y);
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
                  });
    }


    void RunActionBtnPlay()
    {
        //动画：https://blog.csdn.net/agsgh/article/details/79447090
        //iTween.ScaleTo(info.obj, new Vector3(0f, 0f, 0f), 1.5f);
        float duration = timeAction;
        Vector2 ptNormal = GetPosOfBtnPlay();
        RectTransform rctran = btnPlay.GetComponent<RectTransform>();
        Vector2 sizeCanvas = this.frame.size;
        float x, y;
        x = 0;
        y = -sizeCanvas.y / 2 - rctran.rect.height;
        rctran.anchoredPosition = new Vector2(x, y);
        Vector2 toPos = new Vector2(ptNormal.x, ptNormal.y);
        rctran.DOLocalMove(toPos, duration).OnComplete(() =>
                  {
                      this.RunActionUpDown();
                      isActionFinish = true;
                  });
    }

    //上下晃动动画
    void RunActionUpDown()
    {
        //动画：https://blog.csdn.net/agsgh/article/details/79447090
        //iTween.ScaleTo(info.obj, new Vector3(0f, 0f, 0f), 1.5f);
        float duration = timeAction * 4;
        Vector2 size = AppSceneBase.main.sizeCanvas;
        float w, h;

        Button btn = btnPlay;
        RectTransform rctran = btn.GetComponent<RectTransform>();
        h = rctran.rect.height;
        float y_step = h / 10;
        Vector2 pt = GetPosOfBtnPlay();
        // var actionUp = cc.moveBy(duration, 0, y_step);
        // var actionDown = cc.moveBy(duration, 0, -y_step);
        //  var time = cc.delayTime(0.5 * i);
        //   var seq = cc.sequence([time, actionUp, actionUp.reverse(), actionDown, actionDown.reverse()]);
        //  btn.node.runAction(seq.repeatForever());
        float z = btn.transform.localPosition.z;

        Vector3 posNormal = new Vector3(pt.x, pt.y, z);
        Vector3 toPos = new Vector3(pt.x, pt.y + y_step, z);
        Sequence seq = DOTween.Sequence();
        //actionUp
        Tweener acUp = rctran.DOLocalMove(toPos, duration);

        //normal
        Tweener acNormal = rctran.DOLocalMove(posNormal, duration);
        Tweener acNormal2 = rctran.DOLocalMove(posNormal, duration);
        //actionDown
        toPos = new Vector3(pt.x, pt.y - y_step, z);
        Tweener acDown = rctran.DOLocalMove(toPos, duration);
        float time = 0;
        seq.AppendInterval(time).Append(acUp).Append(acNormal).Append(acDown).Append(acNormal2).SetLoops(-1);

    }
    public void OnClickBtnPlay()
    {
        if (!isActionFinish)
        {
            return;
        }
        //AudioPlay.main.PlayAudioClip(audioClipBtn);
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

    public override void LayOut()
    {
        base.LayOut();
        Vector2 sizeCanvas = this.frame.size;
        float x = 0, y = 0, w = 0, h = 0;
        RectTransform rctranAppIcon = uiHomeAppCenter.transform as RectTransform;

        Vector4 ptImageName = GetPosOfImageName();
        //image name
        {
            RectTransform rctran = imageBgName.GetComponent<RectTransform>();
            rctran.sizeDelta = new Vector2(ptImageName.z, ptImageName.w);
            rctran.anchoredPosition = new Vector2(ptImageName.x, ptImageName.y);
        }

        {
            RectTransform rctran = btnPlay.GetComponent<RectTransform>();
            Vector2 pt = GetPosOfBtnPlay();
            rctran.anchoredPosition = new Vector2(pt.x, pt.y);
        }

        base.LayOut();
    }
}
