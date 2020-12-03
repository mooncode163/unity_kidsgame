#if UNITY_IPHONE && !UNITY_EDITOR
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections;
 
namespace Moonma.Media
{
	internal class iOSWrapper : BasePlatformWrapper
	{

        [DllImport ("__Internal")]
        public static extern void MediaPlayer_Open(string url);
        [DllImport ("__Internal")]
        public static extern void MediaPlayer_Play();
        [DllImport ("__Internal")]
        public static extern void MediaPlayer_Close();
        [DllImport ("__Internal")]
        public static extern void MediaPlayer_Pause();
   	public override void Open(string url)
        {
                MediaPlayer_Open(url);
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
	}
}

#endif