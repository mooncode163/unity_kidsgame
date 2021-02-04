using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIHowToPlayPage1 : UIHowToPlayPage, IUIWordBoardDelegate, IUIWordContentBaseDelegate
{

    Sequence seqAnimate;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {

        // textTitle.color = AppRes.colorTitle;
        textDetail.color = AppRes.colorTitle;
        //textTitle.text = Language.main.GetString("STR_HOWTOPLAY_TITLE");
        textDetail.text = Language.main.GetString("STR_HOWTOPLAY_DETAIL1");
        LoadPrefab();
    }
    void Start()
    {

        gameLevel = index;
        infoItem = GameLevelParse.main.GetGuankaItemInfo(gameLevel) as CaiCaiLeItemInfo;
        LayOut();
        // LevelManager.main.gameLevel = 0;
        UpdateGuankaLevel(gameLevel);
        // Invoke("RunAnimate", 1.0f);
    }


    public void UpdateGuankaLevel(int level)
    {
        GameLevelParse.main.ParseItem(infoItem);
        uiWordFillBox = (UIWordFillBox)GameObject.Instantiate(uiWordFillBoxPrefab);
        uiWordFillBox.transform.SetParent(objTop.transform);
        uiWordFillBox.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        UIViewController.ClonePrefabRectTransform(uiWordFillBoxPrefab.gameObject, uiWordFillBox.gameObject);
        uiWordFillBox.iDelegate = this;
        uiWordFillBox.infoItem = infoItem;
        uiWordFillBox.UpdateGuankaLevel(level);

        UpdateWord();

    }

    public override void LayOut()
    {
        float x, y, w, h;
        RectTransform rctranPage = this.GetComponent<RectTransform>();
        {

            Debug.Log(" page.rect=" + rctranPage.rect + " width=" + width + " heigt=" + heigt);
            x = width / 4;
            y = 0;

        }

        Rect rectImage = Rect.zero;
        //game pic
        {
            float ratio = 0.9f;
            w = this.frame.width * ratio;
            h = (this.frame.height / 2);
            x = 0;
            y = 0;
            if (uiWordFillBox != null)
            {
                RectTransform rctran = uiWordFillBox.GetComponent<RectTransform>();
                w = Mathf.Min(w, h);
                h = w;
                rctran.sizeDelta = new Vector2(w, h);
                rctran.anchoredPosition = new Vector2(x, y);
                uiWordFillBox.LayOut();
            }
        }

        //imageHand
        {
            RectTransform rctran = imageHand.GetComponent<RectTransform>();
            x = rctranPage.rect.size.x / 2 - rctran.rect.size.x;
            y = -rctranPage.rect.size.y / 2 + rctran.rect.size.y;
            rctran.anchoredPosition = new Vector2(x, y);
        }

        if (infoItem != null)
        {
            string strBoard = GameAnswer.main.GetGuankaAnswer(infoItem);
            uiWordBoard.row = 1;
            uiWordBoard.col = strBoard.Length;
            uiWordBoard.LayOut();
        }
    }

    void RunAnimate()
    {
        //position 屏幕坐标
        Vector2 pt0 = imageHand.transform.position;
        UILetterItem uiSel = uiWordFillBox.GetSelItem();
        UILetterItem uiUnSel = uiWordFillBox.GetFistUnSelItem();
        Vector2 pt1 = uiSel.transform.position;
        Vector2 pt2 = uiUnSel.transform.position;
        Debug.Log("RunAnimate pt0=" + pt0 + " pt1=" + pt1 + " pt2" + pt2);
        seqAnimate = DOTween.Sequence();
        RectTransform rctran = imageHand.GetComponent<RectTransform>();
        float t_animation = 1f;
        float t_rotate = t_animation / 2;
        float anglez = 30;
        Tweener acRotate0 = imageHand.transform.DORotate(new Vector3(0, 0, 0), t_rotate);
        Tweener acRotate1 = imageHand.transform.DORotate(new Vector3(0, 0, 0), t_rotate);

        Tweener action0 = imageHand.transform.DOMove(pt1, t_animation).SetEase(Ease.InSine);
        Tweener action1 = imageHand.transform.DOMove(pt2, t_animation).SetEase(Ease.InSine);

        Tweener action_prestart = imageHand.transform.DOMove(pt0, 0.01f);

        seqAnimate
         //prestart
         .Append(action_prestart.OnComplete(
            () =>
            {
                Debug.Log("RunAnimate  pre start OnComplete");
                //解决动画循环播放时 第一次介绍后imageHand角度被修改成-anglez)的问题
                imageHand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
        ))
        //step 0
        .Append(action0.OnComplete(
            () =>
            {
                Debug.Log("RunAnimate  move end 0");
                imageHand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -anglez));
            }
        ))
        .Append(acRotate0.OnComplete(
            () =>
            {
                Debug.Log("RunAnimate  acRotate end 0");
                imageHand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                uiSel.OnClickItem();
            }
        ))


        //step 1
        .Append(action1.OnComplete(
            () =>
            {
                Debug.Log("RunAnimate  move end 1");
                imageHand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -anglez));
            }
        ))

        .Append(acRotate1.OnComplete(
            () =>
            {
                Debug.Log("RunAnimate  acRotate end 1");
                uiUnSel.OnClickItem();
                imageHand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
        ))


        //delay
        .AppendInterval(t_animation / 2)
        .OnComplete(
            () =>
            {
                imageHand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                imageHand.transform.position = pt0;
            }
        )
       .SetLoops(-1);
        seqAnimate.Play();
    }

    void StopAnimate()
    {
        seqAnimate.Pause();
        LayOut();
    }
    void UpdateWord()
    {
        //先计算行列数
        LayOut();
        uiWordBoard.iDelegate = this;
        uiWordBoard.InitItem();
        string strBoard = GameAnswer.main.GetGuankaAnswer(infoItem);
        uiWordBoard.UpdateItem(infoItem, strBoard);
    }


    public void UIWordBoardDidClick(UIWordBoard bd, UIWordItem item)
    {
        Debug.Log("UIWordBoardDidClick");
        if (uiWordFillBox != null)
        {
            uiWordFillBox.OnAddWord(item.wordDisplay);
            item.ShowContent(false);
        }
    }

    public void UIWordContentBaseDidBackWord(UIWordContentBase ui, string word)
    {
        uiWordBoard.BackWord(word);
    }
    public void UIWordContentBaseDidTipsWord(UIWordContentBase ui, string word)
    {
        uiWordBoard.HideWord(word);
    }

    public void UIWordContentBaseDidAdd(UIWordContentBase ui, string word)
    {

    }

    public void UIWordContentBaseDidGameFinish(UIWordContentBase ui, bool isFail)
    {

    }
    public override void OnPageExit()
    {
        Debug.Log("OnPageExit page1 ");
        StopAnimate();
    }
    public override void OnPageEnter()
    {
        Debug.Log("OnPageEnter page1 ");
        RunAnimate();
    }
}
