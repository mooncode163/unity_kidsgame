using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Moonma.Share
{
    public class SharePlatformInfo
    {
        public string source;
        public string appId;
        public string appKey;
    }
 
    public class Share
    {
        public const int SHARE_TYPE_TEXT = 0;//文本
        public const int SHARE_TYPE_IMAGE = 1;//图片
        public const int SHARE_TYPE_IMAGE_TEXT = 2;//图文（包含一张图片和一段文本）
        public const int SHARE_TYPE_WEB = 3;//网页类型（网页链接）
        public const int SHARE_TYPE_MUSIC = 4;//音乐（只支持音乐URL、缩略图及描述
        public const int SHARE_TYPE_VIDEO = 5;//视频（只支持视频URL、缩略图及描述）
        public const int SHARE_TYPE_WEIXIN_MINIAPP = 6;//微信小程序

        BasePlatformWrapper platformWrapper;
        
        static private Share _main = null;
        public static Share main
        {
            get
            {
                if (_main == null)
                {
                    _main = new Share();
                    _main.InitValue();
                }
                return _main;
            }
        }
        void InitValue()
        {
            platformWrapper = PlatformWrapper.platform;
            //umeng
            string source = Source.UMENG;
            Init(source, Config.main.GetShareAppId(source), Config.main.GetShareAppKey(source));

            foreach (SharePlatformInfo info in Config.main.listSharePlatform)
            {
                InitPlatform(info.source, info.appId, info.appKey);

            }

        }

        //平台初始化
        public void Init(string source, string appId, string appKey)
        {
            platformWrapper.Init(source, appId, appKey);
        }

        public void SetObjectInfo(string objName)
        {
            platformWrapper.SetObjectInfo(objName);
        }

        public void InitPlatform(string source, string appId, string appKey)
        {
            platformWrapper.InitPlatform(source, appId, appKey);
        }


        public void ShareWeb(string source, string title, string detail, string url)
        {
            platformWrapper.ShareWeb(source, title, detail, url);
        }
        public void ShareText(string source, string title,string url)
        {
             
        }
        public void ShareImage(string source, string pic,string url)
        {
            platformWrapper.ShareImage(source,pic,url); 
        }
        public void ShareImageText(string source, string title, string pic,string url)
        {
             platformWrapper.ShareImageText(source,title,pic,url);
        }
    }
}
