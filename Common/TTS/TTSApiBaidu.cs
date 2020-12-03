using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Text;
using LitJson;

public class TTSApiBaidu : TTSApiBase
{

    string accessToken;

    public override void SpeakWeb(string text)
    {
        textSpeak = text;
        if (Common.BlankString(accessToken))
        {
            this.GetBaiDuAccessToken();
        }
        else
        {
            this.SpeakInternal(text);
        }
    }


    void SpeakInternal(string text)
    {
        string url = GetTextUrl(text);
        PlayUrl(url);
    }
    public override string GetTextUrl(string text)
    {
        //百度语音官方文档 http://yuyin.baidu.com/docs/tts/133
        //https://www.cnblogs.com/kasher/p/8483274.html
        //MP3:https://blog.csdn.net/zhang_ruiqiang/article/details/50774570
        //mpga 格式：https://tts.baidu.com/text2audio?lan=zh&ie=UTF-8&spd=5&text=
        // var url = "https://tts.baidu.com/text2audio?lan=zh&ie=UTF-8&spd=5&text=" + str;


        //需要对tsn接口的文本字符串参数进行编码 没有做编码，直接上文本的，也会出现安卓正常IOS没有声音的情况
        string strencode = UnityWebRequest.EscapeURL(text);

        //var url = "https://tsn.baidu.com/text2audio?&lan=zh&cuid=moon&ctp=1&tok=24.b79c9ea129a4009fc20b0b542d1aa8e4.2592000.1554471263.282335-15699370&tex=" + strencode;
        string url = "https://tsn.baidu.com/text2audio?&lan=zh&cuid=moon&ctp=1&tok=" + accessToken + "&tex=" + strencode;
        // if ((cc.sys.isBrowser) || (cc.Common.main().isWeiXin))
        // {
        //     //openapi.baidu.com/oauth浏览器不能跨域访问
        //     url = "https://tts.baidu.com/text2audio?lan=zh&ie=UTF-8&spd=5&text=" + strencode;
        // }
        //添加mp3后缀 让cc.loader.load认为加载声音资源
        string ext = "&1.mp3";
        //url = url + ext;
        //url = "https://cdn.feilaib.top/img/sounds/bg.mp3";
        return url;
    }
    //认证权限access_token 
    void GetBaiDuAccessToken()
    {

        //app: 儿童游戏
        var url = "https://openapi.baidu.com/oauth/2.0/token?grant_type=client_credentials&client_id=5kkNuRq7npqTCEUMojvyoyX3&client_secret=rod6yBGG7HobkYVKUci2Z1GQ0zGkzwnZ";
        // var url = "https://7368-shapecolor-4f2a07-1258767259.tcb.qcloud.la/ConfigData/config/config_android.json";//?sign=091a9466897a9f8ad7ab08ce048ada7f&t=1551928854
        //var url = "https://7368-shapecolor-4f2a07-1258767259.tcb.qcloud.la/ConfigData/language/language.csv?sign=bba03f0107d858fb133c429d0db8f713&t=1551933465";


        HttpRequest httpReq = new HttpRequest(OnHttpRequestFinished);
        httpReq.Get(url);

    }

    void ParseData(byte[] data)
    {
        if (data == null)
        {
            return;
        }
        string str = Encoding.UTF8.GetString(data);
        JsonData root = JsonMapper.ToObject(str);
        accessToken = (string)root["access_token"];

    }

    void OnHttpRequestFinished(HttpRequest req, bool isSuccess, byte[] data)
    {
        if (isSuccess)
        {
            ParseData(data);
            SpeakInternal(textSpeak);
        }
    }
}

