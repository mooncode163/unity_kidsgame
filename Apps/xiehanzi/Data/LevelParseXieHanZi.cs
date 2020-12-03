using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
public class LevelParseXieHanZi : LevelParseBase
{
    public const int ADVIDEO_LEVEL = 12;
    static private LevelParseXieHanZi _main = null;
    public static LevelParseXieHanZi main
    {
        get
        {
            if (_main == null)
            {
                _main = new LevelParseXieHanZi();
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
        long tickJson = Common.GetCurrentTimeMs();
        string fileName = Common.GAME_RES_DIR + "/guanka/guanka_list_place" + idx + ".json";
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(fileName);//((TextAsset)Resources.Load(fileName, typeof(TextAsset))).text;
        // Debug.Log("json::"+json);
        JsonData root = JsonMapper.ToObject(json);
        JsonData items = root["items"];
        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];
            WordItemInfo info = new WordItemInfo();
            string word = (string)item["title"];
            info.id = word;
                        info.wordCode = TextUtil.GetUnicode(word);
            info.wordCode = info.wordCode.Replace("\\u", "");
            // 一 \u4e00
            info.dbInfo.id = word;//TextUtil.GetUnicode(word);
            info.dbInfo.title = word;
            info.dbInfo.imagetitle = (string)item["detail"];
            info.url = "https://hanyu.baidu.com/s?wd=" + word + "&ptype=zici";
            string strDirRoot = Common.GAME_RES_DIR + "/image_detail/";
            info.pic = strDirRoot + (i + 1).ToString() + ".jpg";
            strDirRoot = Common.GAME_RES_DIR + "/image/";
            // info.icon = strDirRoot + info.id + "/" + info.id + ".png";
            info.icon = GameLevelParse.main.GetItemThumb(info.id);
            if (FileUtil.FileIsExist(info.icon))
            {
                info.pic = info.icon;
            }
            info.index = i;
             info.imageBihua = CloudRes.main.rootPathGameRes + "/image_bihua_show/" + info.wordCode + "/" + info.wordCode + "_LineShow.png";

            info.isAd = false;
            if(i>ADVIDEO_LEVEL)
            {
                info.isAd = true;
            }
            //info.pic = info.icon;
            // if(FileUtil.FileIsExistAsset(info.icon))
            {
                listGuanka.Add(info);
            }
            // else{
            //     Debug.Log("tickJson word not exit ="+word);
            // }
        }
        count = listGuanka.Count;
        tickJson = Common.GetCurrentTimeMs() - tickJson;
        Debug.Log("tickJson =" + tickJson + " count=" + count);

        return count;
    }
    public int ParseGuanka2()
    {
        int count = 0;
        Debug.Log("ParseGuanka2 start");
        long tickGuanka = Common.GetCurrentTimeMs();
        if ((listGuanka != null) && (listGuanka.Count != 0))
        {
            return listGuanka.Count;
        }
        listGuanka = new List<object>();
        int idx = LevelManager.main.placeLevel;
        long tickJson = Common.GetCurrentTimeMs();
        string fileName = Common.GAME_RES_DIR + "/guanka/guanka_list_place" + idx + ".json";
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(fileName);//((TextAsset)Resources.Load(fileName, typeof(TextAsset))).text;
        // Debug.Log("json::"+json);
        JsonData root = JsonMapper.ToObject(json);
        JsonData items = root["items"];
        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];
            WordItemInfo info = new WordItemInfo();
            info.id = (string)item["id"];
            string word = (string)item["title"];
            info.id = word;


            info.icon = GameLevelParse.main.GetItemThumb(info.id);
            DBWordItemInfo dbInfo = new DBWordItemInfo();
            dbInfo.id = info.id;
            info.dbInfo = dbInfo;
            listGuanka.Add(info);
        }
        tickJson = Common.GetCurrentTimeMs() - tickJson;
        count = listGuanka.Count;

        WordItemInfo infonow = GameLevelParse.main.GetItemInfo();
        long tickItem = Common.GetCurrentTimeMs();
        //ParserGuankaItem2(infonow);
        tickItem = Common.GetCurrentTimeMs() - tickItem;

        tickGuanka = Common.GetCurrentTimeMs() - tickGuanka;
        Debug.Log("ParseGuanka2: tickGuanka=" + tickGuanka + " tickJson=" + tickJson + " tickItem=" + tickItem);
        return count;
    }

    public override void ParseItem(ItemInfo info)
    {
        WordItemInfo infoWord = info as WordItemInfo;

        if (infoWord.dbInfo != null)
        {

            if (!Common.isBlankString(infoWord.dbInfo.bushou))
            {
                return;
            }

        }
        Debug.Log("UpdateItem infoWord.id=" + infoWord.id);
        string str = infoWord.dbInfo.imagetitle;
        infoWord.dbInfo = DBWord.main.GetItemById(infoWord.id);
        infoWord.dbInfo.imagetitle = str;


        if (infoWord.pointInfo == null)
        {
            infoWord.pointInfo = ParseWordPointInfo.main.ParseWordJson(infoWord.wordCode);
        }

        if (infoWord.listBiHuaName == null)
        {
            infoWord.listBiHuaName = new List<string>();
            foreach (string strtmp in infoWord.dbInfo.bihuaName.Split(' '))
            {
                if (!Common.BlankString(strtmp))
                {
                    infoWord.listBiHuaName.Add(strtmp);
                }
            }
        }
        if (infoWord.listBiHuaOrder == null)
        {
            infoWord.listBiHuaOrder = new List<string>();
            foreach (string strtmp in infoWord.dbInfo.bihuaOrder.Split(' '))
            {
                if (!Common.BlankString(strtmp))
                {
                    infoWord.listBiHuaOrder.Add(strtmp);
                }
            }
        }
        if (infoWord.listZuci == null)
        {
            infoWord.listZuci = new List<string>();
            foreach (string strtmp in infoWord.dbInfo.zuci.Split(';'))
            {
                if (!Common.BlankString(strtmp))
                {
                    infoWord.listZuci.Add(strtmp);
                }
            }
        }
    }
   
}
