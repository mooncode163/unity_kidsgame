using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
宝宝巴士 宝宝涂色 
http://as.baidu.com/software/23393827.html
http://app.mi.com/details?id=com.sinyee.babybus.paintingIII&ref=search
*/


public class UIGamePaint : UIGameBase
{
    public const string STR_KEYNAME_VIEWALERT_SAVE_FINISH = "STR_KEYNAME_VIEWALERT_SAVE_FINISH";
    public const string STR_KEYNAME_VIEWALERT_SAVE = "STR_KEYNAME_VIEWALERT_SAVE";
    public const string STR_KEYNAME_VIEWALERT_SAVE_TIPS = "STR_KEYNAME_VIEWALERT_SAVE_TIPS";
    public const string STR_KEYNAME_VIEWALERT_FIRST_USE_FUNCTION = "keyname_viewalert_first_use_function";
    public const string KEY_STR_FIRST_USE_STRAW = "KEY_STR_FIRST_USE_STRAW";
    public const string KEY_STR_FIRST_USE_COLOR_INPUT = "KEY_STR_FIRST_USE_COLOR_INPUT";

    public const string STR_KEYNAME_VIEWALERT_DELETE_ALL = "keyname_viewalert_delete_all_paint";
    public const int GAME_MODE_NORMAL = 0;
    public const int GAME_MODE_FREE_DRAW = 1;
    public const string KEY_STR_COLOR_PEN = "KEY_STR_COLOR_PEN";
    public const string KEY_STR_COLOR_FILL = "KEY_STR_COLOR_FILL";
    public const string KEY_STR_COLOR_SIGN = "KEY_STR_COLOR_SIGN";

    public const int BTN_CLICK_MODE_STRAW = 0;
    public const int BTN_CLICK_MODE_COLOR_INPUT = 1;

    public MeshTexture meshTex;
    public Image imagePenBarBg;

    public UIColorBoard uiColorBoard;
    public UIColorInput uiColorInput;
    UILineSetting uiLineSettingPrefab;

    GamePaint gamePaintPrefab;
    GamePaint gamePaint;


    public UILineSetting uiLineSetting;

    public GameObject objLayoutBtn;
    public GameObject objLayoutBtnPen;
    public GameObject objLayoutTopBar;
    public Button btnColor;//任意颜色
    public Button btnStraw;//颜色吸管
    public Button btnFill;
    public Button btnPen;
    public Button btnSign;
    public Button btnErase;
    public Button btnMagic;
    public Image imagePenSel_H;
    public Image imagePenSel_V;
    Image imagePenSel;
    int btnClickMode;
    //multi-touch
    bool isMultiTouchDownPic;
    float touchDistance;//两个触摸点之间的距离 
    float touchDeltaX;    //目标x轴的改变值
    float touchDeltaY;    //目标y轴的改变值 
    bool isFirstMultiMove;
    bool isHaveTouchMove;
    bool isHaveMultiTouch;
    Vector2 ptDown;
    Vector2 ptDownWorld;
    //color select
    List<Color> listColorSelect;
    Color colorTouch;
    Vector2 ptColorTouch;
    List<ColorItemInfo> listColorFill;

    Texture2D texPic;
    Texture2D texPicFromFile;
    Texture2D texPicOrign;//原始图片
    Texture2D texPicMask;
    Texture2D texBrush;

    bool isGameSelectorClose;
    int gamePicOffsetHeight;
    long tickDraw;
    long tick1, tick2;
    int indexSprite;


    Material matPenColor;
    bool isHaveInitShader;
    bool isNeedUpdateSpriteFill;
    float colorBoardOffsetYNormal;

    static bool isAdHasShow = false;

    bool isFirstUseStraw
    {
        get
        {
            return false;
            if (Common.noad)
            {
                return false;
            }
            return Common.Int2Bool(PlayerPrefs.GetInt(KEY_STR_FIRST_USE_STRAW, Common.Bool2Int(true)));
        }
        set
        {

            PlayerPrefs.SetInt(KEY_STR_FIRST_USE_STRAW, Common.Bool2Int(value));
        }
    }

    bool isFirstUseColorInput
    {
        get
        {
            return false;
            if (Common.noad)
            {
                return false;
            }
            return Common.Int2Bool(PlayerPrefs.GetInt(KEY_STR_FIRST_USE_COLOR_INPUT, Common.Bool2Int(true)));
        }
        set
        {

            PlayerPrefs.SetInt(KEY_STR_FIRST_USE_COLOR_INPUT, Common.Bool2Int(value));
        }
    }

    Color colorPen
    {
        get
        {
            return Common.RGBString2Color(PlayerPrefs.GetString(KEY_STR_COLOR_PEN, "255,0,0"));
        }
        set
        {

            PlayerPrefs.SetString(KEY_STR_COLOR_PEN, Common.Color2RGBString(value));
        }
    }
    Color colorSign
    {
        get
        {
            return Common.RGBString2Color(PlayerPrefs.GetString(KEY_STR_COLOR_SIGN, "0,255,0"));
        }
        set
        {

            PlayerPrefs.SetString(KEY_STR_COLOR_SIGN, Common.Color2RGBString(value));
        }
    }
    Color colorFill
    {
        get
        {
            return Common.RGBString2Color(PlayerPrefs.GetString(KEY_STR_COLOR_FILL, "0,0,255"));
        }
        set
        {

            PlayerPrefs.SetString(KEY_STR_COLOR_FILL, Common.Color2RGBString(value));
        }
    }
    void Awake()
    {
        AppSceneBase.main.UpdateWorldBg(AppRes.IMAGE_GAME_BG);//  
        LoadPrefab();
        matPenColor = new Material(Shader.Find("Custom/PenColor"));
        isGameSelectorClose = false;
        isHaveInitShader = false;

        if (btnShare != null)
        {
            btnShare.gameObject.SetActive(Config.main.isHaveShare);
        }
        // tickUpdateCur = Common.GetCurrentTimeMs();
        // tickUpdatePre = Common.GetCurrentTimeMs();
        objLayoutTopBar.SetActive(false);

        if (Device.isLandscape)
        {
            imagePenSel_H.gameObject.SetActive(true);
            imagePenSel_V.gameObject.SetActive(false);
            imagePenSel = imagePenSel_H;
        }
        else
        {
            imagePenSel_H.gameObject.SetActive(false);
            imagePenSel_V.gameObject.SetActive(true);
            imagePenSel = imagePenSel_V;
        }
        //  ParseGuanka();


        LoadGameTexture(true);


        indexSprite = 0;

        UpdateBtnMusic();
        //ShowFPS();
    }
    // Use this for initialization
    void Start()
    {

        InitPenColor();
        isNeedUpdateSpriteFill = true;
        UpdateGuankaLevel(LevelManager.main.gameLevel);

    }
    // Update is called once per frame
    void Update()
    {

    }
    void OnGUI()
    {
        GUIStyle bb = new GUIStyle();
        bb.normal.background = null;    //这是设置背景填充的
        bb.normal.textColor = new Color(1f, 0f, 0f);   //设置字体颜色的
        bb.fontSize = 20;       //当然，这是字体大小
        if (Common.isiOS || Common.isAndroid)
        {
            bb.fontSize = bb.fontSize * 2;
        }
        //居中显示FPS
        if (colorTouch != null)
        {
            int r = (int)(colorTouch.r * 255);
            int g = (int)(colorTouch.g * 255);
            int b = (int)(colorTouch.b * 255);
            int a = (int)(colorTouch.a * 255);
            // GUI.Label(new Rect(0, 100, 400, 200), "tickUpdateStep=" + tickUpdateStep + " tickPaint=" + tickPaint + "ms", bb);
        }

    }

    void LoadPrefab()
    {

        {
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/Game/GamePaint");
            gamePaintPrefab = obj.GetComponent<GamePaint>();
        }


    }

    public override void UpdateGuankaLevel(int level)
    {
        base.UpdateGuankaLevel(level);
        AdKitCommon.main.callbackAdVideoFinish = OnAdKitAdVideoFinish;

        gamePaint = (GamePaint)GameObject.Instantiate(gamePaintPrefab);
        AppSceneBase.main.AddObjToMainWorld(gamePaint.gameObject);
        gamePaint.transform.localPosition = new Vector3(0f, 0f, -1f);
        InitUI();
        OnGameWinBase();
    }

    void InitUI()
    {
        ColorItemInfo info = GameLevelParse.main.GetItemInfo();
        if (gameMode == GAME_MODE_FREE_DRAW)
        {
            gamePaint.isFreeDraw = true;
            btnFill.gameObject.SetActive(false);
        }
        else
        {
            gamePaint.isFreeDraw = false;
            btnFill.gameObject.SetActive(true);
        }
        if (gamePaint == null)
        {
            return;
        }

        uiLineSetting.gameObject.SetActive(false);
        uiColorInput.gameObject.SetActive(false);
        uiColorBoard.gameObject.SetActive(false);

        uiColorBoard.callBackClick = OnUIColorBoardDidClick;
        uiColorInput.callBackUpdateColor = OnUIColorInputUpdateColor;
        uiLineSetting.callBackSettingLineWidth = OnUILineSettingLineWidth;

        // //init paint color
        gamePaint.colorInfo = GameLevelParse.main.GetItemInfo();
        gamePaint.texPic = texPic;
        gamePaint.texPicMask = texPicMask;
        gamePaint.texBrush = texBrush;
        gamePaint.texPicOrign = texPicOrign;
        gamePaint.colorPaint = colorPen;
        gamePaint.callBackStraw = OnGamePaintClickStraw;
        gamePaint.UpdateMode(GamePaint.MODE_PAINT);

        // SpriteRenderer spRender = objSpriteStraw.GetComponent<SpriteRenderer>();
        // paintColor.objSpriteStraw = objSpriteStraw;
        // paintColor.callBackClickStraw = OnPaintColorClickStraw;
        // paintColor.callBackErase = OnPaintColorErase;

        UpdatePaintRect();//必须在GamePaint.Init前面 

        UpdateColorCur();

        uiColorInput.UpdateInitColor(gamePaint.colorPaint);

        uiLineSetting.lineWidthPixsel = gamePaint.lineWidthPixsel;

        InitPenColor();

        UpdateImagePenSelPosition();
        gamePaint.UpdateBg(AppRes.IMAGE_PAINT_BG);


        gamePaint.UpdateGamePic(info.pic);
        LayOut();
 

        OnUIDidFinish(1);
    }

    void UpdatePaintRect()
    {
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        float x = 0, y = 0, z = 0, w = 0, h = 0;
        float scalex = 0, scaley = 0, scale = 0;
        if (gamePaint == null)
        {
            return;
        }
        if (texPic)
        {
            float topbar_h_world = Common.CanvasToWorldHeight(mainCam, sizeCanvas, 160) + Device.offsetTopWorld;
            float adbar_h_world = topbar_h_world;

            Vector2 world_size = AppSceneBase.main.GetRectMainWorld().rect.size;
            float ratio = 0.95f;
            float oft_y = topbar_h_world;
            if (!Device.isLandscape)
            {
                //topbar + 底部的工具条
                oft_y = topbar_h_world * 2;
            }
            //  GameManager.main.heightAdWorld = 0;
            float h_bottom_oft = Device.offsetBottomWithAdBannerWorld;// GameManager.main.heightAdWorld + Common.ScreenToWorldHeight(mainCam, Device.offsetBottom);
            oft_y += h_bottom_oft;
            w = world_size.x;
            h = (world_size.y - oft_y);

            float w_pic = texPic.width / 100f;
            float h_pic = texPic.height / 100f;
            scale = Common.GetBestFitScale(w_pic, h_pic, w, h) * ratio;

            float w_disp = w_pic * scale;
            float h_disp = h_pic * scale;
            x = -w_disp / 2;
            // y = -h_disp / 2 - oft_y / 2;
            // if (!Device.isLandscape)
            // {
            //     y = -h_disp / 2;
            // }
            float y1 = -world_size.y / 2 + h_bottom_oft + topbar_h_world;
            if (Device.isLandscape)
            {
                y1 = -world_size.y / 2 + h_bottom_oft;
            }
            float y2 = world_size.y / 2 - topbar_h_world;
            y = (y1 + y2) / 2 - h_disp / 2;
            gamePaint.rectMain = new Rect(x, y, w_disp, h_disp);

        }


    }

    public override void LayOut()
    {
        base.LayOut();
        float x = 0, y = 0, z = 0, w = 0, h = 0;
        float scalex = 0, scaley = 0, scale = 0;
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        if (gamePaint == null)
        {
            return;
        }
        UpdatePaintRect();

        //pen button
        {
            GridLayoutGroup gridLayout = objLayoutBtnPen.GetComponent<GridLayoutGroup>();
            Vector2 cellSize = gridLayout.cellSize;
            //  SpriteRenderer render = objSpritePaintBoardMid.GetComponent<SpriteRenderer>();
            float w_canvas = 160f;// Common.WorldToCanvasWidth(mainCam, sizeCanvas, render.bounds.size.x);
            float h_canvas = 512f;// Common.WorldToCanvasHeight(mainCam, sizeCanvas, render.bounds.size.y);
            RectTransform rctran = objLayoutBtnPen.transform as RectTransform;
            float oft = 10;

            Vector2 posScreen = Vector2.zero;// mainCam.WorldToScreenPoint(objSpritePaintBoardMid.transform.position);
            int total_btn = 4;
            if (Device.isLandscape)
            {
                // h = rctran.sizeDelta.y;
                h = (cellSize.y + 8) * total_btn;
                rctran.sizeDelta = new Vector2(cellSize.x, h);
                // x = w_canvas / 2 + cellSize.x / 2 + oft;
                x = -sizeCanvas.x / 2 + cellSize.x / 2;
                y = 0;
            }
            else
            {
                w = (cellSize.x + 8) * total_btn;
                rctran.sizeDelta = new Vector2(w, cellSize.y);
                x = 0;
                Vector3 ptlocal = new Vector3(0, gamePaint.rectMain.y, 0);
                // Vector3 ptworld = mainCam.ScreenToWorldPoint(this.transform.TransformPoint(ptlocal));
                Vector3 ptworld = gamePaint.transform.TransformPoint(ptlocal);
                Vector2 ptcanvas = Common.WorldToCanvasPoint(mainCam, sizeCanvas, ptworld);
                Debug.Log("ptcanvas=" + ptcanvas + " ptworld=" + ptworld + " ptlocal=" + ptlocal);
                oft = cellSize.y + 8f;
                y = -(sizeCanvas.y / 2 - ptcanvas.y) - oft;
            }

            rctran.anchoredPosition = new Vector2(x, y);

            if (Device.isLandscape)
            {
                // Vector2 pos = objLayoutBtnPen.transform.position;
                // objLayoutBtnPen.transform.position = new Vector2(pos.x, posScreen.y);
            }


        }

        if (gamePaint != null)
        {
            gamePaint.LayOut();
        }


        if (gamePaint != null)
        {
            if ((GameManager.main.heightAdWorld > 0) && (!isAdHasShow))
            {
                //区域改变后需要清空复位，比如广告刷新后 不然第一次显示会位置错位
                isAdHasShow = true;
                DoDeleteAll();
                gamePaint.Init();
            }
        }

    }



    //将背景填充成白色
    void FillWhiteBg(Texture2D tex)
    {
        ColorImage colorImageTmp = new ColorImage();
        colorImageTmp.Init(tex);
        for (int j = 0; j < tex.height; j++)
        {
            for (int i = 0; i < tex.width; i++)
            {
                Vector2 pttmp = new Vector2(i, j);

                Color colorpic = colorImageTmp.GetImageColorOrigin(pttmp);

                {
                    //统一为纯白色
                    colorpic.r = 1f;
                    colorpic.g = 1f;
                    colorpic.b = 1f;
                    colorpic.a = 1f;
                    colorImageTmp.SetImageColor(pttmp, colorpic);
                }


            }
        }

        colorImageTmp.UpdateTexture();
    }

    void LoadGameTexture(bool isNew)
    {
        Debug.Log("LoadGameTexture: gameMode=" + gameMode);
        ColorItemInfo info = GameLevelParse.main.GetItemInfo();
        texBrush = TextureCache.main.Load("AppCommon/UI/Brush/brush_dot");
        if (info == null)
        {
            return;
        }
        string picfile = info.pic;
        if (GameManager.main.isLoadGameScreenShot)
        {
            picfile = info.picmask;
        }
        if (gameMode == GAME_MODE_FREE_DRAW)
        {
            //PaintBlank
            //texPicFromFile = LoadTexture.LoadFromResource();
            texPicFromFile = LoadTexture.LoadFromAsset(info.pic);
            FillWhiteBg(texPicFromFile);
            texPic = texPicFromFile;
            texPicOrign = texPicFromFile;
            texPicMask = texPicFromFile;
            //texPicMask = LoadTexture.LoadFromResource("AppCommon/UI/Game/PaintBlank");
            return;
        }
        if (!isNew)
        {
            Debug.Log("fileSave=" + info.infoDB.filesave);
            if (FileUtil.FileIsExist(info.infoDB.filesave))
            {
                picfile = info.infoDB.filesave;
                texPicFromFile = LoadTexture.LoadFromFile(picfile);
            }
            else
            {
                texPicFromFile = LoadTexture.LoadFromAsset(picfile);
            }

        }
        else
        {
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
        // texPic = CreateTexTureBg(texPicFromFile.width, texPicFromFile.height);

        // SpriteRenderer spRender = objSpritePaintPic.GetComponent<SpriteRenderer>();
        // Sprite sp = Sprite.Create(texPic, new Rect(0, 0, texPic.width, texPic.height), new Vector2(0.5f, 0.5f));
        // spRender.sprite = sp;



        // //copy texture 
        // texPic.LoadRawTextureData(texPicFromFile.GetRawTextureData());
        // texPic.Apply();

        //最后初始化更新
        // colorImage.Init(texPic);


        texPicMask = LoadTexture.LoadFromAsset(info.picmask);
    }
    Texture2D UpdateTextureColor(Texture2D tex, Texture2D texMask, Color colorFill, Color colorMask)
    {
        Material mat = matPenColor;//new Material(Shader.Find("Custom/PenColor"));
        mat.SetTexture("_MainTex", tex);
        mat.SetTexture("_TexMask", texMask);
        mat.SetColor("_ColorMask", colorMask);
        mat.SetColor("_ColorFill", colorFill);

        RenderTexture rtTmp = new RenderTexture(tex.width, tex.height, 0);
        //var rtTmp = RenderTexture.GetTemporary(tex.width, tex.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
        Graphics.Blit(tex, rtTmp, mat);
        Texture2D texOut = TextureUtil.RenderTexture2Texture2D(rtTmp);
        //RenderTexture.ReleaseTemporary(rtTmp);
        return texOut;
    }
    void UpdateColorCur()
    {
        btnColor.GetComponent<UIButton>().imageBg.image.color = gamePaint.colorPaint;
    }

    void InitPenColorFill()
    {
        Texture2D tex = LoadTexture.LoadFromResource("AppCommon/UI/Game/btn_fill_pen");
        Texture2D texMask = LoadTexture.LoadFromResource("AppCommon/UI/Game/btn_fill_pen_mask");
        Texture2D texNew = UpdateTextureColor(tex, texMask, colorFill, Color.white);
        btnFill.GetComponent<Image>().sprite = TextureUtil.CreateSpriteFromTex(texNew);
    }
    void InitPenColorSign()
    {
        Texture2D tex = LoadTexture.LoadFromResource("AppCommon/UI/Game/btn_sign_pen");
        Texture2D texMask = LoadTexture.LoadFromResource("AppCommon/UI/Game/btn_sign_pen_mask");
        Texture2D texNew = UpdateTextureColor(tex, texMask, colorSign, Color.white);
        btnSign.GetComponent<Image>().sprite = TextureUtil.CreateSpriteFromTex(texNew);
    }
    void InitPenColorPen()
    {
        Texture2D tex = LoadTexture.LoadFromResource("AppCommon/UI/Game/btn_color_pen");
        Texture2D texMask = LoadTexture.LoadFromResource("AppCommon/UI/Game/btn_color_pen_mask");
        Texture2D texNew = UpdateTextureColor(tex, texMask, colorPen, Color.white);
        btnPen.GetComponent<Image>().sprite = TextureUtil.CreateSpriteFromTex(texNew);
    }
    void InitPenColor()
    {
        InitPenColorPen();
        // Invoke("InitPenColorFill",0.1f);
        InitPenColorFill();
        //
        InitPenColorSign();
        //InitPenColorSign();
        //Invoke("InitPenColorSign",1f);
    }

    void UpdatePenColor(Color color)
    {
        Texture2D texNew = null;
        Texture2D tex = null;
        Texture2D texMask = null;
        switch (gamePaint.mode)
        {
            case GamePaint.MODE_PAINT:
                {
                    tex = LoadTexture.LoadFromResource("AppCommon/UI/Game/btn_color_pen");
                    texMask = LoadTexture.LoadFromResource("AppCommon/UI/Game/btn_color_pen_mask");
                    texNew = UpdateTextureColor(tex, texMask, color, Color.white);
                    btnPen.GetComponent<Image>().sprite = TextureUtil.CreateSpriteFromTex(texNew);
                }
                break;
            case GamePaint.MODE_FILLCOLR:
                {
                    tex = LoadTexture.LoadFromResource("AppCommon/UI/Game/btn_fill_pen");
                    texMask = LoadTexture.LoadFromResource("AppCommon/UI/Game/btn_fill_pen_mask");
                    texNew = UpdateTextureColor(tex, texMask, color, Color.white);
                    btnFill.GetComponent<Image>().sprite = TextureUtil.CreateSpriteFromTex(texNew);
                }
                break;
            case GamePaint.MODE_SIGN:
                {
                    tex = LoadTexture.LoadFromResource("AppCommon/UI/Game/btn_sign_pen");
                    texMask = LoadTexture.LoadFromResource("AppCommon/UI/Game/btn_sign_pen_mask");
                    texNew = UpdateTextureColor(tex, texMask, color, Color.white);
                    btnSign.GetComponent<Image>().sprite = TextureUtil.CreateSpriteFromTex(texNew);
                }
                break;
        }
    }

    public void OnUILineSettingLineWidth(int width)
    {
        gamePaint.lineWidthPixsel = width;
        gamePaint.UpdateLineWidth();

    }
    public void OnPaintColorErase()
    {
        // Vector2 inputPos = Common.GetInputPosition();
        // Vector3 posTouchWorld = mainCam.ScreenToWorldPoint(inputPos);
        // posTouchWorld.z = objSpriteErase.transform.position.z;
        // objSpriteErase.transform.position = posTouchWorld;
        // if (!objSpriteErase.activeSelf)
        // {
        //     objSpriteErase.SetActive(true);
        // }
    }
    public void OnUIColorInputUpdateColor(Color color)
    {

        switch (gamePaint.mode)
        {
            case GamePaint.MODE_PAINT:
                {
                    colorPen = color;
                }
                break;
            case GamePaint.MODE_FILLCOLR:
                {
                    colorFill = color;
                }
                break;
            case GamePaint.MODE_SIGN:
                {
                    colorSign = color;
                }
                break;
        }
        gamePaint.colorPaint = color;
        UpdateColorCur();
        UpdatePenColor(color);
    }
    public void OnUIColorBoardDidClick(UIColorBoard ui, UIColorBoardCellItem item, bool isOutSide)
    {
        if (isOutSide)
        {
            uiColorBoard.gameObject.SetActive(false);
        }
        else
        {
            switch (gamePaint.mode)
            {
                case GamePaint.MODE_PAINT:
                    {
                        colorPen = item.color;
                    }
                    break;
                case GamePaint.MODE_FILLCOLR:
                    {
                        colorFill = item.color;
                    }
                    break;
                case GamePaint.MODE_SIGN:
                    {
                        colorSign = item.color;
                    }
                    break;
            }
            gamePaint.colorPaint = item.color;
            uiColorBoard.gameObject.SetActive(false);
            UpdateColorCur();
            UpdatePenColor(item.color);
        }
    }

    Rect RectString2Rect(string strrect)
    {
        float x, y, w, h;
        x = 0;
        y = 0;
        w = 0;
        h = 0;
        string[] sArray = strrect.Split(',');
        int idx = 0;
        foreach (string str in sArray)
        {
            if (idx == 0)
            {
                x = Common.String2Int(str);
            }
            if (idx == 1)
            {
                y = Common.String2Int(str);
            }
            if (idx == 2)
            {
                w = Common.String2Int(str);
            }
            if (idx == 3)
            {
                h = Common.String2Int(str);
            }

            idx++;
        }
        Rect rc = new Rect(x, y, w, h);
        return rc;
    }


    void UpdateImagePenSelPosition()
    {
        RectTransform rctran = imagePenSel.GetComponent<RectTransform>();
        Vector2 offsetMax = rctran.offsetMax;
        Vector2 offsetMin = rctran.offsetMin;
        if (Device.isLandscape)
        {
            offsetMax.y = -4;
            offsetMin.y = 4;
        }
        else
        {
            offsetMin.x = 4;
            offsetMax.x = -4;

            offsetMax.y = -4;
        }
        rctran.offsetMax = offsetMax;
        rctran.offsetMin = offsetMin;
        if (!Device.isLandscape)
        {
            Vector2 selsize = rctran.sizeDelta;
            selsize.y = 16;
            //offsetMax 修改之后sizeDelta也会跟着变化，需要还原
            rctran.sizeDelta = selsize;
        }

    }
    public void ShowFirstUseAlert()
    {

        string title = Language.main.GetString("STR_UIVIEWALERT_TITLE_FIRST_USE_FUNCTION");
        string msg = Language.main.GetString("STR_UIVIEWALERT_MSG_FIRST_USE_FUNCTION");
        string yes = Language.main.GetString("STR_UIVIEWALERT_YES_FIRST_USE_FUNCTION");
        string no = "no";
        ViewAlertManager.main.ShowFull(title, msg, yes, no, false, STR_KEYNAME_VIEWALERT_FIRST_USE_FUNCTION, OnUIViewAlertFinished);

    }
    //返回保存提示
    public void ShowSaveTipsAlert()
    {

        string title = Language.main.GetString("STR_UIVIEWALERT_TITLE_SAVE");
        string msg = Language.main.GetString("STR_UIVIEWALERT_MSG_SAVE");
        string yes = Language.main.GetString("STR_UIVIEWALERT_YES_SAVE");
        string no = Language.main.GetString("STR_UIVIEWALERT_NO_SAVE");
        ViewAlertManager.main.ShowFull(title, msg, yes, no, true, STR_KEYNAME_VIEWALERT_SAVE_TIPS, OnUIViewAlertFinished);
    }

    public void ShowSaveAlert()
    {
        string title = Language.main.GetString("STR_UIVIEWALERT_TITLE_SAVE");
        string msg = Language.main.GetString("STR_UIVIEWALERT_MSG_SAVE");
        string yes = Language.main.GetString("STR_UIVIEWALERT_YES_SAVE");
        string no = Language.main.GetString("STR_UIVIEWALERT_NO_SAVE");
        ViewAlertManager.main.ShowFull(title, msg, yes, no, true, STR_KEYNAME_VIEWALERT_SAVE, OnUIViewAlertFinished);
    }

    public void ShowSaveFinishAlert()
    {

        string title = Language.main.GetString("STR_UIVIEWALERT_TITLE_SAVE_FINISH");
        string msg = Language.main.GetString("STR_UIVIEWALERT_MSG_SAVE_FINISH");
        string yes = Language.main.GetString("STR_UIVIEWALERT_YES_SAVE_FINISH");
        string no = "no";
        //  viewAlert.HideDelay(2f);
        ViewAlertManager.main.ShowFull(title, msg, yes, no, false, STR_KEYNAME_VIEWALERT_SAVE_FINISH, OnUIViewAlertFinished);
    }
    void DoBtnBack()
    {
        base.OnClickBtnBack();
    }

    public override void OnClickBtnBack()
    {

        if ((!gamePaint.isHasSave) && (gamePaint.isHasPaint))
        {
            ShowSaveTipsAlert();
            return;
        }
        DoBtnBack();
    }


    public void OnClickBtnSave()
    {
        if (gamePaint.isHasPaint)
        {
            ShowSaveAlert();
        }
    }
    void DoBtnSave()
    {

        if (!gamePaint.isHasPaint)
        {
            //没有作画
            return;
        }

        ColorItemInfo info = GameLevelParse.main.GetItemInfo();
        string filepath = GameLevelParse.main.GetFileSave(info);
        info.infoDB.filesave = filepath;
        gamePaint.SaveImage(filepath);


        bool isexist = DBColor.main.IsItemExist(info.infoDB);
        Debug.Log("IsItemExist:" + isexist);
        if (isexist)
        {
            DBColor.main.UpdateItemTime(info.infoDB);
        }
        else
        {
            DBColor.main.AddItem(info.infoDB);
        }

        ShowSaveFinishAlert();
    }

    public void OnGamePaintClickStraw()
    {
        UpdateColorCur();
    }

    public void OnClickBtnPopTool()
    {
        objLayoutTopBar.SetActive(!objLayoutTopBar.activeSelf);
    }

    public void OnClickBtnLineSetting()
    {
        uiLineSetting.gameObject.SetActive(true);
    }
    public void OnClickBtnColorBoard()
    {
        Debug.Log("OnClickBtnColorBoard");
        ResetPaintModeBeforeColorStraw();
        //colorImage.ApplyTexture();
        uiColorBoard.gameObject.SetActive(!uiColorBoard.gameObject.activeSelf);
    }

    void OnUIViewAlertFinished(UIViewAlert alert, bool isYes)

    {
        if (STR_KEYNAME_VIEWALERT_DELETE_ALL == alert.keyName)
        {
            if (isYes)
            {
                DoDeleteAll();
            }
            else
            {

            }
        }

        if (STR_KEYNAME_VIEWALERT_FIRST_USE_FUNCTION == alert.keyName)
        {
            if (isYes)
            {
                if (btnClickMode == BTN_CLICK_MODE_STRAW)
                {
                    DoClickBtnStrawAlert();
                }
                if (btnClickMode == BTN_CLICK_MODE_COLOR_INPUT)
                {
                    DoClickBtnColorInputAlert();
                }
            }
            else
            {

            }
        }

        if (STR_KEYNAME_VIEWALERT_SAVE_TIPS == alert.keyName)
        {
            if (isYes)
            {
                DoBtnSave();
            }

            DoBtnBack();
        }

        if (STR_KEYNAME_VIEWALERT_SAVE == alert.keyName)
        {
            if (isYes)
            {
                DoBtnSave();
            }
        }

    }

    //恢复颜色吸管之前的模式
    void ResetPaintModeBeforeColorStraw()
    {
        // if (objSpriteStraw.activeSelf)
        // {
        //     objSpriteStraw.SetActive(false);
        //     paintColor.mode = paintColor.modePre;
        // }
    }

    //吸管

    public void DoClickBtnStraw()
    {
        gamePaint.DoClickBtnStraw();
    }

    void DoClickBtnStrawAlert()
    {
        if (AppVersion.appCheckHasFinished && !Application.isEditor)
        {
            if (isFirstUseStraw)
            {
                //show ad video
                AdKitCommon.main.ShowAdVideo();
            }
            else
            {
                DoClickBtnStraw();
            }
        }
        else
        {
            DoClickBtnStraw();
        }
    }

    public void OnClickBtnStraw()
    {
        Debug.Log("OnClickBtnStraw");
        btnClickMode = BTN_CLICK_MODE_STRAW;
        // if (AppVersion.appCheckHasFinished && isFirstUseStraw)
        // {
        //     ShowFirstUseAlert();
        // }
        // else
        {
            DoClickBtnStraw();
        }


    }


    void DoClickBtnColorInput()
    {
        isFirstUseColorInput = false;
        uiColorInput.UpdateInitColor(gamePaint.colorPaint);
        uiColorInput.gameObject.SetActive(!uiColorInput.gameObject.activeSelf);
        Color color = colorPen;
        switch (gamePaint.mode)
        {
            case GamePaint.MODE_PAINT:
                {
                    color = colorPen;
                }
                break;
            case GamePaint.MODE_FILLCOLR:
                {
                    color = colorFill;
                }
                break;
            case GamePaint.MODE_SIGN:
                {
                    color = colorSign;
                }
                break;
        }
        uiColorInput.ColorNow = color;
        uiColorInput.UpdateColorNow();
    }
    void DoClickBtnColorInputAlert()
    {
        if (AppVersion.appCheckHasFinished && !Application.isEditor)
        {
            if (isFirstUseColorInput)
            {
                //show ad video
                AdKitCommon.main.ShowAdVideo();
            }
            else
            {
                DoClickBtnColorInput();
            }
        }
        else
        {
            DoClickBtnColorInput();
        }
    }

    public void OnClickBtnColorInput()
    {
        btnClickMode = BTN_CLICK_MODE_COLOR_INPUT;
        ResetPaintModeBeforeColorStraw();
        // if (AppVersion.appCheckHasFinished && isFirstUseColorInput)
        // {
        //     ShowFirstUseAlert();
        // }
        // else
        {
            DoClickBtnColorInput();
        }
    }

    void DoDeleteAll()
    {
        if (listColorFill != null)
        {
            listColorFill.Clear();
        }

        if (gamePaint != null)
        {
            gamePaint.EraseAll();
        }
    }
    public void OnClickBtnDelAll()
    {
        tickDraw = 0;

        {
            string title = Language.main.GetString("STR_UIVIEWALERT_TITLE_DELETE_ALL_PAINT_POINT");
            string msg = Language.main.GetString("STR_UIVIEWALERT_MSG_DELETE_ALL_PAINT_POINT");
            string yes = Language.main.GetString("STR_UIVIEWALERT_YES");
            string no = Language.main.GetString("STR_UIVIEWALERT_NO");
            ViewAlertManager.main.ShowFull(title, msg, yes, no, true, STR_KEYNAME_VIEWALERT_DELETE_ALL, OnUIViewAlertFinished);
        }


    }
    //彩笔
    public void OnClickBtnColorPen()
    {
        imagePenSel.transform.parent = btnPen.transform;
        UpdateImagePenSelPosition();
        // objSpriteErase.SetActive(false);
        //uiColorBoard.gameObject.SetActive(true);
        gamePaint.colorPaint = colorPen;
        gamePaint.UpdateMode(GamePaint.MODE_PAINT);
        gamePaint.UpdateLineWidth();
        UpdateColorCur();
        TTS.main.Speak(Language.main.GetString("STR_BTN_COLOR_PEN"));
    }
    //魔术笔
    public void OnClickBtnMagicPen()
    {
        imagePenSel.transform.parent = btnMagic.transform;
        UpdateImagePenSelPosition();
        // objSpriteErase.SetActive(false);
        // //uiColorBoard.gameObject.SetActive(false); 
        gamePaint.UpdateMode(GamePaint.MODE_MAGIC);
        gamePaint.UpdateLineWidth();
        TTS.main.Speak(Language.main.GetString("STR_BTN_MAGIC_PEN"));
    }
    //油漆桶
    public void OnClickBtnFillPen()
    {
        imagePenSel.transform.parent = btnFill.transform;
        UpdateImagePenSelPosition();
        //  objSpriteErase.SetActive(false);
        //uiColorBoard.gameObject.SetActive(true);
        gamePaint.colorPaint = colorFill;
        gamePaint.UpdateMode(GamePaint.MODE_FILLCOLR);
        gamePaint.UpdateLineWidth();
        UpdateColorCur();
        TTS.main.Speak(Language.main.GetString("STR_BTN_FILLCOLOR"));
    }
    //印章
    public void OnClickBtnSignPen()
    {
        imagePenSel.transform.parent = btnSign.transform;
        UpdateImagePenSelPosition();
        // objSpriteErase.SetActive(false);
        // //uiColorBoard.gameObject.SetActive(true);
        gamePaint.colorPaint = colorSign;
        gamePaint.UpdateMode(GamePaint.MODE_SIGN);
        UpdateColorCur();
        TTS.main.Speak(Language.main.GetString("STR_BTN_SIGN"));
    }
    //橡皮擦
    public void OnClickBtnErasePen()
    {
        imagePenSel.transform.parent = btnErase.transform;
        UpdateImagePenSelPosition();
        // //uiColorBoard.gameObject.SetActive(false);
        gamePaint.UpdateMode(GamePaint.MODE_ERASE);
        gamePaint.UpdateEraseLineWidth();
        TTS.main.Speak(Language.main.GetString("STR_BTN_ERASE"));
    }

    public void OnClickBtnDelLast()
    {

    }

    public void OnClickMainPaint()
    {
        Debug.Log("OnClickMainPaint");
        if (uiColorBoard.gameObject.activeSelf)
        {
            uiColorBoard.gameObject.SetActive(false);
        }
    }



    public void OnAdKitAdVideoFinish(AdKitCommon.AdType type, AdKitCommon.AdStatus status, string str)
    {
        if (type == AdKitCommon.AdType.VIDEO)
        {
            if (status == AdKitCommon.AdStatus.SUCCESFULL)
            {
                if (btnClickMode == BTN_CLICK_MODE_STRAW)
                {
                    DoClickBtnStraw();
                }
                if (btnClickMode == BTN_CLICK_MODE_COLOR_INPUT)
                {
                    DoClickBtnColorInput();
                }
            }

            if (status == AdKitCommon.AdStatus.FAIL)
            {
                ShowAdVideoFailAlert();
            }

        }
    }
}

