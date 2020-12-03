using UnityEngine;
using System.Collections;
using System.IO;
using System.Threading;

public class LoadTexture : MonoBehaviour
{
    static public Texture2D LoadFileAuto(string filePath)
    {
        Texture2D tex = LoadFromResource(filePath);
        if (tex == null)
        {
            if (FileUtil.FileIsExistAsset(filePath))
            {
                tex = LoadFromAsset(filePath);
            }

        }
        if (tex == null)
        {
            if (FileUtil.FileIsExist(filePath))
            {
                tex = LoadFromFile(filePath);
            }
        }
        return tex;
    }

    //filePath 为绝对路径
    static public Texture2D LoadFromFile(string filePath)
    {
        Texture2D tex = null;
        byte[] data = FileUtil.ReadDataFromFile(filePath);
        if (data != null)
        {
            tex = LoadFromData(data);
        }
        return tex;
    }

    //file 为相对路径
    static public Texture2D LoadFromAsset(string file)
    {
        Texture2D tex = null;
        byte[] data = null;

        if (Common.isAndroid)
        {
            long tick = Common.GetCurrentTimeMs();
            //android系统解码
            // int w, h;
            // data = FileUtil.ReadRGBDataAsset(file);
            // using (var javaClass = new AndroidJavaClass(FileUtil.JAVA_CLASS_FILEUTIL))
            // {
            //     w = javaClass.CallStatic<int>("GetRGBDataWidth");
            //     h = javaClass.CallStatic<int>("GetRGBDataHeight");
            // }

            // tex = LoadFromRGBData(data, w, h);

            //unity软件解码
            data = FileUtil.ReadDataAsset(file);
            tex = LoadFromData(data);

            tick = Common.GetCurrentTimeMs() - tick;
            Debug.Log("LoadFromAsset time tick=" + tick + "ms");
        }
        else
        {
            data = FileUtil.ReadDataAsset(file);
            tex = LoadFromData(data);
        }
        return tex;
    }

    //
    static public Texture2D LoadFromRGBData(byte[] data, int w, int h)
    {
        Texture2D tex = null;

        //android Bitmap 读出来的Rgb数据是RGBA32
        tex = new Texture2D(w, h, TextureFormat.RGBA32, false);//RGBA32  ARGB32
        byte[] pixselImage = tex.GetRawTextureData();
        int size = pixselImage.Length;
        System.Array.Copy(data, pixselImage, size);
        tex.LoadRawTextureData(pixselImage);
        tex.Apply(false);
        //tex.LoadImage(data);
        return tex;
    }

    static public Texture2D LoadFromData(byte[] data)
    {
        long tick = Common.GetCurrentTimeMs();
        Texture2D tex = null;
        if (Common.isAndroid)
        {

            //android系统解码
            // int w, h;
            // byte[] rgbdata = FileUtil.ReadRGBDataFromByte(data);
            // using (var javaClass = new AndroidJavaClass(FileUtil.JAVA_CLASS_FILEUTIL))
            // {
            //     w = javaClass.CallStatic<int>("GetRGBDataWidth");
            //     h = javaClass.CallStatic<int>("GetRGBDataHeight");
            // }
            // tex = LoadFromRGBData(rgbdata, w, h);

            // unity软件解码
            tex = new Texture2D(0, 0, TextureFormat.ARGB32, false);//ARGB32
            tex.LoadImage(data);

        }
        else
        {
            //unity软件解码
            tex = new Texture2D(0, 0, TextureFormat.ARGB32, false);//ARGB32
            tex.LoadImage(data);
        }

        tick = Common.GetCurrentTimeMs() - tick;
        Debug.Log("LoadFromData time tick=" + tick + "ms");

        return tex;
    }

    static public Texture2D LoadFromDataWithFormat(byte[] data, TextureFormat format)
    {
        Texture2D tex = null;
        tex = new Texture2D(0, 0, format, false);
        Debug.Log("LoadFromDataWithFormat 1 format=" + tex.format);
        tex.LoadImage(data);
        Debug.Log("LoadFromDataWithFormat 2 format=" + tex.format);
        return tex;
    }

    static public Texture2D LoadFromResource(string file)
    {
        Object obj = Resources.Load(FileUtil.GetFileBeforeExtWithOutDot(file));
        if(obj==null){
            Debug.Log("LoadFromResource file="+file);
            return null;
        }
        Texture2D tex = (Texture2D)obj;
        return tex;
    }

}