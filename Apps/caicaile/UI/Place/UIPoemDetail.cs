using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIPoemDetail : UIItemDetail, ISegmentDelegate
{

    public UISegment uiSegment;
    public UITextView textView;
    public UIText textTitle;
    public UIText textDetail;
    public UIImage imageHead;

    public GameObject objLayoutBtn;

    int indexSegment;

    /// <summary>
    /// Unity's Awake method.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        textView.SetFontSize(80);
        textView.SetTextColor(GameRes.main.colorGameWinTextView);

        textTitle.color = GameRes.main.colorGameWinTitle;


        indexSegment = 0;
        uiSegment.InitValue(64, Color.red, Color.black);
        uiSegment.iDelegate = this;
        UpdateSegment();

        LayOut();
    }

    /// <summary>
    /// Unity's Start method.
    /// </summary>
    protected override void Start()
    {
        base.Start();
        LayOut();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void UpdateSegment()
    {

        //原文
        {
            ItemInfo infoSeg = new ItemInfo();
            infoSeg.id = KEY_GAMEWIN_INFO_YUANWEN;
            infoSeg.title = Language.main.GetString(infoSeg.id);
            uiSegment.AddItem(infoSeg);
        }

        //原文拼音
        {
            ItemInfo infoSeg = new ItemInfo();
            infoSeg.id = KEY_GAMEWIN_ContentPinYin;
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
        // 作者简介
        {
            ItemInfo infoSeg = new ItemInfo();
            infoSeg.id = KEY_GAMEWIN_INFO_AUTHOR_INTRO;
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
            ratio = 0.9f;
        }

        RectTransform rctranRoot = this.GetComponent<RectTransform>();
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        {

            w = sizeCanvas.x * ratio;
            h = sizeCanvas.y * ratio;//rctran.rect.size.y * w / rctran.rect.size.x;
            rctranRoot.sizeDelta = new Vector2(w, h);
            base.LayOut();
        }
        float w_btns_landscape = 420;
        float space = 32f;
        // //textView
        // {
        //     RectTransform rctran = textView.GetComponent<RectTransform>();
        //     float oftTop = 0;
        //     float oftBottom = 0;
        //     float oftLeft = 0;
        //     float oftRight = 0;
        //     if (Device.isLandscape)
        //     {
        //         oftLeft = space;
        //         oftRight = w_btns_landscape + space;
        //         oftTop = 400;
        //         oftBottom = space;
        //     }
        //     else
        //     {
        //         oftLeft = space;
        //         oftRight = space;
        //         oftTop = 400;
        //         oftBottom = 200;
        //     }
        //     w = rctranRoot.rect.width - oftLeft - oftRight;
        //     h = rctranRoot.rect.height - oftTop - oftBottom;
        //     x = ((-rctranRoot.rect.width / 2 + oftLeft) + (rctranRoot.rect.width / 2 - oftRight)) / 2;
        //     y = ((-rctranRoot.rect.height / 2 + oftBottom) + (rctranRoot.rect.height / 2 - oftTop)) / 2;
        //     rctran.sizeDelta = new Vector2(w, h);
        //     rctran.anchoredPosition = new Vector2(x, y);
        //     textView.LayOut();
        // }

        {
            // RectTransform rctran = objLayoutBtn.GetComponent<RectTransform>();
            // if (Device.isLandscape)
            // {
            //     w = w_btns_landscape;
            //     h = rctranRoot.rect.height;
            //     y = 0;
            //     x = rctranRoot.rect.width / 2 - w / 2 - space;
            // }
            // else
            // {
            //     w = rctranRoot.rect.width;
            //     h = 160;
            //     x = 0;
            //     y = -rctranRoot.rect.height / 2 + h / 2 + space;
            // }
            // rctran.sizeDelta = new Vector2(w, h);
            // rctran.anchoredPosition = new Vector2(x, y);


            LayOutGrid lg = objLayoutBtn.GetComponent<LayOutGrid>();
            lg.enableHide = false;
            int btn_count = lg.GetChildCount(false);
            // if (Device.isLandscape)
            // {
            //     lg.row = btn_count;
            //     lg.col = 1;
            // }
            // else
            {
                lg.row = 1;
                lg.col = btn_count;
            }
            lg.LayOut();
            base.LayOut();
        }

        // imageHead.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public override void UpdateItem(CaiCaiLeItemInfo info)
    {
        infoItem = info as CaiCaiLeItemInfo;
        GameLevelParse.main.ParseItem(infoItem);
        Debug.Log("UpdateItem ParseItem infoItem.id=" + infoItem.id + " info.pinyin=" + infoItem.pinyin + " info.title=" + infoItem.title);
        IdiomItemInfo dbInfo = infoItem.dbInfo as IdiomItemInfo;
        textTitle.text = dbInfo.title;
        textDetail.text = dbInfo.author + "|" + dbInfo.year;

        uiSegment.Select(indexSegment, true);
        UpdateLoveStatus();
    }

    public void UpdateText(ItemInfo info)
    {
        CaiCaiLeItemInfo infoGuanka = GameLevelParse.main.GetItemInfo();

        GameLevelParse.main.ParseItem(infoGuanka);

        IdiomItemInfo dbInfo = infoGuanka.dbInfo as IdiomItemInfo;

        string str = "";
        {
            if (info.id == KEY_GAMEWIN_ContentPinYin)
            {
                str = dbInfo.content_pinyin;
            }
            if (info.id == KEY_GAMEWIN_INFO_YUANWEN)
            {
                str = dbInfo.content;
            }
            if (info.id == KEY_GAMEWIN_INFO_TRANSLATION)
            {
                str = dbInfo.translation;
                if (Common.BlankString(str))
                {
                    str = Language.main.GetString("STR_UNKNOWN_TRANSLATION");
                }
            }
            if (info.id == KEY_GAMEWIN_INFO_JIANSHUANG)
            {
                str = dbInfo.appreciation;
                if (Common.BlankString(str))
                {
                    str = Language.main.GetString("STR_UNKNOWN_JIANSHUANG");
                }
            }
            if (info.id == KEY_GAMEWIN_INFO_AUTHOR_INTRO)
            {
                str = dbInfo.authorDetail;
                if (Common.BlankString(str))
                {
                    str = Language.main.GetString("STR_UNKNOWN");
                }
            }

        }

        if (Common.BlankString(str))
        {
            str = Language.main.GetString("STR_UIVIEWALERT_MSG_GAME_FINISH");
        }
        textView.text = str;
    }

    public void SegmentDidClick(UISegment seg, SegmentItem item)
    {
        // UpdateSortList(item.index);
        UpdateText(item.infoItem);

    }

}
