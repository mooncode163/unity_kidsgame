
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class ColorConfig
{
    //color
    //f88816 248,136,22
    JsonData rootJson;
    static private ColorConfig _main = null;
    public static ColorConfig main
    {
        get
        {
            if (_main == null)
            {
                _main = new ColorConfig();
                string filePath = Common.RES_CONFIG_DATA + "/Color/Color.json";
                _main.Init(filePath);
            }
            return _main;
        }
    }

    void Init(string filePath)
    {
        string json = FileUtil.ReadStringAuto(filePath);
        rootJson = JsonMapper.ToObject(json);
    }

    public Color GetColor(string key)
    {
        return GetColor(key, Color.black);
    }


    public Color GetColor(string key, Color def)
    {
        Color cr = def;
        if (JsonUtil.ContainsKey(rootJson, key))
        {
            string str = (string)rootJson[key];
            cr = Common.RGBString2ColorA(str);
        }
        else
        {
            Debug.Log("ColorConfig ContainsKey no key =" + key);
        }
        return cr;
    }
}
