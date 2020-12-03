using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;
using Moonma.IAP;
using Moonma.Share;
using UnityEngine;
using UnityEngine.UI;

public class UIAdBanner : UIView
{

    public const string PREFAB_UIAdBanner = "Common/Prefab/AdKit/Banner/UIAdBanner";
    public const string URL_AD_LIST = "https://6d6f-moonma-dbb297-1258816908.tcb.qcloud.la/adbanner/ad_list.json?sign=747d21f7431c81908c4ef07d19ae096b&t=1559294845";
    public Button btnClose;
    public Text textTitle;
    public Text textDetail;
    public RawImage imageBg;
    public RawImage imageIcon;
    public RawImage imageAd;
    float timeUpdate = 30f;//second
    public List<ItemInfo> listAd;

    HttpRequest httpReqBg;
    HttpRequest httpReqIcon;

    bool isDownloadBg;
    bool isDownloadIcon;
    int indexAd;
    public float offsetY = 0;
    bool isDestroy = false;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        listAd = new List<ItemInfo>();
        indexAd = 0;
        this.gameObject.SetActive(false);

        TextureUtil.UpdateRawImageTexture(imageAd, "Common/UI/Home/AdBannerIconAd", true);

        StartParseAd();
        LayOut();
    }

    void Start()
    {
        isDestroy = false;
        LayOut();
    }
    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        isDestroy = true;
    }
    public void OnUpdateTime()
    {
        UpdateItem();
        Invoke("OnUpdateTime", timeUpdate);
    }


    public void UpdateItem()
    {
        if (listAd == null || (listAd.Count == 0))
        {
            return;
        }
        ItemInfo info = listAd[indexAd];
        textTitle.text = info.title;
        textDetail.text = info.description;

        isDownloadBg = false;
        isDownloadIcon = false;

        httpReqBg = new HttpRequest(OnHttpRequestFinishedImage);
        Debug.Log("UIAdBanner info.pic=" + info.pic);
        httpReqBg.Get(info.pic);

        httpReqIcon = new HttpRequest(OnHttpRequestFinishedImage);
        httpReqIcon.Get(info.icon);

        indexAd++;
        if (indexAd >= listAd.Count)
        {
            indexAd = 0;
        }
    }
    public void SetBottomOffsetY(float y)
    {
        offsetY = y;
        // LayOutRelation ly = this.gameObject.GetComponent<LayOutRelation>();
        // ly.offset = new Vector2(0,y);
        // ly.LayOut();
        LayOut();
    }

    public override void LayOut()
    {
        base.LayOut();
        float x, y, w, h, oft;
        RectTransform rctran = this.gameObject.GetComponent<RectTransform>();
        RectTransform rctranBg = imageBg.GetComponent<RectTransform>();
        RectTransform rctranIcon = imageIcon.GetComponent<RectTransform>();
        RectTransform rctranAd = imageAd.GetComponent<RectTransform>();

        RectTransform rctranTitle = textTitle.GetComponent<RectTransform>();
        RectTransform rctranDetail = textDetail.GetComponent<RectTransform>();
        //banner 显示在屏幕底部
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        float scale = 1f;
        float ratio = 1f;
        float x_left = 0;
        oft = 16;
        w = rctran.rect.width;
        h = rctran.rect.height;
        int w_screen = (int)Common.CanvasToScreenWidth(sizeCanvas, w);
        int h_screen = (int)Common.CanvasToScreenWidth(sizeCanvas, h);
        Debug.Log("UIAdBanner AdBannerDidReceiveAd::w=" + w_screen + " h=" + h_screen);
        AdKitCommon.main.AdBannerDidReceiveAd(w_screen.ToString() + ":" + h_screen.ToString());


        x = 0;
        y = Common.ScreenToCanvasHeigt(sizeCanvas, Device.heightSystemHomeBar);
        // rctran.anchoredPosition = new Vector2(x, y);
        if (imageBg.texture == null)
        {
            return;
        }

        {
            w = imageBg.texture.width;
            h = imageBg.texture.height;


            float scaley = rctran.rect.height / h;
            float scalex = scaley;
            if (scalex * w > rctran.rect.width)
            {
                scalex = rctran.rect.width / w;
            }
            LayOutRelation ly = this.GetComponent<LayOutRelation>();
            if (ly != null)
            {
                ly.offset = new Vector2(0, offsetY);
            }

            //scalex = scalex/2;
            //scale = Common.GetBestFitScale(w, h, rctran.rect.width, rctran.rect.height) * ratio;
            imageBg.transform.localScale = new Vector3(scalex, scaley, 1.0f);
            w = w * scalex;
            h = h * scaley;
            x_left = rctranBg.anchoredPosition.x - w / 2;


            w = rctran.rect.width;
            h = rctran.rect.height;
            // int w_screen = (int)Common.CanvasToScreenWidth(sizeCanvas, w);
            // int h_screen = (int)Common.CanvasToScreenWidth(sizeCanvas, h);
            // Debug.Log("UIAdBanner AdBannerDidReceiveAd::w=" + w_screen + " h=" + h_screen);
            // AdKitCommon.main.AdBannerDidReceiveAd(w_screen.ToString() + ":" + h_screen.ToString());




            if (imageAd.texture != null)
            {
                w = imageBg.texture.width * imageBg.transform.localScale.x;
                x = rctran.rect.width / 2 - (rctranBg.anchoredPosition.x + w / 2);
                x = -x;
                y = 0;//rctranBg.anchoredPosition.y + h / 2;
                rctranAd.anchoredPosition = new Vector2(x, y);
                w = imageAd.texture.width;//rectTransform.rect.width;
                h = imageAd.texture.height;//rectTransform.rect.height;
                float sz = rctran.rect.height / 3;
                scale = Common.GetBestFitScale(w, h, sz, sz);
                imageAd.transform.localScale = new Vector3(scale, scale, 1.0f);
            }

        }



        {
            w = imageIcon.texture.width;//rectTransform.rect.width;
            h = imageIcon.texture.height;//rectTransform.rect.height;
            scale = Common.GetBestFitScale(w, h, rctran.rect.height, rctran.rect.height) * 0.8f;
            imageIcon.transform.localScale = new Vector3(scale, scale, 1.0f);

            w = w * scale;
            h = h * scale;

            x = x_left + oft + w / 2;
            y = 0;
            rctranIcon.anchoredPosition = new Vector2(x, y);

            x_left = x + w / 2;
        }



        {
            x = x_left + oft;
            y = rctran.rect.height / 2 + 2;
            rctranTitle.anchoredPosition = new Vector2(x, y);
        }

        {
            x = x_left + oft;
            y = -(rctran.rect.height / 2 + 2);
            rctranDetail.anchoredPosition = new Vector2(x, y);
        }




    }

    public void OnClickAd()
    {
        if (Config.main.APP_FOR_KIDS)
        {
            ParentGateViewController.main.Show(null, null);
            ParentGateViewController.main.ui.callbackClose = OnUIParentGateDidClose;
        }
        else
        {
            OnClickAdInternal();
        }
    }

    public void OnClickAdInternal()
    {
        ItemInfo info = listAd[indexAd];
        string url = info.url;
        if (!Common.BlankString(url))
        {
            Application.OpenURL(url);
        }
    }

    public void OnClickBtnClose()
    {
        int w_screen = 0;
        int h_screen = 0;
        AdKitCommon.main.AdBannerDidReceiveAd(w_screen.ToString() + ":" + h_screen.ToString());
    }
    public void StartParseAd()
    {
        HttpRequest http = new HttpRequest(OnHttpRequestFinished);
        http.Get(URL_AD_LIST);
    }

    static public void parserJson(byte[] data, List<ItemInfo> list)
    {
        string str = Encoding.UTF8.GetString(data);
        JsonData root = JsonMapper.ToObject(str);
        JsonData appList = root["app"];
        string key = "";
        for (int i = 0; i < appList.Count; i++)

        {
            ItemInfo info = new ItemInfo();
            JsonData current = appList[i];

            info.pic = (string)current["pic"];
            info.icon = (string)current["icon"];
            key = "detail";
            if (Common.JsonDataContainsKey(current, key))
            {
                info.description = (string)current[key];
            }

            info.title = (string)current["title"];

            JsonData jsonPackage = current["PACKAGE"];
            key = Source.IOS;
            if (Common.isAndroid)
            {
                key = Source.ANDROID;
            }
            info.id = (string)jsonPackage[key];


            JsonData jsonAppId = current["APPID"];
            key = Source.APPSTORE;
            if (Common.isAndroid)
            {
                key = Source.TAPTAP;
            }

            if (Common.JsonDataContainsKey(jsonAppId, key))
            {
                info.appid = (string)jsonAppId[key];
            }


            JsonData jsonUrl = current["URL"];
            key = Source.IOS;
            if (Common.isAndroid)
            {
                key = Source.ANDROID;
            }
            info.url = (string)jsonUrl[key];

            string appname = Common.GetAppName();
            //(GetAppIdCur() != info.appid)
            if (!Common.BlankString(info.url) && (!appname.Contains(info.title)) && (!(Common.GetAppPackage() + ".pad").Contains(info.id)))
            {
                list.Add(info);

            }

        }
    }

    public void OnUIParentGateDidClose(UIParentGate ui, bool isLongPress)
    {
        if (isLongPress)
        {
            OnClickAdInternal();
        }
    }
    void OnHttpRequestFinished(HttpRequest req, bool isSuccess, byte[] data)
    {
        // Debug.Log("UIAdBanner OnHttpRequestFinished isSucces="+isSuccess); 
        if (isSuccess)
        {
            parserJson(data, listAd);
            Debug.Log("UIAdBanner OnHttpRequestFinished listAd Count=" + listAd.Count);
            Invoke("OnUpdateTime", 0);
        }else{
            Debug.Log("UIAdBanner OnHttpRequestFinished Fail");
        }
    }



    void OnHttpRequestFinishedImage(HttpRequest req, bool isSuccess, byte[] data)
    {
        //   Debug.Log("MoreAppParser OnHttpRequestFinished:isSuccess=" + isSuccess);
        //  return;
        if (isSuccess)
        {
            if (!GameViewController.main.isActive)
            {
                // return;
            }
            if (isDestroy)
            {
                return;
            }
            Texture2D tex = LoadTexture.LoadFromData(data);
            RawImage image = null;
            if (httpReqIcon == req)
            {
                float value = (156 * 1f / 1024);
                tex = TextureUtil.RoundRectTexture(tex, value);
                image = imageIcon;
                isDownloadIcon = true;
            }
            if (httpReqBg == req)
            {
                isDownloadBg = true;
                image = imageBg;
            }

            TextureUtil.UpdateRawImageTexture(image, tex, true);
            if ((isDownloadBg) && (isDownloadIcon))
            {
                this.gameObject.SetActive(true);
                LayOut();
            }
        }

    }
}


