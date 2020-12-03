
#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
using System.Collections;

namespace Moonma.Tongji
{
internal class TongjiAndroidWrapper : TongjiBasePlatformWrapper
	{
		public const string JAVA_CLASS_AD = "com.moonma.common.CommonAd";
        public override void Init(string appKey)
		{ 
				// using(var javaClass = new AndroidJavaClass(JAVA_CLASS_AD))
				// {
				// 	javaClass.CallStatic("tongji_Init",appKey);
				// }
		}
 

    }
}
#endif
