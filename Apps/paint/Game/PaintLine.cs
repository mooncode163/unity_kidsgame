using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Vectrosity;

public class PaintLine : UIView
{
    //UITouchEventWithMove uiTouchEvent;
    VectorLine lineCur;
    List<Vector3> listPointCur;
    float lineWidth = 20f;//屏幕像素单位
    BoxCollider boxCollider;
    public Rect rectMain;
    public MeshTexture meshTex;
    public RenderTexture rtMain;
    public Camera camRender;
    Material matLine;
    GameObject objLine;
    int layerLine = 8;
    Color colorLine = Color.white;
    float radio = 1f;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        matLine = new Material(Shader.Find("Custom/PaintLine"));
        camRender.transform.position = new Vector3(0, 0, camRender.transform.position.z);
        CreateLine();
        UpdateMeshTex();

    }

    // Use this for initialization
    void Start()
    {
        objLine = lineCur.GetObj();
        objLine.transform.SetParent(this.gameObject.transform);
        objLine.transform.localPosition = Vector3.zero;
        objLine.layer = layerLine;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Init()
    {
        int w = (int)(Screen.width * radio);
        int h = (int)(Screen.height * radio);
        // rtMain = new RenderTexture(w, h, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
        //boxCollider = this.gameObject.AddComponent<BoxCollider>();
        // uiTouchEvent = this.gameObject.AddComponent<UITouchEventWithMove>();
        // uiTouchEvent.callBackTouch = OnUITouchEvent;
        VectorLine.SetCamera3D(mainCam);
        // rtMain.width = Screen.width;
        // rtMain.height = Screen.height;
        camRender.targetTexture = rtMain;
        // camRender.pixelRect = new Rect((Screen.width - w) / 2, (Screen.height - h) / 2, w, h);
        //camRender.rect = new Rect(0, 0.5f, 1, 1);
        mainCam.cullingMask &= ~(1 << layerLine); // 关闭层x
        // mainCam.cullingMask |= (1 << layer);  // 打开层x

        camRender.cullingMask = (1 << layerLine);
        UpdateMeshTex();
    }

    void UpdateMeshTex()
    {
        if ((meshTex != null) && (rtMain != null))
        {
            meshTex.UpdateTexture(rtMain);
            Vector2 worldsize = Common.GetWorldSize(mainCam) * radio;
            meshTex.UpdateSize(worldsize.x, worldsize.y);
        }
    }

    //单位：屏幕像素
    public void UpdateLineWidth(float w)
    {
        lineWidth = w;
        if (lineCur != null)
        {
            lineCur.lineWidth = lineWidth;
        }
    }

    public void UpdateRect(Rect rc)
    {
        // if (boxCollider != null)
        // {
        //     boxCollider.size = new Vector3(rc.width, rc.height);
        // }

    }
    public void UpdateColor(Color cr)
    {
        colorLine = cr;
        //在draw的时候再更新颜色，防止已经画好的线改变颜色
        //SetLineColor(cr);
    }
    public void Clear()
    {
        if (listPointCur != null)
        {
            listPointCur.Clear();
        }
        Draw();
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
        //AppSceneBase.main.AddObjToMainWorld()
        lineCur.lineType = LineType.Continuous;
        //圆滑填充画线
        lineCur.joins = Joins.Fill;
        lineCur.material = matLine;
        lineCur.color = colorLine;
        SetLineColor(colorLine);

        Draw();
    }
    public void Draw()
    {
        if (lineCur != null)
        {
            SetLineColor(colorLine);
            lineCur.Draw3D();
        }
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
    }

    //添加世界坐标
    public void AddPoint(Vector3 pos)
    {
        pos.z = 0;
        Debug.Log("PaintLine AddPoint pos=" + pos);
        listPointCur.Add(GetTouchLocalPosition(pos));
        //listPointCur.Add(pos);
        Draw();
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
    void onTouchDown()

    {
        // CreateLine(); 
        Clear();

    }
    void onTouchMove()
    {
        // Vector3 pos = GetTouchLocalPosition();


    }
    void onTouchUp()
    {
        // Vector3 pos = GetTouchLocalPosition();

    }

}
