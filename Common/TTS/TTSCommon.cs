using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Moonma.IAP;

public class TTSCommon : MonoBehaviour
{

    public static TTSCommon main;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (main == null)
        {
            main = this;
        }

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    #region TTS
    public void TTSSpeechDidStart(string str)
    { 
    }
    public void TTSSpeechDidFinish(string str)
    {
        bool ret = Common.GetBool(AppString.STR_KEY_BACKGROUND_MUSIC);
        if (ret)
        {
            //恢复播放
            AudioPlay.main.Play();
        }
    }
    #endregion

}
