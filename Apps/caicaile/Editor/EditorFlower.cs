using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using UnityEditor;
using System.Text;


public class FlowerEditorJsonItemInfo
{
    public string rowcol;
}

public class FlowerEditorItemInfo
{
    public int index;
    public int row;
    public int col;
    public int wordNum;//字数 比如 成语是4  诗词是5或7
    public int max;
}

public class PoemLevelJsonInfo
{
    public string id;
    public string content0;
    public string content1;
}

public class EditorFlower : Editor
{
    static public int rowTotal;
    static public int colTotal;
    static public int totalIdiom;
    static List<FlowerEditorJsonItemInfo> listJson = new List<FlowerEditorJsonItemInfo>();
    static public FlowerEditorItemInfo infoSeed;
    static public List<FlowerEditorItemInfo> listItemSel;//选中 

    static public List<object> listAllIdiom;


    static public List<FlowerEditorItemInfo> listPosition;

    //生成成语相邻位置的数据
    [MenuItem("CaiCaiLe/Flower/OnMakeFlowerPositionData")]
    static void OnMakeFlowerPositionData()
    {
        Debug.Log("OnMakeFlowerData start");

        listItemSel = new List<FlowerEditorItemInfo>();
        listPosition = new List<FlowerEditorItemInfo>();

        {
            FlowerEditorItemInfo info = new FlowerEditorItemInfo();
            info.max = 1000;
            info.row = 4;
            info.col = 4;
            info.wordNum = 4;
            listPosition.Add(info);
        }
        {
            FlowerEditorItemInfo info = new FlowerEditorItemInfo();
            info.max = 3;
            info.row = 2;
            info.col = 2;
            info.wordNum = 4;
            listPosition.Add(info);
        }
        {
            FlowerEditorItemInfo info = new FlowerEditorItemInfo();
            info.max = 10;
            info.row = 3;
            info.col = 3;
            info.wordNum = 5;
            listPosition.Add(info);
        }

        {
            FlowerEditorItemInfo info = new FlowerEditorItemInfo();
            info.max = 4;
            info.row = 3;
            info.col = 3;
            info.wordNum = 7;
            listPosition.Add(info);
        }

        {
            FlowerEditorItemInfo info = new FlowerEditorItemInfo();
            info.max = 300;
            info.row = 4;
            info.col = 4;
            info.wordNum = 5;
            listPosition.Add(info);
        }

        {
            FlowerEditorItemInfo info = new FlowerEditorItemInfo();
            info.max = 200;
            info.row = 4;
            info.col = 4;
            info.wordNum = 7;
            listPosition.Add(info);
        }

        foreach (FlowerEditorItemInfo info in listPosition)
        {
            int total = info.max;//1000
            int count = 0;
            rowTotal = info.row;
            colTotal = info.col;
            totalIdiom = rowTotal * colTotal / info.wordNum;
            Debug.Log("totalIdiom=" + totalIdiom);
            listItemSel.Clear();
            listJson.Clear();
            while (count < total)
            {
                bool ret = MakeFlowerData(info);
                if (ret)
                {
                    // Debug.Log("listItemSel.Count=" + listItemSel.Count);
                    AddJson();
                    count++;
                }
            }

            SaveJson(info);
        }
        Debug.Log("OnMakeFlowerData Finish");

    }


    //生成成语关卡数据：不出现重复的字
    [MenuItem("CaiCaiLe/Flower/OnMakeIdiomFlowerLevelData")]
    static void OnMakeIdiomFlowerLevelData()
    {
        GetAllIdiom();
        LevelParseIdiomFlower.main.ParseCategory();
        int count = LevelParseIdiomFlower.main.listCategory.Count;
        for (int i = 0; i < count; i++)
        {
            CaiCaiLeItemInfo info = LevelParseIdiomFlower.main.listCategory[i] as CaiCaiLeItemInfo;
            LevelParseIdiomFlower.main.categoryTitle = info.title;

            List<object> listsort = LevelParseIdiomFlower.main.ParseSort(info.title);

            for (int j = 0; j < listsort.Count; j++)
            {
                CaiCaiLeItemInfo infoSort = listsort[j] as CaiCaiLeItemInfo;
                LevelParseIdiomFlower.main.sortTitle = infoSort.title;
                LevelParseIdiomFlower.main.CleanGuankaList();
                // LevelParseIdiomFlower.main.ParseGuanka();


                string filepath = CloudRes.main.rootPathGameRes + "/guanka/" + LevelParseIdiomFlower.main.categoryTitle + "/" + LevelParseIdiomFlower.main.sortTitle + ".json";
                //FILE_PATH
                string json = FileUtil.ReadStringAsset(filepath);
                JsonData root = JsonMapper.ToObject(json);
                JsonData items = root["items"];
                for (int k = 0; k < items.Count; k++)
                {

                    JsonData item = items[k];
                    string title = JsonUtil.GetString(item, "title", "");
                    if (title.Length != 4)
                    {
                        continue;
                    }
                    List<object> listother = GetOtherIdiomList(title);
                    int[] indexOther = Common.RandomIndex(listother.Count, 3);
                    CaiCaiLeItemInfo info0 = listother[indexOther[0]] as CaiCaiLeItemInfo;
                    item["other0"] = info0.title;
                    CaiCaiLeItemInfo info1 = listother[indexOther[1]] as CaiCaiLeItemInfo;
                    item["other1"] = info1.title;
                    CaiCaiLeItemInfo info2 = listother[indexOther[2]] as CaiCaiLeItemInfo;
                    item["other2"] = info2.title;
                }

                //save json 
                string strJson = JsonMapper.ToJson(root);
                filepath = Application.streamingAssetsPath + "/" + CloudRes.main.rootPathGameRes + "/guanka/" + LevelParseIdiomFlower.main.categoryTitle + "/" + LevelParseIdiomFlower.main.sortTitle + ".json";
                byte[] bytes = Encoding.UTF8.GetBytes(strJson);
                System.IO.File.WriteAllBytes(filepath, bytes);

            }
        }
    }


    //生成成语关卡数据：不出现重复的字
    [MenuItem("CaiCaiLe/Flower/OnMakePoemFlowerLevelData")]
    static void OnMakePoemFlowerLevelData()
    {
        List<PoemLevelJsonInfo> listLevelJson = new List<PoemLevelJsonInfo>();

        var listPlace = LevelManager.main.ParsePlaceList();
        int count = listPlace.Count;
        for (int i = 0; i < count; i++)
        {
            ItemInfo infoPlace = LevelManager.main.GetPlaceItemInfo(i);
            LevelParsePoemFlower.main.CleanGuankaList();
            //  LevelParsePoemFlower.main.ParseGuanka();


            string filepath = CloudRes.main.rootPathGameRes + "/guanka/item_" + infoPlace.id + ".json";
            //FILE_PATH
            string json = FileUtil.ReadStringAsset(filepath);
            JsonData root = JsonMapper.ToObject(json);
            JsonData items = root["items"];
            for (int k = 0; k < items.Count; k++)
            {
                JsonData item = items[k];
                CaiCaiLeItemInfo info = new CaiCaiLeItemInfo();
                info.id = JsonUtil.GetString(item, "id", "");
                info.listPoemContent = GameLevelParse.main.ParsePoemContent(info.id);
                if (info.listPoemContent == null)
                {
                    Debug.Log("listPoemContent is null");
                    continue;
                }
                foreach (PoemContentInfo infoPoem in info.listPoemContent)
                {
                    PoemLevelJsonInfo infoJson = new PoemLevelJsonInfo();
                    infoJson.id = info.id;
                    string content = FilterPunctuation(infoPoem.content);
                    string[] listTemp = content.Split('-');
                    if (listTemp.Length >= 2)
                    {
                        string strtmp = listTemp[0];
                        strtmp = strtmp.Replace("-", "");
                        if (!FilterPoemContent(strtmp))
                        {
                            break;
                        }
                        infoJson.content0 = strtmp;


                        strtmp = listTemp[1];
                        strtmp = strtmp.Replace("-", "");
                        if (!FilterPoemContent(strtmp))
                        {
                            break;
                        }
                        infoJson.content1 = strtmp;
                        int idx = Random.Range(0, listLevelJson.Count);
                        //listLevelJson.Add(infoJson);
                        //随机打乱
                        listLevelJson.Insert(idx, infoJson);
                    }
                }
                //    LevelParsePoemFlower.main.listGuanka.Add(info);

            }

            //save json  
            filepath = Application.streamingAssetsPath + "/" + CloudRes.main.rootPathGameRes + "/guanka/item_" + infoPlace.id + ".json";
            SavePoemJson(listLevelJson, filepath);
        }


    }


    static bool FilterPoemContent(string str)
    {
        bool ret = false;
        if ((str.Length == 5) || (str.Length == 7))
        {
            ret = true;
        }
        return ret;
    }

    static string FilterPunctuation(string str)
    {
        string ret = str;

        foreach (string item in GameLevelParse.main.arrayPunctuation)
        {
            ret = ret.Replace(item, "-");
        }
        return ret;
    }

    static void GetAllIdiom()
    {
        listAllIdiom = new List<object>();
        LevelParseIdiomFlower.main.ParseCategory();
        int count = LevelParseIdiomFlower.main.listCategory.Count;
        for (int i = 0; i < count; i++)
        {
            CaiCaiLeItemInfo info = LevelParseIdiomFlower.main.listCategory[i] as CaiCaiLeItemInfo;
            LevelParseIdiomFlower.main.categoryTitle = info.title;

            List<object> listsort = LevelParseIdiomFlower.main.ParseSort(info.title);

            for (int j = 0; j < listsort.Count; j++)
            {
                CaiCaiLeItemInfo infoSort = listsort[j] as CaiCaiLeItemInfo;
                LevelParseIdiomFlower.main.sortTitle = infoSort.title;
                LevelParseIdiomFlower.main.CleanGuankaList();
                LevelParseIdiomFlower.main.ParseGuanka();
                listAllIdiom.AddRange(LevelParseIdiomFlower.main.listGuanka);
            }
        }

        Debug.Log("listAllIdiom Count=" + listAllIdiom.Count);
    }
    static List<object> GetOtherIdiomList(string idiom)
    {
        List<object> listother = new List<object>();
        foreach (object obj in listAllIdiom)
        {
            CaiCaiLeItemInfo info = obj as CaiCaiLeItemInfo;
            if (!IsWordCross(idiom, info.title))
            {
                listother.Add(obj);
            }
        }
        return listother;
    }

    //判断两个string是否有重复的字
    static bool IsWordCross(string word1, string word2)
    {
        bool ret = false;
        for (int i = 0; i < word1.Length; i++)
        {
            if (word2.Contains(word1.Substring(i, 1)))
            {
                ret = true;
                break;
            }
        }
        return ret;
    }



    static void AddJson()
    {

        //save guanka json 
        string strrowcol = "";
        FlowerEditorJsonItemInfo infojson = new FlowerEditorJsonItemInfo();
        int idx = 0;
        foreach (FlowerEditorItemInfo info in listItemSel)
        {

            string str = info.row.ToString() + "-" + info.col.ToString();
            strrowcol += str;
            if (idx < listItemSel.Count - 1)
            {
                strrowcol += ",";
            }
            idx++;
        }
        infojson.rowcol = strrowcol;
        listJson.Add(infojson);

    }

    static void SaveJson(FlowerEditorItemInfo info)
    {
        //save guanka json  
        Hashtable data = new Hashtable();
        data["items"] = listJson;
        string strJson = JsonMapper.ToJson(data);
        string dir = Application.streamingAssetsPath + "/" + CloudRes.main.rootPathGameRes + "/guanka/data";
        FileUtil.CreateDir(dir);
        string filepath = dir + "/item_position_" + info.row + "x" + info.col + "_" + info.wordNum + ".json";
        byte[] bytes = Encoding.UTF8.GetBytes(strJson);
        System.IO.File.WriteAllBytes(filepath, bytes);

    }


    static void SavePoemJson(List<PoemLevelJsonInfo> list, string filepath)
    {
        //save guanka json  
        Hashtable data = new Hashtable();
        data["items"] = list;
        string strJson = JsonMapper.ToJson(data);
        byte[] bytes = Encoding.UTF8.GetBytes(strJson);
        System.IO.File.WriteAllBytes(filepath, bytes);

    }

    static bool MakeFlowerData(FlowerEditorItemInfo info)
    {
        bool ret = true;
        FlowerEditorItemInfo infoSeed = null;
        bool isError = false;
        listItemSel.Clear();
        for (int i = 0; i < rowTotal * colTotal; i++)
        {
            if (listItemSel.Count % (info.wordNum) == 0)
            {
                //成语的开头
                infoSeed = GetSeed();
                if (infoSeed != null)
                {
                    listItemSel.Add(infoSeed);
                    if (infoSeed.col < 0)
                    {
                        // Debug.Log("GetSeed  row=" + infoSeed.row + " col=" + infoSeed.col);
                    }
                }


            }
            if (infoSeed == null)
            {
                //ng
                isError = true;
                break;
            }


            List<FlowerEditorItemInfo> listSide = GetSideItems(infoSeed.row, infoSeed.col);
            if (listSide.Count == 0)
            {
                //ng
                isError = true;
                break;
            }

            int idx = Random.Range(0, listSide.Count);
            FlowerEditorItemInfo infoSide = listSide[idx];
            listItemSel.Add(infoSide);
            infoSeed = infoSide;
            if (infoSeed.col < 0)
            {
                Debug.Log("GetSeed  row=" + infoSeed.row + " col=" + infoSeed.col);
            }
            if (listItemSel.Count >= totalIdiom * info.wordNum)
            {
                // finish
                ret = true;
                isError = false;
                break;
            }

        }

        if (isError)
        {
            if (listItemSel.Count < totalIdiom * info.wordNum)
            {
                // fail
                ret = false;
            }
        }


        return ret;
    }


    static FlowerEditorItemInfo GetSeed()
    {
        List<FlowerEditorItemInfo> list = new List<FlowerEditorItemInfo>();
        for (int i = 0; i < rowTotal; i++)
        {
            for (int j = 0; j < colTotal; j++)
            {
                if (!IsItemHasSel(i, j))
                {
                    FlowerEditorItemInfo info = new FlowerEditorItemInfo();
                    info.row = i;
                    info.col = j;
                    if (info.col < 0)
                    {
                        // Debug.Log("GetSeed inner  row=" + info.row + " col=" + info.col);
                    }
                    list.Add(info);
                }
            }
        }
        if (list.Count > 0)
        {
            int idx = Random.Range(0, list.Count);
            FlowerEditorItemInfo infoRet = list[idx];
            return infoRet;
        }
        // Debug.Log("GetSeed ret null=");
        return null;
    }
    static bool IsItemHasSel(int row, int col)
    {
        bool ret = false;
        foreach (FlowerEditorItemInfo info in listItemSel)
        {
            if ((info.row == row) && (info.col == col))
            {
                ret = true;
                break;
            }
        }
        return ret;
    }

    static void AddItem(List<FlowerEditorItemInfo> list, int rtmp, int ctmp, int direct)
    {
        if (!IsItemHasSel(rtmp, ctmp))
        {
            FlowerEditorItemInfo info = new FlowerEditorItemInfo();
            info.row = rtmp;
            info.col = ctmp;
            if (ctmp < 0)
            {
                Debug.Log("AddItem  row=" + rtmp + " col=" + ctmp + " direct=" + direct);
            }
            list.Add(info);
        }
    }
    //获取相邻的item
    static List<FlowerEditorItemInfo> GetSideItems(int row, int col)
    {
        int r, c;
        List<FlowerEditorItemInfo> list = new List<FlowerEditorItemInfo>();
        //左
        {
            r = row;
            c = col;
            c--;
            if (c >= 0)
            {
                AddItem(list, r, c, 0);
            }

        }
        //右
        {
            r = row;
            c = col;
            c++;
            if (c < colTotal)
            {
                AddItem(list, r, c, 1);
            }
        }
        //上
        {
            r = row;
            c = col;
            r++;
            if (r < rowTotal)
            {
                AddItem(list, r, c, 2);
            }
        }
        //下
        {
            r = row;
            c = col;
            r--;
            if (r >= 0)
            {
                AddItem(list, r, c, 3);
            }
        }



        //左上
        {
            r = row;
            c = col;
            c--;
            r++;
            if ((c >= 0) && (r < rowTotal))
            {
                AddItem(list, r, c, 4);
            }
        }
        //左下
        {
            r = row;
            c = col;
            c--;
            r--;
            if ((c >= 0) && (r >= 0))
            {
                AddItem(list, r, c, 5);
            }
        }

        //右上
        {
            r = row;
            c = col;
            c++;
            r++;
            if ((c < colTotal) && (r < rowTotal))
            {
                AddItem(list, r, c, 6);
            }
        }
        //右下 
        {
            r = row;
            c = col;
            c++;
            r--;
            if ((r >= 0) && (c < colTotal))
            {
                AddItem(list, r, c, 7);
            }
        }

        return list;
    }

}
