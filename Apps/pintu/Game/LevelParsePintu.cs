using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
public class LevelParsePintu : LevelParseBase
{
    static private LevelParsePintu _main = null;
    public static LevelParsePintu main
    {
        get
        {
            if (_main == null)
            {
                _main = new LevelParsePintu();
            }
            return _main;
        }
    }




    public override int GetGuankaTotal()
    {
        ParseGuanka();
        if (listGuanka != null)
        {
            return listGuanka.Count;
        }
        return 0;
    }

    public override void CleanGuankaList()
    {
        if (listGuanka != null)
        {
            listGuanka.Clear();
        }
    }


    public override ItemInfo GetGuankaItemInfo(int idx)
    { 
        if (listGuanka == null)
        {
            return null;
        }
        if (idx >= listGuanka.Count)
        {
            return null;
        }
        ItemInfo info = listGuanka[idx] as ItemInfo;
        return info;
    }

    public ItemInfo GetItemInfo()
    {
        int idx = LevelManager.main.gameLevel; 
        return GetGuankaItemInfo(idx);
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
        ItemInfo infoPlace = LevelManager.main.GetPlaceItemInfo(idx);
        string fileName = CloudRes.main.rootPathGameRes + "/guanka/item_" + infoPlace.id + ".json";
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(fileName);//((TextAsset)Resources.Load(fileName, typeof(TextAsset))).text;
        // Debug.Log("json::"+json);
        JsonData root = JsonMapper.ToObject(json);
        string strPlace = infoPlace.id;
        JsonData items = root["items"];
        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];
            ItemInfo info = new ItemInfo();
            info.id = (string)item["id"];
            string str = "aa";// = languageGame.GetString(info.id);
            //Debug.Log(i + ":ParseGame:" + str);
            string picdir = CloudRes.main.rootPathGameRes + "/image/" + strPlace;
            info.pic = picdir + "/" + info.id + ".png";
            info.source = "png";
            if (!FileUtil.FileIsExistAsset(info.pic))
            {
                info.source = "jpg";
                info.pic = picdir + "/" + info.id + ".jpg";
            }

            string icondir = CloudRes.main.rootPathGameRes + "/image_thumb/" + strPlace;
            info.icon = icondir + "/" + info.id + ".png";
            if (!FileUtil.FileIsExistAsset(info.icon))
            {
                info.icon = icondir + "/" + info.id + ".jpg";
            }
            //Debug.Log("icon="+info.icon);
            string key = "is_alpha";
            if (Common.JsonDataContainsKey(item, key))
            {
                bool isalpha = (bool)item["is_alpha"];
                if (isalpha)
                {
                    info.source = "png";
                }
                else
                {
                    info.source = "jpg";
                }
            }

            listGuanka.Add(info);
        }

        count = listGuanka.Count;

        Debug.Log("ParseGame::count=" + count);
        return count;
    }


}
