using UnityEngine;
using System.Collections;
using Moonma.Media;

public class TTSApiBase
{
    public string textSpeak;
    public virtual string GetTextUrl(string text)
    {
        return "";
    }
    public virtual void SpeakWeb(string text)
    {

    }


    public void PlayUrl(string url)
    {
        if (Common.isWinUWP)
        {
            MediaPlayer.main.callbackMediaPlayerEvent = OnMediaPlayerEvent;
            MediaPlayer.main.Open(url);
        }
        else
        {
            AudioPlay.main.PlayUrl(url);
        }
    }
    public void OnMediaPlayerEvent(string status)
    {
        if (status == MediaPlayer.EVENT_COMPLETE)
        {
            if (TTSCommon.main != null)
            {
                TTSCommon.main.TTSSpeechDidFinish("");
            }
        }
    }

}

