using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIProgress : UIView
{
    public enum Type
    {
        SLIDER,
        NO_SLIDER
    }
    public UIImage imageBg;
    public UIImage imageFt;
    public UIImage imageSlider;
 
    private float _percent = 0;//0-100
    public float percent
    {
        get
        {
            return _percent;
        }
        set
        {
            _percent = value;
            LayOutSize ly = imageFt.transform.GetComponent<LayOutSize>();
            ly.ratioW = _percent / 100;
            ly.LayOut();
        }
    }
    private Type _type;
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
                case Type.SLIDER:
                    {
                        imageSlider.gameObject.SetActive(true);
                    }
                    break;
                case Type.NO_SLIDER:
                    {
                        imageSlider.gameObject.SetActive(false);
                    }
                    break;
            }
        }
    }



    /// </summary>
    public void Awake()
    {
        base.Awake();
    }
    // Use this for initialization
    public void Start()
    {
        base.Start();
        this.percent = _percent;
        LayOut();

    }

}
