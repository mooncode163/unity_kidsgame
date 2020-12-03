
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class ConfigPrefabInternal
{
    //color
    //f88816 248,136,22
    JsonData rootJson;

    public void Init(string filePath)
    {
        if (!FileUtil.FileIsExist(filePath))
        {
            return;
        }
        string json = FileUtil.ReadStringAuto(filePath);
        rootJson = JsonMapper.ToObject(json);
    }


    public bool IsHasKey(string key)
    {
        if (rootJson == null)
        {
            return false;
        }
        return JsonUtil.ContainsKey(rootJson, key);
    }

    public string GetPrefab(string key)
    {
        if (rootJson == null)
        {
            return "Not Find Prefab key="+key;
        }
        return JsonUtil.GetString(rootJson, key, "");
    }
}
