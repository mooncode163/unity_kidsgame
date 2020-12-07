using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OldMoatGames;
public class UIWordGif : UIViewPop
{
    public Button btnClose;

    public AnimatedGifPlayer animatedGifPlayer;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        InitPlay();
    }
    // Use this for initialization
    void Start()
    {

    }


    public void Updateitem(WordItemInfo info)
    {
        GameLevelParse.main.ParseItem(info);

        animatedGifPlayer.FileName = info.dbInfo.gif;//"https://hanyu-word-gif.cdn.bcebos.com/bb3e43bce24154e4ea04e1887c296d7b8.gif";
        animatedGifPlayer.Init();
        Play();
    }
    public void OnClickBtnClose()
    {
        this.Close();

    }


    void InitPlay()
    {
        // Get the GIF player component 
        animatedGifPlayer.OverrideTimeScale = true;
        animatedGifPlayer.TimeScale = 100f;
        // Set the file to use. File has to be in StreamingAssets folder or a remote url (For example: http://www.example.com/example.gif).
        // animatedGifPlayer.FileName = "animatedGifPlayerExampe 3.gif";
        //animatedGifPlayer.FileName = "https://hanyu-word-gif.cdn.bcebos.com/bb3e43bce24154e4ea04e1887c296d7b8.gif";

        // Disable autoplay
        animatedGifPlayer.AutoPlay = true;

        // Add ready event to start play when GIF is ready to play
        animatedGifPlayer.OnReady += OnGifLoaded;

        // Add ready event for when loading has failed
        animatedGifPlayer.OnLoadError += OnGifLoadError;

        // Init the GIF player
        // animatedGifPlayer.Init();

    }

    private void OnGifLoaded()
    {
        //  PlayButton.interactable = true;

        Debug.Log("GIF size: width: " + animatedGifPlayer.Width + "px, height: " + animatedGifPlayer.Height + " px");
    }

    private void OnGifLoadError()
    {
        Debug.Log("Error Loading GIF");
    }

    public void Play()
    {
        // Start playing the GIF
        animatedGifPlayer.Play();

        // Disable the play button
        // PlayButton.interactable = false;

        // Enable the pause button
        //  PauseButton.interactable = true;
    }

    public void Pause()
    {
        // Stop playing the GIF
        animatedGifPlayer.Pause();

        // Enable the play button
        //  PlayButton.interactable = true;

        // Disable the pause button
        //   PauseButton.interactable = false;
    }

    public void OnDisable()
    {
        animatedGifPlayer.OnReady -= Play;
    }
}
