using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public delegate void OnUILineSettingLineWidthDelegate(int width);
public class UILineSetting : UIView
{
    public  int LINE_WIDTH_PIXSEL_MIN = 8;
    public  int LINE_WIDTH_PIXSEL_MAX = 900;//基于参考分辨率 2048x1536
    public Slider slider;
    public Text textPixsel;
    public Text textTitle;
    public Image imageBoard;

    public InputField inputFieldPixsel;

    public int lineWidthPixsel = 8;

    public OnUILineSettingLineWidthDelegate callBackSettingLineWidth { get; set; }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {

    }
    // Use this for initialization
    void Start()
    {

        textTitle.text = Language.main.GetString("STR_TITLE_LINE_WIDTH_SETTING");
        {

            string str = Language.main.GetString("STR_PEN_LINEWIDTH_SETTING_PIXSEL");
            textPixsel.text = str;
            float str_w = Common.GetStringLength(str, AppString.STR_FONT_NAME, textPixsel.fontSize);
            RectTransform rctran = textPixsel.transform as RectTransform;
            Vector2 sizeDelta = rctran.sizeDelta;
            float oft = 4 * AppCommon.scaleBase;
            sizeDelta.x = str_w + oft * 2;
            rctran.sizeDelta = sizeDelta;
        }
        inputFieldPixsel.text = lineWidthPixsel.ToString();
        slider.value = (lineWidthPixsel - LINE_WIDTH_PIXSEL_MIN) * 1f / (LINE_WIDTH_PIXSEL_MAX - LINE_WIDTH_PIXSEL_MIN);

        LayoutChild();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LayoutChild()
    {
        float x = 0, y = 0, w = 0, h = 0;
        RectTransform rctranParent = GetComponent<RectTransform>();
        {
            RectTransform rctran = imageBoard.GetComponent<RectTransform>();
            float ratio = 1f;
            w = rctranParent.rect.width * ratio;
            h = rctranParent.rect.height * ratio;
            rctran.sizeDelta = new Vector2(w, h);
        }

        {
            RectTransform rctran = slider.GetComponent<RectTransform>();
            float ratio = 0.8f;
            w = rctranParent.rect.width * ratio;
            h = rctran.sizeDelta.y;
            rctran.sizeDelta = new Vector2(w, h);
        }


 
    }

    public void OnValueChangedInputFieldPixsel()
    {
        Debug.Log("OnValueChangedInputFieldPixsel");

        string str = inputFieldPixsel.text;
        lineWidthPixsel = Common.String2Int(str);
        float value = (lineWidthPixsel - LINE_WIDTH_PIXSEL_MIN) * 1f / (LINE_WIDTH_PIXSEL_MAX - LINE_WIDTH_PIXSEL_MIN);
        if (value < 0)
        {
            value = 0;
        }
        if (value > 1)
        {
            value = 1;
        }
        slider.value = value;
    }


    public void OnInputEndInputFieldPixsel()
    {
        if (lineWidthPixsel < LINE_WIDTH_PIXSEL_MIN)
        {
            lineWidthPixsel = LINE_WIDTH_PIXSEL_MIN;
        }
        if (lineWidthPixsel > LINE_WIDTH_PIXSEL_MAX)
        {
            lineWidthPixsel = LINE_WIDTH_PIXSEL_MAX;
        }
        inputFieldPixsel.text = lineWidthPixsel.ToString();
        if (callBackSettingLineWidth != null)
        {
            callBackSettingLineWidth(lineWidthPixsel);
        }
        slider.value = (lineWidthPixsel - LINE_WIDTH_PIXSEL_MIN) * 1f / (LINE_WIDTH_PIXSEL_MAX - LINE_WIDTH_PIXSEL_MIN);
    }

    public void OnSliderValueChanged(float value)
    {

        lineWidthPixsel = LINE_WIDTH_PIXSEL_MIN + (int)(slider.value * (LINE_WIDTH_PIXSEL_MAX - LINE_WIDTH_PIXSEL_MIN));

        inputFieldPixsel.text = lineWidthPixsel.ToString();
        if (callBackSettingLineWidth != null)
        {
            callBackSettingLineWidth(lineWidthPixsel);
        }
        Debug.Log("OnSliderValueChanged:value=" + value + " lineWidthPixsel=" + lineWidthPixsel);
    }
    public void OnClickBtnClose()
    {
        this.gameObject.SetActive(false);
    }
}
