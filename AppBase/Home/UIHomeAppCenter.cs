using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHomeAppCenter : UIView
{
    public const string AD_JSON_FILE_KIDS = "applist_home_kids.json";
    public const string AD_JSON_FILE_SMALL_GAME = "applist_home_minigame.json";

    //儿童连连乐 微信小程序id:wx3e44af039aee1b96   
    public const string APPCENTER_HTTP_URL_HOME_SMALL_GAME = "https://6c69-lianlianle-shkb3-1259451541.tcb.qcloud.la/AppCenter/applist_home_minigame.json?sign=df5d132d6ba14d440de4586d9bb66845&t=1589273169";

    //http://www.mooncore.cn/moonma/app_center/applist_home_kids.json
    public const string APPCENTER_HTTP_URL_HOME_KIDS_GAME = "https://6c69-lianlianle-shkb3-1259451541.tcb.qcloud.la/AppCenter/applist_home_kids.json?sign=2354d163fe32793f4512ad648eafc6b9&t=1589273008";
    public Button btnAppIcon0;
    public Button btnAppIcon1;
    public Button btnAppIcon2;

    HttpRequest httpReqJson;
    List<ItemInfo> listApp;
    List<HttpRequest> listHttpReqImage;
    int num_display = 3;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public void Awake()
    {
        //先不显示
        if (btnAppIcon0)
        {
            btnAppIcon0.GetComponent<Image>().color = Color.clear;
        }
        if (btnAppIcon1)
        {
            btnAppIcon1.GetComponent<Image>().color = Color.clear;
        }
        if (btnAppIcon2)
        {
            btnAppIcon2.GetComponent<Image>().color = Color.clear;
        }

        this.gameObject.SetActive(false);


        StartParserAppList();
        LayOut();
    }
    // Use this for initialization
    public void Start()
    {
        LayOut();
    }
    // Update is called once per frame
    void Update()
    {

    }

    public override void LayOut()
    {
        base.LayOut();
        float x = 0, y = 0, w = 0, h = 0;
        //layoutappicon 

        Rect rectParent = this.frameParent;
        {
            GridLayoutGroup gridLayout = this.GetComponent<GridLayoutGroup>();
            Vector2 cellSize = gridLayout.cellSize;
            RectTransform rctran = this.transform as RectTransform;
            float oft = 16f;
            if (Device.isLandscape)
            {
                w = cellSize.x;
                h = (cellSize.y + oft) * num_display;
                rctran.sizeDelta = new Vector2(w, h);
                x = rectParent.width / 2 - rctran.sizeDelta.x / 2 - oft;
                y = 0;
            }
            else
            {
                w = (cellSize.x + oft) * num_display;
                h = cellSize.y;
                rctran.sizeDelta = new Vector2(w, h);
                x = 0;
                y = -rectParent.height / 2 + rctran.sizeDelta.y / 2 + oft;
            }
            rctran.anchoredPosition = new Vector2(x, y);
        }


    }

    void StartParserAppList()
    {

        bool enable = false;
        if (Common.isiOS)
        {
            enable = true;
        }
        else
        {
            if (AppVersion.appCheckHasFinished)
            {
                enable = true;
            }
        }

        if (Common.noad)
        {
            enable = false;
        }

        if (Common.isWinUWP)
        {
            enable = false;
        }

        this.gameObject.SetActive(false);
        if (enable)
        {
            this.gameObject.SetActive(true);
        }


        if (!enable)
        {
            return;
        }
        listHttpReqImage = new List<HttpRequest>();
        listApp = new List<ItemInfo>();
        string url = APPCENTER_HTTP_URL_HOME_KIDS_GAME;
        if (!Config.main.APP_FOR_KIDS)
        {
            url = APPCENTER_HTTP_URL_HOME_SMALL_GAME;
        }
        httpReqJson = new HttpRequest(OnHttpRequestFinished);
        httpReqJson.Get(url);

    }
    void parserAppList(byte[] data)
    {
        MoreAppParser.parserJson(data, listApp);
        //display
        int idx = 0;
        foreach (ItemInfo infoapp in listApp)
        {
            if (idx < num_display)
            {
                HttpRequest http = new HttpRequest(OnHttpRequestFinishedImage);
                listHttpReqImage.Add(http);
                startParserImage(http, infoapp.pic, idx);
            }

            idx++;
        }
    }

    public void startParserImage(HttpRequest http, string url, int idx)
    {

        http.index = idx;
        http.Get(url);
    }

    IEnumerator LoadIconImage(int idx, byte[] data)
    {
        yield return null;


    }
    void OnHttpRequestFinishedImage(HttpRequest req, bool isSuccess, byte[] data)
    {
        if (isSuccess)
        {
            //StartCoroutine(LoadIconImage(req.index, data));
            int idx = req.index;
            Button btn = null;
            switch (idx)
            {
                case 0:
                    {
                        btn = btnAppIcon0;
                    }
                    break;
                case 1:
                    {
                        btn = btnAppIcon1;
                    }
                    break;
                case 2:
                    {
                        btn = btnAppIcon2;
                    }
                    break;

            }
            if (btn != null)
            {
                btn.GetComponent<Image>().color = Color.white;
                Texture2D tex = new Texture2D(0, 0, TextureFormat.ARGB32, false);
                tex.LoadImage(data);
                btn.GetComponent<Image>().sprite = TextureUtil.CreateSpriteFromTex(RoundRectTexture(tex));

            }
            LayOut();
        }


    }
    IEnumerator OnHttpRequestFinishedEnumerator(bool isSuccess, byte[] data)
    {
        yield return null;

    }

    void OnHttpRequestFinished(HttpRequest req, bool isSuccess, byte[] data)
    {
        // StartCoroutine(OnHttpRequestFinishedEnumerator(isSuccess, data));

        if (isSuccess)
        {
            parserAppList(data);

        }
        else
        {
            string filepath = Common.rootDirAppCenter + "/" + AD_JSON_FILE_SMALL_GAME;
            if (Config.main.APP_FOR_KIDS)
            {
                filepath = Common.rootDirAppCenter + "/" + AD_JSON_FILE_KIDS;
            }

            byte[] dataApp = FileUtil.ReadDataFromResources(filepath);
            if (dataApp != null)
            {
                parserAppList(dataApp);
            }
        }
    }

    //圆角
    Texture2D RoundRectTexture(Texture2D tex)
    {
        int w = tex.width;
        int h = tex.height;
        RenderTexture rt = new RenderTexture(w, h, 0);
        string strshader = "Custom/RoundRect";
        //string str = FileUtil.ReadStringAsset(ShotBase.STR_DIR_ROOT_SHADER+"/ShaderRoundRect.shader");
        Material mat = new Material(Shader.Find(strshader));//
        float value = (156 * 1f / 1024);
        //Debug.Log("RoundRectTexture:value=" + value);
        //value = 0.1f;
        //设置半径 最大0.5f
        mat.SetFloat("_RADIUSBUCE", value);
        Graphics.Blit(tex, rt, mat);
        Texture2D texRet = TextureUtil.RenderTexture2Texture2D(rt);
        return texRet;
    }

    void GotoAppUrl(int idx)
    {
        if (listApp == null)
        {
            return;
        }
        ItemInfo info = listApp[idx];
        string appstorePackage = "";
        string appstore = Source.APPSTORE;
        if (Common.isAndroid)
        {
            if (Config.main.channel == Source.TAPTAP)
            {
                appstore = Source.TAPTAP;
                appstorePackage = AppVersion.PACKAGE_APPSTORE_TAPTAP;
            }

            if (Config.main.channel == Source.XIAOMI)
            {
                appstore = Source.XIAOMI;
                appstorePackage = AppVersion.PACKAGE_APPSTORE_XIAOMI;
            }
            if (Config.main.channel == Source.HUAWEI)
            {
                appstore = Source.HUAWEI;
                appstorePackage = AppVersion.PACKAGE_APPSTORE_HUAWEI;
            }

        }
        AppVersion.main.GotoToAppstoreApp(appstore, info.id, appstorePackage, info.url);


    }


    public void OnUIParentGateDidCloseAppCenter(UIParentGate ui, bool isLongPress)
    {
        if (isLongPress)
        {
            Debug.Log("OnUIParentGateDidCloseAppCenter");
            GotoAppUrl(ParentGateViewController.main.index);
        }
    }
    public void ShowParentGate(int idx)
    {
        Debug.Log("ShowParentGate idx=" + idx);

        ParentGateViewController.main.index = idx;
        ParentGateViewController.main.Show(null, null);
        ParentGateViewController.main.ui.callbackClose = OnUIParentGateDidCloseAppCenter;

    }

    public void DoClickBtnAppIcon(int idx)
    {
        if (Config.main.APP_FOR_KIDS)
        {
            ShowParentGate(idx);
        }
        else
        {
            GotoAppUrl(idx);
        }
    }

    public void OnClickBtnAppIcon0()
    {
        DoClickBtnAppIcon(0);
    }
    public void OnClickBtnAppIcon1()
    {
        DoClickBtnAppIcon(1);
    }
    public void OnClickBtnAppIcon2()
    {
        DoClickBtnAppIcon(2);
    }

}
