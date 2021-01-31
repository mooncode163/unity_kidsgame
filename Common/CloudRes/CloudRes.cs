

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LitJson;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
public class CloudRes
{
    //    URL_CLOUND_RES: "https://6c69-lianlianle-shkb3-1259451541.tcb.qcloud.la/CloudRes.zip?sign=0763ef1a87ef54872f92151f308881d9&t=1560564859",
    //http://47.242.56.146/CloudRes/unity/kidsgame/LearnWord/LearnWord/GameRes.zip
    // public const string URL_RES_VERSION = "http://47.242.56.146/CloudRes/unity/kidsgame/LearnWord/LearnWord/version.json";//放在云端的资源
    public const string CLOUD_RES_DIR = "GameRes/CloudRes";//放在云端的资源

    //微信小游戏 连连乐 wx3e44af039aee1b96

    public string resVersionWeb = "";
    public string resVersionLocal = "";
    public bool isNeedShow;
    static private CloudRes _main = null;
    public static CloudRes main
    {
        get
        {
            if (_main == null)
            {
                _main = new CloudRes();
                _main.Init();
            }
            return _main;
        }
    }
    public string rootPathGameRes
    {
        get
        {
            string ret = "";
            string dirRoot = Application.temporaryCachePath;
            if (enable)
            {

                ret = dirRoot + "/" + Common.GAME_RES_DIR;
                //创建文件夹
                // FileUtil.CreateDir(ret);
            }
            else
            {
                ret = Common.GAME_RES_DIR;
            }

            if (Application.isEditor || GameManager.main.isLoadGameScreenShot)
            {
                // F:\sourcecode\unity\product\kidsgame\kidsgameUnity\Assets
                // 模拟测试 debug
                // dirRoot = Application.dataPath;
                // dirRoot = FileUtil.GetFileDir(dirRoot);
                // dirRoot = FileUtil.GetFileDir(dirRoot);
                // dirRoot += "/tmp";
                // ret = dirRoot + "/" + Common.GAME_RES_DIR;

                // 实际地址
                dirRoot = Resource.dirResourceDataApp;
                ret = dirRoot + "/" + Common.GAME_RES_DIR;
                // 判断文件数量多的时候比较费时间
                if (!FileUtil.DirIsExist(ret))
                {
                    dirRoot = Resource.dirGameResCommon;
                    ret = dirRoot + "/" + Common.appKeyName;
                     Debug.Log("CloudRes dirGameResCommon=" + dirRoot);
                }else{
                    Debug.Log("CloudRes dirResourceDataApp=" + dirRoot);
                }
            }
            Debug.Log("CloudRes rootPath=" + ret);
            return ret;
        }
    }

    public bool enable
    {
        get
        {


            if (GameManager.main.isLoadGameScreenShot)
            {
                return false;
            }
            if (Common.BlankString(Config.main.urlGameRes))
            {
                return false;
            }
            if (Application.isEditor)
            {
                return true;
            }

            if (Config.main.channel == Source.GP)
            {
                return true;
            }
            return false;
        }
    }


    public async Task<string> CheckNeedShow()
    {
        Debug.Log("CloudRes Config.main.urlVersionGameRes=" + Config.main.urlVersionGameRes);

        bool ret = false;
        if (CloudRes.main.enable)
        {
            if (!FileUtil.DirIsExist(CloudRes.main.rootPathGameRes))
            {
                // 第一次
                ret = true;
                string tmp = await CheckVersion();
                int retcompare = string.Compare(resVersionLocal, resVersionWeb);
                Debug.Log("CloudRes Compare 1  ret=" + retcompare);
            }
            else
            {
                //核对版本
                string tmp = await CheckVersion();
                int retcompare = string.Compare(resVersionLocal, resVersionWeb);
                Debug.Log("CloudRes Compare 2 ret=" + retcompare);
                if (retcompare >= 0)
                {
                }
                else
                {
                    //版本更新  
                    ret = true;

                }
            }

        }
        isNeedShow = ret;
        return "";

    }
    public async Task<string> CheckVersion()
    {
        Debug.Log("CheckVersion start CloudRes resVersionWeb=" + resVersionWeb + " resVersionLocal=" + resVersionLocal);
        resVersionWeb = await StartParserResWebVersion(Config.main.urlVersionGameRes);
        resVersionLocal = GetResLocalVersion();
        Debug.Log("CheckVersion end CloudRes resVersionWeb=" + resVersionWeb + " resVersionLocal=" + resVersionLocal);
        return "";
    }




    void Init()
    {

    }

    public string GetResLocalVersion()
    {
        string filepath = CloudRes.main.rootPathGameRes + "/version.json";
        if (!File.Exists(filepath))
        {
            return "1.0.0";
        }
        string json = "";
        if (enable && !Application.isEditor)
        {
            json = FileUtil.ReadStringFromRawFile(filepath);
        }
        else
        {
            json = FileUtil.ReadStringAsset(filepath);
        }
        return ParserJsonVersion(json);
    }
    public async Task<string> StartParserResWebVersion(string url)
    {
        await Task.Run(() =>
        {
            HttpRequest http = new HttpRequest(OnHttpRequestFinished);
            http.isEnableCache = false;
            http.Get(url);
            Debug.Log("CloudRes Task Async Executed");
            while (Common.BlankString(resVersionWeb))
            {
                Debug.Log("CloudRes Task Async BlankString ");
                Thread.Sleep(2);
            }
        });

        return resVersionWeb;

    }

    public string ParserJsonVersion(string json)
    {
        JsonData root = JsonMapper.ToObject(json);
        string version = (string)root["version"];
        return version;
    }

    void OnHttpRequestFinished(HttpRequest req, bool isSuccess, byte[] data)
    {
        // StartCoroutine(OnHttpRequestFinishedEnumerator(isSuccess, data));

        if (isSuccess)
        {
            string json = Encoding.UTF8.GetString(data);
            resVersionWeb = ParserJsonVersion(json);
            Debug.Log("CloudRes OnHttpRequestFinished finsh resVersionWeb=" + resVersionWeb);
        }
        else
        {
            Debug.Log("CloudRes OnHttpRequestFinished fail");
        }
    }

}
