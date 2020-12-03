using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;

public class UISegment : UIView, ISegmentItemDelegate
{
    public TableView tableView;
    public GameObject objContent;
    public SegmentItem segmentItem;
    public int numRows;
    public int indexSelect;

    Color colorSel = Color.red;
    Color colorUnSel = Color.white;
    int itemFontSize = 24;

    private int numInstancesCreated = 0;

    private int oneCellNum;
    private int heightCell;
    int totalItem;
    int totalStringWidth;
    ScrollRect scrollRect;
    private List<SegmentItem> listItem;
    private ISegmentDelegate _delegate;

    public ISegmentDelegate iDelegate
    {
        get { return _delegate; }
        set { _delegate = value; }
    }

    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (listItem == null)
        {
            listItem = new List<SegmentItem>();
            //return;
        }
        heightCell = 128;
        oneCellNum = 1;
        scrollRect = GetComponent<ScrollRect>();
        // tableView.dataSource = this;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitValue(int fontsize, Color sel, Color unsel)
    {
        itemFontSize = fontsize;
        colorSel = sel;
        colorUnSel = unsel;
    }
    public void AddItem(ItemInfo info)
    {
        if (listItem == null)
        {
            listItem = new List<SegmentItem>();
            //return;
        }
        if (listItem.Count == 0)
        {
            totalStringWidth = 0;
        }
        if (scrollRect == null)
        {
            scrollRect = GetComponent<ScrollRect>();
        }

        //横向滑动
        int space_x = 10;
        SegmentItem item = (SegmentItem)GameObject.Instantiate(segmentItem);
        item.iDelegate = this;
        item.index = listItem.Count;
        listItem.Add(item);
        item.transform.parent = objContent.transform;
        item.colorSel = colorSel;
        item.colorUnSel = colorUnSel;
        item.textTitle.fontSize = itemFontSize;
        item.UpdateInfo(info);
        int str_width = Common.GetStringLength(info.title, AppString.STR_FONT_NAME, itemFontSize);
        int offsetx = space_x * listItem.Count + totalStringWidth;
        totalStringWidth += str_width;
        RectTransform rctranContent = objContent.transform as RectTransform;
        float left_x = -rctranContent.rect.width / 2;

        RectTransform rctran = item.transform as RectTransform;
        rctran.sizeDelta = new Vector2(str_width, rctranContent.rect.height);
        float x = left_x + offsetx + rctran.sizeDelta.x / 2;
        Vector2 pos = new Vector2(x, 0);
        //item.transform.position = pos;

        rctran.localScale = new Vector3(1f, 1f, 1f);
        //rctran.anchoredPosition = pos;

        //scrollRect.content.sizeDelta = new Vector2(space_x*listItem.Count+totalStringWidth,scrollRect.content.sizeDelta.y);


    }

    public void ShowItemImageBg(bool isShow)
    {
        foreach (SegmentItem item in listItem)
        {
            item.ShowImageBg(isShow);
        }
    }

    public void UpdateList()
    {
        Select(0, true);
        //totalItem = listItem.Count;
        numRows = totalItem;
        // tableView.ReloadData();
    }
    public SegmentItem GetItem(int idx)
    {
        SegmentItem item_ret = null;
        foreach (SegmentItem item in listItem)
        {
            if (idx == item.index)
            {
                item_ret = item;
                break;
            }
        }
        return item_ret;
    }

    public int GetCount()
    {
        int count = 0;
        if (listItem != null)
        {
            count = listItem.Count;
        }
        return count;
    }
    public void Select(int idx, bool isClick = false)
    {
        indexSelect = idx;
        foreach (SegmentItem item in listItem)
        {
            if (idx == item.index)
            {
                item.SetSelect(true);
                if (isClick)
                {
                    item.OnClick();
                }
                //break;
            }
            else
            {
                item.SetSelect(false);
            }
        }
    }

    #region SegmentItemDelegate 

    // public void SegmentCellDidClick(SegmentCell cell,SegmentCellItem item)
    // {
    //       if (_delegate != null)
    //     {
    //          print("onItemClick 2");
    //         _delegate.SegmentDidClick(this,item);
    //     }
    // }
    public void SegmentItemDidClick(SegmentItem item)
    {
        if (item == null)
        {
            return;
        }
        Select(item.index);
        if (_delegate != null)
        {
            _delegate.SegmentDidClick(this, item);
        }
    }
    #endregion
}
