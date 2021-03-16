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
        ONE_STROKE,
        TO_STROKE,
        DEMO,//  
    }

    UIViewSVG uiViewSVGPrefab;
    List<UIViewSVG> listItem;
    WordItemInfo infoWord;
    float runningTime;
    float timeStep = 0.2f;//second
    public int indexStroke;
    int indexPoint;
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
        // type = Type.DEMO;
        // Load(CloudRes.main.rootPathGameRes + "/他1.svg");
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


    void UpdateAnimate(int indexStroke, int indexEnd)
    {
        UIViewSVG ui = listItem[indexStroke] as UIViewSVG;
        if (callBackAnimate != null)
        {
            callBackAnimate(this, indexStroke);
        }
        // svg里固定 Color.gray
        string svg = ParseWordPointInfo.main.GetWordMedianSVG(infoWord.pointInfo, indexStroke, 0, indexEnd, Color.gray, 100);
        // Debug.Log("svg = "+svg);
        ui.LoadData(svg);
        // 动画笔画不需要旋转 bug 
        ui.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

        // 实际显示颜色
        ui.SetColor(colorWord);

        svg = ParseWordPointInfo.main.GetWordSVG(infoWord.pointInfo, indexStroke);
        Texture2D texMask = ui.GetTexture(svg);
        ui.SetTextureMask(texMask);

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
        if (listItem == null)
        {
            listItem = new List<UIViewSVG>();
        }
        Clear();

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

            UIViewSVG ui = (UIViewSVG)GameObject.Instantiate(uiViewSVGPrefab);
            UIViewController.ClonePrefabRectTransform(uiViewSVGPrefab.gameObject, ui.gameObject);
            ui.transform.SetParent(this.transform, false);
            ui.renderMode = renderMode;
            listItem.Add(ui);
            string svg = ParseWordPointInfo.main.GetWordSVG(info.pointInfo, i);
            ui.LoadData(svg);
            // 实际显示颜色
            ui.SetColor(colorWord);
            // ui.LoadFile(CloudRes.main.rootPathGameRes + "/svg/stroke_test.svg");
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

    public Bounds GetBounds()
    {
        UIViewSVG ui = listItem[0];
        Bounds bd = ui.gameObject.GetComponent<Renderer>().bounds;
        return bd;
    }

    public UIViewSVG GetItemSVG(int idx)
    {
        UIViewSVG ui = listItem[idx]; 
        return ui;
    }
    public override void LayOut()
    {
        base.LayOut();

    }


}
