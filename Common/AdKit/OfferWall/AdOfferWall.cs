using UnityEngine;
using System.Collections;
using Moonma.AdKit.AdConfig;

namespace Moonma.AdKit.AdOfferWall
{
    public class AdOfferWall
    { 

        static bool isHasInit = false;
        public static void SetObjectInfo(string objName, string objMethod)
        {
            BasePlatformWrapper platformWrapper = PlatformWrapper.platform;
            platformWrapper.SetObjectInfo(objName, objMethod);
        }
        public static void SetType(int type)
        {
            BasePlatformWrapper platformWrapper = PlatformWrapper.platform;
            platformWrapper.SetType(type);
        }
        public static void InitAd(string source)
        {
           // Moonma.AdKit.AdConfig.AdConfig.main.InitPriority(source,AdConfigParser.SOURCE_TYPE_VIDEO);
            BasePlatformWrapper platformWrapper = PlatformWrapper.platform;
            platformWrapper.InitAd(source);
        }
               public static void PreLoad(string source)
        {
            BasePlatformWrapper platformWrapper = PlatformWrapper.platform;
            platformWrapper.PreLoad(source);
        }

        public static void ShowAd()
        {
            BasePlatformWrapper platformWrapper = PlatformWrapper.platform;
            platformWrapper.ShowAd();
        }
        public static void OnClickAd()
        {
            BasePlatformWrapper platformWrapper = PlatformWrapper.platform;
            platformWrapper.OnClickAd();
        }


    }
}