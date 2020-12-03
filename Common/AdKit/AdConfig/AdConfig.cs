using UnityEngine;
using System.Collections;

namespace Moonma.AdKit.AdConfig
{
    public class AdConfig
    {
        AdConfigParser adConfigParser;
        BasePlatformWrapper platformWrapper;
        static private AdConfig _main = null;
        public static AdConfig main
        {
            get
            {
                if (_main == null)
                {
                    _main = new AdConfig();
                    _main.Init();
                }
                return _main;
            }

        }

        void Init()
        {
            platformWrapper = PlatformWrapper.platform;
            adConfigParser = new AdConfigParser();
            adConfigParser.StartParseConfig(AppCommon.urlAdConfig);

            foreach (AdInfo info in AdConfig.main.adConfigParser.listPlatform)
            {
                platformWrapper.InitPlatform(info.source, AdConfigParser.SOURCE_TYPE_BANNER, info.appid, info.appkey, info.key_banner);
                platformWrapper.InitPlatform(info.source, AdConfigParser.SOURCE_TYPE_INSERT, info.appid, info.appkey, info.key_insert);
                platformWrapper.InitPlatform(info.source, AdConfigParser.SOURCE_TYPE_SPLASH_INSERT, info.appid, info.appkey, info.key_splash_insert);
                platformWrapper.InitPlatform(info.source, AdConfigParser.SOURCE_TYPE_SPLASH, info.appid, info.appkey, info.key_splash);
                platformWrapper.InitPlatform(info.source, AdConfigParser.SOURCE_TYPE_NATIVE, info.appid, info.appkey, info.key_native);
                platformWrapper.InitPlatform(info.source, AdConfigParser.SOURCE_TYPE_VIDEO, info.appid, info.appkey, info.key_video);
                platformWrapper.InitPlatform(info.source, AdConfigParser.SOURCE_TYPE_INSERT_VIDEO, info.appid, info.appkey, info.key_insert_video);

            }


            SetNoAdDay(Config.main.NO_AD_DAY);

            SetEnableAdSplash(true);
        }
        public void InitSDK()
        {
            platformWrapper.InitSDK();
        }
        public void InitPriority(string src, int type)
        {
            adConfigParser.InitPriority(src, type);
        }
        public string GetAdSource(int type)
        {
            return adConfigParser.GetAdSource(type);
        }
        public string GetAppId(string source)
        {
            return adConfigParser.GetAppId(source);
        }

        public string GetAdKey(string source, int type)
        {
            return adConfigParser.GetAdKey(source, type);
        }

        public AdInfo GetNextPriority(int type)
        {
            return adConfigParser.GetNextPriority(type);
        }

        public void SetEnableAdSplash(bool enable)
        {
            platformWrapper.SetEnableAdSplash(enable);
        }

        public void SetNoAd()
        {
            platformWrapper.SetNoAd();
        }
        public void SetNoAdDay(int day)
        {
            platformWrapper.SetNoAdDay(day);
        }
        public void SetAdSource(int type, string source)
        {
            platformWrapper.SetAdSource(type, source);
        }
        public void SetAppId(string source, string appid)
        {
            platformWrapper.SetAppId(source, appid);
        }

        public void SetAdKey(string source, int type, string key)
        {

            platformWrapper.SetAdKey(source, type, key);
        }

        public void SetConfig(int type, string source, string appid, string adkey)
        {

            platformWrapper.SetConfig(type, source, appid, adkey);
        }

        public void AdSplashSetConfig(string source, string appId, string appKey, string type)
        {
            platformWrapper.AdSplashSetConfig(source, appId, appKey, type);

        }

        public void AdSplashInsertSetConfig(string source, string appId, string appKey, string type)
        {

            platformWrapper.AdSplashInsertSetConfig(source, appId, appKey, type);
        }

        public void AdSplashInitAd(string source, int type)
        {

            platformWrapper.AdSplashInitAd(source, type);
        }
        public void AdSplashShow()
        {

            platformWrapper.AdSplashShow();
        }

    }
}
