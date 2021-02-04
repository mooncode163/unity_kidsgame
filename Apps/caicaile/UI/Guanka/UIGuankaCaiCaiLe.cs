using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGuankaCaiCaiLe : UIGuankaBase, ITableViewDataSource
{
    public Button btnBack;
    public UIText textTitle;
    UICellItemBase cellItemPrefab;
    UICellBase cellPrefab;//GuankaItemCell GameObject 
    public TableView tableView;
    public UIImage imageBar;
    public RawImage imageBg;
    public int numRows;
    private int numInstancesCreated = 0;

    int oneCellNum;
    int heightCell;
    int totalItem;
    List<object> listItem;
    static public long tick;

    Language languagePlace;
    HttpRequest httpReqLanguage;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        LoadPrefab();
        switch (Common.appType)
        {
            case AppType.PINTU:
                heightCell = 400;
                break;
            case AppType.FILLCOLOR:
                heightCell = 400;
                break;
            case AppType.PAINT:
                heightCell = 400;
                break;
            case AppType.XIEHANZI:
                heightCell = 320;
                break;
            default:
                //
                heightCell = 192;
                break;
        }



        //bg
        TextureUtil.UpdateRawImageTexture(imageBg, AppRes.IMAGE_GUANKA_BG, true);
        string strlan = CloudRes.main.rootPathGameRes + "/place/language/language.csv";
        if (Common.isWeb)
        {
            httpReqLanguage = new HttpRequest(OnHttpRequestFinished);
            httpReqLanguage.Get(HttpRequest.GetWebUrlOfAsset(strlan));
        }
        else
        {
            byte[] data = FileUtil.ReadDataAuto(strlan);
            OnGetLanguageFileDidFinish(FileUtil.FileIsExistAsset(strlan), data, true);
        }



        LevelManager.main.ParseGuanka();
        listItem = GameLevelParse.main.listGuanka;
        UpdateTable(false);
        tableView.dataSource = this;
        //tableView.ReloadData();

    }
    void Start()
    {
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
    public override void PreLoadDataForWeb()
    {
        string strlan = CloudRes.main.rootPathGameRes + "/place/language/language.csv";
        httpReqLanguage = new HttpRequest(OnHttpRequestFinished);
        httpReqLanguage.Get(HttpRequest.GetWebUrlOfAsset(strlan));
    }
    void OnGetLanguageFileDidFinish(bool isSuccess, byte[] data, bool isLocal)
    {

        {
            //web
            if (isSuccess)
            {
                languagePlace = new Language();
                languagePlace.Init(data);
                languagePlace.SetLanguage(Language.main.GetLanguage());
            }
            else
            {
                languagePlace = Language.main;
            }
        }

        {
            //textTitle.text = Language.main.GetString("STR_GUANKA");
            int idx = LevelManager.main.placeLevel;
            Debug.Log("LevelManager.main.placeTotal=" + LevelManager.main.placeTotal + " idx=" + idx);
            if (idx < LevelManager.main.placeTotal)
            {
                ItemInfo info = LevelManager.main.GetPlaceItemInfo(idx);
                Debug.Log(info.title);
                string str = languagePlace.GetString(info.title);
                textTitle.text = str;


            }


        }
    }
    void LoadPrefab()
    {
        {
            GameObject obj = PrefabCache.main.Load(AppCommon.PREFAB_UICELLBASE);
            cellPrefab = obj.GetComponent<UICellBase>();
        }
        {
            GameObject obj = PrefabCache.main.Load(AppCommon.PREFAB_GUANKA_CELL_ITEM_APP);
            if (obj == null)
            {
                obj = PrefabCache.main.Load(AppCommon.PREFAB_GUANKA_CELL_ITEM_COMMON);
            }


            cellItemPrefab = obj.GetComponent<UICellItemBase>();
        }

    }

    public override void LayOut()
    {
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        {
            RectTransform rectTransform = imageBg.GetComponent<RectTransform>();
            float w_image = rectTransform.rect.width;
            float h_image = rectTransform.rect.height;
            float scalex = sizeCanvas.x / w_image;
            float scaley = sizeCanvas.y / h_image;
            float scale = Mathf.Max(scalex, scaley);
            imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);
            //屏幕坐标 现在在屏幕中央
            imageBg.transform.position = new Vector2(Screen.width / 2, Screen.height / 2);
        }

        UpdateTable(true);
    }

    void OnHttpRequestFinished(HttpRequest req, bool isSuccess, byte[] data)
    {
        if (req == httpReqLanguage)
        {
            OnGetLanguageFileDidFinish(isSuccess, data, false);

        }
    }

    void ShowShop()
    {

    }
    void ShowParentGate()
    {
        ParentGateViewController.main.Show(null, null);
        ParentGateViewController.main.ui.callbackClose = OnUIParentGateDidClose;

    }
    public void OnUIParentGateDidClose(UIParentGate ui, bool isLongPress)
    {
        if (isLongPress)
        {
            ShowShop();
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
    #region GuankaItem_Delegate 
    void GotoGame(int idx)
    {
        LevelManager.main.gameLevel = idx;
        GameManager.main.GotoGame(this.controller);
    }
    public void OnCellItemDidClick(UICellItemBase item)
    {
        if (item.IsLock())
        {
            return;
        }
        tick = Common.GetCurrentTimeMs();
        GotoGame(item.index);

    }

    #endregion


    void UpdateTable(bool isLoad)
    {
        // oneCellNum = 3;
        // if (Device.isLandscape)
        // {
        //     oneCellNum = oneCellNum * 2;
        // }
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        oneCellNum = (int)(sizeCanvas.x / heightCell);
        int total = LevelManager.main.maxGuankaNum;
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

