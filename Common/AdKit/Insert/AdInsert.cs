using UnityEngine;
using System.Collections; 

namespace Moonma.AdKit.AdInsert
{
  public class AdInsert  
	{
		public static void SetObjectInfo(string objName)
		{ 
			BasePlatformWrapper platformWrapper = PlatformWrapper.platform; 
            platformWrapper.SetObjectInfo(objName);
		}
        	public static void InitAd(string source)
		{
            BasePlatformWrapper platformWrapper = PlatformWrapper.platform;
						Debug.Log("AdInsert:InitAd");
						Moonma.AdKit.AdConfig.AdConfig.main.InitPriority(source,AdConfigParser.SOURCE_TYPE_INSERT);
            platformWrapper.InitAd(source);
		}


		public static void ShowAd()
		{
			   BasePlatformWrapper platformWrapper = PlatformWrapper.platform;
            platformWrapper.ShowAd();
		}
    }
}