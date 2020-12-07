using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
public class LevelParseLearnWord : LevelParseBase
{
    static private LevelParseLearnWord _main = null;
    public static LevelParseLearnWord main
    {
        get
        {
            if (_main == null)
            {
                _main = new LevelParseLearnWord();
            }
            return _main;
        }
    }

    public string GetWordImage(string id)
    {
        string strDirRoot = CloudRes.main.rootPathGameRes + "/image";
        string ret = strDirRoot + "/" + id + ".png";
        return ret;
    }

    public List<WordItemInfo> GetOtherWord(WordItemInfo info, int count)
    {
        int[] intOther = new int[listGuanka.Count - 1];
        int idx = 0;
        for (int i = 0; i < listGuanka.Count; i++)
        {
            WordItemInfo infotmp = listGuanka[i] as WordItemInfo;
            if (infotmp.index != info.index)
            {
                intOther[idx++] = i;
            }
        }
        int[] inttmp = Common.RandomIndex(intOther.Length, count);
        List<WordItemInfo> listRet = new List<WordItemInfo>();
        foreach (int v in inttmp)
        {
            listRet.Add(listGuanka[v] as WordItemInfo);
        }
        return listRet;
    }
    public void ConvertChinesWordFileToUnicode()
    {
        ParseGuanka();
        foreach (WordItemInfo info in listGuanka)
        {
            {
                string filepath = CloudRes.main.rootPathGameRes + "/guanka/word/" + info.title + ".json";
                string filepathnew = CloudRes.main.rootPathGameRes + "/guanka/word/" + info.wordCode + ".json";
                // Debug.Log("ConvertChinesWordFileToUnicode filepath"+filepath+" filepathnew="+filepathnew);
                if (FileUtil.FileIsExist(filepath))
                {
                    File.Move(filepath, filepathnew);
                }
            }
            {
                string filepath = CloudRes.main.rootPathGameRes + "/guanka/word_median/" + info.title + ".json";
                string filepathnew = CloudRes.main.rootPathGameRes + "/guanka/word_median/" + info.wordCode + ".json";
                // Debug.Log("ConvertChinesWordFileToUnicode 2 filepath"+filepath+" filepathnew="+filepathnew);
                if (FileUtil.FileIsExist(filepath))
                {
                    File.Move(filepath, filepathnew);
                }
            }
            {
                //目录
                string filepath = CloudRes.main.rootPathGameRes + "/image_bihua_show/" + info.title;
                string filepathnew = CloudRes.main.rootPathGameRes + "/image_bihua_show/" + info.wordCode;
                Debug.Log("ConvertChinesWordFileToUnicode 3 filepath" + filepath + " filepathnew=" + filepathnew);
                if (FileUtil.DirIsExist(filepath))
                {
                    Directory.Move(filepath, filepathnew);
                }
                filepath = CloudRes.main.rootPathGameRes + "/image_bihua_show/" + info.wordCode + "/" + info.title + "_LineShow.png";
                filepathnew = CloudRes.main.rootPathGameRes + "/image_bihua_show/" + info.wordCode + "/" + info.wordCode + "_LineShow.png";

                if (FileUtil.FileIsExist(filepath))
                {
                    File.Move(filepath, filepathnew);
                }


            }


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
        string fileName = CloudRes.main.rootPathGameRes + "/guanka/guanka_list_place" + idx + ".json";
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
            info.title = word;
            info.wordCode = TextUtil.GetUnicode(word);
            info.wordCode = info.wordCode.Replace("\\u", "");
            // 一 \u4e00
            Debug.Log("wordCode =" + info.wordCode);

            info.dbInfo.id = word;//TextUtil.GetUnicode(word);
            info.dbInfo.title = word;
            info.dbInfo.imagetitle = (string)item["detail"];
            info.url = "https://hanyu.baidu.com/s?wd=" + word + "&ptype=zici";
            string strDirRoot = CloudRes.main.rootPathGameRes + "/image_detail/";
            info.pic = strDirRoot + (i + 1).ToString() + ".jpg";
            strDirRoot = CloudRes.main.rootPathGameRes + "/image/";
            info.icon = strDirRoot + info.id + "/" + info.id + ".png";
            info.index = i;
            info.imageBihua = CloudRes.main.rootPathGameRes + "/image_bihua_show/" + info.wordCode + "/" + info.wordCode + "_LineShow.png";

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
    public override void ParseItem(ItemInfo info)
    {
        Debug.Log("LevelParseLearnWord start");
        WordItemInfo infoWord = info as WordItemInfo;
        Debug.Log("LevelParseLearnWord 1");
        if (infoWord.dbInfo != null)
        {
            Debug.Log("LevelParseLearnWord 2");
            if (!Common.isBlankString(infoWord.dbInfo.bushou))
            {
                Debug.Log("LevelParseLearnWord 3");
                return;
            }

        }
        Debug.Log("LevelParseLearnWord 4");
        Debug.Log("LevelParseLearnWord UpdateItem infoWord.id=" + infoWord.id);
        string str = infoWord.dbInfo.imagetitle;
        Debug.Log("LevelParseLearnWord 5");
        infoWord.dbInfo = DBWord.main.GetItemById(infoWord.id);
        Debug.Log("LevelParseLearnWord 6");
        infoWord.dbInfo.imagetitle = str;
        Debug.Log("LevelParseLearnWord 7");

        if (infoWord.pointInfo == null)
        {
            Debug.Log("LevelParseLearnWord 8");
            infoWord.pointInfo = ParseWordPointInfo.main.ParseWordJson(infoWord.wordCode);
        }
        Debug.Log("LevelParseLearnWord 9");
        Debug.Log("LevelParseLearnWord 9 infoWord.dbInfo.id="+infoWord.dbInfo.id);

        if (infoWord.listBiHuaName == null)
        {
            Debug.Log("LevelParseLearnWord 9 1");
            infoWord.listBiHuaName = new List<string>();
            Debug.Log("LevelParseLearnWord 9 2");
            if(infoWord.dbInfo.bihuaName==null)
            {
                Debug.Log("LevelParseLearnWord 9 bihuaName==null");
            }
            foreach (string strtmp in infoWord.dbInfo.bihuaName.Split(' '))
            {
                Debug.Log("LevelParseLearnWord 9 3");
                if (!Common.BlankString(strtmp))
                {
                    Debug.Log("LevelParseLearnWord 9 4 strtmp="+strtmp);
                    infoWord.listBiHuaName.Add(strtmp);
                }
                Debug.Log("LevelParseLearnWord 9 5");
            }
            Debug.Log("LevelParseLearnWord 9 6");
        }
        Debug.Log("LevelParseLearnWord 10");
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
        Debug.Log("LevelParseLearnWord 11");
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

        Debug.Log("LevelParseLearnWord end");
    }

    public int GetLevelByWord(string word)
    {
        int ret = 0;
        for (int i = 0; i < listGuanka.Count; i++)
        {
            WordItemInfo info = listGuanka[i] as WordItemInfo;
            if (info.id == word)
            {
                ret = i;
                break;
            }

        }
        return ret;
    }
}
