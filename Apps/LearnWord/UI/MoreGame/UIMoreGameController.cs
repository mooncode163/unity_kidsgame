using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Moonma.AdKit.AdConfig;
using LitJson;

public class UIMoreGameController : UITableViewControllerBase
{


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public void Awake()
    {
        base.Awake();
        ParseGameList();
        heightCell = 512;
        oneCellNum = 2;


    }
    public void Start()
    {
        base.Start();
        UpdateTable(false);
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



    public void ParseGameList()
    {
        int count = 0;
        listItem = new List<object>();
        long tickJson = Common.GetCurrentTimeMs();
        string fileName = CloudRes.main.rootPathGameRes +"/moregame/moregame" + ".json";
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(fileName);//((TextAsset)Resources.Load(fileName, typeof(TextAsset))).text;
        Debug.Log("json = " + json);
        // Debug.Log("json::"+json);
        JsonData root = JsonMapper.ToObject(json);
        JsonData items = root["items"];
        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];
            ItemInfo info = new ItemInfo();
            info.id = (string)item["id"];
            info.pic = CloudRes.main.rootPathGameRes +"/moregame/image/" + info.id + ".png";
            // info.pic =  CloudRes.main.rootPathGameRes +"/moregame/image/Pintu"+ ".png";
            if (Config.main.channel == Source.HUAWEI)
            {
                if (info.id == MoreGameRes.Book)
                {
                    continue;
                }
            }
            listItem.Add(info);

            // listItem.Add(info);
        }
        count = listItem.Count;
        tickJson = Common.GetCurrentTimeMs() - tickJson;
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
        // LevelManager.main.placeLevel = item.index;
        ItemInfo info = listItem[item.index] as ItemInfo;
        Debug.Log("OnCellItemDidClick index=" + item.index + " info.id=" + info.id);
        AudioPlay.main.PlayBtnSound();
        MoreGameViewController.main.currentGameId = info.id;
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;

            if (info.id == MoreGameRes.MATCH)
            {
                navi.Push(MatchViewController.main);
            }
            if (info.id == MoreGameRes.PINTU)
            {
                navi.Push(PintuViewController.main);
            }
            if (info.id == MoreGameRes.Book)
            {
                navi.Push(BookViewController.main);
            }

            if (info.id == MoreGameRes.FreeWrite)
            {
                navi.Push(FreeWriteViewController.main);
            }
            if (info.id == MoreGameRes.History)
            {
                navi.Push(HistoryViewController.main);
            }
        }


    }

    public void OnClickBtnBack()
    {

    }

    public void OnClickBtnPre()
    {
    }
    public void OnClickBtnNext()
    {
    }



}

