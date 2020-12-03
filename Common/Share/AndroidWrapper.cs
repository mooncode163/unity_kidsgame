
#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
using System.Collections;

namespace Moonma.Share
{
internal class AndroidWrapper : BasePlatformWrapper
	{
		public const string JAVA_CLASS = "com.moonma.common.Share";
        public override void Init(string source,string appId,string appKey)
		{  
				using(var javaClass = new AndroidJavaClass(JAVA_CLASS))
				{
					javaClass.CallStatic("ShareInit",source,appId,appKey);
				}
		}
 
	public override void SetObjectInfo(string objName)
		{
			  using(var javaClass = new AndroidJavaClass(JAVA_CLASS))
				{
					javaClass.CallStatic("SetObjectInfo", objName);
				}
		}

		public override void InitPlatform(string source,string appId,string appKey)
		{
			  using(var javaClass = new AndroidJavaClass(JAVA_CLASS))
				{
					javaClass.CallStatic("InitPlatform", source,appId,appKey);
				}
		}


		public override void ShareWeb(string source,string title,string detail,string url)
		{
			  using(var javaClass = new AndroidJavaClass(JAVA_CLASS))
				{
					javaClass.CallStatic("ShareWeb", source,title,detail,url);
				}
		} 

		 public override void ShareImage(string source, string pic,string url)
        {
             using(var javaClass = new AndroidJavaClass(JAVA_CLASS))
				{
					javaClass.CallStatic("ShareImage", source,pic,url);
				}
        }
		 public override void ShareImageText(string source, string title, string pic,string url)
        {
             using(var javaClass = new AndroidJavaClass(JAVA_CLASS))
				{
					javaClass.CallStatic("ShareImageText", source,title,pic,url);
				}
        }
		 

    }
}
#endif
