#if UNITY_IPHONE && !UNITY_EDITOR
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections;
 
namespace Moonma.AdKit.AdConfig
{
	internal class iOSWrapper : BasePlatformWrapper
	{
		[DllImport ("__Internal")]
		public static extern void AdConfig_InitPlatform(string source,int type,string appId,string appKey, string adKey); 
 		[DllImport ("__Internal")]
		public static extern void AdConfig_InitSDK();
		 [DllImport ("__Internal")]
		public static extern void AdConfig_SetNoAd();
		[DllImport ("__Internal")]
		public static extern void AdConfig_SetAdSource(int type,string source);
		 [DllImport ("__Internal")]
		public static extern void AdConfig_SetAppId(string source,string appid);
		[DllImport ("__Internal")]
		public static extern void AdConfig_SetAdKey(string source,int type,string key);
  		[DllImport ("__Internal")]
		public static extern void AdConfig_SetConfig(int type, string source,string appid,string adkey);
  
  //开屏
	[DllImport ("__Internal")]
   	public static extern void AdSplash_SetConfig(string source,string appId,string appKey,string type);
	//开机插屏
[DllImport ("__Internal")]
   	public static extern void AdSplashInsert_SetConfig(string source,string appId,string appKey,string type);

[DllImport ("__Internal")]
   	public static extern void AdSplash_InitAd(string source,int type);

[DllImport ("__Internal")]
   	public static extern void AdSplash_Show();
  		public override void InitSDK()
        {
			AdConfig_InitSDK();
        }
		public override void InitPlatform(string source,int type,string appId,string appKey,string adKey)
		{
			 AdConfig_InitPlatform(source,type,appId,appKey,adKey);
		}

		  public override void SetNoAd()
        {
			AdConfig_SetNoAd();
        }
		public override void SetAdSource(int type,string source)
		{
			 AdConfig_SetAdSource(type,source);
		} 
		public override void SetAppId(string source,string appid)
		{
			 AdConfig_SetAppId(source,appid);
		} 

		public override void SetAdKey(string source,int type,string key)
		{
			AdConfig_SetAdKey(source,type,key);
		} 

		public override void SetConfig(int type, string source,string appid,string adkey)
        {
             AdConfig_SetConfig(type,source,appid,adkey);
        }
		 
		 public override void AdSplashSetConfig(string source,string appId,string appKey,string type)
        {
             AdSplash_SetConfig(source,appId,appKey,type);
        }

		 public override void AdSplashInsertSetConfig(string source,string appId,string appKey,string type)
        {
             AdSplashInsert_SetConfig(source,appId,appKey,type);
        }

		  public override void AdSplashInitAd(string source, int type)
        {
			AdSplash_InitAd(source,type);
        }
        public override void AdSplashShow()
        {
			AdSplash_Show();
        }
		

	}
}

#endif