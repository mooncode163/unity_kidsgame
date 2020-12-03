
#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
using System.Collections;

namespace Moonma.AdKit.AdBanner
{
internal class AndroidWrapper : BasePlatformWrapper
	{
		public const string JAVA_CLASS_AD = "com.moonma.common.AdBannerCommon";
        public override void InitAd(string source)
		{ 
		Debug.Log("AndroidWrapper:InitAd");

				using(var javaClass = new AndroidJavaClass(JAVA_CLASS_AD))
				{
					Debug.Log("AndroidWrapper:InitAd CallStatic");
					javaClass.CallStatic("adBanner_setAd",source);
				}
		}

		public override void ShowAd(bool isShow)
		{
			 using(var javaClass = new AndroidJavaClass(JAVA_CLASS_AD))
				{
					javaClass.CallStatic("adBanner_show",isShow,0);
				}
		}

		 public override void SetScreenSize(int w,int h)
		{
			 
		}

    }
}
#endif
