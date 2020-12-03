using System.Collections;
using System.Collections.Generic;
using Moonma.Share;
using UnityEngine;
using UnityEngine.UI;
public delegate void OnUIWriteFinishDelegate(UIWriteFinish ui);
public class UIWriteFinish : UIView
{
    public UIImage imageBg;
    public UIImage imageWord;
    public UIImage imageWrite;
    public UIImage imageBar;
    public UIText textTitle;
    public Button btnBack;
    public Button btnHistory;
    public Button btnRewrite;
    public Button btnShare;
    public GameObject objBtnLayout;
    public GameObject objTopBar;

    public int gameLevelNow;
    WordItemInfo wordInfo;

    public OnUIWriteFinishDelegate callbackClose { get; set; }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {


        //bg

        // TextureUtil.UpdateImageTexture(imageBg, AppRes.IMAGE_GAME_BG, true);


        // {
        //     string str = Language.main.GetString(AppXieHanzi.STR_WORD_WRITE_FINISH_TITLE);
        //     textTitle.text = str;
        //     int fontsize = textTitle.fontSize;
        //     float str_w = Common.GetStringLength(str, AppString.STR_FONT_NAME, fontsize);
        //     RectTransform rctran = imageBar.transform as RectTransform;
        //     Vector2 sizeDelta = rctran.sizeDelta;
        //     float oft = 0;
        //     sizeDelta.x = str_w + fontsize + oft * 2;
        //     rctran.sizeDelta = sizeDelta;
        //     //rctran.anchoredPosition = new Vector2(sizeCanvas.x / 2, rctran.anchoredPosition.y);
        // }

        if (btnShare != null)
        {
            btnShare.gameObject.SetActive(Config.main.isHaveShare);
        }
    }

    // Use this for initialization
    void Start()
    {

        InitUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnClickBtnBack();
        }

    }

    void InitUI()
    {
        WordItemInfo info = GameLevelParse.main.GetItemInfo();
        UpdateItem(info);
        LayOut();
    }

    public override void LayOut()
    {
        base.LayOut();

    }
    public void UpdateItem(WordItemInfo info)
    {
        wordInfo = info;
        imageWord.UpdateImage(info.pic);
        imageWrite.UpdateImage(info.dbInfo.filesave); 
        gameLevelNow = LevelManager.main.gameLevel;
        LayOut();

    }
    void ShowShare()
    {

        ShareViewController.main.callBackClick = OnUIShareDidClick;
        ShareViewController.main.Show(null, null);
    }

    public void OnUIShareDidClick(ItemInfo item)
    {
        string title = Language.main.GetString("UIWRITE_FINISH_SHARE_TITLE");
        string detail = Language.main.GetString("UIWRITE_FINISH_SHARE_DETAIL");
        string url = Config.main.shareAppUrl;
        Share.main.ShareWeb(item.source, title, detail, url);
    }

    void OnClose()
    {
        if (callbackClose != null)
        {
            callbackClose(this);
        }
        // PopViewController pop = (PopViewController)this.controller;
        // if (pop != null)
        // {
        //     pop.Close();
        // }
         if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            if (navi != null)
            {
                navi.Pop();
            }
        }
    }
    public void OnClickBtnBack()
    {
        OnClose();
    }
    public void OnClickBtnHistory()
    {
        OnClose();
        // HistoryViewController.main.Show(null, null); 
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            // navi.source = AppRes.SOURCE_NAVI_HISTORY;
            navi.Push(HistoryViewController.main);
        }

    }
    public void OnClickBtnRewrite()
    {
        OnClose();
        LevelManager.main.gameLevel = gameLevelNow;
        // GameManager.main.GotoGame(GuankaViewController.main);
        WriteViewController.main.OnMode(UIWordWrite.Mode.ALL_STROKE);
    }

    public void OnClickBtnShare()
    {
        ShowShare();
    }

}
