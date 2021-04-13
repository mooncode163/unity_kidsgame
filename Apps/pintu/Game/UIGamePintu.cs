using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;
using Moonma.Share;

public class UIGamePintu : UIGameBase, IGamePintuDelegate
{

    static public int rowGame = 0;
    static public int colGame = 0;
    public GameObject objTopBar;
    public GameEndParticle gameEndParticle;
    public UIPintuOption uiPintuOption;
    // public Image imageTopBar;
    // public Text textTitle;
    public UIButton btnTitle;
    public Button btnBack;

    static public ImageItemInfo infoNetImage;

    int save_image_w = 800;
    int save_image_h = 800;
    bool isSavingImage;
    int saveImageIndex;
    bool isAddSaveImageBg;
    long tickSaveImageSecond;
    GameObject objSaveImage;
    List<GameObject> listSaveImage;

    long tickCapture;

    GameObject objBgGamePic;
    static public Texture2D texGamePic;
    long tickLoadGameTexture;
    long tickclear;
    long tickInitUI;
    long tickAwake;
    bool isNeedShowBgGamePic = false;

    static public GamePintu.ImageSource imageSource = GamePintu.ImageSource.GAME_INNER;

    GamePintu gamePintu;

    Camera mainCamera;
    public static Texture2D texBgGamePic
    {
        get
        {
            return TextureCache.main.Load(CloudRes.main.rootPathGameRes + "/bg_game_pic.png");
            // return LoadTexture.LoadFromAsset(CloudRes.main.rootPathGameRes + "/bg_game_pic.png");
        }


        //ng:
        //  get
        // {
        //     if (_main==null)
        //     {
        //         _main = new DBWord();
        //         Debug.Log("DBWord main init");
        //         _main.CreateDb();
        //     }
        //     return _main;
        // }
    }

    void Awake()
    {
        //   InitScalerMatch();
        tickAwake = Common.GetCurrentTimeMs();
        isSavingImage = false;
        saveImageIndex = 0;
        isAddSaveImageBg = false;
        //audioClipBtn = AudioCache.main.Load(AppRes.AUDIO_BTN_CLICK); 

        mainCamera = AppSceneBase.main.mainCamera;

        // listTexTure = new List<Texture2D>();
        languageGame = new Language();
        languageGame.Init(CloudRes.main.rootPathGameRes + "/language/language.csv");
        languageGame.SetLanguage(Language.main.GetLanguage());

        btnBack.gameObject.SetActive(false);

        uiPintuOption.callbackClose = OnUIPintuOptionDidClose;
        uiPintuOption.callbackSlider = OnUIPintuOptionDidSlider;
    
        ParseGame();

        //bg

        AppSceneBase.main.UpdateWorldBg(AppRes.IMAGE_GAME_BG);

        isNeedShowBgGamePic = false;
        //isNeedShowBgGamePic = true;
        tickAwake = Common.GetCurrentTimeMs() - tickAwake;

        // UpdateBtnMusic();

    }
    // Use this for initialization
    void Start()
    {
        UIGuankaController.tick = Common.GetCurrentTimeMs() - UIGuankaController.tick;
        UpdateGuankaLevel(LevelManager.main.gameLevel);

        //图片批处理将背景透明的png叠加一个大背景
        // LoadConvertImageInit();
        // DoConvertImage();
        // return;


        // ShowGameImage(true);


        // OnScreenShot();

        OnUIDidFinish(1);

    }
    // Update is called once per frame
    void Update()
    {
        // DoConvertImage();



        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnClickBtnBack();
        }
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

        //  GUI.Label(new Rect(0, 40, 400, 200), "A=" + tickAwake + " G = " + UIGuankaController.tick + " T=" + tickLoadGameTexture + " U=" + tickInitUI + " ms", bb);

    }

    void InitUI()
    {
        UpdateTitle();
        LayOut();
    }

    void LoadGameTexture()
    {
        if (imageSource == GamePintu.ImageSource.NET)
        {
            StartParsePic(infoNetImage.pic);
            return;
        }
        ItemInfo info = GameLevelParse.main.GetItemInfo();
        if (imageSource == GamePintu.ImageSource.GAME_INNER)
        {
            texGamePic = LoadTexture.LoadFromAsset(info.pic);
        }
        UpdateGameTexture();
    }

    void UpdateGameTexture()
    {
        //需要提前在MergeTextureScene 里做合并操作
        long tick = Common.GetCurrentTimeMs();
        Texture2D texBg = texBgGamePic;
        if (texGamePic != null)
        {
            if ((texGamePic.format == TextureFormat.ARGB32) || (texGamePic.format == TextureFormat.RGBA32))
            {
                texGamePic = PintuUtil.MergeTextureGPU(texBg, texGamePic);
            }
        }



        gamePintu.UpdateTexture(texGamePic);
        tick = Common.GetCurrentTimeMs() - tick;
        Debug.Log("MergeTexture:tick=" + tick + "ms");
    }

    void StartParsePic(string pic)
    {
        if (Common.BlankString(pic))
        {
            return;
        }
        HttpRequest http = new HttpRequest(OnHttpRequestFinished);
        http.Get(pic);
    }
    void OnHttpRequestFinished(HttpRequest req, bool isSuccess, byte[] data)
    {
        Debug.Log("MoreAppParser OnHttpRequestFinished:isSuccess=" + isSuccess);
        if (isSuccess)
        {
            texGamePic = LoadTexture.LoadFromData(data);
            UpdateGameTexture();
        }
        else
        {

        }
    }
    void OnScreenShot()
    {
        float x, y, w, h;
        Vector2 size = new Vector2(Screen.width, Screen.height);
        w = Screen.width;
        h = Screen.height;
        x = (size.x - w) / 2;
        y = (size.y - h) / 2;
        //     x = 0;
        //    y = 0;
        Rect rc = new Rect(x, y, w, h);

        Common.CaptureCamera(mainCamera, rc, null, size);
        //Common.CaptureScreenshotRect(rc,null);

    }


    public override void LayOut()
    {
        //bg
        if (uiPintuOption != null)
        {
            uiPintuOption.LayOut();
        }
        if (gamePintu != null)
        {
            gamePintu.LayOut(uiPintuOption.gameObject.activeSelf);
        }


    }

    void CheckShowTitle()
    {
        btnTitle.gameObject.SetActive(true);
        ItemInfo info = GameLevelParse.main.GetItemInfo();
        if (!languageGame.IsContainsKey(info.id))
        {
            btnTitle.gameObject.SetActive(false);
        }

        if (uiPintuOption.gameObject.activeSelf)
        {
            btnTitle.gameObject.SetActive(false);
        }

        if (imageSource != GamePintu.ImageSource.GAME_INNER)
        {
            btnTitle.gameObject.SetActive(false);
            return;
        }
        if (!Config.main.APP_FOR_KIDS)
        {
            btnTitle.gameObject.SetActive(false);
        }
    }
    void UpdateTitle()
    {
        CheckShowTitle();
        int idx = LevelManager.main.gameLevel;
        ItemInfo info = GameLevelParse.main.GetItemInfo();
        if (!languageGame.IsContainsKey(info.id))
        {
            return;
        }
        string str = languageGame.GetString(info.id);
        Debug.Log("UpdateTitle info.id" + info.id + " str=" + str);
        btnTitle.textTitle.text = str;
        // int fontsize = textTitle.fontSize;
        // float str_w = Common.GetStringLength(str, AppString.STR_FONT_NAME, fontsize);
        // RectTransform rctran = imageTopBar.transform as RectTransform;
        // Vector2 sizeDelta = rctran.sizeDelta;
        // float oft = 0;//AppResImage.REN_XUXIANKUANG_CIRCLE_R * AppCommon.scaleBase;
        // sizeDelta.x = str_w + fontsize + oft * 2;
        // rctran.sizeDelta = sizeDelta;
        //rctran.anchoredPosition = new Vector2(sizeCanvas.x / 2, rctran.anchoredPosition.y);

        if (imageSource == GamePintu.ImageSource.GAME_INNER)
        {
            TTS.main.Speak(str);
        }


    }

    public string GetImageResourcePath(int lv)
    {
        ParseGame();
        ItemInfo info = GameLevelParse.main.GetGuankaItemInfo(lv);
        return info.pic;
    }

    // public override int GetPlaceTotal()
    // {
    //     return 4;
    // }

    public override void UpdateGuankaLevel(int level)
    {
        base.UpdateGuankaLevel(level);
        tickclear = Common.GetCurrentTimeMs();
        AppSceneBase.main.ClearMainWorld();

        GameObject objgame = new GameObject("GamePintu");
        gamePintu = objgame.AddComponent<GamePintu>();
        gamePintu.iDelegate = this;
        gamePintu.row = rowGame;
        gamePintu.col = colGame;
        ItemInfo info = GameLevelParse.main.GetItemInfo();
        gamePintu.infoGuankaItem = info;
        gamePintu.CreateBlock();
        AppSceneBase.main.AddObjToMainWorld(objgame);
        gamePintu.transform.localPosition = new Vector3(0, 0, -1f);

        tickclear = Common.GetCurrentTimeMs() - tickclear;
        tickLoadGameTexture = Common.GetCurrentTimeMs();
        LoadGameTexture();

        tickLoadGameTexture = Common.GetCurrentTimeMs() - tickLoadGameTexture;
        Debug.Log("LoadGameTexture:tick=" + tickLoadGameTexture + "ms");
        tickInitUI = Common.GetCurrentTimeMs();
        InitUI();
        tickInitUI = Common.GetCurrentTimeMs() - tickInitUI;
    }

    public void OnUIPintuOptionDidClose(UIPintuOption pintuOption)
    {
        //StartGame();
        // ShowGameImage(false);
        if (objBgGamePic != null)
        {
            objBgGamePic.SetActive(true);
        }

        // objGamePic.SetActive(true);
        btnBack.gameObject.SetActive(true);
        gamePintu.ShowItemAnimate();
        CheckShowTitle();
    }
    public void OnUIPintuOptionDidSlider(UIPintuOption pintuOption)
    {
        UpdateGuankaLevel(LevelManager.main.gameLevel);
    }



    void ParseGame()
    {
        // ParseGuanka();
    }





    public void ShowAdVideoTips()
    {
        string title = Language.main.GetString("STR_UIVIEWALERT_TITLE_ADVIDEO_TIPS");
        string msg = Language.main.GetString("STR_UIVIEWALERT_MSG_ADVIDEO_TIPS");
        string yes = Language.main.GetString("STR_UIVIEWALERT_YES_ADVIDEO_TIPS");
        string no = yes;

        ViewAlertManager.main.ShowFull(title, msg, yes, no, false, STR_KEYNAME_VIEWALERT_GAME_FINISH, OnUIViewAlertFinishedAdVideoTips);
    }

    public void OnGamePintuDidBack(GamePintu ui)
    {
        OnClickBtnBack();
    }
    public void OnGamePintuDidGameWin(GamePintu ui)
    {
        OnGameWinBase();
    }
    public void OnGamePintuDidNextLevel(GamePintu ui)
    {
        LevelManager.main.GotoNextLevel();
        OnUIPintuOptionDidClose(uiPintuOption);
        // ShowAdInsert(GAME_AD_INSERT_SHOW_STEP);
    }
    public override void OnUIShareDidClick(ItemInfo item)
    {
        Debug.Log("GamePintu OnUIShareDidClick");
        string title = Language.main.GetString("UIGAME_SHARE_TITLE");
        string detail = Language.main.GetString("UIGAME_SHARE_DETAIL");
        string url = Config.main.shareAppUrl;
        Share.main.ShareWeb(item.source, title, detail, url);
    }

    void OnUIViewAlertFinished(UIViewAlert alert, bool isYes)
    {

        if (isYes)
        {
            LevelManager.main.GotoNextLevel();
            OnUIPintuOptionDidClose(uiPintuOption);
        }
        else
        {
            OnClickBtnBack();
        }
    }

    void OnUIViewAlertFinishedAdVideoTips(UIViewAlert alert, bool isYes)
    {

        if (isYes)
        {
            OnShowAdVideo();

        }
        if (uiPintuOption != null)
        {
            uiPintuOption.isNeedAdVideoTips = false;
        }

    }

    public void OnShowAdVideo()
    {
        AdKitCommon.main.callbackAdVideoFinish = OnAdKitFinishAdVideo;
        AdKitCommon.main.ShowAdVideo();
    }

    public override void OnClickBtnBack()
    {

        AudioPlay.main.PlayFile(AppRes.AUDIO_BTN_CLICK);
        Debug.Log("OnClickBtnBack");
        base.OnClickBtnBack();
    }

    void DoConvertImage()
    {


        int idx = 0;
        // foreach (ItemInfo info in listGuanka)
        if (saveImageIndex < GameLevelParse.main.listGuanka.Count)
        {

            ItemInfo info = GameLevelParse.main.GetGuankaItemInfo(saveImageIndex);
            ConvertImage(info);
            // {

            //     StartCoroutine(DelayRun.Run(() =>

            //     {
            //         ConvertImage(info);

            //     }, 1f));
            //     idx++;


            // }

        }

    }


    GameObject LoadConvertImageItem(ItemInfo info)
    {


        float z = -20f;

        GameObject obj = new GameObject(info.id);//"GameItem" + 

        obj.transform.position = new Vector3(0, 0, z - 1);
        obj.AddComponent<RectTransform>();
        RectTransform rcTran = obj.GetComponent<RectTransform>();
        obj.AddComponent<SpriteRenderer>();
        SpriteRenderer objSR = obj.GetComponent<SpriteRenderer>();


        // obj = (GameObject)GameObject.Instantiate(obj);
        //Debug.Log(info.pic);
        Texture2D tex = (Texture2D)Resources.Load(info.pic);
        //listTexTure.Add(tex);
        Debug.Log("tex,w:" + tex.width + " h:" + tex.height);
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        sprite.name = "DaXiang";
        objSR.sprite = sprite;
        float pixsel_w = save_image_w;//GAME_ITEM_SCREEN_WIDTH * AppCommon.scaleBase;
        float pixsel_h = save_image_h;
        Vector2 sizeWorld = Common.ScreenToWorldSize(mainCamera, new Vector2(pixsel_w, pixsel_h));
        float pixsel_per_unit = 100;
        float tex_world_w = tex.width / pixsel_per_unit;
        float tex_world_h = tex.height / pixsel_per_unit;
        float scale_x = sizeWorld.x / tex_world_w;
        float scale_y = sizeWorld.y / tex_world_h;
        float scale2 = Mathf.Min(scale_x, scale_y);

        obj.transform.localScale = new Vector3(scale2, scale2, 1f);
        rcTran.sizeDelta = new Vector2(tex_world_w, tex_world_h);
        obj.SetActive(false);
        listSaveImage.Add(obj);
        return obj;
    }

    void ConvertImageNext()
    {
        objSaveImage.SetActive(false);
        GameObject.Destroy(objSaveImage);
        saveImageIndex++;
        tickSaveImageSecond = Common.GetCurrentTimeSecond() - tickSaveImageSecond;
        // Debug.Log("SaveImage:time=" + tickSaveImageSecond + " idx=" + saveImageIndex + " total=" + listGuanka.Count + " tickCapture=" + tickCapture + "ms");
        DoConvertImage();

    }
    void ConvertImage(ItemInfo info)
    {
        GameObject obj = LoadConvertImageItem(info);
        // GameObject obj =listSaveImage[saveImageIndex];
        obj.SetActive(true);
        tickSaveImageSecond = Common.GetCurrentTimeSecond();
        //     StartCoroutine(DelayRun.Run(() =>

        //    {
        float x, y, w, h;
        Vector2 size = new Vector2(Screen.width, Screen.height);
        w = save_image_w;
        h = save_image_h;
        x = (size.x - w) / 2;
        y = (size.y - h) / 2;
        //     x = 0;
        //    y = 0;
        Rect rc = new Rect(x, y, w, h);
        int idx = LevelManager.main.placeLevel;
        ItemInfo infoPlace = LevelManager.main.GetPlaceItemInfo(idx);
        string strPlace = infoPlace.id;

        string filedir = Application.dataPath + "/convertimage/" + strPlace;
        //创建文件夹
        Directory.CreateDirectory(filedir);
        string filepath = filedir + "/" + info.id + ".png";
        tickCapture = Common.GetCurrentTimeMs();
        Common.CaptureCamera(mainCamera, rc, filepath, size);
        tickCapture = Common.GetCurrentTimeMs() - tickCapture;

        objSaveImage = obj;
        Invoke("ConvertImageNext", 0.5f);
        //ConvertImageNext();
        //延迟时间太短的话可能截不到图 8f
        //     StartCoroutine(DelayRun.Run(() =>
        // {

        //     objSaveImage.SetActive(false);
        //     GameObject.Destroy(objSaveImage);
        //     saveImageIndex++;
        //      tickSaveImageSecond = Common.GetCurrentTimeSecond()-tickSaveImageSecond;
        //      Debug.Log("SaveImage:time="+tickSaveImageSecond+" idx="+saveImageIndex+" total="+listGuanka.Count+" tickCapture="+tickCapture+"ms");
        //     DoConvertImage();
        // }, 10f));


        //    }, 5f));

        //

    }

    public void OnAdKitFinishAdVideo(AdKitCommon.AdType type, AdKitCommon.AdStatus status, string str)
    {
        if (type == AdKitCommon.AdType.VIDEO)
        {
            if (status == AdKitCommon.AdStatus.SUCCESFULL)
            {
                //uiPintuOption.DoSliderValueChanged();
            }
            if (status == AdKitCommon.AdStatus.FAIL)
            {
                //ShowAdVideoFailAlert();
            }
        }
    }

}
