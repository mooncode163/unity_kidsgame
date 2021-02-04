using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;


public class LevelParsePlacePoem : GameLevelParseBase
{
    public List<object> listPosition;

    static private LevelParsePlacePoem _main = null;
    public static LevelParsePlacePoem main
    {
        get
        {
            if (_main == null)
            {
                _main = new LevelParsePlacePoem();

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
        string filepath = CloudRes.main.rootPathGameRes + "/guanka/" + infoPlace.id + ".json";
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
            info.title = JsonUtil.GetString(item, "title", "");
            info.id = info.title;
            JsonData arrayContent = item["content"];
            info.listPoemContent = new List<PoemContentInfo>();
            if (arrayContent.Count > 8)
            {
                continue;
            }
            for (int j = 0; j < arrayContent.Count; j++)
            {
                PoemContentInfo infoPoem = new PoemContentInfo();
                infoPoem.content = (string)arrayContent[j];
                info.listPoemContent.Add(infoPoem);
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
      
        Debug.Log("ParseItem  info.title=" + info.title + " info.id=" + info.id);
        if(info.dbInfo==null)
        { 
            DBItemInfoBase dbInfo = null;
            string title = info.title;
            if (!Common.BlankString(title))
            {
                dbInfo = DBPoem.main.GetItemByTitle(title);
            }
            else
            {
                dbInfo = DBPoem.main.GetItemByTitle(info.id);
            }
            info.dbInfo = dbInfo;
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
