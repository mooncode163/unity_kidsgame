#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
using System.Collections;

namespace Moonma.AdKit.AdNative
{
internal class AndroidWrapper : BasePlatformWrapper
	{
		public const string JAVA_CLASS_AD = "com.moonma.common.AdNativeCommon";

		 public override void SetObjectInfo(string objName, string objMethod)
        {
			using(var javaClass = new AndroidJavaClass(JAVA_CLASS_AD))
				{ 
					javaClass.CallStatic("adNative_SetObjectInfo",objName,objMethod);
				}
        }

        public override void InitAd(string source)
		{  
				using(var javaClass = new AndroidJavaClass(JAVA_CLASS_AD))
				{ 
					javaClass.CallStatic("adNative_setAd",source);
				}
		}

		public override void ShowAd()
		{
			 using(var javaClass = new AndroidJavaClass(JAVA_CLASS_AD))
				{
					javaClass.CallStatic("adNative_show");
				}
		}

			public override void OnClickAd()
		{
			 using(var javaClass = new AndroidJavaClass(JAVA_CLASS_AD))
				{
					javaClass.CallStatic("adNative_onClick");
				}
		}
    }
}
#endif
