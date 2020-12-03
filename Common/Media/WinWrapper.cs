#if UNITY_WSA && !UNITY_EDITOR
using UnityEngine;
//using System;
using System.Runtime.InteropServices;
using System.Collections;
using AOT;//MonoPInvokeCallback
 
 // c++调用 c#: https://blog.csdn.net/fg5823820/article/details/47865741
namespace Moonma.Media
{
	internal class WinWrapper : BasePlatformWrapper
	{
		 
		[DllImport ("Common")]
		public static extern void MediaPlayer_Open(string url);
		[DllImport ("Common")]
		public static extern void MediaPlayer_Close();
		[DllImport ("Common")]
		public static extern void MediaPlayer_Play();
		[DllImport ("Common")]
		public static extern void MediaPlayer_Pause();

		[DllImport ("Common")]
		private static extern void MediaPlayer_SetCallbackUnity(MediaPlayerCallbackFuction callback);

		public delegate void MediaPlayerCallbackFuction(string status);



	//static AdFinishCallbackFuction callback;
 
 		public override void Open(string url)
        {
			 MediaPlayer_Open( url);
			MediaPlayer_SetCallbackUnity(OnMediaPlayerCallBack);
        }

        public override void Close()
        {
 			MediaPlayer_Close();
        }

        public override void Play()
        {
			MediaPlayer_Play();
        }
        public override void Pause()
        {
			MediaPlayer_Pause();
        }

//c#回调函数 
	[MonoPInvokeCallback(typeof (MediaPlayerCallbackFuction))]
		static void OnMediaPlayerCallBack(string status)
	{
		Debug.Log ("OnMediaPlayerCallBack status="+status);
		MediaPlayer.main.OnMediaPlayerEvent(status);
	}

		 
	}
}

#endif