using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;


public class PositionInfo
{
    public List<RowColInfo> listRowCol;
}

public class RowColInfo
{
    public int row;
    public int col;
}
public class LevelParseIdiomFlower : GameLevelParseBase
{
    public List<object> listPosition;
    public List<object> listCategory;

    public string categoryTitle;
    public string sortTitle;

    static private LevelParseIdiomFlower _main = null;
    public static LevelParseIdiomFlower main
    {
        get
        {
            if (_main == null)
            {
                _main = new LevelParseIdiomFlower();
                _main.ParsePosition();
                _main.ParseCategory();
                if (_main.listCategory.Count > 0)
                {
                    CaiCaiLeItemInfo info = _main.listCategory[0] as CaiCaiLeItemInfo;
                    _main.categoryTitle = info.title;
                    List<object> listSort = _main.ParseSort(_main.categoryTitle);

                    info = listSort[0] as CaiCaiLeItemInfo;
                    _main.sortTitle = info.title;
                }

            }
            return _main;
        }
    }

    public override int ParseGuanka()
    {
        int count = 1;
        if ((listGuanka != null) && (listGuanka.Count != 0))
        {
            return listGuanka.Count;
        }

        listGuanka = new List<object>();

        int idx = LevelManager.main.placeLevel;

        ItemInfo infoPlace = LevelManager.main.GetPlaceItemInfo(LevelManager.main.placeLevel);

        string filepath = CloudRes.main.rootPathGameRes + "/guanka/" + categoryTitle + "/" + sortTitle + ".json";
        // Debug.Log("ParseGuanka filepath=" + filepath);
        //string filepath = CloudRes.main.rootPathGameRes + "/guanka/first.json";
        //
        //FILE_PATH
        if (!FileUtil.FileIsExistAsset(filepath))
        {
            return count;
        }
        string json = FileUtil.ReadStringAsset(filepath);
        JsonData root = JsonMapper.ToObject(json);

        string strPlace = infoPlace.id;
        JsonData items = root["items"];
        count = items.Count;
        for (int i = 0; i < count; i++)
        {
            JsonData item = items[i];
            CaiCaiLeItemInfo info = new CaiCaiLeItemInfo();
            info.id = JsonUtil.GetString(item, "id", "");
            info.title = JsonUtil.GetString(item, "title", "");
            if (info.title.Length != 4)
            {
                continue;
            }
            info.pinyin = JsonUtil.GetString(item, "pronunciation", "");
            info.translation = JsonUtil.GetString(item, "translation", "");
            info.album = JsonUtil.GetString(item, "album", "");
            info.pic = CloudRes.main.rootPathGameRes + "/image/" + strPlace + "/" + info.id + ".png";
            info.icon = CloudRes.main.rootPathGameRes + "/image_thumb/" + strPlace + "/" + info.id + ".png";
            if (!FileUtil.FileIsExistAsset(info.icon))
            {
                info.icon = info.pic;
            }
            info.gameType = GameRes.GAME_TYPE_FLOWER;
            info.listIdiom = new List<string>();

            info.listIdiom.Add(info.title);
            info.listIdiom.Add(JsonUtil.GetString(item, "other0", ""));
            info.listIdiom.Add(JsonUtil.GetString(item, "other1", ""));
            info.listIdiom.Add(JsonUtil.GetString(item, "other2", ""));


            listGuanka.Add(info);
        }
        count = listGuanka.Count;

        // Debug.Log("ParseGuankaIdiomConnect count=" + count + " filepath=" + filepath);
        //每4个一组合
        //  count = count / 4;

        return count;
    }

    public override void ParseItem(CaiCaiLeItemInfo info)
    {
        DBItemInfoBase dbInfo = null;
        Debug.Log("Flower UpdateItem ParseItem info.id=" + info.id + " info.pinyin=" + info.pinyin + " info.title=" + info.title);

        // if (Common.BlankString(info.id))
        // {
        //     dbInfo = DBIdiom.main.GetItemByTitle(info.title);
        // }
        // else
        // {
        //     dbInfo = DBIdiom.main.GetItemById(info.id);
        // }
        // IdiomItemInfo idiom = dbInfo as IdiomItemInfo;
        // if (Common.BlankString(info.pinyin))
        // {
        //     info.title = idiom.title;
        //     info.album = idiom.album;
        //     info.translation = idiom.translation;
        //     info.pinyin = idiom.pronunciation;
        //     info.id = idiom.id;
        // }
        // info.dbInfo = idiom;
    }

    public override void ParseItemDetal(CaiCaiLeItemInfo info)
    {


    }
    public List<object> ParseCategory()
    {
        int count = 1;
        if ((listCategory != null) && (listCategory.Count != 0))
        {
            return listCategory;
        }
        listCategory = new List<object>();
        string filepath = CloudRes.main.rootPathGameRes + "/guanka/category.json";
        if (!FileUtil.FileIsExistAsset(filepath))
        {
            return listCategory;
        }
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(filepath);
        JsonData root = JsonMapper.ToObject(json);
        JsonData items = root["items"];
        count = items.Count;
        for (int i = 0; i < count; i++)
        {
            JsonData item = items[i];
            CaiCaiLeItemInfo info = new CaiCaiLeItemInfo();
            info.title = JsonUtil.GetString(item, "title", "");
            listCategory.Add(info);
        }
        return listCategory;
    }

    public List<object> ParseSort(string category)
    {
        int count = 1;
        List<object> listSort = new List<object>();
        string filepath = CloudRes.main.rootPathGameRes + "/guanka/" + category + "/sort.json";
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(filepath);
        JsonData root = JsonMapper.ToObject(json);
        JsonData items = root["items"];
        count = items.Count;
        for (int i = 0; i < count; i++)
        {
            JsonData item = items[i];
            CaiCaiLeItemInfo info = new CaiCaiLeItemInfo();
            info.title = JsonUtil.GetString(item, "title", "");
            listSort.Add(info);
        }
        return listSort;
    }

    // "rowcol": "2-0,3-0,3-1,2-1,3-2,2-2,3-3,2-3,0-3,0-2,1-3,1-2,0-0,1-1,1-0,0-1"
    public List<RowColInfo> ParseRowColInfo(string rowcol)
    {
        List<RowColInfo> list = new List<RowColInfo>();

        string[] listRL = rowcol.Split(',');
        for (int i = 0; i < listRL.Length; i++)
        {
            string tmp = listRL[i];
            string[] listtmp = tmp.Split('-');
            RowColInfo info = new RowColInfo();
            info.row = Common.String2Int(listtmp[0]);
            info.col = Common.String2Int(listtmp[1]);
            list.Add(info);
        }
        return list;
    }

    //排列位置信息

    public void ParsePosition()
    {
        if (listPosition == null)
        {
            listPosition = new List<object>();
        }
        else
        {
            return;
        }
        Debug.Log("ParsePosition ");
        {
            string filepath = CloudRes.main.rootPathGameRes + "/guanka/data/item_position.json";
            //
            //FILE_PATH
            string json = FileUtil.ReadStringAsset(filepath);
            JsonData root = JsonMapper.ToObject(json);
            JsonData items = root["items"];

            for (int i = 0; i < items.Count; i++)
            {
                JsonData item = items[i];
                string strrowcol = JsonUtil.GetString(item, "rowcol", "0");
                PositionInfo infoPos = new PositionInfo();
                infoPos.listRowCol = ParseRowColInfo(strrowcol);
                listPosition.Add(infoPos);
            }



        }
    }
}
