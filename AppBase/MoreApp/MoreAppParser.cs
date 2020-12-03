using System.Collections;
using System.Collections.Generic;

using System.Collections.Generic;
using System.Text;
using LitJson;
using UnityEngine;

public delegate void OnMoreAppParserFinishedDelegate(MoreAppParser parser, List<ItemInfo> listApp);

public class MoreAppParser
{
    HttpRequest httpRequest;
    List<ItemInfo> listApp;
    public OnMoreAppParserFinishedDelegate callback { get; set; }


    string GetAppIdCur()
    {
        string strappid = "";

        strappid = Config.main.appId;

        return strappid;
    }


    public void startParserAppList(string url)
    {
        listApp = new List<ItemInfo>();
        HttpRequest http = new HttpRequest(OnHttpRequestFinished);
        http.Get(url);
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
            key = "detail";
            if (Common.JsonDataContainsKey(current, key))
            {
                info.description = (string)current[key];
            }

            info.title = (string)current["title"];
            info.artist = (string)current["title"];

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
                key = Config.main.channel;
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
            bool isAdd = true;
            if (Common.BlankString(info.appid) || Common.BlankString(info.url) || appname.Contains(info.title) || (Common.GetAppPackage() + ".pad").Contains(info.id))
            {
                isAdd = false;
            }
            if (isAdd)
            {
                list.Add(info);

            }

        }
    }

    void parserAppList(byte[] data)
    {
        parserJson(data, listApp);

        if (this.callback != null)
        {
            this.callback(this, listApp);
        }

    }

    void OnHttpRequestFinished(HttpRequest req, bool isSuccess, byte[] data)
    {
        // Debug.Log("MoreAppParser OnHttpRequestFinished"); 
        if (isSuccess)
        {
            parserAppList(data);

        }
        else
        {

        }
    }


    //   void startParserHome(char *url);
    //     void startParserSort(char *url);
    //     
    //     void  parserListHome(char * data);
    //     void  parserListSort(char * data);

    //     void startParserTuiguangErtong(char *url);
    //    // void  parserListTuiguangErtong(char * data);

    //     void startParserTuiguang(char *url);
    //     void  parserListTuiguang(char * data);

}
