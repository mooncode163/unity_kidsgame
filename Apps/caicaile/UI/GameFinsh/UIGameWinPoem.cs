using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIGameWinPoem : UIGameWinBase, ISegmentDelegate
{
    public const string KEY_GAMEWIN_INFO_INTRO = "KEY_GAMEWIN_INFO_INTRO";
    public const string KEY_GAMEWIN_INFO_YUANWEN = "KEY_GAMEWIN_INFO_YUANWEN";

    public const string KEY_GAMEWIN_INFO_TRANSLATION = "KEY_GAMEWIN_INFO_TRANSLATION";
    public const string KEY_GAMEWIN_INFO_JIANSHUANG = "KEY_GAMEWIN_INFO_JIANSHUANG";
    public const string KEY_GAMEWIN_INFO_AUTHOR_INTRO = "KEY_GAMEWIN_INFO_AUTHOR_INTRO";


    public const string KEY_GAMEWIN_INFO_ALBUM = "KEY_GAMEWIN_INFO_ALBUM";


    public UISegment uiSegment;
    public UITextView textView;
    public UIText textTitle;
    public UIText textPinyin;
    public UIImage imageBg;
    public UIImage imageHead;
    public UIButton btnClose;

    public UIButton btnFriend;
    public UIButton btnNext;
    public UIButton btnAddLove;
    public GameObject objLayoutBtn;


    int indexSegment;

    /// <summary>
    /// Unity's Awake method.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        CaiCaiLeItemInfo info = GameLevelParse.main.GetItemInfo();

        //Common.SetButtonText(btnFriend, Language.main.GetString("STR_GameWin_BtnFriend"));
        // Common.SetButtonText(btnNext, Language.main.GetString("STR_GameWin_BtnNext"), 0, false);
        //Common.SetButtonText(btnAddLove, Language.main.GetString("STR_GameWin_BtnAddLove"));

        string str = info.title;
        if (Common.BlankString(str))
        {
            str = LanguageManager.main.languageGame.GetString(info.id);
        }
        if (Common.appKeyName == GameRes.GAME_XIEHOUYU)
        {
            str = Language.main.GetString("STR_UIVIEWALERT_TITLE_GAME_FINISH");
        }
        if (Common.appKeyName == GameRes.GAME_RIDDLE)
        {
            str = Language.main.GetString("STR_UIVIEWALERT_TITLE_GAME_FINISH");
        }
        if (Common.appKeyName == GameRes.GAME_Image)
        {
            str = Language.main.GetString("STR_UIVIEWALERT_TITLE_GAME_FINISH");
        }
        textTitle.text = str;


        textView.SetFontSize(80);
        textView.SetTextColor(GameRes.main.colorGameWinTextView);

        textTitle.color = GameRes.main.colorGameWinTitle;
        // if (textPinyin != null)
        // {
        //     textPinyin.color = GameRes.main.colorGameWinTitle;
        //     textPinyin.gameObject.SetActive(false);
        // }

        indexSegment = 0;
        uiSegment.InitValue(64, Color.red, Color.black);
        uiSegment.iDelegate = this;

        uiSegment.gameObject.SetActive(true);

        if (Common.appKeyName == GameRes.GAME_POEM)
        {
            UpdateSegmentPoem();
        }
        else if (Common.appKeyName == GameRes.GAME_IDIOM)
        {
            uiSegment.gameObject.SetActive(false);
            // textPinyin.gameObject.SetActive(true);
            UpdateText(null);
        }
        else
        {
            UpdateText(null);
        }
        if (Common.appKeyName == GameRes.GAME_Image)
        {
            btnAddLove.gameObject.SetActive(false);
        }

    }

    /// <summary>
    /// Unity's Start method.
    /// </summary>
    protected override void Start()
    {
        base.Start();
        UpdateLoveStatus();
        LayOut();
    }
 
    public void UpdateSegmentPoem()
    {

        //简介
        {
            ItemInfo infoSeg = new ItemInfo();
            infoSeg.id = KEY_GAMEWIN_INFO_INTRO;
            infoSeg.title = Language.main.GetString(infoSeg.id);
            uiSegment.AddItem(infoSeg);
        }
        //原文
        {
            ItemInfo infoSeg = new ItemInfo();
            infoSeg.id = KEY_GAMEWIN_INFO_YUANWEN;
            infoSeg.title = Language.main.GetString(infoSeg.id);
            uiSegment.AddItem(infoSeg);
        }
        //翻译
        {
            ItemInfo infoSeg = new ItemInfo();
            infoSeg.id = KEY_GAMEWIN_INFO_TRANSLATION;
            infoSeg.title = Language.main.GetString(infoSeg.id);
            uiSegment.AddItem(infoSeg);
        }
        //赏析
        {
            ItemInfo infoSeg = new ItemInfo();
            infoSeg.id = KEY_GAMEWIN_INFO_JIANSHUANG;
            infoSeg.title = Language.main.GetString(infoSeg.id);
            uiSegment.AddItem(infoSeg);
        }
        //作者简介
        // {
        //     ItemInfo infoSeg = new ItemInfo();
        //     infoSeg.id = KEY_GAMEWIN_INFO_AUTHOR_INTRO;
        //     infoSeg.title = Language.main.GetString(infoSeg.id);
        //     uiSegment.AddItem(infoSeg);
        // }

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
             base.LayOut();

        } 
        
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
            lg.align = LayOutBase.Align.CENTER;
            lg.LayOut();
        }

        // imageHead.transform.localScale = new Vector3(1f, 1f, 1f);
        base.LayOut();
    }

    public void UpdateText(ItemInfo info)
    {
        CaiCaiLeItemInfo infoGuanka = GameLevelParse.main.GetItemInfo();
        string str = "";
        //         public string author;
        // public string year;
        // public string style;
        // public string album;
        // public string intro;
        // public string translation;
        // public string appreciation;
        // public List<PoemContentInfo> listPoemContent;
        if (Common.appKeyName == GameRes.GAME_POEM)
        {
            if (info.id == KEY_GAMEWIN_INFO_INTRO)
            {

                string strtmp = infoGuanka.author;
                if (Common.BlankString(strtmp))
                {
                    strtmp = Language.main.GetString("STR_UNKNOWN");
                }

                string strAuthor = Language.main.GetString("STR_AUTHOR") + ":" + strtmp + "    ";



                strtmp = infoGuanka.album;
                if (Common.BlankString(strtmp))
                {
                    strtmp = Language.main.GetString("STR_UNKNOWN");
                }
                string strAlbum = Language.main.GetString("STR_ALBUM") + ":" + strtmp + "\n";


                strtmp = infoGuanka.year;
                if (Common.BlankString(strtmp))
                {
                    strtmp = Language.main.GetString("STR_UNKNOWN");
                }
                string strYear = Language.main.GetString("STR_YEAR") + ":" + strtmp + "    ";


                strtmp = infoGuanka.style;
                if (Common.BlankString(strtmp))
                {
                    strtmp = Language.main.GetString("STR_UNKNOWN");
                }
                string strStyle = Language.main.GetString("STR_STYLE") + ":" + strtmp + "\n";



                str = strAuthor + strAlbum + strYear + strStyle + "\n" + infoGuanka.intro;
            }
            if (info.id == KEY_GAMEWIN_INFO_YUANWEN)
            {
                for (int i = 0; i < infoGuanka.listPoemContent.Count; i++)
                {
                    PoemContentInfo infoPoem = infoGuanka.listPoemContent[i];
                    str += infoPoem.content + "\n";
                }
            }
            if (info.id == KEY_GAMEWIN_INFO_TRANSLATION)
            {
                str = infoGuanka.translation;
                if (Common.BlankString(str))
                {
                    str = Language.main.GetString("STR_UNKNOWN_TRANSLATION");
                }
            }
            if (info.id == KEY_GAMEWIN_INFO_JIANSHUANG)
            {
                str = infoGuanka.appreciation;
                if (Common.BlankString(str))
                {
                    str = Language.main.GetString("STR_UNKNOWN_JIANSHUANG");
                }
            }

            if (info.id == KEY_GAMEWIN_INFO_ALBUM)
            {
                str = infoGuanka.album;
                if (Common.BlankString(str))
                {
                    str = Language.main.GetString("STR_UNKNOWN_ALBUM");
                }
            }
        }


        if (Common.appKeyName == GameRes.GAME_IDIOM)
        {
            textPinyin.text = infoGuanka.pinyin;
            str = Language.main.GetString(KEY_GAMEWIN_INFO_TRANSLATION) + ":" + infoGuanka.translation + "\n\n" + Language.main.GetString(KEY_GAMEWIN_INFO_ALBUM) + ":" + infoGuanka.album;
        }

        if (Common.appKeyName == GameRes.GAME_XIEHOUYU)
        {
            str = infoGuanka.head + "\n" + infoGuanka.end;
        }
        if (Common.appKeyName == GameRes.GAME_RIDDLE)
        {
            str = infoGuanka.tips;
            if (Common.BlankString(str))
            {
                str = Language.main.GetString("STR_UNKNOWN");
            }
            str = Language.main.GetString("STR_GameWin_TIPS") + ":" + str;
        }

        if (Common.BlankString(str))
        {
            str = Language.main.GetString("STR_UIVIEWALERT_MSG_GAME_FINISH");
        }
        textView.text = str;
    }
    public void UpdateLoveStatus()
    {
        CaiCaiLeItemInfo infoItem = GameLevelParse.main.GetItemInfo();
        string key = "";
        if (DBLove.main.IsItemExist(infoItem.dbInfo))
        {
            key = "STR_IdiomDetail_DELETE_LOVE";
        }
        else
        {
            key = "STR_IdiomDetail_ADD_LOVE";
        }

        // Common.SetButtonText(btnAddLove, strBtn, 0, false);
        btnAddLove.textTitle.UpdateTextByKey(key);
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
        CaiCaiLeItemInfo infoItem = GameLevelParse.main.GetItemInfo(); 
        if (DBLove.main.IsItemExist(infoItem.dbInfo))
        {
            DBLove.main.DeleteItem(infoItem.dbInfo);
        }
        else
        {
            DBLove.main.AddItem(infoItem.dbInfo);
        }
        UpdateLoveStatus();
    }
}
