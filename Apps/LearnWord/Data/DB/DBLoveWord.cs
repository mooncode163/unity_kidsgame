
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DBLoveWord : DBLoveBase
{
    public string[] item_col = new string[] { KEY_id, KEY_pinyin,KEY_date,KEY_addtime
 };
    //       public string[] item_col = new string[] { KEY_id, KEY_title, KEY_pinyin, KEY_album, KEY_translation, KEY_year, KEY_usage,  KEY_common_use, KEY_emotional, KEY_structure,
    //         KEY_near_synonym, KEY_antonym, KEY_example, KEY_correct_pronunciation,KEY_date,KEY_addtime
    //  };
    static DBLoveWord _main = null;
    static bool isInited = false;
    public static DBLoveWord main
    {
        get
        {
            if (!isInited)
            {
                isInited = true;
                _main = new DBLoveWord();
                Debug.Log("DBLoveWord main init");
                _main.TABLE_NAME = "table_items";
                _main.dbFileName = "LoveWord.db";
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
        DBWordItemInfo infoidiom = info as DBWordItemInfo;

        int lengh = item_col.Length;
        string[] values = new string[lengh];
        //id,filesave,date,addtime 

        Debug.Log("Love AddItem id=" + infoidiom.id);
        values[0] = infoidiom.id;
        values[1] = infoidiom.pinyin; 

        int year = System.DateTime.Now.Year;
        int month = System.DateTime.Now.Month;
        int day = System.DateTime.Now.Day;
        string str = year + "." + month + "." + day;
        Debug.Log("date:" + str);
        values[lengh - 2] = str;
        long time_ms = Common.GetCurrentTimeMs();//GetCurrentTimeSecond
        values[lengh - 1] = time_ms.ToString();
        dbTool.InsertInto(TABLE_NAME, values);

        // CloseDB();


    }

    public override void ReadInfo(DBItemInfoBase info, SqlInfo infosql)
    {
        DBWordItemInfo infoidiom = info as DBWordItemInfo;
        infoidiom.id = dbTool.GetString(infosql, KEY_id);
        infoidiom.pinyin = dbTool.GetString(infosql, KEY_pinyin);
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
        DBWordItemInfo dbinfo = info as DBWordItemInfo;
        OpenDB();
        // string strsql = "DELETE FROM " + TABLE_NAME + " WHERE id = '" + info.id + "'" + " and addtime = '" + info.addtime + "'";
        string strsql = "DELETE FROM " + TABLE_NAME + " WHERE id = '" + dbinfo.id + "'";
        dbTool.ExecSQL(strsql);
        CloseDB();
    }
    public override bool IsItemExist(DBItemInfoBase info)
    {
        DBWordItemInfo dbinfo = info as DBWordItemInfo;
        bool ret = false;
        OpenDB();
        //string strsql = "SELECT count(*) FROM " + TABLE_NAME + " WHERE id = "" + info.id + "'"; 
        string strsql = "SELECT * FROM " + TABLE_NAME + " WHERE id = '" + dbinfo.id + "'";
        Debug.Log("IsItemExist strsql=" + strsql);
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

