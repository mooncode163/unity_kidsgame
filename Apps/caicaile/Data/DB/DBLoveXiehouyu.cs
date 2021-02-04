
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DBLoveXiehouyu : DBLoveBase
{
    public const string KEY_head = "head";
    public const string KEY_end = "end"; 

    public string[] item_col = new string[] { KEY_id,KEY_head, KEY_end,KEY_date,KEY_addtime
 };
    //       public string[] item_col = new string[] { KEY_id, KEY_title, KEY_pinyin, KEY_album, KEY_translation, KEY_year, KEY_usage,  KEY_common_use, KEY_emotional, KEY_structure,
    //         KEY_near_synonym, KEY_antonym, KEY_example, KEY_correct_pronunciation,KEY_date,KEY_addtime
    //  };
    static DBLoveXiehouyu _main = null;
    static bool isInited = false;
    public static DBLoveXiehouyu main
    {
        get
        {
           if (_main==null)
            {
                isInited = true;
                _main = new DBLoveXiehouyu();
                Debug.Log("DBLoveXiehouyu main init");
                _main.TABLE_NAME = "table_items";
                _main.dbFileName = "LoveXiehouyu.db";
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
        DBInfoXiehouyu dbinfo = info as DBInfoXiehouyu;
        OpenDB();
        int lengh = item_col.Length;
        string[] values = new string[lengh];
        //id,filesave,date,addtime 

        values[0] = dbinfo.id;
        values[1] = dbinfo.head;
        values[2] = dbinfo.end; 


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
        DBInfoXiehouyu dbinfo = info as DBInfoXiehouyu;
         dbinfo.id = dbTool.GetString(infosql, KEY_id);
        dbinfo.head = dbTool.GetString(infosql, KEY_head);
        dbinfo.end = dbTool.GetString(infosql, KEY_end); 

    }
    public override void DeleteItem(DBItemInfoBase info)
    {
         DBInfoXiehouyu dbinfo = info as DBInfoXiehouyu;
        OpenDB();
        // string strsql = "DELETE FROM " + TABLE_NAME + " WHERE id = '" + info.id + "'" + " and addtime = '" + info.addtime + "'";
        string strsql = "DELETE FROM " + TABLE_NAME + " WHERE head = '" + dbinfo.head + "'" + " and end = '" + dbinfo.end + "'";
        dbTool.ExecSQL(strsql);
        CloseDB();
    }
    public override bool IsItemExist(DBItemInfoBase info)
    {
         DBInfoXiehouyu dbinfo = info as DBInfoXiehouyu;
        bool ret = false;
        OpenDB();
        //string strsql = "SELECT count(*) FROM " + TABLE_NAME + " WHERE id = "" + info.id + "'";
        string strsql = "SELECT * FROM " + TABLE_NAME + " WHERE head = '" + dbinfo.head + "'" + " and end = '" + dbinfo.end + "'";
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


}

