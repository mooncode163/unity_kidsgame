using System.Collections;
using System.Collections.Generic;
using Moonma.IAP;
using Moonma.AdKit.AdVideo;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;
using System;
using Moonma.Share;
public delegate void OnUICommentDidClickDelegate(ItemInfo item);
public class UIComment : UIView, ITableViewDataSource
{
    public const string KEY_HAS_COMMENT = "KEY_HAS_COMMENT";


    public GameObject objTopBar;
    public GameObject objTableViewTemplate;
    public Image imageBg;
    public Image imageBar;
    public Text textTitle;

    public TableView tableView;
    UICellItemBase cellItemPrefab;
    UICellBase cellPrefab;

    public string strShareTitle;
    public string strShareDetail;
    public string strSharPic;
    public string strShareUrl;

    public int numRows;
    private int numInstancesCreated = 0;

    private int oneCellNum;
    int heightCell;
    int goldClickItem;

    float textAdOffsetYNormal;
    float heightTobBarNormal;
    float heightTextAdNormal;
    RectTransform rctranTableviewNoarml;

    int totalItem;
    List<object> listItem;
    public OnUICommentDidClickDelegate callBackClick { get; set; }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        heightCell = 256 + 128;
        LoadPrefab();
        Share.main.SetObjectInfo(this.gameObject.name);
        //bg 
        //TextureUtil.UpdateImageTexture(imageBg, AppRes.IMAGE_SETTING_BG, true);


        rctranTableviewNoarml = objTableViewTemplate.GetComponent<RectTransform>();

        //item
        listItem = new List<object>();
        foreach (ItemInfo info in Config.main.listAppStore)
        {
            info.title = Language.main.GetString("COMMENT_" + info.source.ToUpper());
            info.pic = "Common/UI/Comment/icon_" + info.source;
            listItem.Add(info);
        }


        oneCellNum = 4;
        totalItem = listItem.Count;

        numRows = totalItem / oneCellNum;
        if (totalItem % oneCellNum != 0)
        {
            numRows++;
        }
        tableView.dataSource = this;

    }
    // Use this for initialization
    void Start()
    {

        // InitUiScaler();
        InitAlert();
        {
            string str = Language.main.GetString("COMMENT_TITLE");
            textTitle.text = str;
            int fontsize = textTitle.fontSize;
            float str_w = Common.GetStringLength(str, AppString.STR_FONT_NAME, fontsize);
            RectTransform rctran = imageBar.transform as RectTransform;
            Vector2 sizeDelta = rctran.sizeDelta;
            // float oft = AppResImage.REN_XUXIANKUANG_CIRCLE_R * AppCommon.scaleBase;
            sizeDelta.x = str_w + fontsize;
            rctran.sizeDelta = sizeDelta;
        }



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
            GameObject obj = PrefabCache.main.Load("Common/Prefab/Comment/UICommentCellItem");
            cellItemPrefab = obj.GetComponent<UICellItemBase>();
        }

    }
    public override void LayOut()
    {
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
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



    void InitAlert()
    {
        //   viewAlert.callback = OnUIViewAlertFinished;

    }
    void OnUIViewAlertFinished(UIViewAlert alert, bool isYes)
    {

        if (isYes)
        {

        }
        else
        {

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


    public void OnCellItemDidClick(UICellItemBase item)
    {

        int idx = item.index;
        ItemInfo info = listItem[idx] as ItemInfo;
        if (callBackClick != null)
        {
            callBackClick(info);
        }

        AppVersion.main.DoComment(info);

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


    #region Share CallBack

    public void ShareDidFinish(string str)
    {

    }
    public void ShareDidFail(string str)
    {

    }
    #endregion

}
