using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnAppVersionBaseFinishedDelegate(AppVersionBase app);

public class AppVersionBase
{
    public bool appNeedUpdate = false;
    public bool isNetWorkOk = false;
    public bool appCheckForAppstore = false;//app审核中
    public string strUrlComment;
    public string strUrlAppstore;
    public string strUpdateNote;
    public string strVersionStore;
    public bool isFirstCreat = false;

    //public OnAppVersionUpdateDelegate callbackUpdate { get; set; }

    public OnAppVersionBaseFinishedDelegate callbackFinished { get; set; }

    public void Init()
    {
        appNeedUpdate = false;
        isFirstCreat = false;
        appCheckForAppstore = false;
        strVersionStore = "";

    }

    public virtual void StartParseVersion()
    {
        isNetWorkOk = true;
        // int v = Common.Bool2Int(true);
        // PlayerPrefs.SetInt(AppVersion.STRING_KEY_APP_CHECK_FINISHED, v);
        // ParseFinished(this);
        if (this.callbackFinished != null)
        {
            Debug.Log("Appversion ParseFinished callbackFinished");
            this.callbackFinished(this);
        }
    }

    public virtual void ParseData(byte[] data)
    {
    }


    public void ParseFinished(AppVersionBase app)
    {
        Debug.Log("Appversion ParseFinished strVersionStore=" + strVersionStore);
        string appver = Application.version;
        if (Common.BlankString(strVersionStore))
        {

            if (Common.isiOS)
            {
                if (isFirstCreat)
                {
                    appCheckForAppstore = true;
                }
            }
            else
            {
                if (isFirstCreat)
                {
                    appCheckForAppstore = true;
                }
                // appCheckForAppstore = true;
            }
        }
        else
        {

            if (isFirstCreat)
            {
                appCheckForAppstore = true;
            }
            int ret = string.Compare(appver, strVersionStore);
            Debug.Log("Appversion stroe:version:" + strVersionStore + " ret=" + ret);
            if (ret >= 0)
            {
                appNeedUpdate = false;
                if (ret > 0)
                {
                    appCheckForAppstore = true;
                }
            }
            else
            {
                //版本更新
                appNeedUpdate = true;
                //appCheckVersionDidUpdate 

            }
        }

        if (Config.main.channel == Source.XIAOMI)
        {
            AppVersion.appCheckForXiaomi = appCheckForAppstore;
            if (Common.GetDayIndexOfUse() > Config.main.NO_AD_DAY)
            {
                //超过NO_AD_DAY 天数 认为审核完成
                appCheckForAppstore = false;
            }
        }


        Debug.Log("Appversion:appCheckForAppstore=" + appCheckForAppstore + " isNetWorkOk=" + isNetWorkOk);

        // if ((!appCheckForAppstore) && isNetWorkOk)
        if (!appCheckForAppstore)
        {
            int v = Common.Bool2Int(true);
            PlayerPrefs.SetInt(AppVersion.STRING_KEY_APP_CHECK_FINISHED, v);
        }

        //appCheckHasFinished = Common.Int2Bool(PlayerPrefs.GetInt(STRING_KEY_APP_CHECK_FINISHED));

        // Debug.Log("Appversion ParseFinished 1");
        if (this.callbackFinished != null)
        {
            Debug.Log("Appversion ParseFinished callbackFinished");
            this.callbackFinished(this);
        }
    }

    public void OnHttpRequestFinished(HttpRequest req, bool isSuccess, byte[] data)
    {
        Debug.Log("Appversion OnHttpRequestFinished:isSuccess=" + isSuccess);
        if (isSuccess)
        {
            isNetWorkOk = true;
            ParseData(data);

        }
        else
        {
            isNetWorkOk = false;
            ParseFinished(this);
        }
    }


}
