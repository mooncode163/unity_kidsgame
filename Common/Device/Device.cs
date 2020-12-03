using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Device
{

    static public ScreenOrientation orientationPre = ScreenOrientation.Unknown;
    static public ScreenOrientation orientationCur = ScreenOrientation.Unknown;

    static public int screenWidthPre = 0;
    static public int screenWidthCur = 0;

    static public int screenHeightPre = 0;
    static public int screenHeightCur = 0;

    static private Camera _camera;
    static public Camera camera
    {
        get
        {
            if (_camera == null)
            {
                _camera = AppSceneBase.main.mainCamera;
            }
            return _camera;
        }
        set
        {
            _camera = value;
        }
    }

    static public int heightSystemTopBar//异形全面屏顶部“刘海”等的高度
    {
        get
        {
            int ret = 0;
            // if (isiPhoneX())
            {
                CommonBasePlatformWrapper platformWrapper = CommonPlatformWrapper.platform;
                ret = platformWrapper.getHeightSystemTopBar();
            }
            return ret;
        }
    }
    static public int heightSystemHomeBar//全面屏底部操作区高度
    {
        get
        {
            int ret = 0;
            // if (isiPhoneX())
            {
                CommonBasePlatformWrapper platformWrapper = CommonPlatformWrapper.platform;
                //全面屏底部操作区
                ret = platformWrapper.getHeightSystemHomeBar();
            }
            return ret;
        }
    }


    //前面屏边界偏移量
    static public int offsetLeft//单位为像素
    {
        get
        {
            int ret = 0;
            if (isLandscape)
            {
                if (isLandscapeLeft)
                {
                    ret = heightSystemTopBar;
                }


                if (Common.isAndroid)
                {
                    //huawei 全面屏虚拟按键一直显示在右边
                    ret = 0;
                }
            }
            //ret = 32;
            return ret;
        }
    }

    static public int offsetRight//单位为像素
    {
        get
        {
            int ret = 0;
            if (isLandscape)
            {
                if (isLandscapeRight)
                {
                    ret = heightSystemTopBar;
                }

                if (Common.isAndroid)
                {
                    if (isLandscapeRight)
                    {
                        //huawei 全面屏虚拟按键显示在右边
                        ret = heightSystemHomeBar;
                    }
                    else
                    {
                        ret = heightSystemTopBar;
                    }
                }
            }
            //ret = 32;
            return ret;
        }
    }

    static public int offsetTop//单位为像素
    {
        get
        {
            int ret = 0;
            if (!isLandscape)
            {
                ret = heightSystemTopBar;
            }
            //ret = 32;
            return ret;
        }
    }

    static public int offsetBottom//单位为像素
    {
        get
        {
            int ret = 0;
            if (isLandscape)
            {
                if (Common.isAndroid)
                {
                    //huawei 全面屏虚拟按键显示在右边
                    ret = 0;
                }
                if (Common.isiOS)
                {
                    ret = heightSystemTopBar;
                }

            }
            else
            {
                ret = heightSystemHomeBar;
            }
            //ret = 64;
            return ret;
        }
    }


    //camera
    static public float offsetLeftWorld//世界坐标
    {
        get
        {
            return Common.ScreenToWorldWidth(camera, Device.offsetLeft);
        }
    }

    static public float offsetRightWorld//世界坐标
    {
        get
        {
            return Common.ScreenToWorldWidth(camera, Device.offsetRight);
        }
    }


    static public float offsetTopWorld//世界坐标
    {
        get
        {
            return Common.ScreenToWorldWidth(camera, Device.offsetTop);
        }
    }


    static public float offsetBottomWorld//世界坐标
    {
        get
        {
            return Common.ScreenToWorldWidth(camera, Device.offsetBottom);
        }
    }


    static public float offsetBottomWithAdBannerWorld//世界坐标
    {
        get
        {
            return offsetBottomWorld + AdKitCommon.main.heightAdWorld * 1.1f;
        }
    }

    static public bool isLandscape
    {
        get
        {
            bool ret = false;
            if (Screen.width > Screen.height)
            {
                ret = true;
            }
            return ret;
        }
    }
    static public bool isLandscapeLeft
    {
        get
        {
            if (Application.isEditor)
            {
                return true;
            }
            bool ret = false;
            if (Screen.orientation == ScreenOrientation.LandscapeLeft)
            {
                ret = true;

            }
            return ret;
        }
    }
    static public bool isLandscapeRight
    {
        get
        {
            bool ret = false;
            if (Application.isEditor)
            {
                return false;
            }
            if (Screen.orientation == ScreenOrientation.LandscapeRight)
            {
                ret = true;
            }
            return ret;
        }
    }

    static public bool isDeviceDidRotation
    {
        get
        {
            bool ret = false;
            if ((ScreenOrientation.Unknown == orientationPre) && (ScreenOrientation.Unknown == orientationCur))
            {
                //初始化
                orientationCur = Screen.orientation;
                orientationPre = Screen.orientation;
            }
            orientationCur = Screen.orientation;
            if (orientationPre != orientationCur)
            {
                Debug.Log("DeviceDidRotation Screen.orientation=" + Screen.orientation);
                ret = true;
            }
            orientationPre = orientationCur;
            return ret;
        }
    }

    static public bool isScreenDidChange
    {
        get
        {
            bool ret = false;
            if (isScreenWidthDidChange || isScreenHeightDidChange)
            {
                ret = true;
            }
            return ret;
        }
    }

    static public bool isScreenWidthDidChange
    {
        get
        {
            bool ret = false;
            if ((screenWidthPre == 0) && (0 == screenWidthCur))
            {
                //初始化
                screenWidthPre = Screen.width;
                screenWidthCur = Screen.width;
            }
            screenWidthCur = Screen.width;
            if (screenWidthPre != screenWidthCur)
            {
                //Debug.Log("isScreenWidthDidChange Screen.width=" + Screen.width);
                ret = true;
            }
            screenWidthPre = screenWidthCur;
            return ret;
        }
    }


    static public bool isScreenHeightDidChange
    {
        get
        {
            bool ret = false;
            if ((screenHeightPre == 0) && (0 == screenHeightCur))
            {
                //初始化
                screenHeightPre = Screen.height;
                screenHeightCur = Screen.height;
            }
            screenHeightCur = Screen.height;
            if (screenHeightPre != screenHeightCur)
            {
                Debug.Log("isScreenHeightDidChange Screen.height=" + Screen.height);
                ret = true;
            }
            screenHeightPre = screenHeightCur;
            return ret;
        }
    }


    static public bool isiPhoneX()
    {
        //return true;
        CommonBasePlatformWrapper platformWrapper = CommonPlatformWrapper.platform;
        return platformWrapper.isiPhoneX();

    }
    static public void InitOrientation()
    {
        Screen.orientation = ScreenOrientation.AutoRotation;

        if (AppVersion.appForPad)
        {
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
        }
        else
        {

            Screen.autorotateToPortrait = true;
            Screen.autorotateToPortraitUpsideDown = true;
            Screen.autorotateToLandscapeLeft = false;
            Screen.autorotateToLandscapeRight = false;
        }
    }


    //参考设计分辨率
    static public Vector2 sizeDesign
    {
        get
        {
            CanvasScaler canvasScaler = AppSceneBase.main.canvasMain.gameObject.GetComponent<CanvasScaler>();
            return canvasScaler.referenceResolution;
        }
    }


}