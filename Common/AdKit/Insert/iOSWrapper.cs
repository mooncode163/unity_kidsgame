#if UNITY_IPHONE && !UNITY_EDITOR
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections;
 
namespace Moonma.AdKit.AdInsert
{
	internal class iOSWrapper : BasePlatformWrapper
	{
		 
		[DllImport ("__Internal")]
		public static extern void AdInsert_InitAd(string source);
		[DllImport ("__Internal")]
		public static extern void AdInsert_ShowAd();
		[DllImport ("__Internal")]
		public static extern void AdInsert_SetObjectInfo(string objName); 

		public override void SetObjectInfo(string objName)
		{
			 AdInsert_SetObjectInfo( objName);
		}

		public override void InitAd(string source)
		{
			 AdInsert_InitAd( source);
		}

		public override void ShowAd()
		{
			 AdInsert_ShowAd();
		}
		 
		 
	}
}

#endif