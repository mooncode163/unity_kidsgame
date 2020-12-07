using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class DBWord : DBBase
{
    // public const string TABLE_NAME = "TableIdiom"; 
    public const string KEY_filesave = "filesave";

    public const string KEY_zuci = "zuci";
    public const string KEY_bushou = "bushou";
    public const string KEY_bihuaCount = "bihuaCount";
    public const string KEY_bihuaName = "bihuaName";
    public const string KEY_bihuaOrder = "bihuaOrder";
    public const string KEY_audio = "audio";
    public const string KEY_gif = "gif";

    public const string KEY_meanBasic = "meanBasic";
    public const string KEY_meanDetail = "meanDetail";
    public const string KEY_antonym = "antonym";//反义词
    public const string KEY_homoionym = "homoionym";//近义词 
    public const string KEY_wubi = "wubi";//五笔
    public const string KEY_fanti = "fanti";//五笔


    // string[] item_col = new string[] { KEY_id, KEY_title, KEY_pinyin, KEY_zuci, KEY_bushou, KEY_bihua, KEY_audio, KEY_gif, KEY_mean, KEY_addtime, KEY_date };
    string[] item_col = new string[] { KEY_id, KEY_title, KEY_pinyin, KEY_zuci, KEY_bushou, KEY_bihuaCount, KEY_bihuaName, KEY_bihuaOrder, KEY_audio, KEY_gif, KEY_meanBasic, KEY_meanDetail, KEY_antonym, KEY_homoionym, KEY_wubi,KEY_fanti};

    static DBWord _main = null;
    static bool isInited = false;
    public static DBWord main
    {
        get
        {
            if (_main == null)
            {
                _main = new DBWord();
                Debug.Log("DBWord main init");
                _main.TABLE_NAME = "table_word";
                _main.dbFileName = "Word.db";
                _main.Init(true);
            }
            return _main;
        }

    }


    static DBWord _mainZuci = null;
    public static DBWord mainZuci
    {
        get
        {
            if (_mainZuci == null)
            {
                _mainZuci = new DBWord();
                Debug.Log("DBWord mainFreeWrite init");
                _mainZuci.TABLE_NAME = "table_word";
                _mainZuci.dbFileName = "WordZuci.db";
                _mainZuci.Init(true);
            }
            return _mainZuci;
        }

    }

    public void Init(bool isCopy)
    {
        isNeedCopyFromAsset = isCopy;
        if(CloudRes.main.enable)
        {
           isNeedCopyFromAsset = false; 
        }
        if (Application.isEditor || Common.isiOS)
        {
            CopyDbFileFromResource();
        }
        CreateDb();
        CreateTable(item_col);
    }


    public override void AddItem(DBItemInfoBase info)
    {
        DBWordItemInfo dbinfo = info as DBWordItemInfo;
        OpenDB();
        int lengh = item_col.Length;
        string[] values = new string[lengh];
        //id,filesave,date,addtime 

        values[0] = dbinfo.id;
        values[1] = dbinfo.title;
        values[2] = dbinfo.pinyin;
        values[3] = dbinfo.zuci;
        values[4] = dbinfo.bushou;
        values[5] = dbinfo.bihuaCount;
        values[6] = dbinfo.bihuaName;
        values[7] = dbinfo.bihuaOrder;

        values[8] = dbinfo.audio;
        values[9] = dbinfo.gif;
        values[10] = dbinfo.meanBasic;
        values[11] = dbinfo.meanDetail;
        values[12] = dbinfo.antonym;
        values[13] = dbinfo.homoionym;
        values[14] = dbinfo.wubi;
        values[15] = dbinfo.fanti;


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
        DBWordItemInfo dbinfo = info as DBWordItemInfo;
        dbinfo.id = dbTool.GetString(infosql, KEY_id);
        dbinfo.title = dbTool.GetString(infosql, KEY_title);
        dbinfo.pinyin = dbTool.GetString(infosql, KEY_pinyin);
        dbinfo.zuci = dbTool.GetString(infosql, KEY_zuci);
        dbinfo.bushou = dbTool.GetString(infosql, KEY_bushou);
        dbinfo.bihuaCount = dbTool.GetString(infosql, KEY_bihuaCount);
        dbinfo.bihuaName = dbTool.GetString(infosql, KEY_bihuaName);
        dbinfo.bihuaOrder = dbTool.GetString(infosql, KEY_bihuaOrder);
        dbinfo.audio = dbTool.GetString(infosql, KEY_audio);
        dbinfo.gif = dbTool.GetString(infosql, KEY_gif);
        dbinfo.meanBasic = dbTool.GetString(infosql, KEY_meanBasic);
        dbinfo.meanDetail = dbTool.GetString(infosql, KEY_meanDetail);
        dbinfo.antonym = dbTool.GetString(infosql, KEY_antonym);
        dbinfo.homoionym = dbTool.GetString(infosql, KEY_homoionym);
        dbinfo.wubi = dbTool.GetString(infosql, KEY_wubi);
        dbinfo.fanti = dbTool.GetString(infosql, KEY_fanti);

        // info.pinyin = rd.GetString(KEY_pinyin);
        // Debug.Log("ReadInfo info.pinyin=" + info.pinyin);
        /* 

        */
        // info.addtime = dbTool.GetString(infosql, KEY_addtime);
        // info.date = dbTool.GetString(infosql, KEY_date);
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
        if (ret == false)
        {
            return listRet;
        }
        while (true)// 循环遍历数据 
        {
            DBWordItemInfo info = new DBWordItemInfo();
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





