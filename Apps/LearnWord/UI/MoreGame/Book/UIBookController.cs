using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 教材网： http://www.dzkbw.com/books/rjb/yuwen/
public class UIBookController : UITableViewControllerBase, ISegmentDelegate
{
    public const int RATIO_PIC_W = 3;
    public const int RATIO_PIC_H = 4;
    public UISegment uiSegment;
    int indexSegment;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public void Awake()
    {
        base.Awake();
        LayOut();
        uiSegment.InitValue(64, Color.red, Color.black);
        uiSegment.iDelegate = this;
        BookParse.main.ParseSort();
        UpdateBookList();
    }
    public void Start()
    {
        base.Start();
        LayOut();
        RectTransform rctran = this.GetComponent<RectTransform>();
        float width = rctran.rect.width;

        heightCell = 512;
        oneCellNum = (int)(width / heightCell);
        if (((int)width) % heightCell != 0)
        {
            oneCellNum++;
        }

        int w = (int)(width / oneCellNum);
        heightCell = w * RATIO_PIC_H / RATIO_PIC_W;

        UpdateTable(false, false);
        UpdateSegment();
        LayOut();

        OnUIDidFinish();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnClickBtnBack();
        }
    }


    public void UpdateSegment()
    {


        for (int i = 0; i < BookParse.main.listSort.Count; i++)
        {
            ItemInfo infoplace = BookParse.main.listSort[i] as ItemInfo;
            ItemInfo infoSeg = new ItemInfo();
            infoSeg.id = infoplace.id;
            infoSeg.title = Language.main.GetString("Book_" + infoplace.id);
            uiSegment.AddItem(infoSeg);
        }


        if (uiSegment != null)
        {
            uiSegment.UpdateList();
        }
        uiSegment.Select(indexSegment, true);

        if (uiSegment.GetCount() <= 1)
        {
            uiSegment.gameObject.SetActive(false);
        }
    }
    public override void LayOut()
    {
        base.LayOut();

    }
    public void UpdateBookList()
    {
        BookParse.main.ParseBookList(indexSegment);
        listItem = BookParse.main.listBook;
    }
    public void SegmentDidClick(UISegment seg, SegmentItem item)
    {
        indexSegment = item.index;
        UpdateBookList();
        UpdateTable(true, false);

    }
    public override void OnCellItemDidClick(UICellItemBase item)
    {
        if (item.IsLock())
        {
            return;
        }
        // LevelManager.main.placeLevel = item.index;
        ItemInfo info = listItem[item.index] as ItemInfo;
        BookWordViewController.main.infoBook = info;
        Debug.Log("OnCellItemDidClick index=" + item.index + " info.id=" + info.id);
        AudioPlay.main.PlayBtnSound();
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;

            navi.Push(BookWordViewController.main);
        }


    }

    public void OnClickBtnBack()
    {
        if (this.controller.naviController != null)
        {
            this.controller.naviController.Pop();
        }
    }

}
