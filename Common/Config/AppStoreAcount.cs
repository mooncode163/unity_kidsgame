
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class AppStoreAcount
{
    public const string Type_password = "password";
    public const string Type_ClientId = "ClientId";
    public const string Type_ClientSecret = "ClientSecret";
    public const string Type_API_KEY_ID = "API_KEY_ID";
    public const string Type_API_USER_ID = "API_USER_ID";
    public const string Type_teamID = "teamID";
    public const string Type_CertificateID = "CertificateID";


    JsonData rootJson;
    static private AppStoreAcount _main = null;
    public static AppStoreAcount main
    {
        get
        {
            if (_main == null)
            {
                _main = new AppStoreAcount();
                string filePath = Common.RES_CONFIG_DATA_COMMON + "/appstore/AppStoreAcount.json";
                _main.Init(filePath);
            }
            return _main;
        }
    }

    void Init(string filePath)
    {
        string json = FileUtil.ReadStringAuto(filePath);
        rootJson = JsonMapper.ToObject(json);
    }

    public string GetAcountInfo(string store, string acount, string type)
    {
        string ret = "";
        if (JsonUtil.ContainsKey(rootJson, store))
        {
            JsonData list = rootJson[store];
            for (int i = 0; i < list.Count; i++)
            {
                JsonData item = list[i];
                if ((string)item["name"] == acount)
                {
                    ret = (string)item[type];
                    break;
                }
            }

        }
        return ret;
    }


}
