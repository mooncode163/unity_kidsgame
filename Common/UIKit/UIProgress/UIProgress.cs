using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public delegate void OnUIProgressDelegate(UIProgress ui);
public class UIProgress : UIView
{
    public enum Type
    {
        SLIDER,
        NO_SLIDER
    }
    public UIImage imageBg;
    public UIImage imageFt;
    public UIImage imageSlider;
    public OnUIProgressDelegate callBackProgress { get; set; }

    private float _percent = 0;//0-100
    public float percent
    {
        get
        {
            return _percent;
        }
        set
        {
            _percent = value;
            LayOutSize ly = imageFt.transform.GetComponent<LayOutSize>();
            ly.ratioW = _percent / 100;
            ly.LayOut();
            LayOut();
        }
    }
    private Type _type;
    public Type type
    {
        get
        {
            return _type;
        }
        set
        {
            _type = value;
            switch (_type)
            {
                case Type.SLIDER:
                    {
                        imageSlider.gameObject.SetActive(true);
                    }
                    break;
                case Type.NO_SLIDER:
                    {
                        imageSlider.gameObject.SetActive(false);
                    }
                    break;
            }
        }
    }



    /// </summary>
    public void Awake()
    {
        base.Awake();
        UITouchEventWithMove touch_ev = this.gameObject.AddComponent<UITouchEventWithMove>();
        touch_ev.callBackTouch = OnTouchEvent;
    }
    // Use this for initialization
    public void Start()
    {
        base.Start();
        this.percent = _percent;
        LayOut();

    }

    public override void LayOut()
    {
        base.LayOut();
        RectTransform rctran = this.gameObject.GetComponent<RectTransform>();
        // slider
        float x, y, w, h;
        w = Common.CanvasToScreenWidth(AppSceneBase.main.sizeCanvas, rctran.rect.width*this.transform.localScale.x);
        h = Common.CanvasToScreenHeight(AppSceneBase.main.sizeCanvas, rctran.rect.height*this.transform.localScale.y);
        y = imageSlider.transform.position.y;
        Vector2 bd = GetBoundSizeOfGameObject(imageSlider.gameObject);
        Debug.Log("OnTouchEvent imageSlider bd=" + bd);
        float w_ft = Common.CanvasToScreenWidth(AppSceneBase.main.sizeCanvas, bd.x);
        float h_ft = Common.CanvasToScreenHeight(AppSceneBase.main.sizeCanvas, bd.y);
        // x = posTouch.x;
        x = _percent * w/100;
        if ((x + w_ft / 2) > w)
        {
            x = w - w_ft / 2;
        }
          if ((x - w_ft / 2) <0)
        {
            x = w_ft / 2;
        }
        imageSlider.transform.position = new Vector2(x, y);
    }

    public void OnTouchEvent(UITouchEvent ev, PointerEventData eventData, int status)
    {

        switch (status)
        {
            case UITouchEvent.STATUS_TOUCH_DOWN:

                break;

            case UITouchEvent.STATUS_TOUCH_MOVE:
                {
                    RectTransform rctran = this.gameObject.GetComponent<RectTransform>();

                    Vector2 posTouch = eventData.position;
                    Vector2 pos = this.transform.position;
                    float x, y, w, h;
                    w = Common.CanvasToScreenWidth(AppSceneBase.main.sizeCanvas, rctran.rect.width);
                    h = Common.CanvasToScreenHeight(AppSceneBase.main.sizeCanvas, rctran.rect.height);
                    x = pos.x - w / 2;
                    y = pos.y - h / 2;
                    this.percent = (posTouch.x - x) * 100 / w;
 
                    LayOut();

                    if (callBackProgress != null)
                    {
                        callBackProgress(this);
                    }

                }
                break;

            case UITouchEvent.STATUS_TOUCH_UP:


                break;

        }

        LayOut();
    }

}
