using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Moonma.AdKit.AdInsert;

public class UIItemDetail : UIViewPop, ISegmentDelegate
{
    public const string KEY_GAMEWIN_INFO_INTRO = "KEY_GAMEWIN_INFO_INTRO";
    public const string KEY_GAMEWIN_INFO_YUANWEN = "KEY_GAMEWIN_INFO_YUANWEN";

    public const string KEY_GAMEWIN_INFO_TRANSLATION = "KEY_GAMEWIN_INFO_TRANSLATION";
    public const string KEY_GAMEWIN_INFO_JIANSHUANG = "KEY_GAMEWIN_INFO_JIANSHUANG";
    public const string KEY_GAMEWIN_INFO_AUTHOR_INTRO = "KEY_GAMEWIN_INFO_AUTHOR_INTRO";


    public const string KEY_GAMEWIN_INFO_ALBUM = "KEY_GAMEWIN_INFO_ALBUM";


    public UISegment uiSegment;
    public UITextView textView;
    // public UIText textTitle;

    public UIWordTitle uiWordTitle;
    public UIImage imageBg;
    public UIImage imageHeadStar;
    public UIButton btnClose;

    public UIButton btnAdd;
    public GameObject objLayoutBtn;
    public GameObject objWordTitle;
    DBWordItemInfo infoItem;
    int indexSegment;

    /// <summary>
    /// Unity's Awake method.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        textView.SetFontSize(80);
        // textView.SetTextColor(GameRes.main.colorGameWinTextView);

        //textTitle.color = GameRes.main.colorGameWinTitle;
        //textPinyin.color = GameRes.main.colorGameWinTitle;


        indexSegment = 0;
        uiSegment.InitValue(64, Color.red, Color.black);
        uiSegment.iDelegate = this;
        UpdateSegment();


    }

    /// <summary>
    /// Unity's Start method.
    /// </summary>
    protected override void Start()
    {
        base.Start();
        if (Device.isLandscape)
        {
            {

                LayOutRelation ly = btnClose.GetComponent<LayOutRelation>();
                ly.align = LayOutBase.Align.RIGHT;
                RectTransform rctran = btnClose.GetComponent<RectTransform>();
                rctran.anchoredPosition = Vector2.zero;
                //btnClose.imageBg.UpdateImageByKey("DetailClose_H");
                //  btnClose.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90f));
            }

            {

                LayOutRelation ly = imageHeadStar.GetComponent<LayOutRelation>();
                ly.align = LayOutBase.Align.LEFT;
                RectTransform rctran = imageHeadStar.GetComponent<RectTransform>();
                rctran.anchoredPosition = Vector2.zero;
                // imageHeadStar.UpdateImageByKey("HeadStar_H");
                // imageHeadStar.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90f));
            }
        }
        LayOut();
        AdKitCommon.main.ShowAdBanner(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        AdKitCommon.main.ShowAdBanner(true);
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
        uiSegment.ShowItemImageBg(false);
    }
    public override void LayOut()
    {
        base.LayOut();
        float x = 0, y = 0, w = 0, h = 0;

    }

    public void UpdateItem(DBWordItemInfo info)
    {
        infoItem = info;
        // IdiomParser.main.ParseIdiomItem(infoItem);
        string str = info.title;
        if (Common.BlankString(str))
        {
            str = LanguageManager.main.languageGame.GetString(info.id);
        }
        // textTitle.text = str;
        uiWordTitle.UpdateItem(str, info.pinyin);
        uiSegment.Select(indexSegment, true);
        UpdateLoveStatus();
        ShowAdInsert();
    }

    public void UpdateLoveStatus()
    {
        string strBtn = "";
        if (DBLove.main.IsItemExist(infoItem))
        {
            strBtn = Language.main.GetString("STR_IdiomDetail_DELETE_LOVE");
        }
        else
        {
            strBtn = Language.main.GetString("STR_IdiomDetail_ADD_LOVE");
        }
        btnAdd.textTitle.text = strBtn;
    }


    public void UpdateText(ItemInfo info)
    {
        string str = "";
        if (infoItem == null)
        {
            return;
        } 
 

        if (Common.BlankString(str))
        {
            // str = Language.main.GetString("STR_UIVIEWALERT_MSG_GAME_FINISH");
        }
        textView.text = str;
    }
    public void ShowAdInsert()
    {
        AdKitCommon.main.InitAdInsert();
        AdKitCommon.main.ShowAdInsert(100);
        // GameManager.main.isShowGameAdInsert = true; 
    }
    public void SegmentDidClick(UISegment seg, SegmentItem item)
    {
        // UpdateSortList(item.index);
        UpdateText(item.infoItem);

    }
    public void OnClickBtnClose()
    {
        Close();
    }

    public void OnClickBtnAdd()
    {
        // Close();
        if (DBLove.main.IsItemExist(infoItem))
        {
            DBLove.main.DeleteItem(infoItem);
        }
        else
        {
            DBLove.main.AddItem(infoItem);
        }
        UpdateLoveStatus();
    }
}
