#if UNITY_IPHONE && !UNITY_EDITOR
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections;
 
namespace Moonma.AdKit.AdVideo
{
	internal class iOSWrapper : BasePlatformWrapper
	{
		[DllImport ("__Internal")]
		public static extern void AdVideo_SetType(int type); 
		[DllImport ("__Internal")]
		public static extern void AdVideo_PreLoad(string source);	
		[DllImport ("__Internal")]
		public static extern void AdVideo_InitAd(string source);
		[DllImport ("__Internal")]
		public static extern void AdVideo_ShowAd();
		[DllImport ("__Internal")]
		public static extern void AdVideo_SetObjectInfo(string objName,string objMethod);
		[DllImport ("__Internal")]
		public static extern void AdVideo_OnClickAd();	 
	 
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
		 
	}
}

#endif