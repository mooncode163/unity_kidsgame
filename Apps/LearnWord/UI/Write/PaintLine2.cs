using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vectrosity;


public delegate void OnPaintLine2Delegate(PaintLine2 ui, int status);
public class PaintLine2 : UIView
{
    //UITouchEventWithMove uiTouchEvent;
    VectorLine lineCur;
    public List<Vector3> listPointCur;
    float lineWidth = 20f;//屏幕像素
    BoxCollider boxCollider;
    // public RawImage image;
    public bool isHasPaint = false;
    Material matLine;
    GameObject objLine;
    int layerLine = 8;
    Color colorLine = Color.red;
    float radio = 1f;
    Texture2D texClear;
    public bool isHasSave = false;
    public bool enableTouch = true;
    float timeDurationClear = 1f;
    int countClear = 0;
    public OnPaintLine2Delegate callBackPaint { get; set; }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public void Awake()
    {
        base.Awake();
        Init();
        CreateLine();
    }

    // Use this for initialization
    public void Start()
    {
        base.Start();
    }


    public void Init()
    {
        isHasPaint = false;
        isHasSave = false;
        matLine = new Material(Shader.Find("Custom/PaintLine"));
        int w = (int)(Screen.width * radio);
        int h = (int)(Screen.height * radio);
        boxCollider = this.gameObject.AddComponent<BoxCollider>();
        VectorLine.SetCamera3D(mainCam);


        UITouchEventWithMove ev = this.gameObject.AddComponent<UITouchEventWithMove>();
        ev.callBackTouch = OnUITouchEvent;

    }


    public void UpdateRect(Rect rc)
    {
        if (boxCollider != null)
        {
            boxCollider.size = new Vector3(rc.width, rc.height);
        }

    }
    public void UpdateColor(Color cr)
    {
        colorLine = cr;
    }
    public void SetLineWidthPixsel(int w)
    {
        lineWidth = w;
        if (lineCur != null)
        {
            lineCur.lineWidth = lineWidth;
        }
    }

    public void ClearAll()
    {
        Clear();
        isHasPaint = false;
    }
    public void Clear()
    {
        if (listPointCur != null)
        {
            listPointCur.Clear();
        }
        if (lineCur != null)
        {
            lineCur.Draw3D();
        }
        isHasPaint = false;
    }

    // 消失动画
    public void ClearAnimate()
    {
        float t = 0.1f;
        int count = (int)(timeDurationClear / t);
        countClear = listPointCur.Count / count;
        if (countClear <= 1)
        {
            countClear = 1;
        }
        InvokeRepeating("OnClearAnimate", t, t);
    }
    void OnClearAnimate()
    {
        if (listPointCur.Count == 0)
        {
            // end
            CancelInvoke("OnClearAnimate");
            Clear();
            return;
        }
        for (int i = 0; i < countClear; i++)
        {
            if(listPointCur.Count>0)
            {
            listPointCur.RemoveAt(listPointCur.Count - 1);
            }
        }
        if (lineCur != null)
        {
            lineCur.Draw3D();
        }
    }

    void SetLineColor(Color cr)
    {
        if (matLine != null)
        {
            matLine.SetColor("_Color", cr);
        }
        if (lineCur != null)
        {
            lineCur.color = cr;
        }
    }
    void CreateLine()
    {

        listPointCur = new List<Vector3>();
        // listPointCur.Add(Vector3.zero);
        // listPointCur.Add(new Vector3(5, 0, 0));

        lineCur = new VectorLine("line", listPointCur, lineWidth);
        lineCur.lineType = LineType.Continuous;
        //圆滑填充画线
        lineCur.joins = Joins.Fill;
        lineCur.Draw3D();
        objLine = lineCur.GetObj();
        objLine.transform.SetParent(this.gameObject.transform);
        objLine.transform.localPosition = Vector3.zero;
        lineCur.material = matLine;
        lineCur.color = colorLine;
        // SetLineColor(colorLine);
        objLine.layer = layerLine;
    }
    public void OnUITouchEvent(UITouchEvent ev, PointerEventData eventData, int status)
    {
        switch (status)
        {
            case UITouchEvent.STATUS_TOUCH_DOWN:
                onTouchDown();
                break;
            case UITouchEvent.STATUS_TOUCH_MOVE:
                onTouchMove();
                break;
            case UITouchEvent.STATUS_TOUCH_UP:
                onTouchUp();
                break;
        }

        if (callBackPaint != null)
        {
            callBackPaint(this, status);
        }
    }

    //添加世界坐标
    public void AddPoint(Vector3 pos)
    {
        pos.z = 0;
        // Debug.Log("PaintLine AddPoint pos=" + pos);
        listPointCur.Add(GetTouchLocalPosition(pos));
        //listPointCur.Add(pos);
        lineCur.Draw3D();
    }
    Vector3 GetTouchLocalPosition(Vector3 pos)
    {
        // Vector2 inputPos = Common.GetInputPosition();
        // Vector3 posTouchWorld = mainCam.ScreenToWorldPoint(inputPos);
        Vector3 loaclPos = this.transform.InverseTransformPoint(pos);
        loaclPos.z = 0;
        // posTouchWorld.z = 0;
        return loaclPos;
    }

    //防止超出边界和广告区域
    bool IsTouchInRange(Vector3 pos)
    {
        bool ret = true;
        Vector2 size = Common.GetWorldSize(mainCam);
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        float y_top_limit = 0, y_bottom_limit = 0;
        float heightTopbarWorld = Common.CanvasToWorldHeight(mainCam, sizeCanvas, 160) + Device.offsetTopWorld;
        y_top_limit = mainCam.orthographicSize - heightTopbarWorld;
        y_bottom_limit = -mainCam.orthographicSize + Device.offsetBottomWithAdBannerWorld;

        if ((pos.y > y_top_limit) || (pos.y < y_bottom_limit))
        {
            ret = false;
        }
        if ((pos.x > (size.x / 2 - Device.offsetRightWorld)) || (pos.x < (-size.x / 2 + Device.offsetLeftWorld)))
        {
            ret = false;
        }
        return ret;
    }
    void onTouchDown()
    {
        // Debug.Log("PaintLine2 onTouchDown rc=");
        if (!enableTouch)
        {
            return;
        }
        // CreateLine(); 
        Clear();
        SetLineColor(colorLine);
        Vector3 pos = Common.GetInputPositionWorld(mainCam);
        if (!IsTouchInRange(pos))
        {
            return;
        }
        AddPoint(pos);
    }
    void onTouchMove()
    {
        // Debug.Log("PaintLine2 onTouchMove 1");
        if (!enableTouch)
        {
            return;
        }
        Vector3 pos = Common.GetInputPositionWorld(mainCam);
        if (!IsTouchInRange(pos))
        {
            return;
        }
        // Debug.Log("PaintLine2 onTouchMove 2");
        AddPoint(pos);
        isHasPaint = true;
        isHasSave = false;
    }
    void onTouchUp()
    {
        if (!enableTouch)
        {
            return;
        }
        Vector3 pos = Common.GetInputPositionWorld(mainCam);
        if (!IsTouchInRange(pos))
        {
            return;
        }
        AddPoint(pos);
    }

}
