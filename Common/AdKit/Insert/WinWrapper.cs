#if UNITY_WSA && !UNITY_EDITOR
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections;
using AOT;//MonoPInvokeCallback

namespace Moonma.AdKit.AdInsert
{
	internal class WinWrapper : BasePlatformWrapper
	{
		 
		[DllImport ("Common")]
		public static extern void AdInsert_InitAd(string source);
		[DllImport ("Common")]
		public static extern void AdInsert_ShowAd();
		[DllImport ("Common")]
		public static extern void AdInsert_SetObjectInfo(string objName); 
		[DllImport ("Common")]
		private static extern void AdInsert_SetCallbackUnity(AdFinishCallbackFuction callback);


		public delegate void AdFinishCallbackFuction(string source,string method);


		public override void SetObjectInfo(string objName)
		{
			 AdInsert_SetObjectInfo( objName);
		}

		public override void InitAd(string source)
		{
			AdInsert_SetCallbackUnity(CallbackAdFinish);
			 AdInsert_InitAd( source);
		}

		public override void ShowAd()
		{
			 AdInsert_ShowAd();
		}
		 
		 //c#回调函数 
	[MonoPInvokeCallback(typeof (AdFinishCallbackFuction))]
		static void CallbackAdFinish(string source,string method)
	{
		AdKitCommon.main.AdInsertCallbackUnity( source,method);
	}
	}
}

#endif