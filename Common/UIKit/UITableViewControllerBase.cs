using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tacticsoft;
public class UITableViewControllerBase : UIView, ITableViewDataSource
{
    public UICellItemBase cellItemPrefab;
    UICellBase cellPrefab;//GuankaItemCell GameObject 
    public TableView tableView;

    public int numRows;
    int numInstancesCreated = 0;

    public int oneCellNum;
    public int heightCell;
    public int totalItem;

    public List<object> listItem;

    public void Awake()
    {
        //textTitle.color = ColorConfig.main.GetColor(GameRes.KEY_COLOR_LevelTitle, Color.white);
        LoadPrefab();
        heightCell = 512;
    }

    public void Start()
    {
    }
    void LoadPrefab()
    {
        {
            GameObject obj = PrefabCache.main.Load(AppCommon.PREFAB_UICELLBASE);
            cellPrefab = obj.GetComponent<UICellBase>();
        }
        // {
        //     GameObject obj = PrefabCache.main.Load(AppCommon.PREFAB_PLACE_CELL_ITEM_APP);
        //     cellItemPrefab = obj.GetComponent<UICellItemBase>();
        // }

    }


    public void UpdateTable(bool isReload, bool isAuto = true)
    {
        //Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        RectTransform rctran = this.GetComponent<RectTransform>();
        float width = rctran.rect.width;
        if (width == 0)
        {
            return;
        }
        if (isAuto)
        {
            oneCellNum = (int)(width / heightCell);
            if (((int)width) % heightCell != 0)
            {
                oneCellNum++;
            }

            heightCell = (int)(width / oneCellNum);
        }


        int total = listItem.Count;
        totalItem = total;
        Debug.Log("uiplace total:" + total + " oneCellNum=" + oneCellNum + " heightCell=" + heightCell + " width=" + width);
        numRows = total / oneCellNum;
        if (total % oneCellNum != 0)
        {
            numRows++;
        }

        tableView.dataSource = this;
        if (isReload)
        {
            tableView.ReloadData();
        }
    }
    public virtual void OnCellItemDidClick(UICellItemBase item)
    {
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



    public void TableViewCellOnClik()
    {
        print("TableViewCellOnClik1111");
    }

}
