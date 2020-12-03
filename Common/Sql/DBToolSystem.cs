using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class DBToolSystem : DBToolBase
{
    private Sql db = null;

    public override void CopyFromAsset(string dbfile)

    {
        if (db == null)
        {
            db = new Sql();
        } 
        db.CopyFromAsset(dbfile);
    }
    public override void OpenDB(string dbfile)

    { 
        if (db == null)
        {
            db = new Sql();
        } 
        db.Open(dbfile);

    }



    public override void CloseDB()//关闭数据库连接

    {
        if (db != null)
        {
            db.Close();
        }
    }


    public override SqlInfo ExecuteQuery(string sqlQuery, bool isStep)//执行查询

    {
        Debug.Log("ExecuteQuery::" + sqlQuery);
        SqlInfo qr = db.Query(sqlQuery);
        return qr;
    }

    public override void ExecSQL(string sqlQuery)//执行sql

    {
        Debug.Log("ExecSQL::" + sqlQuery);
        db.ExecSQL(sqlQuery);

    }



    public override SqlInfo ReadFullTable(string tableName)//读取整个表

    {

        string query = "SELECT * FROM " + tableName + ";";

        return ExecuteQuery(query, false);

    }



    public override SqlInfo InsertInto(string tableName, string[] values)//在表中插入数据

    {

        string query = "INSERT INTO " + tableName + " VALUES('" + values[0];

        for (int i = 1; i < values.Length; i++)

        {

            query += "','" + values[i];

        }

        query += "')";

        ExecSQL(query);
        return null;

    }



    public override SqlInfo UpdateInto(string tableName, string[] cols, string colsValues, string selectKey, string selectValue)//替换表中数据

    {

        string query = "UPDATE " + tableName + " SET " + cols[0] + " = " + colsValues[0];



        for (int i = 1; i < colsValues.Length; ++i)

        {



            query += ", " + cols[i] + " =" + colsValues[i];

        }



        query += " WHERE " + selectKey + " = " + selectValue + " ";

        return ExecuteQuery(query, true);

    }



    public override SqlInfo Delete(string tableName, string[] cols, string[] colsvalues)//删除表中数据

    {

        string query = "DELETE FROM " + tableName + " WHERE " + cols[0] + " = " + colsvalues[0];



        for (int i = 1; i < colsvalues.Length; ++i)

        {



            query += " or " + cols[i] + " = " + colsvalues[i];

        }

        ExecSQL(query);
        return null;

    }




    public override SqlInfo DeleteContents(string tableName)//删除表

    {
        string query = "DELETE FROM " + tableName;
        ExecSQL(query);
        return null;

    }
    public override string GetString(SqlInfo info, string key)
    {
        return db.GetString(info, key);
    }
    public override int GetCount(SqlInfo info)
    {
        return db.GetCount(info);
    }
    public override bool MoveToFirst(SqlInfo info)
    {
        return db.MoveToFirst(info);
    }
    public override bool MoveToNext(SqlInfo info)
    {
        return db.MoveToNext(info);
    }

    //select count(*)  from sqlite_master where type='table' and name = 'yourtablename';
    public override bool IsExitTable(string name)//创建表

    {
        bool ret = false;
        //string query = "select count(*)  from sqlite_master where type='table' and name = '" + name + "'";
        string query = "select * from sqlite_master where type='table' and name = '" + name + "'";
        SqlInfo rd = ExecuteQuery(query, false);
        //int count = rd.GetCount();//rd.GetInteger("0");
        int count = 0;
        while (db.MoveToNext(rd))
        {
            count++;
        }

        Debug.Log("IsExitTable:count=" + count);
        if (count > 0)
        {
            ret = true;
        }
        return ret;
    }
    public override SqlInfo CreateTable(string name, string[] col, string[] colType)//创建表

    {

        if (col.Length != colType.Length)
        {

            return null;
            //throw new SqliteException("columns.Length != colType.Length");



        }



        string query = "CREATE TABLE " + name + " (" + col[0] + " " + colType[0];


        for (int i = 1; i < col.Length; ++i)
        {



            query += ", " + col[i] + " " + colType[i];



        }



        query += ")";



        // return ExecuteQuery(query, true);
        ExecSQL(query);
        return null;


    }



    public override SqlInfo SelectWhere(string tableName, string[] items, string[] col, string[] operation, string[] values)//集成所有操作后执行

    {

        if (col.Length != operation.Length || operation.Length != values.Length)
        {
            return null;


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



        return ExecuteQuery(query, false);



    }




}
