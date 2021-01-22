using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;
public class UIWriteController : UIView, IUIWordWriteDelegate
{
    public UIWordWrite uiWordWrite;
    public UIWriteTopBar uiWriteTopBar;
    public UIText textTitle;
    public string gameId;

    public UIButton btnModeAll;
    public UIButton btnModeOne;
    public UIButton btnModeNone;
    public UIImage uiImageWord;
    public UIColorBoard uiColorBoard;
    public UIColorInput uiColorInput;
    public UILineSetting uiLineSetting;
    public GameObject objImage;
    List<UIButton> listBtnMode = new List<UIButton>();

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
        listBtnMode.Add(btnModeAll);
        listBtnMode.Add(btnModeOne);
        listBtnMode.Add(btnModeNone);
        SelectBtn(btnModeAll);
        WordItemInfo info = GameLevelParse.main.GetItemInfo();
        Debug.Log("info.id=" + info.id);
        uiWordWrite.UpdateItem(info);
        uiImageWord.UpdateImage(info.pic);
        uiWordWrite.iDelegate = this;


        LayOut();


        Invoke("GotoModeDelay", 0.2f);

    }





    public void GotoModeDelay()
    {
        OnMode(WriteViewController.main.mode);

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

    public void SetBtnGray(UIButton btn)
    {
        btn.imageBg.UpdateImageByKey("BtnBgGrey");
        btn.textTitle.color = new Color32(100, 100, 100, 255);
    }

    public void SelectBtn(UIButton btn)
    {
        SetBtnGray(btnModeAll);
        SetBtnGray(btnModeOne);
        SetBtnGray(btnModeNone);

        btn.imageBg.UpdateImageByKey("BtnBg");
        btn.textTitle.color = Color.white;

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
        objImage.SetActive((md == UIWordWrite.Mode.FREE_WRITE) ? false : true);
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
        switch (md)
        {
            case UIWordWrite.Mode.ALL_STROKE:
                {
                    SelectBtn(btnModeAll);
                }
                break;
            case UIWordWrite.Mode.ONE_STROKE:
                {
                    SelectBtn(btnModeOne);
                }
                break;
            case UIWordWrite.Mode.NO_STROKE:
                {
                    SelectBtn(btnModeNone);
                }
                break;
        }
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
    public void OnBtnClickModeAll()
    {
        OnMode(UIWordWrite.Mode.ALL_STROKE);
    }
    public void OnBtnClickModeOne()
    {
        OnMode(UIWordWrite.Mode.ONE_STROKE);
    }
    public void OnBtnClickModeNone()
    {
        OnMode(UIWordWrite.Mode.NO_STROKE);
    }



    public void OnClickBtnDemo()
    {

        // OnMode(UIWordWrite.Mode.DEMO);

        PopUpManager.main.Show<UIViewPop>(ConfigPrefab.main.GetPrefab("UIWordDemo"), popup =>
        {
            Debug.Log("UIViewAlert Open ");
            UIWordDemo ui = popup as UIWordDemo;
            WordItemInfo info = GameLevelParse.main.GetItemInfo();
            ui.Updateitem(info);
        }, popup =>
        {


        });

    }
    //笔画示意图
    public void OnClickBtnBihua()
    {

        PopUpManager.main.Show<UIViewPop>(ConfigPrefab.main.GetPrefab("UIWordBihuaShow"), popup =>
        {
            Debug.Log("UIViewAlert Open ");
            UIWordBihuaShow ui = popup as UIWordBihuaShow;
            WordItemInfo info = GameLevelParse.main.GetItemInfo();
            ui.Updateitem(info);
        }, popup =>
        {


        });
        //   gameXieHanzi.GotoWordWriteMode(WordWriteMode.ShowBihua);


    }
    public void OnClickBtnSound()
    {
        WordItemInfo info = GameLevelParse.main.GetItemInfo();
        if (info == null)
        {
            return;
        }
        TTS.main.Speak(info.id);
        //  PlaySoundFromResource(info.soundPutonghua);
    }

}
