#if UNITY_IPHONE && !UNITY_EDITOR
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections;
 
namespace Moonma.AdKit.AdBanner
{
	internal class iOSWrapper : BasePlatformWrapper
	{
		 
		[DllImport ("__Internal")]
		public static extern void AdBanner_InitAd(string source);
		[DllImport ("__Internal")]
		public static extern void AdBanner_ShowAd(bool isShow);
		[DllImport ("__Internal")]
		public static extern void AdBanner_SetScreenSize(int w,int h);
 		[DllImport ("__Internal")]
		public static extern void AdBanner_SetScreenOffset(int x,int y);

		public override void InitAd(string source)
		{
			 AdBanner_InitAd( source);
		}

		public override void ShowAd(bool isShow)
		{
			 AdBanner_ShowAd(isShow);
		}
		 public override void SetScreenSize(int w,int h)
		{
			 AdBanner_SetScreenSize(w,h);
		}

		//y 基于屏幕底部
		 public override void SetScreenOffset(int x,int y)
		{
			AdBanner_SetScreenOffset(x,y);
		}
		 
	}
}

#endif