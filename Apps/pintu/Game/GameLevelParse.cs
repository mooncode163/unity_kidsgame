using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
public class GameLevelParse : LevelParseBase
{
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
        ItemInfo infoPlace = LevelManager.main.GetPlaceItemInfo(LevelManager.main.placeLevel);
        Debug.Log("ParseGuanka infoPlace.gameType=" + infoPlace.gameType + " infoPlace.id=" + infoPlace.id);
        if (Common.BlankString(infoPlace.gameId))
        {

        }


        switch (infoPlace.gameId)
        {
            
            case GameRes.GAME_Word:
                {
                    levelParse = LevelParseWord.main;
                }
                break;
            default:
                {
                    levelParse = LevelParsePintu.main;  
                }
                break;
        }

        if (levelParse != null)
        {
            count = levelParse.ParseGuanka();
            listGuanka = levelParse.listGuanka;
        }

        return count;
    }
    
 
    public override void ParseItem(ItemInfo info)
    {
        if (levelParse != null)
        {
            levelParse.ParseItem(info);
        }
    }
}
