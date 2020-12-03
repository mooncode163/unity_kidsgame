#if UNITY_IPHONE && !UNITY_EDITOR
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections;
 
namespace Moonma.AdKit.AdNative
{
	internal class iOSWrapper : BasePlatformWrapper
	{
		 
		[DllImport ("__Internal")]
		public static extern void AdNative_InitAd(string source);
		[DllImport ("__Internal")]
		public static extern void AdNative_ShowAd();
		[DllImport ("__Internal")]
		public static extern void AdNative_SetObjectInfo(string objName,string objMethod);
		[DllImport ("__Internal")]
		public static extern void AdNative_OnClickAd();	 
		[DllImport ("__Internal")]
		public static extern void AdNative_ShowSplash(string source);
		
	 	public override void SetObjectInfo(string objName, string objMethod)
        {
			AdNative_SetObjectInfo( objName,objMethod);
        }
		
		public override void InitAd(string source)
		{
			 AdNative_InitAd( source);
		}

		public override void ShowAd()
		{
			 AdNative_ShowAd();
		}
		
		public override void OnClickAd()
		{
			 AdNative_OnClickAd();
		}

		  public override void ShowSplash(string source)
        {
	 		AdNative_ShowSplash(source);
        }
		 
	}
}

#endif