


#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
using System.Collections;
 namespace Moonma.Html.HtmlWebView
{ 
public class AndroidWrapper : BasePlatformWrapper
	{

	public const string JAVA_CLASS = "com.moonma.common.HtmlWebView"; 
    AndroidJavaClass javaClass; 

	public  void Init( ) 
    {
             if(javaClass==null)
		{ 
		    javaClass = new AndroidJavaClass(JAVA_CLASS);
		}
    }

 		public override void Load(string url)
        {
			     Init();
		if(javaClass!=null)
		{ 
		 	javaClass.CallStatic("Load",url);
		} 
        }

		public override void SetObjectInfo(string name,string methodFinish,string methodFail)
		{ 
			 Init();
				if(javaClass!=null)
		{ 
		 	javaClass.CallStatic("SetObjectInfo",name,methodFinish,methodFail);
		} 
				 
		}
 


    }

	}
#endif

