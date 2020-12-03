#if UNITY_IPHONE && !UNITY_EDITOR
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections;
 
namespace Moonma.Share
{
	internal class iOSWrapper : BasePlatformWrapper
	{
		 
		[DllImport ("__Internal")]
		public static extern void Share_Init(string source,string appId,string appKey);
		[DllImport ("__Internal")]
		public static extern void Share_SetObjectInfo(string objName);
		[DllImport ("__Internal")]
		public static extern void Share_InitPlatform(string source,string appId,string appKey); 
  		[DllImport ("__Internal")]
		public static extern void Share_ShareWeb(string source,string title,string detail,string url); 
		[DllImport ("__Internal")]
		public static extern void Share_ShareImage(string source,string pic,string url);
		[DllImport ("__Internal")]
		public static extern void Share_ShareImageText(string source,string title,string pic,string url);

		public override void Init(string source,string appId,string appKey)
		{
			 Share_Init( source,appId,appKey);
		}

		public override void SetObjectInfo(string objName)
		{
			 Share_SetObjectInfo(objName);
		}

		public override void InitPlatform(string source,string appId,string appKey)
		{
			 Share_InitPlatform( source,appId,appKey);
		}


		public override void ShareWeb(string source,string title,string detail,string url)
		{
			 Share_ShareWeb(source,title,detail,url);
		}
		 public override void ShareImage(string source, string pic,string url)
        {
             Share_ShareImage(source,pic,url);
        }
		 public override void ShareImageText(string source, string title, string pic,string url)
        {
             Share_ShareImageText(source,title,pic,url);
        }
		 
	}
}

#endif