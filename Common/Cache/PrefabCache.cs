

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabCache
{
    Dictionary<string, GameObject> dicItem;

    static private PrefabCache _main = null;
    public static PrefabCache main
    {
        get
        {
            if (_main == null)
            {
                _main = new PrefabCache();
                _main.Init();
            }
            return _main;
        }
    }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Init()
    {
        dicItem = new Dictionary<string, GameObject>();
    }


    public GameObject LoadByKey(string key)
    {
        return Load(ConfigPrefab.main.GetPrefab(key));
    }
    public GameObject Load(string filepath)
    {
        GameObject obj = null;
        string key = filepath.Replace(".prefab","");
        if (dicItem.ContainsKey(key))
        {
            obj = dicItem[key];
        }
        else
        {
            obj = (GameObject)Resources.Load(key);
            if (obj != null)
            {
                dicItem.Add(key, obj);
            }
        }
        return obj;
    }

    public void DestoryAllItem()
    {
        foreach (KeyValuePair<string, GameObject> item in dicItem)
        {
            string key = item.Key;
            GameObject obj = item.Value;
            if (obj != null)
            {
                GameObject.DestroyImmediate(obj);
                obj = null;
            }
        }
        dicItem.Clear();
    }

    public void DeleteItem(string key)
    {
        if (!dicItem.ContainsKey(key))
        {
            return;
        }
        GameObject obj = dicItem[key];
        if (obj != null)
        {
            GameObject.DestroyImmediate(obj);
            obj = null;
        }
        dicItem.Remove(key);
    }
}
