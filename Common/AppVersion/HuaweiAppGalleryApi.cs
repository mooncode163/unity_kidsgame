using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using LitJson;
using Moonma.Html.HtmlWebView;
using BestHTTP;
using System;
using System.Threading;
using System.Threading.Tasks;

public class HuaweiAppGalleryApi
{
    public const string ClientId = "469947311665972416";
    public const string ClientSecret = "7701769ABE85209F58C1736D3FD95C8B9B7225F6EDC1415482D1EB142C8ED201";
    string accessToken = "";
    string appVersion = "";
    HTTPRequest reqHttpToken;

    static private HuaweiAppGalleryApi _main = null;
    public static HuaweiAppGalleryApi main
    {
        get
        {
            if (_main == null)
            {
                _main = new HuaweiAppGalleryApi();

            }
            return _main;
        }
    }

    async Task<string> GetTokenAsync()
    {
        Debug.Log("Task start");
        if (!Common.BlankString(accessToken))
        {
            return accessToken;
        }
        await Task.Run(() =>
        {
            StartGetToken();
            Debug.Log("Task Async Executed");
            while (Common.BlankString(accessToken))
            {
                Thread.Sleep(10);
            }
        });
        Debug.Log("Task End");
        return accessToken;
    }
    public async void GetToken()
    {

        Debug.Log("Task ExecuteAsync before");
        await GetTokenAsync(); //新建一个线程执行这个函数
        // t.Wait();//阻塞,一直等待函数执行完成
        Debug.Log("Task ExecuteAsync end");
    }
    public void StartGetToken()
    {

        // if (!Common.BlankString(accessToken))
        // {
        //     return;
        // }
        string url = "https://connect-api.cloud.huawei.com/api/oauth2/v1/token";
        // header = {
        //     'Content-Type': 'application/json'
        //       # 'Authorization': 'Bearer %s'%str(token, encoding='utf-8')
        //     # 'Authorization': strauthor
        //     # 'Authorization': 'Bearer %s' % token
        //     # 指定JWT
        // }


        Debug.Log("HuaweiAppGalleryApi GetToken url=" + url);
        HTTPRequest reqHttp = new HTTPRequest(new Uri(url), HTTPMethods.Post, OnRequestFinishedToken);
        reqHttp.AddHeader("Content-Type", "application/json");

        // params
        // reqHttp.AddField("grant_type", "client_credentials");
        // reqHttp.AddField("client_id", ClientId);
        // reqHttp.AddField("client_secret", ClientSecret);

        Hashtable data = new Hashtable();
        data["grant_type"] = "client_credentials";
        data["client_id"] = ClientId;
        data["client_secret"] = ClientSecret;

        string strJson = JsonMapper.ToJson(data);
        Debug.Log("HuaweiAppGalleryApi strJson=" + strJson);
        reqHttp.RawData = Encoding.UTF8.GetBytes(strJson);
        reqHttp.Send();
        reqHttpToken = reqHttp;

    }


    //  https://developer.huawei.com/consumer/cn/doc/development/AppGallery-connect-References/agcapi-app-info_query_v2
    public async Task<string> GetVersion(string appId)
    {
        string ret = await GetTokenAsync();
        Debug.Log("Task GetTokenAsync ret=" + ret);
        await Task.Run(() =>
        {
            string url = "https://connect-api.cloud.huawei.com/api/publish/v2/app-info?appId=" + appId;
            Debug.Log("HuaweiAppGalleryApi GetVersion url=" + url);
            HTTPRequest reqHttp = new HTTPRequest(new Uri(url), HTTPMethods.Get, OnRequestFinishedVersion);
            reqHttp.AddHeader("Content-Type", "application/json");
            reqHttp.AddHeader("client_id", ClientId);
            reqHttp.AddHeader("Authorization", "Bearer " + accessToken);
            reqHttp.Send();
            Debug.Log("Task Async Executed");
            while (Common.BlankString(appVersion))
            {
                Thread.Sleep(10);
            }
        });

        return appVersion;

    }

    void OnRequestFinishedVersion(HTTPRequest req, HTTPResponse response)
    {
        Debug.Log("Task HuaweiAppGalleryApi OnRequestFinishedVersion");
        if (response.IsSuccess)
        {

            string str = Encoding.UTF8.GetString(response.Data);
            Debug.Log("Task HuaweiAppGalleryApi version str=" + str);

            JsonData root = JsonMapper.ToObject(str);
            JsonData appInfo = root["appInfo"];
            int releaseState = (int)appInfo["releaseState"];


            Debug.Log("  HuaweiAppGalleryApi version releaseState=" + releaseState);

            string key = "versionNumber";
            string version = "1.0.0";
            string versionApi = version;
            if (JsonUtil.ContainsKey(appInfo, key))
            {
                versionApi = (string)appInfo[key];

            }

            if ((Common.GetAppVersion() == versionApi) && (releaseState != 0))
            {
                //提交版本 审核中等或者审核不通过等
                // releaseState = 0;
                versionApi = "1.0.0";
                version = versionApi;
            }


            // if (releaseState == 0)
            {
                // 已经上架
                version = versionApi;
            }

            Debug.Log("Task  HuaweiAppGalleryApi version=" + version);
            appVersion = version;

        }
        else
        {
            Debug.Log("Task HuaweiAppGalleryApi OnRequestFinishedVersion fail");
        }

    }

    void OnRequestFinishedToken(HTTPRequest req, HTTPResponse response)
    {

        if (response.IsSuccess)
        {
            string str = Encoding.UTF8.GetString(response.Data);
            JsonData root = JsonMapper.ToObject(str);
            accessToken = (string)root["access_token"];
            Debug.Log("HuaweiAppGalleryApi OnRequestFinished accessToken=" + accessToken);
            Debug.Log("Task OnRequestFinished accessToken=" + accessToken);
            // GetVersion(Config.main.appId);
            // GetVersion("103066765");
        }
        else
        {
            Debug.Log("HuaweiAppGalleryApi OnRequestFinished fail");
        }


    }




}
