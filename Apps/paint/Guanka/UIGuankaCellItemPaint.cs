using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuankaCellItemPaint : UICellItemBase
{

    public UIText textTitle;
    public UIImage imagePic;
    public UIImage imageBg;

    public override void UpdateItem(List<object> list)
    {
        ColorItemInfo info = list[index] as ColorItemInfo;
        textTitle.gameObject.SetActive(false);
        imagePic.UpdateImage(info.icon);
        // TextureUtil.UpdateImageTexture(imageBg, info.icon, true);
        LayOut();
    }
    public override bool IsLock()
    {
        return false;//imageBgLock.gameObject.activeSelf;
    }

    public override void LayOut()
    {
        base.LayOut();
        // RectTransform rctran = imageBg.GetComponent<RectTransform>();
        // float ratio = 0.8f;

        // float scale = Common.GetBestFitScale(rctran.rect.width, rctran.rect.height, width, height) * ratio;
        // imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);

    }
}
