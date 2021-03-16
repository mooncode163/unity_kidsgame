using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class LevelParseWord : LevelParseBase
{
    static private LevelParseWord _main = null;
    public static LevelParseWord main
    {
        get
        {
            if (_main == null)
            {
                _main = new LevelParseWord();
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
        long tickJson = Common.GetCurrentTimeMs();
        string fileName = CloudRes.main.rootPathGameRes + "/guanka/guanka_list_place" + idx + ".json";
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(fileName);//((TextAsset)Resources.Load(fileName, typeof(TextAsset))).text;
                                                         // Debug.Log("json::"+json);
        long tickJson1 = Common.GetCurrentTimeMs();
        JsonData root = JsonMapper.ToObject(json);
        tickJson1 = Common.GetCurrentTimeMs() - tickJson1;
        JsonData items = root["items"];
        string rootPath =CloudRes.main.rootPathGameRes;
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
            string strDirRoot = rootPath + "/image_detail/";
            info.pic = strDirRoot + (i + 1).ToString() + ".jpg";

            strDirRoot = rootPath + "/image/";
            info.icon = strDirRoot + info.id + "/" + info.id + ".png";
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
        Debug.Log("tickJson =" + tickJson + " tickJson1=" + tickJson1 + " count=" + count);

        return count;
    }
    public override void ParseItem(ItemInfo info)
    {
        WordItemInfo infoWord = info as WordItemInfo;

        if (infoWord.dbInfo != null)
        {

            if (!Common.isBlankString(infoWord.dbInfo.pinyin))
            {
                return;
            }

        }

        string str = infoWord.dbInfo.imagetitle;
        // infoWord.dbInfo = new DBWordItemInfo();
        // infoWord.dbInfo.bihuaCount = "1";
        infoWord.dbInfo = DBWord.main.GetItemById(infoWord.id);
        infoWord.dbInfo.imagetitle = str;
        Debug.Log("ParseItem dbInfo.bihuaCount=" + infoWord.dbInfo.bihuaCount);

        if (infoWord.pointInfo == null)
        {
            infoWord.pointInfo = ParseWordPointInfo.main.ParseWordJson(infoWord.wordCode);
        }
    }


}
