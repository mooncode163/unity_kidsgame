using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
public class LevelParseBase
{
    public const int GUANKA_ITEM_NUM_ONE_GROUP = 5;
    public const string PLACE_ITEM_TYPE_NONE = "none";
    public const string PLACE_ITEM_TYPE_VIDEO = "video";
    public const string PLACE_ITEM_TYPE_LOCK = "lock";

     public List<object> listGuanka;
    private List<object> _listPlace = null;

    public List<object> listGuankaItemId;//image id
    public List<object> listPlace
    {
        get
        {
            int ret = 0;
            StartParsePlaceList();
            return _listPlace;
        }
    }

    public ItemInfo GetPlaceItemInfo(int idx)
    {
        int index = 0;
        foreach (ItemInfo info in listPlace)
        {
            //  if (info.tag == PlaceScene.PLACE_ITEM_TYPE_GAME)
            {
                if (index == idx)
                {
                    return info;
                }
                index++;
            }
        }

        return null;
    }

    public void StartParsePlaceList()
    {

        if (_listPlace == null)
        {
            _listPlace = new List<object>();
        }
        if (_listPlace.Count > 0)
        {
            //已经解析完成
            return;
        }
        string filepath = CloudRes.main.rootPathGameRes +"/place/place_list.json";
        byte[] data = FileUtil.ReadDataAuto(filepath);
        ParsePlaceList(data);
    }

    void ParsePlaceList(byte[] data)
    {
        if (data == null)
        {
            return;
        }
        if ((_listPlace != null) && (_listPlace.Count != 0))
        {
            return;
        }

        string json = Encoding.UTF8.GetString(data);
        // Debug.Log("json::"+json);
        JsonData root = JsonMapper.ToObject(json);
        JsonData items = null;
        string key = "places";
        if (Common.JsonDataContainsKey(root, key))
        {
            items = root[key];
        }
        else
        {
            items = root["items"];
        }

        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];
            ItemInfo info = new ItemInfo();
            info.id = JsonUtil.GetString(item, "id", "");
            string filepath = CloudRes.main.rootPathGameRes +"/place/image/" + info.id;
            info.pic = filepath + ".png";
            if (!FileUtil.FileIsExistAsset(info.pic))
            {
                info.pic = filepath + ".jpg";
            }
            info.gameType = JsonUtil.GetString(item, "game", JsonUtil.GetString(item, "game_type", ""));
            info.gameId = JsonUtil.GetString(item, "gameId", "");
            info.type = JsonUtil.GetString(item, "type", PLACE_ITEM_TYPE_NONE);
            info.title = JsonUtil.GetString(item, "title", "STR_PLACE_" + info.id);
            info.icon = info.pic;
            info.language = JsonUtil.GetString(item, "language", "language");
            // info.tag = PlaceScene.PLACE_ITEM_TYPE_GAME;
            info.index = i;

            GameManager.main.pathGamePrefab = JsonUtil.GetString(item, "prefab", "");

            info.isAd = false;
            if (AppVersion.appCheckHasFinished && (!Common.noad))
            {
                if (info.type == PLACE_ITEM_TYPE_VIDEO)
                {
                    info.isAd = true;
                }
                // if (Common.isAndroid)
                {
                    if (info.type == PLACE_ITEM_TYPE_LOCK)
                    {
                        info.isAd = true;
                    }
                }
            }

            _listPlace.Add(info);
        }

        Debug.Log("ParsePlaceList count =" + _listPlace.Count);
    }



    //guanka
    public virtual int GetGuankaTotal()
    {
        int count = 0;
        return count;
    }
    public virtual void CleanGuankaList()
    {
        if (listGuanka != null)
        {
            listGuanka.Clear();
        }
    }
    public virtual int ParseGuanka()
    {
        Debug.Log("ParseGuanka UIGameBase");
        return 0;
    }

    public virtual ItemInfo GetGuankaItemInfo(int idx)
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

    public void ParseGuankaItemId(int count_one_group)
    {
        listGuankaItemId = new List<object>();

        // listGuanka = new List<object>();
        ItemInfo infoPlace = GetPlaceItemInfo(LevelManager.main.placeLevel);
        string fileName = CloudRes.main.rootPathGameRes +"/guanka/item_" + infoPlace.id + ".json";
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(fileName); //((TextAsset)Resources.Load(fileName, typeof(TextAsset))).text;
        // Debug.Log("json::"+json);
        JsonData root = JsonMapper.ToObject(json);
        string type = (string)root["type"];
        string picRoot = CloudRes.main.rootPathGameRes +"/image/" + type + "/";

        //search_items
        JsonData items = root["items"];
        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];
            ItemInfo info = new ItemInfo();
            info.id = (string)item["id"];
            info.pic = picRoot + info.id + ".png";
            listGuankaItemId.Add(info);
        }


        //让总数是count_one_group的整数倍
        int tmp = (listGuankaItemId.Count % count_one_group);
        if (tmp > 0)
        {
            for (int i = 0; i < (count_one_group - tmp); i++)
            {
                ItemInfo infoId = listGuankaItemId[i] as ItemInfo;
                ItemInfo info = new ItemInfo();
                info.id = infoId.id;
                info.pic = infoId.pic;
                listGuankaItemId.Add(info);
            }
        }

    }

    public virtual void ParseItem(ItemInfo info)
    { 
    }
}
