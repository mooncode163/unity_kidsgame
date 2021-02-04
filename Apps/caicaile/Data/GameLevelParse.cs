using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class PoemContentInfo
{
    public string content;
    public string pinyin;
    public bool skip;
}

public class AnswerInfo
{
    public int index;
    public bool isFinish;//是否答对
    public string word;//答案
    public bool isFillWord;//是否填了字
    public string wordFill;//实际填充的字
    public int row;
    public int col;
}

public class CaiCaiLeItemInfo : ItemInfo
{
    public string author;
    public string year;
    public string style;
    public string album;
    public string intro;
    public string translation;
    public string appreciation;
    public string pinyin;
    public string head;
    public string end;
    public string tips;


    public List<PoemContentInfo> listPoemContent;


    //idiomconnet


    public List<string> listWord;
    public List<string> listIdiom;
    public List<int> listPosX;
    public List<int> listPosY;
    public List<int> listWordAnswer;

    public string date;
    public string addtime;

    //PoemFlower
    public string content0;
    public string content1;

    //
    public bool isLock;

    public DBItemInfoBase dbInfo;
}
public class GameLevelParse : LevelParseBase
{

    public string strWordEnglish = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public string strWord3500;
    public string[] arrayPunctuation = { "。", "？", "！", "，", "、", "；", "：" };

    GameLevelParseBase levelParse;

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

    public CaiCaiLeItemInfo GetItemInfo()
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
        CaiCaiLeItemInfo info = listGuanka[idx] as CaiCaiLeItemInfo;
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
    public void ParseWord3500()
    {
        //word3500
        string filepath = Common.GAME_DATA_DIR + "/words_3500.json";
        string json = FileUtil.ReadStringAsset(filepath);
        JsonData root = JsonMapper.ToObject(json);
        strWord3500 = (string)root["words"];
        // Debug.Log(strWord3500);
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
            case GameRes.GAME_IDIOM:
                {
                    levelParse = LevelParseIdiom.main;
                }
                break;
            case GameRes.GAME_RIDDLE:
                {
                    levelParse = LevelParseRiddle.main;
                }
                break;
            case GameRes.GAME_XIEHOUYU:
                {
                    levelParse = LevelParseXiehouyu.main;
                }
                break;
            case GameRes.GAME_Image:
                {
                    levelParse = LevelParseImage.main;
                }
                break;

            case GameRes.GAME_IdiomConnect:
                {
                    levelParse = LevelParseIdiomConnect.main;
                }
                break;
            case GameRes.GAME_IdiomFlower:
                {
                    levelParse = LevelParseIdiomFlower.main;
                }
                break;
            case GameRes.GAME_PoemFlower:
                {
                    levelParse = LevelParsePoemFlower.main;
                }
                break;

            case GameRes.GAME_PlacePoem:
                {
                    levelParse = LevelParsePlacePoem.main;
                }
                break;
            case GameRes.GAME_Music:
                {
                    Debug.Log("LevelParseMusic GAME_Music=");
                    levelParse = LevelParseMusic.main;
                }
                break;

            case GameRes.GAME_POEM:
                {
                    levelParse = LevelParsePoem.main;
                }
                break;

            default:
                {
                    Debug.Log("LevelParseMusic default=");
                    levelParse = LevelParseDefault.main;
                    if (Common.appKeyName == GameRes.GAME_fruit)
                    {
                        infoPlace.gameId = GameRes.GAME_fruit;
                    }
                    if (Common.appKeyName == GameRes.GAME_zonghe)
                    {
                        infoPlace.gameId = GameRes.GAME_zonghe;
                    }
                    if (Common.appKeyName == GameRes.GAME_animal)
                    {
                        infoPlace.gameId = GameRes.GAME_animal;
                    }
                }
                break;
        }

        if (levelParse != null)
        {
            count = levelParse.ParseGuanka();
            listGuanka = levelParse.listGuanka;
        }

        ParseWord3500();
        return count;
    }



    //过滤标点符号 点号：句号（ 。）、问号（ ？）、感叹号（ ！）、逗号（ ，）顿号（、）、分号（；）和冒号（：）。
    public string FilterPunctuation(string str)
    {
        string ret = str;

        foreach (string item in arrayPunctuation)
        {
            ret = ret.Replace(item, "");
        }
        return ret;
    }

    public bool IsPunctuation(string str)
    {
        bool ret = false;

        foreach (string item in arrayPunctuation)
        {
            if (str == item)
            {
                ret = true;
                break;
            }
        }
        return ret;
    }

    //非标点符号文字
    public List<int> IndexListNotPunctuation(string str)
    {
        List<int> listRet = new List<int>();

        int len = str.Length;
        for (int i = 0; i < len; i++)
        {
            string word = str.Substring(i, 1);
            if (!IsPunctuation(word))
            {
                listRet.Add(i);
            }

        }
        return listRet;
    }

    public void ParseItem(CaiCaiLeItemInfo info)
    {

        if (Common.appKeyName == GameRes.GAME_POEM)
        {
            ParsePoemItem(info);
            return;
        }

        if (levelParse != null)
        {
            levelParse.ParseItem(info);
        }

        // if ((Common.appKeyName == GameRes.GAME_IDIOM) || (Common.appKeyName == GameRes.GAME_IdiomConnect))
        // {
        //     ParseIdiomItem(info);
        // }

        // if (Common.appKeyName == GameRes.GAME_RIDDLE)
        // {
        //     ParseRiddleItem(info);
        // }
    }
    public void ParseItemDetal(CaiCaiLeItemInfo info)
    {

        if (levelParse != null)
        {
            levelParse.ParseItemDetal(info);
        }

    }

    //诗词
    public void ParsePoemItem(CaiCaiLeItemInfo info)
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

    public List<PoemContentInfo> ParsePoemContent(string id)
    {
        string filepath = CloudRes.main.rootPathGameRes + "/guanka/poem/" + id + ".json";
        Debug.Log("ParsePoemItem filepath=" + filepath);
        if (!FileUtil.FileIsExistAsset(filepath))
        {
            Debug.Log("ParsePoemItem not exist filepath=" + filepath);
            return null;
        }
        //
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(filepath);
        JsonData root = JsonMapper.ToObject(json);

        JsonData itemPoem = root["poem"];
        List<PoemContentInfo> list = new List<PoemContentInfo>();
        for (int i = 0; i < itemPoem.Count; i++)
        {
            JsonData item = itemPoem[i];
            PoemContentInfo infoPoem = new PoemContentInfo();
            infoPoem.content = (string)item["content"];
            infoPoem.pinyin = (string)item["pinyin"];
            bool isSkip = JsonUtil.GetBool(item, "skip", false);
            if (!isSkip)
            {
                list.Add(infoPoem);
            }
        }
        return list;
    }

    //谜语
    public void ParseRiddleItem(CaiCaiLeItemInfo info)
    {
    }
}
