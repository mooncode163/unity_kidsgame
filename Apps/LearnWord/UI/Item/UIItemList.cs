using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;
// public delegate void OnUILoveControllerDidCloseDelegate(UILoveController ui);
public class UIItemList : UIView, ITableViewDataSource
{
    public OnUILoveControllerDidCloseDelegate callbackClose { get; set; }
    UICellItemBase cellItemPrefab;
    UICellBase cellPrefab;
    public UIImage imageBg;
    public int numRows;
    private int numInstancesCreated = 0;

    int totalItem;
    private int oneCellNum;
    private int heightCell;
    public TableView tableView;

    List<object> listItem;

    Color colorSel;
    Color colorUnSel;
    int indexSegment;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        LoadPrefab();
        colorSel = new Color(1f, 0f, 0f, 1f);
        colorUnSel = new Color(1f, 1f, 1f, 1f);
        //bg 


        heightCell = 256;

        oneCellNum = 2;
        if (!Device.isLandscape)
        {
            oneCellNum = oneCellNum / 2;
        }

        LevelManager.main.ParseGuanka();
        tableView.dataSource = this;

        indexSegment = LevelManager.main.placeLevel;
        if (listItem == null)
        {
            listItem = new List<object>();
        }

        LanguageManager.main.UpdateLanguagePlace();
        LanguageManager.main.UpdateLanguage(indexSegment);
    }

    // Use this for initialization
    void Start()
    {
        if (this.controller != null)
        {
            if (this.controller.naviController != null)
            {
                this.controller.naviController.ShowTitle(true);
                this.controller.naviController.UpdateTitle(Language.main.GetString("STR_ITEM_LIST"));
            }
        }
        UpdateList();

        LayOut();



    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnBtnClickBack();
        }
    }
    void LoadPrefab()
    {
        {
            GameObject obj = PrefabCache.main.Load(AppCommon.PREFAB_UICELLBASE);
            cellPrefab = obj.GetComponent<UICellBase>();
        }
        {
            GameObject obj = PrefabCache.main.Load("App/Prefab/Item/UIItemListCellItem");
            cellItemPrefab = obj.GetComponent<UICellItemBase>();
        }

    }


    public override void LayOut()
    {
        base.LayOut();
        // Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        // {
        //     RectTransform rctran = imageBg.GetComponent<RectTransform>();
        //     float w_image = rctran.rect.width;
        //     float h_image = rctran.rect.height;
        //     float scale = Common.GetMaxFitScale(w_image, h_image, sizeCanvas.x, sizeCanvas.y);
        //     imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);
        //     //屏幕坐标 现在在屏幕中央
        //     imageBg.transform.position = new Vector2(Screen.width / 2, Screen.height / 2);
        // }


    }
    public void SetList(List<object> ls)
    {
        if (listItem == null)
        {
            listItem = new List<object>();
        }
        if (ls == null)
        {
            return;
        }
        listItem.Clear();
        listItem.AddRange(ls);
    }
    public void UpdateList()
    {
        //     if (ls == null)
        //     {
        //         return;
        //     }
        //     listItem.Clear();
        //     listItem.AddRange(ls);
        totalItem = listItem.Count;
        numRows = totalItem / oneCellNum;
        if (totalItem % oneCellNum != 0)
        {
            numRows++;
        }

        tableView.ReloadData();
    }

    public void OnBtnClickBack()
    {
        NaviViewController navi = this.controller.naviController;
        if (navi != null)
        {
            navi.Pop();
        }
    }

    public void OnBtnClickDeleteAll()
    {
        DBLove.main.ClearDB();
        UpdateList();
    }

    public void OnCellItemDidClick(UICellItemBase item)
    {
        if (item.IsLock())
        {
            return;
        }
        DBWordItemInfo info = listItem[item.index] as DBWordItemInfo;
        if (!DBHistory.main.IsItemExist(info))
        {
            DBHistory.main.AddItem(info);
        }
        AudioPlay.main.PlayBtnSound();
        LevelManager.main.gameLevel = LevelParseLearnWord.main.GetLevelByWord(info.id);
        if (LevelManager.main.gameLevel > GameLevelParse.ADVIDEO_LEVEL_MIN)
        {
            AdKitCommon.main.ShowAdVideo();
        }
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            if (navi != null)
            {
                navi.Push(DetailViewController.main);

            }
        }
    }

    public void OnCellItemDidClickDelete(UILoveCellItem ui)
    {
        DBWordItemInfo info = listItem[ui.index] as DBWordItemInfo;
        if (info != null)
        {
            DBLove.main.DeleteItem(info);
        }
        UpdateList();
    }

    #region ITableViewDataSource

    //Will be called by the TableView to know how many rows are in this table
    public int GetNumberOfRowsForTableView(TableView tableView)
    {
        return numRows;
    }

    //Will be called by the TableView to know what is the height of each row
    public float GetHeightForRowInTableView(TableView tableView, int row)
    {
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
                UILoveCellItem loveitem = item as UILoveCellItem;
                // loveitem.callbackClickDelete = OnCellItemDidClickDelete;
                cell.AddItem(item);

            }

        }
        cell.totalItem = totalItem;
        cell.oneCellNum = oneCellNum;
        cell.rowIndex = row;
        foreach (UIItemListCellItem it_base in cell.listItem)
        {
            UIItemListCellItem ui = it_base as UIItemListCellItem;
            ui.colorSel = colorSel;
            ui.colorUnSel = colorUnSel;
        }
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
