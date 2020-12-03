
#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
using System.Collections;

namespace Moonma.Pay
{
internal class AndroidWrapper : BasePlatformWrapper
	{
		public const string JAVA_CLASS = "com.moonma.common.pay";
        public override void Init(string source,string appId,string appKey)
		{  
				using(var javaClass = new AndroidJavaClass(JAVA_CLASS))
				{
					javaClass.CallStatic("pay_Init",source,appId,appKey);
				}
		}

		public override void PayInfo(string title,string pic)
		{
			 using(var javaClass = new AndroidJavaClass(JAVA_CLASS))
				{
					javaClass.CallStatic("pay_PayInfo",title,pic);
				}
		}

		 

    }
}
#endif
