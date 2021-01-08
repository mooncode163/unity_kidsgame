using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
public class BookParse
{
    public List<object> listSort = new List<object>();
    public List<object> listBook = new List<object>();
    public List<object> listWord = new List<object>();

    public int indexSort;

    static private BookParse _main = null;
    public static BookParse main
    {
        get
        {
            if (_main == null)
            {
                _main = new BookParse();
                _main.Init();
            }
            return _main;
        }
    }

    public void Init()
    {
     
        
    }
    public void ParseSort()
    {
        int count = 0;
        string fileName = CloudRes.main.rootPathGameRes +"/moregame/Book/book_sort.json";
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(fileName);//((TextAsset)Resources.Load(fileName, typeof(TextAsset))).text;
        // Debug.Log("json::"+json);
        JsonData root = JsonMapper.ToObject(json);
        JsonData items = root["items"];
        listSort.Clear();
        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];
            ItemInfo info = new ItemInfo();
            info.id = (string)item["id"];
            string strDirRoot = CloudRes.main.rootPathGameRes +"/image_detail/";
            info.pic = strDirRoot + (i + 1).ToString() + ".jpg";
            listSort.Add(info);
        }

    }
    public void ParseBookList(int idx)
    {
        int count = 0;
        indexSort = idx;
        string fileName = CloudRes.main.rootPathGameRes +"/moregame/Book/book_list.json";
        ItemInfo infoSort = listSort[indexSort] as ItemInfo;

        //FILE_PATH
        string json = FileUtil.ReadStringAsset(fileName);//((TextAsset)Resources.Load(fileName, typeof(TextAsset))).text;
        // Debug.Log("json::"+json);
        JsonData root = JsonMapper.ToObject(json);
        JsonData items = root["items"];
        listBook.Clear();
        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];
            ItemInfo info = new ItemInfo();
            info.id = (string)item["id"];
            info.title = (string)item["title"];
            string strDirRoot = CloudRes.main.rootPathGameRes +"/moregame/Book/"+infoSort.id;
            info.pic = strDirRoot + "/" +info.id+ ".jpg";
            listBook.Add(info);
        }

    }

    public void ParseBookWordList(string sortid, string bookid)
    {
        int count = 0;
        // string sortid ="";
        // string bookid ="";
        string fileName = CloudRes.main.rootPathGameRes +"/moregame/Book/wordlist/" + sortid + "_" + bookid + ".json";
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(fileName);//((TextAsset)Resources.Load(fileName, typeof(TextAsset))).text;
        // Debug.Log("json::"+json);
        JsonData root = JsonMapper.ToObject(json);
        JsonData items = root["items"];
        listWord.Clear();
        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];
            ItemInfo info = new ItemInfo();
            info.title = (string)item["title"];
            info.detail = (string)item["detail"];
            listWord.Add(info);
        }

    }
}
