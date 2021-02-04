
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using LitJson;
using System;

public class IAPInfo
{
    public string id;
    public string key;
    public Dictionary<string, string> dicTitle;
    public Dictionary<string, string> dicDetail;
    public bool isConsume;
    public string price_tier;


}
public class IAPConfig
{

    List<IAPInfo> listProduct = new List<IAPInfo>();
    public static string osDefault = Source.ANDROID;

    JsonData rootJson;

    static private IAPConfig _main = null;
    public static IAPConfig main
    {
        get
        {
            if (_main == null)
            {
                _main = new IAPConfig();
                _main.Init();
                _main.ParseJson(AppVersion.appForPad);

            }
            return _main;
        }
    }



    public void Init()
    {

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



    void ParseJson(bool ishd)
    {
        if (rootJson != null)
        {
            return;
        }

        //string strDir = Common.GAME_DATA_DIR + "/config";
        string strDir = Common.RES_CONFIG_DATA + "/IAP";

        string fileName = "IAP_ios";

        //Defualt
        fileName = "IAP_" + osDefault;
        if (osDefault == Source.ANDROID)
        {
            fileName = "IAP_android";
        }
        if (osDefault == Source.IOS)
        {
            fileName = "IAP_ios";
        }
        if (osDefault == Source.WIN)
        {

        }

        if (Common.isiOS)
        {
            fileName = "IAP_ios";
        }
        if (Common.isAndroid)
        {
            fileName = "IAP_android";
        }
        if (Common.isWinUWP)
        {
            fileName = "IAP_" + Source.WIN;
        }

        if (ishd)//AppVersion.appForPad
        {
            fileName += "_hd";
        }
        fileName += ".json";

        string json = FileUtil.ReadStringFromResources(strDir + "/" + fileName);//ReadStringAsset
        rootJson = JsonMapper.ToObject(json);

        //appid
        /*
         "key": "unlocklevel",
                    "isConsume": false,
                    "title": {
                        "cn": "解锁",
                        "en": "unlock"
                    },
                    "detail": {
                        "cn": "解锁所有关卡",
                        "en": "unlock all levels"
                    },
                    "price_tier": "3"
        */
        JsonData jsonitems = rootJson["items"];
        string[] listLan = { Source.Language_cn, Source.Language_en };
        foreach (JsonData item in jsonitems)
        {
            IAPInfo info = new IAPInfo();
            info.key = (string)item["key"];
            info.id = (string)item["id"];
            info.isConsume = (bool)item["isConsume"];
            info.price_tier = (string)item["price_tier"];
            JsonData title = item["title"];
            info.dicTitle = new Dictionary<string, string>();
            foreach (string lan in listLan)
            {
                info.dicTitle.Add(lan, (string)title[lan]);
            }

            JsonData detail = item["detail"];
            info.dicDetail = new Dictionary<string, string>();
            foreach (string lan in listLan)
            {
                info.dicDetail.Add(lan, (string)detail[lan]);
            }
            listProduct.Add(info);

        }



    }


    public string GetProductTitle(string key)
    {
        foreach (IAPInfo info in listProduct)
        {
            if (info.key == key)
            {
                string lan = "";

                if (Language.main.IsChinese())
                {
                    lan = Source.Language_cn;
                }
                else
                {
                    lan = Source.Language_en;

                }
                return info.dicTitle[lan];
            }

        }
        return "";
    }
    public string GetProductDetail(string key)
    {
        foreach (IAPInfo info in listProduct)
        {
            if (info.key == key)
            {
                string lan = "";

                if (Language.main.IsChinese())
                {
                    lan = Source.Language_cn;
                }
                else
                {
                    lan = Source.Language_en;

                }
                return info.dicDetail[lan];
            }

        }
        return "";
    }
    public string GetIdByKey(string key)
    {
        foreach (IAPInfo info in listProduct)
        {
            if (info.key == key)
            {
                return info.id;
            }

        }
        return "";
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



}
