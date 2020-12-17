
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class DBItemInfo
{
    public string pic; // 
    public string filesave; //成语出处： 
    public string addtime; 
    public string id; //简拼 ZWJY 
    public string date;

}

public class DBBase
{
    public string TABLE_NAME = "table_items";
    public DBToolBase dbTool;
    public string dbFileName;
    public bool isNeedCopyFromAsset = false;


    public const string KEY_id = "id";
    public const string KEY_title = "title";
    public const string KEY_translation = "translation";
    public const string KEY_date = "date";
    public const string KEY_addtime = "addtime";



    public const string KEY_album = "album";
    public const string KEY_pinyin = "pronunciation";
    public const string KEY_year = "year";

    public const string KEY_usage = "usage";
    public const string KEY_common_use = "common_use";

    public const string KEY_emotional = "emotional";
    public const string KEY_structure = "structure";
    public const string KEY_near_synonym = "near_synonym";  //近义词
    public const string KEY_antonym = "antonym"; //反义词  
    public const string KEY_example = "example";
    public const string KEY_correct_pronunciation = "correct_pronunciation"; //成语正音：别，不能读作“biè”


    public const string KEY_text = "text";

    int colLength;

    static public string strSaveWordShotDir//字截图保存目录
    {
        get
        {
            return Application.temporaryCachePath + "/SaveItem";
        }
    }

    public string dbFilePath
    {
        get
        {
            string appDBPath = Application.temporaryCachePath + "/" + dbFileName;
            Debug.Log("appDBPath=" + appDBPath);
            if (Common.isAndroid)
            {
                appDBPath = dbFileName;
            }
            return appDBPath;
        }
    }
    static bool isInited = false;

    public void CreateDb()
    {
        if (Common.isAndroid || Common.isiOS)
        {
            dbTool = new DBToolSystem();
        }
        else
        {
            dbTool = new DBToolSqliteKit();
        }

        if (Common.isAndroid)
        {
            if (isNeedCopyFromAsset)
            {
                dbTool.CopyFromAsset(dbFilePath);
            }
            dbTool.OpenDB(dbFilePath);
        }
        else if (Common.isiOS)
        {
            dbTool.OpenDB(dbFilePath);
        }
        else
        {
            OpenDB();
        }


        CloseDB();
    }


    public void CreateTable(string[] item_col)
    {
        colLength = item_col.Length;
        OpenDB();
        string[] item_coltype = new string[item_col.Length];
        for (int i = 0; i < item_coltype.Length; i++)
        {
            item_coltype[i] = KEY_text;
        }

        if (item_col.Length != item_coltype.Length)
        {
            Debug.Log("DB Table Error");
        }
        if (!dbTool.IsExitTable(TABLE_NAME))
        {
            dbTool.CreateTable(TABLE_NAME, item_col, item_coltype);
        }

        CloseDB();
    }

    public void OpenDB()
    {
        if (Common.isAndroid)
        {
            return;
        }
        if (Common.isiOS)
        {
            return;
        }

        dbTool.OpenDB(dbFilePath);
        //   string[] item_col = new string[] { "id,filesave,date,addtime" };
        //   string[] item_coltype = new string[] { "string,string,string,string" };

    }
    //判断是否空
    public bool DBEmpty()
    {
        bool ret = true;
        //TABLE_NAME
        string strsql = "select id from " + TABLE_NAME + " order by addtime desc";
        OpenDB();
        //SqliteDataReader reader = dbTool.ReadFullTable(TABLE_NAME);//
        SqlInfo info = dbTool.ExecuteQuery(strsql, false);
        while (dbTool.MoveToNext(info))// 循环遍历数据 
        {
            ret = false;
            break;
        }

        // reader.Release();

        CloseDB();
        return ret;
    }

    public void CloseDB()
    {
        if (Common.isAndroid)
        {
            return;
        }
        if (Common.isiOS)
        {
            return;
        }
        dbTool.CloseDB();
    }

    public void ClearDB()
    {
        //
        string dir = strSaveWordShotDir;
        //Directory.Delete(dir);
        DirectoryInfo TheFolder = new DirectoryInfo(dir);

        // //遍历文件
        // foreach (FileInfo NextFile in TheFolder.GetFiles())
        // {
        //     NextFile.Delete();

        // }
        OpenDB();
        dbTool.DeleteContents(TABLE_NAME);
        CloseDB();
    }



    public void CopyDbFileFromResource()
    {
        string src = CloudRes.main.rootPathGameRes + "/" + dbFileName;
        string dst = dbFilePath;
        if (!FileUtil.FileIsExist(dst))
        {
            byte[] data = FileUtil.ReadDataAsset(src);
            System.IO.File.WriteAllBytes(dst, data);
        }

    }

    //2017.1.10 to 1.10
    static public string getDateDisplay(string date)
    {
        int idx = date.IndexOf(".");
        if (idx >= 0)
        {
            string str = date.Substring(idx + 1);
            return str;
        }
        return date;
    }

    //{ "id", "intro", "album", "translation", "author", "year", "style", "pinyin", "appreciation", "head", "end", "tips", "date", "addtime" };
    public virtual void AddItem(DBItemInfo info)
    {
        OpenDB();
        int lengh = colLength;
        string[] values = new string[lengh];
        //id,filesave,date,addtime 

        values[0] = info.id;
        //values[0] = "性";//ng
 

        

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


    public void DeleteItem(DBItemInfo info)
    {
        OpenDB();
        // string strsql = "DELETE FROM " + TABLE_NAME + " WHERE id = '" + info.id + "'" + " and addtime = '" + info.addtime + "'";
        string strsql = "DELETE FROM " + TABLE_NAME + " WHERE id = '" + info.id + "'";
        dbTool.ExecSQL(strsql);
        CloseDB();
    }


    public bool IsItemExist(DBItemInfo info)
    {
        bool ret = false;
        OpenDB();
        //string strsql = "SELECT count(*) FROM " + TABLE_NAME + " WHERE id = "" + info.id + "'";
        string strsql = "SELECT * FROM " + TABLE_NAME + " WHERE id = '" + info.id + "'";
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

    public virtual void ReadInfo(DBItemInfo info, SqlInfo infosql)
    {
        info.id = dbTool.GetString(infosql, KEY_id);
         
    }


    public List<DBItemInfo> GetAllItem()
    {
        // Distinct 去掉重复
        //desc 降序 asc 升序 
        // string strsql = "select DISTINCT id from " + TABLE_NAME + " order by addtime desc";
        string strsql = "select * from " + TABLE_NAME + " order by addtime desc";

        List<DBItemInfo> listRet = new List<DBItemInfo>();
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
            DBItemInfo info = new DBItemInfo();
            ReadInfo(info, infosql);
            listRet.Add(info);
            if (!dbTool.MoveToNext(infosql))
            {
                break;
            }
        }

        // reader.Release();

        CloseDB();
        Debug.Log("GetAllItem:" + listRet.Count);
        return listRet;
    }

    public List<DBItemInfo> GetAllDate()
    {
        // Distinct 去掉重复
        //desc 降序 asc 升序 
        string strsql = "select DISTINCT date from " + TABLE_NAME + " order by addtime desc";

        List<DBItemInfo> listRet = new List<DBItemInfo>();
        OpenDB();
        SqlInfo infosql = dbTool.ExecuteQuery(strsql, false);
        bool ret = dbTool.MoveToFirst(infosql);
        if (ret == false)
        {
            return listRet;
        }
        while (true)// 循环遍历数据 
        {
            DBItemInfo info = new DBItemInfo();
            info.date = dbTool.GetString(infosql, "date");
            listRet.Add(info);
            if (!dbTool.MoveToNext(infosql))
            {
                break;
            }
        }

        // reader.Release();

        CloseDB();
        return listRet;
    }


    public List<DBItemInfo> GetItemByDate(string date)
    {
        string strsql = "select * from " + TABLE_NAME + " where date = '" + date + "'" + "order by addtime desc";
        List<DBItemInfo> listRet = new List<DBItemInfo>();
        OpenDB();

        SqlInfo infosql = dbTool.ExecuteQuery(strsql, false);
        bool ret = dbTool.MoveToFirst(infosql);
        if (ret == false)
        {
            return listRet;
        }
        while (true)// 循环遍历数据 
        {
            DBItemInfo info = new DBItemInfo();
            ReadInfo(info, infosql);
            listRet.Add(info);
            if (!dbTool.MoveToNext(infosql))
            {
                break;
            }
        }

        // reader.Release();

        CloseDB();
        return listRet;
    }



    public DBItemInfo GetItemById(string id)
    {
        string strsql = "select * from " + TABLE_NAME + " where id = '" + id + "'";
        //List<DBItemInfo> listRet = new List<DBItemInfo>();
        DBItemInfo info = new DBItemInfo();
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
            ReadInfo(info, infosql);
            if (!dbTool.MoveToNext(infosql))
            {
                break;
            }
            break;
            //listRet.Add(info);
        }

        //  reader.Release();

        CloseDB();
        return info;
    }


}

