
#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
using System.Collections;

namespace Moonma.SysImageLib
{
internal class AndroidWrapper : BasePlatformWrapper
	{
		public const string JAVA_CLASS = "com.moonma.common.ImageUtil"; 


	  public override void SetObjectInfo(string objName, string objMethod)
        { 
            using(var javaClass = new AndroidJavaClass(JAVA_CLASS))
				{
					javaClass.CallStatic("SetObjectInfo",objName,objMethod);
				}
        }

        public override void OpenImage()
        {
   				using(var javaClass = new AndroidJavaClass(JAVA_CLASS))
				{
					javaClass.CallStatic("OpenSystemImageLib");
				}
        }   
           public override void OpenCamera()
        {
   				using(var javaClass = new AndroidJavaClass(JAVA_CLASS))
				{
					javaClass.CallStatic("OpenSystemCameraApp");
				}
        }  


    }
}
#endif
