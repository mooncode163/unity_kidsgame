#if UNITY_IPHONE && !UNITY_EDITOR
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections;
 
namespace Moonma.Pay
{
	internal class iOSWrapper : BasePlatformWrapper
	{
		 
		[DllImport ("__Internal")]
		public static extern void Pay_Init(string source,string appId,string appKey);
		[DllImport ("__Internal")]
		public static extern void Pay_PayInfo(string title,string pic); 

		public override void Init(string source,string appId,string appKey)
		{
			 Pay_Init( source,appId,appKey);
		}

		public override void PayInfo(string title,string pic)
		{
			 Pay_PayInfo(title,pic);
		}
		 
	}
}

#endif