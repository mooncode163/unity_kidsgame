
using UnityEngine;
using System.Collections;
using Tacticsoft;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;
using LitJson;
//必应美图
//https://bing.lylares.com/
//code:https://github.com/lylares/bingima

public class ImageItemInfo : ItemInfo//, ISysImageLibDelegate
{
    public string cid;//频道
}

public interface INetImageParseDelegate
{
    void OnNetImageParseDidParseSortList(NetImageParseBase parse, bool isSuccess, List<object> list);
    void OnNetImageParseDidParseImageList(NetImageParseBase parse, bool isSuccess, List<object> list);
}
public class NetImageParseCommon
{


    NetImageParseBase netImageParse;
    HttpRequest httpSortList;
    HttpRequest httpImageList;


    static private NetImageParseCommon _main = null;
    public static NetImageParseCommon main
    {
        get
        {
            if (_main == null)
            {
                _main = new NetImageParseCommon();
            }
            return _main;
        }
    }
    public float GetImageRatio()
    {
        float ret = 16.0f / 9;
        if (netImageParse != null)
        {
            ret = netImageParse.GetImageRatio();
        }
        return ret;
    }
    public void CreateAPI(string source)
    {
        if (source == Source.QIHU_360)
        {
            netImageParse = new NetImageParse360();
        }
    }
    public void SetDelegate(INetImageParseDelegate dg)
    {
        if (netImageParse != null)
        {
            netImageParse.iDelegate = dg;
        }
    }

    public void StartParseSortList()
    {
        if (netImageParse != null)
        {
            netImageParse.StartParseSortList();
        }
    }

    public void StartParseImageList(ImageItemInfo info)
    {
        if (netImageParse != null)
        {
            netImageParse.StartParseImageList(info);
        }
    }


}

