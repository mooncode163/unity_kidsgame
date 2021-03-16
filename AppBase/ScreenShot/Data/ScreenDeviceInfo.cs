using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ShotDeviceInfo
{
    public string name;
    public int width;
    public int height;
    public bool isIconHd;
    public SystemLanguage lan;
    public bool isMain;
    public int index;
}

public class ScreenDeviceInfo
{
    //ipadpro
    public const int SCREEN_WIDTH_IPADPRO = 2732;
    public const int SCREEN_HEIGHT_IPADPRO = 2048;
    public const string DEVICE_NAME_IPADPRO = "ipadpro";
    //ipad
    public const int SCREEN_WIDTH_IPAD = 2048;
    public const int SCREEN_HEIGHT_IPAD = 1536;
    public const string DEVICE_NAME_IPAD = "ipad";

    //iphone
    public const int SCREEN_WIDTH_IPHONE = 2208;
    public const int SCREEN_HEIGHT_IPHONE = 1242;
    public const string DEVICE_NAME_IPHONE = "iphone";

    //1242x2688
    //iPhone X Plus将配备6.5 英寸屏幕 分辨率1242x2688
    public const int SCREEN_WIDTH_IPHONE_6_5 = 2688;
    public const int SCREEN_HEIGHT_IPHONE_6_5 = 1242;
    public const string DEVICE_NAME_IPHONE_6_5 = "iphone_6_5";

    //1080p
    public const int SCREEN_WIDTH_1080P = 1920;
    public const int SCREEN_HEIGHT_1080P = 1080;
    public const string DEVICE_NAME_1080P = "1080p";

    //
    public const int SCREEN_WIDTH_480P = 800;
    public const int SCREEN_HEIGHT_480P = 480;
    public const string DEVICE_NAME_480P = "480p";

    //weibo
    public const int SCREEN_WIDTH_WEIBO = 450;
    public const int SCREEN_HEIGHT_WEIBO = 300;
    public const string DEVICE_NAME_WEIBO = "weibo";

    //ad
    public const string DEVICE_NAME_AD = "ad"; 
    //icon
    public const string DEVICE_NAME_ICON = "icon";
    public const int SCREEN_WIDTH_ICON = 1024;
    public const int SCREEN_HEIGHT_ICON = 1024;

    //copy right huawei
    public const string DEVICE_NAME_COPY_RIGHT_HUAWEI = "copyright";
    public const string DEVICE_NAME_COPY_RIGHT_HD_HUAWEI = "copyright_hd";
    public const int SCREEN_WIDTH_COPY_RIGHT_HUAWEI = 750;
    public const int SCREEN_HEIGHT_COPY_RIGHT_HUAWEI = 1334;


    //pc
    public const int WIDTH_SCREEN_PC = 1920;
    public const int HEIGHT_SCREEN_PC = 1080;


    public List<ShotDeviceInfo> listDevice;

    List<SystemLanguage> listLanguage;
    ShotDeviceInfo deviceInfoNow;
    static private ScreenDeviceInfo _main = null;
    public static ScreenDeviceInfo main
    {
        get
        {
            if (_main == null)
            {
                _main = new ScreenDeviceInfo();
                _main.Init();
            }
            return _main;
        }
    }

    public void Init()
    {
        InitDevice();
    }

    public void InitDevice()
    {


        listLanguage = new List<SystemLanguage>();
        listLanguage.Add(SystemLanguage.Chinese);
        listLanguage.Add(SystemLanguage.English);

        listDevice = new List<ShotDeviceInfo>();


        // {

        // //ipadpro
        CreateDevice(DEVICE_NAME_IPADPRO, SCREEN_WIDTH_IPADPRO, SCREEN_HEIGHT_IPADPRO, true
        , true);

        //iphone_6_5
        CreateDevice(DEVICE_NAME_IPHONE_6_5, SCREEN_WIDTH_IPHONE_6_5, SCREEN_HEIGHT_IPHONE_6_5, true, true);


        //iphone
        CreateDevice(DEVICE_NAME_IPHONE, SCREEN_WIDTH_IPHONE, SCREEN_HEIGHT_IPHONE, true, true);


        //  ipad
        CreateDevice(DEVICE_NAME_IPAD, SCREEN_WIDTH_IPAD, SCREEN_HEIGHT_IPAD, true, false);

        //  1080p
        CreateDevice(DEVICE_NAME_1080P, SCREEN_WIDTH_1080P, SCREEN_HEIGHT_1080P, true, false);
        //  480p
        CreateDevice(DEVICE_NAME_480P, SCREEN_WIDTH_480P, SCREEN_HEIGHT_480P, true, true);


        //   weibo
        CreateDevice(DEVICE_NAME_WEIBO, SCREEN_WIDTH_WEIBO, SCREEN_HEIGHT_WEIBO, false, true);

        //   copy right huawei
        CreateDevice(DEVICE_NAME_COPY_RIGHT_HUAWEI, SCREEN_WIDTH_COPY_RIGHT_HUAWEI, SCREEN_HEIGHT_COPY_RIGHT_HUAWEI, false, true);

        // }

        {
            // //ICON
            CreateDevice(DEVICE_NAME_ICON, SCREEN_WIDTH_ICON, SCREEN_HEIGHT_ICON, false, true);
            //adhome
            CreateDevice(DEVICE_NAME_AD, 1024, 500, false, true);

            CreateDevice(DEVICE_NAME_AD, 1080, 480, false, true);

            CreateDevice(DEVICE_NAME_AD, 1920, 1080, false, true);

        }


        deviceInfoNow = listDevice[0];

    }
    public void CreateDevice(string name, int w, int h, bool isBoth, bool isMain)
    {
        if (name == DEVICE_NAME_ICON)
        {
            {
                //icon
                ShotDeviceInfo info = CreateDeviceItem(name, w, h, SystemLanguage.Chinese, isMain, false);

            }

            {
                //iconhd
                ShotDeviceInfo info = CreateDeviceItem(name, w, h, SystemLanguage.Chinese, isMain, true);
            }
            return;
        }


        if (name == DEVICE_NAME_COPY_RIGHT_HUAWEI)
        {
            //copyright
            ShotDeviceInfo info = CreateDeviceItem(name, w, h, SystemLanguage.Chinese, isMain, false);
            ShotDeviceInfo infohd = CreateDeviceItem(name, w, h, SystemLanguage.Chinese, isMain, true);
            return;
        }

        if (name == DEVICE_NAME_COPY_RIGHT_HD_HUAWEI)
        {
            //copyright hd
            //ShotDeviceInfo infohd = CreateDeviceItem(name, w, h, SystemLanguage.Chinese, isMain, true);
            return;
        }

        if (isBoth)
        {
            foreach (SystemLanguage lan in listLanguage)
            {
                //竖屏
                ShotDeviceInfo info = CreateDeviceItem(name, Mathf.Min(w, h), Mathf.Max(w, h), lan, isMain, false);

            }

            foreach (SystemLanguage lan in listLanguage)
            {
                //横屏
                ShotDeviceInfo info = CreateDeviceItem(name, Mathf.Max(w, h), Mathf.Min(w, h), lan, isMain, false);
            }
        }
        else
        {
            foreach (SystemLanguage lan in listLanguage)
            {

                ShotDeviceInfo info = CreateDeviceItem(name, w, h, lan, isMain, false);
            }

        }


    }

    ShotDeviceInfo CreateDeviceItem(string name, int w, int h, SystemLanguage lan, bool isMain, bool isHd)
    {
        ShotDeviceInfo info = new ShotDeviceInfo();
        info.width = w;
        info.height = h;
        info.lan = lan;
        info.name = name;
        info.isIconHd = isHd;
        info.isMain = isMain;
        info.index = listDevice.Count;
        listDevice.Add(info);
        return info;
    }

}
