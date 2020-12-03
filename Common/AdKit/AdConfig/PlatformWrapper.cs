using UnityEngine;
using System.Collections;

namespace Moonma.AdKit.AdConfig
{
    internal class PlatformWrapper
    {
        public static BasePlatformWrapper platform
        {
            get
            {
#if UNITY_ANDROID && !UNITY_EDITOR
				return new AndroidWrapper();
#elif UNITY_IPHONE && !UNITY_EDITOR
				return new iOSWrapper();
#elif UNITY_WSA && !UNITY_EDITOR
				return new WinWrapper();
#else
                return new BasePlatformWrapper();
#endif
            }
        }
    }


    internal class BasePlatformWrapper
    {
        public virtual void InitPlatform(string source, int type, string appId, string appKey, string adKey)
        {

        }

        public virtual void SetEnableAdSplash(bool enable)
        {

        }
        public virtual void InitSDK()
        {

        }

        public virtual void SetNoAd()
        {

        }
        public virtual void SetNoAdDay(int day)
        {

        }
        public virtual void SetAdSource(int type, string source)
        {

        }
        public virtual void SetAppId(string source, string appid)
        {

        }

        public virtual void SetAdKey(string source, int type, string key)
        {

        }
        public virtual void SetConfig(int type, string source, string appid, string adkey)
        {

        }

        public virtual void AdSplashSetConfig(string source, string appId, string appKey, string type)
        {

        }

        public virtual void AdSplashInsertSetConfig(string source, string appId, string appKey, string type)
        {

        }


        public virtual void AdSplashInitAd(string source, int type)
        {

        }
        public virtual void AdSplashShow()
        {

        }




    }
}