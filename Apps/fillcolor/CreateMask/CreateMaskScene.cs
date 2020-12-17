using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

public class ColorJsonItemInfo
{
    public string color;// 255,255,255
    public string rect;//0,0,100,100
}

public class GuankaColorJsonItemInfo
{
    //public string pic;//0,0,100,100
    public string id;
}

public class CreateMaskScene : MonoBehaviour
{
    public const string STR_PLACE = "vehicle2";
    public GameObject objSpritePic;
    public Texture2D texPic;
    public Button btnMask;
    public Button btnThumb;
    public Button btnGuanJson;
    ColorImage colorImage;

    int indexFillColor;
    long tickTotal;
    List<object> listGuanka;
    List<object> listColorFill;
    List<ColorJsonItemInfo> listColorJson;
    List<GuankaColorJsonItemInfo> listGuankaJson;
    List<Color32> listColor32Random;
    float diffH = 0.1f;//H偏差
    float diffS = 0.2f;
    float diffV = 0.2f;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        LevelManager.main.placeLevel = 0;//ng  8  11
        //ng 5.10 6.0  8all 10.0 10.5 10.11 10.14 11.0 11.1 11.2 11.3 11all #endregion
        listColorFill = new List<object>();
        listColorJson = new List<ColorJsonItemInfo>();
        long tick = Common.GetCurrentTimeMs();
        CreateRandomColorList();
        tick = Common.GetCurrentTimeMs() - tick;
        Debug.Log("CreateRandomColorList:" + tick + "ms");
        //  GameManager.placeLevel = 1;
        CleanGuankaList();
        ParseGuanka();


    }
    // Use this for initialization
    void Start()
    {
        ColorItemInfo info = GetItemInfo(0);
        colorImage = new ColorImage();


        // sp = LoadTexture.CreateSprieFromTex(texPic);
        // spRender.sprite = sp;
        tickTotal = Common.GetCurrentTimeMs();
        // DoFillColor();

        // CreateGuankaJsonFile();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreateGuankaJsonFile()
    {
        string strPlace = STR_PLACE;
        string path = Application.streamingAssetsPath + "/" + CloudRes.main.rootPathGameRes + "/image/" + strPlace + "/draw";
        string path_new = path + "_new";
        int width_save = 1024;
        int height_save = 768;
        //创建文件夹
        Directory.CreateDirectory(path_new);

        listGuankaJson = new List<GuankaColorJsonItemInfo>();
        // C#遍历指定文件夹中的所有文件 
        DirectoryInfo TheFolder = new DirectoryInfo(path);
        int idx = 0;
        // //遍历文件
        foreach (FileInfo NextFile in TheFolder.GetFiles())
        {
            string fullpath = NextFile.ToString();
            //1.jpg

            string ext = FileUtil.GetFileExt(fullpath).ToLower();
            //  Debug.Log("id="+idx.ToString()+" ext:"+ext);
            if ((ext == "png") || (ext == "jpg"))
            {
                Debug.Log("id=" + idx.ToString() + ":" + NextFile.Name);
                string name = strPlace + "_" + idx.ToString() + "." + ext;
                //name = NextFile.Name;
                string filepath_new = NextFile.ToString();
                //重命名
                filepath_new = path_new + "/" + name;
                //NextFile.CopyTo(filepath_new, true);

                GuankaColorJsonItemInfo info = new GuankaColorJsonItemInfo();
                //info.pic = NextFile.Name;
                info.id = FileUtil.GetFileName(name);
                listGuankaJson.Add(info);

                //fullpath = filepath_new;
                Texture2D tex = LoadTexture.LoadFromFile(fullpath);
                int w_save = 0, h_save = 0;
                float scale = 0, scalex = 0, scaley = 0;
                if (tex.width > tex.height)
                {
                    scalex = Mathf.Max(width_save, height_save) * 1.0f / tex.width;
                    scaley = Mathf.Min(width_save, height_save) * 1.0f / tex.height;
                }
                else
                {
                    scalex = Mathf.Min(width_save, height_save) * 1.0f / tex.width;
                    scaley = Mathf.Max(width_save, height_save) * 1.0f / tex.height;
                }
                scale = Mathf.Min(scalex, scaley);
                w_save = (int)(scale * tex.width);
                h_save = (int)(scale * tex.height);

                //统一图片大小
                //if (idx < 2)
                {
                    Texture2D texSave = TextureUtil.ConvertSize(tex, w_save, h_save);
                    FillBgWhiteToAlpha(texSave);
                    string dir = path_new;
                    string filepathSave = dir + "/" + info.id + ".png";
                    //FileUtil.CreateDir(dir);
                    byte[] bytes = texSave.EncodeToPNG();
                    System.IO.File.WriteAllBytes(filepathSave, bytes);
                }
                idx++;
            }

        }

        //save guanka json
        {

            Hashtable data = new Hashtable();
            data["place"] = strPlace;
            data["list"] = listGuankaJson;
            string strJson = JsonMapper.ToJson(data);
            //Debug.Log(strJson);
            string filepath = path_new + "/guanka.json";
            byte[] bytes = Encoding.UTF8.GetBytes(strJson);
            System.IO.File.WriteAllBytes(filepath, bytes);
        }

        Debug.Log("CreateGuankaJsonFile Finished");

    }

    ColorItemInfo GetItemInfo(int idx)
    {

        if (listGuanka == null)
        {
            return null;
        }
        if (idx >= listGuanka.Count)
        {
            return null;
        }
        ColorItemInfo info = listGuanka[idx] as ColorItemInfo;
        return info;
    }

    public void CleanGuankaList()
    {
        if (listGuanka != null)
        {
            listGuanka.Clear();
        }
        indexFillColor = 0;
    }
    public int ParseGuanka()
    {
        int count = 0;

        if ((listGuanka != null) && (listGuanka.Count != 0))
        {
            return listGuanka.Count;
        }

        listGuanka = new List<object>();
        int idx = LevelManager.main.placeLevel;
        ItemInfo infoPlace = LevelManager.main.GetPlaceItemInfo(idx);
        string fileName = CloudRes.main.rootPathGameRes + "/guanka/item_" + infoPlace.id + ".json";
        //FILE_PATH
        string json = FileUtil.ReadStringAsset(fileName);//((TextAsset)Resources.Load(fileName, typeof(TextAsset))).text;
        // Debug.Log("json::"+json);
        JsonData root = JsonMapper.ToObject(json);
        string strPlace = infoPlace.id;//(string)root["place"];
        JsonData items = root["items"];
        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];
            ColorItemInfo info = new ColorItemInfo();
            string strdir = CloudRes.main.rootPathGameRes + "/image/" + strPlace;
            info.id = (string)item["id"];

            info.id = (string)item["id"];
            info.pic = strdir + "/draw/" + info.id + ".png";
            if (!FileUtil.FileIsExist(info.pic))
            {
                info.pic = strdir + "/draw/" + info.id + ".jpg";
            }
            if (!FileUtil.FileIsExist(info.pic))
            {
                info.pic = strdir + "/draw/" + info.id + ".jpeg";
            }

            info.picmask = strdir + "/mask/" + info.id + ".png";
            info.colorJson = strdir + "/json/" + info.id + ".json";
            info.icon = strdir + "/thumb/" + info.id + ".png";
            // string filepath = GetFileSave(info);
            // info.fileSave = filepath;

            // string picname = (i + 1).ToString("d3");
            // info.pic = Common.GAME_RES_DIR + "/animal/draw/" + picname + ".png";
            // info.picmask = Common.GAME_RES_DIR + "/animal/mask/" + picname + ".png";
            // info.colorJson = Common.GAME_RES_DIR + "/animal/draw/" + picname + ".json";
            // info.icon = Common.GAME_RES_DIR + "/animal/thumb/" + picname + ".png";

            listGuanka.Add(info);

        }

        count = listGuanka.Count;

        // Debug.Log("ParseGame::count=" + count);
        return count;
    }

    void GotoNextPlace()
    {
        int total = LevelManager.main.ParsePlaceList().Count;
        LevelManager.main.placeLevel++;
        if (LevelManager.main.placeLevel >= total)
        {
            btnMask.gameObject.SetActive(true);
            Debug.Log("All Place Image has finished!");
            return;
        }
        CleanGuankaList();
        ParseGuanka();

        tickTotal = Common.GetCurrentTimeMs();
        DoFillColor();

    }
    //
    //Unity 为队伍设置不同颜色的shader: http://www.cnblogs.com/veboys/p/6944122.html
    //一般的，Hue十分接近，Saturation和Brightness的误差各不超过0.7的颜色，它们看起来是相似的颜色。
    void FormatTexture()
    {
        //RGB(255,106,89)
        //RGB(135,44,36)
        Color colorRed1 = new Color(255f / 255f, 106f / 255, 89f / 255, 1f);
        Color colorRed2 = new Color(135f / 255f, 44f / 255, 36f / 255, 1f);

        // colorRed1 = new Color(0f/255,0f/255,0f/255,1f);
        // colorRed2 = new Color(130f/255f,130f/255,130f/255,1f);


        float H, S, V;
        float H_black, S_black, V_black;
        Color.RGBToHSV(Color.black, out H_black, out S_black, out V_black);
        //Debug.Log("RGBToHSV:black:h=" + H_black + " s=" + S_black + " v=" + V_black);

        float H1, S1, V1;
        Color.RGBToHSV(colorRed1, out H1, out S1, out V1);
        //Debug.Log("RGBToHSV:colorRed1:h=" + H1 + " s=" + S1 + " v=" + V1);

        float H2, S2, V2;
        Color.RGBToHSV(colorRed2, out H2, out S2, out V2);
        //Debug.Log("RGBToHSV:colorRed2:h=" + H2 + " s=" + S2 + " v=" + V2);

        float diff_h = Mathf.Abs(H2 - H1);
        float diff_s = Mathf.Abs(S2 - S1);
        float diff_v = Mathf.Abs(V2 - V1);
        //Debug.Log("diff_h=" + diff_h + " diff_s=" + diff_s + " diff_v=" + diff_v);

        if ((diff_h < diffH) && (diff_s < diffS) && (diff_v < diffV))
        {
            //Debug.Log("the same color");
        }


        for (int j = 0; j < texPic.height; j++)
        {
            for (int i = 0; i < texPic.width; i++)
            {
                Vector2 pttmp = new Vector2(i, j);

                Color colorpic = colorImage.GetImageColorOrigin(pttmp);

                if (colorpic == Color.white)
                {
                    //白色变透明
                    colorpic.a = 0f;
                }


                //统一为纯黑色
                colorpic.r = 0f;
                colorpic.g = 0f;
                colorpic.b = 0f;
                if (colorpic.a < 0.5f)
                {
                    colorpic.a = 0f;
                }
                else
                {

                    colorpic.a = 1f;

                }
                // colorpic.r = 0;
                // colorpic.g = 0;
                // colorpic.b = 0;
                //colorpic.a = 1f;


                /*
				HSL 表示 hue（色相）、saturation（饱和度）、lightness（亮度），HSV 表示 hue、saturation、value(色调) 而 HSB 表示 hue、saturation、brightness（明度）
				 */
                // RGBToHSV(Color rgbColor, out float H, out float S, out float V);

                // Color.RGBToHSV(colorpic, out H, out S, out V);
                // float diff = H - H_black;


                colorImage.SetImageColor(pttmp, colorpic);

            }
        }

        colorImage.UpdateTexture();
    }

    bool IsSameColor(Color c1, Color c2)
    {
        bool ret = false;
        float diff = 0.05f;
        float diff_r = Mathf.Abs(c1.r - c2.r);
        float diff_g = Mathf.Abs(c1.g - c2.g);
        float diff_b = Mathf.Abs(c1.b - c2.b);
        float diff_a = Mathf.Abs(c1.a - c2.a);
        //Debug.Log("diff_h=" + diff_h + " diff_s=" + diff_s + " diff_v=" + diff_v);

        if ((diff_r < diff) && (diff_g < diff) && (diff_b < diff) && (diff_a < diff))
        {
            ret = true;
        }
        return ret;
    }
    //将白色背景填充成透明
    void FillBgWhiteToAlpha(Texture2D tex)
    {
        ColorImage climage = new ColorImage();
        climage.Init(tex);
        for (int j = 0; j < tex.height; j++)
        {
            for (int i = 0; i < tex.width; i++)
            {
                Vector2 pttmp = new Vector2(i, j);

                Color colorpic = climage.GetImageColorOrigin(pttmp);

                if (IsSameColor(colorpic, Color.white))
                {
                    //统一
                    colorpic.r = 0f;
                    colorpic.g = 0f;
                    colorpic.b = 0f;
                    colorpic.a = 0f;
                    climage.SetImageColor(pttmp, colorpic);
                }


            }
        }

        climage.UpdateTexture();
    }
    //将背景填充成白色
    void FillWhiteBg()
    {

        for (int j = 0; j < texPic.height; j++)
        {
            for (int i = 0; i < texPic.width; i++)
            {
                Vector2 pttmp = new Vector2(i, j);

                Color colorpic = colorImage.GetImageColorOrigin(pttmp);


                if (colorpic.a == 0f)
                {
                    //统一为纯白色
                    colorpic.r = 1f;
                    colorpic.g = 1f;
                    colorpic.b = 1f;
                    colorpic.a = 1f;
                    colorImage.SetImageColor(pttmp, colorpic);
                }


            }
        }

        colorImage.UpdateTexture();
    }


    //去除图片边缘的全透明区域
    void ConverImageSize(ColorItemInfo info)
    {
        FormatTexture();

        int fillXLeft = texPic.width;
        int fillXRight = 0;
        int fillYBottom = texPic.height;
        int fillYTop = 0;
        int x, y, w, h;
        for (int j = 0; j < texPic.height; j++)
        {
            for (int i = 0; i < texPic.width; i++)
            {
                Vector2 pt = new Vector2(i, j);
                Color color = colorImage.GetImageColorOrigin(pt);
                if (color.a == 1)
                {
                    x = i;
                    y = j;
                    if (x < fillXLeft)
                    {
                        fillXLeft = x;
                    }
                    if (x > fillXRight)
                    {
                        fillXRight = x;
                    }


                    if (y < fillYBottom)
                    {
                        fillYBottom = y;
                    }
                    if (y > fillYTop)
                    {
                        fillYTop = y;
                    }
                }

            }
        }



        //rect
        int oft = 10;
        x = fillXLeft - oft;
        if (x < 0)
        {
            x = 0;
        }
        y = fillYBottom - oft;
        if (y < 0)
        {
            y = 0;
        }
        w = fillXRight - fillXLeft + 1 + oft * 2;
        if ((x + w) > texPic.width)
        {
            w = texPic.width - x;
        }
        h = fillYTop - fillYBottom + 1 + oft * 2;
        if ((y + h) > texPic.height)
        {
            h = texPic.height - y;
        }


        //convert
        Texture2D tex = new Texture2D(w, h, TextureFormat.ARGB32, false);
        ColorImage crImage = new ColorImage();
        crImage.Init(tex);

        for (int j = y; j < (y + h); j++)
        {
            for (int i = x; i < (x + w); i++)
            {
                Vector2 pt = new Vector2(i, j);
                Color color = colorImage.GetImageColorOrigin(pt);
                Vector2 pt1 = new Vector2(i - x, j - y);
                crImage.SetImageColor(pt1, color);
            }
        }

        crImage.UpdateTexture();

        //save file 
        byte[] bytes = tex.EncodeToPNG();
        string filepath = Application.streamingAssetsPath + "/" + info.pic;
        System.IO.File.WriteAllBytes(filepath, bytes);

    }

    Color32 RandomColor32()
    {
        int r, g, b;
        int min = 1;
        int max = 254;
        r = Random.Range(min, max);
        g = Random.Range(min, max);
        b = Random.Range(min, max);
        Color32 color = new Color32((byte)r, (byte)g, (byte)b, 255);
        return color;
    }
    bool IsSameColor32(Color32 color1, Color32 color2, int step)
    {
        bool ret = false;
        int diffr = Mathf.Abs(color1.r - color2.r);
        int diffg = Mathf.Abs(color1.g - color2.g);
        int diffb = Mathf.Abs(color1.b - color2.b);
        if ((diffr <= step) && (diffg <= step) && (diffb <= step))
        {
            ret = true;
        }
        return ret;
    }

    bool IsColor32InRandomList(Color32 color)
    {
        bool ret = false;
        foreach (Color32 cr in listColor32Random)
        {
            if (IsSameColor32(cr, color, 10))
            {
                ret = true;
                break;
            }
        }
        return ret;
    }
    void AddColor2RandomList()
    {
        Color32 color = RandomColor32();
        bool isinlist = IsColor32InRandomList(color);
        while (isinlist)
        {
            color = RandomColor32();
            isinlist = IsColor32InRandomList(color);
        }

        listColor32Random.Add(color);
    }
    void CreateRandomColorList()
    {
        listColor32Random = new List<Color32>();
        int total = 4000;
        while (listColor32Random.Count < total)
        {
            AddColor2RandomList();
        }

    }

    void DoFillColor()
    {
        int idx = 0;

        if (indexFillColor < listGuanka.Count)
        {

            ColorItemInfo info = GetItemInfo(indexFillColor);
            Debug.Log("DoFillColor:idx=" + indexFillColor + " path:" + info.pic);
            //ColorItemInfo info = listGuanka[indexFillColor] as ColorItemInfo;
            long tick = Common.GetCurrentTimeMs();
            // string filepath = GetPathMask(info);
            if (!FileUtil.FileIsExistAsset(info.picmask))
            {
                FillColorImage(info);

            }
            tick = Common.GetCurrentTimeMs() - tick;
            Debug.Log("DoFillColor:idx=" + indexFillColor + " count=" + listGuanka.Count + " tick=" + tick + "ms" + " color_count=" + listColorFill.Count);

            indexFillColor++;

            Invoke("DoFillColor", 0.5f);
        }
        else
        {
            tickTotal = Common.GetCurrentTimeMs() - tickTotal;
            tickTotal = tickTotal / 1000;
            Debug.Log("guanka finish: " + tickTotal + "seconds");
            GotoNextPlace();
        }

        // Debug.Log("DoFillColor end");
    }


    void TestFillColor()
    {
        ColorItemInfo info = GetItemInfo(indexFillColor);
        Debug.Log("TestFillColor:idx=" + indexFillColor + " path:" + info.pic);

        long tick = Common.GetCurrentTimeMs();

        string filepath_mask = Application.streamingAssetsPath + "/" + info.picmask;

        listColorFill.Clear();
        listColorJson.Clear();
        texPic = LoadTexture.LoadFromAsset(info.pic);
        colorImage.Init(texPic);

        //不用调整图片大小
        {
            FormatTexture();

        }


        //Debug.Log("texPic.width="+texPic.width+" texPic.height=:"+texPic.height);
        int idx = 0;
        for (int j = 0; j < texPic.height; j++)
        {
            for (int i = 0; i < texPic.width; i++)
            {
                Vector2 pt = new Vector2(i, j);


                Color color = colorImage.GetImageColorOrigin(pt);
                // if ((color.a == 0) || (color == Color.white))
                if (color.a == 0)
                {
                    if (idx >= listColor32Random.Count)
                    {
                        Debug.Log("listColor32Random is too samll count=" + listColor32Random.Count + " texPic.width=" + texPic.width + " texPic.height=" + texPic.height);
                        break;
                    }
                    Color32 colorRandom = listColor32Random[idx];
                    Color colorFill = new Color(colorRandom.r / 255f, colorRandom.g / 255f, colorRandom.b / 255f, 1f);
                    //colorFill = Color.red;
                    colorImage.RunFillColor(pt, colorFill, Color.black);
                    //Debug.Log("idx="+idx+" FillRect:"+colorImage.fillRect);

                    idx++;
                    break;
                }


            }
        }



        UpdateTexture();
        tick = Common.GetCurrentTimeMs() - tick;

        Debug.Log("TestFillColor:idx=" + indexFillColor + " count=" + listGuanka.Count + " tick=" + tick + "ms" + " color_count=" + listColorFill.Count);

    }

    void UpdateTexture()
    {
        colorImage.UpdateTexture();
        Sprite sp = TextureUtil.CreateSpriteFromTex(texPic);
        SpriteRenderer spRender = objSpritePic.GetComponent<SpriteRenderer>();
        spRender.sprite = sp;
    }


    void FillColorImage(ColorItemInfo info)
    {
        string filepath_mask = Application.streamingAssetsPath + "/" + info.picmask;
        if (FileUtil.FileIsExist(filepath_mask))
        {
            Debug.Log("FillColorImage::FileIsExist");
            return;
        }
        listColorFill.Clear();
        listColorJson.Clear();
        texPic = LoadTexture.LoadFromAsset(info.pic);
        colorImage.Init(texPic);

        //不用调整图片大小
        {
            FormatTexture();
            //FillWhiteBg();
            //save
            {
                byte[] bytes = texPic.EncodeToPNG();
                string filepath = Application.streamingAssetsPath + "/" + info.pic;
                // System.IO.File.WriteAllBytes(filepath, bytes);
            }
        }



        //调整图片大小 重新加载图片
        {
            // ConverImageSize(info);
            // texPic = LoadTexture.LoadFromAsset(info.pic);
            // colorImage.Init(texPic);
        }

        //Debug.Log("texPic.width="+texPic.width+" texPic.height=:"+texPic.height);
        int idx = 0;
        for (int j = 0; j < texPic.height; j++)
        {
            for (int i = 0; i < texPic.width; i++)
            {
                Vector2 pt = new Vector2(i, j);


                Color color = colorImage.GetImageColorOrigin(pt);
                // if ((color.a == 0) || (color == Color.white))
                if (color.a == 0)
                {
                    if (idx >= listColor32Random.Count)
                    {
                        Debug.Log("listColor32Random is too samll count=" + listColor32Random.Count + " texPic.width=" + texPic.width + " texPic.height=" + texPic.height);
                        break;
                    }
                    Color32 colorRandom = listColor32Random[idx];
                    Color colorFill = new Color(colorRandom.r / 255f, colorRandom.g / 255f, colorRandom.b / 255f, 1f);
                    //colorFill = Color.red;
                    colorImage.RunFillColor(pt, colorFill, Color.black);
                    //Debug.Log("idx="+idx+" FillRect:"+colorImage.fillRect);

                    ColorItemInfo infoFill = new ColorItemInfo();
                    infoFill.rectFill = colorImage.fillRect;
                    infoFill.colorFill = colorFill;
                    infoFill.color32Fill = colorRandom;
                    listColorFill.Add(infoFill);

                    ColorJsonItemInfo colorJsonInfo = new ColorJsonItemInfo();
                    colorJsonInfo.color = colorRandom.r.ToString() + "," + colorRandom.g.ToString() + "," + colorRandom.b.ToString();
                    int x, y, w, h;
                    x = (int)colorImage.fillRect.x;
                    y = (int)colorImage.fillRect.y;
                    w = (int)colorImage.fillRect.width;
                    h = (int)colorImage.fillRect.height;
                    colorJsonInfo.rect = x.ToString() + "," + y.ToString() + "," + w.ToString() + "," + h.ToString();

                    listColorJson.Add(colorJsonInfo);

                    idx++;
                }


            }
        }



        //save color json
        {

            // Hashtable data = new Hashtable();
            // data["count"] = listColorJson.Count;
            // data["items"] = listColorJson;
            // string strJson = JsonMapper.ToJson(data);

            // string filepath = Application.streamingAssetsPath + "/" + info.colorJson;
            // Debug.Log("json file:" + filepath);
            // byte[] bytes = Encoding.UTF8.GetBytes(strJson);
            // System.IO.File.WriteAllBytes(filepath, bytes);
        }

        UpdateTexture();

        //save mask
        {
            byte[] bytes = texPic.EncodeToPNG();
            string filepath = GetPathMask(info);
            System.IO.File.WriteAllBytes(filepath, bytes);
        }

    }
    public string GetPathMask(ColorItemInfo info)
    {
        string filepath = Application.streamingAssetsPath + "/" + info.picmask;
        if (Application.isEditor)
        {
            filepath = Resource.dirResourceDataApp + "/" + info.picmask;
        }
        return filepath;
    }

    public void OnClickGuankaJson()
    {
        btnGuanJson.gameObject.SetActive(false);
        CreateGuankaJsonFile();
    }



    public void OnClickMask()
    {
        btnMask.gameObject.SetActive(false);
        Invoke("DoFillColor", 0.5f);
    }
    public void OnClickThumb()
    {
        btnThumb.gameObject.SetActive(false);
        {
            string strPlace = STR_PLACE;
            string path = Application.streamingAssetsPath + "/" + CloudRes.main.rootPathGameRes + "/image/" + strPlace + "/draw";
            string path_thumb = Application.streamingAssetsPath + "/" + CloudRes.main.rootPathGameRes + "/image/" + strPlace + "/thumb";
            int width_save = 256;
            int height_save = 256;
            // C#遍历指定文件夹中的所有文件 
            DirectoryInfo TheFolder = new DirectoryInfo(path);
            int idx = 0;
            // //遍历文件
            foreach (FileInfo NextFile in TheFolder.GetFiles())
            {
                string fullpath = NextFile.ToString();
                //1.jpg

                string ext = FileUtil.GetFileExt(fullpath).ToLower();
                //  Debug.Log("id="+idx.ToString()+" ext:"+ext);
                if ((ext == "png") || (ext == "jpg"))
                {
                    string name = NextFile.Name;

                    //fullpath = filepath_new;
                    Texture2D tex = LoadTexture.LoadFromFile(fullpath);
                    int w_save = 0, h_save = 0;
                    float scale = 0, scalex = 0, scaley = 0;
                    if (tex.width > tex.height)
                    {
                        scalex = Mathf.Max(width_save, height_save) * 1.0f / tex.width;
                        scaley = Mathf.Min(width_save, height_save) * 1.0f / tex.height;
                    }
                    else
                    {
                        scalex = Mathf.Min(width_save, height_save) * 1.0f / tex.width;
                        scaley = Mathf.Max(width_save, height_save) * 1.0f / tex.height;
                    }
                    scale = Mathf.Min(scalex, scaley);
                    w_save = (int)(scale * tex.width);
                    h_save = (int)(scale * tex.height);

                    //统一图片大小
                    //if (idx < 2)
                    {
                        Texture2D texSave = TextureUtil.ConvertSize(tex, w_save, h_save);
                        string dir = path_thumb;
                        string filepathSave = dir + "/" + name;
                        //FileUtil.CreateDir(dir);
                        byte[] bytes = texSave.EncodeToPNG();
                        System.IO.File.WriteAllBytes(filepathSave, bytes);
                    }
                    idx++;
                }

            }


            Debug.Log("OnClickThumb Finished");

        }
    }

}
