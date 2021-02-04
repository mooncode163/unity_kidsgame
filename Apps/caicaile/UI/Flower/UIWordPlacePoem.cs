using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

//成语接龙
/* 
 */

public class UIWordPlacePoem : UIWordContentBase, IUIItemFlowerDelegate
{
    public const int GOLD_TITLE = 5;
    public const int GOLD_TRANSLATION = 3;
    public UIImage imageBg;
    public UIImage imageHand;
    public UIText textTitle;
    public UIText textTranslation;
    public UIImageText uiStatus;
    public GameObject objWord;
    public GameObject objBtnBar;
    public Button btnShare;
    public Button btnVideo;
    public UIItemFlower uiItemFlowerPrefab;
    public int row = 7;
    public int col = 7;
    public List<UIItemFlower> listItem;

    List<UIItemFlower> listItemSel;
    List<AnswerInfo> listAnswer;
    LayOutGrid lygrid;
    int indexFillWord;
    int indexAnswer;
    bool isTouchSel;
    UIItemFlower itemTouchSel0;
    UIItemFlower itemTouchSel1;
    UIItemFlower itemTouchSelPre;
    UIItemFlower uiSelect;
    UIItemFlower uiAnswer;
    UIItemFlower itemGuide;
    string strAnswer;
    bool isShowGuide;
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
        objWord.transform.SetAsLastSibling();

        itemTouchSel0 = null;
        itemTouchSel1 = null;
        itemTouchSelPre = null;
        uiStatus.gameObject.SetActive(false);

        ShowGuide();

        textTitle.gameObject.SetActive(false);
        textTranslation.gameObject.SetActive(false);

        if (!AppVersion.appCheckHasFinished)
        {
            btnVideo.gameObject.SetActive(false);
            objBtnBar.gameObject.SetActive(false);
        }

    }

    // Use this for initialization
    void Start()
    {
        LayOut();
        // if (iDelegate != null)
        // {
        //     iDelegate.UIWordContentBaseDidGameFinish(this, false);
        // }
    }


    // Update is called once per frame
    void Update()
    {

    }
    void ShowGuide()
    {
        isShowGuide = false;
        string key = "KEY_FIRST_RUN_PLACE";
        bool isFirst = Common.GetKeyForFirstRun(key);
        if (Application.isEditor)
        {
            // return;
        }
        if (isFirst)
        {
            isShowGuide = true;
            Common.SetKeyForFirstRun(key, false);
        }

        imageHand.gameObject.SetActive(isShowGuide);

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

        //BtnBar
        // LayOutHorizontal lyh = objBtnBar.GetComponent<LayOutHorizontal>();
        // LayOutVertical lyv = objBtnBar.GetComponent<LayOutVertical>();
        // LayOutRelation lyrel = objBtnBar.GetComponent<LayOutRelation>();
        // LayOutSize lysize = objBtnBar.GetComponent<LayOutSize>();
        // RectTransform rctranBtn = objBtnBar.GetComponent<RectTransform>();
        // if (Device.isLandscape)
        // {

        //     lyh.enableLayout = false;
        //     lyv.enableLayout = true;
        //     lyrel.align = LayOutBase.Align.RIGHT;
        //     Vector2 sz = rctranBtn.sizeDelta;
        //     x = Mathf.Min(sz.x, sz.y);
        //     y = Mathf.Max(sz.x, sz.y);
        //     rctranBtn.sizeDelta = new Vector2(x, y);
        //     lysize.typeX = LayOutSize.Type.MATCH_CONTENT;
        //     lysize.typeY = LayOutSize.Type.MATCH_PARENT;

        //     Vector2 pos = rctranBtn.anchoredPosition;
        //     pos.y = 0;
        //     rctranBtn.anchoredPosition = pos;
        // }
        // else
        // {
        //     lyh.enableLayout = true;
        //     lyv.enableLayout = false;
        // }
        // lyh.LayOut();
        // lyv.LayOut();

    }
    public override void UpdateGuankaLevel(int level)
    {
        UpdateItem();

        // if (iDelegate != null)
        // {
        //     iDelegate.UIWordContentBaseDidGameFinish(this, false);
        // }
    }

    public UIItemFlower GetUnLockPlaceItem(int idx)
    {
        UIItemFlower ret = null;
        List<UIItemFlower> listOther = new List<UIItemFlower>();
        foreach (UIItemFlower item in listItem)
        {
            if ((!item.isHavePlaced) && (idx != item.index) && (item.status != UIItemFlower.Status.LOCK))
            {
                listOther.Add(item);
            }
        }
        if (listOther.Count > 0)
        {
            int rdm = Random.Range(0, listOther.Count);
            ret = listOther[rdm];
        }
        return ret;
    }

    public List<UIItemFlower> GetAllUnLock()
    {
        List<UIItemFlower> listOther = new List<UIItemFlower>();
        foreach (UIItemFlower item in listItem)
        {
            if (item.status != UIItemFlower.Status.LOCK)
            {
                listOther.Add(item);
            }
        }
        return listOther;
    }

    //交换位置
    public void SwapItem(UIItemFlower item0, UIItemFlower item1)
    {
        int row0 = item0.indexRow;
        int row1 = item1.indexRow;
        int col0 = item0.indexCol;
        int col1 = item1.indexCol;
        item0.indexRow = row1;
        item0.indexCol = col1;

        item1.indexRow = row0;
        item1.indexCol = col0;

    }

    // 随机排列
    public void RandomPlace()
    {
        List<UIItemFlower> listUnlock = GetAllUnLock();
        int[] indexList = Common.RandomIndex(listUnlock.Count, listUnlock.Count);
        int idx = 0;
        foreach (UIItemFlower item in listItem)
        {
            if ((!item.isHavePlaced) && (item.status != UIItemFlower.Status.LOCK))
            {
                UIItemFlower itemother = GetUnLockPlaceItem(item.index);
                if (itemother != null)
                {
                    SwapItem(item, itemother);
                    item.isHavePlaced = true;
                }

                idx++;
            }
        }

    }

    public int GetMaxCol()
    {
        int ret = 1;
        int level = LevelManager.main.gameLevel;
        CaiCaiLeItemInfo info = GameLevelParse.main.listGuanka[level] as CaiCaiLeItemInfo;
        foreach (PoemContentInfo infopoem in info.listPoemContent)
        {
            int len = infopoem.content.Length;
            if (len > ret)
            {
                ret = len;
            }
        }
        return ret;
    }

    public void UpdateItem()
    {
        if (listAnswer == null)
        {
            listAnswer = new List<AnswerInfo>();
        }
        else
        {
            listAnswer.Clear();
        }
        int level = LevelManager.main.gameLevel;
        CaiCaiLeItemInfo info = GameLevelParse.main.listGuanka[level] as CaiCaiLeItemInfo;
        IdiomItemInfo dbInfo = info.dbInfo as IdiomItemInfo;
        GameLevelParse.main.ParseItem(info);
        textTitle.text = dbInfo.title + "|" + dbInfo.author + "|" + dbInfo.year;
        textTranslation.text = dbInfo.translation;
        PoemContentInfo infoPoem = info.listPoemContent[0];
        string strIdiom = infoPoem.content;
        row = info.listPoemContent.Count;
        col = GetMaxCol();
        lygrid.row = row;
        lygrid.col = col;

        int idx = 0;
        strAnswer = "";
        int max = row * col / 2 - 2;
        int min = 2;
        int lockcount = max - min;// 
        int numlock = (max - level % lockcount) * 2;//Random.Range(min, max);
        int[] indexLock = Common.RandomIndex(row * col, numlock);


        int[] indexRow = Common.RandomIndex(info.listPoemContent.Count, info.listPoemContent.Count);

        for (int i = 0; i < info.listPoemContent.Count; i++)
        {
            infoPoem = info.listPoemContent[i];
            strIdiom = infoPoem.content;
            Debug.Log("conent i=:" + i + ":" + strIdiom);
            strAnswer += strIdiom;
            for (int j = 0; j < strIdiom.Length; j++)
            {
                UIItemFlower ui = (UIItemFlower)GameObject.Instantiate(uiItemFlowerPrefab);
                ui.transform.SetParent(objWord.transform);
                ui.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                UIViewController.ClonePrefabRectTransform(uiItemFlowerPrefab.gameObject, ui.gameObject);
                ui.iDelegate = this;
                ui.indexRow = row - 1 - i;
                ui.indexCol = j;
                ui.index = idx;
                ui.isAnswerItem = false;

                bool islock = false;
                foreach (int idxlock in indexLock)
                {
                    if (idx == idxlock)
                    {
                        islock = true;
                    }
                }
                int rdm = j;
                if (islock)
                {
                    ui.status = UIItemFlower.Status.LOCK;
                    ui.isHavePlaced = true;
                }
                else
                {
                    ui.status = UIItemFlower.Status.NORMAL;
                    ui.isHavePlaced = false;
                }

                string word = strIdiom.Substring(j, 1);
                ui.UpdateItem(word);

                listItem.Add(ui);

                AnswerInfo infoanswer = new AnswerInfo();
                infoanswer.word = word;
                infoanswer.row = ui.indexRow;
                infoanswer.col = ui.indexCol;
                listAnswer.Add(infoanswer);

                idx++;
            }

        }

        RandomPlace();

        LayOut();

        Invoke("OnGuideFirstItem", 0.5f);
    }

    void OnGuideFirstItem()
    {

        imageHand.transform.SetAsLastSibling();
        if (!isShowGuide)
        {
            return;
        }
        itemGuide = GetUnSelectItem();
        if (itemGuide != null)
        {
            Debug.Log(" find guide word=" + itemGuide.word);
            // RectTransform rcguide = itemGuide.GetComponent<RectTransform>();
            // RectTransform rchand = imageHand.GetComponent<RectTransform>();
            // rchand.anchoredPosition = rcguide.anchoredPosition;
            imageHand.transform.position = itemGuide.transform.position;
            ShwoStatusTitle(Language.main.GetString("STR_HOWPLAY_TITLE0"), false);
        }
        else
        {
            Debug.Log(" find guide fail");
        }
    }
    void OnGuideSecondItem()
    {

        imageHand.transform.SetAsLastSibling();
        if (!isShowGuide)
        {
            return;
        }

        itemGuide = GetItemByAnswer(itemGuide.word);
        if (itemGuide != null)
        {
            imageHand.transform.position = itemGuide.transform.position;
            ShwoStatusTitle(Language.main.GetString("STR_HOWPLAY_TITLE1"), false);
        }
    }


    //是否
    bool IsFirstSelect()
    {
        bool ret = false;
        if (itemTouchSel0 == null)
        {
            return false;
        }
        if (itemTouchSel0.status == UIItemFlower.Status.SELECT)
        {
            return true;
        }
        return ret;
    }
    bool IsBothSelect()
    {
        bool ret = false;
        if ((itemTouchSel0 == null) || (itemTouchSel1 == null))
        {
            return false;
        }
        if ((itemTouchSel0.status == UIItemFlower.Status.SELECT) && (itemTouchSel1.status == UIItemFlower.Status.SELECT))
        {
            return true;
        }
        return ret;
    }

    void RunSelectMoveAnimate(UIItemFlower item0, UIItemFlower item1, string endFunc)
    {
        Debug.Log("RunSelectMoveAnimate");
        item0.transform.SetAsLastSibling();
        item1.transform.SetAsLastSibling();
        imageHand.transform.SetAsLastSibling();
        float duration = 1f;
        Vector2 pos0 = item0.transform.position;
        Vector2 pos1 = item1.transform.position;
        item0.transform.DOMove(pos1, duration).OnComplete(() =>
                          {

                          });

        item1.transform.DOMove(pos0, duration).OnComplete(() =>
      {
          //   CheckSelect();
          Invoke(endFunc, 0.0f);

      });
    }
    void CheckSelect()
    {
        // itemTouchSel0.status = UIItemFlower.Status.NORMAL;
        // itemTouchSel1.status = UIItemFlower.Status.NORMAL; 
        SwapItem(itemTouchSel0, itemTouchSel1);


        string answer0 = GetAnswerOfItem(itemTouchSel0);
        string answer1 = GetAnswerOfItem(itemTouchSel1);
        if (answer0 == itemTouchSel0.word)
        {
            itemTouchSel0.status = UIItemFlower.Status.LOCK;
        }
        else
        {
            itemTouchSel0.status = UIItemFlower.Status.NORMAL;
        }
        if (answer1 == itemTouchSel1.word)
        {
            itemTouchSel1.status = UIItemFlower.Status.LOCK;
        }
        else
        {
            itemTouchSel1.status = UIItemFlower.Status.NORMAL;
        }

        CheckAllAnswerFinish();

    }

    string GetAnswerOfItem(UIItemFlower item)
    {
        // int idx = (row - 1 - item.indexRow) * col + item.indexCol;
        // return strAnswer.Substring(idx, 1);
        foreach (AnswerInfo info in listAnswer)
        {
            if ((item.indexRow == info.row) && (item.indexCol == info.col))
            {
                return info.word;
            }
        }
        return "";
    }


    public void OnItemTouchEvent(UITouchEvent ev, PointerEventData eventData, int st)
    {

        float x, y, w, h;
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        Vector2 posScreen = eventData.position;
        Debug.Log("OnItemTouchEvent status=" + st);
        if (st == UITouchEvent.STATUS_TOUCH_DOWN)
        {

            listItemSel.Clear();
            //清空
            // textTitle.text = "";
        }
        if (st == UITouchEvent.STATUS_TOUCH_MOVE)
        {
        }
        if (st == UITouchEvent.STATUS_TOUCH_UP)
        {

            foreach (UIItemFlower item in listItem)
            {
                RectTransform rctran = item.GetComponent<RectTransform>();
                float ratio = 1.0f;
                //屏幕坐标
                w = Common.CanvasToScreenWidth(sizeCanvas, rctran.rect.width * ratio);
                h = Common.CanvasToScreenWidth(sizeCanvas, rctran.rect.height * ratio);
                x = item.transform.position.x;
                y = item.transform.position.y;
                // Vector2 posTouch = mainCam.ScreenToWorldPoint(Common.GetInputPosition());
                Rect rc = new Rect(x - w / 2, y - h / 2, w, h);
                if (rc.Contains(posScreen) && item.gameObject.activeSelf)
                {
                    Debug.Log("OnUIItemFlowerTouchUp item.status=" + item.status);
                    //选中  
                    if (item.status != UIItemFlower.Status.LOCK)
                    {
                        if (item == itemGuide)
                        {
                            OnGuideSecondItem();
                        }
                        else
                        {
                            if (isShowGuide)
                            {
                                return;
                            }
                        }

                        item.status = UIItemFlower.Status.SELECT;

                        if (IsFirstSelect())
                        {
                            Debug.Log("OnUIItemFlowerTouchUp itemTouchSel1");
                            itemTouchSel1 = item;
                            //执行交换
                            RunSelectMoveAnimate(itemTouchSel0, itemTouchSel1, "CheckSelect");
                        }
                        else if (!IsBothSelect())
                        {
                            Debug.Log("OnUIItemFlowerTouchUp itemTouchSel0");
                            itemTouchSel0 = item;
                        }
                        else
                        {
                            itemTouchSel0.status = UIItemFlower.Status.NORMAL;
                            itemTouchSel1.status = UIItemFlower.Status.NORMAL;
                        }

                        item.transform.SetAsLastSibling();
                        imageHand.transform.SetAsLastSibling();
                        OnUIItemFlowerTouchDown(item);
                        itemTouchSelPre = item;
                        break;
                    }
                }
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
    public UIItemFlower GetSelectItem()
    {
        foreach (UIItemFlower item in listItem)
        {
            if (item.status == UIItemFlower.Status.SELECT)
            {
                return item;
            }
        }
        return null;
    }

    public UIItemFlower GetUnSelectItem()
    {
        foreach (UIItemFlower item in listItem)
        {
            if (item.status == UIItemFlower.Status.NORMAL)
            {
                return item;
            }
        }
        return null;
    }

    public UIItemFlower GetItemByAnswer(string answer)
    {

        foreach (AnswerInfo info in listAnswer)
        {
            if (info.word == answer)
            {
                foreach (UIItemFlower item in listItem)
                {
                    if (item.status != UIItemFlower.Status.LOCK)
                    {
                        if ((item.indexRow == info.row) && (item.indexCol == info.col))
                        {
                            return item;
                        }
                    }
                }
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
            if (item.status != UIItemFlower.Status.LOCK)
            {
                isAllAnswer = false;
            }
        }

        Debug.Log("CheckAllAnswerFinish isAllAnswer=" + isAllAnswer);

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
    void CheckTipsSelect()
    {

        SwapItem(uiSelect, uiAnswer);
        uiSelect.status = UIItemFlower.Status.LOCK;
        uiAnswer.status = UIItemFlower.Status.LOCK;
        CheckAllAnswerFinish();
    }


    void OnTipsStatusEnd()
    {
        uiStatus.gameObject.SetActive(false);
        objTopBar.SetActive(true);
    }

    void ShwoStatusTitle(string title, bool autoHide)
    {
        uiStatus.gameObject.SetActive(true);
        uiStatus.transform.SetAsLastSibling();
        uiStatus.UpdateTitle(title);
        objTopBar.SetActive(false);
        if (autoHide)
        {
            Invoke("OnTipsStatusEnd", 1f);
        }
    }

    //STR_TIPS_STATUS_SELECT 提示答案需要先选中一个位置
    public override void OnTips()
    {
        uiSelect = GetSelectItem();
        if (uiSelect == null)
        {
            //提示答案需要先选中一个位置
            Debug.Log("OnTips uiSel is null");
            ShwoStatusTitle(Language.main.GetString("STR_TIPS_STATUS_SELECT"), true);
            return;
        }
        uiAnswer = GetItemByAnswer(uiSelect.word);
        if (uiAnswer != null)
        {
            RunSelectMoveAnimate(uiSelect, uiAnswer, "CheckTipsSelect");
        }
        else
        {
            Debug.Log("OnTips uiAnswer is null word=" + uiSelect.word);
        }

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


    }
    public void OnUIItemFlowerTouchUp(UIItemFlower ui)
    {
        {
            // textTitle.text = "";
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

    public void DoBtnTitle()
    {
        if (Common.gold <= 0)
        {
            OnNotEnoughGold();
            return;
        }
        Common.gold -= GOLD_TITLE;
        if (Common.gold < 0)
        {
            Common.gold = 0;
        }
        textTitle.gameObject.SetActive(true);

    }

    public void OnClickBtnTitle()
    {
        string strPrefab = "App/Prefab/Game/UITipsMsg";
        bool ret = Common.GetBool(UITipsMsg.KEY_TIPS_MSG_TITLE);
        if (!ret)
        {
            PopUpManager.main.Show<UITipsMsg>(strPrefab, popup =>
    {
        popup.keyMsg = UITipsMsg.KEY_TIPS_MSG_TITLE;
        popup.UpdateItem(Language.main.GetString("STR_TIPS_TITLE"));

    }, popup =>
    {
        UITipsMsg ui = popup as UITipsMsg;
        if (ui.isBtnYes)
        {
            DoBtnTitle();
        }
    });
        }
        else
        {
            DoBtnTitle();
        }


    }



    public void DoBtnTranslation()
    {
        if (Common.gold <= 0)
        {
            OnNotEnoughGold();
            return;
        }
        Common.gold -= GOLD_TRANSLATION;
        if (Common.gold < 0)
        {
            Common.gold = 0;
        }

        textTranslation.gameObject.SetActive(true);
    }
    public void OnClickBtnTranslation()
    {
        string strPrefab = "App/Prefab/Game/UITipsMsg";
        bool ret = Common.GetBool(UITipsMsg.KEY_TIPS_MSG_TRANSLATION);
        if (!ret)
        {
            PopUpManager.main.Show<UITipsMsg>(strPrefab, popup =>
            {
                popup.keyMsg = UITipsMsg.KEY_TIPS_MSG_TRANSLATION;
                popup.UpdateItem(Language.main.GetString("STR_TIPS_TRANSLATION"));

            }, popup =>
            {
                UITipsMsg ui = popup as UITipsMsg;
                if (ui.isBtnYes)
                {
                    DoBtnTranslation();
                }

            });
        }
        else
        {
            DoBtnTranslation();
        }

    }

    //观看视频看原文
    public void OnClickBtnVideo()
    {
        AdKitCommon.main.callbackAdVideoFinish = OnAdKitAdVideoFinish;
        AdKitCommon.main.ShowAdVideo();
    }
    public void OnClickBtnRetry()
    {
        GameManager.main.GotoPlayAgain();
    }
    public void OnClickBtnShare()
    {
    }
    public void OnAdKitAdVideoFinish(AdKitCommon.AdType type, AdKitCommon.AdStatus status, string str)
    {
        //if (type == AdKitCommon.AdType.VIDEO)
        {
            if (status == AdKitCommon.AdStatus.SUCCESFULL)
            {
                string strPrefab = "App/Prefab/Game/UITipsMsg";
                PopUpManager.main.Show<UITipsMsg>(strPrefab, popup =>
                           {
                               int level = LevelManager.main.gameLevel;
                               popup.keyMsg = UITipsMsg.KEY_TIPS_MSG_VIDEO;
                               string strmsg = Language.main.GetString("STR_TIPS_VIDEO_RANDOM_ANSWER");
                               CaiCaiLeItemInfo info = GameLevelParse.main.listGuanka[level] as CaiCaiLeItemInfo;
                               int rdm = Random.Range(0, info.listPoemContent.Count);
                               PoemContentInfo infoPoem = info.listPoemContent[rdm];
                               strmsg += infoPoem.content;
                               popup.UpdateItem(strmsg);

                           }, popup =>
                           {


                           });
            }
        }
    }
}
