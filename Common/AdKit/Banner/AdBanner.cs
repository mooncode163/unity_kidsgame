using UnityEngine;
using System.Collections;
using Moonma.AdKit.AdConfig;
namespace Moonma.AdKit.AdBanner
{
	
  public class AdBanner  
	{
        	public static void InitAd(string source)
		{
            BasePlatformWrapper platformWrapper = PlatformWrapper.platform;
						Debug.Log("AdBanner:InitAd"); 
						Moonma.AdKit.AdConfig.AdConfig.main.InitPriority(source,AdConfigParser.SOURCE_TYPE_BANNER);
            platformWrapper.InitAd(source);
		}


		public static void ShowAd(bool isShow)
		{
			   BasePlatformWrapper platformWrapper = PlatformWrapper.platform;
            platformWrapper.ShowAd(isShow);
		}


		 public static void SetScreenSize(int w,int h)
		{
			 BasePlatformWrapper platformWrapper = PlatformWrapper.platform;
       platformWrapper.SetScreenSize(w,h);
		 
		}
		//y 基于屏幕底部
		public static void SetScreenOffset(int x,int y)
		{
				BasePlatformWrapper platformWrapper = PlatformWrapper.platform;
       platformWrapper.SetScreenOffset(x,y);
		}

    }
}
