using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIIdiomDetail : UIItemDetail, ISegmentDelegate
{


    public UISegment uiSegment;
    public UITextView textView;
    public UIText textTitle;
    public UIText textPinyin;
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
        LayOut();
    }

    // Update is called once per frame
    void Update()
    {

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

    public override void UpdateItem(CaiCaiLeItemInfo info)
    {

        GameLevelParse.main.ParseItem(info);
        infoItem = info;
        Debug.Log("UpdateItem ParseItem infoItem.id=" + infoItem.id + " info.pinyin=" + infoItem.pinyin + " info.title=" + infoItem.title);
        IdiomItemInfo dbInfo = info.dbInfo as IdiomItemInfo;
        string str = dbInfo.title;
        if (Common.BlankString(str))
        {
            str = LanguageManager.main.languageGame.GetString(info.id);
        }
        textTitle.text = str;
        textPinyin.text = dbInfo.pronunciation;
        uiSegment.Select(indexSegment, true);
        UpdateLoveStatus();

    }


    public void UpdateText(ItemInfo info)
    {
     
        string str = "";
        if (infoItem == null)
        {
            return;
        }


            IdiomItemInfo dbInfo = infoItem.dbInfo as IdiomItemInfo; 

        if (info.id == KEY_GAMEWIN_INFO_TRANSLATION)
        {
            str = dbInfo.translation;
        }

        if (info.id == KEY_GAMEWIN_INFO_ALBUM)
        {
            str = dbInfo.album;
        }

        if (Common.BlankString(str))
        {
            // str = Language.main.GetString("STR_UIVIEWALERT_MSG_GAME_FINISH");
        }
        textView.text = str;
    }

    public void SegmentDidClick(UISegment seg, SegmentItem item)
    {
        // UpdateSortList(item.index);
        UpdateText(item.infoItem);

    }
}
