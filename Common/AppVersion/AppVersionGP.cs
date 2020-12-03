using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using LitJson;
public class AppVersionGP : AppVersionBase
{
    public override void StartParseVersion()
    {
        string strappid = Config.main.GetAppIdOfStore(Source.HUAWEI);
        //https://play.google.com/store/apps/details?id=com.moonma.shapecolor
        string url  = "https://play.google.com/store/apps/details?id=" + Common.GetAppPackage();
        //url = "https://play.google.com/store/apps/details?id=com.moonma.shapecolor";
        strUrlAppstore = url;
        strUrlComment = url;
        HttpRequest http = new HttpRequest(OnHttpRequestFinished);
        http.Get(url);
    }

    public override void ParseData(byte[] data)
    {
        string strData = Encoding.UTF8.GetString(data);

        /*
       </script><script nonce="">AF_initDataCallback({key: 'ds:7', isError:  false , hash: '7', data:function(){return ["55M","1.0.0","5.0 及更高版本"]
         */
        string ptmp_start = "AF_initDataCallback({key: 'ds:7'";
        string ptmp_mid = "data:function()";
        string ptmp_mid2 = ",\"";
        string ptmp_end = "\"";

        int idx = strData.IndexOf(ptmp_start);
        if (idx >= 0)
        {
            string ptmp = strData.Substring(idx);
            idx = ptmp.IndexOf(ptmp_mid);
            if (idx >= 0)
            {
                string p = ptmp.Substring(idx + ptmp_mid.Length);
                idx = p.IndexOf(ptmp_mid2);
                if (idx >= 0)

                {
                    p = p.Substring(idx + ptmp_mid2.Length);
                    idx = p.IndexOf(ptmp_end);
                    string version = p.Substring(0, idx);
                    strVersionStore = version;
                }
            }

        }



        ParseFinished(this);
    }


    // private void updateApp() {
    //     String packageName = context.getPackageName();
    //     Intent intent = new Intent(Intent.ACTION_VIEW);
    //     intent.setData(Uri.parse("market://details?id="+packageName));
    //     context.startActivity(intent);
    // }
}
