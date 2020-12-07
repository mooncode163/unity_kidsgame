using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
public class GameLevelParse : LevelParseBase
{
    public const int ADVIDEO_LEVEL_MIN =10;
    LevelParseBase levelParse;

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
    public string strSaveWordShotDir//字截图保存目录
    {
        get
        {
            return Application.temporaryCachePath + "/word";
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

    public WordItemInfo GetItemInfo()
    {
        int idx = LevelManager.main.gameLevel;
        return GetGuankaItemInfo(idx) as WordItemInfo;
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
    public string GetItemThumb(string id)
    {
        string strDirRoot = Common.GAME_RES_DIR;
        string strDirRootImage = strDirRoot + "/image/" + id;
        //thumb
        string ret = strDirRootImage + "/" + id + "_thumb.png";

        return ret;
    }
    public override int ParseGuanka()
    {
        int count = 0;
        ItemInfo infoPlace = LevelManager.main.GetPlaceItemInfo(LevelManager.main.placeLevel);
      
        if (infoPlace.id == GameRes.GAME_ID_LearnWord)
        {
            levelParse = LevelParseLearnWord.main;
        }

        if (levelParse != null)
        {
            levelParse.ParseGuanka();
            listGuanka = levelParse.listGuanka;
        }
        count = listGuanka.Count;
        return count;
    }

    public override void ParseItem(ItemInfo info)
    {
        if (levelParse != null)
        {
            levelParse.ParseItem(info);
        }
    }

      public string GetSavePath(WordItemInfo info)
    {
        string filedir = strSaveWordShotDir;

        //创建文件夹
        Directory.CreateDirectory(filedir);
        string filepath = filedir + "/" + info.dbInfo.id + ".png";
        // info.fileSaveWord = filepath;
        info.dbInfo.filesave = filepath;

        return filepath;
    }

     public int GetGuankaIndexByWord(WordItemInfo info)
    {
        int idx = 0;
        foreach (WordItemInfo infotmp in GameLevelParse.main.listGuanka)
        {
            if (infotmp.id == info.id)
            {
                break;
            }
            idx++;
        }
        return idx;
    }
}
