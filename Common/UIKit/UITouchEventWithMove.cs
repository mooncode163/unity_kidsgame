using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 
public class UITouchEventWithMove: UITouchEvent, IDragHandler
{  

    //相当于touchMove
    public void OnDrag(PointerEventData eventData)
    {
        if (callBackTouch != null)
        {
            callBackTouch(this,eventData, STATUS_TOUCH_MOVE);
        }
    }

}
