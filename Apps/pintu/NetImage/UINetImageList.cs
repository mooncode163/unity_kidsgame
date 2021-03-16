﻿using System.Collections;
using System.Collections.Generic;
using Moonma.SysImageLib;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;

public class UINetImageList : UIView, ITableViewDataSource, INetImageParseDelegate
{
    public int index;
    public GameObject objLayoutBtn;
    public Button btnPlay;
    public Button btnNetImage;
    public UIImage imageBg;
    public UIText textTitle;
    public Image imageBar;
    UICellItemBase cellItemPrefab;
    UICellBase cellPrefab;//GuankaItemCell GameObject 
    public TableView tableView;
    public int numRows;
    private int numInstancesCreated = 0;
    int oneCellNum;
    int heightCell;
    int totalItem;
    List<object> listItem;
    static ImageItemInfo infoImage = null;
    NetImageParseCommon netImageParse;
    public UIViewLoading uiViewLoading;
    void Awake()
    {
        LoadPrefab();
        listItem = new List<object>();
        heightCell = 256 + 128;
        UpdateTable(false);
        tableView.dataSource = this;
        // TextureUtil.UpdateImageTexture(imageBg, AppRes.IMAGE_HOME_BG, true);
        netImageParse = NetImageParseCommon.main;
        // netImageParse.CreateAPI(Source.QIHU_360);
        netImageParse.SetDelegate(this);

        // textTitle.text = Language.main.GetString("STR_NETIMAGE_LIST");
    }

    // Use this for initialization
    void Start()
    {
        if (infoImage != null)
        {
            //  uiViewLoading.Show(true);
            StartParseImageList(infoImage);
        }
        LayOut();
        OnUIDidFinish();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void LoadPrefab()
    {
        {
            GameObject obj = PrefabCache.main.Load(AppCommon.PREFAB_UICELLBASE);
            cellPrefab = obj.GetComponent<UICellBase>();
        }
        {
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/NetImage/UINetImageCellItem");

            cellItemPrefab = obj.GetComponent<UICellItemBase>();
        }

    }
    public void StartParseImageList(ImageItemInfo info)
    {
        infoImage = info;
        if (netImageParse != null)
        {
            netImageParse.StartParseImageList(info);
            textTitle.text = info.title;
        }
    }

    public void OnClickBtnBack()
    {
        NaviViewController navi = this.controller.naviController;
        if (navi != null)
        {
            navi.Pop();
        }
    }

    public void OnClickBtnPlay()
    {


    }

    public override void LayOut()
    {
        base.LayOut();
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        // {
        //     RectTransform rectTransform = imageBg.GetComponent<RectTransform>();
        //     float w_image = rectTransform.rect.width;
        //     float h_image = rectTransform.rect.height;
        //     float scale = Common.GetMaxFitScale(w_image, h_image, sizeCanvas.x, sizeCanvas.y);
        //     imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);
        //     //屏幕坐标 现在在屏幕中央
        //     imageBg.transform.position = new Vector2(Screen.width / 2, Screen.height / 2);
        // }
    }

    #region NetImageParse_Delegate 
    public void OnNetImageParseDidParseSortList(NetImageParseBase parse, bool isSuccess, List<object> list)
    {

    }
    public void OnNetImageParseDidParseImageList(NetImageParseBase parse, bool isSuccess, List<object> list)
    {
        if ((isSuccess) && (list != null))
        {
            // uiViewLoading.Show(false);
            listItem.Clear();
            foreach (object obj in list)
            {
                listItem.Add(obj);
            }
            UpdateTable(true);
        }
    }
    #endregion

    #region GuankaItem_Delegate 
    void GotoGame(int idx)
    {
        ImageItemInfo info = listItem[idx] as ImageItemInfo;
        UIGamePintu.imageSource = GamePintu.ImageSource.NET;
        UIGamePintu.infoNetImage = info;

        LevelManager.main.gameLevel = idx;
        GameManager.main.GotoGame(this.controller);
    }
    public void OnCellItemDidClick(UICellItemBase item)
    {
        if (item.IsLock())
        {
            return;
        }
        GotoGame(item.index);

    }

    #endregion


    void UpdateTable(bool isLoad)
    {

        float w_cell = Device.sizeDesign.x / 2;
        if (Device.isLandscape)
        {
            w_cell = Device.sizeDesign.x / 4;
        }
        RectTransform rctran = tableView.GetComponent<RectTransform>();
        float w = AppSceneBase.main.sizeCanvas.x;
        // w = rctran.rect.width;
        Debug.Log("tableView rctran:" + rctran.rect + " w=" + w);
        float num = (w / w_cell);
        oneCellNum = (int)num;
        if ((num - oneCellNum) > 0)
        {
            oneCellNum++;
        }
        w_cell = w / oneCellNum;
        heightCell = (int)(w_cell / NetImageParseCommon.main.GetImageRatio());

        int total = listItem.Count;
        totalItem = total;
        Debug.Log("total:" + total);
        numRows = total / oneCellNum;
        if (total % oneCellNum != 0)
        {
            numRows++;
        }

        if (isLoad)
        {
            tableView.ReloadData();
        }

    }

    void AddCellItem(UICellBase cell, TableView tableView, int row)
    {
        Rect rctable = (tableView.transform as RectTransform).rect;

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
            item.tagValue = UINetImageCellItem.TAG_IMAGE_LIST;
            cell.AddItem(item);

        }
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

            AddCellItem(cell, tableView, row);

        }
        cell.totalItem = totalItem;
        if (oneCellNum != cell.oneCellNum)
        {
            //relayout
            cell.ClearAllItem();
            AddCellItem(cell, tableView, row);
        }
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
