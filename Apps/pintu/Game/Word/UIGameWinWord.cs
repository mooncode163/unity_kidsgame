using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


// https://hanyu.sogou.com/result?query=%E4%B8%8A&pagetype=result
public class UIGameWinWord : UIGameWinBase, ISegmentDelegate
{


    public const string KEY_Basic_Explanation = "KEY_Basic_Explanation";
    public const string KEY_Detail_Explanation = "KEY_Detail_Explanation";
    public const string KEY_Word_GROUP = "KEY_Word_GROUP"; //组词

    // 反义词
    public const string KEY_Word_antonym = "KEY_Word_antonym";
    //近义词
    public const string KEY_Word_homoionym = "KEY_Word_homoionym";
    public UIText textWord;
    public UIText textZuci;//组词
    public UIText textBushou;//部首
    public UIText textBihua;//笔画
    public UIText textWubi;//五笔
    public UIText textFanti;//繁体
    public UIText bihuaOrder;//笔顺 丨  一  一 
    public UIText bihuaName;// 名称： 竖 横 横

    WordItemInfo infoGuanka;
    /// <summary>
    /// Unity's Awake method.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        textPinyin.gameObject.SetActive(true);
        // UpdateText(null);

    }

    /// <summary>
    /// Unity's Start method.
    /// </summary>
    protected override void Start()
    {
        base.Start();
        LayOut();
    }


    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    // void Update()
    // {

    //     this.LayOut();
    // }
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

        //objLayoutBtn
        {

            LayOutGrid lg = objLayoutBtn.GetComponent<LayOutGrid>();
            lg.enableHide = false;
            int btn_count = lg.GetChildCount(false);
            if (Device.isLandscape)
            {
                // lg.row = btn_count;
                // lg.col = 1;
                lg.row = 1;
                lg.col = btn_count;
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
        textTitle.text = dbInfo.id;
        textPinyin.text = dbInfo.pinyin;
        textBushou.text = Language.main.GetString("STR_BUSHOU") + ":" + dbInfo.bushou;
        textBihua.text = Language.main.GetString("STR_BIHUA") + ":" + dbInfo.bihuaCount;
        textWubi.text = Language.main.GetString("STR_WUBI") + ":" + dbInfo.wubi;
        textFanti.text = Language.main.GetString("STR_FANTI") + ":" + dbInfo.fanti;
        bihuaOrder.text = Language.main.GetString("STR_BIHUA_ORDER") + ":" + dbInfo.bihuaOrder;
        bihuaName.text = Language.main.GetString("STR_BIHUA_NAME") + ":" + dbInfo.bihuaName;

        Debug.Log("UpdateItem info.icon=" + info.icon);

        base.LayOut();
    }


    public void UpdateLoveStatus()
    {
        string strBtn = "";
        DBWordItemInfo dbInfo = infoGuanka.dbInfo as DBWordItemInfo;

        if (DBLoveWord.main.IsItemExistId(infoGuanka.id))
        {
            strBtn = Language.main.GetString("STR_Detail_DELETE_LOVE");
        }
        else
        {
            strBtn = Language.main.GetString("STR_Detail_ADD_LOVE");
        }
        btnAddLove.textTitle.text = strBtn;
    }
    public void ShowDetail()
    {
        Debug.Log("UIWordDetail ShowDetail ");
        WordItemInfo info = GameLevelParse.main.GetItemInfo() as WordItemInfo;
        ItemInfo infoPlace = LevelManager.main.GetPlaceItemInfo(LevelManager.main.placeLevel);
        string strPrefab = "AppCommon/Prefab/Game/GameFinish/UIWordDetail";
        PopUpManager.main.Show<UIGameWinBase>(strPrefab, popup =>
        {
            Debug.Log("UIWordDetail Open ");
            popup.UpdateItem(info);

        }, popup =>
        {

            Debug.Log("UIWordDetail Open ");
        });
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
        DBWordItemInfo infoidiom = infoGuanka.dbInfo as DBWordItemInfo;
 
        if (DBLoveWord.main.IsItemExist(infoidiom))
        {
            DBLoveWord.main.DeleteItem(infoidiom);
        }
        else
        {
            DBLoveWord.main.AddItem(infoidiom);
        }
        UpdateLoveStatus();
    }


    public void OnClickBtnDetail()
    {
        // Close();

        // Invoke("ShowDetail", 1f);
        ShowDetail();
    }
}
