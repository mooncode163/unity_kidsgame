using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class ColorItemInfo : ItemInfo
{
    public List<Color> listColor;
    public string name;
    public string picmask;
    public string colorJson;
    public Vector2 pt;
    public Color colorOrigin;//填充前原来颜色
    public Color colorFill;//当前填充颜色
    public Color colorMask;
    public Color32 color32Fill;
    public string fileSave;
    public Rect rectFill;
    public DBItemInfo infoDB;
}

public class GameLevelParse : LevelParseBase
{
    static private GameLevelParse _main = null;
    public static GameLevelParse main
    {
        get
        {
            if (_main == null)
            {
                _main = new GameLevelParse();
            }
            return _main;
        }
    }




    public ColorItemInfo GetItemInfo()
    {
        int idx = LevelManager.main.gameLevel;
        if (listGuanka == null)
        {
            return null;
        }
        if (idx >= listGuanka.Count)
        {
            return null;
        }
        ColorItemInfo info = listGuanka[idx] as ColorItemInfo;
        return info;
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

    public string GetFileSave(ColorItemInfo info)
    {
        string filedir = DBColor.strSaveColorDir;
        //创建文件夹
        if (!Directory.Exists(filedir))
        {
            Directory.CreateDirectory(filedir);
        }
        string strid = info.id;
        if (UIGamePaint.gameMode == UIGamePaint.GAME_MODE_FREE_DRAW)
        {
            strid = "id_freedraw_" + Common.GetCurrentTimeMs();
            info.id = strid;
        }
        string filepath = filedir + "/" + strid + ".png";
        return filepath;
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
            ColorItemInfo info = new ColorItemInfo();
            info.infoDB = new DBItemInfo();

            string strdir = CloudRes.main.rootPathGameRes + "/image/" + strPlace;

            info.id = (string)item["id"];
            info.pic = strdir + "/draw/" + info.id + ".png";

            info.picmask = strdir + "/mask/" + info.id + ".png";
            info.colorJson = strdir + "/json/" + info.id + ".json";
            info.icon = strdir + "/thumb/" + info.id + ".png";

            //info.pic = info.picmask;

            string filepath = GetFileSave(info);
            info.infoDB.filesave = filepath;

            // string picname = (i + 1).ToString("d3");
            // info.pic = CloudRes.main.rootPathGameRes + "/animal/draw/" + picname + ".png";
            // info.picmask = CloudRes.main.rootPathGameRes + "/animal/mask/" + picname + ".png";
            // info.colorJson = CloudRes.main.rootPathGameRes + "/animal/draw/" + picname + ".json";
            // info.icon = CloudRes.main.rootPathGameRes + "/animal/thumb/" + picname + ".png";

            listGuanka.Add(info);
        }

        count = listGuanka.Count;

        // Debug.Log("ParseGame::count=" + count);
        return count;
    }



}
