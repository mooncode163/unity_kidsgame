using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void OnFillColorTouchEventDelegate(PointerEventData eventData, int status);
public class FillColorTouchEvent : ScriptBase, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public const int STATUS_TOUCH_DOWN = 0;
    public const int STATUS_TOUCH_MOVE = 1;
    public const int STATUS_TOUCH_UP = 2;

    public OnFillColorTouchEventDelegate callBackTouch { get; set; }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //相当于touchDown
    public void OnPointerDown(PointerEventData eventData)
    {
        if (callBackTouch != null)
        {
            callBackTouch(eventData, STATUS_TOUCH_DOWN);
        }

    }
    //相当于touchUp
    public void OnPointerUp(PointerEventData eventData)
    {
        if (callBackTouch != null)
        {
            callBackTouch(eventData, STATUS_TOUCH_UP);
        }
    }
    //相当于touchMove
    public void OnDrag(PointerEventData eventData)
    {
        if (callBackTouch != null)
        {
            callBackTouch(eventData, STATUS_TOUCH_MOVE);
        }
    }
}
