using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System.Threading;
using System;
public class TextureThread : MonoBehaviour
{
    Thread thread;
    byte[] dataIn;//file date in

    byte[] dataOut;//decode out RGB
    int width;
    int height;
    Texture2D texOut;
    // Action actionFinish;
    Action<object,int,int> actionFinish;
    static private TextureThread _main = null;
    public static TextureThread main
    {
        get
        {
            if (_main == null)
            {
                _main = new TextureThread();

            }
            return _main;
        }
    }

    //线程加载纹理
    //https://www.cnblogs.com/lancidie/p/5877696.html
    public void LoadTexThread(byte[] data, Action<object,int,int> a)
    {

        //texOut = new Texture2D(0, 0, TextureFormat.ARGB32, false);//ARGB32

        dataIn = data;
        actionFinish = a;
        thread = new Thread(new ThreadStart(ProcThread));
        thread.Start();
    }

    public void ProcThread()
    {

        //只能在主线程load
        // texOut = LoadTexture.LoadFromData(dataIn);
        // texOut.LoadImage(dataIn);

        if (Common.isAndroid)
        {
            //android系统解码 
            dataOut = FileUtil.ReadRGBDataFromByte(dataIn);
            using (var javaClass = new AndroidJavaClass(FileUtil.JAVA_CLASS_FILEUTIL))
            {
                width = javaClass.CallStatic<int>("GetRGBDataWidth");
                height = javaClass.CallStatic<int>("GetRGBDataHeight");
            }
        }


        if (actionFinish != null)
        {
            actionFinish(dataOut,width,height);
        }
    }

}