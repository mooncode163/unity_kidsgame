using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public delegate void OnUIWordSVGAnimateDelegate(UIWordSVG ui, int index);
public class UIWordSVG : UIView
{

    public enum Type
    {
        NONE = 0,// all words
        ALL_STROKE,// all words
        ONE_STROKE,
        TO_STROKE,
        DEMO,//  
    }

    UIViewSVG uiViewSVGPrefab;
    List<UIViewSVG> listItem = new List<UIViewSVG>();
    WordItemInfo infoWord;
    float runningTime;
    float timeStep = 0.4f;//second 0.2
    public int indexStroke;
    public int indexStrokePre;
    int indexPoint;
    public float scaleWord = 1f;
    UIViewSVG.RenderMode renderMode;
    public Color colorWord = Color.green;
    public OnUIWordSVGAnimateDelegate callBackAnimate { get; set; }
    Type _type = Type.NONE;
    public Type type
    {
        get
        {
            return _type;
        }
        set
        {
            _type = value;
            switch (_type)
            {
                case Type.DEMO:
                    {

                    }
                    break;

            }
        }
    }


    /// <summary>
    /// Unity's Awake method.
    /// </summary>
    public void Awake()
    {
        runningTime = 0;
        indexPoint = 0;
        indexStroke = 0;
        indexStrokePre = -1;
        // type = Type.DEMO;
        // Load(Common.GAME_RES_DIR + "/他1.svg");
    }

    /// <summary>
    /// Unity's Start method.
    /// </summary>
    public void Start()
    {
        LayOut();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if ((_type == Type.DEMO) && (infoWord != null))
        {
            runningTime += Time.deltaTime;
            if (runningTime > timeStep)
            {
                runningTime = 0;
                if (indexStroke == 0)
                {
                    //clear
                    HideAll();
                }
                UpdateAnimate(indexStroke, indexPoint);
                indexPoint++;
                if (indexPoint >= ParseWordPointInfo.main.GetWordMedianPointCount(infoWord.pointInfo, indexStroke))
                {
                    indexPoint = 0;
                    indexStroke++;
                    if (indexStroke >= infoWord.pointInfo.listStroke.Count)
                    {
                        //repeate all
                        indexPoint = 0;
                        indexStroke = 0;
                    }
                }

            }

        }
    }


    void UpdateAnimate(int idxStroke, int indexEnd)
    {
        indexStroke = idxStroke;
        UIViewSVG ui = listItem[indexStroke] as UIViewSVG;
        ui.gameObject.SetActive(true);

        if (indexStroke != indexStrokePre)
        {
            if (callBackAnimate != null)
            {
                callBackAnimate(this, indexStroke);
            }
            indexStrokePre = indexStroke;
        }
        // svg里固定 Color.gray
        string svg = ParseWordPointInfo.main.GetWordMedianSVG(infoWord.pointInfo, indexStroke, 0, indexEnd, Color.gray, 100);
        // Debug.Log("svg = "+svg);
        ui.LoadData(svg);
        // 动画笔画不需要旋转 bug 
        ui.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

        // 实际显示颜色
        ui.SetColor(colorWord);

        float scale = scaleWord;
        ui.transform.localScale = new Vector3(scale, scale, 1.0f);
        svg = ParseWordPointInfo.main.GetWordSVG(infoWord.pointInfo, indexStroke);
        Texture2D texMask = ui.GetTexture(svg);
        ui.SetTextureMask(texMask);

    }

    public void HideAll()
    {
        foreach (UIViewSVG ui in listItem)
        {
            ui.gameObject.SetActive(false);
        }

    }

    public void Clear()
    {
        foreach (UIViewSVG ui in listItem)
        {
            DestroyImmediate(ui.gameObject);
        }
        listItem.Clear();
    }


    public void LoadWord(WordItemInfo info)
    {
        infoWord = info;
        if (uiViewSVGPrefab == null)
        {
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/SVG/UIViewSVG");
            if (obj != null)
            {
                uiViewSVGPrefab = obj.GetComponent<UIViewSVG>();
            }
        }

        Clear();
        if (_type == Type.NONE)
        {
            return;
        }
        for (int i = 0; i < info.pointInfo.listStroke.Count; i++)
        {
            if (_type == Type.ONE_STROKE)
            {
                if (i != indexStroke)
                {
                    continue;
                }
            }
            if (_type == Type.TO_STROKE)
            {
                if (i > indexStroke)
                {
                    continue;
                }
            }
            LoadWordStroke(info, i);
            // UIViewSVG ui = (UIViewSVG)GameObject.Instantiate(uiViewSVGPrefab);
            // UIViewController.ClonePrefabRectTransform(uiViewSVGPrefab.gameObject, ui.gameObject);
            // ui.transform.SetParent(this.transform, false);
            // ui.renderMode = renderMode;
            // listItem.Add(ui);
            // string svg = ParseWordPointInfo.main.GetWordSVG(info.pointInfo, i);
            // ui.LoadData(svg);
            // // 实际显示颜色
            // ui.SetColor(colorWord);
            // // ui.LoadFile(Common.GAME_RES_DIR + "/svg/stroke_test.svg");
        }

        LayOut();

    }


    public void LoadWordStroke(WordItemInfo info, int index)
    {
        infoWord = info;
        if (uiViewSVGPrefab == null)
        {
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/SVG/UIViewSVG");
            if (obj != null)
            {
                uiViewSVGPrefab = obj.GetComponent<UIViewSVG>();
            }
        }
        {


            UIViewSVG ui = (UIViewSVG)GameObject.Instantiate(uiViewSVGPrefab);
            UIViewController.ClonePrefabRectTransform(uiViewSVGPrefab.gameObject, ui.gameObject);
            ui.transform.SetParent(this.transform, false);
            ui.renderMode = renderMode;
            if (renderMode == UIViewSVG.RenderMode.CANVAS)
            {
                // CANVAS不需要旋转 bug 
                ui.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            listItem.Add(ui);
            string svg = ParseWordPointInfo.main.GetWordSVG(info.pointInfo, index);
            ui.LoadData(svg);
            // 实际显示颜色
            ui.SetColor(colorWord);

            float scale = scaleWord;
            ui.transform.localScale = new Vector3(scale, scale, 1.0f);
            // ui.LoadFile(Common.GAME_RES_DIR + "/svg/stroke_test.svg");

            Vector2 size = ui.GetBoundSize();
            Debug.Log("UIViewSVG GetBoundSize size=" + size);
        }

        LayOut();

    }

    public void UpdateStrokeColor(int index, Color color)
    {
        UIViewSVG ui = listItem[index];
        ui.SetColor(color);
    }

    public void SetRenderMode(UIViewSVG.RenderMode mode)
    {
        renderMode = mode;
        if (listItem != null)
        {
            foreach (UIViewSVG ui in listItem)
            {
                ui.renderMode = renderMode;
            }
        }


    }
    public Texture2D GetTexture()
    {
        UIViewSVG ui = listItem[0];
        return ui.texSVG;
    }

    public Vector2 GetBoundWord()
    {
        UIViewSVG ui = listItem[0];
        return ui.GetBoundSize();
    }


    public override void LayOut()
    {
        base.LayOut();
        foreach (UIViewSVG ui in listItem)
        {
            float scale = scaleWord;
            ui.transform.localScale = new Vector3(scale, scale, 1.0f);
        }

        RectTransform rctan = this.GetComponent<RectTransform>();
        if (renderMode == UIViewSVG.RenderMode.CANVAS)
        {
            rctan.sizeDelta = new Vector2(StrokeMatche.IMAGE_SIZE, StrokeMatche.IMAGE_SIZE);
        }

        base.LayOut();
    }


}
