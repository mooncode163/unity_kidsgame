using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;


public class LevelParseIdiomConnect : GameLevelParseBase
{
    static private LevelParseIdiomConnect _main = null;
    public static LevelParseIdiomConnect main
    {
        get
        {
            if (_main == null)
            {
                _main = new LevelParseIdiomConnect();
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

        string filepath = CloudRes.main.rootPathGameRes + "/guanka/item_" + infoPlace.id + ".json";
        //string filepath = CloudRes.main.rootPathGameRes + "/guanka/first.json";
        //
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(filepath);
        JsonData root = JsonMapper.ToObject(json);
        count = root.Count;
        Debug.Log("ParseGuankaIdiomConnect count=" + count);
        string strPlace = infoPlace.id;
        //JsonData items = root["items"];
        for (int i = 0; i < count; i++)
        {
            JsonData item = root[(i + 1).ToString()];
            CaiCaiLeItemInfo info = new CaiCaiLeItemInfo();
            JsonData word = item["word"];
            info.listWord = new List<string>();
            for (int j = 0; j < word.Count; j++)
            {
                string strword = (string)word[j];
                info.listWord.Add(strword);
            }

            info.listIdiom = new List<string>();
            JsonData idiom = item["idiom"];
            for (int j = 0; j < idiom.Count; j++)
            {
                string strword = (string)idiom[j];
                info.listIdiom.Add(strword);
            }

            info.listPosX = new List<int>();
            JsonData posx = item["posx"];
            for (int j = 0; j < posx.Count; j++)
            {
                int v = (int)posx[j];
                info.listPosX.Add(v);
            }

            info.listPosY = new List<int>();
            JsonData posy = item["posy"];
            for (int j = 0; j < posy.Count; j++)
            {
                int v = (int)posy[j];
                info.listPosY.Add(v);
            }

            info.listWordAnswer = new List<int>();
            JsonData answer = item["answer"];
            for (int j = 0; j < answer.Count; j++)
            {
                int v = (int)answer[j];
                info.listWordAnswer.Add(v);
            }


            info.id = ((int)item["id"]).ToString();
            info.gameType = GameRes.GAME_TYPE_CONNECT;
            listGuanka.Add(info);
        }

        return count;
    }

    public override void ParseItem(CaiCaiLeItemInfo info)
    {
        DBItemInfoBase dbInfo = null;
        // string title = info.title;
        // if (!Common.BlankString(title))
        // {
        //     dbInfo = DBIdiom.main.GetItemByTitle(title);
        // }
        // else
        // {
        //     dbInfo = DBIdiom.main.GetItemById(info.id);
        // }
        // Debug.Log("ParseItem title=" + title);
        // IdiomItemInfo idiom = dbInfo as IdiomItemInfo;
        // info.title = idiom.title;
        // info.album = idiom.album;
        // info.translation = idiom.translation;
        // info.pinyin = idiom.pronunciation;
        // info.dbInfo = dbInfo;

    }


}
