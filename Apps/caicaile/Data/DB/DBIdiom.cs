
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class DBIdiom : DBBase
{
    // public const string TABLE_NAME = "TableIdiom"; 
    public string[] item_col = new string[] { KEY_id, KEY_title, KEY_pinyin, KEY_album, KEY_translation, KEY_year, KEY_usage,  KEY_common_use, KEY_emotional, KEY_structure,
        KEY_near_synonym, KEY_antonym, KEY_example, KEY_correct_pronunciation
 };
    static DBIdiom _main = null;
    static bool isInited = false;
    public static DBIdiom main
    {
        get
        {
            if (!isInited)
            {
                isInited = true;
                _main = new DBIdiom();
                Debug.Log("DBIdiom main init");
                _main.TABLE_NAME = "TableIdiom";
                _main.dbFileName = "Idiom.db";
                _main.Init();
            }
            return _main;
        }


        //ng:
        //  get
        // {
        //     if (_main==null)
        //     {
        //         _main = new LoveDB();
        //         Debug.Log("LoveDB main init");
        //         _main.CreateDb();
        //     }
        //     return _main;
        // }
    }
    public void Init()
    {
        isNeedCopyFromAsset = true;
        if (Application.isEditor)
        {
            CopyDbFileFromResource();
        }
        CreateDb();
        CreateTable(item_col);
    }


    //{ "id", "intro", "album", "translation", "author", "year", "style", "pinyin", "appreciation", "head", "end", "tips", "date", "addtime" };
    public override void AddItem(DBItemInfoBase info)
    {
        IdiomItemInfo dbinfo = info as IdiomItemInfo;
        OpenDB();
        int lengh = item_col.Length;
        string[] values = new string[lengh];
        //id,filesave,date,addtime 


        values[0] = dbinfo.id;
        //values[0] = "性";//ng
        values[1] = dbinfo.title;
        values[2] = dbinfo.pronunciation;
        values[3] = dbinfo.album;
        values[4] = dbinfo.translation;

        values[5] = dbinfo.year;
        values[6] = dbinfo.usage;
        values[7] = dbinfo.common_use;
        values[8] = dbinfo.emotional;

        values[9] = dbinfo.structure;
        values[10] = dbinfo.near_synonym;
        values[11] = dbinfo.antonym;
        values[12] = dbinfo.example;
        values[13] = dbinfo.correct_pronunciation;

        // int year = System.DateTime.Now.Year;
        // int month = System.DateTime.Now.Month;
        // int day = System.DateTime.Now.Day;
        // string str = year + "." + month + "." + day;
        // Debug.Log("date:" + str);
        // values[lengh - 2] = str;
        // long time_ms = Common.GetCurrentTimeMs();//GetCurrentTimeSecond
        // values[lengh - 1] = time_ms.ToString();


        dbTool.InsertInto(TABLE_NAME, values);

        CloseDB();
        //  GetItemsByWord();


    }





    public override void ReadInfo(DBItemInfoBase info, SqlInfo infosql)
    {
        IdiomItemInfo dbinfo = info as IdiomItemInfo;
        dbinfo.id = dbTool.GetString(infosql, KEY_id);
        Debug.Log("DBIdiom ReadInfo   dbinfo.id=" + dbinfo.id);
        dbinfo.title = dbTool.GetString(infosql, KEY_title);
        dbinfo.pronunciation = dbTool.GetString(infosql, KEY_pinyin);
        dbinfo.album = dbTool.GetString(infosql, KEY_album);
        dbinfo.translation = dbTool.GetString(infosql, KEY_translation);
        dbinfo.year = dbTool.GetString(infosql, KEY_year);
        dbinfo.usage = dbTool.GetString(infosql, KEY_usage);
        dbinfo.common_use = dbTool.GetString(infosql, KEY_common_use);
        dbinfo.emotional = dbTool.GetString(infosql, KEY_emotional);
        dbinfo.structure = dbTool.GetString(infosql, KEY_structure);
        dbinfo.near_synonym = dbTool.GetString(infosql, KEY_near_synonym);
        dbinfo.antonym = dbTool.GetString(infosql, KEY_antonym);
        dbinfo.example = dbTool.GetString(infosql, KEY_example);
        dbinfo.correct_pronunciation = dbTool.GetString(infosql, KEY_correct_pronunciation);



        // Debug.Log("ReadInfo dbinfo.pinyin=" + dbinfo.pinyin);
        /* 
     
        */
        //  dbinfo.addtime = rd.GetString(KEY_addtime);
        // dbinfo.date = rd.GetString(KEY_date);
    }


    // public IdiomItemInfo GetItemById(string id)
    // {
    //     string strsql = "select * from " + TABLE_NAME + " where id = '" + id + "'";
    //     Debug.Log("GetItemById strsql=" + strsql);
    //     //List<DBItemInfoBase> listRet = new List<DBItemInfoBase>();
    //     IdiomItemInfo info = new IdiomItemInfo();
    //     OpenDB();
    //     //"select * from %s where keyZi = \"%s\" order by addtime desc"
    //     SqlInfo infosql = dbTool.ExecuteQuery(strsql, false);
    //     bool ret = dbTool.MoveToFirst(infosql);
    //     if (ret == false)
    //     {
    //         Debug.Log("GetItemById MoveToFirst fail");
    //         return info;
    //     }
    //     while (true)// 循环遍历数据 
    //     {
    //         ReadInfo(info, infosql);
    //         if (!dbTool.MoveToNext(infosql))
    //         {
    //             break;
    //         }
    //         break;
    //         //listRet.Add(info);
    //     }

    //     //  reader.Release();

    //     CloseDB();
    //     return info;
    // }

    public List<DBItemInfoBase> GetItemListById(string id)
    {
        string strsql = "select * from " + TABLE_NAME + " where id = '" + id + "'" + "order by addtime desc";
        List<DBItemInfoBase> listRet = new List<DBItemInfoBase>();
        OpenDB();
        //"select * from %s where keyZi = \"%s\" order by addtime desc"
        SqlInfo infosql = dbTool.ExecuteQuery(strsql, false);
        dbTool.MoveToFirst(infosql);
        while (dbTool.MoveToNext(infosql))// 循环遍历数据 
        {
            DBItemInfoBase info = new DBItemInfoBase();
            ReadInfo(info, infosql);
            listRet.Add(info);
        }

        //   reader.Release();

        CloseDB();
        return listRet;
    }



    public List<object> Search(string word)
    {
        List<object> listRet = new List<object>();
        if (Common.isBlankString(word))
        {
            return listRet;
        }
        string strsearch = word;
        string strsql = "SELECT * FROM " + TABLE_NAME + " WHERE title LIKE '%" + strsearch + "%'" + " OR id LIKE '%" + strsearch + "%'";
        //  strsql = "SELECT * FROM " + TABLE_NAME;
        // strsql = "SELECT rowid , * FROM " + TABLE_NAME;//SELECT rowid, * FROM "TableIdiom"
        //  strsql = "select * from TableIdiom where title like '%一%'";
        OpenDB();

        SqlInfo infosql = dbTool.ExecuteQuery(strsql, false);
        dbTool.MoveToFirst(infosql);
        while (true)// 循环遍历数据 
        {
            IdiomItemInfo info = new IdiomItemInfo();
            ReadInfo(info, infosql);
            listRet.Add(info);
            if (!dbTool.MoveToNext(infosql))
            {
                break;
            }
        }
        //   reader.Release();

        CloseDB();
        return listRet;
    }
}

