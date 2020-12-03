using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class DBToolBase
{
    public virtual void CopyFromAsset(string dbfile)

    {

    }
 public virtual void OpenDB(string dbfile)

    {

    }


    public virtual void CloseDB()//关闭数据库连接

    {

    }


    public virtual SqlInfo ExecuteQuery(string sqlQuery, bool isStep)//执行查询

    {
        return null;
    }
   public virtual void ExecSQL(string sqlQuery)//执行sql

    { 

    }


    public virtual SqlInfo ReadFullTable(string tableName)//读取整个表

    {
        return null;

    }



    public virtual SqlInfo InsertInto(string tableName, string[] values)//在表中插入数据

    {
        return null;

    }



    public virtual SqlInfo UpdateInto(string tableName, string[] cols, string colsValues, string selectKey, string selectValue)//替换表中数据

    {
        return null;

    }



    public virtual SqlInfo Delete(string tableName, string[] cols, string[] colsvalues)//删除表中数据

    {
        return null;

    }




    public virtual SqlInfo DeleteContents(string tableName)//删除表

    {
        return null;

    }



    //select count(*)  from sqlite_master where type='table' and name = 'yourtablename';
    public virtual bool IsExitTable(string name)//创建表

    {
        bool ret = false;

        return ret;
    }
    public virtual SqlInfo CreateTable(string name, string[] col, string[] colType)//创建表

    {
        return null;



    }



    public virtual SqlInfo SelectWhere(string tableName, string[] items, string[] col, string[] operation, string[] values)//集成所有操作后执行

    {
        return null;


    }

    public virtual bool MoveToFirst(SqlInfo info)
    {
        return false;
    }
    public virtual bool MoveToNext(SqlInfo info)
    {
        return false;
    }
    public virtual string GetString(SqlInfo info, string key)
    {
        return "";
    }
    public virtual int GetCount(SqlInfo info)
    {
        return 0;
    }

}
