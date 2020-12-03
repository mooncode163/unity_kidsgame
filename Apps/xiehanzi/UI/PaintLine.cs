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
    float lineWidth = 20f;//屏幕像素
    BoxCollider boxCollider;
    public Rect rectMain;
    public MeshTexture meshTex;
    public RenderTexture rtMain;
    public Camera camRender;
    public bool isHasPaint = false;
    Material matLine;
    GameObject objLine;
    int layerLine = 8;
    Color colorLine = Color.red;
    float radio = 1f;
    Texture2D texClear;
    public bool isHasSave = false;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {

        camRender.transform.position = new Vector3(0, 0, camRender.transform.position.z);
        Init();
        CreateLine();
    }

    // Use this for initialization
    void Start()
    {

        UpdateMeshTex();
    }


    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        mainCam.cullingMask |= (1 << layerLine);  // 打开层x
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void Init()
    {
        isHasPaint = false;
        isHasSave = false;
        matLine = new Material(Shader.Find("Custom/PaintLine"));
        int w = (int)(Screen.width * radio);
        int h = (int)(Screen.height * radio);
        rtMain = new RenderTexture(w, h, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
        boxCollider = this.gameObject.AddComponent<BoxCollider>();
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

        UITouchEventWithMove ev = this.gameObject.AddComponent<UITouchEventWithMove>();
        ev.callBackTouch = OnUITouchEvent;

        texClear = new Texture2D(rtMain.width, rtMain.height, TextureFormat.ARGB32, false);
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
    public void SaveImage(string filePath)
    {
        float x = 0, y = 0, w = 0, h = 0;
        w = Common.WorldToScreenWidth(mainCam, rectMain.size.x);
        h = Common.WorldToScreenHeight(mainCam, rectMain.size.y);
        if (rectMain == Rect.zero)
        {
            w = rtMain.width;
            h = rtMain.height;
        }
        x = (rtMain.width - w) / 2;
        y = (rtMain.height - h) / 2;
        Rect rc = new Rect(x, y, w, h);
        Debug.Log("SaveImage rc=" + rc);
        Texture2D tex = TextureUtil.RenderTexture2Texture2D(rtMain, rc);

        Rect rcsave = TextureUtil.GetRectNotAlpha(tex);

        // rcsave.x = 0;
        // rcsave.y = h-rcsave.y;
        Debug.Log("SaveImage rcsave=" + rcsave);
        // GetSubTexture
        Texture2D texsave = TextureUtil.GetSubRenderTexture(rtMain, rcsave, true);

        // texsave = tex;
        TextureUtil.SaveTextureToFile(texsave, filePath);
        isHasSave = true;
    }

    public void ClearAll()
    {
        Clear();
        Graphics.Blit(texClear, rtMain);
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
    }

    //添加世界坐标
    public void AddPoint(Vector3 pos)
    {
        pos.z = 0;
        Debug.Log("PaintLine AddPoint pos=" + pos);
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
        Vector3 pos = Common.GetInputPositionWorld(mainCam);
        if (!IsTouchInRange(pos))
        {
            return;
        }
        AddPoint(pos);
        isHasPaint = true;
        isHasSave = false;
    }
    void onTouchUp()
    {
        Vector3 pos = Common.GetInputPositionWorld(mainCam);
        if (!IsTouchInRange(pos))
        {
            return;
        }
        AddPoint(pos);
    }

}
