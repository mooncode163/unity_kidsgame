#if UNITY_IPHONE && !UNITY_EDITOR
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections;
 
namespace Moonma.SysImageLib
{
	internal class iOSWrapper : BasePlatformWrapper
	{
		[DllImport ("__Internal")] 
		public static extern void ImageUtil_SetObjectInfo(string objName,string objMethod);
		[DllImport ("__Internal")]
		public static extern void ImageUtil_OpenSysImageLib(); 
		[DllImport ("__Internal")]
	 	public static extern void ImageUtil_OpenSysCamera(); 
	  	 
   		 public override void SetObjectInfo(string objName, string objMethod)
        { 
            ImageUtil_SetObjectInfo( objName,objMethod);
        }

       public override void OpenImage()
        {
   			ImageUtil_OpenSysImageLib();
        }   
           public override void OpenCamera()
        {
   			ImageUtil_OpenSysCamera();	 
        }  
	}
}

#endif