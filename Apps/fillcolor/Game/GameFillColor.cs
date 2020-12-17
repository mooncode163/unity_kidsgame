using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//JS实现照片图片变成黑白线条线稿:https://www.zhangxinxu.com/wordpress/2018/06/js-canny-edge-photo-image-to-line/
//https://www.zhangxinxu.com/study/201806/image-to-be-line.html
//线稿生成算法 https://github.com/inspirit/jsfeat

public delegate void OnGameDidStrawDelegate();
public class GameFillColor : GameBase
{
    public const float MAX_MULTI_TOUCH_SCALE = 256.0f;
    public const float MIN_MULTI_TOUCH_SCALE = 1.0f;
    public const string KEY_STR_COLOR_FILL = "KEY_STR_COLOR_FILL";
    public GameObject objSpriteStraw;//颜色吸管
    //public GameObject objPlanePaint;
    public MeshTexture meshTex;
    public Rect rectMain;//world中的显示区域
    public bool isHasPaint = false;
    public bool isHasSave = false;
    Shader shaderFill;

    public RenderTexture rtMainPaint;
    public RenderTexture rtPaintTemp;
    Material matFillColor;

    Vector2 sizeMainPaint;

    Texture2D texPic;
    Texture2D texPicFromFile;//可能是保存图片
    Texture2D texPicOrign;//原始图片
    Texture2D texPicMask;
    ColorImage colorImage;
    ColorImage colorImageMask;

    int gamePicOffsetHeight;



    //multi-touch
    bool isMultiTouchDownPic;
    float touchDistance;//两个触摸点之间的距离
    float scaleGamePic;
    float scaleGamePicX;
    float scaleGamePicY;

    float scaleGamePicNormal;//初始地图缩放比例 
    float scaleGamePicNormalX;
    float scaleGamePicNormalY;

    float touchDeltaX;    //目标x轴的改变值
    float touchDeltaY;    //目标y轴的改变值 
    bool isFirstMultiMove;
    bool isFirstFillColorGPU = true;
    bool isHaveTouchMove;
    bool isHaveMultiTouch;
    Vector2 ptDown;
    Vector2 ptDownWorld;



    long tickDraw;
    long tick1, tick2;


    bool isFirstClick = true;
    long tickClickPre;
    long tickClickCur;


    FillColorTouchEvent fillColorTouchEvent;
    bool isTouchDownPic = false;

    public List<ColorItemInfo> listColorFill;


    Color colorTouch;
    Vector2 ptColorTouch;



    public Color colorFill
    {
        get
        {
            return Common.RGBString2Color(PlayerPrefs.GetString(KEY_STR_COLOR_FILL, "255,0,0"));
        }
        set
        {

            PlayerPrefs.SetString(KEY_STR_COLOR_FILL, Common.Color2RGBString(value));
        }
    }


    public OnGameDidStrawDelegate callBackStraw { get; set; }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        string strshader = "Custom/FillColor";
        shaderFill = Shader.Find(strshader);
        listColorFill = new List<ColorItemInfo>();

        colorImage = new ColorImage();
        colorImageMask = new ColorImage();
        //fillColorTouchEvent = objPlanePaint.AddComponent<FillColorTouchEvent>();
        //fillColorTouchEvent.callBackTouch = OnFillColorTouchEvent;

        matFillColor = new Material(Shader.Find("Custom/FillColor"));

    }
    // Use this for initialization
    void Start()
    {
        LayOut();
    }

    void Update()
    {

        // mobile touch
        //if ((Input.touchCount > 0) && (!GameScene.isAlertHasShow) && (!gameSelector.gameObject.activeSelf) && (!uiColorBoard.gameObject.activeSelf))
        {
            if (Input.touchCount == 2)
            {
                //多点触摸
                if ((Input.touches[0].phase == TouchPhase.Began) || (Input.touches[1].phase == TouchPhase.Began))
                {
                    onMultiTouchDown();
                }

                if ((Input.touches[0].phase == TouchPhase.Moved) || (Input.touches[1].phase == TouchPhase.Moved))
                {
                    onMultiTouchMove();
                }
                if ((Input.touches[0].phase == TouchPhase.Ended) && (Input.touches[1].phase == TouchPhase.Ended))
                {
                    onMultiTouchUp();
                }
            }

        }
    }

    public override void LayOut()
    {
        float x = 0, y = 0, z = 0, w = 0, h = 0;
        LayoutMainPaint();

        //objSpriteStraw
        {
            SpriteRenderer sprender = objSpriteStraw.GetComponent<SpriteRenderer>();
            float w_screen = sprender.sprite.texture.width * AppCommon.scaleBase;
            float scale = Common.ScreenToWorldWidth(AppSceneBase.main.mainCamera, w_screen) / (sprender.sprite.texture.width / 100f);
            objSpriteStraw.transform.localScale = new Vector3(scale, scale, 1f);
        }


    }

    public void Init(ColorItemInfo info)
    {

        isHasPaint = false;
        isHasSave = false;
        texPicMask = LoadTexture.LoadFromAsset(info.picmask);
        colorImageMask.Init(texPicMask);


        matFillColor.SetTexture("_TexMask", texPicMask);
        matFillColor.SetTexture("_TexPic", texPicFromFile);


        // Debug.Log("sizeMainPaint=" + sizeMainPaint);
        //Material mat = render.material;
        if (texPic != null)
        {
            rtPaintTemp = new RenderTexture(texPic.width, texPic.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
            rtMainPaint = new RenderTexture(texPic.width, texPic.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
            //mat.SetTexture("_MainTex", rtMainPaint);

            Graphics.Blit(texPic, rtMainPaint);


            fillColorTouchEvent = meshTex.gameObject.AddComponent<FillColorTouchEvent>();
            fillColorTouchEvent.callBackTouch = OnFillColorTouchEvent;
            meshTex.UpdateTexture(rtMainPaint);

            Renderer render = meshTex.GetComponent<Renderer>();
            sizeMainPaint = render.bounds.size;

            meshTex.EnableTouch(true);
        }

        //
        //objPlanePaint.SetActive(false);

    }

    public void LoadGamePic(ColorItemInfo info, bool isNew)
    {

        InitSpriteTexture(info, isNew);


        if (texPic)
        {


            //Debug.Log(info.pic);



            if (!isNew)
            {
                //读取原始图片
                // Texture2D texOrigin = LoadTexture.LoadFromAsset(info.pic);
                // colorImage.InitOrigin(texOrigin);
            }

        }

        //FillAlphaBg();
        Init(info);
        LayOut();
    }

    void InitSpriteTexture(ColorItemInfo info, bool isNew)
    {
        string picfile = info.pic;
        if(GameManager.main.isLoadGameScreenShot)
        {
            picfile = info.picmask;
        }
        if (!isNew)
        {
            Debug.Log("fileSave=" + info.infoDB.filesave);
            if (FileUtil.FileIsExist(info.infoDB.filesave))
            {
                Debug.Log("fileSave is exist");
                picfile = info.infoDB.filesave;
                texPicFromFile = LoadTexture.LoadFromFile(picfile);
            }
            else
            {
                Debug.Log("fileSave is not exist");
                texPicFromFile = LoadTexture.LoadFromAsset(picfile);
            }

        }
        else
        {
            Debug.Log("texPicFromFile LoadFromAsset= " + picfile);
            texPicFromFile = LoadTexture.LoadFromAsset(picfile);
        }
        if (info.pic == picfile)
        {
            texPicOrign = texPicFromFile;
        }
        else
        {
            texPicOrign = LoadTexture.LoadFromAsset(info.pic);
        }

        texPic = texPicFromFile;

        //@moon解决Texture.Apply()在像素比较多的时候无法更新sptire的各个像素点alpha值的bug。先给sprie设置一个空的单色的或空的texture(所有像素的alpha值为1f)
        //然后再把实际的图片texture拷贝到texPic更新sprite显示
        //texPic = CreateTexTureBg(texPicFromFile.width, texPicFromFile.height);

        // SpriteRenderer spRender = objSpritePic.GetComponent<SpriteRenderer>();
        // Sprite sp = Sprite.Create(texPic, new Rect(0, 0, texPic.width, texPic.height), new Vector2(0.5f, 0.5f));
        // spRender.sprite = sp;

        //copy texture 
        // texPic.LoadRawTextureData(texPicFromFile.GetRawTextureData());
        // texPic.Apply();

        //最后初始化更新
        colorImage.Init(texPic);
    }


    public bool IsHaveStartDraw()
    {
        bool ret = true;
        if (listColorFill.Count == 0)
        {
            //没有作画
            ret = false;
        }
        return ret;
    }
    Texture2D CreateTexTureBg(int w, int h)
    {
        Texture2D tex = new Texture2D(w, h, TextureFormat.ARGB32, false);
        return tex;
        ColorImage crImage = new ColorImage();
        crImage.Init(tex);
        for (int j = 0; j < h; j++)
        {
            for (int i = 0; i < w; i++)
            {
                Vector2 pttmp = new Vector2(i, j);
                Color colorpic = new Color(1f, 1f, 1f, 1f);
                crImage.SetImageColor(pttmp, colorpic);
            }
        }

        crImage.UpdateTexture();

        return tex;
    }

    void UpdatePaintRect()
    {
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        float x = 0, y = 0, z = 0, w = 0, h = 0;
        float scalex = 0, scaley = 0, scale = 0;
        //   if (texPic)
        {
            //float adbar_h_world = GameManager.main.heightAdWorld;
            float h_bottom_oft = Device.offsetBottomWithAdBannerWorld;
            // adbar_h_world = 0;
            float topbar_h_world = Common.CanvasToWorldHeight(mainCam, sizeCanvas, 160) + Device.offsetTopWorld;
            // topbar_h_world = 0;

            Vector2 world_size = AppSceneBase.main.GetRectMainWorld().rect.size;
            float ratio = 0.9f;
            w = world_size.x;
            h = (world_size.y - topbar_h_world - h_bottom_oft);
            x = 0;
            y = ((world_size.y / 2 - topbar_h_world) + (-world_size.y / 2 + h_bottom_oft)) / 2;
            //Debug.Log("rectMain world_size=" + world_size+" adbar_h_world="+adbar_h_world+" topbar_h_world="+topbar_h_world);
            float w_disp = w * ratio;
            float h_disp = h * ratio;
            x = x - w_disp / 2;
            y = y - h_disp / 2;
            rectMain = new Rect(x, y, w_disp, h_disp);

        }

    }
    void LayoutMainPaint()
    {
        UpdatePaintRect();
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        if (texPic)
        {


            float w_tex = texPic.width / 100f;
            float h_tex = texPic.height / 100f;
            float scalex = rectMain.width / w_tex;
            float scaley = rectMain.height / h_tex;
            float scale = Mathf.Min(scalex, scaley);
            float w_disp = scale * w_tex;
            float h_disp = scale * h_tex;
            scaleGamePicNormal = scale;
            scaleGamePic = scale;
            Debug.Log("w_tex=" + w_tex + " h_tex=" + h_tex + " scalex=" + scalex);

            scaleGamePicNormalX = scaleGamePicX;
            scaleGamePicNormalY = scaleGamePicY;

            UpdateMainPaintSize(w_disp, h_disp);


        }

        float z = meshTex.transform.localPosition.z;
        Debug.Log("rectMain =" + rectMain);
        meshTex.transform.localPosition = new Vector3(rectMain.center.x, rectMain.center.y, z);
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

    void limitSpritePos(bool isTouchUp)
    {
        /* 

        Vector3 ptNow = objPlanePaint.transform.position;

        Renderer spRender = objPlanePaint.GetComponent<Renderer>();
        Bounds bd = spRender.bounds;
        Vector2 size = Common.ScreenToWorldSize(mainCamera, new Vector2(Screen.width, Screen.height - gamePicOffsetHeight));

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

        ptNow.z = objPlanePaint.transform.position.z;
        objPlanePaint.transform.position = ptNow;

*/
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
        Debug.Log("onTouchDown:");
        isTouchDownPic = false;
        isHaveTouchMove = false;
        isHaveMultiTouch = false;
        // Bounds bd = objSpritePic.GetComponent<SpriteRenderer>().bounds;
        // Rect rc = new Rect(bd.center.x - bd.size.x / 2, bd.center.y - bd.size.y / 2, bd.size.x, bd.size.y);
        //Debug.Log("left:"+bd+"rc="+rc);
        Vector2 inputPos = Common.GetInputPosition();
        Vector3 posTouchWorld = mainCam.ScreenToWorldPoint(inputPos);
        ptDown = inputPos;
        ptDownWorld = meshTex.transform.localPosition;
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
                        colorFill = color;
                        //  UpdateColorSelect();
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
        //移动图片
        Vector2 pt = Common.GetInputPosition();
        float oftx = pt.x - ptDown.x;
        oftx = Common.ScreenToWorldWidth(mainCam, oftx);
        float ofty = pt.y - ptDown.y;
        ofty = Common.ScreenToWorldHeight(mainCam, ofty);
        Vector3 pos = meshTex.transform.localPosition;

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
                meshTex.transform.localPosition = new Vector3(x, y, z);

            }
            //limitSpritePos(false); 


        }

    }
    void onTouchUp()
    {
        Debug.Log("onTouchUp");

        if (!isTouchDownPic)
        {
            Debug.Log("onTouchUp not isTouchDownPic");
            return;
        }
        isTouchDownPic = false;
        onClickPic();
        if (isHaveTouchMove)
        {
            limitSpritePos(false);
        }
    }

    //多点触摸
    void onMultiTouchDown()
    {
        if (!isTouchDownPic)
        {
            return;
        }

        isMultiTouchDownPic = false;
        isHaveMultiTouch = false;
        // Bounds bd = objSpritePic.GetComponent<SpriteRenderer>().bounds;
        // Rect rc = new Rect(bd.center.x - bd.size.x / 2, bd.center.y - bd.size.y / 2, bd.size.x, bd.size.y);
        //Debug.Log("left:"+bd+"rc="+rc);
        Vector2 inputPos0 = Input.touches[0].position; ;
        Vector3 posTouchWorld0 = mainCam.ScreenToWorldPoint(inputPos0);
        Vector2 inputPos1 = Input.touches[1].position; ;
        Vector3 posTouchWorld1 = mainCam.ScreenToWorldPoint(inputPos1);

        isFirstMultiMove = true;
        isHaveTouchMove = false;

        if (isTouchInRangeOfPic(inputPos0) || isTouchInRangeOfPic(inputPos1))
        {
            isMultiTouchDownPic = true;
            Vector3 pos = meshTex.transform.localPosition;

            touchDeltaX = (posTouchWorld0.x + posTouchWorld1.x) / 2 - pos.x;       //计算新的偏移量
            touchDeltaY = (posTouchWorld0.y + posTouchWorld1.y) / 2 - pos.y;
        }

    }
    void onMultiTouchMove()
    {
        if (!isTouchDownPic)
        {
            return;
        }
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
        float z = meshTex.transform.localPosition.z;
        if (scaleGamePic > scaleGamePicNormal)
        {
            scale = scaleGamePic;
            //  objSpritePic.transform.localScale = new Vector3(scale, scale, 1f);

        }
        else
        {
            //恢复原来位置 

            //  objSpritePic.transform.position = new Vector3(0, 0, z);
            scale = scaleGamePicNormal;
            //  objSpritePic.transform.localScale = new Vector3(scale, scale, 1f);

            scaleGamePic = scaleGamePicNormal;
            meshTex.transform.localPosition = new Vector3(0, 0, z - 1);
        }

        //计算两触点中点与精灵锚点的差值
        //保持两触点中点与精灵锚点的差值不变
        float x = (pt0.x + pt1.x) / 2 - touchDeltaX;
        float y = (pt0.y + pt1.y) / 2 - touchDeltaY;
        z = meshTex.transform.localPosition.z;
        meshTex.transform.localPosition = new Vector3(x, y, z);

        // z = objSpriteFill.transform.position.z;
        // objSpriteFill.transform.position = new Vector3(x, y, z);


        Vector3 pos = meshTex.transform.localPosition;

        touchDeltaX = (pt0.x + pt1.x) / 2 - pos.x;       //计算新的偏移量
        touchDeltaY = (pt0.y + pt1.y) / 2 - pos.y;
        // limitSpritePos(false);


        float w_tex = texPic.width / 100f;
        float h_tex = texPic.height / 100f;
        float w_disp = scaleGamePic * w_tex;
        float h_disp = scaleGamePic * h_tex;

        UpdateMainPaintSize(w_disp, h_disp);

    }
    void onMultiTouchUp()
    {
        if (!isMultiTouchDownPic)
        {
            return;
        }
        isMultiTouchDownPic = false;
    }
    void DoFillColor(Vector2 pt, Color fill)
    {
        isHasPaint = true;
        //DoFillColorCPU
        DoFillColorGPU(pt, fill);
    }

    void onFillColor()
    {
        float x, y, w, h;
        Vector2 inputPos = Common.GetInputPosition();
        Vector3 posTouchWorld = mainCam.ScreenToWorldPoint(inputPos);

        //图片左下脚位置
        Renderer spRender = meshTex.GetComponent<Renderer>();
        w = spRender.bounds.size.x;
        h = spRender.bounds.size.y;
        x = spRender.bounds.center.x - w / 2;
        y = spRender.bounds.center.y - h / 2;
        Vector2 ptPicLeftBottom = new Vector2(x, y);

        float ratio_x = (posTouchWorld.x - x) / w;
        float ratio_y = (posTouchWorld.y - y) / h;

        //相对左下角的像素坐标
        x = texPic.width * ratio_x;
        y = texPic.height * ratio_y;
        if (x >= texPic.width)
        {
            x = texPic.width - 1;
        }
        if (y >= texPic.height)
        {
            y = texPic.height - 1;
        }
        long tick = Common.GetCurrentTimeMs();


        Vector2 pt = new Vector2(x, y);
        Color colorMask = colorImageMask.GetImageColor(pt);
        Debug.Log("x=" + x + " y=" + y + " colorMask=" + colorMask);
        Color color = colorImage.GetImageColor(pt);
        Debug.Log("x=" + x + " y=" + y + " color=" + color);
        colorTouch = color;
        ptColorTouch = pt;

        if (colorMask == Color.black)
        {
            Debug.Log("Click The Board Color!");
            return;
        }

        ColorItemInfo info = new ColorItemInfo();
        info.pt = pt;
        info.colorFill = colorFill;
        info.colorMask = colorMask;
        listColorFill.Add(info);

        DoFillColor(pt, colorFill);


        tick = Common.GetCurrentTimeMs() - tick;

        if (listColorFill.Count > 0)
        {
            // btnDelLast.gameObject.SetActive(true);
        }

        tickDraw = tick;
        Debug.Log("draw time = " + tick + "ms" + " tick1 = " + tick1 + "ms" + " tick2 = " + tick2 + "ms");
    }



    void UpdateMainPaintSize(float w, float h)
    {

        float scalex = scaleGamePicX;
        float scaley = scaleGamePicY;
        // objPlanePaint.transform.localScale = new Vector3(scalex, 1f, scaley);
        meshTex.UpdateSize(w, h);
    }


    void DoFillColorGPU(Vector2 pt, Color fill)
    {

        long tick = Common.GetCurrentTimeMs();

        Color colorMask = colorImageMask.GetImageColor(pt);

        if (colorMask == Color.black)
        {
            Debug.Log("Click The Board Color!");
            return;
        }
        tick = Common.GetCurrentTimeMs();

        //GPU FillColor

        matFillColor.SetColor("_ColorMask", colorMask);
        matFillColor.SetColor("_ColorFill", fill);

        //matFillColor.SetTexture("_TexDisplay", texPic);

        // var rtTmp = RenderTexture.GetTemporary(rtMainPaint.width, rtMainPaint.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
        // Graphics.Blit(rtMainPaint, rtTmp, matFillColor);
        // Graphics.Blit(rtTmp, rtMainPaint);
        // RenderTexture.ReleaseTemporary(rtTmp);

        Graphics.Blit(rtMainPaint, rtPaintTemp, matFillColor);
        Graphics.Blit(rtPaintTemp, rtMainPaint);

        tick = Common.GetCurrentTimeMs() - tick;
        //meshTex.Draw();

        tickDraw = tick;
        //Debug.Log("draw time = " + tick + "ms" + " tick1 = " + tick1 + "ms" + " tick2 = " + tick2 + "ms");
    }

    bool IsSameColor(Color color1, Color color2, float step)
    {
        bool ret = false;
        float diffr = Mathf.Abs(color1.r - color2.r);
        float diffg = Mathf.Abs(color1.g - color2.g);
        float diffb = Mathf.Abs(color1.b - color2.b);
        if ((diffr <= step) && (diffg <= step) && (diffb <= step))
        {
            ret = true;
        }
        return ret;
    }


    void onClickPic()
    {


        /*
        //解决GPU模式下点击太快填色出现黑色区域的问题。
        if (isFirstClick)
        {
            tickClickCur = Common.GetCurrentTimeMs();
        }
        if (!isFirstClick)
        {
            tickClickPre = tickClickCur;
            tickClickCur = Common.GetCurrentTimeMs();
            float step = Mathf.Abs(tickClickCur - tickClickPre);
            if (step < 400)
            {
                Debug.Log("onClickPic touch too fast...");
                return;
            }
        }
        isFirstClick = false;
*/

        // ///  ////

        /* 

                if (gameSelector.gameObject.activeSelf)
                {
                    Debug.Log("onClickPic gameSelector.gameObject.activeSelf");
                    return;
                }
                if (isGameSelectorClose)
                {
                    Debug.Log("onClickPic isGameSelectorClose");
                    isGameSelectorClose = false;
                    //return;
                }

                */
        if (isHaveTouchMove)
        {
            Debug.Log("onClickPic isHaveTouchMove");
            return;
        }

        onFillColor();


    }


    //将背景填充成白色
    void FillWhiteBg()
    {

        for (int j = 0; j < texPic.height; j++)
        {
            for (int i = 0; i < texPic.width; i++)
            {
                Vector2 pttmp = new Vector2(i, j);

                Color colorpic = colorImage.GetImageColorOrigin(pttmp);


                if (colorpic.a == 0f)
                {
                    //统一为纯白色
                    colorpic.r = 1f;
                    colorpic.g = 1f;
                    colorpic.b = 1f;
                    colorpic.a = 1f;
                    colorImage.SetImageColor(pttmp, colorpic);
                }


            }
        }

        colorImage.UpdateTexture();
    }

    //将背景白色变成纯透明
    void FillAlphaBg()
    {

        for (int j = 0; j < texPic.height; j++)
        {
            for (int i = 0; i < texPic.width; i++)
            {
                Vector2 pttmp = new Vector2(i, j);

                Color colorpic = colorImage.GetImageColorOrigin(pttmp);


                if (colorpic == Color.white)
                {
                    //统一为
                    colorpic.r = 0f;
                    colorpic.g = 0f;
                    colorpic.b = 0f;
                    colorpic.a = 0f;
                    colorImage.SetImageColor(pttmp, colorpic);
                }


            }
        }

        colorImage.UpdateTexture();
    }
    void FormatGamePicAlpha()
    {
        for (int j = 0; j < texPic.height; j++)
        {
            for (int i = 0; i < texPic.width; i++)
            {
                Vector2 pttmp = new Vector2(i, j);

                Color colorpic = colorImage.GetImageColorOrigin(pttmp);
                if (colorpic.a < 0.5f)
                {
                    colorpic.a = 0f;
                }
                else
                {
                    colorpic.a = 1f;
                }
                colorImage.SetImageColor(pttmp, colorpic);

            }
        }

        colorImage.UpdateTexture();
    }


    public void DoDeleteAll()
    {
        isHasPaint = false;
        isHasSave = false;
        // listColorFill.Clear(); 
        //恢复
        Graphics.Blit(texPicOrign, rtMainPaint);
    }

    public void DoSave(string filepath)
    {
        Texture2D texSave = TextureUtil.RenderTexture2Texture2D(rtMainPaint);
        if (texSave)
        {
            TextureUtil.SaveTextureToFile(texSave, filepath);
            isHasSave = true;
        }
    }

    public void OnFillColorTouchEvent(PointerEventData eventData, int status)
    {
        //单点触摸
        switch (status)
        {
            case FillColorTouchEvent.STATUS_TOUCH_DOWN:
                onTouchDown();
                break;

            case FillColorTouchEvent.STATUS_TOUCH_MOVE:
                onTouchMove();
                break;

            case FillColorTouchEvent.STATUS_TOUCH_UP:
                onTouchUp();
                break;

        }
    }


    public void OnDelLast()
    {
        // tickDraw = 0;


        if (!IsHaveStartDraw())
        {
            //没有作画
            return;
        }
        int idx = listColorFill.Count - 1;
        ColorItemInfo info = listColorFill[idx];

        Color colorClear = new Color(0f, 0f, 0f, 0f);
        if (listColorFill.Count >= 2)
        {
            ColorItemInfo info2 = listColorFill[idx - 1];
            if (info.colorMask == info2.colorMask)
            {
                //填充成上一个颜色
                colorClear = info2.colorFill;
            }

        }
        DoFillColor(info.pt, colorClear);

        listColorFill.RemoveAt(idx);
        if (listColorFill.Count == 0)
        {
            //  btnDelLast.gameObject.SetActive(false);
        }
    }

    //显示吸管
    public void ShowStraw(bool show)
    {
        objSpriteStraw.gameObject.SetActive(show);
    }

    public bool IsStrawActive()
    {
        return objSpriteStraw.gameObject.activeSelf;
    }

}
