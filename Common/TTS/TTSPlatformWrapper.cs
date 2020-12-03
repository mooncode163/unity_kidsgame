using UnityEngine;
using System.Collections;

internal class TTSPlatformWrapper
{
    public static TTSBasePlatformWrapper platform
    {
        get
        {
#if UNITY_ANDROID && !UNITY_EDITOR
				return new TTSAndroidWrapper();
#elif UNITY_IPHONE && !UNITY_EDITOR
				return new TTSiOSWrapper();
#else
            return new TTSBasePlatformWrapper();
#endif
        }
    }
}


internal class TTSBasePlatformWrapper
{


    public virtual void Speak(string text)
    {

    } 
    
}
