using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using LitJson;
using Moonma.Html.HtmlWebView;

public class AppVersionHuawei : AppVersionBase
{
    // http://47.242.56.146:8080/AppVersion_huawei?cur_version=1.2.0&package=com.moonma.caicaile&appid=100270155
    // http://mooncore.cn:8080/AppVersion_huawei?cur_version=1.2.0&package=com.moonma.caicaile&appid=100270155
    public const string URL_HEAD = "https://6c69-lianlianle-shkb3-1259451541.tcb.qcloud.la/AppVersion";
    // public const string URL_HEAD_MoonCore = "http://mooncore.cn:8080/AppVersion_huawei";

    public const string URL_HEAD_MoonCore = "http://47.242.56.146:8080/AppVersion_huawei";

    HtmlWebView htmlWebView;

    static private AppVersionHuawei _main = null;
    public static AppVersionHuawei main
    {
        get
        {
            if (_main == null)
            {
                _main = new AppVersionHuawei();
            }
            return _main;
        }
    }

    //http://appstore.huawei.com/app/C100270155
    //new:https://appgallery1.huawei.com/#/app/C100270155


    //http://mooncore.cn:8080/AppVersion_huawei?cur_version=1.2.0&package=com.moonma.caicaile&appid=100270155 
    public void StartParseVersionOld()
    {
        string strappid = Config.main.GetAppIdOfStore(Source.HUAWEI);
        // strappid = "100136735";
        string url = "https://appgallery1.huawei.com/#/app/C" + strappid;
        Debug.Log("version huawei url=" + url);
        strUrlAppstore = url;
        strUrlComment = url;

        // url = URL_HEAD + "/" + Common.appType + "/" + Common.appKeyName + "/appversion.json";
        url = URL_HEAD_MoonCore + "?cur_version=" + Common.GetAppVersion() + "&package=" + Common.GetAppPackage() + "&appid=" + strappid;
        if (Application.isEditor)
        {
            // url = "http://47.242.56.146:8080/AppVersion_huawei?cur_version=1.2.0&package=com.moonma.hanziyuan&appid=100278849";
        }
        Debug.Log("version huawei url2=" + url);
        HttpRequest http = new HttpRequest(OnHttpRequestFinished);
        http.Get(url);
        // htmlWebView = new HtmlWebView();
        // htmlWebView.SetObjectInfo("Scene", "OnWebViewDidFinish", "OnWebViewDidFail");
        // htmlWebView.Load(url); 

        if (Common.GetAppVersion() == "1.0.0")
        {
            isFirstCreat = true;
        }

    }
    public override async void StartParseVersion()
    {
        // return await StartParseVersionOld();
        // StartParseVersionOld();
        // return;

        string strappid = Config.main.GetAppIdOfStore(Source.HUAWEI);
        // strappid = "100136735";
        if (Application.isEditor)
        {
            // strappid = "100278849";
        }

        string url = "https://appgallery1.huawei.com/#/app/C" + strappid;
        Debug.Log("version huawei url=" + url);
        strUrlAppstore = url;
        strUrlComment = url;

        // url = URL_HEAD + "/" + Common.appType + "/" + Common.appKeyName + "/appversion.json";
        url = URL_HEAD_MoonCore + "?cur_version=" + Common.GetAppVersion() + "&package=" + Common.GetAppPackage() + "&appid=" + strappid;

        Debug.Log("version huawei url2=" + url);
        // HttpRequest http = new HttpRequest(OnHttpRequestFinished);
        // http.Get(url);


        if (Common.GetAppVersion() == "1.0.0")
        {
            isFirstCreat = true;
        }

        strVersionStore = await HuaweiAppGalleryApi.main.GetVersion(strappid);
        Debug.Log("version huawei Task strVersionStore=" + strVersionStore);
        ParseFinished(this);
    }

    public async void GetAppVersionAPI(string appid)
    {
        Debug.Log("Task GetVersion start");
        string ret = await HuaweiAppGalleryApi.main.GetVersion("103066765");
        Debug.Log("Task GetVersion end ret=" + ret);
    }
    //   //儿童连连乐 微信小程序id:wx3e44af039aee1b96   
    //https://6c69-lianlianle-shkb3-1259451541.tcb.qcloud.la/AppVersion/shapecolor/shapecolor/appversion.json
    public void StartParseVersionWeixinJsonFile()
    {
        string strappid = Config.main.GetAppIdOfStore(Source.HUAWEI);
        // strappid = "100270155";
        string url = "https://appgallery1.huawei.com/#/app/C" + strappid;
        Debug.Log("version huawei url=" + url);
        strUrlAppstore = url;
        strUrlComment = url;

        // url = URL_HEAD + "/" + Common.appType + "/" + Common.appKeyName + "/appversion.json";
        url = URL_HEAD + "/" + Application.productName + "/" + Common.appType + "/appversion.json";

        Debug.Log("version huawei url2=" + url);
        HttpRequest http = new HttpRequest(OnHttpRequestFinished);
        http.Get(url);
        // htmlWebView = new HtmlWebView();
        // htmlWebView.SetObjectInfo("Scene", "OnWebViewDidFinish", "OnWebViewDidFail");
        // htmlWebView.Load(url); 

        if (Common.GetAppVersion() == "1.0.0")
        {
            isFirstCreat = true;
        }

    }
    public void OnWebViewDidFinish(string html)
    {
        DoParse(html);
    }
    public void OnWebViewDidFail(string html)
    {
        ParseFinished(this);
    }


    public void DoParse(string html)
    {
        DoParseJson(html);
        return;
        /*
      <li class="ul-li-detail">版本： <span>1.1.1</span>
      </li>

       */

        //<span itemprop="softwareVersion" class="info-item-content">1.0.5</span>
        // string ptmpversion = "softwareVersion"; 
        // <div data-v-684c8842="" class="info_val">2.1.7</div>
        string ptmpversion_start = "版本"; //版本 Version
        string ptmpversion_start_en = "Version"; //版本 Version
        string ptmpversion_mid = "info_val\">";
        string ptmpversion_end = "</div>";


        int idx = html.IndexOf(ptmpversion_start);
        if (idx < 0)
        {
            idx = html.IndexOf(ptmpversion_start_en);
        }

        Debug.Log("version huawei html=" + html);
        if (idx >= 0)
        {
            string ptmp = html.Substring(idx);
            idx = ptmp.IndexOf(ptmpversion_mid);
            if (idx >= 0)
            {
                string p = ptmp.Substring(idx + ptmpversion_mid.Length);
                idx = p.IndexOf(ptmpversion_end);
                if (idx >= 0)

                {
                    string version = p.Substring(0, idx);
                    strVersionStore = version;
                }
            }

        }
        Debug.Log("version huawei =" + strVersionStore);


        ParseFinished(this);
    }
    public override void ParseData(byte[] data)
    {
        //utf8
        string strData = Encoding.UTF8.GetString(data);
        //utf8 bom
        // strData = Encoding.UTF8.GetString(data,3,data.Length-3);
        DoParse(strData);

    }

    public void DoParseJson(string data)
    {
        // { "version": "2.2.2" }

        JsonData jsonRoot = JsonMapper.ToObject(data);
        string key = "";
        {
            key = "update_note";
            if (Common.JsonDataContainsKey(jsonRoot, key))
            {
                JsonData jsonNote = jsonRoot[key];
                strUpdateNote = JsonUtil.GetString(jsonNote, "cn", "update note");
            }
            key = "version";
            // if (Device.isLandscape)
            // {
            //     key = "version_hd";
            // }
            if (Common.JsonDataContainsKey(jsonRoot, key))
            {
                strVersionStore = JsonUtil.GetString(jsonRoot, key, Application.version);
                Debug.Log("version huawei strVersionStore=" + strVersionStore);
            }
        }


        ParseFinished(this);
    }

    public void DoParseJsonWeixinJsonFile(string data)
    {

        // {
        //     if (Common.GetDayIndexOfUse() <= 1)
        //     {
        //           strVersionStore =  Application.version;
        //     }
        //     return;
        // }

        JsonData jsonRoot = JsonMapper.ToObject(data);
        string key = Common.appKeyName;
        if (Common.JsonDataContainsKey(jsonRoot, key))
        {
            JsonData jsonApp = jsonRoot[key];
            key = "update_note";
            if (Common.JsonDataContainsKey(jsonApp, key))
            {
                JsonData jsonNote = jsonApp[key];
                strUpdateNote = JsonUtil.GetString(jsonNote, "cn", "update note");
            }
            key = "version";
            if (Device.isLandscape)
            {
                key = "version_hd";
            }
            if (Common.JsonDataContainsKey(jsonApp, key))
            {
                JsonData jsonVersion = jsonApp[key];
                strVersionStore = JsonUtil.GetString(jsonVersion, "android", Application.version);
                Debug.Log("version huawei strVersionStore=" + strVersionStore);
            }
        }


        ParseFinished(this);
    }
}
