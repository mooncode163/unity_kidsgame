using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LitJson;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class AbcWordPointJsonItemInfo
{
    public string ratiox;
    public string ratioy;
}
public class UIEditorWordABCController : UIView
{
    List<UIImage> listDotImage = new List<UIImage>();
    List<string> listImage = new List<string>();
    List<AbcWordPointJsonItemInfo> listPoint = new List<AbcWordPointJsonItemInfo>();
    public UIImage imageWord;
    bool isStarted = false;
    string idWord;
    int indexWord;
    /// <summary>
    /// Unity's Awake method.
    /// </summary>
    public void Awake()
    {
        base.Awake();
        LoadPrefab();
        UITouchEvent ev = this.gameObject.AddComponent<UITouchEvent>();
        ev.callBackTouch = OnUITouchEvent;

        string dirlower = CloudRes.main.rootPathGameRes + "/image/lower";

        string dirupper = CloudRes.main.rootPathGameRes + "/image/upper";
        ScanImageFiles(dirlower, listImage);
        ScanImageFiles(dirupper, listImage);

        indexWord = 0;
        UpdateWordImage();
    }

    /// <summary>
    /// Unity's Start method.
    /// </summary>
    public void Start()
    {
        base.Start();
        LayOut();
        OnUIDidFinish();
    }
    void LoadPrefab()
    {

    }
    static public void ScanImageFiles(string dir, List<string> list)
    {
        Debug.Log("ScanImageFiles dir=" + dir);

        DirectoryInfo TheFolder = new DirectoryInfo(dir); ;
        // //遍历文件
        foreach (FileInfo NextFile in TheFolder.GetFiles())
        {
            string fullpath = NextFile.ToString();
            string ext = FileUtil.GetFileExt(fullpath);
            if ((ext == "png") || (ext == "jpg"))
            {
                Debug.Log("ScanImageFiles fullpath=" + fullpath);
                list.Add(fullpath);
            }

        }


    }

    void UpdateWordImage()
    {
        string filepath = listImage[indexWord];
        idWord = FileUtil.GetFileName(filepath);
        imageWord.UpdateImage(filepath);
    }
    UIImage CreateDotImage()
    {
        GameObject obj = PrefabCache.main.LoadByKey("UIImage");
        if (obj != null)
        {
            UIImage uiPrefab = obj.GetComponent<UIImage>();
            UIImage ui = (UIImage)GameObject.Instantiate(uiPrefab);
            ui.transform.SetParent(this.transform);
            ui.transform.localScale = new Vector3(1f, 1f, 1f);
            RectTransform rctran = ui.GetComponent<RectTransform>();
            rctran.anchoredPosition = Vector2.zero;
            ui.UpdateImage(ImageRes.main.GetImage("EditorWordDot"));
            return ui;
        }
        return null;
    }

    public void OnUITouchEvent(UITouchEvent ev, PointerEventData eventData, int status)
    {
        Debug.Log("OnUITouchEvent status=" + status);

        switch (status)
        {
            case UITouchEvent.STATUS_TOUCH_DOWN:
                {
                    break;
                }
            case UITouchEvent.STATUS_TOUCH_UP:
                {
                    break;
                }
            case UITouchEvent.STATUS_Click:
                {
                    if (isStarted)
                    {
                        UIImage ui = CreateDotImage();
                        ui.transform.position = eventData.position;
                        listDotImage.Add(ui);
                        Vector2 pt = eventData.position;
                        RectTransform rctran = imageWord.GetComponent<RectTransform>();
                        float w = rctran.rect.width * imageWord.transform.localScale.x;
                        float h = rctran.rect.height * imageWord.transform.localScale.y;
                        float w_screen = Common.CanvasToScreenWidth(AppSceneBase.main.sizeCanvas, w);
                        float h_screen = Common.CanvasToScreenHeight(AppSceneBase.main.sizeCanvas, h);

                        float ratiox = (pt.x - (imageWord.transform.position.x - w_screen / 2)) / w_screen;
                        float ratioy = (pt.y - (imageWord.transform.position.y - h_screen / 2)) / h_screen;
                        pt.x = ratiox;
                        pt.y = ratioy;
                        AbcWordPointJsonItemInfo info = new AbcWordPointJsonItemInfo();

                        info.ratiox = ratiox.ToString("0.000");
                        info.ratioy = ratioy.ToString("0.000");
                        listPoint.Add(info);
                    }

                    break;
                }

        }

    }


    public override void LayOut()
    {
        base.LayOut();
    }
    public void OnClickBtnStart()
    {
        isStarted = !isStarted;
    }
    public void OnClickBtnClear()
    {
        listPoint.Clear();

        foreach (UIImage ui in listDotImage)
        {
            GameObject.DestroyImmediate(ui.gameObject);
        }
        listDotImage.Clear();
    }
    public void OnClickBtnSave()
    {

        //save guanka json
        {
            Hashtable data = new Hashtable();
            data["items"] = listPoint;
            string strJson = JsonMapper.ToJson(data);
            string filepath = CloudRes.main.rootPathGameRes + "/guanka/" + idWord + ".json";
            byte[] bytes = Encoding.UTF8.GetBytes(strJson);
            System.IO.File.WriteAllBytes(filepath, bytes);
        }
    }

    public void OnClickBtnNext()
    {
        OnClickBtnClear();
        indexWord++;
        if (indexWord >= listImage.Count)
        {
            indexWord = 0;
        }
        UpdateWordImage();
    }
    public void OnClickBtnPre()
    {
         OnClickBtnClear();
        indexWord--;
        if (indexWord < 0)
        {
            indexWord = listImage.Count - 1;
        }
        UpdateWordImage();
    }
}
