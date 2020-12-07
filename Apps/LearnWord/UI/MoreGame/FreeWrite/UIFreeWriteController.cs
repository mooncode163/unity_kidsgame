using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;
public class UIFreeWriteController : UIView, IUIWordWriteDelegate
{
    public UIWordWrite uiWordWrite;
    public UIFreeWriteTopBar uiWriteTopBar;
    public UIText textTitle;  
    public UIColorBoard uiColorBoard;
    public UIColorInput uiColorInput;
    public UILineSetting uiLineSetting; 

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public void Awake()
    {
        base.Awake();
        LoadPrefab();

        {

            uiColorBoard.callBackClick = OnUIColorBoardDidClick;

        }
        {

            uiColorInput.callBackUpdateColor = OnUIColorInputUpdateColor;

        }

        {

            uiLineSetting.LINE_WIDTH_PIXSEL_MIN = 4;
            uiLineSetting.LINE_WIDTH_PIXSEL_MAX = 256;
            uiLineSetting.callBackSettingLineWidth = OnUILineSettingLineWidth;
        }
        UpdateColorSelect(uiWriteTopBar.colorWord);
    }

    // Use this for initialization
    public void Start()
    {
        base.Start(); 
        WordItemInfo info = GameLevelParse.main.GetItemInfo();
        Debug.Log("info.id=" + info.id);
        uiWordWrite.UpdateItem(info); 
        uiWordWrite.iDelegate = this;


        LayOut();


        Invoke("GotoModeDelay", 0.2f);

    }





    public void GotoModeDelay()
    {
        OnMode(UIWordWrite.Mode.FREE_WRITE);
        OnUIDidFinish();
    }



    void LoadPrefab()
    {

    }



    public override void LayOut()
    {
        base.LayOut();
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;


    }
  
    void UpdateColorSelect(Color color)
    {
        uiWriteTopBar.colorWord = color;
        uiWriteTopBar.btnColorInput.imageBg.image.color = color;
        uiWordWrite.UpdateColor(color);
    }

    public void OnMode(UIWordWrite.Mode md)
    {
        uiWordWrite.GotoMode(md);
        uiWriteTopBar.OnMode(md); 
        LayOut();
    }
    public void OnUIWordWriteDidWriteFinish(UIWordWrite ui)
    {
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            if (navi != null)
            {
                navi.Push(WriteFinishViewController.main);
            }
        }
    }
    public void OnUIWordWriteDidMode(UIWordWrite ui, UIWordWrite.Mode md)
    {
       
    }

    public void OnUIColorBoardDidClick(UIColorBoard ui, UIColorBoardCellItem item, bool isOutSide)
    {
        //  Debug.Log("OnUIColorBoardDidClick isOutSide=" + isOutSide + " item.color=" + item.color);
        if (isOutSide)
        {

        }
        else
        {
            UpdateColorSelect(item.color);
        }

        uiColorBoard.gameObject.SetActive(false);
    }
    public void OnUIColorInputUpdateColor(Color color)
    {
        UpdateColorSelect(color);
    }
    public void OnUILineSettingLineWidth(int width)
    {
        Debug.Log("OnUILineSettingLineWidth w=" + width);
        uiWordWrite.SetLineWidthPixsel(width);

    }

    public void OnBtnClickBack()
    {
        NaviViewController navi = this.controller.naviController;
        if (navi != null)
        {
            navi.Pop();
        }
    }

    public void OnBtnClickDeleteAll()
    {
        DBLove.main.ClearDB();
    }
   
 

}
