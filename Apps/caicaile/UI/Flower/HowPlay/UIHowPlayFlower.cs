using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class UIHowPlayFlower : UIView

{
    public UIImage imageBoard;
    public UIImage imageBg;
    public GameObject objContent;
    public GameObject objScrollView;
    public GameObject objScrollViewContent;


    public UIScrollViewDot uiScrollDot;
    public ScrollRect scrollRect;

    List<object> listPage;
    int totalPage = 6;
    int indexPage = 0;
    int indexPagePre = 0;
    int indexPageCur = 0;
    float action_time = 0.5f;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {

        LoadPrefab();
        scrollRect = objScrollView.GetComponent<ScrollRect>();
        scrollRect.onValueChanged.AddListener(ScrollViewValueChanged);
        //bg 
        // TextureUtil.UpdateImageTexture(imageBg, AppRes.IMAGE_COMMON_BG, true);
        UIScrollViewTouchEvent ev = objScrollView.AddComponent<UIScrollViewTouchEvent>();
        ev.callbackTouch = OnScrollViewDrag;
        RectTransform rctran = objContent.GetComponent<RectTransform>();


    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        UpdateItem();
        LayOut();
        ShowAction();
        AdKitCommon.main.ShowAdBanner(false);
    }

    void LoadPrefab()
    {
        if (listPage == null)
        {
            listPage = new List<object>();
        }
        for (int i = 0; i < totalPage; i++)
        {
            PageInfo info = new PageInfo();
            info.index = i;
            //info.name = pageName[i];
            info.name = "UIHowPlayFlowerPage" + i;
            {
                GameObject obj = PrefabCache.main.Load("App/Prefab/HowPlay/" + info.name);
                if (obj != null)
                {
                    info.uiPrefab = obj.GetComponent<UIHowToPlayPageBase>();
                    info.uiPrefab.index = i;
                }
            }
            listPage.Add(info);
        }
        totalPage = listPage.Count;
    }

    void UpdateItem()
    {
        indexPagePre = 0;
        indexPage = 0;
        indexPageCur = 0;
        for (int i = 0; i < listPage.Count; i++)
        {
            PageInfo info = listPage[i] as PageInfo;
            UIHowToPlayPageBase ui = (UIHowToPlayPageBase)GameObject.Instantiate(info.uiPrefab);
            ui.index = i;
            ui.transform.SetParent(objScrollViewContent.transform);
            ui.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            UIViewController.ClonePrefabRectTransform(info.uiPrefab.gameObject, ui.gameObject);
            info.ui = ui;

        }
        uiScrollDot.SetTotal(totalPage);
    }

    void ShowAction()
    {
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        RectTransform rctran = objContent.GetComponent<RectTransform>();
        Vector2 pt = new Vector2(0, sizeCanvas.y / 2 + rctran.rect.height / 2);
        rctran.DOLocalMove(pt, action_time).From().SetEase(Ease.InOutBounce).OnComplete(
            () =>
            {
                OnUIDidFinish();
            }
        );
    }
    public void OnClickBtnBack()
    {
        PopViewController pop = (PopViewController)this.controller;
        if (pop != null)
        {
            pop.Close();
        }
        AdKitCommon.main.ShowAdBanner(true);
    }

    public override void LayOut()
    {
        base.LayOut();
        float x, y, w, h;
        float ratio = 1f;
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;

        float page_w, page_h;
        {
            RectTransform rctran = objContent.GetComponent<RectTransform>();
            if (Device.isLandscape)
            {
                ratio = 0.8f;
                w = this.frame.width * ratio;//Mathf.Min(this.frame.width, this.frame.height) * 0.7f;
                h = this.frame.height * ratio;//w;
            }
            else
            {
                ratio = 0.7f;
                w = this.frame.width * ratio;//Mathf.Min(this.frame.width, this.frame.height) * 0.7f;
                h = this.frame.height * ratio;//w;
            }

            rctran.sizeDelta = new Vector2(w, h);


            // //更新scrollview 内容的长度 
            rctran = objScrollViewContent.GetComponent<RectTransform>();
            Vector2 size = rctran.sizeDelta;
            size.x = w * totalPage;
            rctran.sizeDelta = size;
            page_w = w;
            page_h = h;
            if (listPage != null)
            {
                foreach (PageInfo info in listPage)
                {
                    UIHowToPlayPageBase ui = info.ui;
                    ui.width = page_w;
                    ui.heigt = page_h;
                    RectTransform rctranPage = ui.GetComponent<RectTransform>();
                    rctranPage.sizeDelta = new Vector2(page_w, page_h);
                    ui.LayOut();
                }
            }


        }


    }


    void ScrollViewValueChanged(Vector2 newScrollValue)
    {
        indexPage = GetScrollViewPage();
        Debug.Log("ScrollViewValueChanged:" + " page=" + indexPage + " pos=" + scrollRect.content.anchoredPosition);
        uiScrollDot.UpdateItem(indexPage);

    }

    int GetScrollViewPage()
    {
        int ret = 0;
        RectTransform rctran = objContent.GetComponent<RectTransform>();
        float w = rctran.rect.width;
        float v = Mathf.Abs(scrollRect.content.anchoredPosition.x / w);
        int page = (int)v;
        float dif = v - page;
        ret = dif < 0.5f ? page : (page + 1);
        if (ret >= totalPage)
        {
            ret = totalPage - 1;
        }
        return ret;
    }

    void SetScrollViewPage(int page)
    {
        indexPagePre = indexPageCur;
        indexPageCur = page;
        RectTransform rctran = objContent.GetComponent<RectTransform>();
        float w = rctran.rect.width;
        scrollRect.content.anchoredPosition = new Vector2(-w * page, 0);
    }
    public void OnScrollViewDrag(PointerEventData eventData, int status)
    {

        if (status == UIScrollViewTouchEvent.DRAG_END)
        {
            Debug.Log("OnScrollViewDrag pos=" + eventData.position);
            SetScrollViewPage(indexPage);
            if (listPage != null)
            {
                Debug.Log("OnScrollViewDrag indexPagePre= " + indexPagePre + " indexPage=" + indexPage);
                PageInfo infoPre = listPage[indexPagePre] as PageInfo;
                UIHowToPlayPageBase uiPre = infoPre.ui;
                uiPre.OnPageExit();


                PageInfo info = listPage[indexPageCur] as PageInfo;
                UIHowToPlayPageBase ui = info.ui;
                ui.OnPageEnter();
            }

        }

    }
}
