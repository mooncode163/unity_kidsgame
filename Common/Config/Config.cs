
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using LitJson;
using Moonma.Share;

public class Config
{
    // private static Plist plist;

    // private static Plist plistAppName;
    private static Config instanceAppName;

    public static string osDefault = Source.ANDROID;
    public List<SharePlatformInfo> listSharePlatform;

    public List<ItemInfo> listAppStore;

    JsonData rootJson;
    JsonData rootJsonCommon;
    JsonData rootJsonAppname;
    JsonData rootJsonChannel;
    JsonData jsonShare;
    JsonData jsonPay;
    static private Config _main = null;
    public static Config main
    {
        get
        {
            if (_main == null)
            {
                _main = new Config();
                _main.Init();
                _main.ParseJsonCommon();
                _main.ParseJsonChannel();
                _main.ParseJson(AppVersion.appForPad);

            }
            return _main;
        }
    }


    public string GetAppNameJson(bool ishd)
    {
        ParseJsonAppname(ishd);
        string appname = Common.appType + "_" + Common.appKeyName; ;
        //appname json
        if (rootJsonAppname != null)
        {
            //APP_NAME_CN_IOS
            string lan = "CN";
            if (Language.main.GetLanguage() != SystemLanguage.Chinese)
            {
                lan = "EN";
            }

            string os = osDefault;
            if (Common.isAndroid)
            {
                os = Source.ANDROID;
            }
            if (Common.isiOS)
            {
                os = Source.IOS;
            }

            string key = "APP_NAME_" + lan + "_" + os.ToUpper();
            if (Common.JsonDataContainsKey(rootJsonAppname, key))
            {
                appname = (string)rootJsonAppname[key];
            }
            else
            {


            }
        }

        return appname;

    }

    public int NO_AD_DAY
    {
        get
        {
            int ret = 1;
            string str = GetStringCommon("NO_AD_DAY", "0");
            if (!Common.BlankString(str))
            {
                ret = Common.String2Int(str);
            }
            if ((channel == Source.TAPTAP) || (channel == Source.GP))
            {
                //TapTap直接显示广告
                ret = 0;
            }


            if (channel == Source.XIAOMI)
            {
                //xiaomi 审核通过
                if (AppVersion.appCheckHasFinished)
                {

                    ret = 0;
                }
            }
            if (channel == Source.GP)
            {
                //GP 审核通过
                if (AppVersion.appCheckHasFinished)
                {
                    ret = 0;
                }
            }

            if (channel == Source.HUAWEI)
            {
                //huawei 审核通过
                if (AppVersion.appCheckHasFinished)
                {
                    ret = 0;
                }
                else
                {
                    //ret = 49;
                }
            }



            return ret;
        }
    }


    public bool APP_FOR_KIDS
    {
        get
        {
            string key = "APP_FOR_KIDS";
            bool ret = GetBoolKeyCommon(key, true);
            return ret;
        }
    }
    public bool Is3D
    {
        get
        {
            string key = "3D";
            bool ret = GetBoolKeyCommon(key, false);
            return ret;
        }
    }

    public string urlGameRes
    {
        get
        {
            JsonData data = JsonUtil.GetJsonData(rootJsonCommon, "CloudRes");
            return JsonUtil.GetString(data, "GameRes", "");
        }
    }
    public string urlVersionGameRes
    {
        get
        {
            JsonData data = JsonUtil.GetJsonData(rootJsonCommon, "CloudRes");
            // if (data == null)
            // {
            //     Debug.Log("CloudRes Config.main.urlVersionGameRes= data == null");
            // }
            return JsonUtil.GetString(data, "version", "");
        }
    }

    public bool isHaveShare
    {
        get
        {
            string key = "HAVE_SHARE";
            bool ret = GetBoolJson(jsonShare, key);
            return ret;
        }
    }
    public bool isHaveIAP
    {
        get
        {
            string key = "HAVE_IAP";
            bool ret = GetBoolKeyCommon(key, false);

            if (Common.isAndroid)
            {
                ret = false;
            }
            if (Common.isWinUWP)
            {
                ret = false;
            }
            return ret;
        }
    }

    public bool isHaveRemoveAd
    {
        get
        {
            bool ret = true;
            if (Common.isAndroid)
            {
                ret = false;
                if (Config.main.channel == Source.GP)
                {
                    //GP市场内购
                    ret = false;//true
                }
            }
            if (Common.isWinUWP)
            {
                ret = false;
            }
            if (Common.isiOS)
            {
                if (Config.main.isNoIDFASDK)
                {
                    ret = false;
                }
            }
            return ret;
        }
    }

    public string sourceIAP
    {
        get
        {
            string ret = Source.XIAOMI;
            if (Common.isiOS)
            {
                ret = Source.APPSTORE;
            }
            if (Common.isAndroid)
            {
                ret = Source.XIAOMI;
                if (!IPInfo.isInChina)
                {
                    ret = Source.XIAOMI;
                }

                if (Config.main.channel == Source.GP)
                {
                    //GP市场内购
                    ret = Source.GP;
                }

            }
            return ret;
        }
    }


    public string IDNoadIAP
    {
        get
        {
            return GetString("ID_NOAD_IAP", "");
        }
    }

    public string PrivacyPolicy
    {
        get
        {
            if (Language.main.IsChinese())
            {
                return GetString("PrivacyPolicy", "PrivacyPolicy_chyfemail163@163.com.txt");
            }
            else
            {
                return GetString("PrivacyPolicy_en", "PrivacyPolicy_chyfemail163@163.com_en.txt");

            }


        }
    }


    public bool isHaveShop
    {
        get
        {
            if (!isHaveIAP)
            {
                if (!AppVersion.appCheckHasFinished)
                {
                    return false;
                }
            }
            string key = "HAVE_SHOP";
            bool ret = GetBoolKeyCommon(key, false);
            return ret;
        }
    }

    public bool isiOS
    {
        get
        {
            bool ret = false;
#if UNITY_IOS && !UNITY_EDITOR
        ret = true;
#endif

            return ret;
        }
    }


    // ios IDFA sdk 身份识别
    public bool isNoIDFASDK
    {
        get
        {
            // if (!isiOS)
            // {
            //     return false;
            // }
            string key = "NoIDFASDK";
            bool ret = GetBoolKeyCommon(key, false);
            return ret;
        }
    }

    public bool isHaveIntroduce
    {
        get
        {
            if (Common.isAndroid)
            {
                return false;
            }
            if (AppVersion.appCheckHasFinished)
            {
                return false;
            }
            string key = "HAVE_INTRODUCE";
            bool ret = GetBoolKey(key, false);
            return ret;
        }
    }

    public string appId
    {
        get
        {
            string key_store = Source.APPSTORE;
            if (Common.isAndroid)
            {
                key_store = channel;
            }
            string strid = GetAppIdOfStore(key_store);
            return strid;
        }
    }

    public string channel
    {
        get
        {
            string ret = Source.XIAOMI;
            if (Common.isiOS)
            {
                ret = Source.APPSTORE;
            }
            if (Common.isAndroid)
            {
                ret = GetStringJson(rootJsonChannel, "channel_android", Source.XIAOMI);
            }
            if (Common.isWeb)
            {
                ret = Source.FACEBOOK;
            }
            return ret;
        }
    }

    public string shareAppUrl
    {
        get
        {
            //jsonShare
            string str = (string)rootJsonCommon["SHARE_APP_URL"];
            return str;
        }
    }
    public string iapAppKeyGoogle
    {
        get
        {
            //jsonShare
            string str = GetString("IAP_APP_KEY_GOOGLE", "0");
            return str;
        }
    }

    public void Init()
    {
        listAppStore = new List<ItemInfo>();
    }

    //重新解析
    public void ReParseJson(bool ishd)
    {
        //重新解析
        rootJson = null;
        ParseJson(ishd);
    }
    public string GetAppStoreAcount(string store)
    {
        string key = "appstore_acount";
        string acount_default = "chyfemail163@163.com";
        bool ishave = Common.JsonDataContainsKey(rootJsonCommon, key);
        if (!ishave)
        {
            return acount_default;
        }
        JsonData json = rootJsonCommon[key];
        return JsonUtil.GetString(json, store, acount_default);


    }
    public string GetAppIdOfStore(string store)
    {
        JsonData jsonAppId = rootJson["APPID"];
        string strid = "0";
        if (Common.JsonDataContainsKey(jsonAppId, store))
        {
            strid = (string)jsonAppId[store];
        }

        return strid;
    }

    public string GetStringCommon(string key, string def)
    {
        return GetStringJson(rootJsonCommon, key, def);
    }
    public string GetString(string key, string def)
    {
        return GetStringJson(rootJson, key, def);
    }

    public bool IsHaveKey(string key)
    {
        bool ret = false;
        if (Common.JsonDataContainsKey(rootJson, key))
        {
            ret = true;
        }
        return ret;

    }
    public bool GetBoolKey(string key, bool v)
    {
        bool ishave = IsHaveKey(key);
        if (!ishave)
        {
            return v;
        }
        bool ret = GetBoolJson(rootJson, key);
        return ret;
    }

    public bool GetBoolKeyCommon(string key, bool v)
    {
        bool ishave = Common.JsonDataContainsKey(rootJsonCommon, key);

        if (!ishave)
        {
            return v;
        }
        bool ret = GetBoolJson(rootJsonCommon, key);
        return ret;
    }

    // public string GetStringAppName(string key)
    // {

    //     string str = plistAppName.GetString(key);
    //     return str;

    // }


    void ParseJsonCommon()
    {
        if (rootJsonCommon != null)
        {
            return;
        }

        //string strDir = Common.GAME_DATA_DIR + "/config";
        string strDir = Common.RES_CONFIG_DATA + "/config";
        string fileName = "config_common";
        fileName += ".json";

        string json = FileUtil.ReadStringFromResources(strDir + "/" + fileName);
        rootJsonCommon = JsonMapper.ToObject(json);
    }

    void ParseJsonChannel()
    {
        if (Common.isWeb)
        {
            return;
        }
        if (rootJsonChannel != null)
        {
            return;
        }

        string filePath = Common.GAME_DATA_DIR + "/common/channel.json";
        string json = FileUtil.ReadStringAsset(filePath);
        rootJsonChannel = JsonMapper.ToObject(json);
    }

    void ParseJsonAppname(bool isHd)
    {
        if (rootJsonAppname != null)
        {
            // return;
        }

        string strDir = AppsConfig.ROOT_DIR_PC + "/ProjectConfig/" + Common.appType + "/" + Common.appKeyName + "/appname";
        string filePath = strDir + "/appname.json";
        //if (Device.isLandscape)
        if (isHd)
        {
            filePath = strDir + "/appname_hd.json";
        }
        if (FileUtil.FileIsExist(filePath))
        {
            string json = FileUtil.ReadStringFromFile(filePath);
            rootJsonAppname = JsonMapper.ToObject(json);
        }
    }

    void ParseJson(bool ishd)
    {
        if (rootJson != null)
        {
            return;
        }

        //string strDir = Common.GAME_DATA_DIR + "/config";
        string strDir = Common.RES_CONFIG_DATA + "/config";

        string fileName = "config_ios";

        //Defualt
        fileName = "config_" + osDefault;
        if (osDefault == Source.ANDROID)
        {
            fileName = "config_android";
        }
        if (osDefault == Source.IOS)
        {
            fileName = "config_ios";
        }
        if (osDefault == Source.WIN)
        {

        }

        if (Common.isiOS)
        {
            fileName = "config_ios";
        }
        if (Common.isAndroid)
        {
            fileName = "config_android";
        }
        if (Common.isWinUWP)
        {
            fileName = "config_" + Source.WIN;
        }

        if (ishd)//AppVersion.appForPad
        {
            fileName += "_hd";
        }
        fileName += ".json";

        string json = FileUtil.ReadStringFromResources(strDir + "/" + fileName);//ReadStringAsset
        rootJson = JsonMapper.ToObject(json);

        //appid

        JsonData jsonAppId = rootJson["APPID"];
        foreach (string key in jsonAppId.Keys)
        {
            string value = (string)jsonAppId[key];
            Debug.Log("APPID:key=" + key + " value=" + value);
            ItemInfo iteminfo = new ItemInfo();
            iteminfo.source = key;
            iteminfo.appid = value;
            listAppStore.Add(iteminfo);

        }






        jsonShare = rootJson["SHARE"];
        jsonPay = rootJson["PAY"];

        if (listSharePlatform == null)
        {
            listSharePlatform = new List<SharePlatformInfo>();
        }

        JsonData jsonPlatform = jsonShare["platform"];
        foreach (JsonData data in jsonPlatform)
        {
            SharePlatformInfo info = new SharePlatformInfo();
            info.source = (string)data["source"];
            info.appId = (string)data["id"];
            info.appKey = (string)data["key"];
            if (info.appId == "0")
            {
                continue;
            }
            listSharePlatform.Add(info);
            if (info.source == Source.WEIXIN)
            {
                //同时添加朋友圈
                AddShareBrother(Source.WEIXINFRIEND, info.appId, info.appKey);
            }

            if (info.source == Source.QQ)
            {
                //同时添加qq空间
                AddShareBrother(Source.QQZONE, info.appId, info.appKey);
            }

        }

        //统一添加email和短信
        AddShareBrother(Source.EMAIL, "0", "0");
        AddShareBrother(Source.SMS, "0", "0");
    }
    void AddShareBrother(string source, string appid, string appkey)
    {
        SharePlatformInfo info = new SharePlatformInfo();
        info.source = source;
        info.appId = appid;
        info.appKey = appkey;
        listSharePlatform.Add(info);
    }
    public string GetShareAppId(string source)
    {
        return GetShareIdorKey(source, "id");
    }
    public string GetShareAppKey(string source)
    {
        return GetShareIdorKey(source, "key");
    }
    string GetShareIdorKey(string source, string key)
    {
        string str = "0";
        if (Common.JsonDataContainsKey(jsonShare, source))
        {
            JsonData json = jsonShare[source];
            str = (string)json[key];
        }
        else
        {
            foreach (SharePlatformInfo info in listSharePlatform)
            {
                if (source == info.source)
                {
                    if (key == "id")
                    {
                        str = info.appId;
                    }
                    else
                    {
                        str = info.appKey;
                    }

                    break;
                }
            }
        }


        return str;
    }


    public string GetPayAppId(string source)
    {
        return GetPayIdorKey(source, "id");
    }
    public string GetPayAppKey(string source)
    {
        return GetPayIdorKey(source, "key");
    }
    string GetPayIdorKey(string source, string key)
    {
        string str = "";
        if (Common.JsonDataContainsKey(jsonPay, source))
        {
            JsonData json = jsonPay[source];
            str = (string)json[key];
        }

        return str;
    }

    string GetStringJson(JsonData json, string key, string def)
    {

        string str = def;
        if (Common.JsonDataContainsKey(json, key))
        {
            str = (string)json[key];
        }

        return str;

    }


    public bool GetBoolJson(JsonData json, string key)
    {

        bool ret = false;
        if (Common.JsonDataContainsKey(json, key))
        {
            ret = (bool)json[key];
        }
        return ret;

    }


    public string GetIAPProduct(string product)
    {
        JsonData jsonIAP = rootJsonCommon["IAP"];
        string str = "";
        if (Common.JsonDataContainsKey(jsonIAP, product))
        {
            str = (string)jsonIAP[product];
        }

        return str;
    }

}
