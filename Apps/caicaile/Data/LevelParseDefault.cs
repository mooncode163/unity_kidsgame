using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class LevelParseDefault : GameLevelParseBase
{
    static private LevelParseDefault _main = null;
    public static LevelParseDefault main
    {
        get
        {
            if (_main == null)
            {
                _main = new LevelParseDefault();
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
            // info.title = JsonUtil.GetString(item, "title", ""); 
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

            info.gameType = infoPlace.gameType;
            Debug.Log("ParseGame::info.pic=" + info.pic + " info.gameType=" + info.gameType);
            listGuanka.Add(info);
            ParseItem(info);
        }

        count = listGuanka.Count;


        Debug.Log("ParseGame::count=" + count);
        return count;
    }



    public override void ParseItem(CaiCaiLeItemInfo info)
    {
         LanguageManager.main.UpdateLanguage(LevelManager.main.placeLevel);
        info.title = LanguageManager.main.languageGame.GetString(info.id);
    }



}
