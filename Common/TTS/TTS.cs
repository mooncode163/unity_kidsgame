using UnityEngine;
using System.Collections;

public class TTS
{
    TTSApiBase ttsApi;

    static private TTS _main = null;
    public static TTS main
    {
        get
        {
            if (_main == null)
            {
                _main = new TTS();
                _main.Init();
            }
            return _main;
        }
    }

    public void Init()
    {
        ttsApi = new TTSApiBaidu();
    }

    public void Speak(string text)
    {
            if (MusicBgPlay.main == null)
            {
               return;
            }
        bool ret = Common.GetBool(AppString.STR_KEY_BACKGROUND_MUSIC);
        if (ret)
        {
            MusicBgPlay.main.Stop();
        }

        if (Common.isAndroid || Common.isiOS)
        {
            //TTSBasePlatformWrapper platformWrapper = TTSPlatformWrapper.platform;
            // platformWrapper.Speak(text);
            SpeakWeb(text);
        }
        else
        {
            SpeakWeb(text);
        }
    }

    public void SpeakWeb(string text)
    {
        if (ttsApi != null)
        {
            ttsApi.SpeakWeb(text);
        }
    }



}

