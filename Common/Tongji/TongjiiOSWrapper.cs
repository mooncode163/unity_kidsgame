#if UNITY_IPHONE && !UNITY_EDITOR
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections;
 
namespace Moonma.Tongji
{
	internal class TongjiiOSWrapper : TongjiBasePlatformWrapper
	{
		 
		[DllImport ("__Internal")]
		public static extern void Tongji_Init(string appKey);
		 
 
		public override void Init(string appKey)
		{
			 Tongji_Init(appKey);
		}

		 
		 
	}
}

#endif