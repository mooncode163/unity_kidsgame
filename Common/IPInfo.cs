using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;
using UnityEngine;
using System;
using System.Threading;
using System.Threading.Tasks;
using BestHTTP;
//ip 库
//http://blog.sina.com.cn/s/blog_68786ef60101p3nj.html
//http://int.dpool.sina.com.cn/iplookup/iplookup.php?format=json
public class IPInfo
{
    public const string Key_IP_IN_CHINA = "key_ip_in_china";
    static public string country;//国家
    static public string city;
    static public string province;//省份

    static bool isParseFinished = false;


    bool isHttpFinish = false;
    JsonData jsonRoot;
    static private IPInfo _main = null;
    public static IPInfo main
    {
        get
        {
            if (_main == null)
            {
                _main = new IPInfo();
            }
            return _main;
        }
    }

    static public bool isInChina
    {
        get
        {
            bool ret = Common.GetBool(Key_IP_IN_CHINA, true);
            return ret;
        }
    }
    public void StartParserInfo()
    {
        // if (isHttpFinish)
        // {
        //     return;
        // }
        // httpReq = new HttpRequest(OnHttpRequestFinished);
        // //sina
        // //http.Get("http://int.dpool.sina.com.cn/iplookup/iplookup.php?format=json");
        // //ip api
        // httpReq.Get("http://ip-api.com/json");


        // HttpRequest httpReq = new HttpRequest(OnHttpRequestFinished);
        // httpReq.Get("http://ip-api.com/json");
        // while (!isHttpFinish)
        // {
        //     Debug.Log("IPInfo StartParserInfo waiting for isHttpFinish");
        //     Thread.Sleep(10);
        // }


        StartParserInfoAsync();
    }
    public async void StartParserInfoAsync()
    {
        if (isHttpFinish)
        {
            return;
        }
        await GetIpInfoAsync();

    }

    public async Task<string> GetIpInfoAsync()
    {
        Debug.Log("IPInfo GetIpInfoAsync start");
        if (isHttpFinish)
        {
            return "";
        }
        Debug.Log("IPInfo GetIpInfoAsync 0");
        // isHttpFinish = false;
        await Task.Run(() =>
        {
            Debug.Log("IPInfo GetIpInfoAsync 1");
            // HttpRequest httpReq = new HttpRequest(OnHttpRequestFinished);
            string url = "http://ip-api.com/json";
            Debug.Log("IPInfo GetIpInfoAsync 2");
            // HttpRequest.get 在android上卡死 使用besthttp
            // httpReq.Get("http://ip-api.com/json");

            HTTPRequest reqHttp = new HTTPRequest(new Uri(url), HTTPMethods.Get, OnRequestFinishedBesthttp);
            reqHttp.Send();

            Debug.Log("IPInfo GetIpInfoAsync 3");
            while (!isHttpFinish)
            {
                Debug.Log("IPInfo GetIpInfoAsync waiting for isHttpFinish");
                Thread.Sleep(10);
            }
        });
        Debug.Log("IPInfo GetIpInfoAsync end");
        return "";

    }

    // 根据ip区域判断是否是华为审核  深圳和东莞
    public bool IsHuaweiAppStoreCheck()
    {
        bool ret = false;
        if (jsonRoot != null)
        {
            ret = false;
            if (!JsonUtil.ContainsKey(jsonRoot, "city"))
            {
                return ret;
            }
            string city = (string)jsonRoot["city"];
            city = city.ToLower();
            if ((city == "shenzhen") || (city == "dongguan"))
            {
                ret = true;
            }
            // if (city == "ganzhou")
            // {
            //     ret = true;
            // }

        }

        return ret;
    }

    void UpdateInfo(bool isChina)
    {
        Debug.Log("isChina: country:" + country + " isChina=" + isChina);
        Common.SetIpInChina(isChina);
        Common.SetBool(Key_IP_IN_CHINA, isChina);
        int value = PlayerPrefs.GetInt(Key_IP_IN_CHINA);
        bool ret = Common.GetBool(Key_IP_IN_CHINA);
        Debug.Log("read: isChina=" + ret + " value=" + value);
    }
    void ParseDataIpApiCom(byte[] data)
    {
        if (data == null)
        {
            Debug.Log("IPInfo ParseData data is null");
            return;
        }
        string str = Encoding.UTF8.GetString(data);

        jsonRoot = JsonMapper.ToObject(str);
        if (jsonRoot == null)
        {
            return;
        }
        if (!JsonUtil.ContainsKey(jsonRoot, "country"))
        {
            return;
        }
        country = (string)jsonRoot["country"];
        city = (string)jsonRoot["city"];
        province = (string)jsonRoot["region"];
        isParseFinished = true;

        bool isChina = false;
        if (country.ToLower() == "china")
        {
            isChina = true;
        }
        UpdateInfo(isChina);

    }

    void ParseDataSina(byte[] data)
    {
        if (data == null)
        {
            Debug.Log("IPInfo ParseData data is null");
            return;
        }
        string str = Encoding.UTF8.GetString(data);

        JsonData jsonRoot = JsonMapper.ToObject(str);
        if (jsonRoot == null)
        {
            return;
        }
        country = (string)jsonRoot["country"];
        city = (string)jsonRoot["city"];
        province = (string)jsonRoot["province"];
        isParseFinished = true;

        bool isChina = false;
        if (country == "中国")
        {
            isChina = true;
        }
        UpdateInfo(isChina);
    }

    void OnRequestFinishedBesthttp(HTTPRequest req, HTTPResponse response)
    {
        isHttpFinish = true;
        Debug.Log("Task   OnRequestFinishedBesthttp");
        if (response != null && response.IsSuccess)
        {
   ParseDataIpApiCom(response.Data);

        }
        else
        {
      
        }

    }
    void OnHttpRequestFinished(HttpRequest req, bool isSuccess, byte[] data)
    {
        Debug.Log("IPInfo  OnHttpRequestFinished");
        if (isSuccess)
        {

            ParseDataIpApiCom(data);

        }
        else
        {

        }

        isHttpFinish = true;
    }

}
