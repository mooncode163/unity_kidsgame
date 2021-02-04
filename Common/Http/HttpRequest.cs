using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using BestHTTP;
using UnityEngine;
using UnityEngine.UI;

public delegate void OnHttpRequestFinishedDelegate(HttpRequest req, bool isSuccess, byte[] data);
public class HttpRequest
{
    public bool isReadFromCatch;
    public int index;
    HTTPRequest reqHttp;
    public string strUrl;
    public bool isEnableCache = true;
    public OnHttpRequestFinishedDelegate Callback { get; set; }

    public HttpRequest(OnHttpRequestFinishedDelegate callback)
    {
        this.Callback = callback;
    }
    static public string GetWebUrlOfAsset(string filePath)
    {
        return Application.streamingAssetsPath + "/" + filePath;
    }
    bool EnableReadFromCache()
    {
        bool ret = false;
        if (Common.isWinUWP || Common.isiOS || Common.isAndroid)
        {
            ret = true;
        }
        if (!isEnableCache)
        {
            ret = false;
        }
        return ret;
    }
    public void Get(string url)
    {
        Debug.Log("HttpRequest Get start");
    
        strUrl = url;
        isReadFromCatch = false;
        if (EnableReadFromCache())
        {
             Debug.Log("HttpRequest Get EnableReadFromCache");
            string filePath = GetCatchFilePathOfUrl(url);
            bool isExist = File.Exists(filePath);
            if (isExist)
            {
                FileStream fs = new FileStream(filePath, FileMode.Open);
                if (fs != null)
                {
                    int len = (int)fs.Length;
                    if (len > 0)
                    {
                        byte[] data = new byte[len];
                        fs.Read(data, 0, len);
                        fs.Close();
                        isReadFromCatch = true;
                        if (this.Callback != null)
                        {

                            this.Callback(this, true, data);
                        }
                        return;
                    }
                }
            }
        }

        // StartCoroutine(WWWGet(url));
        Debug.Log("HttpRequest Get:url=" + url);
        //@moon:HTTPRequest 如果用局部变量 windows 10 uwp il2cpp 运行时socket io会crash,故而用成员变量
        reqHttp = new HTTPRequest(new Uri(url), HTTPMethods.Get, OnRequestFinished);
        //ios6 ua
        //该ua会导致小米网址无法获取版本号
        // reqHttp.AddHeader("User-Agent", "Mozilla/5.0 (iPhone; CPU iPhone OS 6_0 like Mac OS X) AppleWebKit/536.26 (KHTML, like Gecko) Version/6.0 Mobile/10A403 Safari/8536.25");
        reqHttp.Send();
    }
    void OnRequestFinished(HTTPRequest req, HTTPResponse response)
    {
        // Debug.Log("HttpRequest OnRequestFinished 1");
        if (response == null)
        {
            if (this.Callback != null)
            {
                // Debug.Log("HttpRequest OnRequestFinished response is null");
                this.Callback(this, false, null);
            }
            return;
        }

        if (response.IsSuccess)
        {
            //Debug.Log("HttpRequest OnRequestFinished 2");
            if (EnableReadFromCache())
            {
                string filePath = GetCatchFilePathOfUrl(req.Uri.AbsoluteUri);
                FileStream fs = new FileStream(filePath, FileMode.Create);
                if (fs != null)
                {
                    byte[] data = response.Data;
                    int len = data.Length;
                    if (len > 0)
                    {
                        fs.Write(data, 0, len);
                        fs.Close();
                    }

                }
            }
        }

        //Debug.Log("HttpRequest OnRequestFinished 3");

        if (this.Callback != null)
        {
            //Debug.Log("HttpRequest OnRequestFinished callback");
            this.Callback(this, response.IsSuccess, response.Data);
        }
    }



    string GetCatchFilePathOfUrl(string url)
    {

        // log("MCHttpRequest::getFilePathOfUrl 0");
        // Debug.Log("GetCachePath 0");
        string strDir = Common.GetCachePath();// FileUtils::getInstance()->getWritablePath();
                                              //  Debug.Log("GetCachePath 1:"+strDir);
        string strMD5 = MD5.GetMD5(url);
        // Debug.Log("GetCachePath 2");
        string strRet = strDir + "/" + strMD5;
        // Debug.Log(strMD5);

        return strRet;
    }


}
