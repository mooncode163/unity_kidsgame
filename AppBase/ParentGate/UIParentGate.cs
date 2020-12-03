using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tacticsoft;

public delegate void OnUIParentGateDidCloseDelegate(UIParentGate ui, bool isLongPress);
public class UIParentGate : UIView
{
    public const string IMAGE_DIR = "Common/UI/ParentGate";
    public const string IMAGE_LOGO_BG = IMAGE_DIR + "/LogoBg";
    public const string IMAGE_LOGO = IMAGE_DIR + "/Logo";

    public const string IMAGE_NUM_BOARD_BG = IMAGE_DIR + "/NumBoardBg";
    public const string IMAGE_NUM_BG = IMAGE_DIR + "/NumBg";
    public const string IMAGE_NUM_RESULT_BG = IMAGE_DIR + "/ResultBg";
    public const string IMAGE_STATUS_OK = IMAGE_DIR + "/StatusOk";
    public const string IMAGE_STATUS_FAIL = IMAGE_DIR + "/StatusFail";
    public GameObject objContent;
    public GameObject objLogo;
    public GameObject objNum;
    public GameObject objMathResult;
    public GameObject objNumBoard;

    public Image imageLogoBg;
    public Image imageLogo;
    public Image imageResult0;
    public Image imageResult1;
    public Image imageStatus;

    public Image imageNumBoardBg;
    public Image imageNum0;
    public Image imageNum1;
    public Image imageNum2;
    public Image imageNum3;
    public Image imageNum4;
    public Image imageNum5;
    public Image imageNum6;
    public Image imageNum7;
    public Image imageNum8;
    public Image imageNum9;
    public Text textTitle;

    public Text textMath;

    int indexSelectWord;

    int randomNum0;//0-9 
    int randomNum1;//0-9 

    int indexInputNum;
    int totalInputNum;

    Text textResult0;
    Text textResult1;
    int resultValue;
    public OnUIParentGateDidCloseDelegate callbackClose { get; set; }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        indexInputNum = 0;
        totalInputNum = 2;
        randomNum0 = Random.Range(3, 10);
        randomNum1 = Random.Range(4, 10);
        resultValue = randomNum0 * randomNum1;
        imageStatus.gameObject.SetActive(false);
        textMath.text = randomNum0.ToString() + "X" + randomNum1.ToString() + "=";

        textResult0 = GetImageText(imageResult0);
        textResult1 = GetImageText(imageResult1);

        TextureUtil.UpdateImageTexture(imageLogoBg, IMAGE_LOGO_BG, false);
        TextureUtil.UpdateImageTexture(imageLogo, IMAGE_LOGO, true);

        TextureUtil.UpdateImageTexture(imageNumBoardBg, IMAGE_NUM_BOARD_BG, false);
        TextureUtil.UpdateImageTexture(imageResult0, IMAGE_NUM_RESULT_BG, false);
        TextureUtil.UpdateImageTexture(imageResult1, IMAGE_NUM_RESULT_BG, false);

        TextureUtil.UpdateImageTexture(imageStatus, IMAGE_STATUS_OK, false);

        TextureUtil.UpdateImageTexture(imageNum0, IMAGE_NUM_BG, false);
        TextureUtil.UpdateImageTexture(imageNum1, IMAGE_NUM_BG, false);
        TextureUtil.UpdateImageTexture(imageNum2, IMAGE_NUM_BG, false);
        TextureUtil.UpdateImageTexture(imageNum3, IMAGE_NUM_BG, false);
        TextureUtil.UpdateImageTexture(imageNum4, IMAGE_NUM_BG, false);
        TextureUtil.UpdateImageTexture(imageNum5, IMAGE_NUM_BG, false);
        TextureUtil.UpdateImageTexture(imageNum6, IMAGE_NUM_BG, false);
        TextureUtil.UpdateImageTexture(imageNum7, IMAGE_NUM_BG, false);
        TextureUtil.UpdateImageTexture(imageNum8, IMAGE_NUM_BG, false);
        TextureUtil.UpdateImageTexture(imageNum9, IMAGE_NUM_BG, false);

        Button[] btnList = objNumBoard.GetComponentsInChildren<Button>();
        foreach (Button btn in btnList)
        {
           // btn.onClick.AddListener(() => OnClickImageNum(btn));
        }



    }
    // Use this for initialization
    void Start()
    {
        textTitle.text = Language.main.GetString("STR_PARENTGATE");
        LayOut();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnClickBtnBack();
        }
    }

    public override void LayOut()
    {
        float x = 0, y = 0, w = 0, h = 0, w_content = 0, h_content = 0;
        float ratio = 1f;
        RectTransform rctranContent = objContent.GetComponent<RectTransform>();
        GridLayoutGroup gridLayout = objNumBoard.GetComponent<GridLayoutGroup>();


        {
            ratio = 0.8f;
            w_content = this.frame.width * ratio;
            h_content = this.frame.height * ratio;
            rctranContent.sizeDelta = new Vector2(w_content, h_content);
        }

        if (Device.isLandscape)
        {
            //logo
            {
                ratio = 1f / 3;
                RectTransform rctran = objLogo.GetComponent<RectTransform>();
                w = w_content * ratio;
                h = h_content;
                rctran.sizeDelta = new Vector2(w, h);
                x = -w_content / 2 + w / 2;
                y = 0;
                rctran.anchoredPosition = new Vector2(x, y);
            }
            //num
            {
                ratio = 1 - ratio;
                RectTransform rctran = objNum.GetComponent<RectTransform>();
                w = w_content * ratio;
                h = h_content;
                rctran.sizeDelta = new Vector2(w, h);
                x = w_content / 2 - w / 2;
                y = 0;
                rctran.anchoredPosition = new Vector2(x, y);
            }


        }
        else
        {
            //logo
            {
                ratio = 1f / 2;
                RectTransform rctran = objLogo.GetComponent<RectTransform>();
                w = w_content;
                h = h_content * ratio;
                rctran.sizeDelta = new Vector2(w, h);
                y = h_content / 2 - h / 2;
                x = 0;
                rctran.anchoredPosition = new Vector2(x, y);
            }
            //num
            {
                ratio = 1 - ratio;
                RectTransform rctran = objNum.GetComponent<RectTransform>();
                w = w_content;
                h = h_content * ratio;
                rctran.sizeDelta = new Vector2(w, h);
                y = -h_content / 2 + h / 2;
                x = 0;
                rctran.anchoredPosition = new Vector2(x, y);
            }
        }

        //numboard
        {
            RectTransform rctran = objNumBoard.GetComponent<RectTransform>();
            w = rctran.rect.width;
            h = rctran.rect.height;
            float w_cell = Mathf.Min(w / 5, h / 2);
            w_cell = Mathf.Min(w_cell, 160);
            float h_cell = w_cell;
            gridLayout.cellSize = new Vector2(w_cell, h_cell);
        }

        {
            RectTransform rctran = objLogo.GetComponent<RectTransform>();
            ratio = 0.7f;
            float scale = Common.GetBestFitScale(imageLogo.sprite.texture.width, imageLogo.sprite.texture.height, rctran.rect.width * ratio, rctran.rect.height * ratio);
            imageLogo.transform.localScale = new Vector3(scale, scale, 1f);
        }
    }
    Text GetImageText(Image btn)
    {
        Transform tr = btn.transform.Find("Text");
        Text btnText = tr.GetComponent<Text>();
        return btnText;
    }
    public void OnClickBtnBack()
    { 
        PopViewController pop = (PopViewController)this.controller;
        if (pop != null)
        {
            pop.Close();
        }
    }


    public void OnClickImageNum(Button btn)
    {
        //AudioPlay.main.PlayBtnSound();
        Image image = btn.gameObject.GetComponent<Image>();
        Text textNum = GetImageText(image);
        switch (indexInputNum)
        {
            case 0:
                textResult0.text = textNum.text;
                break;
            case 1:
                textResult1.text = textNum.text;
                break;
        }



        indexInputNum++;
        if (indexInputNum >= totalInputNum)
        {
            indexInputNum = 0;
            //check result
            int value = Common.String2Int(textResult0.text) * 10 + Common.String2Int(textResult1.text);
            imageStatus.gameObject.SetActive(true);
            if (resultValue == value)
            {
                //正确，解锁
                TextureUtil.UpdateImageTexture(imageStatus, IMAGE_STATUS_OK, false);
                Invoke("OnResultOk", 0.5f);
            }
            else
            {
                //错误
                TextureUtil.UpdateImageTexture(imageStatus, IMAGE_STATUS_FAIL, false);
                Invoke("OnResultFail", 1f);
            }

        }
    }

    void OnResultOk()
    {
        if (this.callbackClose != null)
        {
            this.callbackClose(this, true);
        }
        OnClickBtnBack();
    }

    void OnResultFail()
    {
        indexInputNum = 0;
        imageStatus.gameObject.SetActive(false);
        textResult0.text = "";
        textResult1.text = "";
    }

}
