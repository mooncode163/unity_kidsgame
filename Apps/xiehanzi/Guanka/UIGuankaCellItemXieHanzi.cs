using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuankaCellItemXieHanzi : UICellItemBase
{

    public UIText textTitle;
    public UIImage imageBg;
    public UIImage imageAdVideo;
    Color32 [] listColor = {new Color32(81,159,135,255),new Color32(132,136,235,255),new Color32(167,98,93,255),new Color32(182,79,126,255),new Color32(18,126,221,255),new Color32(201,163,142,255)};
    public override void UpdateItem(List<object> list)
    {
        WordItemInfo info = list[index] as WordItemInfo;
        // textTitle.gameObject.SetActive(false);
        if (!Common.isBlankString(info.icon))
        {
            // TextureUtil.UpdateImageTexture(imageBg, info.icon, true);
        }
        textTitle.text = info.id;
        imageAdVideo.gameObject.SetActive(info.isAd);
        if(Config.main.isNoIDFASDK)
        {
        imageAdVideo.UpdateImageByKey("icon_lock");
        }
        textTitle.color = listColor[index%listColor.Length];
        LayOut();
    }
    public override bool IsLock()
    {
        return false;//imageBgLock.gameObject.activeSelf;
    }

    public override void LayOut()
    {
        base.LayOut();

        RectTransform rctran = this.GetComponent<RectTransform>();

        // 0.5f
        textTitle.SetFontSize((int)(width*0.8f));

    }
}
