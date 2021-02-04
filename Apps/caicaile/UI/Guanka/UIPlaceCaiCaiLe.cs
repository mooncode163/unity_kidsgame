using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Moonma.AdKit.AdConfig;

public class UIPlaceCaiCaiLe : UIPlaceBase, ITableViewDataSource, ISegmentDelegate
{
    public Button btnBack;
    public UIText textTitle;
    UICellItemBase cellItemPrefab;
    UICellBase cellPrefab;//GuankaItemCell GameObject 
    public TableView tableView;
    public GameObject objTableViewTemplate;
    public UIImage imageBar;
    public RawImage imageBg;
    public UISegment uiSegment;
    public GameObject objTopBar;
    public int numRows;
    private int numInstancesCreated = 0;

    int oneCellNum;
    int heightCell;
    int totalItem;

    Language languagePlace;
    List<object> listItem;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        //textTitle.color = ColorConfig.main.GetColor(GameRes.KEY_COLOR_LevelTitle, Color.white);
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
            case AppType.WORDCONNECT:
                heightCell = 360;
                break;
            default:
                //
                heightCell = 512;
                break;
        }



        //bg
        TextureUtil.UpdateRawImageTexture(imageBg, AppRes.IMAGE_PLACE_BG, true);//IMAGE_GAME_BG

        string strlan = CloudRes.main.rootPathGameRes + "/place/language/language.csv";
        if (FileUtil.FileIsExistAsset(strlan))
        {
            languagePlace = new Language();
            languagePlace.Init(strlan);
            languagePlace.SetLanguage(Language.main.GetLanguage());
        }
        else
        {
            languagePlace = Language.main;
        }



        // oneCellNum = 2;
        // if (Device.isLandscape)
        // {
        //     oneCellNum *= 2;
        // }
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        oneCellNum = (int)(sizeCanvas.x / heightCell);
        if (((int)sizeCanvas.x) % heightCell != 0)
        {
            oneCellNum++;
        }

        heightCell = (int)(sizeCanvas.x / oneCellNum);

        if (Common.appKeyName != GameRes.GAME_IdiomFlower)

        {
            listItem = LevelManager.main.ParsePlaceList();
            int total = listItem.Count;
            totalItem = total;
            Debug.Log("uiplace total:" + total + " oneCellNum=" + oneCellNum + " heightCell=" + heightCell);
            numRows = total / oneCellNum;
            if (total % oneCellNum != 0)
            {
                numRows++;
            }

        }



        tableView.dataSource = this;
        //tableView.ReloadData();

    }
    void Start()
    {

        InitSegment();
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
    void LoadPrefab()
    {
        {
            GameObject obj = PrefabCache.main.Load(AppCommon.PREFAB_UICELLBASE);
            cellPrefab = obj.GetComponent<UICellBase>();
        }
        {
            GameObject obj = PrefabCache.main.Load(AppCommon.PREFAB_PLACE_CELL_ITEM_APP);
            if (obj == null)
            {
                obj = PrefabCache.main.Load(AppCommon.PREFAB_PLACE_CELL_ITEM_COMMON);
            }
            cellItemPrefab = obj.GetComponent<UICellItemBase>();
        }

    }
    public void InitSegment()
    {
        uiSegment.gameObject.SetActive(false);
        if (Common.appKeyName == GameRes.GAME_IdiomFlower)
        {
            uiSegment.gameObject.SetActive(true);
            List<object> list = LevelParseIdiomFlower.main.ParseCategory();
            //   indexSegment = 0;
            uiSegment.InitValue(64, Color.red, Color.black);
            uiSegment.iDelegate = this;

            foreach (CaiCaiLeItemInfo info in LevelParseIdiomFlower.main.listCategory)
            {
                ItemInfo infoSeg = new ItemInfo();
                infoSeg.title = info.title;
                uiSegment.AddItem(infoSeg);
            }
            uiSegment.UpdateList();
        }

        if (!uiSegment.gameObject.activeSelf)
        {
            {
                LayOutSize ly = objTableViewTemplate.GetComponent<LayOutSize>();
                if (ly != null)
                {
                    ly.target = objTopBar;
                    ly.LayOut();
                }
            }
            {
                LayOutRelation ly = objTableViewTemplate.GetComponent<LayOutRelation>();
                if (ly != null)
                {
                    ly.target = objTopBar;
                    ly.LayOut();
                }
            }

        }

    }

    public void SegmentDidClick(UISegment seg, SegmentItem item)
    {
        CaiCaiLeItemInfo info = LevelParseIdiomFlower.main.listCategory[item.index] as CaiCaiLeItemInfo;
        LevelParseIdiomFlower.main.categoryTitle = info.title;
        listItem = LevelParseIdiomFlower.main.ParseSort(info.title);
        int total = listItem.Count;
        totalItem = total;
        Debug.Log("uiplace total:" + total + " oneCellNum=" + oneCellNum + " heightCell=" + heightCell);
        numRows = total / oneCellNum;
        if (total % oneCellNum != 0)
        {
            numRows++;
        }
        tableView.ReloadData();
    }
    public override void LayOut()
    {
        base.LayOut();
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



    }

    public override void PreLoadDataForWeb()
    {
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
        AudioPlay.main.PlayBtnSound();
        NaviViewController navi = this.controller.naviController;
        if (navi != null)
        {
            navi.Pop();
        }

    }
    public void OnCellItemDidClick(UICellItemBase item)
    {
        if (item.IsLock())
        {
            return;
        }


        ItemInfo info = listItem[item.index] as ItemInfo;

        if (Common.appKeyName == GameRes.GAME_IdiomFlower)
        {
            LevelParseIdiomFlower.main.sortTitle = info.title;

            LevelManager.main.placeLevel = 0;
        }
        else
        {

            LevelManager.main.placeLevel = item.index;
        }




        AudioPlay.main.PlayBtnSound();
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;

            if (Common.appType == AppType.STICKER)
            {
                LevelManager.main.ParseGuanka();
                navi.Push(GameViewController.main);
            }
            else
            {
                GuankaViewController guanka = GuankaViewController.main;
                guanka.indexPlace = item.index;
                navi.Push(guanka);
            }
        }

        if (info.isAd)
        {
            bool isAdVideo = true;
            int type = AdConfigParser.SOURCE_TYPE_VIDEO;
            string keyAdVideo = AdConfig.main.GetAdKey(Source.GDT, type);
            if (Common.isAndroid)
            {
                if ((Common.BlankString(keyAdVideo)) || (keyAdVideo == "0"))
                {
                    //android 显示插屏广告
                    isAdVideo = false;
                }
            }
            if (isAdVideo)
            {
                AdKitCommon.main.ShowAdVideo();
            }
            else
            {
                AdKitCommon.main.InitAdInsert();
                AdKitCommon.main.ShowAdInsert(100);
            }
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

