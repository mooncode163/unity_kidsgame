using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using LitJson;

public class AppVersionXiaomi : AppVersionBase
{
    public override void StartParseVersion()

    {
        //xiaomi APPID_XIAOMI

        string strappid = Config.main.appId;

        int v = 0;
        int.TryParse(strappid, out v);
        int vaule_appid = v;
        string  url = "http://app.xiaomi.com/details?id=" + Common.GetAppPackage();
        //url = "http://app.xiaomi.com/details?id=com.moonma.nongchangfind.pad";

        strUrlAppstore = url;
        strUrlComment = url;
        HttpRequest http = new HttpRequest(OnHttpRequestFinished);
        http.Get(url);
    }



    /*
     http://app.xiaomi.com/detail/87601

     <ul class=" cf"><li class="weight-font">软件大小:</li><li>59.13 M</li><li class="weight-font">版本号：</li><li>1.1.0</li><li class="weight-font">更新时间：</li><li>2015-07-03</li><li class="weight-font">包名：</li><li class="special-li">com.moonma.hanziyuan</li></ul>


     <h3 class="special-h3">新版特性</h3>
     <p class="pslide">修复笔画写失败后消除比较慢的问题</p>
     */

    public override void ParseData(byte[] data)
    {
        string strData = Encoding.UTF8.GetString(data);
      
        Debug.Log("ParseDataXiaomi start");
        //Debug.Log("ParseDataXiaomi strData:"+strData);

        string ptmpupdate = "新版特性";
        string ptmpupdate_start = "\"pslide\">";
        string ptmpupdate_end = "</p>";
        int idx = strData.IndexOf(ptmpupdate);
        if (idx >= 0)
        {
            Debug.Log("ParseDataXiaomi 1");
            string ptmp = strData.Substring(idx);
            idx = ptmp.IndexOf(ptmpupdate_start);
            if (idx >= 0)
            {
                string p = ptmp.Substring(idx + ptmpupdate_start.Length);
                idx = p.IndexOf(ptmpupdate_end);
                if (idx >= 0)
                {
                    string str = p.Substring(0, idx);
                    strUpdateNote = str;
                }
            }
        }



        string ptmpversion = "版本";
        string ptmpversion_start = "<li>";
        string ptmpversion_end = "</li>";


        idx = strData.IndexOf(ptmpversion);
        Debug.Log("ParseDataXiaomi get version idx=" + idx);
        if (idx >= 0)
        {
            Debug.Log("ParseDataXiaomi 2");
            string ptmp = strData.Substring(idx);
            idx = ptmp.IndexOf(ptmpversion_start);
            if (idx >= 0)
            {
                Debug.Log("ParseDataXiaomi 3");
                string p = ptmp.Substring(idx + ptmpversion_start.Length);
                idx = p.IndexOf(ptmpversion_end);
                if (idx >= 0)
                {

                    strVersionStore = p.Substring(0, idx);   

                }
            }
        }

      

        ParseFinished(this);
        Debug.Log("ParseDataXiaomi end");
    }
}
