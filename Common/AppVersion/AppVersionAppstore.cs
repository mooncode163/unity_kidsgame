using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using LitJson;
public class AppVersionAppstore : AppVersionBase
{
    public override void StartParseVersion()
    {
        //ios
        Debug.Log("AppVersion StartParseVersion");
        string appver = Application.version;
        if (appver.Equals("1.0.0") || appver.Equals("1.0"))
        {
            isFirstCreat = true;
        }


        //appCheckHasFinished = Common.Int2Bool(PlayerPrefs.GetInt(STRING_KEY_APP_CHECK_FINISHED));
        //Debug.Log("AppVersion new HttpRequest");
        HttpRequest http = new HttpRequest(OnHttpRequestFinished);
        //Debug.Log("AppVersion new HttpRequest end");
        // http.text = textTest;
        string strappid = Config.main.appId;
        strUrlComment = "https://itunes.apple.com/cn/app/id" + Config.main.appId;
        //http://itunes.apple.com/lookup?id=914391781
        //Debug.Log("AppVersion  HttpRequest Get");
        Debug.Log(strappid);
        //http://itunes.apple.com/cn/lookup?id=914391781
        http.Get("https://itunes.apple.com/cn/lookup?id=" + strappid);
    }

    public override void ParseData(byte[] data)
    {
        string str = Encoding.UTF8.GetString(data);
        Debug.Log(str);
        JsonData jsonRoot = JsonMapper.ToObject(str);
        JsonData jsonResult = jsonRoot["results"];
        //Debug.Log("Appversion ParseAppstore 0");
        if (jsonResult.Count == 0)
        {
            Debug.Log("Appversion ParseAppstore 00");
            if (isFirstCreat)
            {
                appCheckForAppstore = true;
            }
            ParseFinished(this);
            return;
        }
        //Debug.Log("Appversion ParseAppstore 1");
        JsonData jsonItem = jsonResult[0];
        string url = (string)jsonItem["trackViewUrl"];
        //Debug.Log("Appversion ParseAppstore 2");
        strUrlAppstore = url;

        //us:https://itunes.apple.com/us/app/id1104615904
        //cn:https://itunes.apple.com/cn/app/id1104615904
        strUrlComment = url;
        if (IPInfo.isInChina)
        {
            strUrlComment = "https://itunes.apple.com/cn/app/id" + Config.main.appId;
        }


        string key_releaseNotes = "releaseNotes";
        if (Common.JsonDataContainsKey(jsonItem, key_releaseNotes))
        {
            strUpdateNote = (string)jsonItem[key_releaseNotes];
        }

        //Debug.Log("Appversion ParseAppstore 4");
        string version = (string)jsonItem["version"];
        strVersionStore = version;

        ParseFinished(this);
    }



}
