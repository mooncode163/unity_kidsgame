using UnityEngine;
using System.Collections;

namespace Moonma.IAP
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

        public virtual void SetAppKey(string key)
        {

        }
        public virtual void SetObjectInfo(string objName, string objMethod)
        {

        }
        public virtual void SetSource(string source)
        {

        }
        public virtual void StartBuy(string product, bool isConsume)
        {

        }

        public virtual void RestoreBuy(string product)
        {

        }
        public virtual void StopBuy(string product)
        {

        }

    }
}