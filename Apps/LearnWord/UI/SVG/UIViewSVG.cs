using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


// [RequireComponent(typeof(Renderer))]
// [DisallowMultipleComponent()]
// [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class UIViewSVG : MeshTexture
{
    public Texture2D texSVG;
    public enum RenderMode
    {
        CANVAS = 0,// 
        WORLD,//  
    }

    public Image image;

    public TextAsset SVGFile = null;
    [Tooltip("Use a faster rendering approach that takes notably more memory.")]
    public bool fastRenderer = false;

    [Space(15)]
    public TextureWrapMode wrapMode = TextureWrapMode.Clamp;
    public FilterMode filterMode = FilterMode.Trilinear;
    [Range(0, 9)]
    public int anisoLevel = 9;

    Material matSVG;
    public RenderMode renderMode;

    Color colorSVG = Color.black;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public void Awake()
    {
        base.Awake();
        matSVG = new Material(Shader.Find("Custom/SVGMeshTexture"));
        // matSVG.SetColor("_Color", Color.gray);

        if (image != null)
        {
            image.material = matSVG;
        }
        UpdateMaterial(matSVG);
        renderMode = RenderMode.CANVAS;
        matSVG.SetFloat("_EnableMask", 0);

        SetColor(colorSVG);
    }
    /// <summary>
    /// Unity's Start method.
    /// </summary>
    public void Start()
    {
        //   AddPoint(Vector3.zero); 
        //LoadFile(CloudRes.main.rootPathGameRes +"/他1.svg");
        LayOut();
    }


    public void SetColor(Color color)
    {
        colorSVG = color;
        matSVG.SetColor("_Color", color);
        meshRender.material.SetColor("_Color", color);
        UnityEngine.Debug.Log("SetColor svg color=" + color);

        if (renderMode == RenderMode.WORLD)
        {
            // UpdateMaterial(matSVG);
           // Draw();
        }
    }

    public void SetTextureMask(Texture2D mask)
    {
        //    _MainTex
        matSVG.SetTexture("_TexMask", mask);
        matSVG.SetFloat("_EnableMask", 1);
    }


    public void LoadFile(string filepath)
    {
        byte[] data = FileUtil.ReadDataAsset(filepath);
        string strfile = Encoding.UTF8.GetString(data);
        LoadData(strfile);
    }

    public Texture2D GetTexture(string strsvg)
    {
        SVGFile = new TextAsset(strsvg);
        Texture2D tex = null;
        if (SVGFile != null)
        {
            Stopwatch w = new Stopwatch();

            w.Reset();
            w.Start();
            ISVGDevice device;
            if (fastRenderer)
                device = new SVGDeviceFast();
            else
                device = new SVGDeviceSmall();
            var implement = new Implement(SVGFile, device);
            w.Stop();
            long c = w.ElapsedMilliseconds;

            w.Reset();
            w.Start();
            implement.StartProcess();
            w.Stop();
            long p = w.ElapsedMilliseconds;

            w.Reset();
            w.Start();
            var myRenderer = GetComponent<Renderer>();
            tex = implement.GetTexture();

            // string savepath = GetRootDirSaveIcon(false) + "/svg.png";
            // UnityEngine.Debug.Log("svg savepath=" + savepath);
            // TextureUtil.SaveTextureToFile(texSVG, savepath);

            w.Stop();


        }
        return tex;

    }
    public void LoadData(string strsvg)
    {
        SVGFile = new TextAsset(strsvg);
        texSVG = GetTexture(strsvg);
        if (renderMode == RenderMode.CANVAS)
        {
            image.gameObject.SetActive(true);
            //ui 
            TextureUtil.UpdateImageTexture(image, texSVG, true);
        }
        else
        { 
            image.gameObject.SetActive(false);
            UpdateTexture(texSVG);
        }
        SetColor(colorSVG);
    }
    public string GetRootDirSaveIcon(bool ishd)
    {
        string name = ishd ? "iconhd" : "icon";
        string ret = UIScreenShotController.GetRootDirOutPut() + "/" + name;
        return ret;
    }

    public override void LayOut()
    {
        base.LayOut();

    }


}
