using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;
using UnityEngine;
 
public class AudioUtil: MonoBehaviour
{ 
    static public void PlayFileResource(string file)
    {
        Debug.Log("PlayFileResource:"+file);
         GameObject audioPlayer = GameObject.Find("AudioPlayer");
        if (audioPlayer != null)
        {
            Debug.Log("PlayFileResource:audioPlayer");
            AudioSource audioSource = audioPlayer.GetComponent<AudioSource>();
            AudioClip audioClip = AudioCache.main.Load(file);
            audioSource.PlayOneShot(audioClip);
        }
    }
     

}
