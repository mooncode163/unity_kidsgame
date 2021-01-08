using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using Moonma.Share;
using SimpleSQL;
//笔画顺序查询网： http://bishun.shufaji.com/0x6211.html
//https://github.com/chanind/hanzi-writer
//https://github.com/chanind/hanzi-writer-data
//https://cdn.jsdelivr.net/npm/hanzi-writer-data@latest/%E6%88%91.json
//


public class UIWordDetail : UIView
{

    // public SimpleSQLManager dbManager;
    //top
    public UIImage imageBgWord;
    public UIImage imageWord;

    //detail
    public UIImage imageBgDetail;
    public UIText textWord;
    public UIText textPinyin;//拼音
    public UIText textZuci;//组词
    public UIText textBushou;//部首
    public UIText textBihua;//笔画
    public UIText textContentPinyin;
    public UIText textContentZuci;
    public UIText textContentBushou;
    public UIText textContentBihua;

    void Awake()
    {
        LoadPrefab();
        // AppSceneBase.main.UpdateWorldBg(AppRes.IMAGE_GAME_BG);
    }
    // Use this for initialization
    void Start()
    {
        UpdateGuankaLevel(LevelManager.main.gameLevel);
        LayOut();

        OnUIDidFinish();
    }

    void LoadPrefab()
    {


    }

    void UpdateItem(WordItemInfo info)
    {
        textWord.text = info.dbInfo.title;
        Debug.Log("UpdateItem info.dbInfo.pinyin=" + info.dbInfo.pinyin);

        textContentPinyin.text = info.dbInfo.pinyin;
        textContentZuci.text = info.dbInfo.imagetitle;
        textContentBushou.text = info.dbInfo.bushou;
        textContentBihua.text = info.dbInfo.bihuaCount;
        if(FileUtil.FileIsExist(info.pic))
        {
            imageWord.UpdateImage(info.pic);
        }

        LayOut();
    }


    public void UpdateGuankaLevel(int level)
    {
        AppSceneBase.main.ClearMainWorld();
        string word = DetailViewController.main.word;
        WordItemInfo infonow = GameLevelParse.main.GetItemInfo();
        GameLevelParse.main.ParseItem(infonow);
        //infonow.id = "abc";
        //  infonow.word = infonow.id;
        // string strsql = "select * from " + DBLearnWord.TABLE_NAME + " where word = '" + infonow.word + "'";
        // //string strsql = "select * from  TableItem  where id = '一'";
        // //string strsql = "select * from " + DBLearnWord.TABLE_NAME;
        // Debug.Log("strsql = " + strsql);
        // List<DBWordInfo> listRet = dbManager.Query<DBWordInfo>(strsql);
        // WordItemInfo info = infonow;//new WordItemInfo();

        // foreach (DBWordInfo dbinfo in listRet)
        // {
        //     info.id = dbinfo.id;
        //     info.word = dbinfo.word;
        //     info.pinyin = dbinfo.pinyin;
        //     info.zuci = dbinfo.zuci;
        //     info.bushou = dbinfo.bushou;
        //     info.bihua = dbinfo.bihua;
        //     info.audio = dbinfo.audio;
        //     info.gif = dbinfo.gif;
        //     info.mean = dbinfo.mean;
        //     break;
        // }
        long tickItem = Common.GetCurrentTimeMs();
        tickItem = Common.GetCurrentTimeMs() - tickItem;
        Debug.Log("ParserGuankaItem: tickItem=" + tickItem);
        UpdateItem(infonow);
    }
    public override void LayOut()
    {
        base.LayOut();
        // base.LayOut();
    }
    public void OnClickBtnWrite()
    {

    }
    public void OnClickBtnBack()
    {
        NaviViewController navi = this.controller.naviController;
        if (navi != null)
        {
            navi.Pop();
        }
    } 

}
