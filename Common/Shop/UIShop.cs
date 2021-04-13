using System.Collections;
using System.Collections.Generic;
using Moonma.IAP;
using Moonma.AdKit.AdVideo;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;
using System;
using Moonma.AdKit.AdConfig;
using Moonma.Share;

public class ShopItemInfo : ItemInfo
{
    public int gold;
    public bool isIAP;
}
public class UIShop : UIView, ITableViewDataSource
{
    public const string KEYNAME_VIEWALERT = "viewalert";
    public const string ID_GOLD_VIDEO = "id_gold_video";
    public const string ID_GOLD_COMMENT = "id_gold_comment";
    public const string ID_GOLD_SHARE = "id_gold_share";
    public const string KEY_HAS_COMMENT = "KEY_HAS_COMMENT";

    public const int GOLD_VIDEO = 10;
    public const int GOLD_COMMENT = 5;
    public const int GOLD_SHARE = 5;
    public const int GET_BUY_GOLD0 = 3;
    public const int GET_BUY_GOLD1 = 9;
    public const int GET_BUY_GOLD2 = 30;

    //public GameObject objTopBar;
    //public GameObject objTableViewTemplate;
    public Image imageBg;
    public UIImage imageBar;
    public UIText textTitle;
    public Text textAd;

    public TableView tableView;
    public Image imageGoldBg;
    public Text textGold;
    UICellItemBase cellItemPrefab;
    UICellBase cellPrefab;//GuankaItemCell GameObject 

    public int numRows;
    private int numInstancesCreated = 0;

    private int oneCellNum;
    int heightCell;
    int goldClickItem;

    int totalItem;
    List<object> listItem;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        LoadPrefab();
        heightCell = 256;
        IAP.main.SetObjectInfo(this.gameObject.name, "IAPCallBack");
        //textTitle.color = ColorConfig.main.GetColor(GameRes.KEY_COLOR_LevelTitle, Color.white);

        InitAd();
        //bg 
       // TextureUtil.UpdateImageTexture(imageBg, AppRes.IMAGE_SETTING_BG, true);

        textAd.text = Language.main.GetString("STR_SHOP_TIP_AD");
        textAd.gameObject.SetActive(false);

        //    topBarOffsetYNormal = objTopBar.GetComponent<RectTransform>().offsetMax.y;
        // textAdOffsetYNormal = textAd.GetComponent<RectTransform>().offsetMax.y;
        // heightTobBarNormal = objTopBar.GetComponent<RectTransform>().rect.height;
        // heightTextAdNormal = textAd.GetComponent<RectTransform>().rect.height;
        // rctranTableviewNoarml = objTableViewTemplate.GetComponent<RectTransform>();

        //item
        listItem = new List<object>();
        int idx = 0;
        {
            ShopItemInfo info = new ShopItemInfo();
            info.gold = GOLD_VIDEO;
            info.title = Language.main.GetReplaceString("STR_SHOP_GOLD_VIDEO_TITLE", AppCommon.STR_LANGUAGE_REPLACE, info.gold.ToString());
            info.artist = Language.main.GetString("STR_SHOP_IAP_BTN_BUY_TITLE0");
            info.tag = idx++;
            info.id = ID_GOLD_VIDEO;//Common.GetAppPackage() + ".gold0";
            info.isIAP = false;
            listItem.Add(info);
        } 
        bool isHave = true; 
        if (Common.isAndroid)
        { 
            if (Config.main.channel == Source.HUAWEI)
            {
               isHave = false; 
            }
        }
        if(isHave)
        {
            ShopItemInfo info = new ShopItemInfo();
            info.gold = GOLD_COMMENT;
            info.title = Language.main.GetReplaceString("STR_SHOP_GOLD_COMMENT_TITLE", AppCommon.STR_LANGUAGE_REPLACE, info.gold.ToString());
            info.artist = Language.main.GetString("STR_SHOP_IAP_BTN_BUY_TITLE0");
            info.tag = idx++;
            info.id = ID_GOLD_COMMENT;
            info.isIAP = false;
            listItem.Add(info);
        }
        if (Config.main.isHaveShare)
        {
            ShopItemInfo info = new ShopItemInfo();
            info.gold = GOLD_SHARE;
            info.title = Language.main.GetReplaceString("STR_SHOP_GOLD_SHARE_TITLE", AppCommon.STR_LANGUAGE_REPLACE, info.gold.ToString());
            info.artist = Language.main.GetString("STR_SHOP_IAP_BTN_BUY_TITLE0");
            info.tag = idx++;
            info.id = ID_GOLD_SHARE;//Common.GetAppPackage() + ".gold0";
            info.isIAP = false;
            listItem.Add(info);
        }

        if (Config.main.isHaveIAP)
        {
            if (AppVersion.appCheckHasFinished)
            {
                textAd.gameObject.SetActive(true);
            }

            {
                ShopItemInfo info = new ShopItemInfo();
                info.title = Language.main.GetString("STR_SHOP_IAP_TITLE0");
                info.artist = Language.main.GetString("STR_SHOP_IAP_BTN_BUY_TITLE0");
                info.tag = idx++;
                string product = Config.main.GetIAPProduct("IAP_PRODUCT0");
                if (Common.BlankString(product))
                {
                    product = "gold0";
                }
                info.id = product;//Common.GetAppPackage() + ".gold0";
                info.gold = GET_BUY_GOLD0;
                info.isIAP = true;
                listItem.Add(info);
            }
            {
                ShopItemInfo info = new ShopItemInfo();
                info.title = Language.main.GetString("STR_SHOP_IAP_TITLE1");
                info.artist = Language.main.GetString("STR_SHOP_IAP_BTN_BUY_TITLE1");
                info.tag = idx++;
                string product = Config.main.GetIAPProduct("IAP_PRODUCT1");
                if (Common.BlankString(product))
                {
                    product = "gold1";
                }
                info.id = product;
                info.gold = GET_BUY_GOLD1;
                info.isIAP = true;
                listItem.Add(info);
            }
            {
                ShopItemInfo info = new ShopItemInfo();
                info.title = Language.main.GetString("STR_SHOP_IAP_TITLE2");
                info.artist = Language.main.GetString("STR_SHOP_IAP_BTN_BUY_TITLE2");
                info.tag = idx++;
                string product = Config.main.GetIAPProduct("IAP_PRODUCT2");
                if (Common.BlankString(product))
                {
                    product = "gold2";
                }
                info.id = product;
                info.gold = GET_BUY_GOLD2;
                info.isIAP = true;
                listItem.Add(info);
            }
        }

        oneCellNum = 1;
        totalItem = listItem.Count;
        numRows = listItem.Count;
    }
    // Use this for initialization
    void Start()
    {
        {
            string str = textTitle.text;//Language.main.GetString(AppString.STR_SHOP);
                                        // textTitle.text = str;
            int fontsize = textTitle.fontSize;
            float str_w = Common.GetStringLength(str, AppString.STR_FONT_NAME, fontsize);
            RectTransform rctran = imageBar.transform as RectTransform;
            Vector2 sizeDelta = rctran.sizeDelta;
            float oft = 0;
            sizeDelta.x = str_w + fontsize;
            rctran.sizeDelta = sizeDelta;
        }

        tableView.dataSource = this;
        UpdateGold();
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
            GameObject obj = PrefabCache.main.Load(AppCommon.PREFAB_SHOP_CELL_ITEM);
            cellItemPrefab = obj.GetComponent<UICellItemBase>();
        }

    }

    /// <summary>
    /// Callback sent to all game objects when the player pauses.
    /// </summary>
    /// <param name="pauseStatus">The pause state of the application.</param>
    void OnApplicationPause(bool pauseStatus)
    {
        Debug.Log("UIShop OnApplicationPause pauseStatus=" + pauseStatus);
        if (!pauseStatus)
        {
            //app返回前台 刷新评论状态
            tableView.ReloadData();
        }
    }
    /// <summary>
    /// Callback sent to all game objects when the player gets or loses focus.
    /// </summary>
    /// <param name="focusStatus">The focus state of the application.</param>
    void OnApplicationFocus(bool focusStatus)
    {
        Debug.Log("UIShop OnApplicationFocus focusStatus=" + focusStatus);
    }


    void UpdateGold()
    {

        string str = Language.main.GetString("STR_GOLD") + ":" + Common.gold.ToString();
        textGold.text = str;
        int fontsize = textGold.fontSize;
        float str_w = Common.GetStringLength(str, AppString.STR_FONT_NAME, fontsize);
        RectTransform rctran = imageGoldBg.transform as RectTransform;
        Vector2 sizeDelta = rctran.sizeDelta;

        sizeDelta.x = str_w + fontsize;
        rctran.sizeDelta = sizeDelta;


    }
  public override  void LayOut()
    {
        base.LayOut();
        Vector2 sizeCanvas = AppScene.main.sizeCanvas;
        {
            RectTransform rctran = imageBg.GetComponent<RectTransform>();
            float w_image = rctran.rect.width;
            float h_image = rctran.rect.height;
            float scalex = sizeCanvas.x / w_image;
            float scaley = sizeCanvas.y / h_image;
            float scale = Mathf.Max(scalex, scaley);
            imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);
            //屏幕坐标 现在在屏幕中央
            imageBg.transform.position = new Vector2(Screen.width / 2, Screen.height / 2);
        }

    }
    static public string GetDateString()
    {
        return DateTime.Now.ToString("yyyy-MM-dd");// 2008-09-04
    }
    void InitAd()
    {
        AdKitCommon.main.callbackAdVideoFinish = OnAdKitAdVideoFinish;
        AdVideo.SetType(AdVideo.ADVIDEO_TYPE_REWARD);
        int type = AdConfigParser.SOURCE_TYPE_VIDEO;
        string source = AdConfig.main.GetAdSource(type);
        AdVideo.InitAd(source);
    }

    public void OnAdKitAdVideoFinish(AdKitCommon.AdType type, AdKitCommon.AdStatus status, string str)
    {
        if (type == AdKitCommon.AdType.VIDEO)
        {
            if (status == AdKitCommon.AdStatus.SUCCESFULL)
            {
                OnBuyFinish();
            }
        }
    }

    void ShowShare()
    {
        ShareViewController.main.callBackClick = OnUIShareDidClick;
        ShareViewController.main.Show(null, null);
    }



    public void OnUIShareDidClick(ItemInfo item)
    {
        string title = Language.main.GetString("UIMAIN_SHARE_TITLE");
        string detail = Language.main.GetString("UIMAIN_SHARE_DETAIL");
        string url = Config.main.shareAppUrl;
        Share.main.ShareWeb(item.source, title, detail, url);
        OnBuyFinish();
    }
    void OnUIViewAlertFinished(UIViewAlert alert, bool isYes)
    {

        if (isYes)
        {
            StopBuy();
        }
        else
        {

        }



    }
    public void StopBuy()
    {

    }

    public void OnBuyFinish()
    {
        Common.gold = Common.gold + goldClickItem;
        UpdateGold();
    }
    public void IAPCallBack(string str)
    {
        Debug.Log("IAPCallBack::" + str);
        if (str == IAP.UNITY_CALLBACK_BUY_DID_FINISH)
        {
            ViewAlertManager.main.Hide();
            OnBuyFinish();
            //去除广告
            Common.noad = true;
            Common.isRemoveAd = true;
        }
        if (str == IAP.UNITY_CALLBACK_BUY_DID_Fail)
        {

            string title = Language.main.GetString(AppString.STR_UIVIEWALERT_TITLE_SHOP_BUY_FAIL);
            string msg = Language.main.GetString(AppString.STR_UIVIEWALERT_MSG_SHOP_BUY_FAIL);
            string yes = Language.main.GetString(AppString.STR_UIVIEWALERT_YES_SHOP_BUY_FAIL);
            string no = Language.main.GetString(AppString.STR_UIVIEWALERT_YES_SHOP_BUY_FAIL);

            ViewAlertManager.main.ShowFull(title, msg, yes, no, false, KEYNAME_VIEWALERT, OnUIViewAlertFinished);

        }
        if (str == IAP.UNITY_CALLBACK_DID_BUY)
        {
            ViewAlertManager.main.Hide();
        }
        if (str == IAP.UNITY_CALLBACK_BUY_DID_RESTORE)
        {
            ViewAlertManager.main.Hide();
        }
        if (str == IAP.UNITY_CALLBACK_BUY_CANCEL_BY_USER)
        {
            ViewAlertManager.main.Hide();
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

    public void OnUICommentDidClick(ItemInfo item)
    {
        OnBuyFinish();
    }

    public void OnCellItemDidClick(UICellItemBase item)
    {
        int idx = item.index;
        ShopItemInfo info = listItem[idx] as ShopItemInfo;
        goldClickItem = info.gold;
        if (info.isIAP)
        {
            string title = Language.main.GetString(AppString.STR_UIVIEWALERT_TITLE_SHOP_START_BUY);
            string msg = Language.main.GetString(AppString.STR_UIVIEWALERT_MSG_SHOP_START_BUY);
            string yes = Language.main.GetString(AppString.STR_UIVIEWALERT_YES_SHOP_START_BUY);
            string no = Language.main.GetString(AppString.STR_UIVIEWALERT_YES_SHOP_START_BUY);
            ViewAlertManager.main.ShowFull(title, msg, yes, no, false, KEYNAME_VIEWALERT, OnUIViewAlertFinished);

            IAP.main.StartBuy(info.id);
            //IAPCallBack(IAP.UNITY_CALLBACK_BUY_DID_Fail);
        }
        else
        {
            if (info.id == ID_GOLD_VIDEO)
            {
                AdKitCommon.main.callbackAdVideoFinish = OnAdKitAdVideoFinish;
                AdKitCommon.main.ShowAdVideo();
                if (Application.isEditor)
                {
                    OnBuyFinish();
                }
            }
            if (info.id == ID_GOLD_SHARE)
            {
                ShowShare();
            }
            if (info.id == ID_GOLD_COMMENT)
            {
                string str_date = GetDateString();
                PlayerPrefs.SetString(KEY_HAS_COMMENT, str_date);
                //AppVersion.main.callBackCommentClick = OnUICommentDidClick;
                //AppVersion.main.OnComment();
                AppVersion.main.GotoComment();
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



}
