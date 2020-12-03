using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
//using uAudio;
using System;
using Moonma.Media;
public class MusicBgPlay : MonoBehaviour//, uAudio_backend.IAudioPlayer
{

    public static MusicBgPlay main;
    //public uAudioStreamer uAudioNet;


    //public uAudioStreamer_UI uAudioUI;

    private AudioSource audioSource;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        audioSource = this.gameObject.AddComponent<AudioSource>();
        MusicBgPlay.main = this;

    }
    // Use this for initialization
    void Start()
    {
        bool ret = Common.GetBool(AppString.STR_KEY_BACKGROUND_MUSIC);
        Debug.Log("MusicBgPlay Start");
        if (ret)
        {
            PlayMusicBg();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    //设置背景音乐
    public void SetMusicBg()
    {
        audioSource.clip = AudioCache.main.Load(AppRes.AUDIO_BG);
    }


    public void PlayMusicBg()
    {
        if (Common.isMonoPlayer)//isPC 
        {
            return;
        }
        SetMusicBg();
        Play();
    }

    public void Stop()
    {
        audioSource.Stop();
    }
    public void Play()
    {
        Debug.Log("AudioPlay play()");
        audioSource.Play();
    }

    public void Pause()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }
}
