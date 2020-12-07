using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;
public class UIWriteController : UIView, IUIWordWriteDelegate
{
    public UIWordWrite uiWordWrite;
    public UIText textTitle;
    public string gameId;
    public UIButton btnBack;
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


    }

    // Use this for initialization
    public void Start()
    {
        base.Start();
        listBtnMode.Add(btnModeAll);
        listBtnMode.Add(btnModeOne);
        listBtnMode.Add(btnModeNone);
        SelectBtn(btnModeAll);
        Debug.Log("UIWriteController LevelManager.main.gameLevel="+LevelManager.main.gameLevel);
        WordItemInfo info = GameLevelParse.main.GetItemInfo();
        uiWordWrite.UpdateItem(info);
        uiWordWrite.iDelegate = this;
        LayOut();

        Invoke("GotoModeDelay", 0.2f);

    }

    void LoadPrefab()
    {

    }
    public void ShowBtnBack(bool isShow)
    {
        btnBack.gameObject.SetActive(isShow);
    }
    public void GotoModeDelay()
    {
        OnMode(WriteViewController.main.mode);

        OnUIDidFinish();
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
    public void OnMode(UIWordWrite.Mode md)
    {
        uiWordWrite.GotoMode(md);
        // uiWriteTopBar.OnMode(md);
        // objImage.SetActive((md == UIWordWrite.Mode.FREE_WRITE) ? false : true);
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
        uiWordWrite.GotoMode(UIWordWrite.Mode.ALL_STROKE);
    }
    public void OnBtnClickModeOne()
    {
        uiWordWrite.GotoMode(UIWordWrite.Mode.ONE_STROKE);
    }
    public void OnBtnClickModeNone()
    {
        uiWordWrite.GotoMode(UIWordWrite.Mode.NO_STROKE);
    }

}
