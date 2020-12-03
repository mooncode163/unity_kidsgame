using UnityEngine;
using System.Collections;

namespace Moonma.Tongji
{
 internal class TongjiPlatformWrapper
	{
	public static TongjiBasePlatformWrapper platform
		{
			get {
				#if UNITY_ANDROID && !UNITY_EDITOR
				return new TongjiAndroidWrapper();
				#elif UNITY_IPHONE && !UNITY_EDITOR
				return new TongjiiOSWrapper();
				#else
				return new TongjiBasePlatformWrapper();
				#endif
			}
		}
    }


internal class TongjiBasePlatformWrapper  
	{


		public virtual void Init(string appKey)
		{
			 
		}
 
 
    }
}