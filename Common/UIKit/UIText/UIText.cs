using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;

//text android bug： bestfit 开启可能闪退
public class UIText : UIView
{
    public Text title;
    public bool isFitFontWidth;//和字串等宽
    public float offsetW;
    public float width;
    public string text
    {
        get
        {
            return title.text;
        }

        set
        {
            title.text = value;
            LayOut();
        }

    }

    public int fontSize
    {
        get
        {
            return title.fontSize;
        }

        set
        {
            title.fontSize = value;
            LayOut();
        }

    }
    public Color color
    {
        get
        {
            return title.color;
        }

        set
        {
            title.color = value;
        }

    }

    //



    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public void Awake()
    {
        base.Awake();
        UpdateLanguage();
        Debug.Log("keyColor=" + keyColor);
        if (!Common.isBlankString(keyColor))
        {
            this.color = GetKeyColor();
        }
        if (!Common.isBlankString(keyText))
        {
            this.text = GetKeyText();
        }
        LayOut();
    }
    // Use this for initialization
    public void Start()
    {
        base.Start();
        LayOut();
    }

    public void SetFontSize(int sz)
    {
        title.fontSize = sz;
        LayOut();
    }

    public override void LayOut()
    {
        base.LayOut();
        if (isFitFontWidth)
        {
            RectTransform rctranOrigin = this.GetComponent<RectTransform>();
            Vector2 offsetMin = rctranOrigin.offsetMin;
            Vector2 offsetMax = rctranOrigin.offsetMax;
            float str_w = Common.GetStringLength(this.text, title.font.name, fontSize);
            RectTransform rctran = this.transform as RectTransform;
            Vector2 sizeDelta = rctran.sizeDelta;
            width = str_w + offsetW;
            sizeDelta.x = width;
            rctran.sizeDelta = sizeDelta;
            if ((rctran.anchorMin == new Vector2(0.5f, 0.5f)) && (rctran.anchorMax == new Vector2(0.5f, 0.5f)))
            {
            }
            else
            {
                //sizeDelta 会自动修改offsetMin和offsetMax 所以需要还原
                rctran.offsetMin = offsetMin;
                rctran.offsetMax = offsetMax;
            }

        }
    }
    public override void UpdateLanguage()
    {
        base.UpdateLanguage();
        string str = GetKeyText();
        if (!Common.isBlankString(str))
        {
            this.text = GetKeyText();
        }
        LayOut();
    }

    public void UpdateTextByKey(string key)
    {
        this.text = GetTextOfKey(key);
        LayOut();
    }
    public void UpdateColorByKey(string key)
    {
        this.color = GetColorOfKey(key);
    }
}
