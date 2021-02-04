using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIGameWinIdiom : UIGameWinBase, ISegmentDelegate
{ 
  

    /// <summary>
    /// Unity's Awake method.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        CaiCaiLeItemInfo info = GameLevelParse.main.GetItemInfo();
 

        textView.SetFontSize(80);
    
        if (textPinyin != null)
        {
            textPinyin.gameObject.SetActive(false);
        }

        indexSegment = 0;
        uiSegment.InitValue(64, Color.red, Color.black);
        uiSegment.iDelegate = this;

        uiSegment.gameObject.SetActive(true);
        textPinyin.gameObject.SetActive(true);
        // UpdateText(null);
    }

    /// <summary>
    /// Unity's Start method.
    /// </summary>
    protected override void Start()
    {
        base.Start();
        UpdateSegment();
        LayOut();
    }

 

    public void UpdateSegment()
    {

        //翻译
        {
            ItemInfo infoSeg = new ItemInfo();
            infoSeg.id = KEY_GAMEWIN_INFO_TRANSLATION;
            infoSeg.title = Language.main.GetString(infoSeg.id);
            uiSegment.AddItem(infoSeg);
        }

        //出处
        {
            ItemInfo infoSeg = new ItemInfo();
            infoSeg.id = KEY_GAMEWIN_INFO_ALBUM;
            infoSeg.title = Language.main.GetString(infoSeg.id);
            uiSegment.AddItem(infoSeg);
        }

        if (uiSegment != null)
        {
            uiSegment.UpdateList();
        }
        uiSegment.Select(indexSegment, true);
    }
    public override void LayOut()
    {
        base.LayOut();
        float x = 0, y = 0, w = 0, h = 0;
        float ratio = 0.8f;
        if (Device.isLandscape)
        {
            ratio = 0.8f;
        }

        RectTransform rctranRoot = this.GetComponent<RectTransform>();
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        {

            w = sizeCanvas.x * ratio;
            h = sizeCanvas.y * ratio;//rctran.rect.size.y * w / rctran.rect.size.x;
            rctranRoot.sizeDelta = new Vector2(w, h);

        }
        base.LayOut();

        //objLayoutBtn
        {

            LayOutGrid lg = objLayoutBtn.GetComponent<LayOutGrid>();
            lg.enableHide = false;
            int btn_count = lg.GetChildCount(false);
            if (Device.isLandscape)
            {
                lg.row = btn_count;
                lg.col = 1;
            }
            else
            {
                lg.row = 1;
                lg.col = btn_count;
            }
            lg.LayOut();
        }
        base.LayOut();
        // imageHead.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void UpdateText(ItemInfo info)
    {
        CaiCaiLeItemInfo infoGuanka = GameLevelParse.main.GetItemInfo();
        GameLevelParse.main.ParseItem(infoGuanka);
        infoItem = infoGuanka;

        IdiomItemInfo dbInfo = infoGuanka.dbInfo as IdiomItemInfo;

        string str = "";
        textTitle.text = dbInfo.title;
        //         public string author;
        // public string year;
        // public string style;
        // public string album;
        // public string intro;
        // public string translation;
        // public string appreciation;
        // public List<PoemContentInfo> listPoemContent;
        // if (Common.appKeyName == GameRes.GAME_POEM)
        // {
        //     if (info.id == KEY_GAMEWIN_INFO_INTRO)
        //     {

        //         string strtmp = infoGuanka.author;
        //         if (Common.BlankString(strtmp))
        //         {
        //             strtmp = Language.main.GetString("STR_UNKNOWN");
        //         }

        //         string strAuthor = Language.main.GetString("STR_AUTHOR") + ":" + strtmp + "    ";



        //         strtmp = infoGuanka.album;
        //         if (Common.BlankString(strtmp))
        //         {
        //             strtmp = Language.main.GetString("STR_UNKNOWN");
        //         }
        //         string strAlbum = Language.main.GetString("STR_ALBUM") + ":" + strtmp + "\n";


        //         strtmp = infoGuanka.year;
        //         if (Common.BlankString(strtmp))
        //         {
        //             strtmp = Language.main.GetString("STR_UNKNOWN");
        //         }
        //         string strYear = Language.main.GetString("STR_YEAR") + ":" + strtmp + "    ";


        //         strtmp = infoGuanka.style;
        //         if (Common.BlankString(strtmp))
        //         {
        //             strtmp = Language.main.GetString("STR_UNKNOWN");
        //         }
        //         string strStyle = Language.main.GetString("STR_STYLE") + ":" + strtmp + "\n";



        //         str = strAuthor + strAlbum + strYear + strStyle + "\n" + infoGuanka.intro;
        //     }
        //     if (info.id == KEY_GAMEWIN_INFO_YUANWEN)
        //     {
        //         for (int i = 0; i < infoGuanka.listPoemContent.Count; i++)
        //         {
        //             PoemContentInfo infoPoem = infoGuanka.listPoemContent[i];
        //             str += infoPoem.content + "\n";
        //         }
        //     }
        //     if (info.id == KEY_GAMEWIN_INFO_TRANSLATION)
        //     {
        //         str = infoGuanka.translation;
        //         if (Common.BlankString(str))
        //         {
        //             str = Language.main.GetString("STR_UNKNOWN_TRANSLATION");
        //         }
        //     }
        //     if (info.id == KEY_GAMEWIN_INFO_JIANSHUANG)
        //     {
        //         str = infoGuanka.appreciation;
        //         if (Common.BlankString(str))
        //         {
        //             str = Language.main.GetString("STR_UNKNOWN_JIANSHUANG");
        //         }
        //     }

        //     if (info.id == KEY_GAMEWIN_INFO_ALBUM)
        //     {
        //         str = infoGuanka.album;
        //         if (Common.BlankString(str))
        //         {
        //             str = Language.main.GetString("STR_UNKNOWN_ALBUM");
        //         }
        //     }
        // } 
        if (info.id == KEY_GAMEWIN_INFO_TRANSLATION)
        {
            str = dbInfo.translation;
        }

        if (info.id == KEY_GAMEWIN_INFO_ALBUM)
        {
            str = dbInfo.album;
        }
        textPinyin.text = dbInfo.pronunciation;
        if (Common.BlankString(str))
        {
            str = Language.main.GetString("STR_UIVIEWALERT_MSG_GAME_FINISH");
        }
        textView.text = str;
    }

    public void UpdateLoveStatus()
    {
        string strBtn = "";
        IdiomItemInfo infoidiom = infoItem.dbInfo as IdiomItemInfo;

        if (DBLoveIdiom.main.IsItemExistId(infoidiom.id))
        {
            strBtn = Language.main.GetString("STR_Detail_DELETE_LOVE");
        }
        else
        {
            strBtn = Language.main.GetString("STR_Detail_ADD_LOVE");
        } 
        btnAddLove.textTitle.text = strBtn;
    }

    public void SegmentDidClick(UISegment seg, SegmentItem item)
    {
        // UpdateSortList(item.index);
        UpdateText(item.infoItem);

    }
    public void OnClickBtnClose()
    {
        Close();
        GameManager.main.GotoPlayAgain();
    }
    public void OnClickBtnFriend()
    {
    }
    public void OnClickBtnNext()
    {
        Close();
        LevelManager.main.GotoNextLevel();
    }
    public void OnClickBtnAddLove()
    {
        IdiomItemInfo infoidiom = infoItem.dbInfo as IdiomItemInfo;
        if (DBLoveIdiom.main.IsItemExistId(infoidiom.id))
        {
            DBLoveIdiom.main.DeleteItemId(infoidiom.id);
        }
        else
        {
            DBLoveIdiom.main.AddItem(infoidiom);
        }
        UpdateLoveStatus();
    }
}
