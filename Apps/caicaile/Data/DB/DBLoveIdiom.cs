
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DBLoveIdiom : DBLoveBase
{
    public string[] item_col = new string[] { KEY_id, KEY_pinyin,KEY_date,KEY_addtime
 };
    //       public string[] item_col = new string[] { KEY_id, KEY_title, KEY_pinyin, KEY_album, KEY_translation, KEY_year, KEY_usage,  KEY_common_use, KEY_emotional, KEY_structure,
    //         KEY_near_synonym, KEY_antonym, KEY_example, KEY_correct_pronunciation,KEY_date,KEY_addtime
    //  };
    static DBLoveIdiom _main = null;
    static bool isInited = false;
    public static DBLoveIdiom main
    {
        get
        {
            if (!isInited)
            {
                isInited = true;
                _main = new DBLoveIdiom();
                Debug.Log("DBLoveIdiom main init");
                _main.TABLE_NAME = "table_items";
                _main.dbFileName = "LoveIdiom.db";
                // _main.CopyDbFileFromResource();
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
        isNeedCopyFromAsset = false;
        CreateDb();
        CreateTable(item_col);
    }

    //{ "id", "intro", "album", "translation", "author", "year", "style", "pinyin", "appreciation", "head", "end", "tips", "date", "addtime" };
    public override void AddItem(DBItemInfoBase info)
    {
        OpenDB();
        IdiomItemInfo infoidiom = info as IdiomItemInfo;

        int lengh = item_col.Length;
        string[] values = new string[lengh];
        //id,filesave,date,addtime 

        Debug.Log("Love AddItem id=" + infoidiom.id);
        values[0] = infoidiom.id;
        //values[0] = "性";//ng
        // values[1] = info.title;
        // values[2] = info.pronunciation;
        // values[3] = info.album;
        // values[4] = info.translation;

        // values[5] = info.year;
        // values[6] = info.usage;
        // values[7] = info.common_use;
        // values[8] = info.emotional;

        // values[9] = info.structure;
        // values[10] = info.near_synonym;
        // values[11] = info.antonym;
        // values[12] = info.example;
        // values[13] = info.correct_pronunciation;



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


    }

    public override void ReadInfo(DBItemInfoBase info, SqlInfo infosql)
    {
        IdiomItemInfo infoidiom = info as IdiomItemInfo;
        infoidiom.id = dbTool.GetString(infosql, KEY_id);
        // info.title = dbTool.GetString(infosql, KEY_title);
        // info.pronunciation = dbTool.GetString(infosql, KEY_pinyin);
        // info.album = dbTool.GetString(infosql, KEY_album);
        // info.translation = dbTool.GetString(infosql, KEY_translation);
        // info.year = dbTool.GetString(infosql, KEY_year);
        // info.usage = dbTool.GetString(infosql, KEY_usage);
        // info.common_use = dbTool.GetString(infosql, KEY_common_use);
        // info.emotional = dbTool.GetString(infosql, KEY_emotional);
        // info.structure = dbTool.GetString(infosql, KEY_structure);
        // info.near_synonym = dbTool.GetString(infosql, KEY_near_synonym);
        // info.antonym = dbTool.GetString(infosql, KEY_antonym);
        // info.example = dbTool.GetString(infosql, KEY_example);
        // info.correct_pronunciation = dbTool.GetString(infosql, KEY_correct_pronunciation);


    }

    public override void DeleteItem(DBItemInfoBase info)
    {
        IdiomItemInfo dbinfo = info as IdiomItemInfo;
        OpenDB();
        // string strsql = "DELETE FROM " + TABLE_NAME + " WHERE id = '" + info.id + "'" + " and addtime = '" + info.addtime + "'";
        string strsql = "DELETE FROM " + TABLE_NAME + " WHERE id = '" + dbinfo.id + "'";
        dbTool.ExecSQL(strsql);
        CloseDB();
    }
    public override bool IsItemExist(DBItemInfoBase info)
    {
        IdiomItemInfo dbinfo = info as IdiomItemInfo;
        bool ret = false;
        OpenDB();
        //string strsql = "SELECT count(*) FROM " + TABLE_NAME + " WHERE id = "" + info.id + "'";
        string strsql = "SELECT * FROM " + TABLE_NAME + " WHERE id = '" + dbinfo.id + "'" + "'";
        SqlInfo infosql = dbTool.ExecuteQuery(strsql, false);
        int count = 0;//qr.GetCount();

        ret = dbTool.MoveToFirst(infosql);
        if (ret == false)
        {
            return false;
        }
        while (true)// 循环遍历数据 
        {
            count++;
            if (!dbTool.MoveToNext(infosql))
            {
                break;
            }
        }

        // qr.Release();
        Debug.Log("IsItemExist count=" + count);
        CloseDB();
        if (count > 0)
        {
            ret = true;
        }
        return ret;
    }


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



    public DBItemInfoBase GetItemByTitle(string title)
    {
        string strsql = "select * from " + TABLE_NAME + " where title = '" + title + "'";
        //List<DBItemInfoBase> listRet = new List<DBItemInfoBase>();
        DBItemInfoBase info = new DBItemInfoBase();
        OpenDB();
        //"select * from %s where keyZi = \"%s\" order by addtime desc"
        SqlInfo infosql = dbTool.ExecuteQuery(strsql, false);
        dbTool.MoveToFirst(infosql);
        while (dbTool.MoveToNext(infosql))// 循环遍历数据 
        {
            ReadInfo(info, infosql);
            break;
            //listRet.Add(info);
        }

        // reader.Release();

        CloseDB();
        return info;
    }


}

