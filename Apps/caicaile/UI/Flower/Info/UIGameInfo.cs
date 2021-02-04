using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class UIGameInfo : UIView

{
    public UIImage imageBg;
    public UITextView uiTextView;


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
        UpdateItem();
        LayOut();
        AdKitCommon.main.ShowAdBanner(false);
    }


    void UpdateItem()
    {


    }
    public void OnClickBtnBack()
    {
        PopViewController pop = (PopViewController)this.controller;
        if (pop != null)
        {
            pop.Close();
        }
        AdKitCommon.main.ShowAdBanner(true);
    }

    public override void LayOut()
    {
        base.LayOut();

    } 
}
