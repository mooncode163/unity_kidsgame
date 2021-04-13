
#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
using System.Collections;

namespace Moonma.SysImageLib
{
internal class AndroidWrapper : BasePlatformWrapper
	{ 
   // public const string JAVA_CLASS = "com.moonma.common.ImageUtil"; 
    public const string JAVA_CLASS = "com.moonma.common.ImageSelectUnity"; 
	  public override void SetObjectInfo(string objName, string objMethod)
        { 
            using(var javaClass = new AndroidJavaClass(JAVA_CLASS))
				{
					javaClass.CallStatic("UnitySetObjectInfo",objName,objMethod);
				}
        }

        public override void OpenImage()
        {
   				using(var javaClass = new AndroidJavaClass(JAVA_CLASS))
				{
					javaClass.CallStatic("UnityOpenImage");
				}
        }   
           public override void OpenCamera()
        {
   				using(var javaClass = new AndroidJavaClass(JAVA_CLASS))
				{
					javaClass.CallStatic("UnityOpenCamera");
				}
        }  


    }
}
#endif
