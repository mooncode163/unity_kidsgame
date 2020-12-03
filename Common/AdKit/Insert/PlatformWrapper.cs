using UnityEngine;
using System.Collections;

namespace Moonma.AdKit.AdInsert
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

		public virtual void SetObjectInfo(string objName)
		{ 
		}
		public virtual void InitAd(string source)
		{
			 
		}

		public virtual void ShowAd()
		{
			 
		}
    }
}