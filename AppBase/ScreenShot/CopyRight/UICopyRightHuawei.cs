using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UICopyRightHuawei : UIView
{
    public Image imageBg;
    public Text textName;
    public Text textAppId;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {



    }
    void Start()
    {
        Config.osDefault = Source.ANDROID;
        bool ishd = false;
        CopyRightViewController copyright = (CopyRightViewController)this.controller;
        if (copyright != null)
        {
            if (copyright.deviceInfo.isIconHd)
            {
                ishd = true;
            }

        }
        string appname = Common.GetAppName();
        if (Application.isEditor || Common.isPC)
        {
            // appname = Config.main.GetAppNameJson(copyright.deviceInfo.isIconHd);
            Language.main.SetLanguage(SystemLanguage.Chinese);
            appname = Language.main.GetString("APP_NAME");
            if(ishd){
                appname = Language.main.GetString("APP_NAME_HD");
            }

        }
        textName.text = appname;

        
        Config.main.ReParseJson(ishd);

        textAppId.text = Config.main.GetAppIdOfStore(Source.HUAWEI);
        Debug.Log("name=" + textName.text + " appid=" + textAppId.text);

        LayOut();
        OnUIDidFinish();
    }
    public override void LayOut()
    {
        base.LayOut();
        float x, y, w = 0, h = 0;
        {
            RectTransform rctran = imageBg.GetComponent<RectTransform>();
            if ((imageBg.sprite != null) && (imageBg.sprite.texture != null))
            {
                w = imageBg.sprite.texture.width;//rectTransform.rect.width;
                h = imageBg.sprite.texture.height;//rectTransform.rect.height;
            }
            if (w != 0)
            {
                float scalex = this.frame.width / w;
                float scaley = this.frame.height / h;
                float scale = Mathf.Min(scalex, scaley);
                imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);

            }

        }
        float w_pic = 750f;
        float h_pic = 1334f;
        RectTransform rctranBg = imageBg.GetComponent<RectTransform>();
        w = rctranBg.rect.width * imageBg.transform.localScale.x;
        h = rctranBg.rect.height * imageBg.transform.localScale.y;

        {
            RectTransform rctran = textName.GetComponent<RectTransform>();
            float ratiox = 555f / w_pic;
            float ratioy = (h_pic - 255) / h_pic;

            Debug.Log("w=" + w + " h=" + h);
            x = rctranBg.anchoredPosition.x - w / 2 + ratiox * w;
            y = rctranBg.anchoredPosition.y - h / 2 + ratioy * h;
            rctran.anchoredPosition = new Vector2(x, y);
        }
        {
            RectTransform rctran = textAppId.GetComponent<RectTransform>();
            float ratiox = 322f / w_pic;
            float ratioy = (h_pic - 292f) / h_pic;
            x = rctranBg.anchoredPosition.x - w / 2 + ratiox * w;
            y = rctranBg.anchoredPosition.y - h / 2 + ratioy * h;
            rctran.anchoredPosition = new Vector2(x, y);
        }

    }
}
