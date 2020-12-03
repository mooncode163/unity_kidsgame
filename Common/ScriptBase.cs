using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public delegate void OnInitUIFinishDelegate();
public class ScriptBase : MonoBehaviour
{

    public Camera mainCamera;
    public Vector2 sizeRef;
    public float scaleUI;
    public Vector2 sizeCanvas;
    public bool isHasStarted;
    public float topBarOffsetYNormal;
    public long tickUpdatePre = 0;
    public long tickUpdateCur = 0;
    public int tickUpdateStep;

    public OnInitUIFinishDelegate callbackInitUIFinish { get; set; }

    public void SetCanvasScalerMatch(GameObject obj)
    {
        float w, h;
        CanvasScaler canvasScaler = obj.GetComponent<CanvasScaler>();
        if (canvasScaler == null)
        {
            return;
        }
        sizeRef = canvasScaler.referenceResolution;
        float scale = canvasScaler.scaleFactor;
        w = sizeRef.x;
        h = sizeRef.y;

        if (Screen.height > Screen.width)
        {
            canvasScaler.matchWidthOrHeight = 1;
            canvasScaler.referenceResolution = new Vector2(Mathf.Min(w, h), Mathf.Max(w, h));
        }
        else
        {
            canvasScaler.matchWidthOrHeight = 0;
            canvasScaler.referenceResolution = new Vector2(Mathf.Max(w, h), Mathf.Min(w, h));
        }
    }

    //必须在InitUiScaler前面执行
    public void InitScalerMatch()
    {
        float w, h;
        CanvasScaler canvasScaler = this.gameObject.GetComponent<CanvasScaler>();
        if (canvasScaler == null)
        {
            GameObject obj = GameObject.Find("Canvas");
            if (obj == null)
            {
                obj = GameObject.Find("UI");
            }

            if (obj != null)
            {
                canvasScaler = obj.GetComponent<CanvasScaler>();
            }
            if (canvasScaler == null)
            {
                Debug.Log("InitScalerMatch canvasScaler is null");
                return;
            }
        }
        sizeRef = canvasScaler.referenceResolution;
        float scale = canvasScaler.scaleFactor;
        w = sizeRef.x;
        h = sizeRef.y;

        if (Screen.height > Screen.width)
        {
            //canvasScaler.matchWidthOrHeight = 1.0f * sizeRef.y / sizeRef.x;
            //canvasScaler.referenceResolution = new Vector2(rectRef.y,rectRef.x);
            //rectRef = canvasScaler.referenceResolution;
            canvasScaler.matchWidthOrHeight = 1;
            canvasScaler.referenceResolution = new Vector2(Mathf.Min(w, h), Mathf.Max(w, h));
        }
        else
        {
            canvasScaler.matchWidthOrHeight = 0;
            canvasScaler.referenceResolution = new Vector2(Mathf.Max(w, h), Mathf.Min(w, h));
        }
    }

    public void InitUiScaler()
    {
        CanvasScaler canvasScaler = this.gameObject.GetComponent<CanvasScaler>();
        if (canvasScaler == null)
        {
            GameObject obj = GameObject.Find("Canvas");
            if (obj == null)
            {
                obj = GameObject.Find("UI");
            }

            if (obj != null)
            {
                canvasScaler = obj.GetComponent<CanvasScaler>();
            }
            else
            {
                Debug.Log("InitUiScaler not Find UI");
            }
            if (canvasScaler == null)
            {
                Debug.Log("InitUiScaler canvasScaler is null");
                return;
            }
            Rect rc = (obj.transform as RectTransform).rect;
            sizeCanvas = new Vector2(rc.width, rc.height);
        }
        else
        {
            Rect rc = (this.gameObject.transform as RectTransform).rect;
            sizeCanvas = new Vector2(rc.width, rc.height);
        }
        float scale = canvasScaler.scaleFactor;
        sizeRef = canvasScaler.referenceResolution;

        //Debug.Log("Place:sizeCanvas:" + sizeCanvas);
        float match = canvasScaler.matchWidthOrHeight;

        //match = 0;
        float scale_x = Screen.width / sizeRef.x;
        float scale_y = Screen.height / sizeRef.y;
        scaleUI = scale_x * (1 - match) + scale_y * match;

    }

    public GameObject GetUICanvas()
    {
        GameObject obj = GameObject.Find("Canvas");
        if (obj == null)
        {
            obj = GameObject.Find("UI");
        }
        return obj;
    }
    public void ShowFPS()
    {
        this.gameObject.AddComponent<ShowFPS>();
    }

}
