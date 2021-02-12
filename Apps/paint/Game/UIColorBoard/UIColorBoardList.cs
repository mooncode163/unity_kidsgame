using System.Collections;
using System.Collections.Generic;
using LitJson;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;

public delegate void OnUIColorBoardListDidClickDelegate(UIColorBoardList ui, UIColorBoardCellItem item, bool isOutSide);


public class UIColorBoardList : UIView, ITableViewDataSource, ISegmentDelegate
{
    public UISegment segment;
    public TableView tableView;
    UICellItemBase cellItemPrefab;
    UICellBase cellPrefab;
    public int numRows;
    private int numInstancesCreated = 0;

    private int oneCellNum;
    private int heightCell;

    int totalItem;
    List<ColorItemInfo> listSort;

    List<object> listItem;

    public OnUIColorBoardListDidClickDelegate callBackClick { get; set; }


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        LoadPrefab();
        heightCell = 160;
        listItem = new List<object>();

        segment.InitValue(64, Color.red, Color.black);
        segment.iDelegate = this;
        tableView.dataSource = this;


    }
    // Use this for initialization
    void Start()
    {
        ParserColorSortList();
        SegmentDidClick(segment, segment.GetItem(0));
        LayoutChild();
    }

    // Update is called once per frame
    void Update()
    {

        // mobile touch
        if ((Input.touchCount > 0))
        {

            {
                //单点触摸
                switch (Input.touches[0].phase)
                {
                    case TouchPhase.Began:
                        onTouchDown();
                        break;

                    case TouchPhase.Moved:
                        onTouchMove();
                        break;

                    case TouchPhase.Ended:
                        onTouchUp();
                        break;

                }
            }
        }


        //pc mouse
        //#if UNITY_EDITOR
        if ((Input.touchCount == 0))
        {
            if (Input.GetMouseButtonUp(0))
            {
                onTouchUp();
            }



            if (Input.GetMouseButtonDown(0))
            {
                //  Debug.Log("Input:" + Input.mousePosition);
                onTouchDown();
            }


            if (Input.GetMouseButton(0))
            {
                onTouchMove();

            }

        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnClickBtnBack();
        }

    }


    void LoadPrefab()
    {
        {
            GameObject obj = PrefabCache.main.Load(AppCommon.PREFAB_UICELLBASE);
            cellPrefab = obj.GetComponent<UICellBase>();
        }
        {
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/Game/UIColorBoardCellItem");

            cellItemPrefab = obj.GetComponent<UICellItemBase>();
        }

    }

    void onTouchDown()
    {
        //ScreenPointToLocalPointInRectangle
    }
    void onTouchMove()
    {

    }
    void onTouchUp()
    {
        Vector2 sizeCanvas = this.frame.size;
        RectTransform rctran = GetComponent<RectTransform>();
        float width = rctran.rect.width;
        float w_sceent = Common.CanvasToScreenWidth(sizeCanvas, width);
        float height = rctran.rect.height;
        float h_sceent = Common.CanvasToScreenHeight(sizeCanvas, height);
        Vector2 pt_input = Common.GetInputPosition();
        bool isclick_outside = false;

        // if (Device.isLandscape)
        // {
        //     if (pt_input.x < (Screen.width - w_sceent))
        //     {
        //         isclick_outside = true;
        //     }
        // }
        // else
        // {
        //     if (pt_input.y > h_sceent)
        //     {
        //         isclick_outside = true;
        //     }
        // }

        Vector2 posCanvas = Common.ScreenToCanvasPoint(sizeCanvas, pt_input);
        posCanvas.x -= sizeCanvas.x / 2;
        posCanvas.y -= sizeCanvas.y / 2;
        Rect rc = new Rect(rctran.anchoredPosition.x - rctran.rect.size.x / 2, rctran.anchoredPosition.y - rctran.rect.size.y / 2, rctran.rect.size.x, rctran.rect.size.y);
        if (!rc.Contains(posCanvas))
        {
            isclick_outside = true;
        }
        Debug.Log("UIColorBoard rc=" + rc + " center=" + rc.center + " posCanvas=" + posCanvas);
        if (isclick_outside)//&& (pt_input.y < (Screen.height - 160)
        {
            // if (this.gameObject.transform.parent.gameObject.activeSelf)
            // {
            //     Debug.Log("onTouchUp click hide height=" + height + " h_sceent=" + h_sceent);
            //     this.gameObject.transform.parent.gameObject.SetActive(false);
            // }


            if (callBackClick != null)
            {
                callBackClick(this, null, true);
            }
        }
    }

    public void LayoutChild()
    {
        Vector2 sizeCanvas = this.frame.size;
        float x = 0, y = 0, w = 0, h = 0, w_parent = 0, h_parent = 0;
        RectTransform rcTranParent = this.gameObject.transform.parent as RectTransform;
        w_parent = rcTranParent.rect.width;
        h_parent = rcTranParent.rect.height;
        //uicolorBoardList
        {
            // float topbar_h_canvas = 160;
            // sizeCanvas.x = w_parent;
            // sizeCanvas.y = h_parent;
            // RectTransform rctran = this.gameObject.transform as RectTransform;

            // w = w_parent;
            // h = h_parent / 2;
            // //x = 2048;
            // //y = 768;
            // rctran.sizeDelta = new Vector2(w, h);
            // float oft = topbar_h_canvas + Common.ScreenToCanvasHeigt(sizeCanvas, Device.offsetBottom);
            // {
            //     x = 0;
            //     y = -sizeCanvas.y / 2 + h / 2 + oft;
            // }
            // rctran.anchoredPosition = new Vector2(x, y);

        }
    }

    public void ParserColorSortList()
    {
        string jsonFile = CloudRes.main.rootPathGameRes + "/colorboard/colorpacks.json";
        string json = FileUtil.ReadStringAsset(jsonFile);//((TextAsset)Resources.Load(fileName, typeof(TextAsset))).text;
        // Debug.Log("json::"+json);
        JsonData root = JsonMapper.ToObject(json);
        //strPlace = (string)root["place"];
        JsonData items = root["colorpacks"];

        listSort = new List<ColorItemInfo>();
        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];
            ColorItemInfo info = new ColorItemInfo();

            info.title = (string)item["title"];
            info.name = (string)item["name"];
            //color

            JsonData colors = item["colors"];
            //IList<JsonData> colors = (IList<JsonData>)items["colors"];
            info.listColor = new List<Color>();
            for (int j = 0; j < colors.Count; j++)
            {
                //#fac9c4
                string str_color = (string)colors[j];
                //hex to string
                //fac9c4 to r,g,b
                string strR = str_color.Substring(1, 2);
                string strG = str_color.Substring(3, 2);
                string strB = str_color.Substring(5, 2);
                int intR = int.Parse(strR, System.Globalization.NumberStyles.HexNumber);
                int intG = int.Parse(strG, System.Globalization.NumberStyles.HexNumber);
                int intB = int.Parse(strB, System.Globalization.NumberStyles.HexNumber);
                //Debug.Log(str_color+"Hex:R="+strR+" G="+strG+" B="+strB+" \nRGB:R="+intR+" G="+intG+" B="+intB);
                Color color = new Color(intR / 255f, intG / 255f, intB / 255f, 1f);
                //Debug.Log(color);
                info.listColor.Add(color);

            }

            listSort.Add(info);
            if (segment != null)
            {
                ItemInfo infoSeg = new ItemInfo();
                infoSeg.title = Language.main.GetString(info.title);
                segment.AddItem(infoSeg);
            }
        }
        if (segment != null)
        {
            segment.UpdateList();
        }
        Debug.Log("colors count = " + listSort.Count);

    }
    public void UpdateColorList(int idx)
    {
        ColorItemInfo info = (ColorItemInfo)listSort[idx];
        listItem.Clear();
        foreach (Color cr in info.listColor)
        {
            listItem.Add(cr);
        }
        totalItem = listItem.Count;
        Debug.Log("UpdateColorList totalItem = " + totalItem);
        tableView.ReloadData();

    }
    public void OnClickBtnBack()
    {
        this.gameObject.SetActive(false);
    }
    public void OnClickImageBg()
    {
        // 
    }
    public void SegmentDidClick(UISegment seg, SegmentItem item)
    {
        UpdateColorList(item.index);

    }

    public void OnCellItemDidClick(UICellItemBase item)
    {
        if (callBackClick != null)
        {
            callBackClick(this, (UIColorBoardCellItem)item, false);
        }

    }

    #region ITableViewDataSource

    //Will be called by the TableView to know how many rows are in this table
    public int GetNumberOfRowsForTableView(TableView tableView)
    {

        int total = 0;
        if (listItem != null)
        {
            total = listItem.Count;

        }

        Rect rctable = (tableView.transform as RectTransform).rect;
        float h_cell = GetHeightForRowInTableView(tableView, 0);
        if (h_cell > 0)
        {
            oneCellNum = (int)(rctable.width / h_cell);
        }
        Debug.Log("oneCellNum=" + oneCellNum + " h_cell=" + h_cell + " rctable.width=" + rctable.width);
        numRows = total / oneCellNum;
        if (total % oneCellNum != 0)
        {
            numRows++;
        }
        // 
        return numRows;
    }

    //Will be called by the TableView to know what is the height of each row
    public float GetHeightForRowInTableView(TableView tableView, int row)
    {
        //return 200f;

        return heightCell;
        //return (cellPrefab.transform as RectTransform).rect.height;
    }

    //Will be called by the TableView when a cell needs to be created for display

    public TableViewCell GetCellForRowInTableView(TableView tableView, int row)
    {
        UICellBase cell = tableView.GetReusableCell(cellPrefab.reuseIdentifier) as UICellBase;

        if (cell == null)
        {

            cell = (UICellBase)GameObject.Instantiate(cellPrefab);
            cell.name = "UICellBase" + (++numInstancesCreated).ToString();
            Rect rccell = (cellPrefab.transform as RectTransform).rect;
            Rect rctable = (tableView.transform as RectTransform).rect;
            Vector2 sizeCell = (cellPrefab.transform as RectTransform).sizeDelta;
            Vector2 sizeTable = (tableView.transform as RectTransform).sizeDelta;
            Vector2 sizeCellNew = sizeCell;
            sizeCellNew.x = rctable.width;

            Debug.Log("GetCellForRowInTableView null row=" + row + " oneCellNum=" + oneCellNum);
            for (int i = 0; i < oneCellNum; i++)
            {
                int itemIndex = row * oneCellNum + i;
                float cell_space = 10;
                UICellItemBase item = (UICellItemBase)GameObject.Instantiate(cellItemPrefab);
                //item.itemDelegate = this;
                Rect rcItem = (item.transform as RectTransform).rect;
                item.width = (rctable.width - cell_space * (oneCellNum - 1)) / oneCellNum;
                item.height = heightCell;
                item.transform.SetParent(cell.transform, false);
                item.index = itemIndex;
                item.totalItem = totalItem;
                item.callbackClick = OnCellItemDidClick;
                Debug.Log("cell.AddItem i=" + i);
                cell.AddItem(item);

            }

        }
        cell.totalItem = totalItem;
        cell.oneCellNum = oneCellNum;
        cell.rowIndex = row;
        cell.UpdateItem(listItem);
        return cell;
    }

    #endregion

    #region Table View event handlers

    //Will be called by the TableView when a cell's visibility changed
    public void TableViewCellVisibilityChanged(int row, bool isVisible)
    {
        //Debug.Log(string.Format("Row {0} visibility changed to {1}", row, isVisible));
        if (isVisible)
        {

        }
    }

    #endregion

}
