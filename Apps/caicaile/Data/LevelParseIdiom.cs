using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class LevelParseIdiom : GameLevelParseBase
{
    static private LevelParseIdiom _main = null;
    public static LevelParseIdiom main
    {
        get
        {
            if (_main == null)
            {
                _main = new LevelParseIdiom();
            }
            return _main;
        }
    }

    public override int ParseGuanka()
    {
        int count = 0;

        if ((listGuanka != null) && (listGuanka.Count != 0))
        {
            return listGuanka.Count;
        }

        listGuanka = new List<object>();

        int idx = LevelManager.main.placeLevel;

        ItemInfo infoPlace = LevelManager.main.GetPlaceItemInfo(LevelManager.main.placeLevel); 
        string filepath = CloudRes.main.rootPathGameRes + "/guanka/guanka_list_place" + idx + ".json";
        if (!FileUtil.FileIsExistAsset(filepath))
        {
            filepath = CloudRes.main.rootPathGameRes + "/guanka/item_" + infoPlace.id + ".json";
        }

        //
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(filepath);

        JsonData root = JsonMapper.ToObject(json);
        string strPlace = infoPlace.id;
        JsonData items = root["items"];

        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];
            CaiCaiLeItemInfo info = new CaiCaiLeItemInfo();
            IdiomItemInfo dbInfo = new IdiomItemInfo();
            info.dbInfo = dbInfo;
            info.id = JsonUtil.GetString(item, "id", "");
            info.title = JsonUtil.GetString(item, "title", "");
            info.pinyin = JsonUtil.GetString(item, "pronunciation", "");
            info.translation = JsonUtil.GetString(item, "translation", "");
            info.album = JsonUtil.GetString(item, "album", "");

            dbInfo.id = info.id;
            dbInfo.title = info.title;
            Debug.Log("ParseGuanka info.id=" + dbInfo.id);
            //string str = "aa";// = languageGame.GetString(info.id);
            //Debug.Log(i + ":ParseGame:" + str);
            info.pic = CloudRes.main.rootPathGameRes + "/image/" + strPlace + "/" + info.id + ".png";
            info.icon = CloudRes.main.rootPathGameRes + "/image_thumb/" + strPlace + "/" + info.id + ".png";
            if (!FileUtil.FileIsExistAsset(info.icon))
            {
                info.icon = info.pic;
            }
            string key = "xiehouyu";
            if (JsonUtil.ContainsKey(item, key))
            {
                JsonData xiehouyu = item[key];
                for (int j = 0; j < xiehouyu.Count; j++)
                {
                    JsonData item_xhy = xiehouyu[j];
                    if (j == 0)
                    {
                        info.head = (string)item_xhy["content"];
                    }
                    if (j == 1)
                    {
                        info.end = (string)item_xhy["content"];
                    }
                }

            }

            key = "head";
            if (JsonUtil.ContainsKey(item, key))
            {
                //Riddle
                info.head = (string)item["head"];
                info.end = (string)item["end"];
                info.tips = (string)item["tips"];
                info.type = (string)item["type"];
            }
            info.gameType = infoPlace.gameType;

            listGuanka.Add(info);
        }

        count = listGuanka.Count;


        Debug.Log("ParseGame::count=" + count);
        return count;
    }



    public override void ParseItem(CaiCaiLeItemInfo info)
    {
        IdiomItemInfo idiom = info.dbInfo as IdiomItemInfo;
        Debug.Log("info.id=" + info.id + " info.title=" + info.title);
        DBIdiom.main.GetItemById(info.dbInfo, info.id);
        // IdiomItemInfo idiom = DBIdiom.main.GetItemById(info.id); 

        // Debug.Log("dbInfo info.id=" + idiom.id + " info.title=" + idiom.title);
        if (Common.BlankString(idiom.title))
        {
            DBIdiom.main.GetItemByTitle(info.dbInfo, info.id);
            Debug.Log("dbInfo2 info.id=" + idiom.id + " info.title=" + idiom.title);
        }

       // info.dbInfo = idiom;
    }



}
