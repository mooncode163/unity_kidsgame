using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;
public delegate void OnUILoveControllerDidCloseDelegate(UILoveController ui);
public class UILoveController : UIView, ITableViewDataSource, ISegmentDelegate
{
    public OnUILoveControllerDidCloseDelegate callbackClose { get; set; }
    UICellItemBase cellItemPrefab;
    UICellBase cellPrefab;
    public UIButton btnDeleteAll;
    public UISegment uiSegment;
    public int numRows;
    private int numInstancesCreated = 0;

    int totalItem;
    private int oneCellNum;
    private int heightCell;
    public TableView tableView;
    public GameObject tableViewTemplate;
    public GameObject topBar;

    List<object> listItem;
    public UIText textTitle;
    public UIText textDetail;

    Color colorSel;
    Color colorUnSel;
    int indexSegment;
    public string gameId;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        LoadPrefab();
        colorSel = new Color(1f, 0f, 0f, 1f);
        colorUnSel = new Color(1f, 1f, 1f, 1f);

        oneCellNum = 2;
        if (!Device.isLandscape)
        {
            oneCellNum = oneCellNum / 2;
        }
        heightCell = 256;
        LevelManager.main.ParseGuanka();
        tableView.dataSource = this;


        listItem = new List<object>();


        indexSegment = 0;
        // uiSegment.InitValue(64, Color.red, Color.black);
        // uiSegment.iDelegate = this;

        LanguageManager.main.UpdateLanguagePlace();
        LanguageManager.main.UpdateLanguage(indexSegment);


    }

    // Use this for initialization
    void Start()
    {
        // UpdateSegment();
        UpdateTitle();
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
            GameObject obj = PrefabCache.main.LoadByKey("UILoveCellItem");
            cellItemPrefab = obj.GetComponent<UICellItemBase>();
        }

    }


    public override void LayOut()
    {
        base.LayOut();
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;

        // if (!uiSegment.gameObject.activeSelf)
        // {
        //     LayOutSize ly = tableViewTemplate.GetComponent<LayOutSize>();
        //     ly.target = topBar;
        //     ly.LayOut();
        //     base.LayOut();
        // }

    }
    public bool IsRepeatGameId(List<object> list, ItemInfo info)
    {
        bool ret = false;
        if (list.Count == 0)
        {
            return ret;
        }
        for (int i = 0; i < list.Count; i++)
        {
            ItemInfo infoplace = list[i] as ItemInfo;
            if (infoplace.gameId == info.gameId)
            {
                ret = true;
                break;
            }
        }
        return ret;
    }

    public void UpdateSegment()
    {
        List<object> listPlace = LevelManager.main.ParsePlaceList();
        LanguageManager.main.UpdateLanguagePlace();
        List<object> listShow = new List<object>();

        for (int i = 0; i < listPlace.Count; i++)
        {
            ItemInfo infoplace = listPlace[i] as ItemInfo;
            Debug.Log("infoplace.gameId=" + infoplace.gameId);
            if (IsRepeatGameId(listShow, infoplace))
            {
                continue;
            }
            listShow.Add(infoplace);
            ItemInfo infoSeg = new ItemInfo();
            infoSeg.id = infoplace.id;
            infoSeg.title = LanguageManager.main.languagePlace.GetString("STR_PLACE_" + infoplace.id);
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
    void UpdateTitle()
    {
    }
    void UpdateList()
    {
        ItemInfo infoPlace = LevelManager.main.GetPlaceItemInfo(indexSegment);
        gameId = infoPlace.gameId;
        Debug.Log("gameId=" + gameId);
        DBLove.main.GetDB(infoPlace.gameId);

        listItem.Clear();
        //     List<IdiomItemInfo> ls = DBLove.main.GetAllItem<IdiomItemInfo>();
        // foreach (IdiomItemInfo info in ls)
        // {
        //     WordItemInfo infocaicaile = new WordItemInfo();

        //     infocaicaile.title = info.title;
        //     infocaicaile.id = info.id;
        //     Debug.Log("UpdateList info.id=" + info.id + " info.title=" + info.title);
        //     infocaicaile.dbInfo = info;
        //     // infocaicaile.dbInfo = info;
        //     listItem.Add(infocaicaile);
        // }

        DBLove.main.GetAllItem(listItem);
        // listItem.AddRange(ls);
        totalItem = listItem.Count;
        numRows = totalItem / oneCellNum;
        if (totalItem % oneCellNum != 0)
        {
            numRows++;
        }
        textDetail.gameObject.SetActive(DBLove.main.DBEmpty());
        btnDeleteAll.gameObject.SetActive(!DBLove.main.DBEmpty());
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
        WordItemInfo info = listItem[item.index] as WordItemInfo;
        ItemInfo infoPlace = LevelManager.main.GetPlaceItemInfo(LevelManager.main.placeLevel);
        string strPrefab = ConfigPrefab.main.GetPrefab("UIGameWinPintu");
        Debug.Log("GameWin strPrefab=" + strPrefab);
        PopUpManager.main.Show<UIGameWinBase>(strPrefab, popup =>
        {
            Debug.Log("UIGameWinBase Open ");
            popup.ShowBtnNext(false);
            popup.UpdateItem(info);

        }, popup =>
        {


        });
    }

    public void OnCellItemDidClickDelete(UILoveCellItem ui)
    {
        WordItemInfo info = listItem[ui.index] as WordItemInfo;
        if (info != null)
        {
            DBLove.main.DeleteItem(info.dbInfo);
        }
        UpdateList();
    }
    public void SegmentDidClick(UISegment seg, SegmentItem item)
    {
        indexSegment = item.index;
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
                loveitem.callbackClickDelete = OnCellItemDidClickDelete;
                cell.AddItem(item);

            }

        }
        cell.totalItem = totalItem;
        cell.oneCellNum = oneCellNum;
        cell.rowIndex = row;
        foreach (UICellItemBase it_base in cell.listItem)
        {
            UILoveCellItem ui = it_base as UILoveCellItem;
            // ui.colorSel = colorSel;
            // ui.colorUnSel = colorUnSel;
            ui.gameId = gameId;
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
