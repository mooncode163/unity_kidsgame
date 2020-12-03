using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;

public interface IUIInputBarDelegate
{
    void OnUIInputBarValueChanged(UIInputBar ui);
    void OnUIInputBarEnd(UIInputBar ui);
}
public class UIInputBar : UIView
{
    public InputField inputField;
    public Text titlePlaceHold;
    public bool isFitFontWidth;//和字串等宽
    public float offsetW;

    public string keyTextPlaceHold;
    public string keyColorPlaceHold;
    public IUIInputBarDelegate iDelegate;
    public string text
    {
        get
        {
            return inputField.text;
        }

        set
        {
            inputField.text = value;
            LayOut();
        }

    }

    public string textPlaceHold
    {
        get
        {
            return titlePlaceHold.text;
        }

        set
        {
            titlePlaceHold.text = value;
            LayOut();
        }

    }

    public int fontSize
    {
        get
        {
            return inputField.textComponent.fontSize;
        }

        set
        {
            inputField.textComponent.fontSize = value;
            LayOut();
        }

    }
    public int fontSizePlaceHold
    {
        get
        {
            return titlePlaceHold.fontSize;
        }

        set
        {
            titlePlaceHold.fontSize = value;
            LayOut();
        }

    }

    public Color color
    {
        get
        {
            return inputField.textComponent.color;
        }

        set
        {
            inputField.textComponent.color = value;
        }

    }

    public Color colorPlaceHold
    {
        get
        {
            return titlePlaceHold.color;
        }

        set
        {
            titlePlaceHold.color = value;
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
        if (!Common.isBlankString(keyColorPlaceHold))
        {
            this.colorPlaceHold = GetColorOfKey(keyColorPlaceHold);
        }


        LayOut();
    }
    // Use this for initialization
    public void Start()
    {
        base.Start();
    }

    public void SetFontSize(int sz)
    {
        // title.fontSize = sz;
        LayOut();
    }

    public override void LayOut()
    {
        base.LayOut();

    }
    public override void UpdateLanguage()
    {
        base.UpdateLanguage();
        if (!Common.isBlankString(keyText))
        {
            this.text = GetKeyText();
        }
        if (!Common.isBlankString(keyTextPlaceHold))
        {
            this.textPlaceHold = GetTextOfKey(keyTextPlaceHold);
        }

        LayOut();
    }

    public void OnInputFieldValueChanged()
    {
        Debug.Log("OnInputFieldValueChanged text=" + this.text);
        if (iDelegate != null)
        {
            iDelegate.OnUIInputBarValueChanged(this);
        }
    }

    public void OnInputFieldEnd()
    {
        Debug.Log("OnInputFieldEnd text=" + this.text);
        if (iDelegate != null)
        {
            iDelegate.OnUIInputBarEnd(this);
        }
    }
}
