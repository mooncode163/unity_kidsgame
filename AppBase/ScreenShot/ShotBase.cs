using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public delegate void OnShotPageFinishDelegate(ShotItemInfo info);

public class ShotItemInfo
{
    public bool isRealGameUI;//
    public string functionName;
    public int index;
    public UIViewController controller;
}
public class ShotBase : MonoBehaviour
{
    public const string STR_DIR_ROOT_SCREENSHOT = Common.GAME_DATA_DIR + "/screenshot";
    public const string STR_DIR_ROOT_ICON = STR_DIR_ROOT_SCREENSHOT + "/icon";
    public const string STR_DIR_ROOT_SHADER = STR_DIR_ROOT_SCREENSHOT + "/shader";
    public const string STR_PIC_BG_ICON = STR_DIR_ROOT_ICON + "/bg.jpg";

    public GameObject objSpriteBg;
    public Camera mainCamera;
    public int indexShot;
    public int totalShot = 5;
    public List<string> listBgPic;
    public List<GameObject> listItem;
    public List<object> listGuankaOfAllPlace;
    public List<ShotItemInfo> listPage;
    public Language language;
    public Language languageShot;
    // public GameBase gameBase;
    // public GameBase gameBaseRun;
    public ShotDeviceInfo deviceInfo;

    static public int roundRectRadiusIcon = 156;//icon圆角半径

    public OnShotPageFinishDelegate callbackShotPageFinish { get; set; }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init()
    {
        listBgPic = new List<string>();
        listItem = new List<GameObject>();
        listPage = new List<ShotItemInfo>();


        language = new Language();
        language.Init(Common.GAME_DATA_DIR + "/language/language.csv");
        language.SetLanguage(Language.main.GetLanguage());


        string strFile = STR_DIR_ROOT_SCREENSHOT + "/language/language.csv";
        if (FileUtil.FileIsExistAsset(strFile))
        {
            languageShot = new Language();
            languageShot.Init(strFile);
            languageShot.SetLanguage(Language.main.GetLanguage());
        }
        else
        {
            languageShot = language;
        }

        // gameBase = GameScene.gameBase;
        // gameBase.ParseGuanka();

        // listGuankaOfAllPlace = LevelManager.GetGuankaListOfAllPlace();

    }

    public string GetTitle(int idx)
    {
        //language.SetLanguage(Language.main.GetLanguage());
        languageShot.SetLanguage(Language.main.GetLanguage());
        return languageShot.GetString("SCREENSHOT_TITLE_" + idx);
    }

    public string GetTitleAdHome()
    {
        language.SetLanguage(Language.main.GetLanguage());
        return language.GetString("APP_NAME");
    }
    public Texture2D LoadTex(string pic)
    {
        Texture2D tex = null;
        if (FileUtil.FileIsExistAsset(pic))
        {
            tex = LoadTexture.LoadFromAsset(pic);
        }
        else
        {
            tex = LoadTexture.LoadFromResource(pic);
        }
        return tex;
    }
    public void ClearAllChild()
    {
        foreach (GameObject obj in listItem)
        {
            // Debug.Log("Destroy Sub Child");
            DestroyImmediate(obj); 
            GameObject objtmp = obj;
            objtmp = null;
        }
    }

    public Vector2 GetPositionOfRect(Rect rc, int row, int col, int row_i, int col_j)
    {
        float x = 0, y = 0, w, h;
        w = rc.size.x / col;
        h = rc.size.y / row;
        x = rc.x + w / 2 + w * col_j;
        y = rc.y + h / 2 + h * row_i;
        return new Vector2(x, y);
    }
    // public void LoadGame()
    // {
    //     gameBaseRun = (GameBase)GameObject.Instantiate(gameBase);
    //     gameBase.mainCamera = mainCamera;
    //     gameBaseRun.mainCamera = mainCamera;
    //     gameBaseRun.Init(); 
    // }

    public void OnShotPageFinish(ShotItemInfo info)
    {
        if (callbackShotPageFinish != null)
        {
            callbackShotPageFinish(info);
        }
    }

    public virtual void ShowPage(int idx)
    {
    }
}
