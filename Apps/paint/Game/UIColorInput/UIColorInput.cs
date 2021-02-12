using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public delegate void OnUIColorInputUpdateColorDelegate(Color color);
public class UIColorInput : UIView
{
    public Image imageColorBg;
    public Image imageColorG_B;//G B
    public Image imageColorRed;
    public Image imageColorNew;
    public Image imageColorNow;
    public Image imageColorSelectDot;
    public Text textColorNew;
    public Text textColorNow;
    public Text textR;
    public Text textB;
    public Text textG;
    public InputField inputFieldR;
    public InputField inputFieldG;
    public InputField inputFieldB;
    public Text textTitle;
    public GameObject objLayoutColor;
    public GameObject objLayoutRGB;
    public GameObject objLayoutBtn;
    public GameObject objLayoutItem;

    public Button btnYes;
    public Button btnNo;

    public Color ColorNew = Color.white;
    public Color ColorNow = Color.white;
    public OnUIColorInputUpdateColorDelegate callBackUpdateColor { get; set; }
    Material matColorInput0;
    Material matColorInput1;

    int colorR;//0-255
    int colorG;//0-255
    int colorB;//0-255
    int barHeight = 160;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // colorR = 255;
        // colorG = 255;
        // colorB = 255;
        matColorInput0 = new Material(Shader.Find("Custom/ColorInput"));
        matColorInput0.SetInt("_Type", 0);
        matColorInput0.SetFloat("_ColorR", colorR / 255f);
        imageColorG_B.material = matColorInput0;

        matColorInput1 = new Material(Shader.Find("Custom/ColorInput"));
        matColorInput1.SetInt("_Type", 1);
        matColorInput1.SetFloat("_ColorG", colorG / 255f);
        matColorInput1.SetFloat("_ColorB", colorB / 255f);
        imageColorRed.material = matColorInput1;

        inputFieldR.text = colorR.ToString();
        inputFieldG.text = colorG.ToString();
        inputFieldB.text = colorB.ToString();

        {
            EventColorInputImage eventImage = imageColorG_B.gameObject.AddComponent<EventColorInputImage>();
            eventImage.type = EventColorInputImage.TYPE_GREEN_AND_BLUE;
            eventImage.imageDot = imageColorSelectDot;
            eventImage.callBackClick = OnEventColorInputImageClick;
        }
        {
            EventColorInputImage eventImage = imageColorRed.gameObject.AddComponent<EventColorInputImage>();
            eventImage.type = EventColorInputImage.TYPE_RED;
            eventImage.imageDot = imageColorSelectDot;
            eventImage.callBackClick = OnEventColorInputImageClick;
        }

        UpdateColorNew();
        UpdateColorNow();

    }

    // Use this for initialization
    void Start()
    {
        textColorNew.text = Language.main.GetString("STR_COLOR_INPUT_NEW");
        textColorNow.text = Language.main.GetString("STR_COLOR_INPUT_NOW");
        textTitle.text = Language.main.GetString("STR_TITLE_COLOR_INPUT");
        {
            string strYes = Language.main.GetString(AppString.STR_UIVIEWALERT_YES);
            string strNo = Language.main.GetString(AppString.STR_UIVIEWALERT_NO);
            Transform tr = btnYes.transform.Find("Text");
            Text btnText = tr.GetComponent<Text>();
            float strWYes = Common.GetButtonTextWidth(btnYes, strYes);
            float strWNo = Common.GetButtonTextWidth(btnNo, strNo);
            float strW = Mathf.Max(strWYes, strWNo) + btnText.fontSize;
            Common.SetButtonTextWidth(btnYes, strYes, strW);
            Common.SetButtonTextWidth(btnNo, strNo, strW);
        }

        LayoutChild();
        UpdateColor();
        UpdateImageDot();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LayoutChild()
    {
        float x = 0, y = 0, w = 0, h = 0;
        RectTransform rctranRoot = GetComponent<RectTransform>();
        float w_root = rctranRoot.rect.width;
        float h_root = rctranRoot.rect.height;
        Debug.Log("UIColorInput :w_root=" + w_root + " h_root=" + h_root);
        {
            if (Device.isLandscape)
            {
                w = w_root / 2;
                h = h_root - barHeight ;
                x = -w_root / 4;
                y = -barHeight/2;
            }
            else
            {
                w = w_root;
                h = h_root / 2 -barHeight;
                x = 0;
                y = h / 2;
            }

            w = h = Mathf.Min(w, h) * 0.8f;
            RectTransform rctran = imageColorG_B.GetComponent<RectTransform>();

            rctran.sizeDelta = new Vector2(w, h);
            rctran.anchoredPosition = new Vector2(x, y);
        }

        {
            RectTransform rctranGB = imageColorG_B.GetComponent<RectTransform>();
            RectTransform rctran = imageColorRed.GetComponent<RectTransform>();
            x = rctranGB.anchoredPosition.x + rctranGB.rect.width / 2 + rctran.rect.width / 2 + 32;
            y = rctranGB.anchoredPosition.y;
            w = rctran.sizeDelta.x;
            rctran.sizeDelta = new Vector2(w, h);
            rctran.anchoredPosition = new Vector2(x, y);
        }

        {

            RectTransform rctran = objLayoutItem.GetComponent<RectTransform>();

            if (Device.isLandscape)
            {
                x = w_root / 4;
                y = 0;
            }
            else
            {
                x = 0;
                y = -h_root / 4;
            }
            rctran.anchoredPosition = new Vector2(x, y);
        }

       
    }
    public void UpdateInitColor(Color color)
    {
        colorR = (int)(color.r * 255);
        colorG = (int)(color.g * 255);
        colorB = (int)(color.b * 255);
        UpdateColor();
        UpdateImageDot();
    }
    void UpdateColor()
    {
        if (matColorInput0 == null)
        {
            return;
        }

        matColorInput0.SetFloat("_ColorR", colorR / 255f);
        matColorInput1.SetFloat("_ColorG", colorG / 255f);
        matColorInput1.SetFloat("_ColorB", colorB / 255f);

        inputFieldR.text = colorR.ToString();
        inputFieldG.text = colorG.ToString();
        inputFieldB.text = colorB.ToString();
        UpdateColorNew();
        UpdateColorNow();
        UpdateImageDot();
    }
    void UpdateImageDot()
    {
        RectTransform rcTran = imageColorG_B.GetComponent<RectTransform>();
        RectTransform rcTranDot = imageColorSelectDot.GetComponent<RectTransform>();
        Vector2 posDot = rcTranDot.anchoredPosition;
        float x = rcTran.anchoredPosition.x - rcTran.rect.size.x / 2;
        float y = rcTran.anchoredPosition.y - rcTran.rect.size.y / 2;

        float x_ratio = 0, y_ratio = 0, x_delta = 0, y_delta = 0;
        x_ratio = colorG / 255f;
        y_ratio = colorB / 255f;
        x_delta = rcTran.rect.size.x * x_ratio;
        y_delta = rcTran.rect.size.y * y_ratio;
        posDot.x = x_delta + x;
        posDot.y = y_delta + y;
        rcTranDot.anchoredPosition = posDot;

        Debug.Log("UpdateImageDot x_ratio=" + x_ratio + " y_ratio=" + y_ratio);
    }
    void UpdateColorNew()
    {
        imageColorNew.color = ColorNew;

    }
    public void UpdateColorNow()
    {
        imageColorNow.color = ColorNow;
    }
    string ColorVaule2String(float v)
    {
        int color = (int)(v * 255);
        return color.ToString();
    }
    int String2ColorVaule(string str)
    {
        int v = Common.String2Int(str);
        if (v < 0)
        {
            v = 0;
        }
        if (v > 255)
        {
            v = 255;
        }
        return v;
    }

    void LimitInputText(InputField inputField)
    {
        string str = inputField.text;
        int v = Common.String2Int(str);
        if (v < 0)
        {
            inputField.text = "0";
        }
        if (v > 255)
        {
            inputField.text = "255";
        }
    }

    public void OnEventColorInputImageClick(int type, float x_ratio, float y_ratio)
    {
        if (type == EventColorInputImage.TYPE_GREEN_AND_BLUE)
        {
            matColorInput1.SetFloat("_ColorG", x_ratio);
            matColorInput1.SetFloat("_ColorB", y_ratio);
            float color_g = x_ratio * 255;
            float color_b = y_ratio * 255;
            ColorNew.g = x_ratio;
            ColorNew.b = y_ratio;
            inputFieldG.text = ColorVaule2String(x_ratio);
            LimitInputText(inputFieldG);
            inputFieldB.text = ColorVaule2String(y_ratio);
            LimitInputText(inputFieldB);
            UpdateColorNew();
        }
        if (type == EventColorInputImage.TYPE_RED)
        {
            matColorInput0.SetFloat("_ColorR", y_ratio);
            inputFieldR.text = ColorVaule2String(y_ratio);
            LimitInputText(inputFieldR);

            ColorNew.r = y_ratio;
            UpdateColorNew();
        }
    }
    public void OnValueChangedR()
    {
        string str = inputFieldR.text;
        int v = String2ColorVaule(str);
        float rgb = v / 255f;
        ColorNew.r = rgb;
        Debug.Log("str=" + str + " v=" + v + " rgb=" + rgb);
        colorR = v;
        matColorInput0.SetFloat("_ColorR", rgb);
        UpdateColorNew();
    }
    public void OnValueChangedG()
    {
        string str = inputFieldG.text;
        int v = String2ColorVaule(str);
        float rgb = v / 255f;
        ColorNew.g = rgb;
        Debug.Log("str=" + str + " v=" + v + " rgb=" + rgb);
        colorG = v;
        matColorInput1.SetFloat("_ColorG", rgb);
        UpdateColorNew();
        UpdateImageDot();
    }
    public void OnValueChangedB()
    {
        string str = inputFieldB.text;
        int v = String2ColorVaule(str);
        float rgb = v / 255f;
        ColorNew.b = rgb;
        Debug.Log("str=" + str + " v=" + v + " rgb=" + rgb);
        colorB = v;
        matColorInput1.SetFloat("_ColorB", rgb);
        UpdateColorNew();
        UpdateImageDot();
    }

    public void OnInputEndR()
    {
        LimitInputText(inputFieldR);
    }
    public void OnInputEndG()
    {
        LimitInputText(inputFieldG);
    }
    public void OnInputEndB()
    {
        LimitInputText(inputFieldB);
    }

    public void OnClickBtnYes()
    {
        if (callBackUpdateColor != null)
        {
            callBackUpdateColor(ColorNew);
        }
        this.gameObject.SetActive(false);
    }
    public void OnClickBtnNo()
    {
        this.gameObject.SetActive(false);
    }
     public void OnClickBtnBack()
    {
         this.gameObject.SetActive(false);
    }
}
