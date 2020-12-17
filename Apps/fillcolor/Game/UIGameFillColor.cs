using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LitJson;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//ps制作线稿教程：https://www.cnblogs.com/lrxsblog/p/6902377.html
public class UIGameFillColor : UIGameBase
{

    GameFillColor gameFillColorPrefab;
    public GameFillColor gameFillColor;
    public UIItemColorSelector colorSelector;
    public UIGameSelector gameSelector;
    public UIToolBar uiToolBar;
    public Button btnPopTool;
    public GameObject objPlanePaint;

    UIColorBoard uiColorBoardPrefab;
    public UIColorBoard uiColorBoard;
    UIColorInput uiColorInputPrefab;
    public UIColorInput uiColorInput;
    // UILineSetting uiLineSettingPrefab;
    // UILineSetting uiLineSetting;




    //color select
    List<Color> listColorSelect;


    List<ColorItemInfo> listColorJson;

    Camera mainCamera;
    bool isGameSelectorClose;

    int indexSprite;


    bool isHaveInitShader;
    bool isNeedUpdateSpriteFill;
    float colorBoardOffsetYNormal;
    void Awake()
    {

        LoadPrefab();
        mainCamera = AppSceneBase.main.mainCamera;
        isGameSelectorClose = false;
        isHaveInitShader = false;

        listColorSelect = new List<Color>();
        listColorSelect.Add(Color.red);
        listColorSelect.Add(Color.green);
        listColorSelect.Add(Color.blue);

        // colorSelector.callBackClick = OnUIItemColorSelectorDidClick;
        // colorSelector.index = 0;

        AppSceneBase.main.UpdateWorldBg(AppRes.IMAGE_GAME_BG);

        // ParseGuanka();

        ColorItemInfo info = GameLevelParse.main.GetItemInfo();

        //gameFillColor.Init(info);


        indexSprite = 0;

        uiToolBar.uiGameFillColor = this;
        gameSelector.uiGameFillColor = this;
        uiToolBar.gameObject.SetActive(false);
        UpdateBtnMusic();
        //ShowFPS();
    }
    // Use this for initialization
    void Start()
    {
        UpdateGuankaLevel(LevelManager.main.gameLevel);
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void OnGUI()
    {
        // GUIStyle bb = new GUIStyle();
        // bb.normal.background = null;    //这是设置背景填充的
        // bb.normal.textColor = new Color(1f, 0f, 0f);   //设置字体颜色的
        // bb.fontSize = 20;       //当然，这是字体大小
        // if (Common.isiOS || Common.isAndroid)
        // {
        //     bb.fontSize = bb.fontSize * 2;
        // }
        // //居中显示FPS
        // if (colorTouch != null)
        // {
        //     int r = (int)(colorTouch.r * 255);
        //     int g = (int)(colorTouch.g * 255);
        //     int b = (int)(colorTouch.b * 255);
        //     int a = (int)(colorTouch.a * 255);
        //     GUI.Label(new Rect(0, 40, 400, 200), "x=" + (int)ptColorTouch.x + " y=" + (int)ptColorTouch.y + "  " + r + "," + g + "," + b + ",a=" + a + " tick=" + tickDraw + "ms" + " tick2 = " + tick2 + "ms" + " cnt = " + listColorFill.Count + " ", bb);
        // }

    }

    public override void UpdateGuankaLevel(int level)
    {
        base.UpdateGuankaLevel(level);
        InitUI();
        isNeedUpdateSpriteFill = true;

        gameFillColor = (GameFillColor)GameObject.Instantiate(gameFillColorPrefab);
        gameFillColor.gameObject.transform.parent = AppSceneBase.main.objMainWorld.transform;
        gameFillColor.callBackStraw = OnGameClickStraw;
        gameFillColor.transform.localPosition = new Vector3(0, 0, -1);
        gameFillColor.transform.localScale = new Vector3(1f, 1f, 1f);

        uiColorInput.UpdateInitColor(gameFillColor.colorFill);
        UpdateColorSelect();
        OnGameWinBase();
    }
    void LoadPrefab()
    {

        // {
        //     GameObject obj = (GameObject)Resources.Load("AppCommon/Prefab/Game/UIColorBoard");
        //     if (obj != null)
        //     {
        //         uiColorBoardPrefab = obj.GetComponent<UIColorBoard>();
        //         uiColorBoard = (UIColorBoard)GameObject.Instantiate(uiColorBoardPrefab);
        //         uiColorBoard.gameObject.SetActive(false);
        //         GameObject objCanvas = AppSceneBase.main.canvasMain.gameObject;
        //         if (objCanvas != null)
        //         {
        //             uiColorBoard.gameObject.transform.parent = this.transform;
        //         }


        //         uiColorBoard.callBackClick = OnUIColorBoardDidClick;
        //         uiColorBoard.transform.localScale = new Vector3(1f, 1f, 1f);
        //         UIViewController.ClonePrefabRectTransform(uiColorBoardPrefab.gameObject, uiColorBoard.gameObject);
        //     }
        // }
        {
            //  GameObject obj = (GameObject)Resources.Load("AppCommon/Prefab/Game/UIColorInput");
            // if (obj != null)
            {
                // uiColorInputPrefab = obj.GetComponent<UIColorInput>();
                // uiColorInput = (UIColorInput)GameObject.Instantiate(uiColorInputPrefab);
                //  uiColorInput.gameObject.SetActive(false);

                // GameObject objCanvas = AppSceneBase.main.canvasMain.gameObject;
                // if (objCanvas != null)
                // {
                //     uiColorInput.gameObject.transform.parent = this.transform;
                // }

                //  uiColorInput.callBackUpdateColor = OnUIColorInputUpdateColor;
                //   uiColorInput.transform.localScale = new Vector3(1f, 1f, 1f);
                // UIViewController.ClonePrefabRectTransform(uiColorInputPrefab.gameObject, uiColorInput.gameObject);
            }
        }

        //gameFillColorPrefab
        {
            GameObject obj = (GameObject)Resources.Load("AppCommon/Prefab/Game/GameFillColor");
            if (obj != null)
            {
                gameFillColorPrefab = obj.GetComponent<GameFillColor>();
                //在awake修改位置不起作用，需要放在start
                // gameFillColor.transform.position = new Vector3(0, 0, gameFillColor.transform.position.z);
                // gameFillColor.transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }



    }

    void InitUI()
    {

        uiColorInput.gameObject.SetActive(false);
        uiColorInput.callBackUpdateColor = OnUIColorInputUpdateColor;


        uiColorBoard.gameObject.SetActive(false);
        uiColorBoard.callBackClick = OnUIColorBoardDidClick;
        // topBarOffsetYNormal = objLayoutBtn.GetComponent<RectTransform>().offsetMax.y;
        //colorBoardOffsetYNormal = objLayoutColorBoard_H.GetComponent<RectTransform>().offsetMax.y;

        gameSelector.callbackClose = OnUIGameSelectorDidClose;
        gameSelector.Show(true);

        //OnUIItemColorSelectorDidClick(colorSelector);

        LayOut();

        OnUIDidFinish();
    }


    public override void LayOut()
    {
        float x = 0, y = 0, z = 0, w = 0, h = 0;
        if (gameFillColor != null)
        {
            gameFillColor.LayOut();
        }
    }



    void LoadGamePic(bool isNew)
    {
        ColorItemInfo info = GameLevelParse.main.GetItemInfo();
        gameFillColor.LoadGamePic(info, isNew);
    }


    Color RGBString2Color(string strrgb)
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

    Rect RectString2Rect(string strrect)
    {
        float x, y, w, h;
        x = 0;
        y = 0;
        w = 0;
        h = 0;
        string[] sArray = strrect.Split(',');
        int idx = 0;
        foreach (string str in sArray)
        {
            if (idx == 0)
            {
                x = Common.String2Int(str);
            }
            if (idx == 1)
            {
                y = Common.String2Int(str);
            }
            if (idx == 2)
            {
                w = Common.String2Int(str);
            }
            if (idx == 3)
            {
                h = Common.String2Int(str);
            }

            idx++;
        }
        Rect rc = new Rect(x, y, w, h);
        return rc;
    }
    void ParseColorJson()
    {
        listColorJson = new List<ColorItemInfo>();
        ColorItemInfo info = GameLevelParse.main.GetItemInfo();
        string json = FileUtil.ReadStringAsset(info.colorJson);//((TextAsset)Resources.Load(fileName, typeof(TextAsset))).text;
        string filePath = Application.streamingAssetsPath + "/" + info.colorJson;
        //  System.IO.StreamReader file = new System.IO.StreamReader(filePath);//读取文件中的数据  
        // json = file.ReadToEnd(); 

        Debug.Log("json:" + json + " size = " + json.Length);
        JsonData root = JsonMapper.ToObject(json);
        //strPlace = (string)root["place"];
        JsonData items = root["items"];
        Debug.Log("items count::" + items.Count);
        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];
            string strcolor = (string)item["color"];
            string strrect = (string)item["rect"];
            ColorItemInfo infocolor = new ColorItemInfo();
            infocolor.colorFill = RGBString2Color(strcolor);
            infocolor.rectFill = RectString2Rect(strrect);
            listColorJson.Add(infocolor);
        }
    }


    UIItemColorSelector GetColorSelect()
    {
        UIItemColorSelector sel = colorSelector;
        if (colorSelector.IsSelect())
        {
            sel = colorSelector;
        }
        // if (colorSelector1.IsSelect())
        // {
        //     sel = colorSelector1;
        // }
        // if (colorSelector2.IsSelect())
        // {
        //     sel = colorSelector2;
        // }

        return sel;

    }

    void UpdateColorSelect()
    {
        // colorSelector.UpdateColor(colorFill);
        uiToolBar.UpdateColor(gameFillColor.colorFill);
    }

    public void OnUIItemColorSelectorDidClick(UIItemColorSelector ui)
    {
        if (ui == colorSelector)
        {
            colorSelector.SetSelect(true);
            // colorSelector1.SetSelect(false);
            // colorSelector2.SetSelect(false);

        }

        //   OnClickBtnColorInput();
        // if (ui == colorSelector1)
        // {
        //     colorSelector0.SetSelect(false);
        //     colorSelector1.SetSelect(true);
        //     colorSelector2.SetSelect(false);
        // }
        // if (ui == colorSelector2)
        // {
        //     colorSelector0.SetSelect(false);
        //     colorSelector1.SetSelect(false);
        //     colorSelector2.SetSelect(true);
        // }

    }


    public void OnGameClickStraw()
    {
        UpdateColorSelect();
    }
    public void OnUIColorBoardDidClick(UIColorBoard ui, UIColorBoardCellItem item, bool isOutSide)
    {
        if (isOutSide)
        {

        }
        else
        {
            gameFillColor.colorFill = item.color;
            UpdateColorSelect();
        }

        uiColorBoard.gameObject.SetActive(false);
    }
    public void OnUIColorInputUpdateColor(Color color)
    {
        gameFillColor.colorFill = color;
        UpdateColorSelect();
    }

    public void OnUIGameSelectorDidClose(UIGameSelector ui, bool isNew)
    {
        isGameSelectorClose = true;
        LoadGamePic(isNew);
        LayOut();

    }



    void UpdateMaterial()
    {
        /*
        向shader传递数组时候 shader里数组大小不能过大,否则shader会出错。android设备测试通过数组大小有:256 500
        shader error:
        -------- GLSL link error: WARNING: Output of vertex shader 'vs_TEXCOORD0' not read by fragment shader
ERROR: Implementation limit of 4096 (e.g., number of built-in plus user defined active uniforms components) exceeded, fragment shader uses 4100 total uniforms.

         */

        /* 
       Renderer spRender = objPlanePaint.GetComponent<Renderer>();
       int count = listColorFill.Count;
       ColorItemInfo infolast = listColorFill[listColorFill.Count - 1];
       List<Color> listMask = new List<Color>();
       List<Color> listFill = new List<Color>();
       int idx = 0;
       foreach (ColorItemInfo info in listColorFill)
       {

           {

               listMask.Add(info.colorMask);
           }

           {

               listFill.Add(info.colorFill);
           }

           idx++;
       }

       //  string strshader = "Custom/FillColorArray";
       // shaderFill = Shader.Find(strshader);
       //if(isHaveInitShader==false)
       {
           isHaveInitShader = true;
           spRender.material = new Material(shaderFill);
       }
       Material mat = spRender.material;
       mat.SetTexture("_MainTex", texPic);
       mat.SetTexture("_TexMask", texPicMask);

       mat.SetColor("_ColorMask", infolast.colorMask);
       mat.SetColor("_ColorFill", infolast.colorFill);

       // count = 500;
       mat.SetInt("_ListCount", count);
       //     mat.SetInt("_drawCount",0);

       mat.SetColorArray("_ListColorMask", listMask);
       mat.SetColorArray("_ListColorFill", listFill);

       //  Color[] _ListColorFill = new Color[1024];
       //  idx = 0;
       // foreach (ColorItemInfo info in listColorFill)
       // {

       //         _ListColorFill[idx] = info.colorFill;


       //     idx++;
       // }
       //  mat.SetColorArray("_ListColorFill",_ListColorFill);




       // //Color[] listColor = new Color[10];
       //  List<Color> listColor = new List<Color>();
       //  listColor.Add(Color.green);
       //         //m_color[0] = Color.green;
       //            if(mat != null)
       //         {
       //             //listColor[0] = Color.green;
       //             //listColor[1] = Color.blue;
       //             mat.SetColorArray("_ListColorMask", listColor);
       //         }

       //spRender.material = mat;

*/

    }





    public void OnClickSpriteBg()
    {
        Debug.Log("OnClickSpriteBg");
        if (uiColorBoard.gameObject.activeSelf)
        {
            uiColorBoard.gameObject.SetActive(false);
        }
    }

    public void OnClickBtnPopTool()
    {
        uiToolBar.gameObject.SetActive(!uiToolBar.gameObject.activeSelf);
    }

}

