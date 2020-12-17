using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTextureScene : MonoBehaviour
{
    public GameObject objSpritePic;
    public Texture2D texPic;
    public Texture2D texPicFormFile;
    ColorImage colorImage;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        string picname = (0 + 1).ToString("d3");
        string pic = CloudRes.main.rootPathGameRes + "/animal/draw/" + picname + ".png";

        texPicFormFile = LoadTex(pic);


        // texPic = Resources.Load("001_24") as Texture2D;

        colorImage = new ColorImage();
        // colorImage.Init(texPic);
    }
    // Use this for initialization
    void Start()
    {

        texPic = CreateTexTureBg(texPicFormFile.width, texPicFormFile.height);
        // string picbg = Common.GAME_RES_DIR + "/animal/draw/"  + "001_bg.png";
        // texPic = LoadTex(picbg);

        UpdateSpriteTexture();

        CopyTexture(texPic, texPicFormFile);

        colorImage.Init(texPic);

    }

    // Update is called once per frame
    void Update()
    {

    }
    void CopyTexture(Texture2D dst, Texture2D src)
    {
        ColorImage crImageSrc = new ColorImage();
        crImageSrc.Init(src);
        ColorImage crImageDst = colorImage;
        crImageDst.Init(dst);
        int w = src.width;
        int h = src.height;
        Debug.Log("CopyTexture:w=" + w + " h=" + h);
        for (int j = 0; j < h; j++)
        {
            for (int i = 0; i < w; i++)
            {
                Vector2 pttmp = new Vector2(i, j);
                Color colorpic = crImageSrc.GetImageColorOrigin(pttmp);

                crImageDst.SetImageColor(pttmp, colorpic);
            }
        }
        crImageDst.UpdateTexture();
    }

    Texture2D CreateTexTureBg(int w, int h)
    {
        Texture2D tex = new Texture2D(w, h, TextureFormat.ARGB32, false);

        ColorImage crImage = new ColorImage();
        crImage.Init(tex);
        for (int j = 0; j < h; j++)
        {
            for (int i = 0; i < w; i++)
            {
                Vector2 pttmp = new Vector2(i, j);
                Color colorpic = new Color(1f, 1f, 1f, 1f);
                crImage.SetImageColor(pttmp, colorpic);
            }
        }

        crImage.UpdateTexture();

        return tex;
    }

    Texture2D LoadTex(string file)
    {
        Texture2D tex = null;
        byte[] data = FileUtil.ReadDataAsset(file);
        if (data != null)
        {
            //tex = LoadTexture.LoadFromData(data,TextureFormat.RGB24);
            tex = LoadTexture.LoadFromDataWithFormat(data, TextureFormat.ARGB32);
        }
        return tex;
    }
    void UpdateSpriteTexture()
    {
        //colorImage.UpdateTexture();
        Sprite sp = TextureUtil.CreateSpriteFromTex(texPic);
        SpriteRenderer spRender = objSpritePic.GetComponent<SpriteRenderer>();
        spRender.sprite = sp;
    }

    void ApplayTexture()
    {
        colorImage.UpdateTexture();

    }

    void TestTexture()
    {
        for (int j = 0; j < texPic.height; j++)
        {
            for (int i = 0; i < texPic.width; i++)
            {
                Vector2 pttmp = new Vector2(i, j);

                //
                Color colorpic;
                if (texPic.format == TextureFormat.RGB24)
                {
                    colorImage.GetImageColorRGB24(pttmp);
                }
                else
                {
                    colorpic = colorImage.GetImageColorOrigin(pttmp);
                }
                //统一为纯黑色
                colorpic.r = 1f;
                colorpic.g = 0.5f;
                colorpic.b = 0.5f;
                colorpic.a = 1f;

                if (texPic.format == TextureFormat.RGB24)
                {
                    colorImage.SetImageColorRGB24(pttmp, colorpic);
                }
                else
                {
                    colorImage.SetImageColor(pttmp, colorpic);
                }


            }
        }


        ApplayTexture();
    }

    public void OnClickBtn()
    {
        TestTexture();

    }
}
