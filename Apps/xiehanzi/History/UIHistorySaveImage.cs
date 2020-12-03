using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public delegate void OnUIHistorySaveImageDidDeleteDelegate(UIHistorySaveImage ui, int btnIndex);
public class UIHistorySaveImage : UIView
{
    public const int BTN_INDEX_DELETE = 0;
    public const int BTN_INDEX_RETRY = 1;
    public Image imageBg;
    public Image imageBoard;
    public Image imagePic;
    public Button btnClose;
    public Button btnDel;
    public Button btnRetry;
    public GameObject objLayoutBtn; 
    public WordItemInfo itemInfo;
    public OnUIHistorySaveImageDidDeleteDelegate callBackDelete { get; set; }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {

        ShowBtnRetry(false);
    }

    // Use this for initialization
    void Start()
    {
        InitUI();
    }

    // Update is called once per frame
    void Update()
    {
    }
    void InitUI()
    {

        LayoutChild();
    }
    void LayoutChild()
    {
        float x = 0, y = 0, w = 0, h = 0;
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;

        {
            RectTransform rctran = imageBoard.GetComponent<RectTransform>();
            float ratio = 0.7f;
            w = sizeCanvas.x * ratio;
            h = sizeCanvas.y * ratio;
            rctran.sizeDelta = new Vector2(w, h);

        }
        {
            RectTransform rctranBoard = imageBoard.GetComponent<RectTransform>();
            RectTransform rctran = btnClose.GetComponent<RectTransform>();
            w = rctran.rect.width;
            h = rctran.rect.height;
            x = rctranBoard.rect.width / 2 + w / 2;
            y = rctranBoard.rect.height / 2 + h / 2;
            rctran.anchoredPosition = new Vector2(x, y);

        }

        {
            RectTransform rctranBoard = imageBoard.GetComponent<RectTransform>();
            RectTransform rctran = objLayoutBtn.GetComponent<RectTransform>();
            w = rctran.rect.width;
            h = rctran.rect.height;
            x = 0;
            y = -rctranBoard.rect.height / 2 - h / 2;
            rctran.anchoredPosition = new Vector2(x, y);

        }
        {
            int width = imagePic.sprite.texture.width;
            int height = imagePic.sprite.texture.height;
            RectTransform rctan = imagePic.GetComponent<RectTransform>();
            rctan.sizeDelta = new Vector2(width, height);
            RectTransform rctranBoard = imageBoard.GetComponent<RectTransform>();
            float scalex = rctranBoard.rect.width / width;
            float scaley = rctranBoard.rect.height / height;
            float scale = Mathf.Min(scalex, scaley);
            scale = scale * 0.9f;
            imagePic.transform.localScale = new Vector3(scale, scale, 1f);
        }
    }
    public void ShowBtnRetry(bool isShow)
    {
        if (btnRetry)
        {
            btnRetry.gameObject.SetActive(isShow);
        }

    }
    public void UpdateItem(WordItemInfo info)
    {
        itemInfo = info;
        if (FileUtil.FileIsExist(info.dbInfo.filesave))
        {
            Texture2D tex = LoadTexture.LoadFromFile(info.dbInfo.filesave);
            imagePic.sprite = TextureUtil.CreateSpriteFromTex(tex);

        }
        LayoutChild();
    }
 
    public void OnClickBtnClose()
    {
        PopViewController pop = (PopViewController)this.controller;
        if (pop != null)
        {
            pop.Close();
        }
    }
    public void OnClickBtnDel()
    {
        if (callBackDelete != null)
        {
            callBackDelete(this, BTN_INDEX_DELETE);
        }
        OnClickBtnClose();
    }
    public void OnClickBtnRetry()
    {
        if (callBackDelete != null)
        {
            callBackDelete(this, BTN_INDEX_RETRY);
        }
        OnClickBtnClose();
    }
}
