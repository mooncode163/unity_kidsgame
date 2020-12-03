#if UNITY_WSA && !UNITY_EDITOR
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections;
using AOT;//MonoPInvokeCallback

namespace Moonma.AdKit.AdOfferWall
{
	internal class WinWrapper : BasePlatformWrapper
	{
		[DllImport ("Common")]
		public static extern void AdVideo_SetType(int type);	 
		[DllImport ("Common")]
		public static extern void AdVideo_InitAd(string source);
		[DllImport ("Common")]
		public static extern void AdVideo_PreLoad(string source);
		[DllImport ("Common")]
		public static extern void AdVideo_ShowAd();
		[DllImport ("Common")]
		public static extern void AdVideo_SetObjectInfo(string objName,string objMethod);
		[DllImport ("Common")]
		public static extern void AdVideo_OnClickAd();	 
	 	[DllImport ("Common")]
		private static extern void AdVideo_SetCallbackUnity(AdFinishCallbackFuction callback);


	 public delegate void AdFinishCallbackFuction(string source,string method);

	 	public override void SetObjectInfo(string objName, string objMethod)
        {
			AdVideo_SetObjectInfo( objName,objMethod);
        }

		public override void SetType(int type)
		{
			AdVideo_SetType(type);
		}
		public override void PreLoad(string source)
		{  
			AdVideo_PreLoad(source);
		}

		public override void InitAd(string source)
		{
			AdVideo_SetCallbackUnity(CallbackAdFinish);
			 AdVideo_InitAd( source);
		}

		public override void ShowAd()
		{
			 AdVideo_ShowAd();
		}
		
		public override void OnClickAd()
		{
			 AdVideo_OnClickAd();
		}
		 

		 		 //c#回调函数 
	[MonoPInvokeCallback(typeof (AdFinishCallbackFuction))]
		static void CallbackAdFinish(string source,string method)
	{ 
		AdKitCommon.main.AdVideoCallbackUnity( source,method);
	}

	}
}

#endif