using UnityEngine;
using System.Collections;
using System;

public class SqlInfo
{
    public string sql;
#if UNITY_ANDROID && !UNITY_EDITOR
	public	AndroidJavaObject obj; 
   
#endif
    public IntPtr dataBase;
    public IntPtr sqlite3_stmt;
    public SQLiteQuery sq;

}
public class Sql
{
    // static private Sql _main = null;
    // public static Sql main
    // {
    //     get
    //     {
    //         if (_main == null)
    //         {
    //             _main = new Sql();
    //             _main.Init();
    //         }
    //         return _main;
    //     }
    // }

    private SqlBasePlatformWrapper _platform = null;
    public SqlBasePlatformWrapper platform
    {
        get
        {

            if (_platform == null)
            {
#if UNITY_ANDROID && !UNITY_EDITOR
				_platform = new SqlAndroidWrapper();
#elif UNITY_IPHONE && !UNITY_EDITOR
				_platform = new SqliOSWrapper();
#else
                _platform = new SqlBasePlatformWrapper();
#endif
            }
            return _platform;


        }
    }

    public void Init()
    {

    }
    public void CopyFromAsset(string dbfile)
    {
        this.platform.CopyFromAsset(dbfile);
    }
    public void Open(string dbfile)
    {
        this.platform.Open(dbfile);
    }

    public void Close()//关闭数据库连接

    {
        this.platform.Close();
    }

    //执行查询
    public SqlInfo Query(string sql)
    {
        Debug.Log("Sql:Query sql=" + sql);
        SqlInfo info = null;
#if UNITY_ANDROID && !UNITY_EDITOR
			   info = this.platform.Query(sql);
#elif UNITY_IPHONE && !UNITY_EDITOR
			  info = this.platform.Query(sql);
#else
        this.platform.Query(sql);
#endif
        return info;

    }

    public void ExecSQL(string sql)
    {
        Debug.Log("Sql:ExecSQL sql=" + sql);
#if UNITY_ANDROID && !UNITY_EDITOR
			     this.platform.ExecSQL(sql);
#elif UNITY_IPHONE && !UNITY_EDITOR
			   this.platform.ExecSQL(sql);
#else
        this.platform.ExecSQL(sql);
#endif 

    }


    public bool MoveToFirst(SqlInfo info)
    {
        return this.platform.MoveToFirst(info);
    }
    public bool MoveToNext(SqlInfo info)
    {
        return this.platform.MoveToNext(info); ;
    }
    public string GetString(SqlInfo info, string key)
    {
        return this.platform.GetString(info, key);
    }
    public int GetCount(SqlInfo info)
    {
        return this.platform.GetCount(info);
    }

    public void ReadFullTable(string tableName)//读取整个表

    {
        this.platform.ReadFullTable(tableName);
    }



    public void Insert(string tableName, string[] values)//在表中插入数据

    {
        this.platform.Insert(tableName, values);
    }



    public void Update(string tableName, string[] cols, string colsValues, string selectKey, string selectValue)//替换表中数据

    {


    }



    public void Delete(string tableName, string[] cols, string[] colsvalues)//删除表中数据

    {
        // this.platform.Delete(tableName,values);

    }




    public void DeleteTable(string tableName)//删除表

    {
        this.platform.DeleteTable(tableName);

    }



    //select count(*)  from sqlite_master where type='table' and name = 'yourtablename';
    public bool IsExitTable(string name)//创建表

    {
        return this.platform.IsExitTable(name);
    }
    public void CreateTable(string name, string[] col, string[] colType)//创建表

    {
        if (col.Length != colType.Length)
        {
            // return null;
            //throw new SqliteException("columns.Length != colType.Length")

        }

        string query = "CREATE TABLE " + name + " (" + col[0] + " " + colType[0];
        for (int i = 1; i < col.Length; ++i)
        {
            query += ", " + col[i] + " " + colType[i];

        }

        query += ")";

        Query(query);



    }



    public void SelectWhere(string tableName, string[] items, string[] col, string[] operation, string[] values)//集成所有操作后执行

    {
        if (col.Length != operation.Length || operation.Length != values.Length)
        {

        }

        string query = "SELECT " + items[0];
        for (int i = 1; i < items.Length; ++i)
        {
            query += ", " + items[i];

        }
        query += " FROM " + tableName + " WHERE " + col[0] + operation[0] + "'" + values[0] + "' ";
        for (int i = 1; i < col.Length; ++i)
        {
            query += " AND " + col[i] + operation[i] + "'" + values[0] + "' ";

        }
        Query(query);

    }


}

