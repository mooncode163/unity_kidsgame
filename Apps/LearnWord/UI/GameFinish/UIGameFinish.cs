
using System.Collections;
using System.Collections.Generic;
using Moonma.Share;
using UnityEngine;
using UnityEngine.UI;
public class UIGameFinish : UIView
{
    public GameObject objContent;
    public Image imageBg;
    public Image imageBoard;
    public Image imageLogo;
    public Text textTitle;
    public Button btnShare;
    public Button btnComment;
    public Button btnContinue;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    { 
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        LayOut();
    }


    public override void LayOut()
    {
        base.LayOut();
        float x, y, w, h;
      

    }
 
}
