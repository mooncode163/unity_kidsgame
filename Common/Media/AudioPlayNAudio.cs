using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking; 
using System;

//https://blog.csdn.net/qq992817263/article/details/54647741
public class AudioPlayNAudio : MonoBehaviour//, uAudio_backend.IAudioPlayer
{ 
    private AudioSource audioSource;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        audioSource = this.gameObject.AddComponent<AudioSource>();
  
 
    }
    // Use this for initialization
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Stop()
    {
       
    }
    public void Play()
    {
     
    }

    public void Pause()
    { 

    } 

    public void PlayFile(string audiofile)
    { 
    }


    public void PlayUrl(string url)
    {
      

    }
 
}
