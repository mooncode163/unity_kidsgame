using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void OnPaintColorEraseDelegate();
public delegate void OnGamePaintDidStrawDelegate();
/*
关于场景里的对象添加EventSystem的方法：
因为3d物体上没有canvas上边的GraphicRaycaster所以要Main Camera上添加Physics Raycaster，就相当于UI上的GraphicRaycaster
//https://www.cnblogs.com/lanrenqilanming/p/7001983.html
*/

public class GamePaint : UIView
{

    public const string KEY_STR_LINE_WDITH_PIXSEL = "KEY_STR_LINE_WDITH_PIXSEL";
    public const float MAX_MULTI_TOUCH_SCALE = 256.0f;
    public const float MIN_MULTI_TOUCH_SCALE = 1.0f;
    public const int MODE_PAINT = 0;//涂色
    public const int MODE_FILLCOLR = 1;//填色
    public const int MODE_MAGIC = 2;//魔法
    public const int MODE_SIGN = 3;//印章
    public const int MODE_ERASE = 4;//擦除
    public const int MODE_STRAW = 5;

    public GameObject objSpriteStraw;//颜色吸管 
    public GameObject objSpriteErase;

    public GameObject objSpriteBg;
    public GameObject objPaint;
    public UIGameImage uiGameImage;
    public UIFillColor uiFillColor;
    public MeshTexture meshTex;
    //paint
    public RenderTexture rtMainPaint;
    public Rect rectMain;//world中的显示区域 local position
    public Texture2D texPic;
    public Texture2D texPicOrign;//原始图片

    public Texture2D texPicBlank;//空白图片
    Texture2D texPicFromFile;
    public Texture2D texPicMask;


    Material matPenColor;
    Material matErase;
    Material matPaintMeshTex;
    public bool enableMove = false;
    public bool isFreeDraw = false;


    public int mode;
    public int modePre;

    public ColorItemInfo colorInfo;

    public Color colorStraw;

    ColorImage colorImage;
    bool isTouchDownPic;

    public Texture2D texBrush;

    LayerMask paintLayerMask;
    float brushSize;
    Vector2 pixelUV; // with mouse
    Vector2 pixelUVOld; // with mouse
    Color colorMaskPaint;
    long tickPaint;
    float gamePicOffsetHeight;
    //multi-touch
    bool isMultiTouchDownPic;
    float touchDistance;//两个触摸点之间的距离

    public float scaleGamePic;
    float scaleGamePicX;
    float scaleGamePicY;

    public float scaleGamePicNormal;//初始地图缩放比例 
    float scaleGamePicNormalX;
    float scaleGamePicNormalY;
    float touchDeltaX;    //目标x轴的改变值
    float touchDeltaY;    //目标y轴的改变值 
    bool isFirstMultiMove;
    bool isHaveTouchMove;
    bool isHaveMultiTouch;
    Vector2 ptDown;
    Vector2 ptDownWorld;

    Vector2 sizeMainPaint;
    bool isPainColorClickDown = false;

    bool isFirstClick = true;
    long tickClickPre;
    long tickClickCur;

    public bool isHasPaint = false;
    public bool isHasSave = false;

    UITouchEventWithMove uiTouchEvent;
    BoxCollider boxColliderPaint;

    PaintLine paintLinePrefab;
    PaintLine paintLine;
    Color _colorPaint;
    public OnPaintColorEraseDelegate callBackErase { get; set; }
    public OnGamePaintDidStrawDelegate callBackStraw { get; set; }

    public Color colorPaint
    {
        get
        {
            return _colorPaint;
        }
        set
        {
            _colorPaint = value;
            if (paintLine != null)
            {
                paintLine.UpdateColor(_colorPaint);
            }
            if (uiFillColor != null)
            {
                uiFillColor.colorFill = _colorPaint;
            }
        }
    }
    public int lineWidthPixsel//线宽 像素
    {
        get
        {
            return PlayerPrefs.GetInt(KEY_STR_LINE_WDITH_PIXSEL, 64);
        }
        set
        {
            PlayerPrefs.SetInt(KEY_STR_LINE_WDITH_PIXSEL, value);
        }
    }
    void Awake()
    {
        LoadPrefab();
        mode = MODE_FILLCOLR;
        isPainColorClickDown = false;

        matPenColor = new Material(Shader.Find("Custom/PenColor"));
        matPaintMeshTex = new Material(Shader.Find("Custom/PaintMeshTex"));
        //matErase = new Material(Shader.Find("Custom/EraseBlit"));
        matErase = new Material(Shader.Find("Custom/Erase"));

        objSpriteStraw.SetActive(false);
        objSpriteErase.SetActive(false);

        texPicBlank = TextureCache.main.Load("AppCommon/UI/Game/Blank");
        //  ParseGuanka(); 
        //  AppSceneBase.main.AddObjToMainWorld(objSpritePaintBoardMid);
        //AppSceneBase.main.AddObjToMainWorld(objPaint);
        //  LoadGameTexture(true);

        //   indexSprite = 0;

        uiTouchEvent = objPaint.AddComponent<UITouchEventWithMove>();

        uiTouchEvent.callBackTouch = OnUITouchEvent;


        paintLine = (PaintLine)GameObject.Instantiate(paintLinePrefab);
        paintLine.transform.SetParent(this.transform);
        paintLine.transform.localPosition = new Vector3(0f, 0f, 0f);




    }

    void LoadPrefab()
    {
        {
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/Game/PaintLine");
            paintLinePrefab = obj.GetComponent<PaintLine>();
        }
    }
    public override void LayOut()
    {
        float x = 0, y = 0, z = 0, w = 0, h = 0;
        float scalex = 0, scaley = 0, scale = 0;
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        //objSpriteStraw
        {
            SpriteRenderer sprender = objSpriteStraw.GetComponent<SpriteRenderer>();
            float w_screen = sprender.sprite.texture.width * AppCommon.scaleBase;
            scale = Common.ScreenToWorldWidth(mainCam, w_screen) / (sprender.sprite.texture.width / 100f);
            objSpriteStraw.transform.localScale = new Vector3(scale, scale, 1f);
        }



        //bg
        {
            SpriteRenderer render = objSpriteBg.GetComponent<SpriteRenderer>();
            if (render != null)
            {

                w = render.sprite.texture.width / 100f;
                h = render.sprite.texture.height / 100f;
                if ((w != 0) && (h != 0))
                {
                    scalex = rectMain.width / w;
                    scaley = rectMain.height / h;
                    objSpriteBg.transform.localScale = new Vector3(scalex, scaley, 1f);
                }

                z = objSpriteBg.transform.localPosition.z;
                objSpriteBg.transform.localPosition = new Vector3(rectMain.center.x, rectMain.center.y, z);

            }
        }

        //paint
        {
            z = objPaint.transform.localPosition.z;
            //objPaint.transform.localPosition = new Vector3(rectMain.center.x, rectMain.center.y, z);
            objPaint.transform.localPosition = new Vector3(0, 0, z);
            w = meshTex.width;
            h = meshTex.height;//
            if ((w != 0) && (h != 0))
            {
                scalex = rectMain.width / w;
                scaley = rectMain.height / h;
                scale = Mathf.Min(scalex, scaley);
                //objPaint.transform.localScale = new Vector3(scale, scale, 1f);
            }
        }

        //sprite pic
        if (texPic)
        {
            scale = Common.GetBestFitScale(texPic.width / 100f, texPic.height / 100f, rectMain.width, rectMain.height);
            z = uiGameImage.objSpritePic.transform.localPosition.z;
            uiGameImage.objSpritePic.transform.localPosition = new Vector3(rectMain.center.x, rectMain.center.y, z);

            uiGameImage.objSpritePic.transform.localScale = new Vector3(scale, scale, 1f);

            //uifillcolor
            if (uiFillColor != null)
            {
                z = uiFillColor.objSpriteMask.transform.localPosition.z;
                uiFillColor.objSpriteMask.transform.localPosition = new Vector3(rectMain.center.x, rectMain.center.y, z);
                uiFillColor.objSpriteMask.transform.localScale = new Vector3(scale, scale, 1f);
            }
        }

        if (uiGameImage != null)
        {
            uiGameImage.rectMain = rectMain;
            uiGameImage.LayOut();
        }

    }


    bool isTouchInRange(Vector2 inputPos)
    {
        return true;
        // bool ret = false;
        // Bounds bd = objPaint.GetComponent<Renderer>().bounds;
        // Rect rc = new Rect(bd.center.x - bd.size.x / 2, bd.center.y - bd.size.y / 2, bd.size.x, bd.size.y);
        // //Debug.Log("left:"+bd+"rc="+rc);

        // Vector3 posTouchWorld = mainCam.ScreenToWorldPoint(inputPos);

        // if (rc.Contains(posTouchWorld) && rectMain.Contains(posTouchWorld))
        // {
        //     ret = true;
        // }


        // return ret;
    }
    public void InitFillColor()
    {


    }
    public void InitSign()
    {
        // matSign = new Material(Shader.Find("Custom/PaintSign"));
        // matSign.SetTexture("_MainTex", rtMainPaint);
        // matSign.SetTexture("_Brush", texBrush);
        // matSign.SetTexture("_TexMask", texPicMask);
        // //_BrushW，_BrushH:图片的完整像素为0.5f
        // float brush_w = 0.04f;
        // float brush_h = brush_w;
        // //brushSize = brush_w;
        // matSign.SetFloat("_BrushW", brush_w);//0.5f
        // matSign.SetFloat("_BrushH", brush_h);
        // //在位置设置为中心点
        // matSign.SetVector("_PaintUV", new Vector4(0.5f, 0.5f, 0, 0));
    }
    public void Init()
    {
        isHasPaint = false;
        isHasSave = false;
        ColorItemInfo info = colorInfo;
        colorImage = new ColorImage();
        scaleGamePicNormal = scaleGamePic;

        Renderer render = objPaint.GetComponent<Renderer>();
        sizeMainPaint = render.bounds.size;
        Debug.Log("sizeMainPaint=" + sizeMainPaint + " screen w=" + Screen.width + " h=" + Screen.height);

        //_BrushW，_BrushH:图片的完整像素为0.5f
        float brush_w = 0.04f;
        float brush_h = brush_w;
        brushSize = brush_w;
        Debug.Log("scale:brush_w=" + brush_w + " brush_h=" + brush_h);

        EnbaleErase(false);
        EnbaleFreeDraw(isFreeDraw);

        Material mat = render.material;

        //Texture2D tex = LoadTexture.LoadFromResource("UI/StartUp");
        int w = texPic.width;
        int h = texPic.height;
        // w = (int)Common.WorldToScreenWidth(mainCam, rectMain.width);
        // h = (int)Common.WorldToScreenHeight(mainCam, rectMain.height);
        // w = (int)(rectMain.width * 100f);
        // h = (int)(rectMain.height * 100f);
        w = Screen.width;
        h = Screen.height;
        rtMainPaint = new RenderTexture(w, h, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);

        // mat.SetTexture("_MainTex", rtMainPaint);

        // Texture2D texScale = TextureUtil.ConvertSize(texPic, w, h);
        //Graphics.Blit(texPic, rtMainPaint);
        if (texPic != null)
        {
            TextureUtil.UpdateSpriteTexture(uiGameImage.objSpritePic, texPic);
        }


        UpdateMainPaintScale();
        UpdateLineWidth();
        InitSign();
        InitFillColor();
        meshTex.EnableTouch(true);
        meshTex.UpdateMaterial(matPaintMeshTex);
        matPaintMeshTex = meshTex.GetMaterial();
        meshTex.UpdateTexture(rtMainPaint);
        Vector2 worldsize = Common.GetWorldSize(mainCam);
        meshTex.UpdateSize(worldsize.x, worldsize.y);

        //只显示绘图区域
        // float uv_dispx = (rectMain.x + worldsize.x / 2) / worldsize.x;
        // float uv_dispy = (rectMain.y + worldsize.y / 2) / worldsize.y;
        // float uv_dispw = rectMain.width / worldsize.x;
        // float uv_disph = rectMain.height / worldsize.y;
        Rect rc_uv = GetUVRectOfPaint();
        matPaintMeshTex.SetFloat("_DispX", rc_uv.x);
        matPaintMeshTex.SetFloat("_DispY", rc_uv.y);
        matPaintMeshTex.SetFloat("_DispW", rc_uv.width);
        matPaintMeshTex.SetFloat("_DispH", rc_uv.height);

        if (paintLine != null)
        {
            paintLine.rtMain = rtMainPaint;
            paintLine.Init();
            paintLine.UpdateRect(rectMain);
            paintLine.UpdateColor(colorPaint);
        }

        if (uiGameImage != null)
        {
            uiGameImage.rtMain = rtMainPaint;
            uiGameImage.Init();
        }

        //uifillcolor
        uiFillColor.cam.targetTexture = rtMainPaint;
        uiFillColor.UpdateMask(texPicMask);
        uiFillColor.colorFill = colorPaint;

        if (isFreeDraw)
        {
            uiFillColor.gameObject.SetActive(false);
            uiGameImage.objSpritePic.SetActive(false);
        }
        else
        {
            uiFillColor.gameObject.SetActive(true);
            uiGameImage.objSpritePic.SetActive(true);
        }

        matPaintMeshTex.SetTexture("_TexErase", texBrush);
    }


    public void DoClickBtnStraw()
    {
        objSpriteStraw.SetActive(!objSpriteStraw.activeSelf);

        if (objSpriteStraw.activeSelf)
        {
            objSpriteErase.SetActive(false);
            modePre = mode;
            mode = MODE_STRAW;
        }
        else
        {
            //恢复之前的模式
            mode = modePre;
            if (mode == MODE_ERASE)
            {
                objSpriteErase.SetActive(true);
            }
            // }
        }
    }
    void UpdateErase()
    {

        float x, y, w, h;


        // w = (texBrush.width) * 1f / (rtMainPaint.width);
        // h = (texBrush.height) * 1f / (rtMainPaint.height);

        // matPaintMeshTex.SetFloat("_EraseW", w);//0.5f
        // matPaintMeshTex.SetFloat("_EraseH", h);
        // matPaintMeshTex.SetTexture("_TexErase", texBrush);

        // Vector3 posworld = Common.GetInputPositionWorld(mainCam);
        // Vector3 poslocal = this.transform.InverseTransformPoint(posworld);
        // w = meshTex.width;
        // h = meshTex.height;
        // x = (poslocal.x + w / 2) / w;
        // y = (poslocal.y + h / 2) / h;

        // matPaintMeshTex.SetFloat("_EraseUvX", x);
        // matPaintMeshTex.SetFloat("_EraseUvY", y);


        uiGameImage.UpdateErase();
    }
    public void UpdateLineWidth()
    {
        Renderer render = objPaint.GetComponent<Renderer>();
        sizeMainPaint = render.bounds.size;
        float line_width_world = Common.ScreenToWorldWidth(mainCam, lineWidthPixsel * AppCommon.scaleBase);
        float brush_w = (line_width_world / sizeMainPaint.x) / 2;
        float brush_h = brush_w;
        brushSize = brush_w;
        //  Debug.Log("scale:brush_w=" + brush_w + " brush_h=" + brush_h);
        if (paintLine != null)
        {
            paintLine.UpdateLineWidth(lineWidthPixsel * AppCommon.scaleBase);
        }
    }


    public void UpdateEraseLineWidth()
    {
        Renderer render = objPaint.GetComponent<Renderer>();
        sizeMainPaint = render.bounds.size;
        float line_width_world = Common.ScreenToWorldWidth(mainCam, 80 * AppCommon.scaleBase);
        float brush_w = (line_width_world / sizeMainPaint.x) / 2;
        float brush_h = brush_w;
        brushSize = brush_w;
        //  Debug.Log("scale:brush_w=" + brush_w + " brush_h=" + brush_h);

    }
    void UpdateMainPaintScale()
    {
        float w_tex = texPic.width / 100f;
        float h_tex = texPic.height / 100f;
        float w_disp = scaleGamePic * w_tex;
        float h_disp = scaleGamePic * h_tex;

        scaleGamePicX = w_disp / sizeMainPaint.x;
        scaleGamePicY = h_disp / sizeMainPaint.y;
    }


    void EnbaleErase(bool enable)
    {
        // matPaint.SetInt("_isErase", enable ? 1 : 0);
    }
    void EnbaleFreeDraw(bool enable)
    {
        //  matPaint.SetInt("_isFreeDraw", enable ? 1 : 0);
    }
    Rect GetUVRectOfPaint()
    {
        LayOut();
        Vector2 worldsize = Common.GetWorldSize(mainCam);
        Rect world_rect = AppSceneBase.main.GetRectMainWorld().rect;
        //Vector3 posworld = this.transform.TransformPoint(new Vector3(rectMain.x, rectMain.y, 0)); 
        SpriteRenderer rd = objSpriteBg.GetComponent<SpriteRenderer>();
        float x = (rd.bounds.center.x - rd.bounds.size.x / 2) + world_rect.size.x / 2;
        float y = (rd.bounds.center.y - rd.bounds.size.y / 2) + world_rect.size.y / 2;
        Debug.Log("worldsize=" + worldsize + " world_rect=" + world_rect + " y=" + y);
        //只显示绘图区域
        float uv_x = x / worldsize.x;
        float uv_y = y / worldsize.y;
        float uv_w = rd.bounds.size.x / worldsize.x;
        float uv_h = rd.bounds.size.y / worldsize.y;
        return new Rect(uv_x, uv_y, uv_w, uv_h);
    }
    public void SaveImage(string filePath)
    {
        Texture2D texSave = TextureUtil.RenderTexture2Texture2D(rtMainPaint);
        if (texSave)
        {
            Rect rc_uv = GetUVRectOfPaint();
            int oft = 1;//四舍五入
            float x = rc_uv.x * rtMainPaint.width + oft;
            float y = rc_uv.y * rtMainPaint.height;
            float w = rc_uv.width * rtMainPaint.width;
            float h = rc_uv.height * rtMainPaint.height;
            if (x > (int)x)
            {
                //四舍五入
                x += oft;
                w -= 2 * oft;
            }
            if (y > (int)y)
            {
                //四舍五入
                y -= oft;
                h -= 2 * oft;
            }
            Rect rc = new Rect(x, y, w, h);
            Debug.Log("SaveImage:rc=" + rc + " rc_uv=" + rc_uv + " filePath=" + filePath + " rtMainPaint.width=" + rtMainPaint.width + " rtMainPaint.height=" + rtMainPaint.height);
            // rc.y = 40;

            //顶点为y坐标0
            rc.y = rtMainPaint.height - (rc.y + rc.height);
            Texture2D tex = TextureUtil.GetSubTexture(texSave, rc);
            TextureUtil.SaveTextureToFile(tex, filePath);
        }

        isHasSave = true;
    }

    public void UpdateBg(string pic)
    {
        TextureUtil.UpdateSpriteTexture(objSpriteBg, pic);
        SpriteRenderer render = objSpriteBg.GetComponent<SpriteRenderer>();

        LayOut();
    }

    public void UpdateGamePic(string pic)
    {
        Texture2D tex = TextureCache.main.Load(pic);
        boxColliderPaint = objPaint.GetComponent<BoxCollider>();
        if (boxColliderPaint != null)
        {
            boxColliderPaint.size = new Vector2(tex.width / 100f, tex.height / 100f);
        }

        Init();
        LayOut();
    }
    void limitSpritePos(bool isTouchUp)
    {
        Vector3 ptNow = this.transform.position;
        SpriteRenderer spRender = this.GetComponent<SpriteRenderer>();
        Bounds bd = spRender.bounds;
        Vector2 size = Common.ScreenToWorldSize(mainCam, new Vector2(Screen.width, Screen.height - gamePicOffsetHeight));

        if (isTouchUp)
        {
            //居中显示
            //if (spriteGame->getBoundingBox().size.width<frame.size.width)
            {
                ptNow.x = 0;
            }
            //if (spriteGame->getBoundingBox().size.height<frame.size.height)
            {
                ptNow.y = 0;
            }
        }
        else
        {

            {
                //左边
                if ((ptNow.x - bd.size.x / 2) > -size.x / 2)
                {
                    ptNow.x = bd.size.x / 2 - size.x / 2;
                    if (bd.size.x < size.x)
                    {
                        //显示在中心
                        ptNow.x = 0;
                    }
                }
                //右边
                if ((ptNow.x + bd.size.x / 2) < size.x / 2)
                {
                    ptNow.x = size.x / 2 - bd.size.x / 2;
                    if (bd.size.x < size.x)
                    {
                        //显示在中心
                        ptNow.x = 0;
                    }
                }

            }


            {
                //底部
                if ((ptNow.y - bd.size.y / 2) > -size.y / 2)
                {
                    ptNow.y = bd.size.y / 2 - size.y / 2;
                    if (bd.size.y < size.y)
                    {
                        //显示在中心
                        ptNow.y = 0;
                    }
                }
                //顶部
                if ((ptNow.y + bd.size.y / 2) < size.y / 2)
                {
                    ptNow.y = size.y / 2 - bd.size.y / 2;
                    if (bd.size.y < size.y)
                    {
                        //显示在中心
                        ptNow.y = 0;
                    }
                }
            }
        }

        ptNow.z = this.transform.position.z;
        this.transform.position = ptNow;

        // ptNow.z = objSpriteFill.transform.position.z;
        // objSpriteFill.transform.position = ptNow;

    }


    void DrawPoint(float x, float y)
    {
        //在位置设置为中心点
        // matPaint.SetVector("_PaintUV", new Vector4(x, y, 0, 0));
        // matPaint.SetVector("_ColorMask", new Vector4(colorMaskPaint.r, colorMaskPaint.g, colorMaskPaint.b, colorMaskPaint.a));

        // //这种方法在android上无法DrawLine
        // // Graphics.Blit(rtMainPaint, rtMainPaint, matPaint);

        // var rtTmp = RenderTexture.GetTemporary(rtMainPaint.width, rtMainPaint.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
        // Graphics.Blit(rtMainPaint, rtTmp, matPaint);
        // Graphics.Blit(rtTmp, rtMainPaint);
        // RenderTexture.ReleaseTemporary(rtTmp);
    }



    // draw line between 2 points (if moved too far/fast)
    // http://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm
    //坐标单位为像素 int
    public void DrawLine(Vector2 start, Vector2 end, int lineSize)
    {
        int x0 = (int)start.x;
        int y0 = (int)start.y;
        int x1 = (int)end.x;
        int y1 = (int)end.y;
        int dx = Mathf.Abs(x1 - x0); // TODO: try these? http://stackoverflow.com/questions/6114099/fast-integer-abs-function
        int dy = Mathf.Abs(y1 - y0);
        int sx, sy;
        if (x0 < x1) { sx = 1; } else { sx = -1; }
        if (y0 < y1) { sy = 1; } else { sy = -1; }
        int err = dx - dy;
        bool loop = true;
        //			int minDistance=brushSize-1;
        int minDistance = (int)(lineSize >> 1); // divide by 2, you might want to set mindistance to smaller value, to avoid gaps between brushes when moving fast
        int pixelCount = 0;
        int e2;
        while (loop)
        {
            pixelCount++;
            if (pixelCount > minDistance)
            {
                pixelCount = 0;
                DrawPoint(x0 * 1f / rtMainPaint.width, y0 * 1f / rtMainPaint.height);
            }
            if ((x0 == x1) && (y0 == y1)) loop = false;
            e2 = 2 * err;
            if (e2 > -dy)
            {
                err = err - dy;
                x0 = x0 + sx;
            }
            if (e2 < dx)
            {
                err = err + dx;
                y0 = y0 + sy;
            }
        }
    } // drawline



    public void UpdateMode(int m)
    {
        mode = m;
        switch (mode)
        {
            case MODE_PAINT:
                uiFillColor.Clear();

                break;
            case MODE_FILLCOLR:
                paintLine.Clear();

                break;

            case MODE_MAGIC:

                break;
            case MODE_ERASE:
                uiFillColor.Clear();
                paintLine.Clear();
                break;
            case MODE_SIGN:
                uiFillColor.Clear();
                paintLine.Clear();
                break;


        }

        if (uiGameImage != null)
        {
            uiGameImage.UpdateMode(m);
        }
    }


    void onTouchImage(Vector2 pt, int status)
    {
        EnbaleErase(false);
        switch (mode)
        {
            case MODE_PAINT:
                isHasPaint = true;
                onPaint(pt, status);
                break;
            case MODE_FILLCOLR:
                if (status == UITouchEvent.STATUS_TOUCH_UP)
                {
                    isHasPaint = true;
                    // onFillColor(pt);
                }

                break;

            case MODE_MAGIC:
                isHasPaint = true;
                onMagic(pt);
                break;
            case MODE_ERASE:
                {
                    EnbaleErase(true);
                    UpdateEraseLineWidth();
                    onErase(pt, status);
                }
                break;
            case MODE_SIGN:
                isHasPaint = true;
                if (status == UITouchEvent.STATUS_TOUCH_UP)
                {
                    onSign(pt);
                }

                break;


        }
    }

    void onPaint(Vector2 pt, int status)
    {
        // tickPaint = Common.GetCurrentTimeMs();
        var ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        // if (Physics.Raycast(ray, out hitInfo))
        {
            pixelUVOld = pixelUV;
            Vector2 uv = Vector2.zero;
            // uv = hitInfo.textureCoord;
            pixelUV = uv;
            //uv坐标转换成世界坐标
            Renderer rd = meshTex.GetComponent<Renderer>();
            float x = (uv.x - 0.5f) * rd.bounds.size.x;
            float y = (uv.y - 0.5f) * rd.bounds.size.y;
            Vector3 posWorld = new Vector3(x, y, 0);

            Vector2 inputPos = Common.GetInputPosition();
            Vector3 posTouchWorld = mainCam.ScreenToWorldPoint(inputPos);
            posWorld = posTouchWorld;


            paintLine.AddPoint(posWorld);
        }

    }

    void onMagic(Vector2 pt)
    {

    }

    void onSign(Vector2 pt)
    {

        if (uiGameImage != null)
        {
            uiGameImage.UpdateSignPic();
            uiGameImage.UpdateSignColor(colorPaint);
        }
        /* 
                RaycastHit hitInfo;
                Vector2 uv = Vector2.zero;
                var ray = mainCam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hitInfo))
                {
                    uv = hitInfo.textureCoord;
                }
                int idx = Random.Range(0, 3);
                //idx =0;
                matSign.SetVector("_ControlColor", new Vector4(colorPaint.r, colorPaint.g, colorPaint.b, colorPaint.a));
                Texture2D texSign = LoadTexture.LoadFromResource("AppCommon/UI/Game/Sign/icon_sign" + idx);
                matSign.SetTexture("_Brush", texSign);
                Renderer render = objPaint.GetComponent<Renderer>();
                sizeMainPaint = render.bounds.size;
                float line_width_world = Common.ScreenToWorldWidth(mainCam, texSign.width * AppCommon.scaleBase);
                float brush_w = (line_width_world / sizeMainPaint.x) / 2;
                float brush_h = brush_w;
                //brushSize = brush_w;
                matSign.SetFloat("_BrushW", brush_w);//0.5f
                matSign.SetFloat("_BrushH", brush_h);
                //在位置设置为中心点
                matSign.SetVector("_PaintUV", new Vector4(uv.x, uv.y, 0, 0));

                float rotation_min = -30f;
                float rotation_max = 30f;
                int rdm = Random.Range(0, 100);
                float rotation = rotation_min + (rotation_max - rotation_min) * rdm / 100;
                matSign.SetFloat("_BrushRotate", rotation);

                var rtTmp = RenderTexture.GetTemporary(rtMainPaint.width, rtMainPaint.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
                Graphics.Blit(rtMainPaint, rtTmp, matSign);
                Graphics.Blit(rtTmp, rtMainPaint);
                RenderTexture.ReleaseTemporary(rtTmp);
                */
    }

    void onErase(Vector2 pt, int status)
    {
        float x, y, w, h;
        if (status == UITouchEvent.STATUS_TOUCH_DOWN)
        {



            w = (texBrush.width) * 1f / (rtMainPaint.width);
            h = (texBrush.height) * 1f / (rtMainPaint.height);

            matErase.SetFloat("_EraseW", w);//0.5f
            matErase.SetFloat("_EraseH", h);
            matErase.SetTexture("_TexErase", texBrush);

        }

        uiGameImage.UpdateEraseMaterial(matErase);
        matErase = uiGameImage.GetEraseMaterial();

        Vector3 posworld = Common.GetInputPositionWorld(mainCam);
        Vector3 poslocal = objPaint.transform.InverseTransformPoint(posworld);
        w = meshTex.width;
        h = meshTex.height;
        x = (poslocal.x + w / 2) / w;
        y = (poslocal.y + h / 2) / h;

        matErase.SetFloat("_EraseUvX", x);
        matErase.SetFloat("_EraseUvY", y);
        matErase.SetTexture("_TexContent", rtMainPaint);

        posworld.z = uiGameImage.objSpriteErase.transform.position.z;
        uiGameImage.objSpriteErase.transform.position = posworld;
        // UpdateErase();
        Debug.Log("onErase  uv x=" + x + " y=" + y);
    }

    //blit touch move的时候不流畅
    void onEraseByBlit(Vector2 pt, int status)
    {
        float x, y, w, h;
        if (status == UITouchEvent.STATUS_TOUCH_DOWN)
        {



            w = (texBrush.width) * 1f / (rtMainPaint.width);
            h = (texBrush.height) * 1f / (rtMainPaint.height);

            matErase.SetFloat("_EraseW", w);//0.5f
            matErase.SetFloat("_EraseH", h);
            matErase.SetTexture("_TexErase", texBrush);

        }


        // onPaint(pt, UITouchEvent.STATUS_TOUCH_DOWN);

        //uiGameImage.UpdateEraseMaterial(matErase);
        // matErase = uiGameImage.GetEraseMaterial();

        Vector3 posworld = Common.GetInputPositionWorld(mainCam);
        Vector3 poslocal = objPaint.transform.InverseTransformPoint(posworld);
        w = meshTex.width;
        h = meshTex.height;
        x = (poslocal.x + w / 2) / w;
        y = (poslocal.y + h / 2) / h;

        matErase.SetFloat("_EraseUvX", x);
        matErase.SetFloat("_EraseUvY", y);


        posworld.z = uiGameImage.objSpriteErase.transform.position.z;
        // uiGameImage.objSpriteErase.transform.position = posworld;

        //Graphics.Blit(texBrush, rtMainPaint, matErase);
        long tick = Common.GetCurrentTimeMs();
        var rtTmp = RenderTexture.GetTemporary(rtMainPaint.width, rtMainPaint.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
        Graphics.Blit(rtMainPaint, rtTmp, matErase);
        Graphics.Blit(rtTmp, rtMainPaint);
        RenderTexture.ReleaseTemporary(rtTmp);
        tick = Common.GetCurrentTimeMs() - tick;
        if (status == UITouchEvent.STATUS_TOUCH_MOVE)
        {
            Debug.Log("onErase texBrush uv w=" + w + " h=" + h + " tick=" + tick + "ms");
        }
        // if (callBackErase != null)
        // {
        //     callBackErase();
        // }
    }
    public void EraseAll()
    {
        isHasPaint = false;
        isHasSave = false;
        //恢复
        Graphics.Blit(texPicBlank, rtMainPaint);
        if (uiFillColor != null)
        {
            uiFillColor.Clear();
        }
        if (paintLine != null)
        {
            paintLine.Clear();
        }
    }


    RenderTexture SetupRenderTexture(Texture baseTex, string texName, Material material)
    {
        var rt = new RenderTexture(baseTex.width, baseTex.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
        rt.filterMode = baseTex.filterMode;
        Graphics.Blit(baseTex, rt);
        material.SetTexture(texName, rt);

        return rt;
    }

    void CreateFullScreenQuad()
    {
        // Vector2 canvasSizeAdjust = new Vector2(0, 0); // this means, "ScreenResolution.xy+screenSizeAdjust.xy" (use only minus values, to add un-drawable border on right or bottom)
        // Camera cam = mainCam;
        // Debug.Log("");
        // // create mesh plane, fits in camera view (with screensize adjust taken into consideration)
        // Mesh go_Mesh = objPaint.GetComponent<MeshFilter>().mesh;
        // go_Mesh.Clear();
        // go_Mesh.vertices = new[] {
        //         cam.ScreenToWorldPoint(new Vector3(0, canvasSizeAdjust.y, cam.nearClipPlane + 0.1f)), // bottom left
        // 		cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight+canvasSizeAdjust.y, cam.nearClipPlane + 0.1f)), // top left
        // 		cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth+canvasSizeAdjust.x, cam.pixelHeight+canvasSizeAdjust.y, cam.nearClipPlane + 0.1f)), // top right
        // 		cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth+canvasSizeAdjust.x, canvasSizeAdjust.y, cam.nearClipPlane + 0.1f)) // bottom right
        // };
        // go_Mesh.uv = new[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };
        // go_Mesh.triangles = new[] { 0, 1, 2, 0, 2, 3 };

        // // TODO: add option for this
        // go_Mesh.RecalculateNormals();

        // // TODO: add option to calculate tangents
        // go_Mesh.tangents = new[] { new Vector4(1.0f, 0.0f, 0.0f, -1.0f), new Vector4(1.0f, 0.0f, 0.0f, -1.0f), new Vector4(1.0f, 0.0f, 0.0f, -1.0f), new Vector4(1.0f, 0.0f, 0.0f, -1.0f) };


        // // add mesh collider
        // objPaint.AddComponent<MeshCollider>();
    }

    Color GetColorOfPosUVNg(Vector2 uv)
    {
        Color color = Color.white;
        int x = (int)(uv.x * rtMainPaint.width);
        int y = (int)(uv.y * rtMainPaint.height);
        Rect rect = new Rect(x, y, 10, 10);
        // Rect rect = new Rect(0, 0, rtMainPaint.width, rtMainPaint.height);
        // 激活这个rt, 并从中中读取像素。  
        RenderTexture.active = rtMainPaint;
        Texture2D tex = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.ARGB32, false);
        // rect.x =0;
        // rect.y = 0;
        tex.ReadPixels(rect, 0, 0);// 注：这个时候，它是从RenderTexture.active中读取像素  
        tex.Apply();
        x = 0;
        y = 0;
        color = tex.GetPixel(x, y);
        //ps: camera2.targetTexture = null;  
        RenderTexture.active = null; // JC: added to avoid errors  
        return color;
    }

    Color GetColorOfPosUV(Vector2 uv)
    {
        Color color = Color.white;
        int x = (int)(uv.x * rtMainPaint.width);
        int y = (int)(uv.y * rtMainPaint.height);
        // Rect rect = new Rect(x, y, 1, 1);
        Rect rect = new Rect(0, 0, rtMainPaint.width, rtMainPaint.height);
        // 激活这个rt, 并从中中读取像素。  
        RenderTexture.active = rtMainPaint;
        Texture2D tex = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.ARGB32, false);
        // rect.x =0;
        // rect.y = 0;
        tex.ReadPixels(rect, 0, 0);// 注：这个时候，它是从RenderTexture.active中读取像素  
        tex.Apply();
        color = tex.GetPixel(x, y);
        //ps: camera2.targetTexture = null;  
        RenderTexture.active = null; // JC: added to avoid errors  
        tex = null;
        return color;
    }

    public void OnUITouchEvent(UITouchEvent ev, PointerEventData eventData, int status)
    {
        if (mode == MODE_PAINT)
        {
            paintLine.OnUITouchEvent(ev, eventData, status);
            //return;
        }
        if (mode == MODE_FILLCOLR)
        {
            uiFillColor.OnUITouchEvent(ev, eventData, status);
            // return;
        }
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

    bool isTouchInRangeOfPic(Vector2 inputPos)
    {
        bool ret = false;
        Bounds bd = meshTex.GetComponent<Renderer>().bounds;
        Rect rc = new Rect(bd.center.x - bd.size.x / 2, bd.center.y - bd.size.y / 2, bd.size.x, bd.size.y);
        //Debug.Log("left:"+bd+"rc="+rc);

        Vector3 posTouchWorld = mainCam.ScreenToWorldPoint(inputPos);

        Vector2 size = Common.ScreenToWorldSize(mainCam, new Vector2(Screen.width, Screen.height - gamePicOffsetHeight));
        Rect rcSize = new Rect(-size.x / 2, -size.y / 2, size.x, size.y);

        if (rc.Contains(posTouchWorld) && rcSize.Contains(posTouchWorld))
        {
            ret = true;
        }

        return ret;
    }
    void onTouchDown()
    {

        if (isPainColorClickDown == false)
        {
            //return;
        }
        isHaveTouchMove = false;
        isHaveMultiTouch = false;
        isTouchDownPic = true;
        // Bounds bd = objSpritePaintPic.GetComponent<SpriteRenderer>().bounds;
        // Rect rc = new Rect(bd.center.x - bd.size.x / 2, bd.center.y - bd.size.y / 2, bd.size.x, bd.size.y);
        //Debug.Log("left:"+bd+"rc="+rc);
        Vector2 inputPos = Common.GetInputPosition();
        Vector3 posTouchWorld = mainCam.ScreenToWorldPoint(inputPos);
        ptDown = inputPos;
        ptDownWorld = this.transform.position;
        //Debug.Log("onTouchDown:ptDown=" + ptDown);
        onTouchImage(posTouchWorld, UITouchEvent.STATUS_TOUCH_DOWN);

        if (isTouchInRangeOfPic(inputPos))
        {
            isTouchDownPic = true;

            //on straw
            if (objSpriteStraw.activeSelf)
            {
                var ray = mainCam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    isTouchDownPic = false;
                    Vector2 uv = hitInfo.textureCoord;

                    Color color = GetColorOfPosUV(uv);
                    Debug.Log("OnPointerDown color=" + color);
                    // colorStraw = color;

                    posTouchWorld.z = objSpriteStraw.transform.position.z;
                    objSpriteStraw.transform.position = posTouchWorld;
                    if (color.a != 0)
                    {
                        colorPaint = color;
                        if (callBackStraw != null)
                        {
                            callBackStraw();
                        }
                    }


                }

            }
        }
    }
    void onTouchMove()
    {

        if (!isTouchDownPic)
        {
            return;
        }
        Vector2 inputPos = Common.GetInputPosition();
        Vector3 posTouchWorld = mainCam.ScreenToWorldPoint(inputPos);
        onTouchImage(posTouchWorld, UITouchEvent.STATUS_TOUCH_MOVE);
        // return;
        if (!enableMove)
        {
            return;
        }
        //移动图片
        Vector2 pt = Common.GetInputPosition();
        float oftx = pt.x - ptDown.x;
        oftx = Common.ScreenToWorldWidth(mainCam, oftx);
        float ofty = pt.y - ptDown.y;
        ofty = Common.ScreenToWorldHeight(mainCam, ofty);
        Vector3 pos = this.transform.position;
        float x = ptDownWorld.x + oftx;
        float y = ptDownWorld.y + ofty;


        float touch_move = Mathf.Sqrt((pt.x - ptDown.x) * (pt.x - ptDown.x) + (pt.y - ptDown.y) * (pt.y - ptDown.y));
        if (touch_move > Common.TOUCH_MOVE_STEP_MIN)
        {
            isHaveTouchMove = true;
        }
        Debug.Log("onTouchMove:touch_move=" + touch_move);

        //if (scaleGamePic > scaleGamePicNormal)
        if (isHaveTouchMove)
        {
            // spriteGame->setPosition(posNew);
            // isMoveByUser = true;
            if (!isHaveMultiTouch)
            {
                float z = pos.z;
                this.transform.position = new Vector3(x, y, z);
                z = this.transform.position.z;
                this.transform.position = new Vector3(x, y, z);
            }
            //limitSpritePos(false); 


        }

    }

    void onTouchUp()
    {
        if (!isTouchDownPic)
        {
            Debug.Log("onTouchUp not isTouchDownPic");
            return;
        }
        isTouchDownPic = false;
        //   onClick();
        Vector2 inputPos = Common.GetInputPosition();
        Vector3 posTouchWorld = mainCam.ScreenToWorldPoint(inputPos);
        //Debug.Log("onTouchUp posTouchWorld=" + posTouchWorld);
        if (isTouchInRange(inputPos))
        {
            RaycastHit hitInfo;
            var ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo))
            {

                //读取 RaycastHit.textureCoord 必须使用MeshCollider 否则返回zero
                Vector2 uv = hitInfo.textureCoord;
                pixelUV = uv;

            }

            // colorMaskPaint = GetColorMask(pixelUV);
            Debug.Log("onTouchUp:uv=" + pixelUV + " colorMaskPaint=" + colorMaskPaint);
            // if (colorMaskPaint != Color.black)
            {
                onTouchImage(posTouchWorld, UITouchEvent.STATUS_TOUCH_UP);
            }
        }


        if (isHaveTouchMove)
        {
            limitSpritePos(false);
        }
    }


    //多点触摸
    void onMultiTouchDown()
    {

        isMultiTouchDownPic = false;
        isHaveMultiTouch = false;
        // Bounds bd = objSpritePaintPic.GetComponent<SpriteRenderer>().bounds;
        // Rect rc = new Rect(bd.center.x - bd.size.x / 2, bd.center.y - bd.size.y / 2, bd.size.x, bd.size.y);
        //Debug.Log("left:"+bd+"rc="+rc);
        Vector2 inputPos0 = Input.touches[0].position; ;
        Vector3 posTouchWorld0 = mainCam.ScreenToWorldPoint(inputPos0);
        Vector2 inputPos1 = Input.touches[1].position; ;
        Vector3 posTouchWorld1 = mainCam.ScreenToWorldPoint(inputPos1);

        isFirstMultiMove = true;
        isHaveTouchMove = false;

        if (isTouchInRange(inputPos0) || isTouchInRange(inputPos1))
        {
            isMultiTouchDownPic = true;
            Vector3 pos = this.transform.position;
            touchDeltaX = (posTouchWorld0.x + posTouchWorld1.x) / 2 - pos.x;       //计算新的偏移量
            touchDeltaY = (posTouchWorld0.y + posTouchWorld1.y) / 2 - pos.y;
        }

    }
    void onMultiTouchMove()
    {
        if (!isMultiTouchDownPic)
        {
            return;
        }

        //计算两个触摸点距离
        Vector2 inputPos0 = Input.touches[0].position; ;
        Vector3 pt0 = mainCam.ScreenToWorldPoint(inputPos0);
        Vector2 inputPos1 = Input.touches[1].position; ;
        Vector3 pt1 = mainCam.ScreenToWorldPoint(inputPos1);
        float distance_move = Mathf.Sqrt((pt1.x - pt0.x) * (pt1.x - pt0.x) + (pt1.y - pt0.y) * (pt1.y - pt0.y));

        if (isFirstMultiMove)
        {
            isFirstMultiMove = false;
            touchDistance = distance_move;
        }
        if (touchDistance > 0)
        {
            isHaveTouchMove = true;
            isHaveMultiTouch = true;
            //   新的距离 / 老的距离  * 原来的缩放比例，即为新的缩放比例
            scaleGamePic = distance_move * scaleGamePic / touchDistance;                      //   新的距离 / 老的距离  * 原来的缩放比例，即为新的缩放比例
                                                                                              //limit
            if (scaleGamePic > MAX_MULTI_TOUCH_SCALE)
            {
                scaleGamePic = MAX_MULTI_TOUCH_SCALE;
            }
            if (scaleGamePic < MIN_MULTI_TOUCH_SCALE)
            {
                //scaleImage = MIN_MULTI_TOUCH_SCALE;
            }
        }
        touchDistance = distance_move;

        float scale = 0;
        float z = this.transform.position.z;
        if (scaleGamePic > scaleGamePicNormal)
        {
            scale = scaleGamePic;
            this.transform.localScale = new Vector3(scale, scale, 1f);
            //objSpriteFill.transform.localScale = new Vector3(scale, scale, 1f);
        }
        else
        {
            //恢复原来位置 

            this.transform.position = new Vector3(0, 0, z);
            scale = scaleGamePicNormal;
            this.transform.localScale = new Vector3(scale, scale, 1f);

            // z = objSpriteFill.transform.position.z;
            // objSpriteFill.transform.position = new Vector3(0, 0, z);
            // objSpriteFill.transform.localScale = new Vector3(scale, scale, 1f);

        }

        //计算两触点中点与精灵锚点的差值
        //保持两触点中点与精灵锚点的差值不变
        float x = (pt0.x + pt1.x) / 2 - touchDeltaX;
        float y = (pt0.y + pt1.y) / 2 - touchDeltaY;
        z = this.transform.position.z;
        this.transform.position = new Vector3(x, y, z);

        // z = objSpriteFill.transform.position.z;
        // objSpriteFill.transform.position = new Vector3(x, y, z);


        Vector3 pos = this.transform.position;
        touchDeltaX = (pt0.x + pt1.x) / 2 - pos.x;       //计算新的偏移量
        touchDeltaY = (pt0.y + pt1.y) / 2 - pos.y;
        // limitSpritePos(false);
        UpdateMainPaintScale();
    }
    void onMultiTouchUp()
    {
        if (!isMultiTouchDownPic)
        {
            return;
        }
        isMultiTouchDownPic = false;
    }
}
