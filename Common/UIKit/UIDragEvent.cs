using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class UIDragEvent : UIView//, IPointerUpHandler, IPointerDownHandler//, IDragHandler
{
    public Vector3 posDown;//物体坐标
    public Vector3 posInPutDown;//鼠标坐标
    public bool enableDrag = false;


    public void OnUIDragEvent(PointerEventData eventData, int status)
    {
        switch (status)
        {
            case UITouchEvent.STATUS_TOUCH_DOWN:
                {
                    OnPointerDown(eventData);
                }
                break;
            case UITouchEvent.STATUS_TOUCH_MOVE:
                {
                    OnDrag(eventData);
                }
                break;
            case UITouchEvent.STATUS_TOUCH_UP:
                OnPointerUp(eventData);
                break;

        }
    }
    //相当于touchDown
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("UIDragEvent OnPointerDown");
        if (!enableDrag)
        {
            return;
        }
        Vector3 posworld = Common.GetInputPositionWorld(mainCam);
        posInPutDown = posworld;
        posDown = this.gameObject.transform.position;

        Vector3 poslocal = this.transform.InverseTransformPoint(posworld);
        // if (callBackTouch != null)
        // {
        //     callBackTouch(this, eventData, STATUS_TOUCH_DOWN);
        // }

    }
    //相当于touchUp
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        // if (callBackTouch != null)
        // {
        //     callBackTouch(this, eventData, STATUS_TOUCH_UP);
        // }
    }
    //相当于touchMove
    public virtual void OnDrag(PointerEventData eventData)
    {
     
        if (!enableDrag)
        {
            return;
        }
        // if (callBackTouch != null)
        // {
        //     callBackTouch(this, eventData, STATUS_TOUCH_MOVE);
        // }
        Vector3 posworld = Common.GetInputPositionWorld(mainCam);
        float stepx = posworld.x - posInPutDown.x;
        float stepy = posworld.y - posInPutDown.y;
        float x = posDown.x + stepx;
        float y = posDown.y + stepy;
        Vector3 posnow = new Vector3(x, y, posDown.z);
        this.gameObject.transform.position = posnow;
    }
}
