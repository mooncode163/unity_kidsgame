using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;
using UnityEngine;

public class JsonUtil : MonoBehaviour
{

    static public string GetString(JsonData data, string key, string strdefault)
    {
        return JsonGetString(data, key, strdefault);
    }
    static public bool GetBool(JsonData data, string key, bool _default)
    {
        return JsonGetBool(data, key, _default);
    }

    static public JsonData GetJsonData(JsonData data, string key)
    {
        if (ContainsKey(data, key))
        {
            return data[key];
        }
        return null;
    }
    static public string JsonGetString(JsonData data, string key, string strdefault)
    {
        string ret = strdefault;
        if (data == null)
        {
            return ret;
        }
        if (ContainsKey(data, key))
        {
            ret = (string)data[key];
        }
        return ret;
    }

    static public bool JsonGetBool(JsonData data, string key, bool _default)
    {
        bool ret = _default;
        if (data == null)
        {
            return ret;
        }
        if (ContainsKey(data, key))
        {
            ret = (bool)data[key];
        }
        return ret;
    }

    static public int GetInt(JsonData data, string key, int _default)
    {
        int ret = _default;
        if (data == null)
        {
            return ret;
        }
        if (ContainsKey(data, key))
        {
            ret = (int)data[key];
        }
        return ret;
    }

    static public bool ContainsKey(JsonData data, string key)
    {
        bool result = false;
        if (data == null)
            return result;
        if (!data.IsObject)
        {
            return result;
        }
        IDictionary tdictionary = data as IDictionary;
        if (tdictionary == null)
            return result;
        if (tdictionary.Contains(key))
        {
            result = true;
        }
        return result;
    }





}
