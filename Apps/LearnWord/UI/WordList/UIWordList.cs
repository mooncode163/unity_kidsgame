using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Moonma.AdKit.AdConfig;

public class UIWordList : UITableViewControllerBase, IUIInputBarDelegate
{
    public UIRawImage imageBg;
    public UIInputBar uiInputBar;
    public UIButton btnBack;


    public enum Type
    {
        LIST = 0,
        SEARCH,
    }

    public Type _type;
    public Type type
    {
        get
        {
            return _type;
        }

        set
        {
            _type = value;
            switch (_type)
            {
                case Type.LIST:
                    {
                        bool isShow = true;
                        tableView.gameObject.SetActive(isShow);
                        btnBack.gameObject.SetActive(!isShow);
                    }
                    break;
                case Type.SEARCH:
                    {
                        bool isShow = false;
                        tableView.gameObject.SetActive(isShow);
                        btnBack.gameObject.SetActive(!isShow);
                    }
                    break;
            }
            LayOut();
        }

    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public void Awake()
    {
        base.Awake();
        //  textPlaceHold.text = Language.main.GetString("STR_SEARCH");
        uiInputBar.iDelegate = this;
        // heightCell = 320;
        // oneCellNum = 4;

        heightCell = 480;
        oneCellNum = 3;
        long tick = Common.GetCurrentTimeMs();
        // LevelManager.main.ParseGuanka();
        // GameLevelParse.main.ParseGuanka();
        tick = Common.GetCurrentTimeMs() - tick;
        // 15s
        Debug.Log("UIWordList tick =" + tick);
        listItem = GameLevelParse.main.listGuanka;
        //bg
        //TextureUtil.UpdateRawImageTexture(imageBg, AppRes.IMAGE_PLACE_BG, true);//IMAGE_GAME_BG
        this.type = Type.LIST;

    }
    public void Start()
    {
        base.Start();
        UpdateTable(false);
        LayOut();

        OnUIDidFinish();

        Common.UnityStartUpFinish();

        // GetAppVersion();
    }

    public async void GetAppVersion()
    {
        Debug.Log("Task GetVersion start");
        string ret = await HuaweiAppGalleryApi.main.GetVersion("103066765");
        Debug.Log("Task GetVersion end ret=" + ret);
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
        base.LayOut();

    }
    public override void OnCellItemDidClick(UICellItemBase item)
    {
        if (item.IsLock())
        {
            return;
        }
        GotoDetail(item.index);
    }

    void GotoDetail(int idx)
    {
        LevelManager.main.gameLevel = idx;
        NaviViewController navi = this.controller.naviController;
        if (navi != null)
        {
            navi.Push(DetailViewController.main);

        }
        if (idx > GameLevelParse.ADVIDEO_LEVEL_MIN)
        {
            AdKitCommon.main.ShowAdVideo();
        }
    }

    public void OnClickBtnBack()
    {
        this.type = Type.LIST;
    }

    public void OnClickBtnPre()
    {
    }
    public void OnClickBtnNext()
    {
    }




    public void OnUIInputBarValueChanged(UIInputBar ui)
    {

        string str = ui.text;
        // this.type = Type.SEARCH; 

        if (Common.BlankString(ui.text))
        {
            return;
        }
        List<object> ls = DBWord.main.Search(ui.text);
        // ui.text = "T";
        Debug.Log("OnUIInputBarEnd text=" + ui.text + " count=" + ls.Count);
        // uiIdiomList.UpdateList(ls);

        SearchViewController p = SearchViewController.main;
        p.litItem = ls;
        // p.Show(null, null);

        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            if (navi != null)
            {
                navi.Push(p);

            }
        }
    }
    public void OnUIInputBarEnd(UIInputBar ui)
    {

    }

}

