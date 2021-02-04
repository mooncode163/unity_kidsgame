using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using UnityEditor;
using System.Text;
public class EditorConvertIdiomData : UIView
{
    // Use this for initialization
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {


    }

    [MenuItem("CaiCaiLe/ConvertIdiomData")]
    static void OnConvertIdiomData()
    {
        Debug.Log("OnConvertIdiomData start");
        string filepath = CloudRes.main.rootPathGameRes + "/guanka/wordInfo.json";
        string json = FileUtil.ReadStringAsset(filepath);
        JsonData root = JsonMapper.ToObject(json);
        int count = root.Count;
        //count = 1;
        Debug.Log("OnConvertIdiomData count=" + count);
        //JsonData items = root["items"];
        for (int i = 0; i < count; i++)
        {
            JsonData item = root[i];
            CaiCaiLeItemInfo info = new CaiCaiLeItemInfo();
            info.pinyin = (string)item["pron"];
            info.title = (string)item["word"];
            info.album = (string)item["source"];
            info.translation = (string)item["expl"];
            string filesave = Application.streamingAssetsPath + "/" + CloudRes.main.rootPathGameRes + "/guanka/data/" + info.title + ".json";
            if (!FileUtil.FileIsExistAsset(filesave))
            {
                SaveJson(filesave, info);
            }
        }

        Debug.Log("OnConvertIdiomData end");
    }


    static void SaveJson(string filepath, CaiCaiLeItemInfo info)
    {
        //save guanka json

        Hashtable data = new Hashtable();
        data["title"] = info.title;
        data["pinyin"] = info.pinyin;
        data["album"] = info.album;
        data["translation"] = info.translation;

        string strJson = JsonMapper.ToJson(data);
        // Debug.Log(strJson);
        byte[] bytes = Encoding.UTF8.GetBytes(strJson);
        System.IO.File.WriteAllBytes(filepath, bytes);

        // string filesave = CloudRes.main.rootPathGameRes + "/guanka/data/" + info.title + ".json";
        // string json = FileUtil.ReadStringAsset(filesave);
        // JsonData root = JsonMapper.ToObject(json);
        // string title = (string)root["title"];
        // Debug.Log("title=" + title);
    }
 
}
