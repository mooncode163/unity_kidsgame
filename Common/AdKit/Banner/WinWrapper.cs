#if UNITY_WSA && !UNITY_EDITOR 
using UnityEngine;
//using System;
using System.Runtime.InteropServices;
using System.Collections;
using AOT;//MonoPInvokeCallback
 
 // c++调用 c#: https://blog.csdn.net/fg5823820/article/details/47865741
namespace Moonma.AdKit.AdBanner
{
	internal class WinWrapper : BasePlatformWrapper
	{
		 
		[DllImport ("Common")]
		public static extern void AdBanner_InitAd(string source);
		[DllImport ("Common")]
		public static extern void AdBanner_ShowAd(bool isShow);
		[DllImport ("Common")]
		public static extern void AdBanner_SetScreenSize(int w,int h);
 		[DllImport ("Common")]
		public static extern void AdBanner_SetScreenOffset(int x,int y);
		[DllImport ("Common")]
		private static extern void AdBanner_SetCallbackUnity(AdFinishCallbackFuction callback);

		public delegate void AdFinishCallbackFuction(string source,string method,int w,int h);



	//static AdFinishCallbackFuction callback;


		public override void InitAd(string source)
		{
			Debug.Log("WinWrapper InitAd："+source);
		//	callback = CallbackAdFinish;
			AdBanner_SetCallbackUnity(CallbackAdFinish);
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

//c#回调函数 
	[MonoPInvokeCallback(typeof (AdFinishCallbackFuction))]
		static void CallbackAdFinish(string source,string method,int w,int h)
	{
		Debug.Log ("CallbackAdFinish method="+method+"  w="+w+" h="+h);
		AdKitCommon.main.AdBannerCallbackUnity( source,method,w,h);
	}

		 
	}
}

#endif