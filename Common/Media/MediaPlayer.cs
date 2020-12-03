using UnityEngine;
using System.Collections;
namespace Moonma.Media
{

    public delegate void OnMediaPlayerEventCallBack(string status);
    public class MediaPlayer
    {

        public const string EVENT_OPEN = "media_open";

        public const string EVENT_COMPLETE = "media_complete"; //播放完成

        public OnMediaPlayerEventCallBack callbackMediaPlayerEvent { get; set; }
        BasePlatformWrapper platform;
        static private MediaPlayer _main = null;
        public static MediaPlayer main
        {
            get
            {
                if (_main == null)
                {
                    _main = new MediaPlayer();
                    _main.Init();
                }
                return _main;
            }
        }

        public void Init()
        {
            platform = PlatformWrapper.platform;
        }

        public void Open(string url)
        {
            if (platform != null)
            {
                platform.Open(url);
            }
        }

        public void Close()
        {
            if (platform != null)
            {
                platform.Close();
            }
        }

        public void Play()
        {
            if (platform != null)
            {
                platform.Play();
            }
        }
        public void Pause()
        {
            if (platform != null)
            {
                platform.Pause();
            }
        }


        //c++调用c#的回调
        public void OnMediaPlayerEvent(string status)
        {
            if (callbackMediaPlayerEvent != null)
            {
                callbackMediaPlayerEvent(status);
            }
        }

    }
}
