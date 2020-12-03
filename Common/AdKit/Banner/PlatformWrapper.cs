using UnityEngine;
using System.Collections;

namespace Moonma.AdKit.AdBanner
{
 internal class PlatformWrapper
	{
	public static BasePlatformWrapper platform
		{
			get {
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


		public virtual void InitAd(string source)
		{
			 
		}

		public virtual void ShowAd(bool isShow)
		{
			 
		}

		 public virtual void SetScreenSize(int w,int h)
		{
			  
		}

		//y 基于屏幕底部
		public virtual void SetScreenOffset(int x,int y)
		{
			  
		}
    }
}