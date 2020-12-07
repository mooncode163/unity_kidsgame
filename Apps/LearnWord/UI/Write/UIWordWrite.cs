using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vectrosity;
public interface IUIWordWriteDelegate
{
    void OnUIWordWriteDidMode(UIWordWrite ui, UIWordWrite.Mode md);
    void OnUIWordWriteDidWriteFinish(UIWordWrite ui);
}

public class UIWordWrite : UIViewPop
{
    public enum Mode
    {
        ALL_STROKE,
        ONE_STROKE,//显示一个笔画
        NO_STROKE,//没有笔画
        FREE_WRITE,
        DEMO,
    }
    public IUIWordWriteDelegate iDelegate;
    public List<PaintLine2> listLine = new List<PaintLine2>();
    public List<UISprite> listGuide = new List<UISprite>();
    public List<UISprite> listGuideAnimate = new List<UISprite>();
    public Button btnClose;
    public UIWordSVG uiWordSVG;
    UISprite uiSpritePrefab;
    public WordItemInfo infoWord;
    PaintLine2 paintLinePrefab;
    PaintLine2 paintLine;

    GameObject objBihuaLine;
    RenderTexture rtMainBihuaLine;
    int layerLine = 8;


    PaintLine paintLineFreewritePrefab;
    PaintLine paintLineFreewrite;

    UIBgWrite uiBgWritePrefab;
    UIBgWrite uiBgWrite;
    Material matLine;
    float lineWidth = 20f;//屏幕像素
    Color colorLine = Color.red;
    Color colorWord = Color.gray;
    public Mode mode = Mode.ALL_STROKE;
    float scaleWord = 1f;
    float scaleGuide = 1f;
    float widthGuide;//world
    float delayTime = 0.1f;
    float animateTime = 1f;
    int indexAnimate = 0;
    float zGuide = -12f;

    public UIAdBanner uiAdBannerPrefab;
    public UIAdBanner uiAdBanner;


    public bool isHasPaint
    {
        get
        {

            if (mode == Mode.FREE_WRITE)
            {
                if (!paintLineFreewrite)
                {
                    return false;
                }
                return paintLineFreewrite.isHasPaint;
            }
            else
            {

                return true;
                if (!paintLine)
                {
                    return false;
                }
                return paintLine.isHasPaint;
            }

        }
    }

    public bool isHasSave
    {
        get
        {

            if (mode == Mode.FREE_WRITE)
            {
                if (!paintLineFreewrite)
                {
                    return false;
                }
                return paintLineFreewrite.isHasSave;
            }
            else
            {
                if (!paintLine)
                {
                    return false;
                }
                return paintLine.isHasSave;
            }
        }
    }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        LoadPrefab();
        // PopUpManager.main.ShowPannl(false); 
        uiWordSVG.callBackAnimate = OnUIWordSVGAnimate;
        uiWordSVG.colorWord = colorWord;
        matLine = new Material(Shader.Find("Custom/PaintLine"));

        uiBgWrite = (UIBgWrite)GameObject.Instantiate(uiBgWritePrefab);
        AppSceneBase.main.AddObjToMainWorld(uiBgWrite.gameObject);
        UIViewController.ClonePrefabRectTransform(uiBgWritePrefab.gameObject, uiBgWrite.gameObject);
        uiBgWrite.transform.localPosition = new Vector3(0, 0, 0);

        Texture2D tex = TextureCache.main.LoadImageKey("WriteGuideStart");
        scaleGuide = Mathf.Max(Screen.width, Screen.height) * 0.3f / Screen.height;
        // scaleGuide = 0.3f;
        LayOut();

    }
    // Use this for initialization
    void Start()
    {
        // AppSceneBase.main.UpdateWorldBg("GameBg", true);
        // AppSceneBase.main.ShowRootViewController(false);
        AppSceneBase.main.AddObjToMainWorld(uiWordSVG.gameObject);
        uiWordSVG.gameObject.transform.localPosition = new Vector3(0, 0, -11f);

        LayOut();

        Invoke("LayOut", delayTime);

        // GotoWriteFinish();
        InvokeRepeating("OnTimerAnimate", 0f, animateTime);
        bool isShowAd = false;
        if (Device.isLandscape)
        {
            isShowAd = false;
        }

        //显示横幅广告
        if (isShowAd)
        {
            AdKitCommon.main.InitAdBanner();
            AdKitCommon.main.ShowAdBanner(true);
            AdKitCommon.main.AdBannerSetScreenOffsetY(Common.CanvasToScreenHeight(AppSceneBase.main.sizeCanvas, 160f));
        }

        AdKitCommon.main.ShowAdInsertWithStep(UIGameBase.GAME_AD_INSERT_SHOW_STEP, false);
        LoadUIAdBanner();
        if (GameViewController.main.EnableUIAdBanner())
        {
            uiAdBanner = (UIAdBanner)GameObject.Instantiate(uiAdBannerPrefab);
            uiAdBanner.SetViewParent(AppSceneBase.main.canvasMain.gameObject);
            UIViewController.ClonePrefabRectTransform(uiAdBannerPrefab.gameObject, uiAdBanner.gameObject);
            uiAdBanner.gameObject.SetActive(true);
            uiAdBanner.SetBottomOffsetY(160f);
        }

    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        // AppSceneBase.main.ShowRootViewController(true);
        AppSceneBase.main.ClearMainWorld();
        AdKitCommon.main.ShowAdBanner(false);
        DestroyUIAdBanner();
    }

    void LoadUIAdBanner()
    {
        if (!GameViewController.main.EnableUIAdBanner())
        {
            return;
        }
        GameObject obj = PrefabCache.main.Load(UIAdBanner.PREFAB_UIAdBanner);
        if (obj != null)
        {
            uiAdBannerPrefab = obj.GetComponent<UIAdBanner>();
        }
    }

    void DestroyUIAdBanner()
    {
        if (!GameViewController.main.EnableUIAdBanner())
        {
            return;
        }
        if (uiAdBanner != null)
        {
            GameObject.Destroy(uiAdBanner.gameObject);
            uiAdBanner = null;
        }
    }
    void LoadPrefab()
    {
        {
            string path = ConfigPrefab.main.GetPrefab("PaintLine2");
            Debug.Log("PaintLine2 path=" + path);
            GameObject obj = PrefabCache.main.LoadByKey("PaintLine2");
            {
                paintLinePrefab = obj.GetComponent<PaintLine2>();

            }
        }
        {
            GameObject obj = PrefabCache.main.LoadByKey("PaintLine");
            {
                paintLineFreewritePrefab = obj.GetComponent<PaintLine>();

            }
        }



        {
            GameObject obj = PrefabCache.main.LoadByKey("UIBgWrite");
            if (obj != null)
            {
                uiBgWritePrefab = obj.GetComponent<UIBgWrite>();

            }
        }

        {

            GameObject obj = PrefabCache.main.Load(UIKitRes.Prefab_UISprite);
            if (obj != null)
            {
                uiSpritePrefab = obj.GetComponent<UISprite>();
            }

        }
    }

    public override void LayOut()
    {
        base.LayOut();
        float x = 0, y = 0, w = 0, h = 0, z = 0;
        float scale = 0; float scalex = 0; float scaley = 0;
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        Vector2 worldSize = Common.GetWorldSize(mainCam);
        float oft_top = Common.ScreenToWorldHeight(mainCam, Device.offsetTop);
        float oft_bottom = Common.ScreenToWorldHeight(mainCam, Device.offsetBottom);
        float oft_left = Common.ScreenToWorldHeight(mainCam, Device.offsetLeft);
        float oft_right = Common.ScreenToWorldHeight(mainCam, Device.offsetRight);
        float topbar_height = Common.CanvasToWorldHeight(mainCam, sizeCanvas, 160);
        RectTransform rctranWorld = AppSceneBase.main.GetRectMainWorld();
        float offsetTopbarY = 160;
        float heightTopbarWorld = Common.CanvasToWorldHeight(mainCam, sizeCanvas, offsetTopbarY) + Device.offsetTopWorld;
        float heightAdBannerWorld = Device.offsetBottomWithAdBannerWorld;
        float ratio;
        {

            ratio = 0.8f;
            w = (rctranWorld.rect.width) * ratio;
            float oft_y = Mathf.Max(heightTopbarWorld, heightAdBannerWorld);
            h = (rctranWorld.rect.height - oft_y) * ratio;
            x = 0 - w / 2;
            y = 0 - h / 2;
            Rect rc = new Rect(x, y, w, h);
        }


        foreach (PaintLine2 line in listLine)
        {
            line.UpdateRect(new Rect(x, y, w, h));
        }

        if (uiBgWrite != null)
        {
            x = uiBgWrite.uiWriteBoard.transform.position.x;
            y = uiBgWrite.uiWriteBoard.transform.position.y;
            z = uiWordSVG.transform.position.z;
            uiWordSVG.transform.position = new Vector3(x, y, z);

            Vector2 sizeBoard = uiBgWrite.GetBoundBoard();
            float sz_image = StrokeMatche.IMAGE_SIZE / 100f;
            Vector2 sizeWord = new Vector2(sz_image, sz_image);
            ratio = 0.8f;
            if (Device.isLandscape)
            {
                // ratio = 1f;
            }
            scaleWord = Common.GetBestFitScale(sizeWord.x, sizeWord.y, sizeBoard.x, sizeBoard.y);
            scaleWord = scaleWord * ratio;
            Debug.Log(" scaleWord =" + scaleWord + " sizeWord=" + sizeWord + " sizeBoard=" + sizeBoard);

            uiWordSVG.scaleWord = scaleWord;
            StrokeMatche.main.scaleWord = scaleWord;
            StrokeMatche.main.wordPosition = uiWordSVG.transform.position;
            uiWordSVG.LayOut();
        }

        UpdateRectFreeWrite();
    }

    void OnTimerAnimate()
    {
        if (listGuideAnimate.Count == 0)
        {
            return;
        }
        foreach (UISprite uisp in listGuideAnimate)
        {
            uisp.gameObject.SetActive(false);
        }
        for (int i = 0; i <= indexAnimate; i++)
        {
            UISprite ui = listGuideAnimate[i];
            ui.gameObject.SetActive(true);
        }


        indexAnimate++;
        if (indexAnimate >= listGuideAnimate.Count)
        {
            indexAnimate = 0;
        }
    }
    public void GotoMode(Mode md)
    {
        mode = md;
        uiBgWrite.OnMode(md);
        if (iDelegate != null)
        {
            iDelegate.OnUIWordWriteDidMode(this, md);
        }
        Clear();
        uiWordSVG.SetRenderMode(UIViewSVG.RenderMode.WORLD);
        switch (md)
        {
            case Mode.ALL_STROKE:
                {

                    uiWordSVG.type = UIWordSVG.Type.ALL_STROKE;
                    uiWordSVG.LoadWord(infoWord);
                }
                break;
            case Mode.ONE_STROKE:
                {
                    uiWordSVG.type = UIWordSVG.Type.ONE_STROKE;
                    uiWordSVG.indexStroke = 0;
                    uiWordSVG.LoadWordStroke(infoWord, uiWordSVG.indexStroke);
                }
                break;
            case Mode.NO_STROKE:
                {
                    uiWordSVG.type = UIWordSVG.Type.NONE;
                    uiWordSVG.LoadWord(infoWord);

                }
                break;
            case Mode.FREE_WRITE:
                {
                    uiWordSVG.type = UIWordSVG.Type.NONE;
                    uiWordSVG.Clear();
                    CreateLineFreeDraw();
                }
                break;
            case Mode.DEMO:
                {
                    uiWordSVG.type = UIWordSVG.Type.DEMO;
                    uiWordSVG.LoadWord(infoWord);
                }
                break;
        }
        if (mode != Mode.FREE_WRITE)
        {
            CreateBihuaLine();
            PaintLine2 paintLine = CreateLine();
        }

        // 显示笔画中线
        // List<Vector2> listPoint = infoWord.pointInfo.listMedian[0] as List<Vector2>;
        // foreach (Vector2 pt in listPoint)
        // {
        //     Vector2 worldSize = Common.GetWorldSize(mainCam);
        //     Vector2 ptdraw = StrokeMatche.main.StrokePoint2WorldPoint(pt);
        //     paintLine.AddPoint(ptdraw);
        // } 

        LayOut();
    }

    public int GetStrokeCount()
    {
        int count = infoWord.pointInfo.listStroke.Count;
        return count;
    }

    public void HideLine(PaintLine2 line)
    {
        // listLine.Remove(line);
        // GameObject.DestroyImmediate(line.gameObject);
        line.gameObject.SetActive(false);
    }

    public void Clear()
    {
        ClearAllLine();
        uiWordSVG.Clear();
        uiWordSVG.indexStroke = 0;
    }

    public void ClearAllLine()
    {
        foreach (PaintLine2 line in listLine)
        {
            DestroyImmediate(line.gameObject);
        }
        listLine.Clear();
    }

    public void CreateBihuaLine()
    {
        if (objBihuaLine != null)
        {
            return;
        }
        objBihuaLine = new GameObject("BihuaLine");
        AppSceneBase.main.AddObjToMainWorld(objBihuaLine);
        objBihuaLine.transform.localPosition = new Vector3(0f, 0f, -11f);

        GameObject cameraPhoto = new GameObject("CameraPhoto");

        cameraPhoto.transform.SetParent(objBihuaLine.transform);
        cameraPhoto.transform.localPosition = new Vector3(0f, 0f, -10f);

        // cameraPhoto.hideFlags = HideFlags.HideAndDontSave;
        // cameraPhoto.transform.position = b.center - new Vector3(0, 0, b.extents.z + 10);
        // cameraPhoto.transform.LookAt(b.center);

        Camera c = cameraPhoto.AddComponent<Camera>();

        // c.cullingMask = 1 << LayerMask.NameToLayer("PhotoLayer");
        c.clearFlags = CameraClearFlags.Nothing;
        c.orthographic = true;
        c.cullingMask = (1 << layerLine);
        // mainCam.cullingMask &= ~(1 << layerLine); // 关闭层x
        objBihuaLine.layer = layerLine;

        // float size = Mathf.Max(b.extents.x, b.extents.y);

        // print(b.extents.x + "   " + b.extents.y);
        // c.orthographicSize = size;
        int w = (int)(Screen.width);
        int h = (int)(Screen.height);
        rtMainBihuaLine = new RenderTexture(w, h, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
        c.targetTexture = rtMainBihuaLine;
        // camRender.pixelRect = new Rect((Screen.width - w) / 2, (Screen.height - h) / 2, w, h);
        //camRender.rect = new Rect(0, 0.5f, 1, 1);


    }

    public PaintLine2 CreateLine()
    {
        PaintLine2 paintLine = (PaintLine2)GameObject.Instantiate(paintLinePrefab);
        // AppSceneBase.main.AddObjToMainWorld(paintLine.gameObject);
        // paintLine.transform.localPosition = new Vector3(0f, 0f, -1f * (listLine.Count + 1) - 10);
        paintLine.transform.SetParent(objBihuaLine.transform);
        paintLine.transform.localPosition = new Vector3(0f, 0f, -1f * (listLine.Count + 1));

        // paintLine.transform.SetParent(this.transform);
        UIViewController.ClonePrefabRectTransform(paintLinePrefab.gameObject, paintLine.gameObject);
        listLine.Add(paintLine);
        paintLine.Clear();
        paintLine.UpdateColor(colorLine);
        float w = 40f * Screen.height / 2048;
        if (Device.isLandscape)
        {
            w = 30f * Screen.width / 2048;
        }
        paintLine.SetLineWidthPixsel((int)w);
        paintLine.callBackPaint = OnCallBackPaintLine;
        LayOut();

        ShowStrokeGuide();
        ShowStrokeGuideAnimate();

        return paintLine;
    }

    public void CreateLineFreeDraw()
    {
        paintLineFreewrite = (PaintLine)GameObject.Instantiate(paintLineFreewritePrefab);
        AppSceneBase.main.AddObjToMainWorld(paintLineFreewrite.gameObject);
        float x = uiBgWrite.uiWriteBoard.transform.position.x;
        float y = uiBgWrite.uiWriteBoard.transform.position.y;
        paintLineFreewrite.transform.localPosition = new Vector3(x, y, -1f * (listLine.Count + 1) - 10);
        // paintLine.transform.SetParent(this.transform);
        UIViewController.ClonePrefabRectTransform(paintLineFreewritePrefab.gameObject, paintLineFreewrite.gameObject);

        paintLineFreewrite.Clear();
        paintLineFreewrite.UpdateColor(colorLine);

        // paintLineFreewrite.callBackPaint = OnCallBackPaintLine;
        LayOut();



    }

    public void UpdateColor(Color cr)
    {
        colorLine = cr;
        foreach (PaintLine2 line in listLine)
        {
            line.UpdateColor(cr);
        }
        if (paintLineFreewrite != null)
        {
            paintLineFreewrite.UpdateColor(cr);
        }
    }
    public void SetLineWidthPixsel(int w)
    {
        if (paintLineFreewrite != null)
        {
            paintLineFreewrite.SetLineWidthPixsel(w);
        }
    }
    public void UpdateRectFreeWrite()
    {
        Rect rc = Rect.zero;
        rc.x = uiBgWrite.uiWriteBoard.transform.position.x;
        rc.y = uiBgWrite.uiWriteBoard.transform.position.y;
        rc.width = uiBgWrite.uiWriteBoard.GetBoundSize().x;
        rc.height = uiBgWrite.uiWriteBoard.GetBoundSize().y;

        // rectWordWrite = rc;
        if (paintLineFreewrite != null)
        {
            paintLineFreewrite.UpdateRect(rc);
            float x = uiBgWrite.uiWriteBoard.transform.position.x;
            float y = uiBgWrite.uiWriteBoard.transform.position.y;
            paintLineFreewrite.transform.localPosition = new Vector3(x, y, -1f * (listLine.Count + 1) - 10);

        }

    }

    public void SaveImage(string filePath)
    {

        if (Mode.FREE_WRITE == mode)
        {
            if (paintLineFreewrite != null)
            {
                paintLineFreewrite.SaveImage(filePath);
            }
        }
        else
        {
            SaveBihuaLine(filePath);
            if (paintLine != null)
            {

                // paintLine.SaveImage(filePath);
            }
        }
    }


    public void Shoot(Rect rc, string filePath)
    {
        StartCoroutine(GetScreenTexture(rc, filePath));
    }
    /// <summary>
    /// 截指定区域图并赋值给材质
    /// </summary>
    /// <returns></returns>
    IEnumerator GetScreenTexture(Rect rc, string filePath)
    {
        float x = rc.x;
        float y = rc.y;
        float width = rc.width;
        float height = rc.height;

        yield return new WaitForEndOfFrame();
        Texture2D t = new Texture2D((int)width, (int)height, TextureFormat.RGB24, false);//需要正确设置好图片保存格式
        t.ReadPixels(new Rect(x, y, width, height), 0, 0, false);//按照设定区域读取像素；注意是以左下角为原点读取
        // t.Compress(false);
        t.Apply();
        //二进制转换，保存到手机
        // byte[] byt = t.EncodeToPNG();
        //   File.WriteAllBytes(Application.dataPath + this.index + ".png", byt);

        TextureUtil.SaveTextureToFile(t, filePath);

    }

    public void SaveBihuaLine(string filePath)
    {
        float x = 0, y = 0, w = 0, h = 0;
        Rect rectMain = Rect.zero;
        w = Common.WorldToScreenWidth(mainCam, rectMain.size.x);
        h = Common.WorldToScreenHeight(mainCam, rectMain.size.y);
        if (rectMain == Rect.zero)
        {
            w = rtMainBihuaLine.width;
            h = rtMainBihuaLine.height;
        }
        x = (rtMainBihuaLine.width - w) / 2;
        y = (rtMainBihuaLine.height - h) / 2;
        Rect rc = new Rect(x, y, w, h);
        Debug.Log("SaveImage rc=" + rc);
        Texture2D tex = TextureUtil.RenderTexture2Texture2D(rtMainBihuaLine, rc);

        Rect rcsave = TextureUtil.GetRectNotAlpha(tex);

        // // rcsave.x = 0;
        // // rcsave.y = h-rcsave.y;
        Debug.Log("SaveImage rcsave=" + rcsave);
        Texture2D texsave = TextureUtil.GetSubRenderTexture(rtMainBihuaLine, rcsave, true);
        // texsave = tex;
        TextureUtil.SaveTextureToFile(texsave, filePath);

        // filePath += "_test.png";
        // Shoot(rcsave, filePath);

    }

    public void ClearAll()
    {
        if (paintLineFreewrite != null)
        {
            paintLineFreewrite.ClearAll();
        }
    }
    void ShowStrokeGuide()
    {
        foreach (UISprite uisp in listGuide)
        {
            DestroyImmediate(uisp.gameObject);
        }

        listGuide.Clear();
        List<Vector2> listPoint = infoWord.pointInfo.listMedian[uiWordSVG.indexStroke] as List<Vector2>;
        // List<Vector2> listPoint = infoWord.pointInfo.listMedian[0] as List<Vector2>;

        for (int i = 0; i < listPoint.Count; i++)
        {
            Vector2 pt = listPoint[i];
            Vector2 worldSize = Common.GetWorldSize(mainCam);
            UISprite ui = (UISprite)GameObject.Instantiate(uiSpritePrefab);
            // ui.transform.SetParent(this.transform);
            AppSceneBase.main.AddObjToMainWorld(ui.gameObject);

            float scale = scaleGuide;
            ui.transform.localScale = new Vector3(scale, scale, 1f);
            Vector2 ptdraw = StrokeMatche.main.StrokePoint2WorldPoint(pt);
            float z = zGuide;

            // paintLine.AddPoint(ptdraw);
            string key = "WriteGuideMiddle";
            if (i == 0)
            {
                key = "WriteGuideStart";
            }
            if (i == listPoint.Count - 1)
            {
                key = "WriteGuideEnd";
                z = z - 2;
            }

            ptdraw.x += uiBgWrite.uiWriteBoard.transform.position.x;
            ptdraw.y += uiBgWrite.uiWriteBoard.transform.position.y;

            ui.transform.localPosition = new Vector3(ptdraw.x, ptdraw.y, z);
            ui.name = key;
            ui.UpdateImageByKey(key);
            float angle = 0f;
            if (i < listPoint.Count - 1)
            {
                Vector2 ptNext = listPoint[i + 1];
                Debug.Log("ptNext = " + ptNext + " pt=" + pt);
                angle = MathUtil.GetAngleOfTwoPoint(pt, ptNext);
            }

            ui.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));

            widthGuide = ui.GetBoundSize().x;
            listGuide.Add(ui);
        }
    }

    Vector2 GetMidPoint(Vector2 ptStart, Vector2 ptEnd, int count, int index)
    {
        Vector2 pt = Vector2.zero;
        pt.x = ptStart.x + (ptEnd.x - ptStart.x) * index / count;
        pt.y = ptStart.y + (ptEnd.y - ptStart.y) * index / count;
        return pt;
    }

    void MakeStrokeGuideAnimate(Vector2 ptStart, Vector2 ptEnd)
    {
        Vector2 pt0 = StrokeMatche.main.StrokePoint2WorldPoint(ptStart);
        Vector2 pt1 = StrokeMatche.main.StrokePoint2WorldPoint(ptEnd);
        if (Mathf.Abs(pt1.x - pt0.x) < widthGuide * 2)
        {
            return;
        }

        int count = (int)(Mathf.Abs(pt1.x - pt0.x) * 1 / widthGuide);
        Debug.Log("widthGuide=" + widthGuide + " count=" + count);
        for (int i = 0; i < count; i++)
        {
            Vector2 pt = GetMidPoint(ptStart, ptEnd, count, i);
            Vector2 worldSize = Common.GetWorldSize(mainCam);
            UISprite ui = (UISprite)GameObject.Instantiate(uiSpritePrefab);
            // ui.transform.SetParent(this.transform);
            AppSceneBase.main.AddObjToMainWorld(ui.gameObject);

            float scale = scaleGuide;
            ui.transform.localScale = new Vector3(scale, scale, 1f);
            Vector2 ptdraw = StrokeMatche.main.StrokePoint2WorldPoint(pt);
            ptdraw.x += uiBgWrite.uiWriteBoard.transform.position.x;
            ptdraw.y += uiBgWrite.uiWriteBoard.transform.position.y;
            ui.transform.localPosition = new Vector3(ptdraw.x, ptdraw.y, zGuide);
            // paintLine.AddPoint(ptdraw);
            string key = "WriteGuideAnimate";
            ui.name = key;
            ui.UpdateImageByKey(key);
            float angle = 0f;
            if (i < count - 1)
            {
                Vector2 ptNext = GetMidPoint(ptStart, ptEnd, count, i + 1);
                angle = MathUtil.GetAngleOfTwoPoint(pt, ptNext);
            }

            ui.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            listGuideAnimate.Add(ui);
        }
    }
    void ShowStrokeGuideAnimate()
    {
        foreach (UISprite uisp in listGuideAnimate)
        {
            DestroyImmediate(uisp.gameObject);
        }

        listGuideAnimate.Clear();
        List<Vector2> listPoint = infoWord.pointInfo.listMedian[uiWordSVG.indexStroke] as List<Vector2>;

        for (int i = 0; i < listPoint.Count; i++)
        {
            Vector2 pt = listPoint[i];
            if (i < listPoint.Count - 1)
            {
                Vector2 ptNext = listPoint[i + 1];
                MakeStrokeGuideAnimate(pt, ptNext);
            }

        }
    }
    public PaintLine2 GetCurLine()
    {
        return listLine[listLine.Count - 1];
    }

    public void UpdateItem(WordItemInfo info)
    {
        GameLevelParse.main.ParseItem(info);
        infoWord = info;

        List<Vector2> listPoint = info.pointInfo.listMedian[0] as List<Vector2>;
        // 
        // PaintLine2 paintLine = CreateLine();
        // paintLine.UpdateColor(Color.yellow);
        foreach (Vector2 pt in listPoint)
        {
            Vector2 worldSize = Common.GetWorldSize(mainCam);
            Vector2 ptdraw = StrokeMatche.main.StrokePoint2WorldPoint(pt);
            // paintLine.AddPoint(ptdraw);
        }
        LayOut();
        // Invoke("GotoModeDelay", delayTime * 1.1f);

    }

    void GotoWriteFinish()
    {

        if (infoWord == null)
        {
            return;
        }
        string filePath = GameLevelParse.main.GetSavePath(infoWord);
        infoWord.dbInfo.filesave = filePath;
        SaveImage(filePath);
        DBHistory.main.AddItem(infoWord.dbInfo);


        if (iDelegate != null)
        {
            iDelegate.OnUIWordWriteDidWriteFinish(this);
        }


    }

    public void OnCallBackPaintLine(PaintLine2 ui, int status)
    {
        if (status == UITouchEvent.STATUS_TOUCH_UP)
        {


            if (uiWordSVG.indexStroke >= GetStrokeCount())
            {
                return;
            }

            PaintLine2 line = GetCurLine();
            line.gameObject.SetActive(true);
            StrokeMatche.main.UpdateDrawPoint(line.listPointCur);
            List<Vector2> listPoint = infoWord.pointInfo.listMedian[uiWordSVG.indexStroke] as List<Vector2>;
            StrokeMatche.main.listPointStroke = listPoint;
            if (StrokeMatche.main.IsMatches(1f))
            {
                line.enableTouch = false;
                uiWordSVG.indexStroke++;
                if (uiWordSVG.indexStroke < GetStrokeCount())
                {
                    if (mode == Mode.ONE_STROKE)
                    {
                        uiWordSVG.LoadWordStroke(infoWord, uiWordSVG.indexStroke);
                    }
                    CreateLine();
                }
                else
                {
                    //goto next mode
                    if (mode == Mode.ALL_STROKE)
                    {
                        GotoMode(Mode.ONE_STROKE);
                    }
                    else if (mode == Mode.ONE_STROKE)
                    {
                        GotoMode(Mode.NO_STROKE);
                    }
                    else if (mode == Mode.NO_STROKE)
                    {
                        GotoWriteFinish();
                    }
                }

            }
            else
            {
                //书写失败  显示动画 
                line.ClearAnimate();
            }


        }
    }
    public void OnClickBtnClose()
    {
        this.Close();

    }

    public void OnUIWordSVGAnimate(UIWordSVG ui, int index)
    {

    }
}
