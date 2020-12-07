using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;


public class UISearch : UIView, IUIInputBarDelegate
{
    public UIInputBar uiInputBar;
    public UIImage imageBg;
    public UIItemList uiItemList;


    public void Awake()
    {
        base.Awake();
        uiInputBar.iDelegate = this;


    }

    // Use this for initialization
    public void Start()
    {
        base.Start();
        LayOut();
        if (GameManager.main.isLoadGameScreenShot)
        {
            uiInputBar.text = "ren";
            OnUIInputBarEnd(uiInputBar);
        }
        OnUIDidFinish(0.5f);
    }

    public override void LayOut()
    {
        base.LayOut();
    }

    public void OnClickBtnBack()
    {
        // if (this.controller != null)
        // {
        //     PopViewController p = this.controller as PopViewController;
        //     p.Close();
        // }
        NaviViewController navi = this.controller.naviController;
        if (navi != null)
        {
            navi.Pop();
        }
    }

    public void OnUIInputBarValueChanged(UIInputBar ui)
    {
    }

    public void OnUIInputBarEnd(UIInputBar ui)
    {
        if (Common.BlankString(ui.text))
        {
            return;
        }
        List<object> ls = DBWord.main.Search(ui.text);
        uiItemList.SetList(ls);
        // ui.text = "T";
        Debug.Log("OnUIInputBarEnd text=" + ui.text + " count=" + ls.Count);
        uiItemList.UpdateList();
    }




}
