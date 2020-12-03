#if UNITY_WSA && !UNITY_EDITOR
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections;
 
namespace Moonma.AdKit.AdConfig
{
	internal class WinWrapper : BasePlatformWrapper
	{
		[DllImport ("Common")]
		public static extern void AdConfig_InitPlatform(string source,int type,string appId,string appKey, string adKey); 

		 [DllImport ("Common")]
		public static extern void AdConfig_SetNoAd();
		/* 
		[DllImport ("Common")]
		public static extern void AdConfig_SetAdSource(int type,string source);
		 [DllImport ("Common")]
		public static extern void AdConfig_SetAppId(string source,string appid);
		[DllImport ("Common")]
		public static extern void AdConfig_SetAdKey(string source,int type,string key);
  		[DllImport ("Common")]
		public static extern void AdConfig_SetConfig(int type, string source,string appid,string adkey);
   */

		public override void InitPlatform(string source,int type,string appId,string appKey,string adKey)
		{
			 AdConfig_InitPlatform(source,type,appId,appKey,adKey);
		}

		  public override void SetNoAd()
        {
			AdConfig_SetNoAd();
        }
		/* 
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
		 
		 */
	 
		 
	}
}

#endif