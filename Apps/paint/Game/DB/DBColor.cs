
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class DBColor : DBBase
{
    // public const string TABLE_NAME = "TableIdiom"; 
    //  string[] item_col = new string[] { "id", "pic", "filesave", "date", "addtime" };
    public const string KEY_filesave = "filesave";
    public const string KEY_pic = "pic";
    public string[] item_col = new string[] { KEY_id, KEY_pic, KEY_filesave, KEY_date, KEY_addtime
 };

    static public string strSaveColorDir//字截图保存目录
    {
        get
        {
            return Application.temporaryCachePath + "/color";
        }
    }
    static DBColor _main = null;
    static bool isInited = false;
    public static DBColor main
    {
        get
        {
            if (!isInited)
            {
                isInited = true;
                _main = new DBColor();
                Debug.Log("DBColor main init");
                _main.TABLE_NAME = "table_color";
                _main.dbFileName = "Color.db";
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
        if (Application.isEditor || Common.isiOS)
        {
            // CopyDbFileFromResource();
        }
        CreateDb();
        CreateTable(item_col);
    }


    //{ "id", "intro", "album", "translation", "author", "year", "style", "pinyin", "appreciation", "head", "end", "tips", "date", "addtime" };
    public override void AddItem(DBItemInfo info)
    {
        OpenDB();
        int lengh = item_col.Length;
        string[] values = new string[lengh];
        //id,filesave,date,addtime  
        values[0] = info.id;
        //values[0] = "性";//ng
        values[1] = info.pic;
        values[2] = info.filesave;


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



    public override void ReadInfo(DBItemInfo info, SqlInfo infosql)
    {

        info.id = dbTool.GetString(infosql, KEY_id);
        info.pic = dbTool.GetString(infosql, KEY_pic);
        info.filesave = dbTool.GetString(infosql, KEY_filesave);
        info.addtime = dbTool.GetString(infosql, KEY_addtime);
        info.date = dbTool.GetString(infosql, KEY_date);
    }
    //更新时间信息
    public void UpdateItemTime(DBItemInfo info)
    {
        OpenDB();
        int year = System.DateTime.Now.Year;
        int month = System.DateTime.Now.Month;
        int day = System.DateTime.Now.Day;
        string strDate = year + "." + month + "." + day;
        long time_sconde = Common.GetCurrentTimeSecond();
        string strTime = time_sconde.ToString();

        string strsql = "UPDATE " + TABLE_NAME + " SET date = '" + strDate + "', " + "addtime = '" + strTime + "'" + " where id  = '" + info.id + "'";
        //string strsql = "update " + WORD_TABLE_NAME + " set addtime = '" + strTime + "'"+ " where id  = '" + info.id + "'";
        dbTool.ExecSQL(strsql);
        CloseDB();
    }

}

