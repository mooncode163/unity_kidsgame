
#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
using System.Collections;

namespace Moonma.IAP
{
internal class AndroidWrapper : BasePlatformWrapper
	{
		public const string JAVA_CLASS = "com.moonma.common.IAPCommon";
    
	      public override void SetAppKey(string key)
        {
          using(var javaClass = new AndroidJavaClass(JAVA_CLASS))
				{
					javaClass.CallStatic("SetAppKey",key);
				}
        }

	  public override void SetObjectInfo(string objName, string objMethod)
        { 
            using(var javaClass = new AndroidJavaClass(JAVA_CLASS))
				{
					javaClass.CallStatic("SetObjectInfo",objName,objMethod);
				}
        }
		public override void SetSource(string source)
		{
			using(var javaClass = new AndroidJavaClass(JAVA_CLASS))
				{
					javaClass.CallStatic("SetSource",source);
				}
			 
		}

	 public override void StartBuy(string product, bool isConsume)
		{
			using(var javaClass = new AndroidJavaClass(JAVA_CLASS))
				{
					javaClass.CallStatic("StartBuy",product,isConsume);
				}
			 
		}

		public override void RestoreBuy(string product)
		{
			using(var javaClass = new AndroidJavaClass(JAVA_CLASS))
				{
					javaClass.CallStatic("RestoreBuy",product);
				}
			  
		} 

    }
}
#endif
