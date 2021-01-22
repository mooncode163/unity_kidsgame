using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LitJson;
using UnityEngine;
using UnityEditor;
public class ConvertImage : Editor
{
    public const string KEY_MENU_Root = "WordABC";


    [MenuItem(KEY_MENU_Root + "/OnConvertImage", false, 4)]
    static void OnConvertImage()
    {
        string path = Resource.dirResourceDataApp + "/ps/NEW lowercase do-a-dot white";
        string savedir = Resource.dirResourceDataApp + "/ps/lower";
        DoConvertImage(path, savedir);


        path = Resource.dirResourceDataApp + "/ps/NEW Uppercase do-a-dot  white";
        savedir = Resource.dirResourceDataApp + "/ps/upper";
        DoConvertImage(path, savedir);
    }

    static void DoConvertImage(string path, string savedir)
    {

        DirectoryInfo TheFolder = new DirectoryInfo(path);
        int idx = 0;
        // //遍历文件
        foreach (FileInfo NextFile in TheFolder.GetFiles())
        {
            string fullpath = NextFile.ToString();
            //1.jpgs
            // Debug.Log(NextFile.Name);
            string ext = FileUtil.GetFileExt(fullpath);
            if ((ext == "png") || (ext == "jpg"))
            {
                //将网上下载的图片 统一调整分辨率 
                Texture2D tex = LoadTexture.LoadFromFile(fullpath);
                float x, y, w, h;
                w = 1600;
                h = 1200;
                x = (tex.width - w) / 2;
                y = (tex.height - h) / 2;
                Rect rc = new Rect(x, y, w, h);
                Texture2D subtex = TextureUtil.GetSubTexture(tex, rc);
                // FormatTexture(subtex);
                FileUtil.CreateDir(savedir);

                string fullpath_save = savedir + "/" + FileUtil.GetFileName(NextFile.Name) + ".png";
                TextureUtil.SaveTextureToFile(subtex, fullpath_save);
                idx++;
            }

        }
    }
    static void FormatTexture(Texture2D texPic)
    {
        ColorImage colorImage = new ColorImage();
        colorImage.Init(texPic);
        // colorRed1 = new Color(0f/255,0f/255,0f/255,1f);
        // colorRed2 = new Color(130f/255f,130f/255,130f/255,1f);



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
                if ((colorpic.r > 200) || (colorpic.g > 200) || (colorpic.b > 200))
                {
                    colorpic.a = 0f;
                }

                //统一为纯黑色
                colorpic.r = 0f;
                colorpic.g = 0f;
                colorpic.b = 0f;
                if (colorpic.a < 0.5f)
                {
                    // colorpic.a = 0f;
                }
                else
                {

                    //  colorpic.a = 1f;

                }
                // colorpic.r = 0;
                // colorpic.g = 0;
                // colorpic.b = 0;
                //colorpic.a = 1f;


                colorImage.SetImageColor(pttmp, colorpic);

            }
        }

        colorImage.UpdateTexture();
    }


}
