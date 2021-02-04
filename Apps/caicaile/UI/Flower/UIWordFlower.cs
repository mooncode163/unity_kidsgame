using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

//成语接龙
/*
成语小秀才
https://www.taptap.com/app/167939
 */

public class UIWordFlower : UIWordContentBase, IUIItemFlowerDelegate
{
    public UIImage imageBg;
    public UIText textTitle;
    // public GameObject objTopBar;
    public GameObject objWord;
    public GameObject objBtnBar;
    public UIAnswer uiAnswer;
    public Button btnShare;
    public UIItemFlower uiItemFlowerPrefab;
    public int row = 7;
    public int col = 7;
    public List<UIItemFlower> listItem;

    List<UIItemFlower> listItemSel;
    LayOutGrid lygrid;
    int indexFillWord;
    int indexAnswer;
    bool isTouchSel;
    UIItemFlower itemTouchSel;
    UIItemFlower itemTouchSelPre;
    void Awake()
    {
        base.Awake();
        lygrid = objWord.GetComponent<LayOutGrid>();
        listItem = new List<UIItemFlower>();
        listItemSel = new List<UIItemFlower>();
        if (btnShare != null)
        {
            btnShare.gameObject.SetActive(Config.main.isHaveShare);
        }
        row = 4;
        col = 4;
        lygrid.row = row;
        lygrid.col = col;
        lygrid.enableLayout = false;
        lygrid.dispLayVertical = LayOutBase.DispLayVertical.TOP_TO_BOTTOM;
        isTouchSel = false;
        indexAnswer = 0;
        UITouchEventWithMove ev = objWord.AddComponent<UITouchEventWithMove>();
        ev.callBackTouch = OnItemTouchEvent;
        ev.enableLongPress = false;
        objWord.transform.SetAsLastSibling();
    }

    // Use this for initialization
    void Start()
    {
        LayOut();
        //    if (iDelegate != null)
        //     {
        //         iDelegate.UIWordContentBaseDidGameFinish(this, false);
        //     }


    }


    // Update is called once per frame
    void Update()
    {

    }

    public override void LayOut()
    {
        base.LayOut();
        float x, y, w, h;
        RectTransform rctranRoot = objWord.GetComponent<RectTransform>();
        Debug.Log("rctranRoot w=" + rctranRoot.rect.width + " h=" + rctranRoot.rect.height);

        if (lygrid != null)
        {
            //lygrid.LayOut();
            foreach (UIItemFlower item in listItem)
            {

                RectTransform rctran = item.GetComponent<RectTransform>();
                w = (rctranRoot.rect.width - (lygrid.space.x) * (col + 1)) / col;
                h = w;
                rctran.sizeDelta = new Vector2(w, h);
                item.SetFontSize((int)(w * 0.7f));
                Vector2 pos = lygrid.GetItemPostion(item.gameObject, item.indexRow, item.indexCol);
                rctran.anchoredPosition = pos;
                item.LayOut();
            }
        }


    }
    public override void UpdateGuankaLevel(int level)
    {
        UpdateItem();

        // if (iDelegate != null)
        // {
        //     iDelegate.UIWordContentBaseDidGameFinish(this, false);
        // }
    }
    public void UpdateItem()
    {
        int level = LevelManager.main.gameLevel;
        CaiCaiLeItemInfo info = GameLevelParse.main.listGuanka[level] as CaiCaiLeItemInfo;

        List<object> listPos = null;
        if (Common.appKeyName == GameRes.GAME_PoemFlower)
        {
            LevelParsePoemFlower.main.ParsePosition(row, col, info.content0.Length);
            listPos = LevelParsePoemFlower.main.listPosition;
            Debug.Log("content0="+info.content0);
            Debug.Log("content1="+info.content1);
        }
        else
        {
            listPos = LevelParseIdiomFlower.main.listPosition;
        }

        int idx_pos = Random.Range(0, listPos.Count);
        //idx_pos = 0;
        PositionInfo infoPos = listPos[idx_pos] as PositionInfo;

        lygrid.row = row;
        lygrid.col = col;

        int idx = 0;
        for (int i = 0; i < info.listIdiom.Count; i++)
        {
            string strIdiom = info.listIdiom[i];
            for (int j = 0; j < strIdiom.Length; j++)
            {
                UIItemFlower ui = (UIItemFlower)GameObject.Instantiate(uiItemFlowerPrefab);
                ui.transform.SetParent(objWord.transform);
                ui.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                UIViewController.ClonePrefabRectTransform(uiItemFlowerPrefab.gameObject, ui.gameObject);
                ui.iDelegate = this;
                RowColInfo infoRowCol = infoPos.listRowCol[idx];
                ui.indexRow = infoRowCol.row;
                ui.indexCol = infoRowCol.col;
                ui.index = idx;
                ui.isAnswerItem = false;
                ui.status = UIItemFlower.Status.NORMAL;
                ui.UpdateItem(strIdiom.Substring(j, 1));
                listItem.Add(ui);
                idx++;
            }

        }
        uiAnswer.UpdateItem(level);

        LayOut();

    }

    //是否相邻
    bool IsSideItem()
    {
        bool ret = false;
        if (itemTouchSelPre == null)
        {
            return true;
        }

        if (!itemTouchSel.gameObject.activeSelf)
        {
            return false;
        }

        if ((Mathf.Abs(itemTouchSel.indexRow - itemTouchSelPre.indexRow) <= 1) && (Mathf.Abs(itemTouchSel.indexCol - itemTouchSelPre.indexCol) <= 1))
        {
            return true;
        }
        return ret;
    }


    public void OnItemTouchEvent(UITouchEvent ev, PointerEventData eventData, int st)
    {

        float x, y, w, h;
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        Vector2 posScreen = eventData.position;
        //Debug.Log("OnItemTouchEvent status=" + status);
        if (st == UITouchEvent.STATUS_TOUCH_DOWN)
        {
            itemTouchSel = null;
            itemTouchSelPre = null;
            listItemSel.Clear();
            //清空
            textTitle.text = "";

        }
        if (st == UITouchEvent.STATUS_TOUCH_MOVE)
        {

            //position 当gameObject为canvas元素时为屏幕坐标而非世界坐标


            foreach (UIItemFlower item in listItem)
            {
                RectTransform rctran = item.GetComponent<RectTransform>();
                float ratio = 0.8f;

                //屏幕坐标
                w = Common.CanvasToScreenWidth(sizeCanvas, rctran.rect.width * ratio);
                h = Common.CanvasToScreenWidth(sizeCanvas, rctran.rect.height * ratio);
                x = item.transform.position.x;
                y = item.transform.position.y;

                // Vector2 posTouch = mainCam.ScreenToWorldPoint(Common.GetInputPosition());
                Rect rc = new Rect(x - w / 2, y - h / 2, w, h);
                if (rc.Contains(posScreen) && item.gameObject.activeSelf)
                {
                    //选中
                    if (item.status != UIItemFlower.Status.SELECT)
                    {
                        isTouchSel = true;
                        itemTouchSel = item;
                        if (IsSideItem())
                        {
                            item.status = UIItemFlower.Status.SELECT;
                            item.transform.SetAsLastSibling();
                            //  AudioPlay.main.PlayFile(AppRes.AUDIO_LETTER_DRAG_1); 
                            OnUIItemFlowerTouchMove(item);
                            itemTouchSelPre = item;
                        }
                    }

                    // isClickDown = true;
                }
            }

            // status = Status.SELECT;
        }
        if (st == UITouchEvent.STATUS_TOUCH_UP)
        {
            Debug.Log("OnUIItemFlowerTouchUp isTouchSel=" + isTouchSel);
            if (isTouchSel)
            {
                isTouchSel = false;
                OnUIItemFlowerTouchUp(itemTouchSel);
            }

        }

    }

    public UIItemFlower GetSelItem()
    {
        UIItemFlower ui = listItem[indexFillWord];
        return ui;
    }

    public UIItemFlower GetItem(int idx)
    {
        foreach (UIItemFlower item in listItem)
        {
            if (idx == item.index)
            {
                return item;
            }
        }
        return null;
    }
    public UIItemFlower GetItem(int idxRow, int idxCol)
    {
        foreach (UIItemFlower item in listItem)
        {
            if ((idxRow == item.indexRow) && (idxCol == item.indexCol))
            {
                return item;
            }
        }
        return null;
    }


    //判断答案是否正确
    public override bool CheckAllAnswerFinish()
    {
        bool isAllAnswer = true;

        foreach (UIItemFlower item in listItem)
        {
            if (item.gameObject.activeSelf)
            {
                isAllAnswer = false;
            }
        }

        if (isAllAnswer)
        {
            //全部猜对 game win
            if (iDelegate != null)
            {
                iDelegate.UIWordContentBaseDidGameFinish(this, false);
            }

        }
        else
        {
            //游戏失败
            //  OnGameFail();
        }
        return isAllAnswer;
    }

    public override void OnAddWord(string word)
    {


    }

    public override void OnTips()
    {
        uiAnswer.OnTips();

    }

    public override void OnReset()
    {

    }

    public void OnUIItemFlowerTouchDown(UIItemFlower ui)
    {
    }
    public void OnUIItemFlowerTouchMove(UIItemFlower ui)
    {
        Debug.Log("OnUIItemFlowerTouchMove ui.word=" + ui.word + " textTitle=" + textTitle.text);
        listItemSel.Add(ui);
        textTitle.text = textTitle.text + ui.word;

        if (iDelegate != null)
        {
            iDelegate.UIWordContentBaseDidAdd(this, ui.word);
        }

    }
    public void OnUIItemFlowerTouchUp(UIItemFlower ui)
    {
        string strword = textTitle.text;
         Debug.Log("strword="+strword);
        if (uiAnswer.IsRightAnswer(strword))
        {
            int idx = 0;
            indexAnswer = uiAnswer.GetIndexAnswer(textTitle.text);
            Debug.Log("strword indexAnswer="+indexAnswer);
            if (indexAnswer < 0)
            {
                return;
            }
            for (int i = 0; i < listItemSel.Count; i++)
            {
                UIItemFlower item = listItemSel[i];
                float duration = 1f;
                Vector2 toPos = uiAnswer.GetPosAnswerLetter(indexAnswer, i);
                item.transform.DOMove(toPos, duration).OnComplete(() =>
                          {
                              item.gameObject.SetActive(false);
                              uiAnswer.ShowAnswerWord(indexAnswer, idx);
                              idx++;
                              CheckAllAnswerFinish();
                          });
            }

        }
        else
        {
            textTitle.text = "";
            foreach (UIItemFlower item in listItemSel)
            {
                item.status = UIItemFlower.Status.NORMAL;
            }
            if (iDelegate != null)
            {
                iDelegate.UIWordContentBaseDidGameFinish(this, true);
            }

        }

    }
    void OnUIViewAlertFinished(UIViewAlert alert, bool isYes)
    {

        if (STR_KEYNAME_VIEWALERT_GOLD == alert.keyName)
        {
            if (isYes)
            {
                ShowShop();
            }
        }



    }

    public void OnNotEnoughGold()
    {

        string title = Language.main.GetString(AppString.STR_UIVIEWALERT_TITLE_NOT_ENOUGH_GOLD);
        string msg = Language.main.GetString(AppString.STR_UIVIEWALERT_MSG_NOT_ENOUGH_GOLD);
        string yes = Language.main.GetString(AppString.STR_UIVIEWALERT_YES_NOT_ENOUGH_GOLD);
        string no = Language.main.GetString(AppString.STR_UIVIEWALERT_NO_NOT_ENOUGH_GOLD);

        ViewAlertManager.main.ShowFull(title, msg, yes, no, false, STR_KEYNAME_VIEWALERT_GOLD, OnUIViewAlertFinished);

    }

    public void OnClickBtnHowPlay()
    {
        HowPlayFlowerViewController.main.Show(null, null);
    }
    public void OnClickBtnInfo()
    {
        GameInfoViewController.main.Show(null, null);
    }

    public void OnClickBtnTips()
    {

        if (Common.gold <= 0)
        {
            OnNotEnoughGold();
            return;
        }
        Common.gold--;
        if (Common.gold < 0)
        {
            Common.gold = 0;
        }
        UpdateGold();
        OnTips();

    }

    public void OnClickBtnRetry()
    {
        GameManager.main.GotoPlayAgain();
    }
    public void OnClickBtnShare()
    {
    }

}
