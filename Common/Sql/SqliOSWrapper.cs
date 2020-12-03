#if UNITY_IPHONE && !UNITY_EDITOR
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections;
using System;
	internal class SqliOSWrapper : SqlBasePlatformWrapper
	{
		  
	[DllImport ("__Internal")]
	public static extern IntPtr Sql_Open(string dbfile); 
	[DllImport ("__Internal")]
	public static extern void Sql_Close(IntPtr p);
	[DllImport ("__Internal")]
	public static extern void Sql_ExecSQL(IntPtr p,string sql);
	[DllImport ("__Internal")]
	public static extern IntPtr Sql_Query(IntPtr p,string sql);//sqlite3_stmt * sqlite3_stmt * 
	[DllImport ("__Internal")]
	public static extern bool Sql_MoveToNext(IntPtr p);
	[DllImport ("__Internal")]
	public static extern bool Sql_MoveToFirst(IntPtr p);
	[DllImport ("__Internal")]
	public static extern int Sql_GetCount(IntPtr p);
	[DllImport ("__Internal")]
	public static extern string Sql_GetString(IntPtr p,string key);


    public	IntPtr dataBase; 

    public override void CopyFromAsset(string dbfile) 
    {
        
         
    }
	public override void Open(string dbfile)
    {   
       dataBase =  Sql_Open(dbfile);
    }

    public override void Close() 

    {
        Sql_Close(dataBase);
    }


     public override int GetCount(SqlInfo info)
    { 
    return Sql_GetCount(info.sqlite3_stmt);  
    }
 	public override string GetString(SqlInfo info,string key)
    {
            return Sql_GetString(info.sqlite3_stmt,key);  
    }

       public override bool MoveToFirst(SqlInfo info)
    { 
         return Sql_MoveToFirst(info.sqlite3_stmt);  
    }
     public override bool MoveToNext(SqlInfo info)
    { 
          return Sql_MoveToNext(info.sqlite3_stmt);   
    }
 	public override void ExecSQL(string sql)
    {
		Sql_ExecSQL(dataBase,sql);
    }
   public override SqlInfo Query(string sql)
    {
		SqlInfo info = new SqlInfo();  
       info.sqlite3_stmt = Sql_Query(dataBase,sql);
		 return info;
    }

	}

#endif