using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;
public delegate void OnUIColorHistoryDidCloseDelegate(UIColorHistory ui);
public class UIColorHistory : UIView, ITableViewDataSource
{
    public const int SORT_TYPE_WORD = 0;
    public const int SORT_TYPE_DATE = 1;
    public Image imageBg;
    public TableView tableView;

    public Text textTitle;
    public Text textDB;
    public Image imageBar;
    public Button btnCleanDB;

    public OnUIColorHistoryDidCloseDelegate callbackClose { get; set; }
    UICellItemBase cellItemPrefab;
    UICellBase cellPrefab;
    public int numRows;
    private int numInstancesCreated = 0;

    private int oneCellNum;
    private int heightCell;

    int totalItem;
    List<object> listItem;

    UIHistorySaveImage uiHistorySaveImage;
    int sortType;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        LoadPrefab();
        // listSort = new List<WordItemInfo>();
        // listWord = new List<WordItemInfo>();
        listItem = new List<object>();
        heightCell = 512;
        oneCellNum = 2;
        if (Device.isLandscape)
        {
            oneCellNum *= 2;
        }

        {
            string strPrefab = "AppCommon/Prefab/History/UIHistorySaveImage";
            GameObject obj = (GameObject)Resources.Load(strPrefab);
            if (obj != null)
            {
                uiHistorySaveImage = obj.GetComponent<UIHistorySaveImage>();
            }

        }


        {
            string str = Language.main.GetString("STR_HISTORY_TITLE");
            textTitle.text = str;
            int fontsize = textTitle.fontSize;
            float str_w = Common.GetStringLength(str, AppString.STR_FONT_NAME, fontsize);
            RectTransform rctran = imageBar.transform as RectTransform;
            Vector2 sizeDelta = rctran.sizeDelta;
            float oft = 0;
            sizeDelta.x = str_w + fontsize;
            rctran.sizeDelta = sizeDelta;
            //rctran.anchoredPosition = new Vector2(sizeCanvas.x / 2, rctran.anchoredPosition.y);
        }





        tableView.dataSource = this;

        textDB.text = Language.main.GetString("HISTORY_DB_EMPTY");
        textDB.gameObject.SetActive(false);
        btnCleanDB.gameObject.SetActive(true);
        if (DBColor.main.DBEmpty())
        {
            textDB.gameObject.SetActive(true);
            btnCleanDB.gameObject.SetActive(false);
        }

    }
    // Use this for initialization
    void Start()
    {
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        //bg
        {
            Texture2D tex = LoadTexture.LoadFromAsset(CloudRes.main.rootPathGameRes + "/common/bg.png");
            imageBg.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            RectTransform rctran = imageBg.GetComponent<RectTransform>();
            float w = imageBg.sprite.texture.width;//rectTransform.rect.width;
            float h = imageBg.sprite.texture.height;//rectTransform.rect.height;
            print("imageBg size:w=" + w + " h=" + h);
            w = 2048;
            h = 2048;
            rctran.sizeDelta = new Vector2(w, h);
            float scalex = sizeCanvas.x / w;
            float scaley = sizeCanvas.y / h;
            float scale = Mathf.Max(scalex, scaley);
            //imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);
        }

        if (Device.isLandscape)
        {
            //  Init();

        }
        else
        {
            //横屏 InitScalerMatch 会调整scaler 要保持同步
            // StartCoroutine("InitUiScalerDelay");

            // StartCoroutine("InitDelay");

        }
        Init();
    }

    // Update is called once per frame
    void Update()
    {
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
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/History/UIColorHistoryCellItem");

            cellItemPrefab = obj.GetComponent<UICellItemBase>();
        }

    }

    void Init()
    {
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        //bg
        {

            RectTransform rctran = imageBg.GetComponent<RectTransform>();
            float w = imageBg.sprite.texture.width;//rectTransform.rect.width;
            float h = imageBg.sprite.texture.height;//rectTransform.rect.height;
            print("imageBg size:w=" + w + " h=" + h);
            rctran.sizeDelta = new Vector2(w, h);
            float scalex = sizeCanvas.x / w;
            float scaley = sizeCanvas.y / h;
            float scale = Mathf.Max(scalex, scaley);
            imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);
        }


        //init 
        UpdateList();
    }


    public void OnClickBtnBack()
    {

        {
            NaviViewController navi = this.controller.naviController;
            if (navi != null)
            {
                navi.Pop();
            }
        }


    }


    public void OnClickBtnClear()
    {
        string title = Language.main.GetString("STR_UIVIEWALERT_TITLE_CLEAR_DB");
        string msg = Language.main.GetString("STR_UIVIEWALERT_MSG_CLEAR_DB");
        string yes = Language.main.GetString("STR_UIVIEWALERT_YES_CLEAR_DB");
        string no = Language.main.GetString("STR_UIVIEWALERT_NO_CLEAR_DB");

        ViewAlertManager.main.ShowFull(title, msg, yes, no, true, "STR_KEYNAME_VIEWALERT_CLEAR_DB", OnUIViewAlertFinished);

    }

    void ClearDB()
    {
        Debug.Log("ClearDB start");
        DBColor.main.ClearDB();

        if (listItem != null)
        {
            listItem.Clear();
            tableView.ReloadData();
        }
    }
    public void OnUIHistorySaveImageDidDelete(UIHistorySaveImage ui, int btnIndex)
    {
        if (btnIndex == UIHistorySaveImage.BTN_INDEX_DELETE)
        {
            DBItemInfo info = ui.itemInfo;
            DBColor.main.DeleteItem(info);
            if (listItem != null)
            {
                listItem.Remove(info);
                if (tableView != null)
                {
                    tableView.ReloadData();
                }

            }
        }
    }
    void ShowGamePaintSaveImage(DBItemInfo info)
    {

        // UIHistorySaveImage uiRun = (UIHistorySaveImage)GameObject.Instantiate(uiHistorySaveImage);
        // uiRun.callBackDelete = OnUIHistorySaveImageDidDelete;
        // uiRun.UpdateItem(info);
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            if (navi != null)
            {
                navi.source = AppRes.SOURCE_NAVI_HISTORY;
                HistorySaveImageViewController controller = HistorySaveImageViewController.main;
                controller.infoItem = info;
                controller.callBackDelete = OnUIHistorySaveImageDidDelete;
                navi.Push(controller);
            }

        }
    }
    void GotoGame(ColorItemInfo info)
    { 
        LevelManager.main.ParseGuanka();

        int idx = 0;//game.GetGuankaIndexByWord(info);
        LevelManager.main.gameLevel = idx;
        GameManager.main.GotoGame(this.controller);
    }

    void UpdateList()
    {
        List<DBItemInfo> listDB = DBColor.main.GetAllItem();
        listItem.Clear();
        foreach (DBItemInfo info in listDB)
        {
            listItem.Add(info);
        }
        totalItem = listItem.Count;
        tableView.ReloadData();

    }

    public void OnCellItemDidClick(UICellItemBase item)
    {
        if (item.IsLock())
        {
            return;
        }
        DBItemInfo info = listItem[item.index] as DBItemInfo;
        // GotoGame(info);
        ShowGamePaintSaveImage(info);
    }

    #region ITableViewDataSource

    //Will be called by the TableView to know how many rows are in this table
    public int GetNumberOfRowsForTableView(TableView tableView)
    {

        int total = 0;

        {

            if (listItem != null)
            {
                total = listItem.Count;

            }
            totalItem = total;
        }
        Rect rctable = (tableView.transform as RectTransform).rect;
        float h_cell = GetHeightForRowInTableView(tableView, 0);
        if (h_cell > 0)
        {
            // oneCellNum = (int)(rctable.width / h_cell);
        }
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

        return heightCell;
    }


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

                cell.AddItem(item);

            }

        }
        cell.totalItem = totalItem;
        cell.oneCellNum = oneCellNum;
        cell.rowIndex = row;
        cell.UpdateItem(listItem);
        return cell;
    }

    //Will be called by the TableView when a cell needs to be created for display
    // public TableViewCell GetCellForRowInTableView(TableView tableView, int row)
    // {
    //     // int itemType = 0;
    //     // Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;

    //     // Rect rctable = (tableView.transform as RectTransform).rect;
    //     // float h_cell = GetHeightForRowInTableView(tableView, row);
    //     // ColorHistoryCell cell = tableView.GetReusableCell(cellPrefab.reuseIdentifier) as ColorHistoryCell;
    //     // if (cell == null)
    //     // {
    //     //     cell = (ColorHistoryCell)GameObject.Instantiate(cellPrefab);
    //     //     cell.name = "WordWriteHistoryCell" + (++numInstancesCreated).ToString();
    //     //     Rect rccell = (cellPrefab.transform as RectTransform).rect;

    //     //     Vector2 sizeCell = (cellPrefab.transform as RectTransform).sizeDelta;
    //     //     Vector2 sizeTable = (tableView.transform as RectTransform).sizeDelta;
    //     //     Vector2 sizeCellNew = sizeCell;
    //     //     sizeCellNew.x = rctable.width;

    //     //     if (h_cell > 0)
    //     //     {
    //     //         // oneCellNum = (int)(rctable.width / h_cell);
    //     //     }

    //     //     //(cell.transform as RectTransform).sizeDelta = sizeCellNew;
    //     //     cell.SetCellSize(sizeCellNew);


    //     //     //Debug.LogFormat("TableView Cell Add Item:rcell:{0}, sizeCell:{1},rctable:{2},sizeTable:{3}", rccell, sizeCell, rctable, sizeTable);
    //     //     //oneCellNum = (int)(rctable.width / rccell.height);
    //     //     //int i =0;
    //     //     for (int i = 0; i < oneCellNum; i++)
    //     //     {

    //     //         ColorHistoryCellItem item = cellItemPrefab;
    //     //         item = (ColorHistoryCellItem)GameObject.Instantiate(item);
    //     //         item.iDelegate = this;
    //     //         item.tableView = tableView;
    //     //         item.SetItemType(itemType);
    //     //         Rect rcItem = (item.transform as RectTransform).rect;
    //     //         item.transform.SetParent(cell.transform, false);
    //     //         item.index = row * oneCellNum + i;

    //     //         float cell_space = 10;
    //     //         RectTransform rctran = (tableView.transform as RectTransform);
    //     //         float cell_width = sizeCanvas.x - 16 * 2;
    //     //         cell_width = rctable.width;
    //     //         item.itemWidth = (cell_width - cell_space * (oneCellNum - 1)) / oneCellNum;
    //     //         item.itemHeight = heightCell;
    //     //         Debug.Log("cell_width=" + cell_width + " oneCellNum=" + oneCellNum + " item.itemWidth=" + item.itemWidth + " item.itemHeight=" + item.itemHeight);
    //     //         // LayoutElement layoutElement = item.GetComponent<LayoutElement>();
    //     //         // if (layoutElement == null) {
    //     //         //     layoutElement = item.gameObject.AddComponent<LayoutElement>();
    //     //         // }
    //     //         // layoutElement.preferredWidth =160;// GetHeightForRowInTableView(tableView,row);
    //     //         //  item.transform.SetSiblingIndex(1);


    //     //         RectTransform rectTransform = item.GetComponent<RectTransform>();
    //     //         //RectTransform rectTransform = item.transform as RectTransform;
    //     //         // Vector2 size = rectTransform.sizeDelta * scaleUI;
    //     //         Vector3 pos = new Vector3(rcItem.width * i, 0, 0);

    //     //         // rectTransform.position = pos;
    //     //         rectTransform.anchoredPosition = pos;
    //     //         cell.AddItem(i, item);


    //     //         Vector2 sizeItem = (item.transform as RectTransform).sizeDelta;
    //     //         //Debug.LogFormat("TableView Item:rcItem:{0}, sizeItem:{1},rectTransform.position:{2}", rcItem, sizeItem, rectTransform.position);
    //     //         rcItem.x = 0;
    //     //         rcItem.y = 0;
    //     //         // (item.transform as RectTransform).rect = rcItem;

    //     //     }

    //     // }

    //     // if (h_cell > 0)
    //     // {
    //     //     // oneCellNum = (int)(rctable.width / h_cell);
    //     // }



    //     // int cellNumCur = oneCellNum;
    //     // if (row == GetNumberOfRowsForTableView(tableView) - 1)
    //     // {

    //     //     cellNumCur = listItem.Count - (GetNumberOfRowsForTableView(tableView) - 1) * oneCellNum;
    //     // }

    //     // cell.SetRowNumber(row, oneCellNum, cellNumCur);
    //     // cell.UpdateItem(listItem);
    //     // return cell;
    // }

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



    void OnUIViewAlertFinished(UIViewAlert alert, bool isYes)
    {

        if (isYes)
        {
            ClearDB();
        }
        else
        {

        }



    }

}
