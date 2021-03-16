using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WordPointInfo
{
    public List<string> listStroke;


    // List<Vector2>
    public List<object> listMedian;
}

public class WordItemInfo : ItemInfo
{
    public List<object> listDemoPoint;//演示笔画的坐标点
    public List<object> listGuidePoint;//笔画提示图片坐标

    public List<string> listImageBihua0;//笔画图片0
    public List<string> listImageBihua1;//笔画图片1

    public string soundPutonghua;
    public string soundGuangdonghua;
    public int countBihua;
    public string imageLetter;
    public string thumbLetter;
    public string imageBihua;
    public int lineWidth;//基于图片大小 如 45

    public string fileSaveWord;
    public string date;
    public string addtime;

public string wordCode;

    //point
    public WordPointInfo pointInfo;


    //DB
    public DBWordItemInfo dbInfo;
    // public string word;
    // public string pinyin;//拼音
    // public string zuci;//组词
    // public string bushou;//部首
    // public string bihua;//笔画
    // public string audio;
    // public string gif;
    // public string mean;
    public WordItemInfo()
    {
        dbInfo = new DBWordItemInfo();
    }


}


public class GuideItemInfo
{
    public const int IMAGE_TYPE_START = 0;
    public const int IMAGE_TYPE_MIDDLE_ANIMATE = 3;
    public const int IMAGE_TYPE_MIDDLE = 1;
    public const int IMAGE_TYPE_END = 2;

    public float angle;
    public Vector2 point;
    public int type;
    public int count;
    public int direction;
}
