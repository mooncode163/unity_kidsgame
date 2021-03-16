using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Moonma.AdKit.AdConfig;
using Moonma.IAP;
public class UIPlaceController : UIPlaceBase, ITableViewDataSource
{

    public Button btnBack;
    public UIText textTitle;
    UICellItemBase cellItemPrefab;
    UICellBase cellPrefab;//GuankaItemCell GameObject 
    public TableView tableView;
    public UIImage imageBar;
    // public RawImage imageBg;
    public int numRows;
    private int numInstancesCreated = 0;

    int oneCellNum;
    int heightCell;
    int totalItem;

    Language languagePlace;
    List<object> listItem;
    int indexClick;

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

        listItem = LevelManager.main.ParsePlaceList();
        //bg
        //TextureUtil.UpdateRawImageTexture(imageBg, AppRes.IMAGE_PLACE_BG, true);//IMAGE_GAME_BG

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

        int total = listItem.Count;
        totalItem = total;
        Debug.Log("uiplace total:" + total + " oneCellNum=" + oneCellNum + " heightCell=" + heightCell);
        numRows = total / oneCellNum;
        if (total % oneCellNum != 0)
        {
            numRows++;
        }


        tableView.dataSource = this;
        //tableView.ReloadData();

    }
    void Start()
    {
        LayOut();
        OnUIDidFinish(0.2f);
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

    public override void LayOut()
    {
        base.LayOut();
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        // {
        //     RectTransform rectTransform = imageBg.GetComponent<RectTransform>();
        //     float w_image = rectTransform.rect.width;
        //     float h_image = rectTransform.rect.height;
        //     float scalex = sizeCanvas.x / w_image;
        //     float scaley = sizeCanvas.y / h_image;
        //     float scale = Mathf.Max(scalex, scaley);
        //     imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);
        //     //屏幕坐标 现在在屏幕中央
        //     imageBg.transform.position = new Vector2(Screen.width / 2, Screen.height / 2);
        // }

        // {
        //     string str = textTitle.text; 
        //     int fontsize = textTitle.fontSize;
        //     float str_w = Common.GetStringLength(str, AppString.STR_FONT_NAME, fontsize);
        //     RectTransform rctran = imageBar.transform as RectTransform;
        //     Vector2 sizeDelta = rctran.sizeDelta;
        //     float oft = 0;
        //     sizeDelta.x = str_w + fontsize + oft * 2;
        //     rctran.sizeDelta = sizeDelta; 
        // }

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
        bool enable = true;
        ItemInfo info = listItem[item.index] as ItemInfo;
        indexClick = item.index;
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


            if (Common.isiOS && (!GameManager.main.isHaveUnlockLevel))
            {
                enable = false;
                // 内购解锁
                OnUnLockLevelIAP();
            }
        }


        if (enable)
        {
            GotoNext(item.index);
        }

    }
    public void GotoNext(int idx)
    {
        LevelManager.main.placeLevel = idx;
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
                guanka.indexPlace = idx;
                navi.Push(guanka);
            }
        }
    }



    public void OnUnLockLevelIAP()
    {
        if (Config.main.APP_FOR_KIDS)
        {
            ParentGateViewController.main.Show(null, null);
            ParentGateViewController.main.ui.callbackClose = OnUIParentGateDidCloseNoAd;
        }
        else
        {
            DoUnLockLevelAlert();
        }
    }

    public void OnUIParentGateDidCloseNoAd(UIParentGate ui, bool isLongPress)
    {
        if (isLongPress)
        {
            DoUnLockLevelAlert();
        }
    }
    public void DoUnLockLevelAlert()
    {
        string key = "unlocklevel";
        string title = IAPConfig.main.GetProductTitle(key);// Language.main.GetString("STR_UIVIEWALERT_TITLE_UnlockLevel");
        string msg = IAPConfig.main.GetProductDetail(key);// Language.main.GetString("STR_UIVIEWALERT_MSG_UnlockLevel");
        if ((!Config.main.isNoIDFASDK) && AppVersion.appCheckHasFinished)
        {
            msg += "," + Language.main.GetString("AndRemoveAd");
        }
        string yes = Language.main.GetString("STR_UIVIEWALERT_YES_UnlockLevel");
        string no = Language.main.GetString("STR_UIVIEWALERT_NO_UnlockLevel");

        ViewAlertManager.main.ShowFull(title, msg, yes, no, true, UIGuankaController.STR_KEYNAME_VIEWALERT_UNLOCK_LEVLE, OnUIViewAlertFinished);
    }

    public void DoUnLockLevelIAP(bool isRestore)
    {
        IAP.main.SetObjectInfo(this.gameObject.name, "IAPCallBack");

        // viewAlert.ShowBtnNo(false);
        // viewAlert.keyName = STR_KEYNAME_VIEWALERT_LOADING;
        // viewAlert.callback = OnUIViewAlertFinished;
        // string title = Language.main.GetString(AppString.STR_UIVIEWALERT_TITLE_SHOP_START_BUY);
        // string msg = Language.main.GetString(AppString.STR_UIVIEWALERT_MSG_SHOP_START_BUY);
        // string yes = Language.main.GetString(AppString.STR_UIVIEWALERT_YES_SHOP_START_BUY);
        // string no = Language.main.GetString(AppString.STR_UIVIEWALERT_YES_SHOP_START_BUY);
        // viewAlert.SetText(title, msg, yes, no);
        // viewAlert.Show();
        string product = Common.GetAppPackage() + "." + IAPConfig.main.GetIdByKey("unlocklevel");
        if (isRestore)
        {
            IAP.main.RestoreBuy(product);
        }
        else
        {
            IAP.main.StartBuy(product, false);
        }


    }
    public void IAPCallBack(string str)
    {
        Debug.Log("IAPCallBack::" + str);
        IAP.main.IAPCallBackBase(str);

        if ((str == IAP.UNITY_CALLBACK_BUY_DID_FINISH) || (str == IAP.UNITY_CALLBACK_BUY_DID_RESTORE))
        {
            GameManager.main.isHaveUnlockLevel = true;

            Loom.QueueOnMainThread(() =>
            {
                GotoNext(indexClick);

            });
        }

    }

    #region UIViewAlert
    void OnUIViewAlertFinished(UIViewAlert alert, bool isYes)
    {
        if (UIGuankaController.STR_KEYNAME_VIEWALERT_UNLOCK_LEVLE == alert.keyName)
        {
            DoUnLockLevelIAP(!isYes);
        }


    }



    #endregion

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

