
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class ImageResInternal
{
    public const string KEY_BOARD = "board";
    public const string KEY_PATH = "path";
    public bool isOldVersion;
    //color
    //f88816 248,136,22
    JsonData rootJson;
    // static private ImageResInternal _main = null;
    // public static ImageResInternal main
    // {
    //     get
    //     {
    //         if (_main == null)
    //         {
    //             _main = new ImageResInternal();
    //             // string filePath = Common.RES_CONFIG_DATA + "/Image/ImageRes.json";
    //             // _main.Init(filePath);
    //         }
    //         return _main;
    //     }
    // }

    public void Init(string filePath)
    {
        string json = FileUtil.ReadStringAuto(filePath);
        rootJson = JsonMapper.ToObject(json);
    }

    // 255,100,200,255 to color return Vector4 47,47,47,255
    //Vector4 (left,right,top,bottom)
    Vector4 String2Vec4(string str)
    {
        float x, y, z, w;
        string[] rgb = str.Split(',');
        x = Common.String2Int(rgb[0]);
        y = Common.String2Int(rgb[1]);
        z = Common.String2Int(rgb[2]);
        w = Common.String2Int(rgb[3]);
        return new Vector4(x, y, z, w);
    }
    string GetBoardKey(string key)
    {
        return key + "_BOARD";
    }

    public string FindKeyByPath(string path)
    {
        string str = "";
        if (isOldVersion)
        {
            if (rootJson != null)
            {
                foreach (string keytmp in rootJson.Keys)
                {
                    if ((string)rootJson[keytmp] == path)
                    {
                        str = keytmp;
                        break;
                    }
                }
            }
        }
        else
        {
            if (rootJson != null)
            {
                foreach (string keytmp in rootJson.Keys)
                {
                    if (JsonUtil.GetString(rootJson[keytmp], KEY_BOARD, "") == path)
                    {
                        str = keytmp;
                        break;
                    }
                }
            }
        }
        return str;
    }

    public string GetImageBoardString(string key)
    {
        string str = "";
        if (isOldVersion)
        {
            str = JsonUtil.GetString(rootJson, GetBoardKey(key), "");
        }
        else
        {
            str = JsonUtil.GetString(rootJson[key], KEY_BOARD, "");
        }
        return str;
    }

    //9宫格图片边框参数 (left,right,top,bottom)
    //cc.Vec4 (left,right,top,bottom)
    public Vector4 GetImageBoard(string key)
    {
        string str = "";
        if (isOldVersion)
        {
            str = JsonUtil.GetString(rootJson, GetBoardKey(key), "");
        }
        else
        {
            str = JsonUtil.GetString(rootJson[key], KEY_BOARD, "");
        }
        return String2Vec4(str);
    }

    public bool IsHasKey(string key)
    {
        return JsonUtil.ContainsKey(rootJson, key);
    }
    public bool IsHasBoard(string key)
    {
        if (isOldVersion)
        {
            return JsonUtil.ContainsKey(rootJson, GetBoardKey(key));
        }
        else
        {
            if (!IsHasKey(key))
            {
                return false;
            }
            return JsonUtil.ContainsKey(rootJson[key], KEY_BOARD);
        }

    }
    public string GetImage(string key)
    {
        if (JsonUtil.ContainsKey(rootJson, key))
        {
            if (isOldVersion)
            {
                return JsonUtil.GetString(rootJson, key, "");

            }
            else
            {
                return JsonUtil.GetString(rootJson[key], KEY_PATH, "");
            }
        }
        Debug.Log("GetImage _NO_KEY_ =" + key);
        // return "_NO_KEY_";
        return "";
    }
}
