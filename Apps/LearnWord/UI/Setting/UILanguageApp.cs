using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Tacticsoft;

public class UILanguageApp : UIViewPop, ITableViewDataSource
{

    public UIWordTitle uiWordTitle;
    public UIImage imageBg;
    public UIImage imageHeadStar;
    public UIButton btnClose;

    public GameObject objWordTitle;


    UICellItemBase cellItemPrefab;
    UICellBase cellPrefab;
    public TableView tableView;
    public int numRows;
    private int numInstancesCreated = 0;


    public List<object> listItem;
    int oneCellNum;
    int heightCell;
    int totalItem;
    public OnUILanguageDidCloseDelegate callbackClose { get; set; }

    /// <summary>
    /// Unity's Awake method.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        heightCell = 160;
        // textTitle.color = AppRes.colorTitle;
        // textTitle.text = Language.main.GetString("STR_LANGUAGE");
        LoadPrefab();

        uiWordTitle.UpdateItem(Language.main.GetString("STR_LANGUAGE"), "");
        uiWordTitle.UpdateColor(Color.white);
        listItem = new List<object>();
        UpdateItem();
        oneCellNum = 1;
        totalItem = listItem.Count;
        numRows = totalItem;
        tableView.dataSource = this;

    }

    /// <summary>
    /// Unity's Start method.
    /// </summary>
    protected override void Start()
    {
        base.Start();
        if (Device.isLandscape)
        {
            {

                LayOutRelation ly = btnClose.GetComponent<LayOutRelation>();
                ly.align = LayOutBase.Align.RIGHT;
                RectTransform rctran = btnClose.GetComponent<RectTransform>();
                rctran.anchoredPosition = Vector2.zero;
                // btnClose.imageBg.UpdateImageByKey("DetailClose_H");
                //  btnClose.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90f));
            }

            {

                LayOutRelation ly = imageHeadStar.GetComponent<LayOutRelation>();
                ly.align = LayOutBase.Align.LEFT;
                RectTransform rctran = imageHeadStar.GetComponent<RectTransform>();
                rctran.anchoredPosition = Vector2.zero;
                // imageHeadStar.UpdateImageByKey("HeadStar_H");
                // imageHeadStar.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90f));
            }
        }
        LayOut(); 
    }

    void Update()
    {
        //tableView.scrollY = 0;
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnClickBtnClose();
        }

        if (Device.isDeviceDidRotation)
        {
            LayOut();
        }
    }
    void LoadPrefab()
    {
        {
            GameObject obj = PrefabCache.main.Load(AppCommon.PREFAB_UICELLBASE);
            cellPrefab = obj.GetComponent<UICellBase>();
        }
        {
            GameObject obj = PrefabCache.main.Load("App/Prefab/Setting/UILanguageCellItem");
            cellItemPrefab = obj.GetComponent<UICellItemBase>();
        }

    }
    public override void LayOut()
    {
        base.LayOut();
        float x = 0, y = 0, w = 0, h = 0;

    }
    public void UpdateItem()
    {
        listItem.Clear();
        {
            ItemInfo info = new ItemInfo();
            info.title = "中文";
            info.tag = (int)SystemLanguage.Chinese;
            listItem.Add(info);
        }
        {
            ItemInfo info = new ItemInfo();
            info.title = "English";
            info.tag = (int)SystemLanguage.English;
            listItem.Add(info);
        }

    }

    public void OnClickBtnClose()
    {
        Close();
    }
    public void OnCellItemDidClick(UICellItemBase item)
    {
        SystemLanguage lan = (SystemLanguage)item.tagValue;
        Language.main.SetLanguage(lan);
        PlayerPrefs.SetInt(AppString.STR_KEY_LANGUAGE, item.tagValue);

        OnClickBtnClose();
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

            //  cell.SetCellSize(sizeCellNew);

            // Debug.LogFormat("TableView Cell Add Item:rcell:{0}, sizeCell:{1},rctable:{2},sizeTable:{3}", rccell, sizeCell, rctable, sizeTable);
            // oneCellNum = (int)(rctable.width / heightCell);
            //int i =0;
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
