using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using SimpleSQL;

public class DBToolSimpleSQL
{
    public SimpleSQLManager dbManager;

    public void OpenDB(string dbfile)

    {
        string connectionString = "Data Source=" + dbfile;
        // if (db == null)
        // {
        //     db = new SQLiteDB();
        // }


        // string filename = dbfile;


        // try
        // {
        //     //
        //     // initialize database
        //     //
        //     db.Open(filename);

        //     //	Test(db, ref log);

        // }
        // catch (Exception e)
        // {
        //     //log += 	"\nTest Fail with Exception " + e.ToString();
        //     //log += 	"\n on WebPlayer it must give an exception, it's normal.";
        // }

    }



    public void CloseDB()//关闭数据库连接

    {
        // if (db != null)
        // {
        //     db.Close();
        // }
    }


    public SQLiteQuery ExecuteQuery(string sqlQuery, bool isStep)//执行查询

    {
        Debug.Log("ExecuteQuery::" + sqlQuery);
        // // dbCommand = dbConnection.CreateCommand();

        // // dbCommand.CommandText = sqlQuery;

        // // dbReader = dbCommand.ExecuteReader();

        // // return dbReader;

        // SQLiteQuery qr = new SQLiteQuery(db, sqlQuery);
        // if(isStep){
        //      qr.Step();
        // }

        // //qr.Release();
        // return qr;
        dbManager.Execute(sqlQuery);

        

        return null;

    }



    public SQLiteQuery ReadFullTable(string tableName)//读取整个表

    {

        string query = "SELECT * FROM " + tableName + ";";

        return ExecuteQuery(query, false);

    }



    public SQLiteQuery InsertInto(string tableName, string[] values)//在表中插入数据

    {

        string query = "INSERT INTO " + tableName + " VALUES('" + values[0];

        for (int i = 1; i < values.Length; i++)

        {

            query += "','" + values[i];

        }

        query += "')";

        return ExecuteQuery(query, true);

    }



    public SQLiteQuery UpdateInto(string tableName, string[] cols, string colsValues, string selectKey, string selectValue)//替换表中数据

    {

        string query = "UPDATE " + tableName + " SET " + cols[0] + " = " + colsValues[0];



        for (int i = 1; i < colsValues.Length; ++i)

        {



            query += ", " + cols[i] + " =" + colsValues[i];

        }



        query += " WHERE " + selectKey + " = " + selectValue + " ";

        return ExecuteQuery(query, true);

    }



    public SQLiteQuery Delete(string tableName, string[] cols, string[] colsvalues)//删除表中数据

    {

        string query = "DELETE FROM " + tableName + " WHERE " + cols[0] + " = " + colsvalues[0];



        for (int i = 1; i < colsvalues.Length; ++i)

        {



            query += " or " + cols[i] + " = " + colsvalues[i];

        }

        return ExecuteQuery(query, true);

    }




    public SQLiteQuery DeleteContents(string tableName)//删除表

    {

        string query = "DELETE FROM " + tableName;

        return ExecuteQuery(query, true);

    }



    //select count(*)  from sqlite_master where type='table' and name = 'yourtablename';
    public bool IsExitTable(string name)//创建表

    {
        bool ret = false;
        //string query = "select count(*)  from sqlite_master where type='table' and name = '" + name + "'";
        string query = "select * from sqlite_master where type='table' and name = '" + name + "'";
        SQLiteQuery rd = ExecuteQuery(query, false);
        //int count = rd.GetCount();//rd.GetInteger("0");
        int count = 0;
        while (rd.Step())
        {
            count++;
        }

        Debug.Log("IsExitTable:count=" + count);
        if (count > 0)
        {
            ret = true;
        }
        rd.Release();
        return ret;
    }
    public SQLiteQuery CreateTable(string name, string[] col, string[] colType)//创建表

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



        return ExecuteQuery(query, true);



    }



    public SQLiteQuery SelectWhere(string tableName, string[] items, string[] col, string[] operation, string[] values)//集成所有操作后执行

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
