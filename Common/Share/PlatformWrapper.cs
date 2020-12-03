using UnityEngine;
using System.Collections;

namespace Moonma.Share
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
		public virtual void SetObjectInfo(string objName)
		{
			 
		}

		public virtual void InitPlatform(string source,string appId,string appKey)
		{
			 
		}


		public virtual void ShareWeb(string source,string title,string detail,string url)
		{
			 
		}

		 public virtual void ShareImage(string source, string pic,string url)
        {
             
        }
		 public virtual void ShareImageText(string source, string title, string pic,string url)
        {
             
        }
 
    }
}