using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Moonma.Tongji;
using Moonma.AdKit.AdConfig;

public class AppSceneBase : ScriptBase
{
    AppVersion appVersion;
    public Image imageBg;
    public UIViewController rootViewController;
    public List<UIViewPop> listPopup;

    public Canvas canvasMain;
    public Light lightMain;
    public Canvas canvasCamera;
    public GameObject objMainWorld;
    public GameObject objSpriteBg;

    public GameObject objuAudio;

    //public Camera mainCamera;
    float _adBannerHeightCanvas = 0;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary> 
    public static AppSceneBase main;

    bool isReLayout = false;
    HttpRequest httpReqBg;

    void Awake()
    {
        if (AppSceneBase.main == null)
        {
            AppSceneBase.main = this;
        }
        isReLayout = false;
        IPInfo.main.StartParserInfo();
        InitScalerMatch();
        if (canvasCamera != null)
        {
            SetCanvasScalerMatch(canvasCamera.gameObject);
        }
        Common.CleanCache();
        AdConfig.main.InitSDK();
        InitValue();

        //Component
        this.gameObject.AddComponent<AdKitCommon>();
        this.gameObject.AddComponent<IAPCommon>();
        this.gameObject.AddComponent<ShareCommon>();
        this.gameObject.AddComponent<TTSCommon>();
        //this.gameObject.AddComponent<MusicBgPlay>();
        this.gameObject.AddComponent<AudioPlay>();
        this.gameObject.AddComponent<PopUpManager>();


        //app启动初始化多线程工具LOOM
        Loom loom = Loom.Current;

        //初始化广告id key等参数
        AdConfig adcf = AdConfig.main;

        //bg
        // Texture2D tex = AppResImage.texMainBg;
        // if (tex)
        {
            //ios unity 2017.3.1 Sprite.Create 会出现crash
            // Sprite sp = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            // imageBg.sprite = sp;
            // RectTransform rctan = imageBg.GetComponent<RectTransform>();
            // rctan.sizeDelta = new Vector2(tex.width, tex.height);
        }


        if (Common.isWeb)
        {
            GameManager.main.PreLoadDataForWeb();
        }
    }

    // Use this for initialization
    void Start()
    {
        if (Device.isLandscape)
        {
            OnStart();
        }
        else
        {
            //竖屏等待canvas调整大小 InitScalerMatch
            Invoke("OnStart", 0.1f);
        }

    }
    void OnStart()
    {
        isHasStarted = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isHasStarted)
        {
            isHasStarted = false;
            InitUiScaler();
            UpdateMainWorldRect(0);
            //LayoutChild();
            RunCheckApp();

            OnResize();
        }
        if (Device.isScreenDidChange)
        {
            InitScalerMatch();
            if (canvasCamera != null)
            {
                SetCanvasScalerMatch(canvasCamera.gameObject);
            }
            isReLayout = true;

        }

        if (isReLayout)
        {
            isReLayout = false;
            OnResize();
        }
    }

    public void OnResize()
    {
        //InitScalerMatch 和 InitUiScaler 异步执行
        InitUiScaler();
        UpdateMainWorldRect(_adBannerHeightCanvas);
        LayoutChild();
        if (rootViewController != null)
        {
            rootViewController.UpdateCanvasSize(sizeCanvas);
        }
        // Debug.Log("Device.isScreenDidChange:sizeCanvas = " + sizeCanvas);
    }

    void OnAppVersionFinished(AppVersion app)
    {
        Debug.Log("StartScene OnAppVersionFinished");
        RunApp();
    }

    void InitValue()
    {
        bool isFirstRun = !Common.GetBool(AppString.STR_KEY_NOT_FIRST_RUN);
        mainCamera = Common.GetMainCamera();

        listPopup = new List<UIViewPop>();


        Tongji.Init(Config.main.GetString("APPTONGJI_ID", "0"));
        Device.InitOrientation();

        if (isFirstRun)
        {
            Common.gold = AppRes.GOLD_INIT_VALUE;
            //第一次安装
            Common.SetBool(AppString.STR_KEY_NOT_FIRST_RUN, true);

            Common.SetBool(AppString.KEY_ENABLE_PLAYSOUND, true);
            Common.SetBool(AppString.STR_KEY_BACKGROUND_MUSIC, true);

            int lanTag = (int)Application.systemLanguage;
            PlayerPrefs.SetInt(AppString.STR_KEY_LANGUAGE, lanTag);
            SystemLanguage lan = (SystemLanguage)lanTag;
            Language.main.SetLanguage(lan);
        }
        else
        {

            int lanTag = PlayerPrefs.GetInt(AppString.STR_KEY_LANGUAGE);
            SystemLanguage lan = (SystemLanguage)lanTag;
            //lan = SystemLanguage.English;

            Language.main.SetLanguage(lan);

        }

        SetMode3D(Config.main.Is3D);

    }
    void SetMode3D(bool is3D)
    {
        if (lightMain != null)
        {
            lightMain.gameObject.SetActive(is3D);
        }
        if (is3D)
        {
            mainCamera.orthographic = false;
            //-10f
            mainCamera.transform.position = new Vector3(0, 0, -10f);
        }
    }
    void RunCheckApp()
    {
        appVersion = AppVersion.main;
        if (!AppVersion.appCheckHasFinished)
        {
            appVersion.callbackFinished = OnAppVersionFinished;
            appVersion.StartParseVersion();
        }
        else
        {
            appVersion.callbackFinished = null;
            RunApp();
        }

    }

    public virtual void RunApp()
    {
        Debug.Log("base RunApp");

        //  Common.UnityStartUpFinish();
    }

    public void SetRootViewController(UIViewController controller)
    {
        float x = 0, y = 0;
        if (rootViewController != null)
        {
            rootViewController.DestroyObjController();
        }
        rootViewController = controller;
        rootViewController.SetViewParent(canvasMain.gameObject);


        float oft_top = Common.ScreenToCanvasHeigt(sizeCanvas, Device.offsetTop);
        float oft_bottom = Common.ScreenToCanvasHeigt(sizeCanvas, Device.offsetBottom);
        float oft_left = Common.ScreenToCanvasWidth(sizeCanvas, Device.offsetLeft);
        float oft_right = Common.ScreenToCanvasWidth(sizeCanvas, Device.offsetRight);
        RectTransform rctran = rootViewController.objController.GetComponent<RectTransform>();
        y = oft_bottom - oft_top;
        x = oft_left - oft_right;
        //rctran.anchoredPosition = new Vector2(x, y);
        rctran.offsetMin = new Vector2(oft_left, oft_bottom);
        rctran.offsetMax = new Vector2(-oft_right, -oft_top);

        OnResize();
    }

    public void ClearMainWorld()
    {
        int idx = 0;
        //FindObjectOfType

        // 
        // Transform[] allChild = objMainWorld.transform.FindObjectOfType(typeof(Transform)) as Transform[];

        /* 
        foreach (Transform child in objMainWorld.transform)这种方式遍历子元素会漏掉部分子元素
        */

        //GetComponentsInChildren寻找的子对象也包括父对象自己本身和子对象的子对象
        foreach (Transform child in objMainWorld.GetComponentsInChildren<Transform>(true))
        {
            if (child == null)
            {
                // 过滤已经销毁的嵌套子对象
                Debug.Log("ClearMainWorld child is null idx=" + idx);
                continue;
            }
            GameObject objtmp = child.gameObject;
            if ((objMainWorld == objtmp) || (objSpriteBg == objtmp))
            {
                continue;
            }
            //Debug.Log("ClearMainWorld idx=" + idx + " name=" + objtmp.name);
            GameObject.DestroyImmediate(objtmp);//Destroy
            objtmp = null;

            idx++;
        }
        // Debug.Log("ClearMainWorld idx=" + idx);
    }

    public void UpdateMainWorldRect(float adBannerHeightCanvas)
    {
        float x, y, w, h;
        _adBannerHeightCanvas = adBannerHeightCanvas;
        float adBannerHeight_world = Common.CanvasToWorldHeight(mainCamera, sizeCanvas, adBannerHeightCanvas);
        //world
        float oft_top = Common.ScreenToWorldHeight(mainCamera, Device.offsetTop);
        float oft_bottom = Common.ScreenToWorldHeight(mainCamera, Device.offsetBottom);
        float oft_left = Common.ScreenToWorldWidth(mainCamera, Device.offsetLeft);
        float oft_right = Common.ScreenToWorldWidth(mainCamera, Device.offsetRight);


        RectTransform rctranWorld = objMainWorld.GetComponent<RectTransform>();
        Vector2 size = Common.GetWorldSize(mainCamera);
        w = size.x;// - oft_left - oft_right;
        h = size.y;// - adBannerHeight_world - oft_top - oft_bottom;
        //y = mainCamera.orthographicSize - oft_top - h / 2;
        y = 0;
        x = 0;
        float z = rctranWorld.position.z;
        rctranWorld.sizeDelta = new Vector2(w, h);
        rctranWorld.position = new Vector3(x, y, z);
        LayoutChild();
    }

    public void LayoutChild()
    {
        if (imageBg != null)
        {
            RectTransform rectTransform = imageBg.GetComponent<RectTransform>();
            float w_image = rectTransform.rect.width;
            float h_image = rectTransform.rect.height;
            print(rectTransform.rect);
            float scalex = sizeCanvas.x / w_image;
            float scaley = sizeCanvas.y / h_image;
            float scale = Mathf.Max(scalex, scaley);
            imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);
            //屏幕坐标 现在在屏幕中央
            imageBg.transform.position = new Vector2(Screen.width / 2, Screen.height / 2);
        }

        if (objSpriteBg != null)
        {
            SpriteRenderer render = objSpriteBg.GetComponent<SpriteRenderer>();
            Vector2 worldsize = Common.GetWorldSize(mainCamera);
            Sprite sp = render.sprite;
            if (sp != null)
            {
                Texture2D tex = sp.texture;
                float w = tex.width / 100f;//render.size.x;
                float h = tex.height / 100f;//render.size.y;
                if ((w != 0) && (h != 0))
                {
                    float scalex = worldsize.x / w;
                    float scaley = worldsize.y / h;
                    float scale = Mathf.Max(scalex, scaley);
                    objSpriteBg.transform.localScale = new Vector3(scale, scale, 1.0f);
                    objSpriteBg.transform.position = new Vector3(0, 0, objSpriteBg.transform.position.z);
                }

            }

        }

        if (rootViewController != null)
        {
            UIView ui = rootViewController.view;
            if (ui != null)
            {
                ui.LayOut();
            }

        }
        int len = AppSceneBase.main.listPopup.Count;
        for (int i = 0; i < len; i++)
        {
            UIViewPop ui = AppSceneBase.main.listPopup[i];
            if (ui != null)
            {
                ui.LayOut();
            }
        }
    }
    void OnHttpRequestFinished(HttpRequest req, bool isSuccess, byte[] data)
    {
        if (req == httpReqBg)
        {
            Texture2D tex = LoadTexture.LoadFromData(data);
            OnGetBgFileDidFinish(isSuccess, tex, false, req.strUrl);

        }
    }
    void OnGetBgFileDidFinish(bool isSuccess, Texture2D tex, bool isLocal, string filepath)
    {
        if (isSuccess && (tex != null))
        {
            TextureCache.main.AddCache(filepath, tex);
            SpriteRenderer render = objSpriteBg.GetComponent<SpriteRenderer>();
            render.sprite = TextureUtil.CreateSpriteFromTex(tex);
            LayoutChild();
        }

    }
    public void UpdateWorldBg(string pic,bool isByKey=false)
    {
        string picnew = pic;
        if(isByKey)
        {
            picnew = ImageRes.main.GetImage(pic);
        }
        bool is_cache = TextureCache.main.IsInCache(picnew);
        if (is_cache)
        {
            Texture2D tex = TextureCache.main.Load(picnew);
            OnGetBgFileDidFinish(true, tex, true, picnew);
        }
        else
        {
            if (Common.isWeb)
            {
                httpReqBg = new HttpRequest(OnHttpRequestFinished);
                httpReqBg.Get(HttpRequest.GetWebUrlOfAsset(picnew));
            }
            else
            {
                Texture2D tex = LoadTexture.LoadFileAuto(picnew);
                Debug.Log("UpdateWorldBg::picnew=" + picnew + " w=" + tex.width + " h=" + tex.height);
                OnGetBgFileDidFinish(true, tex, true, picnew);
            }
        }



    }

    public void AddObjToMainWorld(GameObject obj)
    {
        obj.transform.parent = objMainWorld.transform;
        obj.transform.localScale = new Vector3(1f, 1f, 1f);
    }
    public void AddObjToMainCanvas(GameObject obj)
    {
        obj.transform.parent = canvasMain.transform;
        obj.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void AddObjToCanvasCamera(GameObject obj)
    {
        obj.transform.parent = canvasCamera.transform;
        obj.transform.localScale = new Vector3(1f, 1f, 1f);
    }
    public RectTransform GetRectMainWorld()
    {
        RectTransform rcTran = objMainWorld.GetComponent<RectTransform>();
        return rcTran;
    }

    public void ShowMainCamera2D(bool isShow)
    {
        mainCamera.gameObject.SetActive(isShow);
    }

    public void ShowCanvas(bool isShow)
    {
        canvasMain.gameObject.SetActive(isShow);
    }

    public void ShowRootViewController(bool isShow)
    {
        rootViewController.objController.SetActive(isShow);
    }
    public void UpdateLanguage()
    {
        if (rootViewController != null)
        {
            rootViewController.UpdateLanguage();
        }
        int len = AppSceneBase.main.listPopup.Count;
        for (int i = 0; i < len; i++)
        {
            UIViewPop ui = AppSceneBase.main.listPopup[i];
            ui.UpdateLanguage();
        }
    }


    //android callback
    public void OnAndroidGlobalLayout(string str)
    {
        Debug.Log("OnAndroidGlobalLayout::str=" + str);
        LayoutChild();
    }


    public void OnWebViewDidFinish(string html)
    {
        AppVersionHuawei.main.OnWebViewDidFinish(html);
    }
    public void OnWebViewDidFail(string html)
    {
        AppVersionHuawei.main.OnWebViewDidFail(html);
    }
}
