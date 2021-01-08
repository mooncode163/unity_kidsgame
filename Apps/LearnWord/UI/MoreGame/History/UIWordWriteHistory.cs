using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;
public delegate void OnUIWordWriteHistoryDidCloseDelegate(UIWordWriteHistory ui);
public class UIWordWriteHistory : UIView, ITableViewDataSource
{
    public const int SORT_TYPE_WORD = 0;
    public const int SORT_TYPE_DATE = 1;
    public Button btnWord;
    public Button btnDate;
    public Button btnFreeWrite;
    public Button btnCleanDB; 
    public TableView tableViewSort;
    public TableView tableViewWord;
 
    public UIText textDB;
    public Image imageBar;
    public GameObject objTopBar;
    public OnUIWordWriteHistoryDidCloseDelegate callbackClose { get; set; }
    UICellItemBase cellItemPrefab;
    UICellBase cellPrefab;
    public int numRows;
    private int numInstancesCreated = 0;

    private int oneCellNumWord;
    private int heightCellWord;

    private int oneCellNumSort;
    private int heightCellSort;

    int totalItemSort;
    int totalItemWord;

    int oneCellNum;
    int heightCell;
    int totalItem;

    List<object> listSort;
    List<object> listWord;

    float tableViewSortOffsetYNormal;
    float tableViewWordOffsetYNormal;
    Vector2 offsetMinNormalTableViewSort;
    Vector2 offsetMaxNormalTableViewSort;
    Vector2 offsetMinNormalTableViewWord;
    Vector2 offsetMaxNormalTableViewWord;
    int sortType;

    UIWordWrite.Mode gameMode;

    UIHistorySaveImage uiHistorySaveImagePrefab;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        LoadPrefab();
        listSort = new List<object>();
        listWord = new List<object>();
        heightCellSort = 200;
        heightCellWord = 400;
        oneCellNumSort = 4;
 
        GameObject obj = PrefabCache.main.LoadByKey("UIHistorySaveImage");
        if (obj != null)
        {
            uiHistorySaveImagePrefab = obj.GetComponent<UIHistorySaveImage>();
        }
 
        imageBar.gameObject.SetActive(false);
        gameMode = UIWordWrite.Mode.ALL_STROKE;
        //bg
 
 
  
        tableViewSort.dataSource = this;

        oneCellNumWord = 4;
        tableViewWord.dataSource = this;

    }
    // Use this for initialization
    void Start()
    {
        InitUI();
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
            GameObject obj = PrefabCache.main.LoadByKey("UIWordWriteHistoryCellItem");
            cellItemPrefab = obj.GetComponent<UICellItemBase>();
        }

    }

    void UpdateDBStatus()
    {
        if (DBHistory.main.DBEmpty())
        {
            btnDate.gameObject.SetActive(false);
            btnWord.gameObject.SetActive(false);
        }
        else
        {
            btnDate.gameObject.SetActive(true);
            btnWord.gameObject.SetActive(true);
        }


        if (DBHistory.mainFreeWrite.DBEmpty())
        {
            btnFreeWrite.gameObject.SetActive(false);
        }
        else
        {
            btnFreeWrite.gameObject.SetActive(true);
        }

        btnCleanDB.gameObject.SetActive(true);
        textDB.gameObject.SetActive(false);
        if ((DBHistory.main.DBEmpty()) && (DBHistory.mainFreeWrite.DBEmpty()))
        {
            textDB.gameObject.SetActive(true);
            btnCleanDB.gameObject.SetActive(false);
        }
    }
    void InitUI()
    {

        UpdateDBStatus();


        LayOut();

        //init 
        if (!DBHistory.main.DBEmpty())
        {
            OnClickBtnWord();
        }
        else
        {
            OnClickBtnFreeWrite();
        }

    }
    public override void LayOut()
    {
        base.LayOut();
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
     

        //iphonex
        float ofty_top = Common.ScreenToCanvasHeigt(sizeCanvas, Device.heightSystemTopBar);
        float ofty_left = ofty_top;



    }

    void ShowSaveImage(WordItemInfo info)
    {
        HistorySaveImageViewController controllerSaveImage = HistorySaveImageViewController.main;
        controllerSaveImage.Show(null, null);
        UIHistorySaveImage uiRun = (UIHistorySaveImage)controllerSaveImage.ui;
        uiRun.callBackDelete = OnUIHistorySaveImageDidDelete;
        uiRun.UpdateItem(info);

        uiRun.ShowBtnRetry(true);
        if (gameMode == UIWordWrite.Mode.FREE_WRITE)
        {
            uiRun.ShowBtnRetry(false);
        }
    }
    public void OnClose()
    {
        // PopViewController pop = (PopViewController)this.controller;
        // if (pop != null)
        // {
        //     pop.Close();
        // }

        NaviViewController navi = this.controller.naviController;
        if (navi != null)
        {
            navi.Pop();
        }
    }
    public void OnClickBtnBack()
    {

        OnClose();

    }

    void UpdateListSortByWord()
    {
        List<WordItemInfo> listtmp = GetDB().GetAllWord();
        listSort.Clear();
        foreach (WordItemInfo info in listtmp)
        {
            listSort.Add(info);
        }
    }

    void UpdateListSortByDate()
    {
        List<WordItemInfo> listtmp = GetDB().GetAllDate();
        listSort.Clear();
        foreach (WordItemInfo info in listtmp)
        {
            listSort.Add(info);
        }
    }

    public void OnClickBtnWord()
    {
        gameMode = UIWordWrite.Mode.ALL_STROKE;

        UpdateListSortByWord();
        heightCell = heightCellWord;
        sortType = SORT_TYPE_WORD;
        tableViewSort.ReloadData();

        UpdateListWord(0);
        //  Debug.Log("OnClickBtnWord:listWord=" + listWord.Count);
    }
    public void OnClickBtnDate()
    {
        gameMode = UIWordWrite.Mode.ALL_STROKE;
        UpdateListSortByDate();
        heightCell = heightCellSort;
        sortType = SORT_TYPE_DATE;
        tableViewSort.ReloadData();

        UpdateListWord(0);
    }

    public void OnClickBtnFreeWrite()
    {
        gameMode = UIWordWrite.Mode.FREE_WRITE;
        UpdateListSortByDate();

        sortType = SORT_TYPE_DATE;
        tableViewSort.ReloadData();

        UpdateListWord(0);
    }

    public void OnClickBtnClear()
    {

        string title = Language.main.GetString("STR_UIVIEWALERT_TITLE_CLEAR_DB");
        string msg = Language.main.GetString("STR_UIVIEWALERT_MSG_CLEAR_DB");
        string yes = Language.main.GetString("STR_UIVIEWALERT_YES_CLEAR_DB");
        string no = Language.main.GetString("STR_UIVIEWALERT_NO_CLEAR_DB");
        ViewAlertManager.main.ShowFull(title, msg, yes, no, true, "STR_KEYNAME_VIEWALERT", OnUIViewAlertFinished);


    }
    DBHistory GetDB()
    {
        DBHistory db = DBHistory.main;
        if (gameMode == UIWordWrite.Mode.ALL_STROKE)
        {
            db = DBHistory.main;
        }
        if (gameMode == UIWordWrite.Mode.FREE_WRITE)
        {
            db = DBHistory.mainFreeWrite;
        }

        return db;
    }

    void ClearDB()
    {
        GetDB().ClearDB();
        if (listSort != null)
        {
            listSort.Clear();
            tableViewSort.ReloadData();
        }
        if (listWord != null)
        {
            listWord.Clear();
            tableViewWord.ReloadData();
        }
        UpdateDBStatus();
    }
    void GotoWordWrite(WordItemInfo info)
    {
        GameLevelParse.main.ParseGuanka();

        int idx = GameLevelParse.main.GetGuankaIndexByWord(info);
        WordItemInfo infoGuanka = GameLevelParse.main.GetGuankaItemInfo(idx) as WordItemInfo;
        GameLevelParse.main.ParseItem(infoGuanka);

        infoGuanka.dbInfo.filesave = info.dbInfo.filesave;
        // UIWordWriteFinish.wordInfo = infoGuanka;
        LevelManager.main.gameLevel = idx;

        OnClose();

        GotoGame(UIWordWrite.Mode.ALL_STROKE);
    }


    public void GotoGame(UIWordWrite.Mode mode)
    {
        // GameManager.main.gameMode = mode;
        UIViewController controller = null;
        controller = WriteViewController.main;

        GuankaViewController.main.toController = WriteViewController.main;
        WriteViewController.main.mode = mode;
        AudioPlay.main.PlayFile(AppRes.AUDIO_BTN_CLICK);
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            navi.source = AppRes.SOURCE_NAVI_HISTORY;
            navi.Push(controller);
            WriteViewController.main.ShowBtnBack(true);

        }
    }
    void UpdateListWord(int idx)
    {
        if ((listSort != null) && (listSort.Count > 0))
        {
            WordItemInfo info = listSort[idx] as WordItemInfo;
            switch (sortType)
            {
                case SORT_TYPE_WORD:
                    {
                        List<WordItemInfo> listtmp = GetDB().GetItemsOfWord(info.dbInfo.id);
                        listWord.Clear();
                        foreach (WordItemInfo infotmp in listtmp)
                        {
                            listWord.Add(infotmp);
                        }
                    }

                    break;
                case SORT_TYPE_DATE:
                    {
                        List<WordItemInfo> listtmp = GetDB().GetItemsOfDate(info.dbInfo.date);
                        listWord.Clear();
                        foreach (WordItemInfo infotmp in listtmp)
                        {
                            listWord.Add(infotmp);
                        }
                    }

                    break;
            }
            Debug.Log("CellItemDidClick count = " + listWord.Count + " info.id=" + info.id + " info.date=" + info.date);
            tableViewWord.ReloadData();
        }
        else
        {
            if (listWord != null)
            {
                listWord.Clear();
            }
            tableViewWord.ReloadData();
        }
    }

    public void OnCellItemDidClick(UICellItemBase item)
    {
        if (item.IsLock())
        {
            return;
        }

        UIWordWriteHistoryCellItem itemCell = item as UIWordWriteHistoryCellItem;
        if (itemCell.tableView == tableViewSort)
        {
            Debug.Log("CellItemDidClick tableViewSort ");
            UpdateListWord(item.index);
        }
        if (itemCell.tableView == tableViewWord)
        {
            Debug.Log("CellItemDidClick tableViewWord ");
            WordItemInfo info = listWord[item.index] as WordItemInfo;
            ShowSaveImage(info);

        }

    }
    public void OnUIHistorySaveImageDidDelete(UIHistorySaveImage ui, int btnIndex)
    {
        WordItemInfo info = ui.itemInfo;
        if (btnIndex == UIHistorySaveImage.BTN_INDEX_DELETE)
        {

            GetDB().DeleteItem(info.dbInfo);
            if (listWord != null)
            {
                listWord.Remove(info);
                tableViewWord.ReloadData();
            }
            if (gameMode == UIWordWrite.Mode.FREE_WRITE)
            {
                OnClickBtnFreeWrite();
            }
            else
            {
                if (sortType == SORT_TYPE_WORD)
                {
                    OnClickBtnWord();
                }
                if (sortType == SORT_TYPE_DATE)
                {
                    OnClickBtnDate();
                }
            }

        }

        if (btnIndex == UIHistorySaveImage.BTN_INDEX_RETRY)
        {
            GotoWordWrite(info);
        }

    }

    #region ITableViewDataSource

    //Will be called by the TableView to know how many rows are in this table
    public int GetNumberOfRowsForTableView(TableView tableView)
    {

        int total = 0;

        if (tableView == tableViewSort)
        {
            oneCellNum = oneCellNumSort;

            if (listSort != null)
            {
                total = listSort.Count;
                // Debug.Log("total=" + total);
            }
            else
            {
                Debug.Log("listSort is null");
            }

            totalItemSort = total;
        }
        if (tableView == tableViewWord)
        {
            oneCellNum = oneCellNumWord;
            if (listWord != null)
            {
                total = listWord.Count;

            }
            totalItemWord = total;
        }
        Rect rctable = (tableView.transform as RectTransform).rect;
        // float h_cell = GetHeightForRowInTableView(tableView, 0);
        // if (h_cell > 0)
        // {
        //     oneCellNum = (int)(rctable.width / h_cell);
        // }
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
        if (tableView == tableViewSort)
        {
            Debug.Log("heightCellSort=" + heightCellSort);
            heightCell = heightCellSort;
            return heightCellSort;
        }
        if (tableView == tableViewWord)
        {
            heightCell = heightCellWord;
            return heightCellWord;
        }
        return 0;
        //return (cellPrefab.transform as RectTransform).rect.height;
    }


    public TableViewCell GetCellForRowInTableView(TableView tableView, int row)
    {
        UICellBase cell = tableView.GetReusableCell(cellPrefab.reuseIdentifier) as UICellBase;
        int itemType = 0;
        List<object> listItem = null;

        if (tableView == tableViewSort)
        {
            heightCell = heightCellSort;
            listItem = listSort;
            totalItem = listSort.Count;
            itemType = UIWordWriteHistoryCellItem.ITEM_TYPE_SORT;
        }
        if (tableView == tableViewWord)
        {
            heightCell = heightCellWord;
            listItem = listWord;
            totalItem = listWord.Count;
            itemType = UIWordWriteHistoryCellItem.ITEM_TYPE_WORD;
        }

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

                UIWordWriteHistoryCellItem itemCell = item as UIWordWriteHistoryCellItem;
                itemCell.tableView = tableView;
                itemCell.SetItemType(itemType);


            }

        }
        cell.totalItem = totalItem;
        cell.oneCellNum = oneCellNum;
        cell.rowIndex = row;
        cell.UpdateItem(listItem);
        return cell;
    }
    /* 
        //Will be called by the TableView when a cell needs to be created for display
        public TableViewCell GetCellForRowInTableView(TableView tableView, int row)
        {
            int itemType = 0;
            List<WordItemInfo> listArray = null;
            int oneCellNum = 0;
            if (tableView == tableViewSort)
            {
                //oneCellNum = oneCellNumSort;
                listArray = listSort;
                itemType = WordWriteHistoryCellItem.ITEM_TYPE_SORT;
            }
            if (tableView == tableViewWord)
            {
                //oneCellNum = oneCellNumWord;
                listArray = listWord;
                itemType = WordWriteHistoryCellItem.ITEM_TYPE_WORD;
            }
            Rect rctable = (tableView.transform as RectTransform).rect;
            float h_cell = GetHeightForRowInTableView(tableView, row);
            WordWriteHistoryCell cell = tableView.GetReusableCell(cellPrefab.reuseIdentifier) as WordWriteHistoryCell;
            if (cell == null)
            {
                cell = (WordWriteHistoryCell)GameObject.Instantiate(cellPrefab);
                cell.name = "WordWriteHistoryCell" + (++numInstancesCreated).ToString();
                Rect rccell = (cellPrefab.transform as RectTransform).rect;

                Vector2 sizeCell = (cellPrefab.transform as RectTransform).sizeDelta;
                Vector2 sizeTable = (tableView.transform as RectTransform).sizeDelta;
                Vector2 sizeCellNew = sizeCell;
                sizeCellNew.x = rctable.width;

                if (h_cell > 0)
                {
                    oneCellNum = (int)(rctable.width / h_cell);
                }

                //(cell.transform as RectTransform).sizeDelta = sizeCellNew;
                cell.SetCellSize(sizeCellNew);

                //Debug.LogFormat("TableView Cell Add Item:rcell:{0}, sizeCell:{1},rctable:{2},sizeTable:{3}", rccell, sizeCell, rctable, sizeTable);
                //oneCellNum = (int)(rctable.width / rccell.height);
                //int i =0;
                for (int i = 0; i < oneCellNum; i++)
                {

                    WordWriteHistoryCellItem item = cellItemPrefab;
                    item = (WordWriteHistoryCellItem)GameObject.Instantiate(item);
                    item.iDelegate = this;
                    item.tableView = tableView;
                    item.SetItemType(itemType);
                    Rect rcItem = (item.transform as RectTransform).rect;
                    item.transform.SetParent(cell.transform, false);
                    item.index = row * oneCellNum + i;
                    // LayoutElement layoutElement = item.GetComponent<LayoutElement>();
                    // if (layoutElement == null) {
                    //     layoutElement = item.gameObject.AddComponent<LayoutElement>();
                    // }
                    // layoutElement.preferredWidth =160;// GetHeightForRowInTableView(tableView,row);
                    //  item.transform.SetSiblingIndex(1);


                    RectTransform rectTransform = item.GetComponent<RectTransform>();
                    //RectTransform rectTransform = item.transform as RectTransform;
                    // Vector2 size = rectTransform.sizeDelta * scaleUI;
                    Vector3 pos = new Vector3(rcItem.width * i, 0, 0);

                    // rectTransform.position = pos;
                    rectTransform.anchoredPosition = pos;
                    cell.AddItem(i, item);


                    Vector2 sizeItem = (item.transform as RectTransform).sizeDelta;
                    //Debug.LogFormat("TableView Item:rcItem:{0}, sizeItem:{1},rectTransform.position:{2}", rcItem, sizeItem, rectTransform.position);
                    rcItem.x = 0;
                    rcItem.y = 0;
                    // (item.transform as RectTransform).rect = rcItem;

                }

            }

            if (h_cell > 0)
            {
                oneCellNum = (int)(rctable.width / h_cell);
            }



            int cellNumCur = oneCellNum;
            if (row == GetNumberOfRowsForTableView(tableView) - 1)
            {

                cellNumCur = listArray.Count - (GetNumberOfRowsForTableView(tableView) - 1) * oneCellNum;
            }

            cell.SetRowNumber(row, oneCellNum, cellNumCur);
            cell.UpdateItem(listArray);
            return cell;
        }
    */

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
