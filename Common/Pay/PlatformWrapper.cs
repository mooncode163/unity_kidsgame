using UnityEngine;
using System.Collections;

namespace Moonma.Pay
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
				#else
				return new BasePlatformWrapper();
				#endif
			}
		}
    }


internal class BasePlatformWrapper  
	{


		public virtual void Init(string source,string appId,string appKey)
		{
			 
		}

		public virtual void PayInfo(string title,string pic)
		{
			 
		}
 
    }
}