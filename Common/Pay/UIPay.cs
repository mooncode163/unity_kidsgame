using System.Collections;
using System.Collections.Generic;
using Moonma.IAP;
using Moonma.AdKit.AdVideo;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIPay : UIView, ITableViewDataSource
{
    public const string KEY_HAS_COMMENT = "KEY_HAS_COMMENT";


    public GameObject objTopBar;
    public GameObject objTableViewTemplate;
    public Image imageBg;
    public Image imageBar;
    public Text textTitle;

    public TableView tableView;
    public UIViewAlert viewAlert;
    public PayCellItem cellItemPrefab;
    public PayCell cellPrefab;//GuankaItemCell GameObject 

    public int numRows;
    private int numInstancesCreated = 0;

    private int oneCellNum;
    int heightCell;
    int goldClickItem;

    float textAdOffsetYNormal;
    float heightTobBarNormal;
    float heightTextAdNormal;
    RectTransform rctranTableviewNoarml;

    List<ItemInfo> listItem;
    float topBarOffsetYNormal;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        heightCell = 256 + 128;

        //bg
       //TextureUtil.UpdateImageTexture(imageBg, AppRes.IMAGE_SETTING_BG,true);

        topBarOffsetYNormal = objTopBar.GetComponent<RectTransform>().offsetMax.y;

        heightTobBarNormal = objTopBar.GetComponent<RectTransform>().rect.height;

        rctranTableviewNoarml = objTableViewTemplate.GetComponent<RectTransform>();

        //item
        listItem = new List<ItemInfo>();
        {
            ItemInfo info = new ItemInfo();
            info.source = Source.WEIXIN;
            info.title = Language.main.GetString("STR_PAY_WEIXIN");
            info.pic = "AppCommon/UI/Pay/pay_icon_weixin";
            listItem.Add(info);
        }
        {
            ItemInfo info = new ItemInfo();
            info.source = Source.QQ;
            info.title = Language.main.GetString("STR_PAY_QQ");
            info.pic = "AppCommon/UI/Pay/pay_icon_qq";
            listItem.Add(info);
        }

        oneCellNum = 4;
        int total = listItem.Count;

        numRows = total / oneCellNum;
        if (total % oneCellNum != 0)
        {
            numRows++;
        }

    }
    // Use this for initialization
    void Start()
    {
        // InitUiScaler();
        InitAlert();
        {
            string str = Language.main.GetString("STR_UIPAY_TITLE");
            textTitle.text = str;
            int fontsize = textTitle.fontSize;
            float str_w = Common.GetStringLength(str, AppString.STR_FONT_NAME, fontsize);
            RectTransform rctran = imageBar.transform as RectTransform;
            Vector2 sizeDelta = rctran.sizeDelta;
           // float oft = AppResImage.REN_XUXIANKUANG_CIRCLE_R * AppCommon.scaleBase;
            sizeDelta.x = str_w + fontsize;
            rctran.sizeDelta = sizeDelta;
        }

        tableView.dataSource = this;
        // LayOutChild();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnClickBtnBack();
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

        }
        {
            //
            if (!Device.isLandscape)
            {
                {
                    RectTransform rctran = objTopBar.GetComponent<RectTransform>();
                    Vector2 sizeDelta = rctran.sizeDelta;
                    float ofty = Common.ScreenToCanvasHeigt(sizeCanvas, Device.heightSystemTopBar);
                    Debug.Log("ofty =" + ofty + " sizeCanvas=" + sizeCanvas + " rctran.offsetMax.y=" + rctran.offsetMax.y);
                    rctran.offsetMax = new Vector2(rctran.offsetMax.x, topBarOffsetYNormal - ofty);
                    //offsetMax 修改之后sizeDelta也会跟着变化，需要还原
                    rctran.sizeDelta = sizeDelta;
                }


            }

        }

        {
            //
            RectTransform rctran = objTableViewTemplate.GetComponent<RectTransform>();
            Vector2 sizeDelta = rctran.sizeDelta;
            float topbar_offsety = 0;
            if (!Device.isLandscape)
            {
                topbar_offsety = Common.ScreenToCanvasHeigt(sizeCanvas, Device.heightSystemTopBar);
            }
            float ofty = topBarOffsetYNormal - heightTobBarNormal - topbar_offsety;

            Debug.Log("ofty=" + ofty + " " + topBarOffsetYNormal + " " + heightTobBarNormal + " " + heightTextAdNormal + " offsetMax=" + rctran.offsetMax);
            rctran.offsetMax = new Vector2(rctran.offsetMax.x, ofty);
            ofty = Common.ScreenToCanvasHeigt(sizeCanvas, Device.heightSystemHomeBar);
            rctran.offsetMin = new Vector2(rctran.offsetMin.x, rctranTableviewNoarml.offsetMin.y + ofty);

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
        DestroyImmediate(this.gameObject);
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
        PayCell cell = tableView.GetReusableCell(cellPrefab.reuseIdentifier) as PayCell;
        if (cell == null)
        {
            cell = (PayCell)GameObject.Instantiate(cellPrefab);
            cell.name = "PayCell" + (++numInstancesCreated).ToString();
            Rect rctable = (tableView.transform as RectTransform).rect;
            Vector2 sizeCell = (cellPrefab.transform as RectTransform).sizeDelta;
            Vector2 sizeTable = (tableView.transform as RectTransform).sizeDelta;
            Vector2 sizeCellNew = sizeCell;
            sizeCellNew.x = rctable.width;



            for (int i = 0; i < oneCellNum; i++)
            {

                PayCellItem item = cellItemPrefab;
                item = (PayCellItem)GameObject.Instantiate(item);

                // item.iDelegate = this;
                item.transform.SetParent(cell.transform, false);
                item.index = row * oneCellNum + i;


                RectTransform rctran = item.GetComponent<RectTransform>();
                rctran.sizeDelta = new Vector2(rctran.sizeDelta.x, heightCell);

                Rect rcItem = rctran.rect;
                Vector3 pos = new Vector3(rcItem.width * i, 0, 0);
 
                rctran.anchoredPosition = pos;
                cell.AddItem(i, item);
 

            }

        }

        int cellNumCur = oneCellNum;
        if (row == GetNumberOfRowsForTableView(tableView) - 1)
        {

            cellNumCur = listItem.Count - (GetNumberOfRowsForTableView(tableView) - 1) * oneCellNum;
        }

        cell.SetRowNumber(row, oneCellNum, cellNumCur);
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
