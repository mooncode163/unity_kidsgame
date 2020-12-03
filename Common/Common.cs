using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;
using LitJson;
using Moonma.AdKit.AdConfig;

public enum UIOrientation
{

    UIORIENTATION_ALL = 0,//all 任意方向
    UIORIENTATION_PortraitUp,
    UIORIENTATION_PortraitDown,
    UIORIENTATION_LandscapeLeft,
    UIORIENTATION_LandscapeRight,
    UIORIENTATION_Portrait,
    UIORIENTATION_Landscape,

}

public class Common
{
    public const string GAME_DATA_DIR = "GameData";//streamingAssetsPath下的游戏配置等数据
    public const string GAME_DATA_DIR_COMMON = Common.GAME_DATA_DIR + "/common";
    public const string GAME_RES_DIR = "GameRes";//streamingAssetsPath 下的游戏图片等资源
    public const string RES_CONFIG_DATA = "ConfigData";//Resources 下的 配置等数据
    public const string RES_CONFIG_DATA_COMMON = "ConfigDataCommon";//Resources 下的 配置等数据
    public const string THUMB_SUFFIX = "_thumb";//小图片后缀

    public const float TOUCH_MOVE_STEP_MIN = 3.0f;//6.0f

    static public string rootDirAppCenter
    {
        get
        {
            string str = Common.RES_CONFIG_DATA + "/app_center";
            // if (!Directory.Exists(str))
            // {
            //     str = Common.GAME_DATA_DIR_COMMON + "/app_center";
            // }
            return str;
        }
    }

    static public bool isMonoPlayer
    {
        get
        {
            bool ret = false;
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        ret = true;
#endif

#if UNITY_STANDALONE_OSX && !UNITY_EDITOR
        ret = true;
#endif

            return ret;
        }
    }

    static public bool isiOS
    {
        get
        {
            bool ret = false;
#if UNITY_IOS && !UNITY_EDITOR
        ret = true;
#endif

            return ret;
        }
    }

    static public bool isAndroid
    {
        get
        {
            bool ret = false;
#if UNITY_ANDROID && !UNITY_EDITOR
            ret = true;
#endif
            return ret;
        }
    }

    static public bool isWeb
    {
        get
        {
            bool ret = false;
#if UNITY_WEBGL && !UNITY_EDITOR
            ret = true;
#endif
            return ret;
        }
    }

    static public bool isPC
    {
        get
        {
            if (isMac || isWin)
            {
                return true;
            }
            return false;
        }
    }

    //https://blog.csdn.net/oskytonight/article/details/40111401
    static public bool isMac
    {
        get
        {
            bool ret = false;
#if UNITY_STANDALONE_OSX && !UNITY_EDITOR
        ret = true;
#endif

            return ret;
        }
    }
    static public bool isWin
    {
        get
        {
            bool ret = false;
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        ret = true;
#endif

            return ret;
        }
    }

    static public bool isWinUWP
    {
        get
        {
            bool ret = false;
#if UNITY_WSA && !UNITY_EDITOR
        ret = true;
#endif

            return ret;
        }
    }
    static public bool noad
    {
        get
        {
            string key = "APP_NO_AD";
            int ret = PlayerPrefs.GetInt(key, 0);
            if (ret > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        set
        {
            string key = "APP_NO_AD";
            int ret = 0;
            if (value)
            {
                ret = 1;
                AdConfig.main.SetNoAd();
            }
            else
            {
                ret = 0;
            }
            PlayerPrefs.SetInt(key, ret);
        }
    }

    static public bool isRemoveAd
    {
        get
        {
            string key = "KEY_HAS_REMOVE_AD";
            return Common.Int2Bool(PlayerPrefs.GetInt(key, 0));
        }
        set
        {
            string key = "KEY_HAS_REMOVE_AD";
            PlayerPrefs.SetInt(key, Common.Bool2Int(value));

        }
    }

    static public string appKeyName
    {
        get
        {
            string str = Config.main.GetStringCommon("APP_NAME_KEYWORD", "0");
            return str;
        }
    }
    static public string appType//
    {
        get
        {
            string str = Config.main.GetStringCommon("APP_TYPE", "0");
            return str;
        }
    }

    static public int gold
    {
        get
        {
            int ret = 0;
            string key = "KEY_GAME_GOLD";
            ret = PlayerPrefs.GetInt(key, 0);
            if (Application.isEditor)
            {
                // ret = AppRes.GOLD_INIT_VALUE;
            }
            return ret;
        }

        set
        {
            string key = "KEY_GAME_GOLD";
            PlayerPrefs.SetInt(key, value);
        }

    }

    static public float GetBestFitScale(float w_content, float h_content, float w_rect, float h_rect)
    {
        if ((w_rect == 0) || (h_rect == 0))
        {
            return 1f;
        }
        float scalex = w_rect / w_content;
        float scaley = h_rect / h_content;
        float scale = Mathf.Min(scalex, scaley);
        return scale;
    }
    static public float GetMaxFitScale(float w_content, float h_content, float w_rect, float h_rect)
    {
        if ((w_rect == 0) || (h_rect == 0))
        {
            return 1f;
        }
        float scalex = w_rect / w_content;
        float scaley = h_rect / h_content;
        float scale = Mathf.Max(scalex, scaley);
        return scale;
    }

    static public bool GetKeyForFirstRun(string key)
    {
        string _key = key + "_" + GetAppVersion();
        bool ret = Common.Int2Bool(PlayerPrefs.GetInt(_key, 1));
        return ret;
    }

    static public void SetKeyForFirstRun(string key, bool v)
    {
        string _key = key + "_" + GetAppVersion();
        PlayerPrefs.SetInt(_key, Common.Bool2Int(v));
    }


    static public bool Int2Bool(int v)
    {
        if (v != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    static public int Bool2Int(bool v)
    {
        if (v)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }


    public static bool GetBool(string key)
    {
        return Int2Bool(PlayerPrefs.GetInt(key));
    }

    public static bool GetBool(string key, bool default_value)
    {
        int v = Bool2Int(default_value);
        return Int2Bool(PlayerPrefs.GetInt(key, v));
    }

    public static void SetBool(string key, bool value)
    {
        int v = Bool2Int(value);
        PlayerPrefs.SetInt(key, v);

    }

    static public int String2Int(string str)
    {
        int ret = 0;
        int.TryParse(str, out ret);
        return ret;
    }
    static public float String2Float(string str)
    {
        float ret = 0;
        float.TryParse(str, out ret);
        return ret;
    }


    //255,100,200 to color
    static public string Color2RGBString(Color color)
    {
        int r = (int)(color.r * 255);
        int g = (int)(color.g * 255);
        int b = (int)(color.b * 255);
        string strsplit = ",";
        string ret = r.ToString() + strsplit + g.ToString() + strsplit + b.ToString();
        return ret;
    }

    //color to 255,100,200
    static public Color RGBString2Color(string strrgb)
    {
        float r, g, b;
        string strsplit = ",";

        int idx = strrgb.IndexOf(strsplit);
        string str = strrgb.Substring(0, idx);
        int v = Common.String2Int(str);
        r = v / 255f;

        string strnew = strrgb.Substring(idx + 1);
        idx = strnew.IndexOf(strsplit);
        str = strnew.Substring(0, idx);
        v = Common.String2Int(str);
        g = v / 255f;


        str = strnew.Substring(idx + 1);
        v = Common.String2Int(str);
        b = v / 255f;


        Color color = new Color(r, g, b, 1f);
        return color;
    }


    static public Color RGBString2ColorA(string strrgb)
    {
        float r, g, b, a;
        string[] rgb = strrgb.Split(',');
        int v = 0;
        v = Common.String2Int(rgb[0]);
        r = v / 255f;

        v = Common.String2Int(rgb[1]);
        g = v / 255f;

        v = Common.String2Int(rgb[2]);
        b = v / 255f;

        
        a = 1f;
        if (rgb.Length > 3)
        {
            v = Common.String2Int(rgb[3]);
            a = v / 255f;
        }


        Color color = new Color(r, g, b, a);
        return color;
    }

    //随机打乱string
    static public string RandomString(string str)
    {
        string strret = "";
        int[] indexSel = Common.RandomIndex(str.Length, str.Length);
        for (int i = 0; i < indexSel.Length; i++)
        {
            int idx = indexSel[i];
            string strtmp = str.Substring(idx, 1);
            strret += strtmp;
        }
        return strret;
    }
    //从数组里随机抽取newsize个元素
    static public int[] RandomIndex(int size, int newsize)
    {
        List<object> listIndex = new List<object>();
        int total = size;
        for (int i = 0; i < total; i++)
        {
            listIndex.Add(i);
        }

        int[] idxTmp = new int[newsize];
        for (int i = 0; i < newsize; i++)
        {
            total = listIndex.Count;
            int rdm = Random.Range(0, total);
            int idx = (int)listIndex[rdm];
            idxTmp[i] = idx;
            listIndex.RemoveAt(rdm);
        }

        return idxTmp;
    }

    static public Rect GetRectOfBounds(Bounds bd)
    {
        return new Rect(bd.center.x - bd.size.x / 2, bd.center.y - bd.size.y / 2, bd.size.x, bd.size.y);
    }
    static public Camera GetMainCamera()
    {
        Camera camRet = null;
        GameObject obj = GameObject.Find("Main Camera");
        if (obj == null)
        {
            obj = GameObject.Find("MainCamera");
        }
        if (obj != null)
        {
            camRet = obj.GetComponent<Camera>();
        }
        return camRet;
    }
    #region screen_world switch

    static public Vector2 GetWorldSize(Camera cam)
    {
        float world_w = Common.GetCameraWorldSizeWidth(cam) * 2;
        float world_h = cam.orthographicSize * 2;
        return new Vector2(world_w, world_h);
    }

    static public Vector2 WorldToScreenSize(Camera cam, Vector2 size)
    {
        Vector2 ret = new Vector2(0f, 0f);
        ret.x = size.x * (Screen.height / 2) / cam.orthographicSize;
        ret.y = size.y * (Screen.height / 2) / cam.orthographicSize;
        return ret;
    }

    static public Vector2 ScreenToWorldSize(Camera cam, Vector2 size)
    {
        Vector2 ret = new Vector2(0f, 0f);
        ret.x = size.x * cam.orthographicSize / (Screen.height / 2);
        ret.y = size.y * cam.orthographicSize / (Screen.height / 2);
        return ret;
    }


    static public float ScreenToWorldWidth(Camera cam, float w)
    {
        float ret = 0;
        ret = w * cam.orthographicSize / (Screen.height / 2);
        return ret;
    }


    static public float WorldToScreenWidth(Camera cam, float w)
    {
        float ret = 0;
        ret = w * (Screen.height / 2) / cam.orthographicSize; ;
        return ret;
    }

    static public float WorldToCanvasWidth(Camera cam, Vector2 canvasSize, float w)
    {
        float w_screen = WorldToScreenWidth(cam, w);
        return ScreenToCanvasWidth(canvasSize, w_screen);
    }
    static public float WorldToCanvasHeight(Camera cam, Vector2 canvasSize, float h)
    {
        float h_screen = WorldToScreenHeight(cam, h);
        return ScreenToCanvasHeigt(canvasSize, h_screen);
    }
    static public float ScreenToWorldHeight(Camera cam, float h)
    {
        float ret = 0;
        ret = ScreenToWorldWidth(cam, h);
        return ret;
    }
    static public float WorldToScreenHeight(Camera cam, float h)
    {
        float ret = 0;
        ret = WorldToScreenWidth(cam, h);
        return ret;
    }

    static public float CanvasToScreenWidth(Vector2 canvasSize, float w)
    {
        float ret = w * Screen.width / canvasSize.x;
        //float image_screen_w = rcImageLeft.rect.width*Screen.width/sizeCanvas.x;
        return ret;
    }


    static public float CanvasToScreenHeight(Vector2 canvasSize, float h)
    {
        float ret = h * Screen.height / canvasSize.y;
        return ret;
    }


    static public Vector2 CanvasToScreenSize(Vector2 canvasSize, Vector2 rectSize)
    {
        float w = CanvasToScreenWidth(canvasSize, rectSize.x);
        float h = CanvasToScreenHeight(canvasSize, rectSize.y);
        return new Vector2(w, h);
    }

    static public float CanvasToWorldWidth(Camera cam, Vector2 canvasSize, float w)
    {
        float screen = CanvasToScreenWidth(canvasSize, w);
        float ret = ScreenToWorldWidth(cam, screen);
        return ret;
    }
    static public float CanvasToWorldHeight(Camera cam, Vector2 canvasSize, float h)
    {
        float screen = CanvasToScreenHeight(canvasSize, h);
        float ret = ScreenToWorldHeight(cam, screen);
        return ret;
    }


    static public float ScreenToCanvasWidth(Vector2 canvasSize, float w)
    {
        float ret = w * canvasSize.x / Screen.width;
        //float image_screen_w = rcImageLeft.rect.width*Screen.width/sizeCanvas.x;
        return ret;
    }


    static public float ScreenToCanvasHeigt(Vector2 canvasSize, float h)
    {
        float ret = h * canvasSize.y / Screen.height;
        return ret;
    }

    static public Vector2 ScreenToCanvasSize(Vector2 canvasSize, Vector2 rectSize)
    {
        float w = ScreenToCanvasWidth(canvasSize, rectSize.x);
        float h = ScreenToCanvasHeigt(canvasSize, rectSize.y);
        return new Vector2(w, h);
    }


    static public Rect WorldToScreenRect(Camera cam, Rect rc)
    {
        Vector2 pt = cam.WorldToScreenPoint(new Vector2(rc.x, rc.y));
        Vector2 size = WorldToScreenSize(cam, new Vector2(rc.width, rc.height));
        return new Rect(pt.x, pt.y, size.x, size.y);
    }

    static public Rect ScreenToWorldTRect(Camera cam, Rect rc)
    {
        Vector2 pt = cam.ScreenToWorldPoint(new Vector2(rc.x, rc.y));
        Vector2 size = ScreenToWorldSize(cam, new Vector2(rc.width, rc.height));
        return new Rect(pt.x, pt.y, size.x, size.y);
    }

    //坐标原点在屏幕左下角
    static public Vector2 WorldToCanvasPoint(Camera cam, Vector2 canvasSize, Vector2 pt)
    {
        Vector2 ptCanvas = Vector2.zero;
        Vector2 ptScreen = cam.WorldToScreenPoint(pt);
        ptCanvas = ScreenToCanvasPoint(canvasSize, ptScreen);
        return ptCanvas;
    }

    //坐标原点在屏幕左下角
    static public Vector2 ScreenToCanvasPoint(Vector2 canvasSize, Vector2 pt)
    {
        Vector2 ptCanvas = Vector2.zero;
        ptCanvas.x = pt.x * canvasSize.x / Screen.width;
        ptCanvas.y = pt.y * canvasSize.y / Screen.height;
        return ptCanvas;
    }

    static public Vector2 CanvasToScreenPoint(Vector2 canvasSize, Vector2 ptCanvas)
    {
        Vector2 pt = Vector2.zero;
        pt.x = ptCanvas.x * Screen.width / canvasSize.x;
        pt.y = ptCanvas.y * Screen.height / canvasSize.y;
        return pt;
    }
    //one screen width half  size  in the world
    static public float GetCameraWorldSizeWidth(Camera cam)
    {
        return (Screen.width * cam.orthographicSize / Screen.height);
    }


    #endregion

    static public Vector3 GetObjBoundSize(GameObject obj)
    {
        Renderer render = obj.GetComponent<Renderer>();
        Bounds bound = render.bounds;
        Vector3 size = bound.size;
        Vector3 center = bound.center;
        return size;

    }
    static public Vector3 GetObjBoundCenter(GameObject obj)
    {
        Renderer render = obj.GetComponent<Renderer>();
        Bounds bound = render.bounds;
        Vector3 center = bound.center;
        return center;

    }

    #region string
    static public int GetStringLength(string text, string fontName, int fontSize)
    {

        //TextTest.font;
        Font font = Font.CreateDynamicFontFromOSFont(fontName, fontSize);//new Font("Arial");// Resources.Load<Font>("FZCQJW");
                                                                         // Font font =  new Font("Arial");
                                                                         //font.dynamic = true;


        // List <CharacterInfo> _list = new List<CharacterInfo>();
        // 	for(int i=0; i<text.Length;i++){
        // 	CharacterInfo info = new CharacterInfo();
        // 	info.index = i;
        //     info.size = fontsize;
        //     info.style = FontStyle.Normal;
        //     info.advance = fontsize;
        //     _list.Add(info);
        // }
        // font.characterInfo = _list.ToArray();

        font.RequestCharactersInTexture(text, fontSize, FontStyle.Normal);
        //Debug.Log("font.fontSize : " + font.fontSize);
        CharacterInfo characterInfo;
        // font.GetCharacterInfo
        int width = 0;
        for (int i = 0; i < text.Length; i++)
        {

            font.GetCharacterInfo(text[i], out characterInfo, fontSize);
            //width+=characterInfo.width; unity5.x提示此方法将来要废弃
            //Debug.Log(i + " advance : " + characterInfo.advance);
            width += characterInfo.advance;
        }

        //Debug.Log("width : " + width);
        return width;
    }
    #endregion
    static public Vector2 GetInputPosition()
    {
        Vector2 pos = Vector2.zero;
        if (Input.touchCount > 0)
        {
            pos = Input.touches[0].position;
        }
        else
        {
            pos = Input.mousePosition;
        }
        return pos;
    }
    static public Vector3 GetInputPositionWorld(Camera cam)
    {
        Vector2 pos = GetInputPosition();
        Vector3 posWorld = cam.ScreenToWorldPoint(pos);
        return posWorld;
    }


    static public string GetAppName()
    {
        CommonBasePlatformWrapper platformWrapper = CommonPlatformWrapper.platform;
        return platformWrapper.GetAppName();
    }
    static public string GetAppNameDisplay()
    {
        string appname = Common.GetAppName();
        if (Application.isEditor || Common.isPC)
        {
            // appname = Config.main.GetAppNameJson(Device.isLandscape);
        }
        appname = Language.main.GetString(Device.isLandscape ? AppString.APP_NAME_HD : AppString.APP_NAME);


        //去除hd
        int idxtmp = appname.IndexOf("HD");
        if (idxtmp > 0)
        {
            string strtmp = appname.Substring(0, idxtmp);
            appname = strtmp;
        }

        return appname;
    }
    static public string GetCachePath()
    {
        CommonBasePlatformWrapper platformWrapper = CommonPlatformWrapper.platform;
        return platformWrapper.GetCachePath();

    }

    static public void CleanCache()
    {
        if (Common.isWeb)
        {
            return;
        }
        string path = GetCachePath();
        if (BlankString(path))
        {
            return;
        }
        // C#遍历指定文件夹中的所有文件 
        DirectoryInfo TheFolder = new DirectoryInfo(path);
        // //遍历文件夹
        // foreach(DirectoryInfo NextFolder in TheFolder.GetDirectories())
        //    this.listBox1.Items.Add(NextFolder.Name);
        // //遍历文件
        foreach (FileInfo NextFile in TheFolder.GetFiles())
        {
            NextFile.Delete();

        }
        //    this.listBox2.Items.Add(NextFile.Name);

    }
    static public string GetAppPackage()
    {
        CommonBasePlatformWrapper platformWrapper = CommonPlatformWrapper.platform;
        return platformWrapper.GetAppPackage();

    }
    static public string GetAppVersion()
    {
        CommonBasePlatformWrapper platformWrapper = CommonPlatformWrapper.platform;
        return platformWrapper.GetAppVersion();

    }
    static public string GetChannelName()
    {
        CommonBasePlatformWrapper platformWrapper = CommonPlatformWrapper.platform;
        return platformWrapper.GetChannelName();

    }


    //android 启动logo结束
    static public void UnityStartUpFinish()
    {
        CommonBasePlatformWrapper platformWrapper = CommonPlatformWrapper.platform;
        platformWrapper.UnityStartUpFinish();

    }

    static public void EnableAdSplash()
    {
        CommonBasePlatformWrapper platformWrapper = CommonPlatformWrapper.platform;
        platformWrapper.EnableAdSplash();

    }

    static public void SetIpInChina(bool isin)
    {
        CommonBasePlatformWrapper platformWrapper = CommonPlatformWrapper.platform;
        platformWrapper.SetIpInChina(isin);

    }
    static public bool JsonDataContainsKey(JsonData data, string key)
    {
        return JsonUtil.ContainsKey(data, key);
    }

    static public bool BlankString(string str)
    {
        return isBlankString(str);
    }
    static public bool isBlankString(string str)
    {
        bool ret = false;
        if (str == null)
        {
            ret = true;
            return ret;
        }
        if ((str.Length == 0) || (str == " "))
        {
            ret = true;
        }
        return ret;
    }


    static public long GetCurrentTimeMs()
    {
        //100 毫微秒
        long tick = System.DateTime.Now.Ticks;
        long second = tick / (10000);
        return second;
    }
    static public long GetCurrentTimeSecond()
    {
        //100 毫微秒
        long tick = System.DateTime.Now.Ticks;
        long second = tick / (10000000);
        return second;
    }
    //获取当前使用第几天 从1开始
    static public int GetDayIndexOfUse()
    {
        int index = 1;
        long second_first_use = 0;

        string pkey_firstuse_second = "key_app_first_use_second_" + GetAppVersion();
        string pkeyfirstuse = "key_app_first_use_" + GetAppVersion();
        bool isfirstuse = !GetBool(pkeyfirstuse);

        if (isfirstuse)
        {
            SetBool(pkeyfirstuse, true);
            second_first_use = GetCurrentTimeSecond();

            string strSecond = second_first_use.ToString();
            PlayerPrefs.SetString(pkey_firstuse_second, strSecond);
        }


        string str = PlayerPrefs.GetString(pkey_firstuse_second);
        long l = 0;
        long.TryParse(str, out l);
        second_first_use = l;

        long second = GetCurrentTimeSecond();
        index = (int)((second - second_first_use) / (3600 * 24));
        if (index < 0)
        {
            index = 0;
        }
        index++;

        return index;
    }

    static public void CaptureScreen()
    {
        ScreenCapture.CaptureScreenshot("Screenshot.png", 0);
    }
    // <summary>  
    /// Captures the screenshot2.  
    /// </summary>  
    /// <returns>The screenshot2.</returns>  
    /// <param name="rect">Rect.截图的区域，左下角为o点</param>  
    static public Texture2D CaptureScreenshotRect(Rect rect, string savepath)
    {
        // 先创建一个的空纹理，大小可根据实现需要来设置  
        Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);

        // 读取屏幕像素信息并存储为纹理数据，  
        screenShot.ReadPixels(rect, 0, 0);
        screenShot.Apply();

        // 然后将这些纹理数据，成一个png图片文件  
        byte[] bytes = screenShot.EncodeToPNG();
        string filename = Application.dataPath + "/Screenshot.png";
        if (savepath != null)
        {
            filename = savepath;
        }
        System.IO.File.WriteAllBytes(filename, bytes);
        Debug.Log(string.Format("截屏了一张图片: {0}", filename));

        // 最后，我返回这个Texture2d对象，这样我们直接，所这个截图图示在游戏中，当然这个根据自己的需求的。  
        return screenShot;
    }


    /// <summary>  
    /// 对相机截图。   
    /// </summary>  
    /// <returns>The screenshot2.</returns>  
    /// <param name="camera">Camera.要被截屏的相机</param>  
    /// <param name="rect">基于 size 像素坐标的Rect.截屏的区域</param>  
    //size 为纹理大小
    static public Texture2D CaptureCamera(Camera camera, Rect rect, string savepath, Vector2 size)
    {
        // 创建一个RenderTexture对象  
        //RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0); 
        Rect camPixselRectOrign = camera.pixelRect;
        //@moon RenderTexture 如果是屏幕的话必须为屏幕大小，不然局部截图时候图片会出错
        RenderTexture rt = new RenderTexture((int)size.x, (int)size.y, 0);
        // 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机  
        camera.pixelRect = new Rect((Screen.width - size.x) / 2, (Screen.height - size.y) / 2, size.x, size.y);
        //camera.pixelRect = new Rect((Screen.width-size.x)/2, (Screen.height-size.y)/2, size.x, size.y);
        camera.targetTexture = rt;
        camera.Render();
        //ps: --- 如果这样加上第二个相机，可以实现只截图某几个指定的相机一起看到的图像。  
        //ps: camera2.targetTexture = rt;  
        //ps: camera2.Render();  
        //ps: -------------------------------------------------------------------  

        // 激活这个rt, 并从中中读取像素。  
        RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGBA32, false);
        // rect.x =0;
        // rect.y = 0;
        screenShot.ReadPixels(rect, 0, 0);// 注：这个时候，它是从RenderTexture.active中读取像素  
        screenShot.Apply();

        // 重置相关参数，以使用camera继续在屏幕上显示  
        camera.targetTexture = null;
        //ps: camera2.targetTexture = null;  
        RenderTexture.active = null; // JC: added to avoid errors  
        camera.pixelRect = camPixselRectOrign;

        GameObject.Destroy(rt);
        // 最后将这些纹理数据，成一个png图片文件  
        byte[] bytes = screenShot.EncodeToPNG();
        string filename = Application.dataPath + "/Screenshot.png";
        if (savepath != null)
        {
            filename = savepath;
        }
        System.IO.File.WriteAllBytes(filename, bytes);
        Debug.Log(string.Format("截屏了一张照片: {0}", filename));

        return screenShot;
    }
    static public Text GetButtonText(Button btn)
    {
        Transform tr = btn.transform.Find("Text");
        Text btnText = tr.GetComponent<Text>();
        return btnText;
    }

    static public float GetButtonTextWidth(Button btn, string str)
    {
        Transform tr = btn.transform.Find("Text");
        Text btnText = tr.GetComponent<Text>();
        float w = Common.GetStringLength(str, AppString.STR_FONT_NAME, btnText.fontSize);
        return w;
    }

    static public void SetButtonTextWidth(Button btn, string str, float w, bool isFitWidth = true)
    {
        Transform tr = btn.transform.Find("Text");
        Text btnText = tr.GetComponent<Text>();
        btnText.text = str;
        if (isFitWidth)
        {
            RectTransform rctran = btn.transform as RectTransform;
            Vector2 sizeDelta = rctran.sizeDelta;
            sizeDelta.x = w;
            rctran.sizeDelta = sizeDelta;
        }
    }

    static public void SetButtonText(Button btn, string str, float offset = 0, bool isFitWidth = true)
    {
        Transform tr = btn.transform.Find("Text");
        Text btnText = tr.GetComponent<Text>();
        float str_w = Common.GetStringLength(str, AppString.STR_FONT_NAME, btnText.fontSize);
        float oft;
        if (offset < 0)
        {
            oft = 0;
        }
        else
        {
            oft = offset;
        }

        SetButtonTextWidth(btn, str, str_w + oft, isFitWidth);

    }



    static public void SetOrientation(int orientaion)
    {
        CommonBasePlatformWrapper platformWrapper = CommonPlatformWrapper.platform;
        platformWrapper.SetOrientation(orientaion);
    }

}
