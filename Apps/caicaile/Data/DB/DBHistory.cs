
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DBHistory : DBBase
{
    public string[] item_col = new string[] { KEY_id, KEY_pinyin,KEY_date,KEY_addtime
 };
    static DBHistory _main = null;
    static bool isInited = false;
    public static DBHistory main
    {
        get
        {
            if (!isInited)
            {
                isInited = true;
                _main = new DBHistory();
                Debug.Log("DBHistory main init");
                _main.dbFileName = "DBHistory.db";
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
        IdiomItemInfo dbinfo = info as IdiomItemInfo;
        OpenDB();
        int lengh = item_col.Length;
        string[] values = new string[lengh];
        //id,filesave,date,addtime 

        values[0] = dbinfo.id;
        //values[0] = "性";//ng
        // values[1] = info.title;
        values[1] = dbinfo.pronunciation;

        /* 
        values[1] = info.intro;
        values[2] = info.album;
        Debug.Log("translation=" + info.translation);
        //values[3] = "成千上万匹马在奔跑腾跃。形容群众性的活动声势浩大或场面热烈。";
        //values[3] = "成千上万匹马在奔跑腾跃。形容群众性";//ng
        // values[3] = "性";//ng
        // values[3] = "性";//ng

        values[3] = "u6027";//\u6027

        values[4] = info.author;
        values[5] = info.year;
        values[6] = info.style;
       
        values[8] = info.appreciation;
        values[9] = info.head;
        values[10] = info.end;
        values[11] = info.tips;
        */

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
        IdiomItemInfo dbinfo = info as IdiomItemInfo;
        dbinfo.id = dbTool.GetString(infosql, KEY_id);
        // info.pronunciation = dbTool.GetString(infosql, KEY_pinyin); 
    }



}

