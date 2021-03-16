using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


// https://hanyu.sogou.com/result?query=%E4%B8%8A&pagetype=result
public class UIWordDetail : UIGameWinBase, ISegmentDelegate
{


    public const string KEY_Basic_Explanation = "KEY_Basic_Explanation";
    public const string KEY_Detail_Explanation = "KEY_Detail_Explanation";
    public const string KEY_Word_GROUP = "KEY_Word_GROUP"; //组词

    // 反义词
    public const string KEY_Word_antonym = "KEY_Word_antonym";
    //近义词
    public const string KEY_Word_homoionym = "KEY_Word_homoionym";

 
    WordItemInfo infoGuanka;
    /// <summary>
    /// Unity's Awake method.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        WordItemInfo info = GameLevelParse.main.GetItemInfo() as WordItemInfo;


        textView.SetFontSize(80);

        indexSegment = 0;
        uiSegment.InitValue(64, Color.red, Color.black);
        uiSegment.iDelegate = this;

        uiSegment.gameObject.SetActive(true); 

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

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    // void Update()
    // {

    //     this.LayOut();
    // }

    public void UpdateSegment()
    {
        // 	 
        // 基础释义详细释义组词近义词反义词

        {
            ItemInfo infoSeg = new ItemInfo();
            infoSeg.id = KEY_Basic_Explanation;
            infoSeg.title = Language.main.GetString(infoSeg.id);
            uiSegment.AddItem(infoSeg);
        }

        {
            ItemInfo infoSeg = new ItemInfo();
            infoSeg.id = KEY_Detail_Explanation;
            infoSeg.title = Language.main.GetString(infoSeg.id);
            uiSegment.AddItem(infoSeg);
        }

        {
            ItemInfo infoSeg = new ItemInfo();
            infoSeg.id = KEY_Word_GROUP;
            infoSeg.title = Language.main.GetString(infoSeg.id);
            uiSegment.AddItem(infoSeg);
        }
        {
            ItemInfo infoSeg = new ItemInfo();
            infoSeg.id = KEY_Word_antonym;
            infoSeg.title = Language.main.GetString(infoSeg.id);
            uiSegment.AddItem(infoSeg);
        }
        {
            ItemInfo infoSeg = new ItemInfo();
            infoSeg.id = KEY_Word_homoionym;
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
        float ratio = 0.9f;
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

        }
        base.LayOut(); 
    }

    public void UpdateText(ItemInfo info)
    {
        // infoGuanka = GameLevelParse.main.GetItemInfo() as WordItemInfo;
        GameLevelParse.main.ParseItem(infoGuanka);
        infoItem = infoGuanka;

        DBWordItemInfo dbInfo = infoGuanka.dbInfo as DBWordItemInfo;

        string str = "";
        switch (info.id)
        {
            case KEY_Basic_Explanation:
                {
                    str = dbInfo.meanBasic;
                }
                break;
            case KEY_Detail_Explanation:
                {
                    str = dbInfo.meanDetail;
                }
                break;
            case KEY_Word_GROUP:
                {
                    str = dbInfo.zuci;
                }
                break;
            case KEY_Word_antonym:
                {
                    str = dbInfo.antonym;
                }
                break;
            case KEY_Word_homoionym:
                {
                    str = dbInfo.homoionym;
                }
                break;

        }

        if (Common.BlankString(str))
        {
            str = Language.main.GetString("STR_UIVIEWALERT_MSG_GAME_FINISH");
        }
        textView.text = str;
    }

    public override void UpdateItem(WordItemInfo info)
    {
        infoGuanka = info;
        DBWordItemInfo dbInfo = infoGuanka.dbInfo as DBWordItemInfo;
 
        Debug.Log("UpdateItem info.icon=" + info.icon);
       
        base.LayOut();
    }

 

    public void SegmentDidClick(UISegment seg, SegmentItem item)
    {
        // UpdateSortList(item.index);
        UpdateText(item.infoItem);

    }
    public void OnClickBtnClose()
    {
        Close();
        // GameManager.main.GotoPlayAgain();
    } 
}
