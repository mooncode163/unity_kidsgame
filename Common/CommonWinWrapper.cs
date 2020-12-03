#if UNITY_WSA && !UNITY_EDITOR
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections;
 
	internal class CommonWinWrapper : CommonBasePlatformWrapper
	{
	 
		[DllImport ("Common")]
		public static extern string Common_GetAppName();
		[DllImport ("Common")]
		public static extern string Common_GetAppPackage();
 		[DllImport ("Common")]
		public static extern string Common_GetAppVerssion();

 
		// [DllImport ("Common")]
		// public static extern void Common_SetOrientation(int orientaion);
 
		

		public override string GetAppName()
		 {
			return Common_GetAppName();
		 }	
	
		  public override string GetAppPackage()
		 {

			return Common_GetAppPackage(); 
		 }	

  		public override string GetAppVersion()
		 {

			return Common_GetAppVerssion();
		 }	

 	public override string GetChannelName()
    {
        return "win";
    }

	
  	 public override void SetOrientation(int orientaion)
	{
//	Common_SetOrientation(orientaion);
	}

 

	}

#endif