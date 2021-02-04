using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIDetailIdiom : UIDetailBase, ISegmentDelegate
{


    /// <summary>
    /// Unity's Awake method.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        CaiCaiLeItemInfo info = GameLevelParse.main.GetItemInfo();


        textView.SetFontSize(80);
        textView.text = "";
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
        infoItem = info;
        Invoke("UpdateItemInternal", 0.1f);
    }

    public void UpdateItemInternal()
    {
        if (uiSegment != null)
        {
            uiSegment.UpdateList();
        }
        uiSegment.Select(indexSegment, true);
    }
    public void UpdateText(ItemInfo info)
    {
        // CaiCaiLeItemInfo infoGuanka = GameLevelParse.main.GetItemInfo();
        GameLevelParse.main.ParseItem(infoItem);
        IdiomItemInfo dbInfo = infoItem.dbInfo as IdiomItemInfo;
        Debug.Log("UpdateText dbInfo.id=" + dbInfo.id);
        string str = "";
        textTitle.text = dbInfo.title;

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
        UpdateLoveStatus();
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
        UpdateText(item.infoItem);

    }
    public void OnClickBtnClose()
    {
        Close(); 
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
