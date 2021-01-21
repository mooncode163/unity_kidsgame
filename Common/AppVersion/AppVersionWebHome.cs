using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using LitJson;

// 官网更新
public class AppVersionWebHome : AppVersionBase
{

    //https://www.taptap.com/app/46445
    public override void StartParseVersion()
    {
        string strappid = Config.main.appId;
// http://mooncore.cn:8080/AppVersion?package=com.moonma.caicaile
        string url = "http://mooncore.cn:8080/AppVersion?package=" + Common.GetAppPackage();
        strUrlAppstore = url;
        strUrlComment = url;
        HttpRequest http = new HttpRequest(OnHttpRequestFinished);
        http.Get(url);
    }
 public override void ParseData(byte[] data)
    {
        //  // { "version": "2.2.2" }
        string str = Encoding.UTF8.GetString(data);
        Debug.Log(str);
        JsonData jsonRoot = JsonMapper.ToObject(str); 
        string version = (string)jsonRoot["version"];
        strVersionStore = version;

        ParseFinished(this);
    }
     
}
