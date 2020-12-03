using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;
using UnityEngine;

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
    HttpRequest httpReq;
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
        if (isParseFinished)
        {
            return;
        }
        httpReq = new HttpRequest(OnHttpRequestFinished);
        //sina
        //http.Get("http://int.dpool.sina.com.cn/iplookup/iplookup.php?format=json");
        //ip api
        httpReq.Get("http://ip-api.com/json");
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

        JsonData jsonRoot = JsonMapper.ToObject(str);
        if (jsonRoot == null)
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


    void OnHttpRequestFinished(HttpRequest req, bool isSuccess, byte[] data)
    {

        if (isSuccess)
        {

            ParseDataIpApiCom(data);

        }
        else
        {

        }
    }

}
