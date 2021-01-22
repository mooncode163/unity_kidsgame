using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
public class MeshPaint : ScriptBase
{
    public MeshTextureRender meshTextureRender;
    public MeshDrawLine meshDrawLine;
    public Camera cameraDrawLine; //需要设置为Depth only 以实现保存上一帧的绘画
    public RenderTexture rtPaintNow;//当前实时绘画

    Texture2D texClear;
    public Rect rectPaint = Rect.zero;
    public bool isHasPaint = false;
    public bool isHasSave = false;
    public int lineWidthPixsel//线宽 像素
    {

        get
        {
            string key = "KEY_STR_LINE_WDITH_PIXSEL_MESH_PAINT";
            return PlayerPrefs.GetInt(key, 64);
        }
        set
        {
            string key = "KEY_STR_LINE_WDITH_PIXSEL_MESH_PAINT";
            PlayerPrefs.SetInt(key, value);
        }
    }
    //RenderTexture rtPaintAll;//包括所有的绘画记录

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        int w = Screen.width;
        int h = Screen.height;
        rtPaintNow = new RenderTexture(w, h, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
        rtPaintNow.width = w;
        rtPaintNow.height = h;
        //抗锯齿级别
        rtPaintNow.antiAliasing = 4;
        cameraDrawLine.targetTexture = rtPaintNow;

        texClear = new Texture2D(rtPaintNow.width, rtPaintNow.height, TextureFormat.ARGB32, false);
        ColorImage colorImage = new ColorImage();
        colorImage.Init(texClear);
        //设置全透明背景
         for (int j = 0; j < texClear.height; j++)
        {
            for (int i = 0; i < texClear.width; i++)
            {
                Vector2 pttmp = new Vector2(i, j);

                Color colorpic = colorImage.GetImageColorOrigin(pttmp);
                //统一为纯黑色
                colorpic.r = 0f;
                colorpic.g = 0f;
                colorpic.b = 0f;
                colorpic.a = 0f;
                colorImage.SetImageColor(pttmp, colorpic);
            }
        }
        colorImage.UpdateTexture();
        
        Graphics.Blit(texClear, rtPaintNow);

        Init();

        //ShowFPS();
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }

    void Init()
    {
        isHasPaint = false;
        isHasSave = false;
        meshTextureRender.UpdateTexture(rtPaintNow);
        if (mainCamera == null)
        {
            mainCamera = Common.GetMainCamera();
        }
        Vector2 sizeworld = Common.GetWorldSize(mainCamera);
        //UpdateSize 必须和 UpdateTexture必须同时执行 不然显示会出问题
        meshTextureRender.UpdateSize(sizeworld);

        meshDrawLine.callbackDraw = OnMeshDrawLineDidDraw;
        meshDrawLine.mainCamera = mainCamera;
        float line_width_world = Common.ScreenToWorldWidth(mainCamera, lineWidthPixsel * AppCommon.scaleBase);
        meshDrawLine.setDrawLineWidth(line_width_world);
        meshDrawLine.Init();
        Debug.Log("MeshPaint init end");

    }

    public void SetLineWidthPixsel(int w)
    {
        lineWidthPixsel = w;
        float w_world = Common.ScreenToWorldWidth(mainCamera, w * AppCommon.scaleBase);
        meshDrawLine.setDrawLineWidth(w_world);
    }

    public void UpdateRectPaint(Rect rc)
    {
        rectPaint = rc;
        meshDrawLine.rectDraw = rc;
    }

    public void SetColor(Color color)
    {
        meshDrawLine.SetColor(color);
    }
 
    public void ClearAll()
    {
        meshDrawLine.ClearDraw();
        Graphics.Blit(texClear, rtPaintNow);
        isHasPaint = false;
    }

    public void SaveImage(string filePath)
    {
        float x = 0, y = 0, w = 0, h = 0;
        w = Common.WorldToScreenWidth(mainCamera, rectPaint.size.x);
        h = Common.WorldToScreenHeight(mainCamera, rectPaint.size.y);
        if (rectPaint == Rect.zero)
        {
            w = rtPaintNow.width;
            h = rtPaintNow.height;
        }
        x = (rtPaintNow.width - w) / 2;
        y = (rtPaintNow.height - h) / 2;
        Rect rc = new Rect(x, y, w, h);
        Texture2D tex = TextureUtil.RenderTexture2Texture2D(rtPaintNow, rc);
        TextureUtil.SaveTextureToFile(tex, filePath);

        isHasSave = true;
    }

    public void OnMeshDrawLineDidDraw()
    {
        isHasPaint = true;
        isHasSave = false;
    }
}
