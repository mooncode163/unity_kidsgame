using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public delegate void OnEventColorInputImageClickDelegate(int type,float x_ratio,float y_ratio);
public class EventColorInputImage : ScriptBase, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerClickHandler
{
    public const int TYPE_GREEN_AND_BLUE = 0;
    public const int TYPE_RED = 1;
    public int type = 0;
    public Image imageDot;
    public OnEventColorInputImageClickDelegate callBackClick {get;set;}
    // Use this for initialization
    void Start()
    {
        InitUiScaler();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //相当于touchDown
    public void OnPointerDown(PointerEventData eventData)
    {
       // imageDot.gameObject.SetActive(true);
    }
    //相当于touchUp
    public void OnPointerUp(PointerEventData eventData)
    {


    }
    //相当于touchMove
    public void OnDrag(PointerEventData eventData)
    {

        //  Debug.Log("OnDrag: position=" + eventData.position+" world ="+eventData.pointerPressRaycast.worldPosition);
        //     Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);
        //     pos.z = -1f;
        //            eventData.pointerPress.transform.position = pos;  

    }
    public void OnPointerClick(PointerEventData eventData)
    {
float x_ratio=0,y_ratio=0;
        if (type == TYPE_GREEN_AND_BLUE)
        {
            Vector2 posScreen = eventData.position;
            RectTransform rcTran = GetComponent<RectTransform>();
            imageDot.transform.position = posScreen;

            RectTransform rcTranDot = imageDot.GetComponent<RectTransform>();
            Vector2 posDot = rcTranDot.anchoredPosition;
            float x = rcTran.anchoredPosition.x - rcTran.rect.size.x / 2;
            float y = rcTran.anchoredPosition.y - rcTran.rect.size.y / 2;
            float x_delta = posDot.x - x;
            float y_delta = posDot.y - y;
             x_ratio = x_delta / rcTran.rect.size.x;
             y_ratio = y_delta / rcTran.rect.size.y;
            Debug.Log("gb:x_delta=" + x_delta + " y_delta=" + y_delta + " x_ratio=" + x_ratio + " y_ratio=" + y_ratio + " rect.size=" + rcTran.rect.size);
        }

        if (type == TYPE_RED)
        {
            Vector2 posScreen = eventData.position;
            RectTransform rcTran = GetComponent<RectTransform>();
            Vector2 size_screen = Common.CanvasToScreenSize(sizeCanvas, rcTran.rect.size);

            float x = transform.position.x - size_screen.x / 2;
            float y = transform.position.y - size_screen.y / 2;
            float x_delta = posScreen.x - x;
            float y_delta = posScreen.y - y;
             x_ratio = x_delta / size_screen.x;
             y_ratio = y_delta / size_screen.y;
            Debug.Log("red:x_delta=" + x_delta + " y_delta=" + y_delta + " x_ratio=" + x_ratio + " y_ratio=" + y_ratio + " rect.size=" + rcTran.rect.size);
        }
        if(callBackClick!=null){
            callBackClick(type,x_ratio,y_ratio);
        }
    }

}
