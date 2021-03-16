using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlaceCellItem : UICellItemBase
{

    public UIText textTitle;
    public UIImage imageBg;
    public UIImage imageIcon;
    public override void UpdateItem(List<object> list)
    {
        ItemInfo info = list[index] as ItemInfo;
        //textTitle.text = (index + 1).ToString();
        textTitle.gameObject.SetActive(false);
        TextureUtil.UpdateImageTexture(imageBg.image, info.pic, true);
        imageIcon.gameObject.SetActive(info.isAd);
        if(Common.noad)
        {
            imageIcon.gameObject.SetActive(false);
        }
        if(info.isAd)
        {
    if (Config.main.isNoIDFASDK && Common.isiOS)
        {
            imageIcon.gameObject.SetActive(!GameManager.main.isHaveUnlockLevel);
        }
        }
      
        if ((Config.main.isNoIDFASDK && Common.isiOS) && (!GameManager.main.isHaveUnlockLevel))
        { 
            imageIcon.UpdateImageByKey("icon_lock");
        }else{
            imageIcon.UpdateImageByKey("BtnIconVideo");
        }
//  imageIcon.UpdateImageByKey("icon_lock");
        LayOut();
    }
    public override bool IsLock()
    {
        return false;//imageBgLock.gameObject.activeSelf;
    }
    public override void LayOut()
    {
        base.LayOut();
        // {
        //     RectTransform rctran = imageBg.GetComponent<RectTransform>();
        //     float w = imageBg.texture.width;//rectTransform.rect.width;
        //     float h = imageBg.texture.height;//rectTransform.rect.height;
        //     RectTransform rctranCellItem = objContent.GetComponent<RectTransform>();

        //     float scalex = width / w;
        //     float scaley = height / h;
        //     float scale = Mathf.Min(scalex, scaley);
        //     scale = scale * 0.8f;
        //     imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);
        // }
    }

}
