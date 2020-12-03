#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
using System.Collections;

namespace Moonma.AdKit.AdInsert
{
internal class AndroidWrapper : BasePlatformWrapper
	{
		public const string JAVA_CLASS_AD = "com.moonma.common.AdInsertCommon";
        public override void InitAd(string source)
		{ 
		Debug.Log("AndroidWrapper:InitAd");

				using(var javaClass = new AndroidJavaClass(JAVA_CLASS_AD))
				{
					Debug.Log("AndroidWrapper:InitAd CallStatic");
					javaClass.CallStatic("adIndsert_setAd",source);
				}
		}

		public override void ShowAd()
		{
			 using(var javaClass = new AndroidJavaClass(JAVA_CLASS_AD))
				{
					javaClass.CallStatic("adIndsert_show");
				}
		}

    }
}
#endif
