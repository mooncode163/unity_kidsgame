
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class ConfigPrefab
{
    //color
    //f88816 248,136,22
    JsonData rootJson;

    ConfigPrefabInternal prefabApp;
    ConfigPrefabInternal prefabAppCommon;
    ConfigPrefabInternal prefabCommon;
    static private ConfigPrefab _main = null;
    public static ConfigPrefab main
    {
        get
        {
            if (_main == null)
            {
                _main = new ConfigPrefab();

                _main.prefabCommon = new ConfigPrefabInternal();
                // _main.prefabCommon.Init(Resource.dirScript + "/Common/Resources/Common/Prefab/ConfigPrefab.json");
                                _main.prefabCommon.Init("Common/Prefab/ConfigPrefab.json");
                
                _main.prefabAppCommon = new ConfigPrefabInternal();
                _main.prefabAppCommon.Init(Common.RES_CONFIG_DATA + "/Prefab/ConfigPrefabAppCommon.json");

                _main.prefabApp = new ConfigPrefabInternal();
                _main.prefabApp.Init(Common.RES_CONFIG_DATA + "/Prefab/ConfigPrefabApp.json");

            }
            return _main;
        }
    }
    public string GetPrefab(string key)
    {
        string ret = "";
        if (prefabApp.IsHasKey(key))
        {
            ret = prefabApp.GetPrefab(key);
        }
        if (prefabAppCommon.IsHasKey(key))
        {
            ret = prefabAppCommon.GetPrefab(key);
        }
        if (prefabCommon.IsHasKey(key))
        {
            ret = prefabCommon.GetPrefab(key);
        }
        return ret;
    }



}
