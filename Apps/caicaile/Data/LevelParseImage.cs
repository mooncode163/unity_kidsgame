using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class LevelParseImage : GameLevelParseBase
{
    static private LevelParseImage _main = null;
    public static LevelParseImage main
    {
        get
        {
            if (_main == null)
            {
                _main = new LevelParseImage();
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
        LanguageManager.main.UpdateLanguage(idx);
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
            info.id = JsonUtil.GetString(item, "id", "");
            // 
            info.title =info.id;// LanguageManager.main.languageGame.GetString(info.id);
            //string str = "aa";// = languageGame.GetString(info.id);
            //Debug.Log(i + ":ParseGame:" + str);
            info.pic = CloudRes.main.rootPathGameRes + "/image/" + strPlace + "/" + info.id + ".png";
            info.icon = CloudRes.main.rootPathGameRes + "/image_thumb/" + strPlace + "/" + info.id + ".png";
            if (!FileUtil.FileIsExistAsset(info.icon))
            {
                info.icon = info.pic;
            }
        

         
            info.gameType = GameRes.GAME_TYPE_IMAGE;


            listGuanka.Add(info);
        }

        count = listGuanka.Count;


        Debug.Log("ParseGame::count=" + count);
        return count;
    }



    public override void ParseItem(CaiCaiLeItemInfo info)
    {
        //info.title = LanguageManager.main.languageGame.GetString(info.id);
    }



}
