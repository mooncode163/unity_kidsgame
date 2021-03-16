using System.Collections;
using System.Collections.Generic;
using Moonma.SysImageLib;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public delegate void OnUIShapeBarItemDidClickDelegate(UIShapeBarItem item);
public class UIShapeBarItem : UIView//, ISysImageLibDelegate
{
    public RawImage imageItem;
    public int index;
    public ItemInfo infoItem;
    public OnUIShapeBarItemDidClickDelegate callBackDidClick { get; set; }
    void Awake()
    {
        // UITouchEvent ev = this.gameObject.AddComponent<UITouchEvent>();
        // ev.callBackTouch = OnUITouchEvent;
    }


    // Use this for initialization
    void Start()
    {

        LayOut();
    }

    // Update is called once per frame
    void Update()
    {


    }


    public override void LayOut()
    {
        base.LayOut();

    }

    public void UpdateItem(ItemInfo info)
    {
        infoItem = info;
          Debug.Log("Shape UpdateItem  info.pic =" + info.pic);
        TextureUtil.UpdateRawImageTexture(imageItem, info.pic, true);
        LayOut();
    }

    public void OnClickItem()
    {
        if (callBackDidClick != null)
        {
            callBackDidClick(this);
        }
    }
    public void OnUITouchEvent(UITouchEvent ev, PointerEventData eventData, int status)
    {
        Debug.Log("Shape OnUITouchEvent  status =" + status);
        switch (status)
        {
            case UITouchEvent.STATUS_TOUCH_DOWN:
                {

                }
                break;
            case UITouchEvent.STATUS_TOUCH_MOVE:
                {

                }
                break;
            case UITouchEvent.STATUS_TOUCH_UP:
                {
                    OnClickItem();
                }
                break;
        }
    }
}
