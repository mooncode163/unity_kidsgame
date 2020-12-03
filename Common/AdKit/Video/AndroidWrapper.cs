#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
using System.Collections;

namespace Moonma.AdKit.AdVideo
{
internal class AndroidWrapper : BasePlatformWrapper
	{
		public const string JAVA_CLASS_AD = "com.moonma.common.AdVideoCommon";

		 public override void SetObjectInfo(string objName, string objMethod)
        {
			using(var javaClass = new AndroidJavaClass(JAVA_CLASS_AD))
				{ 
					javaClass.CallStatic("setObjectInfo",objName,objMethod);
				}
        }
		public override void SetType(int type)
		{
			using(var javaClass = new AndroidJavaClass(JAVA_CLASS_AD))
				{ 
					javaClass.CallStatic("setType",type);
				}
		}

        public override void InitAd(string source)
		{  
				using(var javaClass = new AndroidJavaClass(JAVA_CLASS_AD))
				{ 
					javaClass.CallStatic("setAd",source);
				}
		}
	 
      public override void PreLoad(string source)
		{  
				using(var javaClass = new AndroidJavaClass(JAVA_CLASS_AD))
				{ 
					javaClass.CallStatic("PreLoad",source);
				}
		}
		public override void ShowAd()
		{
			 using(var javaClass = new AndroidJavaClass(JAVA_CLASS_AD))
				{
					javaClass.CallStatic("show");
				}
		}

			public override void OnClickAd()
		{
			 using(var javaClass = new AndroidJavaClass(JAVA_CLASS_AD))
				{
					javaClass.CallStatic("onClick");
				}
		}
    }
}
#endif
