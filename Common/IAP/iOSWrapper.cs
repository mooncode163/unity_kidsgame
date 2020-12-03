#if UNITY_IPHONE && !UNITY_EDITOR
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections;
 
namespace Moonma.IAP
{
	internal class iOSWrapper : BasePlatformWrapper
	{
		[DllImport ("__Internal")]
		public static extern void IAP_SetObjectInfo(string objName,string objMethod);
		[DllImport ("__Internal")]
		public static extern void IAP_SetSource(string source);
		[DllImport ("__Internal")]
		public static extern void IAP_StartBuy(string product);
		[DllImport ("__Internal")]
		public static extern void IAP_RestoreBuy(string product);
	 
	    public override void SetObjectInfo(string objName, string objMethod)
        { 
            IAP_SetObjectInfo( objName,objMethod);
        }

		 public override void SetSource(string source)
		{
			 IAP_SetSource(source);
		}

		 public override void StartBuy(string product, bool isConsume)
		{
			 IAP_StartBuy(product);
		}

		public override void RestoreBuy(string product)
		{
			  IAP_RestoreBuy(product);
		} 
	}
}

#endif