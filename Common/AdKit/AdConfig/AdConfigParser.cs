using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.Text;
using Moonma.AdKit.AdConfig;

public delegate void OnAdConfigFinishedDelegate(AdConfigParser ad);
public class AdInfo
{
    public string source;
    public string appid;
    public string appkey;
    public string key_splash;
    public string key_splash_insert;
    public string key_banner;
    public string key_insert;
    public string key_native;
    public string key_video;
    public string key_insert_video;
}

public class AdConfigParser
{
    public const int SOURCE_TYPE_SPLASH = 0;
    public const int SOURCE_TYPE_BANNER = 1;
    public const int SOURCE_TYPE_INSERT = 2;
    public const int SOURCE_TYPE_SPLASH_INSERT = 3;//开机插屏
    public const int SOURCE_TYPE_NATIVE = 4;
    public const int SOURCE_TYPE_VIDEO = 5;
    public const int SOURCE_TYPE_INSERT_VIDEO = 6;
    public const string COUNTRY_CN = "cn";
    public const string COUNTRY_OTHER = "other";
    public const string SPLASH_TYPE_SPLASH = "splash";
    public const string SPLASH_TYPE_INSERT = "splash_insert";

    static public string adSourceSplash = Source.ADMOB;
    static public string adSourceSplashInsert = Source.ADMOB;
    static public string adSourceInsert = Source.ADMOB;
    static public string adSourceBanner = Source.ADMOB;
    static public string adSourceNative = Source.GDT;
    static public string adSourceVideo = Source.UNITY;

    public OnAdConfigFinishedDelegate callback { get; set; }

    public List<AdInfo> listPlatform;
    List<AdInfo> listPriorityBanner;
    List<AdInfo> listPriorityInsert;
    List<AdInfo> listPrioritySplash;
    List<AdInfo> listPrioritySplashInsert;
    List<AdInfo> listPriorityVideo;
    List<AdInfo> listPriorityNative;

    JsonData rootJsonCommon;
    JsonData rootJsonPriority;
    int indexPriorityBanner = 0;
    int indexPriorityInsert = 0;
    int indexPrioritySplash = 0;
    int indexPrioritySplashInsert = 0;
    int indexPriorityVideo = 0;
    int indexPriorityNative = 0;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    AdInfo GetAdInfo(string source)
    {
        if (listPlatform == null)
        {
            return null;
        }
        foreach (AdInfo info in listPlatform)
        {
            if (info.source == source)
            {
                return info;
            }
        }

        return null;
    }
    bool IsInChina()
    {
        bool ret = IPInfo.isInChina;
        if (Common.isAndroid)
        {
            if (AppVersion.appCheckForXiaomi)
            {
                //xiaomi 审核中,广告用国外的 admob
                // ret = false;
            }
            ret = true;
        }

        return ret;
    }
    public string GetAdSource(int type)
    {
        string src = AdConfigParser.adSourceBanner;
        switch (type)
        {
            case SOURCE_TYPE_SPLASH:
                src = AdConfigParser.adSourceSplash;

                break;
            case SOURCE_TYPE_BANNER:
                src = AdConfigParser.adSourceBanner;

                break;
            case SOURCE_TYPE_INSERT:
                src = AdConfigParser.adSourceInsert;
                break;
            case SOURCE_TYPE_SPLASH_INSERT:
                src = AdConfigParser.adSourceSplashInsert;
                break;
            case SOURCE_TYPE_NATIVE:
                src = AdConfigParser.adSourceNative;
                break;
            case SOURCE_TYPE_VIDEO:
                src = AdConfigParser.adSourceVideo;
                break;
        }

        if (Config.main.channel == Source.INMOB)
        {
            src = Source.INMOB;
        }
        return src;
    }
    public string GetAppId(string source)
    {
        string ret = "0";
        AdInfo info = GetAdInfo(source);
        if (info != null)
        {
            ret = info.appid;
        }
        return ret;
    }

    public string GetAdKey(string source, int type)
    {
        string ret = "0";
        AdInfo info = GetAdInfo(source);
        if (info != null)
        {
            switch (type)
            {
                case SOURCE_TYPE_SPLASH:
                    ret = info.key_splash;
                    break;
                case SOURCE_TYPE_BANNER:
                    ret = info.key_banner;
                    break;
                case SOURCE_TYPE_INSERT:
                    ret = info.key_insert;
                    break;
                case SOURCE_TYPE_SPLASH_INSERT:
                    ret = info.key_splash_insert;
                    break;
                case SOURCE_TYPE_NATIVE:
                    ret = info.key_native;
                    break;

                case SOURCE_TYPE_VIDEO:
                    ret = info.key_video;
                    break;
                case SOURCE_TYPE_INSERT_VIDEO:
                    ret = info.key_insert_video;
                    break;
            }
        }
        return ret;
    }

    public List<AdInfo> GetListPriority(int type)
    {
        List<AdInfo> listPriority = null;
        switch (type)
        {
            case SOURCE_TYPE_SPLASH:
                listPriority = listPrioritySplash;
                break;
            case SOURCE_TYPE_BANNER:
                listPriority = listPriorityBanner;
                break;
            case SOURCE_TYPE_INSERT:
                listPriority = listPriorityInsert;
                break;
            case SOURCE_TYPE_SPLASH_INSERT:
                listPriority = listPrioritySplashInsert;
                break;
            case SOURCE_TYPE_NATIVE:
                listPriority = listPriorityNative;
                break;
            case SOURCE_TYPE_VIDEO:
                listPriority = listPriorityVideo;
                break;
        }
        return listPriority;
    }

    public void InitPriority(string src, int type)
    {
        int idx = 0;
        List<AdInfo> listPriority = GetListPriority(type);
        foreach (AdInfo info in listPriority)
        {
            if (info.source == src)
            {
                switch (type)
                {
                    case SOURCE_TYPE_SPLASH:
                        indexPrioritySplash = idx;
                        break;
                    case SOURCE_TYPE_BANNER:
                        indexPriorityBanner = idx;
                        break;
                    case SOURCE_TYPE_INSERT:
                        indexPriorityInsert = idx;
                        break;
                    case SOURCE_TYPE_SPLASH_INSERT:
                        indexPrioritySplashInsert = idx;
                        break;
                    case SOURCE_TYPE_NATIVE:
                        indexPriorityNative = idx;
                        break;
                    case SOURCE_TYPE_VIDEO:
                        indexPriorityVideo = idx;
                        break;
                }
                break;
            }
            idx++;
        }
    }

    public AdInfo GetNextPriority(int type)
    {
        int idx = 0;
        switch (type)
        {
            case SOURCE_TYPE_SPLASH:
                idx = ++indexPrioritySplash;
                break;
            case SOURCE_TYPE_BANNER:
                idx = ++indexPriorityBanner;
                break;
            case SOURCE_TYPE_INSERT:
                idx = ++indexPriorityInsert;
                break;
            case SOURCE_TYPE_SPLASH_INSERT:
                idx = ++indexPrioritySplashInsert;
                break;
            case SOURCE_TYPE_NATIVE:
                idx = ++indexPriorityNative;
                break;
            case SOURCE_TYPE_VIDEO:
                idx = ++indexPriorityVideo;
                break;
        }
        List<AdInfo> listPriority = GetListPriority(type);
        Debug.Log("GetNextPriority:listPriority.Count=" + listPriority.Count + " type=" + type + " idx=" + idx);
        if (idx >= listPriority.Count)
        {
            return null;
        }
        AdInfo info = listPriority[idx];
        return info;

    }

    public void StartParseConfig(string url)
    {
        Debug.Log("StartParseConfig:" + url);

        listPlatform = new List<AdInfo>();
        //直接从本地读取
        //OnHttpRequestFinished(null, false, null);
        ParseJsonDataApp();

        //common 在后面
        ParseJsonDataCommon();
        ParseJsonPriority();

        // HttpRequest http = new HttpRequest(OnHttpRequestFinished);
        // http.Get(url);



    }

    string GetJsonKey(JsonData data, string key)
    {
        string ret = "0";
        if (Common.JsonDataContainsKey(data, key))
        {
            ret = (string)data[key];
        }
        return ret;
    }

    bool IsInPlatformList(string src)
    {
        bool ret = false;
        foreach (AdInfo info in listPlatform)
        {
            if (info.source == src)
            {
                ret = true;
                break;
            }
        }
        return ret;
    }
    void ParsePlatformData(JsonData rootData)
    {
        string key = "platform";
        if (!Common.JsonDataContainsKey(rootData, key))
        {
            return;
        }
        JsonData jsonItems = rootData[key];

        for (int i = 0; i < jsonItems.Count; i++)

        {
            AdInfo info = new AdInfo();
            JsonData current = jsonItems[i];
            info.source = (string)current["source"];
            if (IsInPlatformList(info.source))
            {
                continue;
            }

            info.appid = (string)current["appid"];

            info.appkey = GetJsonKey(current, "appkey");
            info.key_splash = GetJsonKey(current, "key_splash");
            info.key_splash_insert = GetJsonKey(current, "key_splash_insert");
            info.key_banner = (string)current["key_banner"];
            info.key_insert = (string)current["key_insert"];
            info.key_native = GetJsonKey(current, "key_native");
            info.key_video = GetJsonKey(current, "key_video");
            info.key_insert_video = GetJsonKey(current, "key_insert_video");
            listPlatform.Add(info);

        }
    }
    void ParseData(byte[] data)
    {
        string str = Encoding.UTF8.GetString(data);
        JsonData root = JsonMapper.ToObject(str);
        ParsePlatformData(root);
        if (Config.main.channel == Source.XIAOMI)
        {
            //小米 系统开屏 广告单价高 不再开启应用开屏广告
            //AdConfig.SetEnableAdSplash(false);
        }
        OnConfiFinish();
    }


    void ParseJsonDataApp()
    {
        string filename = "ad_config_ios.json";
        if (AppVersion.appForPad)
        {
            filename = "ad_config_ios_hd.json";
        }
        if (Common.isAndroid)
        {
            filename = "ad_config_android.json";
            if (AppVersion.appForPad)
            {
                filename = "ad_config_android_hd.json";
            }
        }
        if (Common.isWinUWP)
        {
            filename = "ad_config_win.json";
            if (AppVersion.appForPad)
            {
                filename = "ad_config_win_hd.json";
            }
        }

        // string filepath = Common.RES_CONFIG_DATA + "/adconfig/" + filename;
        // byte[] datatmp = FileUtil.ReadDataFromResources(filepath);
        string filepath = Common.GAME_DATA_DIR + "/adconfig/" + filename;
        byte[] datatmp = FileUtil.ReadDataAuto(filepath);
        ParseData(datatmp);
    }

    void ParseJsonDataCommon()
    {
        string filename = "ad_config_common_ios.json";

        if (Common.isAndroid)
        {
            filename = "ad_config_common_android.json";
        }
        if (Common.isWinUWP)
        {
            filename = "ad_config_common_win.json";
        }
        string key = COUNTRY_CN;
        if (IsInChina())
        {
            key = COUNTRY_CN;
        }
        else
        {
            key = COUNTRY_OTHER;
        }

        string filepath = Common.RES_CONFIG_DATA_COMMON + "/adconfig/" + key + "/" + filename;

        byte[] data = FileUtil.ReadDataFromResources(filepath);

        string str = Encoding.UTF8.GetString(data);
        JsonData root = JsonMapper.ToObject(str);

        JsonData ad = root["ad_source"];
        string source_splash = (string)ad["source_splash"];
        string source_splash_insert = (string)ad["source_splash_insert"];
        string source_insert = (string)ad["source_insert"];
        string source_banner = (string)ad["source_banner"];


        AdConfigParser.adSourceSplash = source_splash;
        AdConfigParser.adSourceInsert = source_insert;
        AdConfigParser.adSourceBanner = source_banner;
        AdConfigParser.adSourceSplashInsert = source_splash_insert;

        if (Common.JsonDataContainsKey(ad, "source_native"))
        {
            AdConfigParser.adSourceNative = (string)ad["source_native"];
        }

        if (Common.JsonDataContainsKey(ad, "source_video"))
        {
            AdConfigParser.adSourceVideo = (string)ad["source_video"];
        }


        ParsePlatformData(root);
    }

    void ParsePriorityItem(List<AdInfo> ls, string key, JsonData json)
    {
        JsonData jsonItems = json[key];
        for (int i = 0; i < jsonItems.Count; i++)
        {
            AdInfo info = new AdInfo();
            JsonData current = jsonItems[i];
            info.source = (string)current["source"];
            ls.Add(info);
        }
    }
    void ParseJsonPriority()
    {
        string filename = "ad_config_priority_ios.json";

        if (Common.isAndroid)
        {
            filename = "ad_config_priority_android.json";
        }
        string key = COUNTRY_CN;
        if (IsInChina())
        {
            key = COUNTRY_CN;
        }
        else
        {
            key = COUNTRY_OTHER;
        }
        string filepath = Common.RES_CONFIG_DATA_COMMON + "/adconfig/" + key + "/" + filename;
        byte[] data = FileUtil.ReadDataFromResources(filepath);

        string str = Encoding.UTF8.GetString(data);
        JsonData root = JsonMapper.ToObject(str);

        listPriorityBanner = new List<AdInfo>();
        ParsePriorityItem(listPriorityBanner, "priority_banner", root);

        listPriorityInsert = new List<AdInfo>();
        ParsePriorityItem(listPriorityInsert, "priority_insert", root);

        listPrioritySplash = new List<AdInfo>();
        ParsePriorityItem(listPrioritySplash, "priority_splash", root);
        listPrioritySplashInsert = new List<AdInfo>();
        ParsePriorityItem(listPrioritySplashInsert, "priority_splash_insert", root);
        listPriorityVideo = new List<AdInfo>();
        ParsePriorityItem(listPriorityVideo, "priority_video", root);
        listPriorityNative = new List<AdInfo>();
        ParsePriorityItem(listPriorityNative, "priority_native", root);



    }

    void OnConfiFinish()
    {

        //
        // {
        //     int ty = AdConfigParser.SOURCE_TYPE_SPLASH;
        //     string source = this.GetAdSource(ty);
        //     string appid = this.GetAppId(source);
        //     string key = this.GetAdKey(source, ty);
        //     string type = AdConfigParser.SPLASH_TYPE_SPLASH;
        //     AdConfig.main.AdSplashSetConfig(source, appid, key, type);
        // }
        // {
        //     int ty = AdConfigParser.SOURCE_TYPE_SPLASH_INSERT;
        //     string source = this.GetAdSource(ty);
        //     string appid = this.GetAppId(source);
        //     string key = this.GetAdKey(source, ty);
        //     string type = AdConfigParser.SPLASH_TYPE_INSERT;
        //     AdConfig.main.AdSplashInsertSetConfig(source, appid, key, type);
        // }


        if (this.callback != null)
        {
            this.callback(this);
        }
    }

    void OnHttpRequestFinished(HttpRequest req, bool isSuccess, byte[] data)
    {
        // Debug.Log("MoreAppParser OnHttpRequestFinished"); 
        if (isSuccess)
        {
            Debug.Log("StartParseConfig:OnHttpRequestFinished Success");
            ParseData(data);

        }
        else
        {

            ParseJsonDataApp();
        }

    }
}
