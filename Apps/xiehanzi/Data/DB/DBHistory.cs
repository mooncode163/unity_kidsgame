
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class DBHistory : DBBase
{

    public const string KEY_filesave = "filesave";
    // public string[] item_col = new string[] { KEY_id, KEY_pinyin,KEY_date,KEY_addtime};
    string[] item_col = new string[] { KEY_id, KEY_pinyin, KEY_filesave, KEY_date, KEY_addtime };


    static DBHistory _main = null;
    public static DBHistory main
    {
        get
        {
            if (_main == null)
            {
                _main = new DBHistory();
                Debug.Log("DBLove main init");
                _main.TABLE_NAME = "table_word";
                _main.dbFileName = "History.db";
                _main.Init();
            }
            return _main;
        }

    }

    static DBHistory _mainFreeWrite = null;
    public static DBHistory mainFreeWrite
    {
        get
        {
            if (_mainFreeWrite == null)
            {
                _mainFreeWrite = new DBHistory();
                Debug.Log("DBHistory mainFreeWrite init");
                _mainFreeWrite.TABLE_NAME = "table_word";
                _mainFreeWrite.dbFileName = "History_freewrite.db";
                _mainFreeWrite.Init();
            }
            return _mainFreeWrite;
        }

    }

    public void Init()
    {
        isNeedCopyFromAsset = false;
        CreateDb();
        CreateTable(item_col);
    }


    //{ "id", "intro", "album", "translation", "author", "year", "style", "pinyin", "appreciation", "head", "end", "tips", "date", "addtime" };
    public override void AddItem(DBItemInfoBase info)
    {
        DBWordItemInfo dbinfo = info as DBWordItemInfo;
        OpenDB();
        int lengh = item_col.Length;
        string[] values = new string[lengh];
        //id,filesave,date,addtime 

        values[0] = dbinfo.id;
        values[1] = dbinfo.pinyin;
        values[2] = dbinfo.filesave;
        // values[3] = dbinfo.date;


        int year = System.DateTime.Now.Year;
        int month = System.DateTime.Now.Month;
        int day = System.DateTime.Now.Day;
        string str = year + "." + month + "." + day;
        Debug.Log("date:" + str);
        values[lengh - 2] = str;
        long time_ms = Common.GetCurrentTimeMs();//GetCurrentTimeSecond
        values[lengh - 1] = time_ms.ToString();
        dbTool.InsertInto(TABLE_NAME, values);

        CloseDB();
        //  GetItemsByWord();


    }

    public override void ReadInfo(DBItemInfoBase info, SqlInfo infosql)
    {
        DBWordItemInfo dbinfo = info as DBWordItemInfo;
        dbinfo.id = dbTool.GetString(infosql, KEY_id);
        dbinfo.pinyin = dbTool.GetString(infosql, KEY_pinyin);
        dbinfo.filesave = dbTool.GetString(infosql, KEY_filesave);
        dbinfo.date = dbTool.GetString(infosql, KEY_date);
        dbinfo.addtime = dbTool.GetString(infosql, KEY_addtime);
    }



    public List<WordItemInfo> GetAllWord()
    {

        string strsql = "select  id from " + TABLE_NAME + " order by addtime desc";

        List<WordItemInfo> listRet = new List<WordItemInfo>();
        OpenDB();
        //SqliteDataReader reader = dbTool.ReadFullTable(TABLE_NAME);//
        SqlInfo infosql = dbTool.ExecuteQuery(strsql, false);
        Debug.Log("GetAllItem start read");
        bool ret = dbTool.MoveToFirst(infosql);
        if (ret == false)
        {
            return listRet;
        }
        while (true)// 循环遍历数据 
        {
            Debug.Log("GetAllItem reading");
            DBWordItemInfo infoDB = new DBWordItemInfo();
            // ReadInfo(infoDB, infosql);
            infoDB.id = dbTool.GetString(infosql, KEY_id);
            WordItemInfo info = new WordItemInfo();
            info.id = infoDB.id;
            info.dbInfo = infoDB;
            listRet.Add(info);
            if (!dbTool.MoveToNext(infosql))
            {
                break;
            }

        }

        // reader.Release();

        CloseDB();
        Debug.Log("GetAllWord:" + listRet.Count);
        return listRet;

    }

    public List<WordItemInfo> GetAllDate()
    {
        // Distinct 去掉重复
        //desc 降序 asc 升序 
        string strsql = "select  date from " + TABLE_NAME + " order by addtime desc";


        List<WordItemInfo> listRet = new List<WordItemInfo>();
        OpenDB();
        //SqliteDataReader reader = dbTool.ReadFullTable(TABLE_NAME);//
        SqlInfo infosql = dbTool.ExecuteQuery(strsql, false);
        int count = dbTool.GetCount(infosql);

        bool ret = dbTool.MoveToFirst(infosql);
        Debug.Log("strsql=" + strsql + " count=" + count + " ret=" + ret);
        if (ret == false)
        {
            return listRet;
        }
        while (true)// 循环遍历数据 
        {
            Debug.Log("GetAllItem reading");
            DBWordItemInfo infoDB = new DBWordItemInfo();
            // ReadInfo(infoDB, infosql); 
            infoDB.date = dbTool.GetString(infosql, KEY_date);
            WordItemInfo info = new WordItemInfo();
            // info.id = infoDB.id;
            info.dbInfo = infoDB;
            listRet.Add(info);
            if (!dbTool.MoveToNext(infosql))
            {
                break;
            }
        }


        CloseDB();
        Debug.Log("GetAllDate:" + listRet.Count);
        return listRet;


    }
    public List<WordItemInfo> GetItemsOfDate(string date)
    {
        string strsql = "select * from " + TABLE_NAME + " where date = '" + date + "'" + "order by addtime desc";

        List<WordItemInfo> listRet = new List<WordItemInfo>();
        OpenDB();
        //SqliteDataReader reader = dbTool.ReadFullTable(TABLE_NAME);//
        SqlInfo infosql = dbTool.ExecuteQuery(strsql, false);
        Debug.Log("GetAllItem start read");
        bool ret = dbTool.MoveToFirst(infosql);
        if (ret == false)
        {
            return listRet;
        }
        while (true)// 循环遍历数据 
        {
            DBWordItemInfo infoDB = new DBWordItemInfo();
            ReadInfo(infoDB, infosql);
            WordItemInfo info = new WordItemInfo();
            info.id = infoDB.id;
            info.dbInfo = infoDB;
            listRet.Add(info);
            if (!dbTool.MoveToNext(infosql))
            {
                break;
            }
        }


        CloseDB();
        Debug.Log("GetItemsOfDate:" + listRet.Count);
        return listRet;
    }


    public List<WordItemInfo> GetItemsOfWord(string word)
    {
        string strsql = "select * from " + TABLE_NAME + " where id = '" + word + "'" + "order by addtime desc";
        List<WordItemInfo> listRet = new List<WordItemInfo>();
        OpenDB();
        //SqliteDataReader reader = dbTool.ReadFullTable(TABLE_NAME);//
        SqlInfo infosql = dbTool.ExecuteQuery(strsql, false);
        Debug.Log("GetAllItem start read");
        bool ret = dbTool.MoveToFirst(infosql);
        if (ret == false)
        {
            return listRet;
        }
        while (true)// 循环遍历数据 
        {
            DBWordItemInfo infoDB = new DBWordItemInfo();
            ReadInfo(infoDB, infosql);
            WordItemInfo info = new WordItemInfo();
            info.id = infoDB.id;
            info.dbInfo = infoDB;
            listRet.Add(info);
            if (!dbTool.MoveToNext(infosql))
            {
                break;
            }
        }


        CloseDB();
        Debug.Log("GetItemsOfDate:" + listRet.Count);
        return listRet;
    }



}


