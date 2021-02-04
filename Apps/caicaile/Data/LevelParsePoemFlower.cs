using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;


public class LevelParsePoemFlower : GameLevelParseBase
{
    public List<object> listPosition;

    static private LevelParsePoemFlower _main = null;
    public static LevelParsePoemFlower main
    {
        get
        {
            if (_main == null)
            {
                _main = new LevelParsePoemFlower();

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
        Debug.Log("ParseGuanka filepath=" + filepath);
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
            info.content0 = JsonUtil.GetString(item, "content0", "");
            info.content1 = JsonUtil.GetString(item, "content1", "");
            info.listIdiom = new List<string>();
             info.listIdiom.Add(info.content0); 
            info.listIdiom.Add(info.content1); 
            info.gameType = infoPlace.gameType;

            listGuanka.Add(info);
        }

        count = listGuanka.Count;


        Debug.Log("ParseGame::count=" + count);
        return count;
    }
 

     public override void ParseItem(CaiCaiLeItemInfo info)
    {
        string filepath = CloudRes.main.rootPathGameRes + "/guanka/poem/" + info.id + ".json";
        Debug.Log("ParsePoemItem filepath=" + filepath);
        if (!FileUtil.FileIsExistAsset(filepath))
        {
            Debug.Log("ParsePoemItem not exist filepath=" + filepath);
            return;
        }
        //
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(filepath);
        JsonData root = JsonMapper.ToObject(json);
        info.title = (string)root["title"];
        info.author = (string)root["author"];
        info.year = (string)root["year"];
        info.style = (string)root["style"];
        info.album = (string)root["album"];
        info.url = (string)root["url"];
        info.intro = (string)root["intro"];
        info.translation = (string)root["translation"];
        info.appreciation = (string)root["appreciation"];

        JsonData itemPoem = root["poem"];
        info.listPoemContent = new List<PoemContentInfo>();
        for (int i = 0; i < itemPoem.Count; i++)
        {
            JsonData item = itemPoem[i];
            PoemContentInfo infoPoem = new PoemContentInfo();
            infoPoem.content = (string)item["content"];
            infoPoem.pinyin = (string)item["pinyin"];
            bool isSkip = JsonUtil.GetBool(item, "skip", false);
            if (!isSkip)
            {
                info.listPoemContent.Add(infoPoem);
            }
        }
    }

    public override void ParseItemDetal(CaiCaiLeItemInfo info)
    {


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

    public void ParsePosition(int row, int col, int wordnum)
    {
        if (listPosition == null)
        {
            listPosition = new List<object>();
        }
        else
        {
            listPosition.Clear();
        }
        Debug.Log("ParsePosition ");
        {
            string filepath = CloudRes.main.rootPathGameRes + "/guanka/data/item_position_" + row + "x" + col + "_" + wordnum + ".json";
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
