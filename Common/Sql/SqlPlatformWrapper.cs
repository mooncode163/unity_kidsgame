using UnityEngine;
using System.Collections;


public class SqlBasePlatformWrapper
{
    public virtual void Open(string dbfile)
    {


    }

    public virtual void Close()//关闭数据库连接

    {

    }

      public virtual void CopyFromAsset(string dbfile) 
    {
    }
    public virtual SqlInfo Query(string sql)
    {
        return null;
    }
  public virtual void ExecSQL(string sql)
    {
         
    }
    
    public virtual string GetString(SqlInfo info, string key)
    {
        return "";
    }
    public virtual int GetCount(SqlInfo info)
    {
        return 0;
    }
 
    public virtual bool MoveToFirst(SqlInfo info)
    {
        return false;
    }
    public virtual bool MoveToNext(SqlInfo info)
    {
        return false;
    }
    public virtual void ReadFullTable(string tableName)//读取整个表

    {

    }



    public void Insert(string tableName, string[] values)//在表中插入数据

    {


    }



    public virtual void Update(string tableName, string[] cols, string colsValues, string selectKey, string selectValue)//替换表中数据

    {


    }
    public virtual void Delete(string tableName, string[] cols, string[] colsvalues)//删除表中数据

    {


    }

    public virtual void DeleteTable(string tableName)//删除表

    {
        string query = "DELETE FROM " + tableName;
        Query(query);

    }



    //select count(*)  from sqlite_master where type='table' and name = 'yourtablename';
    public virtual bool IsExitTable(string name)//创建表

    {
        bool ret = false;

        return ret;
    }
    public virtual void CreateTable(string name, string[] col, string[] colType)//创建表

    {


    }


    public virtual void SelectWhere(string tableName, string[] items, string[] col, string[] operation, string[] values)//集成所有操作后执行

    {


    }
}
