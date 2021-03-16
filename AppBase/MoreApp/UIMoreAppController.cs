using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tacticsoft;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMoreAppController : UIView, ITableViewDataSource
{
    //    //儿童连连乐 微信小程序id:wx3e44af039aee1b96   
    //public const string APPCENTER_HTTP_URL_HOME = "http://42.96.196.180/moonma/app_center/applist_home.json";
    public const string APPCENTER_HTTP_URL_HOME_KIDS_GAME = "https://6c69-lianlianle-shkb3-1259451541.tcb.qcloud.la/AppCenter/applist_moreapp_kids.json?sign=87d3ec64c02d7eb3009d6babef86fa21&t=1589272924";
    public const string APPCENTER_HTTP_URL_HOME_SMALL_GAME = "https://6c69-lianlianle-shkb3-1259451541.tcb.qcloud.la/AppCenter/applist_moreapp_minigame.json?sign=94fd38be95dafaf874eb3fdd21b1505d&t=1589259514";
 
    public GameObject objTopBar;
    public GameObject objTableViewTemplate;
    public RawImage imageBg;
    public Image imageBarBg;
    public Button btnBack;
    UICellItemBase cellItemPrefab;
    UICellBase cellPrefab;
    public TableView tableView;
    public Text textTitle;
    public int numRows;
    private int numInstancesCreated = 0;

    private MoreAppParser moreAppParser;

    List<object> listItem;
    int oneCellNum;
    int heightCell;
    int totalItem;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        listItem = new List<object>();
        //bg
       // TextureUtil.UpdateRawImageTexture(imageBg, AppRes.IMAGE_MOREAPP_BG, true);
     
        LoadPrefab();
        heightCell = 512;

        oneCellNum = 1;
        if (Screen.width > Screen.height)
        {
            oneCellNum = 2;
        }

        totalItem = 0;//cene.MAX_GUANKA_NUM;
        if (listItem != null)
        {
            totalItem = listItem.Count;
        }
        numRows = totalItem / oneCellNum;
        if (totalItem % oneCellNum != 0)
        {
            numRows++;
        }

        //.Log("MoreApp Start 1");
        tableView.dataSource = this;


        //Debug.Log("MoreApp Start 2");
        {
            string str = Language.main.GetString(AppString.STR_APPCENTER);
            textTitle.text = str;
            textTitle.color = AppRes.colorTitle;
        }


    }
    // Use this for initialization
    void Start()
    {
           StartParse();
        LayOut();
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
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/MoreApp/UIMoreAppCellItem");
            if (obj == null)
            {
                obj = PrefabCache.main.Load(AppCommon.PREFAB_MOREAPP_CELL_ITEM);
            }


            cellItemPrefab = obj.GetComponent<UICellItemBase>();
        }

    }

    public override void LayOut()
    {
        base.LayOut();

    }
    public void OnClickBtnBack()
    {
        PopViewController pop = (PopViewController)this.controller;
        if (pop != null)
        {
            pop.Close();
        }
    }

    public void ShowParentGate(int idx)
    {
        Debug.Log("ShowParentGate idx=" + idx);
        ParentGateViewController.main.index = idx;
        ParentGateViewController.main.Show(null, null);
        ParentGateViewController.main.ui.callbackClose = OnUIParentGateDidCloseAppCenter;

    }

    public void OnUIParentGateDidCloseAppCenter(UIParentGate ui, bool isLongPress)
    {
        if (isLongPress)
        {
            Debug.Log("OnUIParentGateDidCloseAppCenter");
            GotoAppUrl(ParentGateViewController.main.index);
        }
    }
    public void OnCellItemDidClick(UICellItemBase item)
    {
        if (Config.main.APP_FOR_KIDS)
        {
            ShowParentGate(item.index);
        }
        else
        {
            GotoAppUrl(item.index);
        }
    }
    void GotoAppUrl(int idx)
    {
        if (listItem == null)
        {
            return;
        }
        ItemInfo info = listItem[idx] as ItemInfo;
        string appstorePackage = "";
        string appstore = Source.APPSTORE;
        if (Common.isAndroid)
        {
            if (Config.main.channel == Source.TAPTAP)
            {
                appstore = Source.TAPTAP;
                appstorePackage = AppVersion.PACKAGE_APPSTORE_TAPTAP;
            }

            if (Config.main.channel == Source.XIAOMI)
            {
                appstore = Source.XIAOMI;
                appstorePackage = AppVersion.PACKAGE_APPSTORE_XIAOMI;
            }
            if (Config.main.channel == Source.HUAWEI)
            {
                appstore = Source.HUAWEI;
                appstorePackage = AppVersion.PACKAGE_APPSTORE_HUAWEI;
            }

        }
        AppVersion.main.GotoToAppstoreApp(appstore, info.id, appstorePackage, info.url);


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

    #region  Parse
    void StartParse()
    {
        Debug.Log("MoreAppParser StartParse 0");
        moreAppParser = new MoreAppParser();
        //Debug.Log("MoreApp StartParse 1");
        moreAppParser.callback = OnMoreAppParserFinished;
       
        string url = APPCENTER_HTTP_URL_HOME_KIDS_GAME;
        if (!Config.main.APP_FOR_KIDS)
        {
            url = APPCENTER_HTTP_URL_HOME_SMALL_GAME;
        }

        // url = "http://www.mooncore.cn/moonma/adconfig/kidsgame/pintu/animal/ad_config_ios.json";
         Debug.Log("MoreAppParser StartParse 2");
        moreAppParser.startParserAppList(url);
        Debug.Log("MoreAppParser StartParse 3");
    }

    void OnMoreAppParserFinished(MoreAppParser parser, List<ItemInfo> listApp)
    {
        listItem.Clear();
        foreach (ItemInfo info in listApp)
        {
            listItem.Add(info);
        }

        totalItem = listItem.Count;//cene.MAX_GUANKA_NUM;
        numRows = totalItem / oneCellNum;
        if (totalItem % oneCellNum != 0)
        {
            numRows++;
        }

        tableView.ReloadData();
        //tableView.scrollY = 0;
    }

    #endregion
}

