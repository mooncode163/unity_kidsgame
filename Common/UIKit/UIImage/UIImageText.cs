using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIImageText : UIView
{
    public enum Type
    {
        NONE = 0,
        IMAGE_TEXT,//Image 背景 
        IMAGE_UP_TEXT_DOWN,//Image在上面 Text在下面
        IMAGE_ONLY,
        TEXT_ONLY
    }

    public UIImage imageBg;
    public UIText textTitle;

    public Type _type = Type.NONE;
    public Type type
    {
        set
        {
            _type = value;
            LayOut();
        }
        get
        {
            return _type;
        }
    }

     void Start()
    { 
        LayOut();
    }
    public void UpdateTitle(string title)
    {
        textTitle.text = title;
        LayOut();
    }
    public void UpdateImageByKey(string pic)
    {
        // TextureUtil.UpdateImageTexture(imageBg, pic, true);
        imageBg.UpdateImageByKey(pic);
    }


    public override void LayOut()
    {
        base.LayOut();
        RectTransform rctran = this.GetComponent<RectTransform>();
        RectTransform rctranText = textTitle.GetComponent<RectTransform>();
        RectTransform rctranImage = imageBg.GetComponent<RectTransform>();
        float x, y, w, h;
        float scale, scalex, scaley;
        textTitle.gameObject.SetActive(true);
        imageBg.gameObject.SetActive(true);
        switch (_type)
        {
            case Type.NONE:
                {

                }
                break;
            case Type.IMAGE_ONLY:
                {
                    textTitle.gameObject.SetActive(false);
                    //image
                    {
                        w = rctran.rect.width;
                        h = rctran.rect.height;
                        if ((imageBg.image.sprite.texture != null) && (imageBg.image.sprite.texture.width != 0) && (imageBg.image.sprite.texture.height != 0))
                        {
                            scalex = w / imageBg.image.sprite.texture.width;
                            scaley = h / imageBg.image.sprite.texture.height;
                            scale = Mathf.Min(scalex, scaley);
                            imageBg.transform.localScale = new Vector3(scale, scale, 1f);
                        }
                        x = 0;
                        y = 0;
                        rctranImage.anchoredPosition = new Vector2(x, y);
                    }
                }
                break;
            case Type.TEXT_ONLY:
                {
                    imageBg.gameObject.SetActive(false);
                    //text
                    {
                        if (Common.BlankString(textTitle.text))
                        {
                            w = rctran.rect.width;
                        }
                        else
                        {
                            w = Common.GetStringLength(textTitle.text, AppString.STR_FONT_NAME, textTitle.fontSize);
                        }
                        h = rctran.rect.height;
                        rctranText.sizeDelta = new Vector2(w, h);
                        x = 0;
                        y = 0;
                        rctranText.anchoredPosition = new Vector2(x, y);
                    }
                }
                break;

            case Type.IMAGE_UP_TEXT_DOWN:
                {
                    //text
                    {
                        if (Common.BlankString(textTitle.text))
                        {
                            w = rctran.rect.width;
                        }
                        else
                        {
                            w = Common.GetStringLength(textTitle.text, AppString.STR_FONT_NAME, textTitle.fontSize);
                        }
                        h = 100;
                        rctranText.sizeDelta = new Vector2(w, h);
                        x = 0;
                        y = -rctran.rect.height / 2 + h / 2;
                        rctranText.anchoredPosition = new Vector2(x, y);
                    }
                    //image
                    {
                        w = rctran.rect.width;
                        h = rctran.rect.height - rctranText.rect.height;
                        if ((imageBg.image.sprite.texture != null) && (imageBg.image.sprite.texture.width != 0) && (imageBg.image.sprite.texture.height != 0))
                        {
                            scalex = w / imageBg.image.sprite.texture.width;
                            scaley = h / imageBg.image.sprite.texture.height;
                            scale = Mathf.Min(scalex, scaley);
                            imageBg.transform.localScale = new Vector3(scale, scale, 1f);
                        }



                        x = 0;
                        y = rctran.rect.height / 2 - h / 2;
                        rctranImage.anchoredPosition = new Vector2(x, y);
                    }


                }
                break;


                  case Type.IMAGE_TEXT:
                {
                    //text
                    {
                        if (Common.BlankString(textTitle.text))
                        {
                            w = rctran.rect.width;
                        }
                        else
                        {
                            w = Common.GetStringLength(textTitle.text, AppString.STR_FONT_NAME, textTitle.fontSize);
                        }
                        h = rctran.rect.height;
                        rctranText.sizeDelta = new Vector2(w, h);
                        x = 0;
                        y = 0;
                        rctranText.anchoredPosition = new Vector2(x, y);
                    }
                    //image
                    {
                        w = rctran.rect.width;
                        h = rctran.rect.height;
                        // if ((imageBg.image.sprite.texture != null) && (imageBg.image.sprite.texture.width != 0) && (imageBg.image.sprite.texture.height != 0))
                        // {
                        //     scalex = w / imageBg.image.sprite.texture.width;
                        //     scaley = h / imageBg.image.sprite.texture.height;
                        //     scale = Mathf.Min(scalex, scaley);
                        //     imageBg.transform.localScale = new Vector3(scale, scale, 1f);
                        // }

                         rctranImage.sizeDelta = new Vector2(w, h);

                        x = 0;
                        y = 0;
                        rctranImage.anchoredPosition = new Vector2(x, y);
                    }


                }
                break;

        }
    }
}
