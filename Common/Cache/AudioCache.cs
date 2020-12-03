

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCache
{
    Dictionary<string, AudioClip> dicItem;

    static private AudioCache _main = null;
    public static AudioCache main
    {
        get
        {
            if (_main == null)
            {
                _main = new AudioCache();
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
        dicItem = new Dictionary<string, AudioClip>();
    }

    public AudioClip Load(string filepath)
    { 
        AudioClip audio = null;
        string key = filepath;
        if (dicItem.ContainsKey(key))
        {
            audio = dicItem[key];
        }
        else
        {
            audio = (AudioClip)Resources.Load(key);
            if (audio != null)
            {
                dicItem.Add(key, audio);
            }
        }
        return audio;
    }

    public void DestoryAllItem()
    {
        foreach (KeyValuePair<string, AudioClip> item in dicItem)
        {
            string key = item.Key;
            AudioClip obj = item.Value;
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
        AudioClip obj = dicItem[key];
        if (obj != null)
        {
            GameObject.DestroyImmediate(obj);
            obj = null;
        }
        dicItem.Remove(key);
    }
}
