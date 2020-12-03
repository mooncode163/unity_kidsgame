using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public delegate void OnUIScrollViewTouchEventDelegate(PointerEventData eventData, int status);
public class UIScrollViewTouchEvent : MonoBehaviour, IEndDragHandler, IBeginDragHandler
{
    public const int DRAG_BEGIN = 0;
    public const int DRAG_END = 1; 
    public OnUIScrollViewTouchEventDelegate callbackTouch { get; set; }
    public void OnBeginDrag(PointerEventData eventData)
    {
    //   Debug.Log("OnBeginDrag pos=" + eventData.position);
        if (callbackTouch != null)
        {
            callbackTouch(eventData, DRAG_BEGIN);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
       // Debug.Log("OnEndDrag pos=" + eventData.position);

        if (callbackTouch != null)
        {
            callbackTouch(eventData, DRAG_END);
        }
    }
}


