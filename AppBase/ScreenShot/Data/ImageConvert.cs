using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class IconInfo
{
    public string srcPath;
    public string dstPath;//save file
    public int w;
    public int h;
}

public class ImageConvert
{
    List<IconInfo> listItem;
    int[] imageSizeIos = new int[] { 20, 29, 40, 50, 57, 58, 60, 72, 76, 80, 87, 100, 114, 120, 144, 152, 167, 180, 1024 };

    int[] imageSizeAndroid = new int[] { 72, 48, 96, 144, 192 };
    string[] resAndroid = new string[] { "mipmap-hdpi", "mipmap-mdpi", "mipmap-xhdpi", "mipmap-xxhdpi", "mipmap-xxxhdpi" };


    int[] imageSizeQQ = new int[] { 16 };
    int[] imageSizeShare = new int[] { 16, 28, 80, 120, 108, 512 };
    int[] imageSizeXiaomi = new int[] { 90, 136, 168, 192, 224 };
    int[] imageSizeHuawei = new int[] { 216 };

    int[] imageSizeMicrosoft = new int[] { 88, 24, 24, 300, 50, 48 };
    string[] resMicrosoft = new string[] { "Square44x44Logo.scale-200", "Square44x44Logo.targetsize-24", "Square44x44Logo.targetsize-24_altform-unplated", "Square150x150Logo.scale-200", "StoreLogo", "LockScreenLogo.scale-200" };
    static private ImageConvert _main = null;
    public static ImageConvert main
    {
        get
        {
            if (_main == null)
            {
                _main = new ImageConvert();
                _main.Init();
            }
            return _main;
        }
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public void Init()
    {
        listItem = new List<IconInfo>();

        //ios
        InitIos(false);
        InitIos(true);
        //android 
        InitAndroid(false);
        InitAndroid(true);

        InitOther(false, Source.QQ, imageSizeQQ);
        InitOther(true, Source.QQ, imageSizeQQ);

        InitOther(false, "share", imageSizeShare);
        InitOther(true, "share", imageSizeShare);

        InitOther(false, Source.XIAOMI, imageSizeXiaomi);
        InitOther(true, Source.XIAOMI, imageSizeXiaomi);

        InitOther(false, Source.HUAWEI, imageSizeHuawei);
        InitOther(true, Source.HUAWEI, imageSizeHuawei);

        InitMicrosoft(false);
        InitMicrosoft(true);
    }


    void InitIos(bool ishd)
    {
        int[] listsize = imageSizeIos;
        for (int i = 0; i < listsize.Length; i++)
        {
            IconInfo info = new IconInfo();
            info.w = listsize[i];
            info.h = info.w;
            info.srcPath = GetIconFile(ishd);
            info.dstPath = GetRootDirSaveIcon(ishd) + "/ios/icon_" + info.w + ".png";
            listItem.Add(info);
        }
    }
    void InitAndroid(bool ishd)
    {
        int[] listsize = imageSizeAndroid;
        for (int i = 0; i < listsize.Length; i++)
        {
            IconInfo info = new IconInfo();
            info.w = listsize[i];
            info.h = info.w;
            info.srcPath = GetRootDirSaveIcon(ishd) + "/icon_android.png";
            info.dstPath = GetRootDirSaveIcon(ishd) + "/android/" + resAndroid[i] + "/ic_launcher.png";
            listItem.Add(info);
        }
    }

    void InitMicrosoft(bool ishd)
    {
        int[] listsize = imageSizeMicrosoft;
        for (int i = 0; i < listsize.Length; i++)
        {
            IconInfo info = new IconInfo();
            info.w = listsize[i];
            info.h = info.w;
            info.srcPath = GetRootDirSaveIcon(ishd) + "/icon_android.png";
            info.dstPath = GetRootDirSaveIcon(ishd) + "/microsoft/" + resMicrosoft[i] + ".png";
            listItem.Add(info);
        }
    }
    void InitOther(bool ishd, string dir, int[] listsize)
    {
        //int[] listsize = imageSizeQQ;
        for (int i = 0; i < listsize.Length; i++)
        {
            {
                IconInfo info = new IconInfo();
                info.w = listsize[i];
                info.h = info.w;
                info.srcPath = GetIconFile(ishd);
                info.dstPath = GetRootDirSaveIcon(ishd) + "/" + dir + "/icon_" + info.w + ".png";
                listItem.Add(info);
            }
            {
                IconInfo info = new IconInfo();
                info.w = listsize[i];
                info.h = info.w;
                info.srcPath = GetRootDirSaveIcon(ishd) + "/icon_android.png";
                info.dstPath = GetRootDirSaveIcon(ishd) + "/" + dir + "/icon_android_" + info.w + ".png";
                listItem.Add(info);
            }

        }

    }


    string GetIconFile(bool ishd)
    {
        string icon_path = GetRootDirIconSrc(ishd) + "/icon.jpg";//png
        if (!FileUtil.FileIsExist(icon_path))
        {
            icon_path = GetRootDirIconSrc(ishd) + "/icon.png";
        }
        return icon_path;
    }


    void ConvertMainIcon(bool ishd)
    {
        string icon_path = GetIconFile(ishd);//png 
        string dirsave = GetRootDirSaveIcon(ishd);
        FileUtil.CreateDir(dirsave);
        Debug.Log("ImageConvert icon_path=" + icon_path);

        string icon_path_android = dirsave + "/icon_android.png";
        Texture2D texIcon = LoadTexture.LoadFromFile(icon_path);
        //保存圆角的android  icon 
        {
            Texture2D texTmp = RoundRectTexture(texIcon);
            TextureUtil.SaveTextureToFile(texTmp, icon_path_android);
        }


        Texture2D texIconAndroid = LoadTexture.LoadFromFile(icon_path_android);
        //512 android
        {
            Texture2D tex512 = TextureUtil.ConvertSize(texIconAndroid, 512, 512);
            string filepath = GetRootDirSaveIcon(ishd) + "/icon_android_512.png";
            TextureUtil.SaveTextureToFile(tex512, filepath);
            filepath = GetRootDirSaveIcon(ishd) + "/icon_android_512.jpg";
            TextureUtil.SaveTextureToFile(tex512, filepath);
        }

        //512 ios
        {
            Texture2D tex512 = TextureUtil.ConvertSize(texIcon, 512, 512);
            string filepath = GetRootDirSaveIcon(ishd) + "/icon_512.png";
            TextureUtil.SaveTextureToFile(tex512, filepath);
            filepath = GetRootDirSaveIcon(ishd) + "/icon_512.jpg";
            TextureUtil.SaveTextureToFile(tex512, filepath);
        }


        //1024 ios
        {
            Texture2D tex = TextureUtil.ConvertSize(texIcon, 1024, 1024);
            string filepath = GetRootDirSaveIcon(ishd) + "/icon_1024.jpg";
            TextureUtil.SaveTextureToFile(tex, filepath);
        }

        //1024 android
        {
            Texture2D tex = TextureUtil.ConvertSize(texIconAndroid, 1024, 1024);
            string filepath = GetRootDirSaveIcon(ishd) + "/icon_android_1024.jpg";
            TextureUtil.SaveTextureToFile(tex, filepath);
        }
    }

    //圆角
    Texture2D RoundRectTexture(Texture2D tex)
    {
        int w = tex.width;
        int h = tex.height;
        RenderTexture rt = new RenderTexture(w, h, 0);
        string strshader = "Custom/RoundRect";
        //string str = FileUtil.ReadStringAsset(ShotBase.STR_DIR_ROOT_SHADER+"/ShaderRoundRect.shader");
        Material mat = new Material(Shader.Find(strshader));//
        float value = (ShotBase.roundRectRadiusIcon * 1f / ScreenDeviceInfo.SCREEN_WIDTH_ICON);
        Debug.Log("RoundRectTexture:value=" + value);
        //value = 0.1f;
        //设置半径 最大0.5f
        mat.SetFloat("_RADIUSBUCE", value);
        Graphics.Blit(tex, rt, mat);
        Texture2D texRet = TextureUtil.RenderTexture2Texture2D(rt);
        return texRet;
    }
    string GetRootDirIconSrc(bool ishd)
    {
        string name = ishd ? "iconhd" : "icon";
        string ret = UIScreenShotController.GetRootDirIcon() + "/" + name;
        return ret;
    }
    public string GetRootDirSaveIcon(bool ishd)
    {
        string name = ishd ? "iconhd" : "icon";
        string ret = UIScreenShotController.GetRootDirOutPut() + "/" + name;
        return ret;
    }
    string GetRootDirResourceData()
    {
        string ret = AppsConfig.ROOT_DIR_PC + "/ResourceData/" + Common.appType + "/" + Common.appKeyName;
        return ret;
    }

    string GetRootDirBg()
    {
        string ret = GetRootDirResourceData() + "/Resources/App/UI/Bg";
        return ret;
    }


    void DoConvertIcon(IconInfo info)
    {
        Texture2D texIcon = LoadTexture.LoadFromFile(info.srcPath);

        Texture2D texSave = TextureUtil.ConvertSize(texIcon, info.w, info.h, texIcon.format);
        FileUtil.CreateDir(FileUtil.GetFileDir(info.dstPath));
        // 最后将这些纹理数据，成一个png图片文件  
        TextureUtil.SaveTextureToFile(texSave, info.dstPath);

    }



    public void OnConvertIcon()
    {
        ConvertMainIcon(false);
        ConvertMainIcon(true);
        foreach (IconInfo info in listItem)
        {
            DoConvertIcon(info);
        }
    }


   public void OnConvertBg()
    {

        List<string> listImage = new List<string>();
        string pic;
        //startup
        string file_startup = "/GameData/startup.jpg";
        {
            pic = Resource.dirResourceDataApp + file_startup;
            listImage.Add(pic);
        } 
        //string dirBg = GetRootDirBg();
        string dirBg = Resource.dirResourceDataApp + "/Resources/App/UI/Bg";
        // C#遍历指定文件夹中的所有文件 
        DirectoryInfo TheFolder = new DirectoryInfo(dirBg); ;
        // //遍历文件
        foreach (FileInfo NextFile in TheFolder.GetFiles())
        {
            string fullpath = NextFile.ToString();
            string ext = FileUtil.GetFileExt(fullpath);
            if ((ext == "png") || (ext == "jpg"))
            {
                listImage.Add(fullpath);
            }

        }

        //



        for (int i = 0; i < listImage.Count; i++)
        {
            pic = listImage[i];
            Debug.Log("OnConvertBg pic=" + pic);
            Texture2D texImage = LoadTexture.LoadFromFile(pic);

            //  int w_screen = 2048;//ScreenDeviceInfo.SCREEN_WIDTH_IPHONE_6_5;
            // int h_screen = ScreenDeviceInfo.SCREEN_HEIGHT_IPHONE_6_5 * w_screen / ScreenDeviceInfo.SCREEN_WIDTH_IPHONE_6_5;//ScreenDeviceInfo.SCREEN_HEIGHT_IPHONE_6_5;
            int w_screen = ScreenDeviceInfo.SCREEN_WIDTH_IPADPRO;
            int h_screen = ScreenDeviceInfo.SCREEN_HEIGHT_IPADPRO;

            int w, h;
            if (texImage.width < texImage.height)
            {
                //竖图
                w = Mathf.Min(w_screen, h_screen);
                h = Mathf.Max(w_screen, h_screen);
            }
            else
            {
                w = Mathf.Max(w_screen, h_screen);
                h = Mathf.Min(w_screen, h_screen);
            }

            float scale = Common.GetBestFitScale(texImage.width, texImage.height, w, h);
            Debug.Log("OnConvertBg scale =" + scale + " w=" + " h=" + h);
            Texture2D texSave = TextureUtil.ConvertSize(texImage, (int)(texImage.width * scale), (int)(texImage.height * scale), texImage.format);
            // 最后将这些纹理数据，成一个png图片文件  
            TextureUtil.SaveTextureToFile(texSave, pic);

        }
    }


    public void OnConvertScreenShot()
    {
        //  GameObject obj = new GameObject("UI");
        // UIScreenShotController ui = obj.AddComponent<UIScreenShotController>();
        //  ui.OnPngConVert();
    }


}
