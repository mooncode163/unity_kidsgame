using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public delegate void OnUITouchEventDelegate(UITouchEvent ev, PointerEventData eventData, int status);
//IDragHandler 会导致scrollview的滑动失效 所以和UITouchEventWithMove分开使用
public class UITouchEvent : MonoBehaviour, IPointerUpHandler, IPointerDownHandler//, IDragHandler
{
    public const int STATUS_TOUCH_DOWN = 0;
    public const int STATUS_TOUCH_MOVE = 1;
    public const int STATUS_TOUCH_UP = 2;
    public const int STATUS_Click = 3;

    public const int STATUS_LongPress = 4;

    public const float Time_LongPress = 2f;//second
public bool enableLongPress;
    bool isTouchDown = false;
   
    public int index;

    long tickPress;
    public OnUITouchEventDelegate callBackTouch { get; set; }

    //相当于touchDown
    public void OnPointerDown(PointerEventData eventData)
    {
        if (callBackTouch != null)
        {
            isTouchDown = true;
            tickPress = Common.GetCurrentTimeMs();
            callBackTouch(this, eventData, STATUS_TOUCH_DOWN);
        }

    }
    //相当于touchUp
    public void OnPointerUp(PointerEventData eventData)
    {
        if (callBackTouch == null)
        {
            return;
        }

        tickPress = Common.GetCurrentTimeMs() - tickPress;
        if (isTouchDown&&enableLongPress)
        {

            if (tickPress > Time_LongPress*1000)
            {
                callBackTouch(this, eventData, STATUS_LongPress);
                tickPress = 0;
                return;
            }
        }
        if (callBackTouch != null)
        {
            callBackTouch(this, eventData, STATUS_TOUCH_UP);
        }

        if (isTouchDown)
        {
            if (callBackTouch != null)
            {
                callBackTouch(this, eventData, STATUS_Click);
            }

        }

        isTouchDown = false;
    }


}
