
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DBLove
{
    DBLoveBase dBLove;
    string gameId;
    static DBLove _main = null;
    public static DBLove main
    {
        get
        {
            if (_main == null)
            {
                _main = new DBLove();
                _main.Init();
            }
            return _main;
        }


    }

    public void Init()
    {

    }

    public void GetDB(string id)
    {
        gameId = id;
        switch (gameId)
        {
            case GameRes.GAME_IDIOM:
                {
                    dBLove = DBLoveIdiom.main;
                }
                break;
            case GameRes.GAME_RIDDLE:
                {
                    dBLove = DBLoveRiddle.main;
                }
                break;
            case GameRes.GAME_XIEHOUYU:
                {
                    dBLove = DBLoveXiehouyu.main;
                }
                break;

        }

    }

    public List<T> GetAllItem<T>() where T : new()
    {
        if (dBLove != null)
        {
            return dBLove.GetAllItem<T>();
        }
        return null;
    }

    public void GetAllItem(List<object> list)
    {

        if (dBLove != null)
        {
            switch (gameId)
            {
                case GameRes.GAME_IDIOM:
                    {

                        List<IdiomItemInfo> ls = GetAllItem<IdiomItemInfo>();
                        foreach (IdiomItemInfo info in ls)
                        {
                            CaiCaiLeItemInfo infocaicaile = new CaiCaiLeItemInfo();

                            infocaicaile.title = info.title;
                            infocaicaile.id = info.id;
                            Debug.Log("UpdateList info.id=" + info.id + " info.title=" + info.title);
                            infocaicaile.dbInfo = info;
                            list.Add(infocaicaile);

                        }
                    }
                    break;
                case GameRes.GAME_RIDDLE:
                    {
                        List<DBInfoRiddle> ls = GetAllItem<DBInfoRiddle>();
                        foreach (DBInfoRiddle info in ls)
                        {
                            CaiCaiLeItemInfo infocaicaile = new CaiCaiLeItemInfo();
                            infocaicaile.dbInfo = info;
                            list.Add(infocaicaile);
                        }
                    }
                    break;
                case GameRes.GAME_XIEHOUYU:
                    {
                        List<DBInfoXiehouyu> ls = GetAllItem<DBInfoXiehouyu>();
                        foreach (DBInfoXiehouyu info in ls)
                        {
                            CaiCaiLeItemInfo infocaicaile = new CaiCaiLeItemInfo();
                            infocaicaile.dbInfo = info;
                            list.Add(infocaicaile);
                        }
                    }
                    break;

            }

        }
    }

    public void AddItem(DBItemInfoBase info)
    {
        if (dBLove != null)
        {
            dBLove.AddItem(info);
        }
    }

    public void ReadInfo(DBItemInfoBase info, SqlInfo infosql)
    {

        if (dBLove != null)
        {
            dBLove.ReadInfo(info, infosql);
        }
    }



    public void DeleteItem(DBItemInfoBase info)
    {

        if (dBLove != null)
        {
            dBLove.DeleteItem(info);
        }
    }
    public bool IsItemExist(DBItemInfoBase info)
    {
        if (dBLove != null)
        {
            return dBLove.IsItemExist(info);
        }
        return false;
    }

    public void ClearDB()
    {
        if (dBLove != null)
        {
            dBLove.ClearDB();
        }
    }
    public bool DBEmpty()
    {
        bool ret = false;
        if (dBLove != null)
        {
            return dBLove.DBEmpty();
        }
        return ret;
    }
}

