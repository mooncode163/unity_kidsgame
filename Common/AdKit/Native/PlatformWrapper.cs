using UnityEngine;
using System.Collections;

namespace Moonma.AdKit.AdNative
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
#else
                return new BasePlatformWrapper();
#endif
            }
        }
    }


    internal class BasePlatformWrapper
    {

        public virtual void SetObjectInfo(string objName, string objMethod)
        {

        }
        public virtual void InitAd(string source)
        {

        }

        public virtual void ShowAd()
        {

        }
        public virtual void ShowSplash(string source)
        {

        }
        public virtual void OnClickAd()
        {

        }
    }
}