﻿
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
        // switch (gameId)
        // {
        //     case GameRes.GAME_Word:
        //         {
        //             dBLove = DBLoveWord.main;
        //         }
        //         break;


        // }
        dBLove = DBLoveWord.main;
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

            {
                List<DBWordItemInfo> ls = GetAllItem<DBWordItemInfo>();
                foreach (DBWordItemInfo info in ls)
                {
                    WordItemInfo infocaicaile = new WordItemInfo();
                    infocaicaile.id = info.id;
                    infocaicaile.dbInfo = info;
                    list.Add(infocaicaile);
                }
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

