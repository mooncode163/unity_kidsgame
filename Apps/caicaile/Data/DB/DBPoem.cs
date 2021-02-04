
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class DBPoem : DBBase
{
    // public const string TABLE_NAME = "TableIdiom"; 


    public const string KEY_author = "author";
    public const string KEY_content = "content";

    public const string KEY_content_pinyin = "content_pinyin";
    public const string KEY_authorDetail = "authorDetail";
    public string[] item_col = new string[] {  KEY_title, KEY_year, KEY_author, KEY_content, KEY_content_pinyin, KEY_translation,  KEY_authorDetail, KEY_appreciation

 };
    static DBPoem _main = null;
    static bool isInited = false;
    public static DBPoem main
    {
        get
        {
            if (!isInited)
            {
                isInited = true;
                _main = new DBPoem();
                Debug.Log("DBPoem main init");
                _main.TABLE_NAME = "TablePoem";
                _main.dbFileName = "poem.db";
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
        values[0] = dbinfo.title;
        values[1] = dbinfo.year;
        values[2] = dbinfo.author;
        values[3] = dbinfo.content;
        values[4] = dbinfo.content_pinyin;

        values[5] = dbinfo.translation;
        values[6] = dbinfo.authorDetail;
        values[7] = dbinfo.appreciation;

        dbTool.InsertInto(TABLE_NAME, values);

        CloseDB();
        //  GetItemsByWord();


    }





    public override void ReadInfo(DBItemInfoBase info, SqlInfo infosql)
    {
        IdiomItemInfo dbinfo = info as IdiomItemInfo;
        dbinfo.title = dbTool.GetString(infosql, KEY_title);
        dbinfo.id = dbinfo.title;
        dbinfo.year = dbTool.GetString(infosql, KEY_year);
        dbinfo.author = dbTool.GetString(infosql, KEY_author);
        dbinfo.content = dbTool.GetString(infosql, KEY_content);
        dbinfo.content_pinyin = dbTool.GetString(infosql, KEY_content_pinyin);
        dbinfo.translation = dbTool.GetString(infosql, KEY_translation);
        dbinfo.authorDetail = dbTool.GetString(infosql, KEY_authorDetail);
        dbinfo.appreciation = dbTool.GetString(infosql, KEY_appreciation);
    }



    public List<DBItemInfoBase> GetItemListById(string id)
    {
        string strsql = "select * from " + TABLE_NAME + " where id = '" + id + "'" + "order by addtime desc";
        List<DBItemInfoBase> listRet = new List<DBItemInfoBase>();
        OpenDB();

        //"select * from %s where keyZi = \"%s\" order by addtime desc"
        SqlInfo infosql = dbTool.ExecuteQuery(strsql, false);
        bool ret = dbTool.MoveToFirst(infosql);
        if (ret)
        {
            while (true)// 循环遍历数据 
            {
                DBItemInfoBase info = new DBItemInfoBase();
                ReadInfo(info, infosql);
                listRet.Add(info);
                if (!dbTool.MoveToNext(infosql))
                {
                    break;
                }
            }
        }

        //   reader.Release();

        CloseDB();
        return listRet;
    }



    public DBItemInfoBase GetItemByTitle(string title)
    {
        string strsql = "select * from " + TABLE_NAME + " where title = '" + title + "'";
        Debug.Log("GetItemByTitle strsql=" + strsql);
        //List<DBItemInfoBase> listRet = new List<DBItemInfoBase>();
        IdiomItemInfo info = new IdiomItemInfo();
        OpenDB();
        //"select * from %s where keyZi = \"%s\" order by addtime desc"
        SqlInfo infosql = dbTool.ExecuteQuery(strsql, false);
 
        bool ret = dbTool.MoveToFirst(infosql);
        if (ret == false)
        {
            return info;
        }
        while (true)// 循环遍历数据 
        {
            Debug.Log("GetAllItem reading");
            // T infoitem = new T();
            ReadInfo(info, infosql);

            if (!dbTool.MoveToNext(infosql))
            {
                break;
            }
        }
        // reader.Release();

        CloseDB();
        return info;
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
        bool ret = dbTool.MoveToFirst(infosql);
        if (ret)
        {
            while (true)// 循环遍历数据 
            {
                DBItemInfoBase info = new DBItemInfoBase();
                ReadInfo(info, infosql);
                listRet.Add(info);
                if (!dbTool.MoveToNext(infosql))
                {
                    break;
                }
            }
        }

        //   reader.Release();

        CloseDB();
        return listRet;
    }
}

