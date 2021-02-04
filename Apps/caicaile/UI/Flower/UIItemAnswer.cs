
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Moonma.Share;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIItemAnswer : UIView
{
    public UIImage imageBg;
    public UIText textTitle;
    public string word;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        textTitle.gameObject.SetActive(false);
        LayOut();
    }


    public override void LayOut()
    {
        base.LayOut();
    }
    public void UpdateItem(string wd)
    {
        textTitle.text = wd;
        word = wd;
    }
    public void Show()
    {
        textTitle.gameObject.SetActive(true);
    }


}
