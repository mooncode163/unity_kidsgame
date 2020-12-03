using System.Collections;
using System.Collections.Generic;
using Moonma.IAP;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;
public delegate void OnUILanguageDidCloseDelegate(UILanguage language);
public class UILanguage : UIViewPop, ITableViewDataSource
{
    UICellItemBase cellItemPrefab;
    UICellBase cellPrefab;
    public Image imageBoard;
    public Image imageBg;
    public GameObject objContent;
    public TableView tableView;
    public Text textTitle;
    public int numRows;
    private int numInstancesCreated = 0;


    public List<object> listItem;
    int oneCellNum;
    int heightCell;
    int totalItem;
    public OnUILanguageDidCloseDelegate callbackClose { get; set; }
    protected override void Awake()
    {
        base.Awake();
        heightCell = 160;
        textTitle.color = AppRes.colorTitle;
        textTitle.text = Language.main.GetString("STR_LANGUAGE");
        LoadPrefab();
       // TextureUtil.UpdateImageTexture(imageBg, AppRes.IMAGE_SETTING_BG, true);
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        listItem = new List<object>();
        UpdateItem();
        oneCellNum = 1;
        totalItem = listItem.Count;
        numRows = totalItem;
        tableView.dataSource = this;


        LayOut();
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


   public  override  void LayOut()
    {
        float w, h;
        base.LayOut();

        //Vector2 sizeCanvas = ViewControllerManager.sizeCanvas;
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
        {
            RectTransform rctran = objContent.GetComponent<RectTransform>();
            w = Mathf.Min(this.frame.width, this.frame.height) * 0.8f;
            h = w;
            rctran.sizeDelta = new Vector2(w, h);

        }


    }

    void LoadPrefab()
    {
        {
            GameObject obj = PrefabCache.main.Load(AppCommon.PREFAB_UICELLBASE);
            cellPrefab = obj.GetComponent<UICellBase>();
        }
        {
            GameObject obj = PrefabCache.main.Load(AppCommon.PREFAB_UILanguageCellItem);
            cellItemPrefab = obj.GetComponent<UICellItemBase>();
        }

    }

    public void OnClickBtnBack()
    {
        // PopViewController pop = (PopViewController)this.controller;
        // if (pop != null)
        // {
        //     pop.Close();
        // }
        Close();
    }

    public void UpdateItem()
    {
        listItem.Clear();
        {
            ItemInfo info = new ItemInfo();
            info.title = "中文";
            info.tag = (int)SystemLanguage.Chinese;
            listItem.Add(info);
        }
        {
            ItemInfo info = new ItemInfo();
            info.title = "English";
            info.tag = (int)SystemLanguage.English;
            listItem.Add(info);
        }

    }

    public void OnCellItemDidClick(UICellItemBase item)
    {
        SystemLanguage lan = (SystemLanguage)item.tagValue;
        Language.main.SetLanguage(lan);
        PlayerPrefs.SetInt(AppString.STR_KEY_LANGUAGE, item.tagValue);
        if (this.callbackClose != null)
        {
            this.callbackClose(this);
        }
        OnClickBtnBack();
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


