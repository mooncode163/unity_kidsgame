using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
public class LevelParseXieHanZi : LevelParseBase
{
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
        Debug.Log("ParseGuanka2 start");
        long tickGuanka = Common.GetCurrentTimeMs();
        if ((listGuanka != null) && (listGuanka.Count != 0))
        {
            return listGuanka.Count;
        }
        listGuanka = new List<object>();
        int idx = LevelManager.main.placeLevel;
        long tickJson = Common.GetCurrentTimeMs();
        string fileName = CloudRes.main.rootPathGameRes +"/guanka/guanka_list_place" + idx + ".json";
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
            info.icon = GameLevelParse.main.GetItemThumb(info.id);

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

    }

}
