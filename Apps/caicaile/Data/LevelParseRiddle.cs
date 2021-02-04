using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class LevelParseRiddle : GameLevelParseBase
{
    static private LevelParseRiddle _main = null;
    public static LevelParseRiddle main
    {
        get
        {
            if (_main == null)
            {
                _main = new LevelParseRiddle();
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
            DBInfoRiddle dbInfo = new DBInfoRiddle();
            info.dbInfo = dbInfo;

            info.id = JsonUtil.GetString(item, "id", "");
            info.title = JsonUtil.GetString(item, "title", "");
            info.pinyin = JsonUtil.GetString(item, "pronunciation", "");
            info.translation = JsonUtil.GetString(item, "translation", "");
            info.album = JsonUtil.GetString(item, "album", "");

            // dbInfo.id = info.id;
            // info.dbInfo.title = info.title;
            // Debug.Log("ParseGuanka info.id=" + info.dbInfo.id);
            //string str = "aa";// = languageGame.GetString(info.id);
            //Debug.Log(i + ":ParseGame:" + str);
            info.pic = CloudRes.main.rootPathGameRes + "/image/" + strPlace + "/" + info.id + ".png";
            info.icon = CloudRes.main.rootPathGameRes + "/image_thumb/" + strPlace + "/" + info.id + ".png";
            if (!FileUtil.FileIsExistAsset(info.icon))
            {
                info.icon = info.pic;
            }
        
            {
                //Riddle
                info.head = (string)item["head"];
                dbInfo.head = info.head;
                dbInfo.id = info.head;
                info.end = (string)item["end"];
                 dbInfo.end = info.end;
                    Debug.Log("GAME_RIDDLE ParseGuanka info.end="+info.end);
                info.tips = (string)item["tips"];
                 dbInfo.tips = info.tips;
                info.type = (string)item["type"];
                 dbInfo.type = info.type;
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


    }



}
