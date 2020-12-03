#if UNITY_IPHONE && !UNITY_EDITOR
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections;
 
	internal class CommoniOSWrapper : CommonBasePlatformWrapper
	{
	 
		[DllImport ("__Internal")]
		public static extern string Common_GetAppName();
		[DllImport ("__Internal")]
		public static extern string Common_GetCachePath();
 		[DllImport ("__Internal")]
		public static extern void Common_EnableAdSplash();
		[DllImport ("__Internal")]
		public static extern void Common_SetIpInChina(bool isin);
		[DllImport ("__Internal")]
		public static extern void Common_SetOrientation(int orientaion);

		[DllImport ("__Internal")]
		public static extern bool Common_IsiPhoneX();
		[DllImport ("__Internal")]
		public static extern int Common_GetHeightSystemTopBar();
		[DllImport ("__Internal")]
		public static extern int Common_GetHeightSystemHomeBar();
		

		public override string GetAppName()
		 {
			return Common_GetAppName();
		 }	
		 public override string GetCachePath()
		 {

			return Common_GetCachePath(); 
		 }	

		  public override string GetAppPackage()
		 {

			return Application.identifier; 
		 }	

  		public override string GetAppVersion()
		 {

			return Application.version;
		 }	

 	public override string GetChannelName()
    {
        return "AppStore";
    }

		  public override void UnityStartUpFinish()
    {
           
    }
		 

		   public override void EnableAdSplash()
    {
         Common_EnableAdSplash();
    }

	   public override void SetIpInChina(bool isin)
    {
         Common_SetIpInChina(isin);
    }
	
  	 public override void SetOrientation(int orientaion)
	{
	Common_SetOrientation(orientaion);
	}


	public override bool isiPhoneX()
    {
        return Common_IsiPhoneX();
    }

 	public override int getHeightSystemTopBar()
    {
        return Common_GetHeightSystemTopBar();
    }
    public override int getHeightSystemHomeBar()
    {
        return Common_GetHeightSystemHomeBar();
    }

	}

#endif