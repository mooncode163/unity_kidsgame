
#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
using System.Collections;

namespace Moonma.AdKit.AdConfig
{
internal class AndroidWrapper : BasePlatformWrapper
	{
		public const string JAVA_CLASS_ADConfig = "com.moonma.common.CommonAd";
       
	      public override void SetEnableAdSplash(bool enable)
        {
			 using(var javaClass = new AndroidJavaClass(JAVA_CLASS_ADConfig))
				{
					javaClass.CallStatic("SetEnableAdSplash",enable);
				}
        }

	public override void InitPlatform(string source,int type,string appId,string appKey,string adKey)
		{ 
			  using(var javaClass = new AndroidJavaClass(JAVA_CLASS_ADConfig))
				{
					javaClass.CallStatic("InitPlatform",source,type,appId,appKey,adKey);
				}
		}

	     public override void SetNoAd()
        {
			 using(var javaClass = new AndroidJavaClass(JAVA_CLASS_ADConfig))
				{
					javaClass.CallStatic("SetNoAd");
				}
        }
		 public override void SetNoAdDay(int day)
         {
			 	using(var javaClass = new AndroidJavaClass(JAVA_CLASS_ADConfig))
				{
					javaClass.CallStatic("SetNoAdDay",day);
				}
         }

		public override void SetAdSource(int type,string source) 
		{
			//  using(var javaClass = new AndroidJavaClass(JAVA_CLASS_AD))
			// 	{
			// 		javaClass.CallStatic("AdConfig_SetAdSource",type,source);
			// 	}
		} 
			public override void SetAppId(string source,string appid)
		{ 
			//  using(var javaClass = new AndroidJavaClass(JAVA_CLASS_AD))
			// 	{
			// 		javaClass.CallStatic("AdConfig_SetAppId",source,appid);
			// 	}
		} 

		public override void SetAdKey(string source,int type,string key)
		{ 
			//  using(var javaClass = new AndroidJavaClass(JAVA_CLASS_AD))
			// 	{
			// 		javaClass.CallStatic("AdConfig_SetAdKey",source,type,key);
			// 	}
		} 

 		public override void AdSplashSetConfig(string source,string appId,string appKey,string type)
        {
          
			   using(var javaClass = new AndroidJavaClass(JAVA_CLASS_ADConfig))
				{
					javaClass.CallStatic("AdSplash_SetConfig",source,appId,appKey);
				}
        }

		 public override void AdSplashInsertSetConfig(string source,string appId,string appKey,string type)
        {
             
        }

    }
}
#endif
