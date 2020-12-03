using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Moonma.IAP;
using Moonma.Share;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : UIView
{

    public enum Type
    {
        IMAGE = 0,//一张背景  
        IMAGE_TEXT,
        IMAGE_ICON,//一张背景 一张Icon 叠加

        IMAGE_SWITCH,//一张背景
        IMAGE_ICON_SWITCH,//一张背景 一张Icon 叠加

    }

// github 账号：
// 用户名字 helenmooncom
// 邮箱 baoxuehd@foxmail.com
// 密码 helenmoon2020


    public UIImage imageBg;
    public UIImage imageIcon;
    public UIText textTitle;

    bool isSwicthSelect;
    public bool isFitImageSize=true;//true 和image 一样大小
    public Type _type;

    public Type type
    {
        get
        {
            return _type;
        }

        set
        {
            _type = value;
            imageBg.gameObject.SetActive(true);
            switch (_type)
            {
                case Type.IMAGE:
                case Type.IMAGE_SWITCH:
                    {
                        imageIcon.gameObject.SetActive(false);
                        textTitle.gameObject.SetActive(false);
                    }
                    break;
                case Type.IMAGE_TEXT:
                    {
                        imageIcon.gameObject.SetActive(false);
                        textTitle.gameObject.SetActive(true);
                    }
                    break;
                case Type.IMAGE_ICON:
                case Type.IMAGE_ICON_SWITCH:
                    {
                        imageIcon.gameObject.SetActive(true);
                        textTitle.gameObject.SetActive(false);
                    }
                    break;

            }
        }

    }
    public string text
    {
        get
        {
            return textTitle.text;
        }

        set
        {
            textTitle.text = value;
            LayOut();
        }

    }
    public void Awake()
    {
        base.Awake();
        type = _type;
        LayOut();

    }
    // Use this for initialization
    public void Start()
    {
        base.Start();
        LayOut();

    }

    public override void LayOut()
    {
        base.LayOut();
        float w, h;
        if (type == Type.IMAGE_TEXT)
        {
            if (textTitle.isFitFontWidth)
            {
                textTitle.LayOut();
                //自动适配大小
                RectTransform rctranOrigin = this.GetComponent<RectTransform>();
                Vector2 offsetMin = rctranOrigin.offsetMin;
                Vector2 offsetMax = rctranOrigin.offsetMax;
                RectTransform rctran = this.transform as RectTransform;
                Vector2 sizeDelta = rctran.sizeDelta;
                sizeDelta.x = textTitle.width;
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
        if (type == Type.IMAGE)
        {
            if (isFitImageSize)
            {
                //自动适配大小
                RectTransform rctranOrigin = this.GetComponent<RectTransform>();
                Vector2 offsetMin = rctranOrigin.offsetMin;
                Vector2 offsetMax = rctranOrigin.offsetMax;
                RectTransform rctran = this.transform as RectTransform;
                Vector2 sizeDelta = rctran.sizeDelta;
                w = rctran.rect.width;
                h = rctran.rect.height;
                if (imageBg.image.sprite != null)
                {

                    if ((imageBg.image.sprite.texture != null) && (imageBg.image.sprite.texture.width != 0) && (imageBg.image.sprite.texture.height != 0))
                    {
                        w = imageBg.image.sprite.texture.width;
                        h = imageBg.image.sprite.texture.height;
                    }
                }
                rctran.sizeDelta = new Vector2(w, h);
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

    }
    public void UpdateSwitch(bool isSel)
    {
        isSwicthSelect = isSel;
        if (isSwicthSelect)
        {
            imageBg.UpdateImageByKey(imageBg.keyImage);
            imageIcon.UpdateImageByKey(imageIcon.keyImage);
        }
        else
        {
            imageBg.UpdateImageByKey(imageBg.keyImage2);
            imageIcon.UpdateImageByKey(imageIcon.keyImage2);
        }
    }

}
