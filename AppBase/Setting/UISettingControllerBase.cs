using System.Collections;
using System.Collections.Generic;
using Moonma.IAP;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;


enum SettingItemTag
{
    TAG_SETTING_COMMENT = 0,
    TAG_SETTING_VERSION,
    TAG_SETTING_LANGUAGE,
    TAG_SETTING_BACKGROUND_MUSIC,
    TAG_SETTING_SOUND,
    TAG_SETTING_NOAD,
    TAG_SETTING_RESTORE_IAP,


    TAG_SETTING_LAST
}
public class UISettingControllerBase : UIView, ITableViewDataSource
{
    public const string STR_KEYNAME_VIEWALERT_NOAD = "STR_KEYNAME_VIEWALERT_NOAD";
    public const string STR_KEYNAME_VIEWALERT_LOADING = "STR_KEYNAME_VIEWALERT_LOADING";
    public const string STR_KEYNAME_VIEWALERT_UPDATE_VERSION = "STR_KEYNAME_VIEWALERT_UPDATE_VERSION";
    UICellItemBase cellItemPrefab;
    UICellBase cellPrefab;
    public GameObject objTableViewTemplate;
    public TableView tableView;
    public UIText textTitle;
    public UIImage imageBar;
    public UIButton btnBack;
    // public Image imageBg;
    public int numRows;
    private int numInstancesCreated = 0;


    public List<object> listItem;

    int oneCellNum;
    int heightCell;
    int totalItem;
    void Awake()
    {
        heightCell = 160;
        LoadPrefab();
        //TextureUtil.UpdateImageTexture(imageBg, AppRes.IMAGE_SETTING_BG, true);//IMAGE_SETTING_BG
        // 
        // {
        //     string str = Language.main.GetString(AppString.STR_SETTING);
        //    // textTitle.text = str;
        //     int fontsize = textTitle.fontSize;
        //     float str_w = Common.GetStringLength(str, AppString.STR_FONT_NAME, fontsize);
        //     RectTransform rctran = imageBar.transform as RectTransform;
        //     Vector2 sizeDelta = rctran.sizeDelta;
        //     float oft = 0;
        //     sizeDelta.x = str_w + fontsize + oft * 2;
        //     rctran.sizeDelta = sizeDelta;
        // }
        listItem = new List<object>();
        UpdateItem();
        oneCellNum = 1;
        totalItem = listItem.Count;
        numRows = totalItem;
        tableView.dataSource = this;


    }

    // Use this for initialization
    void Start()
    {
        if (this.controller != null)
        {
            if (this.controller.naviController != null)
            {
                btnBack.gameObject.SetActive(false);
            }
        }
        LayOut();

        OnUIDidFinish(0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        //tableView.scrollY = 0;
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnClickBtnBack();
        }

        if (Device.isDeviceDidRotation)
        {
            LayOut();
        }
    }


    public override void LayOut()
    {
        base.LayOut();
        //Vector2 sizeCanvas = ViewControllerManager.sizeCanvas;
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        // {
        //     RectTransform rctran = imageBg.GetComponent<RectTransform>();
        //     float w_image = rctran.rect.width;
        //     float h_image = rctran.rect.height;
        //     float scalex = sizeCanvas.x / w_image;
        //     float scaley = sizeCanvas.y / h_image;
        //     float scale = Mathf.Max(scalex, scaley);
        //     imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);
        //     //屏幕坐标 现在在屏幕中央
        //     imageBg.transform.position = new Vector2(Screen.width / 2, Screen.height / 2);
        // }


    }

    void LoadPrefab()
    {
        {
            GameObject obj = PrefabCache.main.Load(AppCommon.PREFAB_UICELLBASE);
            cellPrefab = obj.GetComponent<UICellBase>();
        }
        {
            GameObject obj = PrefabCache.main.Load(AppCommon.PREFAB_UISettingCellItem);
            cellItemPrefab = obj.GetComponent<UICellItemBase>();
        }

    }

    public void OnClickBtnBack()
    {
        PopViewController pop = (PopViewController)this.controller;
        if (pop != null)
        {
            pop.Close();
        }
    }

    void ShowLanguage(bool isShow)
    {
        if (isShow)
        {
            string strPrefabApp = "App/Prefab/Setting/UILanguage";
            string strPrefabDefault = "Common/Prefab/Setting/UILanguage";
            string strPrefab = strPrefabApp;
            GameObject obj = PrefabCache.main.Load(strPrefabApp);
            if (obj == null)
            {
                strPrefab = strPrefabDefault;
            }
            PopUpManager.main.Show<UIViewPop>(strPrefab, popup =>
         {
             Debug.Log("UIViewAlert Open ");

             //  popup.callbackClose = OnUILanguageDidClose;

         }, popup =>
         {
             OnUILanguageDidClose();

         });

        }
        else
        {
            //Destroy(uiLanguage);
        }


    }

    public void OnUILanguageDidClose()
    {
        int tag = PlayerPrefs.GetInt(AppString.STR_KEY_LANGUAGE);
        SystemLanguage lan = (SystemLanguage)tag;
        Language.main.SetLanguage(lan, true);

        {
            string str = Language.main.GetString(AppString.STR_SETTING);
            textTitle.text = str;
            int fontsize = textTitle.fontSize;
            float str_w = Common.GetStringLength(str, AppString.STR_FONT_NAME, fontsize);
            RectTransform rctran = imageBar.transform as RectTransform;
            Vector2 sizeDelta = rctran.sizeDelta;
            float oft = 0;
            sizeDelta.x = str_w + fontsize + oft * 2;
            rctran.sizeDelta = sizeDelta;
        }
        UpdateItem();
        tableView.ReloadData();

    }
    public virtual void UpdateItem()
    {
        listItem.Clear();
        if (AppVersion.appCheckHasFinished)
        {
            ItemInfo info = new ItemInfo();
            info.title = Language.main.GetString(AppString.STR_SETTING_COMMENT);
            info.tag = (int)SettingItemTag.TAG_SETTING_COMMENT;
            listItem.Add(info);
        }
        if (AppVersion.appCheckHasFinished)
        {
            ItemInfo info = new ItemInfo();
            string strversin = Common.GetAppVersion();
            string str = Language.main.GetString(AppString.STR_SETTING_VERSION) + "(" + strversin + ")";
            info.title = str;
            info.tag = (int)SettingItemTag.TAG_SETTING_VERSION;
            listItem.Add(info);
        }

        {
            ItemInfo info = new ItemInfo();
            info.title = Language.main.GetString(AppString.STR_SETTING_LANGUAGE);
            info.tag = (int)SettingItemTag.TAG_SETTING_LANGUAGE;
            listItem.Add(info);
        }

        {
            ItemInfo info = new ItemInfo();
            info.title = Language.main.GetString(AppString.STR_SETTING_BACKGROUND_MUSIC);
            info.tag = (int)SettingItemTag.TAG_SETTING_BACKGROUND_MUSIC;
            listItem.Add(info);
        }

        {
            ItemInfo info = new ItemInfo();
            info.title = Language.main.GetString("STR_SETTING_SOUND");
            info.tag = (int)SettingItemTag.TAG_SETTING_SOUND;
            listItem.Add(info);
        }
        if (Config.main.isHaveRemoveAd)
        {
            ItemInfo info = new ItemInfo();
            info.title = Language.main.GetString("STR_BTN_NOAD");
            info.tag = (int)SettingItemTag.TAG_SETTING_NOAD;
            listItem.Add(info);
        }
        // if (Common.isiOS && Config.main.isHaveRemoveAd)
         if (Common.isiOS)
        {
            ItemInfo info = new ItemInfo();
            info.title = Language.main.GetString("STR_BTN_RESTORE_NOAD");
            info.tag = (int)SettingItemTag.TAG_SETTING_RESTORE_IAP;
            listItem.Add(info);
        }

    }

    public virtual void OnClickItem(UICellItemBase item)
    {

    }

    public void OnCellItemDidClick(UICellItemBase item)
    {
        switch (item.tagValue)
        {
            case (int)SettingItemTag.TAG_SETTING_COMMENT:
                {
                    AppVersion.main.GotoComment();
                }
                break;

            case (int)SettingItemTag.TAG_SETTING_VERSION:
                {
                    string title, msg, yes, no;
                    bool isShowBtnNo = false;
                    if (AppVersion.main.appNeedUpdate)
                    {
                        isShowBtnNo = true;
                        title = Language.main.GetString(AppString.STR_UIVIEWALERT_TITLE_UPDATE_VERSION);
                        msg = AppVersion.main.strUpdateNote;
                        yes = Language.main.GetString(AppString.STR_UIVIEWALERT_YES);
                        no = Language.main.GetString(AppString.STR_UIVIEWALERT_NO);

                    }
                    else
                    {
                        isShowBtnNo = false;
                        title = Language.main.GetString(AppString.STR_UIVIEWALERT_TITLE_UPDATE_VERSION);
                        msg = Language.main.GetString(AppString.STR_UIVIEWALERT_MSG_NEWEST_VERSION);
                        yes = Language.main.GetString(AppString.STR_UIVIEWALERT_YES);
                        no = Language.main.GetString(AppString.STR_UIVIEWALERT_NO);

                    }

                    ViewAlertManager.main.ShowFull(title, msg, yes, no, isShowBtnNo, STR_KEYNAME_VIEWALERT_UPDATE_VERSION, OnUIViewAlertFinished);


                }
                break;

            case (int)SettingItemTag.TAG_SETTING_LANGUAGE:
                {
                    ShowLanguage(true);
                }
                break;

            case (int)SettingItemTag.TAG_SETTING_BACKGROUND_MUSIC:
                {

                }
                break;

            case (int)SettingItemTag.TAG_SETTING_NOAD:
                {
                    OnClickBtnNoADIAP();
                }
                break;
            case (int)SettingItemTag.TAG_SETTING_RESTORE_IAP:
                {
                    OnClickBtnRestoreIAP();
                }
                break;

        }

        OnClickItem(item);
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

    public void IAPCallBack(string str)
    {
        Debug.Log("IAPCallBack::" + str);
        IAP.main.IAPCallBackBase(str);
    }

    #region UIViewAlert
    void OnUIViewAlertFinished(UIViewAlert alert, bool isYes)
    {
        if (STR_KEYNAME_VIEWALERT_NOAD == alert.keyName)
        {
            if (isYes)
            {
                DoBtnNoADIAP();

            }
        }
        if (STR_KEYNAME_VIEWALERT_UPDATE_VERSION == alert.keyName)
        {
            if (isYes)
            {

                if (AppVersion.main.appNeedUpdate)
                {
                    string url = AppVersion.main.strUrlAppstore;
                    if (!Common.BlankString(url))
                    {
                        Application.OpenURL(url);
                    }
                }

            }
        }




    }



    #endregion



    public void OnClickBtnNoADIAP()
    {
        if (Config.main.APP_FOR_KIDS)
        {
            ParentGateViewController.main.Show(null, null);
            ParentGateViewController.main.ui.callbackClose = OnUIParentGateDidCloseNoAd;
        }
        else
        {
            DoBtnNoAdAlert();
        }
    }


    public void DoBtnNoAdAlert()
    {
        string title = Language.main.GetString("STR_UIVIEWALERT_TITLE_NOAD");
        string msg = Language.main.GetString("STR_UIVIEWALERT_MSG_NOAD");
        string yes = Language.main.GetString("STR_UIVIEWALERT_YES_NOAD");
        string no = Language.main.GetString("STR_UIVIEWALERT_NO_NOAD");

        ViewAlertManager.main.ShowFull(title, msg, yes, no, true, STR_KEYNAME_VIEWALERT_NOAD, OnUIViewAlertFinished);
    }
    public void DoBtnNoADIAP()
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
        IAP.main.StartBuy(IAP.productIdNoAD, false);

    }

    public void OnClickBtnRestoreIAP()
    {
        if (Config.main.APP_FOR_KIDS)
        {
            ParentGateViewController.main.Show(null, null);
            ParentGateViewController.main.ui.callbackClose = OnUIParentGateDidCloseRestoreIAP;
        }
        else
        {
            DoBtnRestoreIAP();
        }

    }

    //恢复内购
    public void DoBtnRestoreIAP()
    {
        IAP.main.SetObjectInfo(this.gameObject.name, "IAPCallBack");

        // viewAlert.ShowBtnNo(false);
        // viewAlert.keyName = STR_KEYNAME_VIEWALERT_LOADING;
        // viewAlert.callback = OnUIViewAlertFinished;
        // string title = Language.main.GetString(AppString.STR_UIVIEWALERT_TITLE_SHOP_RESTORE_BUY);
        // string msg = Language.main.GetString(AppString.STR_UIVIEWALERT_MSG_SHOP_RESTORE_BUY);
        // string yes = Language.main.GetString(AppString.STR_UIVIEWALERT_YES_SHOP_RESTORE_BUY);
        // string no = Language.main.GetString(AppString.STR_UIVIEWALERT_YES_SHOP_RESTORE_BUY);
        // viewAlert.SetText(title, msg, yes, no);
        // viewAlert.Show();

        string product = IAP.productIdNoAD;
        if (Config.main.isNoIDFASDK && Common.isiOS)
        {
            product = Common.GetAppPackage() + "." + IAPConfig.main.GetIdByKey("unlocklevel");
        }
        IAP.main.RestoreBuy(product);

    }

    public void OnUIParentGateDidCloseNoAd(UIParentGate ui, bool isLongPress)
    {
        if (isLongPress)
        {
            DoBtnNoAdAlert();
        }
    }

    public void OnUIParentGateDidCloseRestoreIAP(UIParentGate ui, bool isLongPress)
    {
        if (isLongPress)
        {
            DoBtnRestoreIAP();
        }
    }
}
