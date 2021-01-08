using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
public class ParseWordPointInfo
{
     // const CHARACTER_BOUNDS = [{x: 0, y: -124}, {x: 1024, y: 900}];
     public const int medianDataOffsetY = 124;

    static private ParseWordPointInfo _main = null;
    public static ParseWordPointInfo main
    {
        get
        {
            if (_main == null)
            {
                _main = new ParseWordPointInfo();
            }
            return _main;
        }
    }

    public WordPointInfo ParseWordJson(string word)
    {
        int count = 0;
        int idx = LevelManager.main.placeLevel;
        long tickJson = Common.GetCurrentTimeMs();
        string filepath = CloudRes.main.rootPathGameRes +"/guanka/word/" + word + ".json";
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(filepath);//((TextAsset)Resources.Load(fileName, typeof(TextAsset))).text;
        WordPointInfo info = new WordPointInfo();
        info.listStroke = new List<string>();
        info.listMedian = new List<object>();

        JsonData root = JsonMapper.ToObject(json);
        JsonData strokes = root["strokes"];
        for (int i = 0; i < strokes.Count; i++)
        {
            string str = (string)strokes[i];
            info.listStroke.Add(str);
        }

        JsonData medians = root["medians"];
        for (int i = 0; i < medians.Count; i++)
        {
            JsonData list = (JsonData)medians[i];
            List<Vector2> listPoint = new List<Vector2>();
            for (int j = 0; j < list.Count; j++)
            {
                JsonData arrayPoint = (JsonData)list[j];
                float x = (int)arrayPoint[0];
                float y = (int)arrayPoint[1];
                Vector2 pt = new Vector2(x, y);
                listPoint.Add(pt);

            }

            info.listMedian.Add(listPoint);
        }

        return info;
    }





    // 不包含indexEnd
    // var strpath = "M 300 793  L 317 780   L 338 747  L 308 679 L 201 496  L 136 410 L 83 355 "
    public string Point2SVGPath(WordPointInfo info, int indexStroke, int indexStart, int indexEnd)
    {
        int count =  info.listMedian.Count;
        Debug.Log("Point2SVGPath count ="+count);
        List<Vector2> listPoint = info.listMedian[indexStroke] as List<Vector2>;
        string strpath = "";

        for (int j = 0; j < listPoint.Count; j++)
        {
            Vector2 pt = (Vector2)listPoint[j];

            if (j >= indexEnd)
            {
                break;
            }
            if (j == indexStart)
            {
                strpath = "M ";
            }
            else if (j > indexStart)
            {
                strpath += "L ";
            }
            strpath += pt.x.ToString() + " ";
            strpath += pt.y.ToString() + " ";
        }
        // console.log(strpath) 
        return strpath;
    }


    // stroke="rgba(170,170,255,1)"
    public string Color2SVGColor(Color color)
    {
        string ret = "";
        int r, g, b;
        r = (int)(color.r * 255);
        g = (int)(color.g * 255);
        b = (int)(color.b * 255);
        // ret = "rgba(" + r.ToString() + "," + g.ToString() + "," + b.ToString() + ",1)";
        ret="#"+r.ToString("X")+g.ToString("X")+b.ToString("X");
        return ret;
    }


    public string GetWordSVG(WordPointInfo info, int idx)
    {
        // _PATH_
        string filepath = CloudRes.main.rootPathGameRes +"/svg/word.svg";
        string strsvg = FileUtil.ReadStringAsset(filepath);
        string strpath = info.listStroke[idx];
        strsvg = strsvg.Replace("_PATH_", strpath);
        return strsvg;
    }

    // idx 笔画

    public string GetWordMedianSVG(WordPointInfo info, int idx, int indexStart, int indexEnd, Color color, int width)
    {
        /*
                  <g transform="translate(0, 450) scale(0.5, -0.5)">
                        <!-- <path d="M 220.6 853.7 L 317 780 L 338 747 L 308 679 L 201 496 L 136 410 L 83 355" stroke="rgba(221,221,221,1)" stroke-width="200" fill="none" stroke-linecap="round" stroke-linejoin="miter" stroke-dasharray="631.0069822069698,631.0069822069698" style="opacity: 1; stroke-dashoffset: 0;"></path> -->
                        <path d="_PATH_" stroke="_COLOR_" stroke-width="_WIDTH_" stroke-linecap="round" stroke-linejoin="miter"></path>
                </g>
        */
        string filepath = CloudRes.main.rootPathGameRes +"/svg/stroke.svg";
        string strsvg = FileUtil.ReadStringAsset(filepath);
        string strpath = Point2SVGPath(info, idx, indexStart, indexEnd);
        strsvg = strsvg.Replace("_PATH_", strpath);
        strsvg = strsvg.Replace("_WIDTH_", width.ToString());
        strsvg = strsvg.Replace("_COLOR_", Color2SVGColor(color));
        return strsvg;
    }

    public int GetWordMedianPointCount(WordPointInfo info, int indexStroke)
    {
        List<Vector2> listPoint = info.listMedian[indexStroke] as List<Vector2>;
        return listPoint.Count;
    }
}
