#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
using System.Collections;
 
internal class SqlAndroidWrapper : SqlBasePlatformWrapper
	{

	public const string JAVA_CLASS = "com.moonma.common.DBInterfaceUnity";

    AndroidJavaClass javaClass;
     public	AndroidJavaObject objDb; 

public  void Init( ) 
    {
             if(javaClass==null)
		{ 
		    javaClass = new AndroidJavaClass(JAVA_CLASS);
		}
    }

    public override void CopyFromAsset(string dbfile) 
    {
       Init();
         if(javaClass!=null)
		{ 
		    javaClass.CallStatic("CopyFromAsset",dbfile);
		}
    }
	public override void Open(string dbfile)
    {   
       Init();

		if(javaClass!=null)
		{ 
		   objDb = javaClass.CallStatic<AndroidJavaObject>("OpenDB",dbfile);
		}

    }

    public override void Close() 

    {
        if(javaClass!=null)
		{ 
		    javaClass.CallStatic("CloseDB");
		}
    }


     public override int GetCount(SqlInfo info)
    { 
        return  info.obj.Call<int>("getCount");   
    }
 public override string GetString(SqlInfo info,string key)
    {
        int idx = info.obj.Call<int>("getColumnIndex",key);  
        return  info.obj.Call<string>("getString",idx);   
    }

       public override bool MoveToFirst(SqlInfo info)
    { 
        if(info.obj==null){
             Debug.Log("MoveToFirst obj is null");
             return false;
        }
        return  info.obj.Call<bool>("moveToFirst");   
    }
     public override bool MoveToNext(SqlInfo info)
    { 
           if(info.obj==null){
             Debug.Log("MoveToNext obj is null");
             return false;
        }
        return  info.obj.Call<bool>("moveToNext");   
    }
 public override void ExecSQL(string sql)
    {
             javaClass.CallStatic("ExecSQL",objDb,sql);
    }
   public override SqlInfo Query(string sql)
    {
        //   if(javaClass==null)
		// { 
		//     javaClass = new AndroidJavaClass(JAVA_CLASS);
		// }
          Debug.Log("SqlAndroidWrapper:Query sql="+sql);
        SqlInfo info = new SqlInfo(); 
				{  
					//return Cursor
				   AndroidJavaObject obj = javaClass.CallStatic<AndroidJavaObject>("Query",objDb,sql);

                    string strtest = javaClass.CallStatic<string>("QueryTest",sql); 
                    Debug.Log("Query AndroidJavaObject strtest="+strtest);

//Call 不执行java
                       string strtest2 = javaClass.Call<string>("QueryTest2",sql); 
                    Debug.Log("Query AndroidJavaObject strtest2="+strtest2);

                   info.obj = obj;
                       if(info.obj==null){
                          Debug.Log("Query AndroidJavaObject obj is null");
                }else
                {
                      Debug.Log("SqlAndroidWrapper:Query end");
                }
                //    if(obj.Call<bool>("moveToFirst"))
                //    {

                //    }
				   /*
				    if (cr.moveToFirst()) {
                    for (int i = 0; i < cr.getCount(); i++) {
                        //cr.getString();
                        FaceInfo info = new FaceInfo();
                        info.id = cr.getString(cr.getColumnIndex("id"));
                        info.title = cr.getString(cr.getColumnIndex("title"));
                        Log.d(TAG, "Seach result: title="+info.title+" id="+info.id);
                        listItem.add(info);
                        cr.moveToNext();
                    }
                }
				*/

				}
        return info;
    }


    }
#endif

