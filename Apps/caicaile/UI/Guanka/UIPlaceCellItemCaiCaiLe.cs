using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlaceCellItemCaiCaiLe : UICellItemBase
{

    public UIText textTitle;
    public UIImage imageBg;
    public UIImage imageIcon;
    public override void UpdateItem(List<object> list)
    {
        ItemInfo info = list[index] as ItemInfo;

        textTitle.gameObject.SetActive(false);
        bool isShowTitle = false;
        if ((Common.appKeyName == GameRes.GAME_RIDDLE) || (Common.appKeyName == GameRes.GAME_POEM) || (Common.appKeyName == GameRes.GAME_XIEHOUYU))
        {
            isShowTitle = true;
        }
        string pic = info.pic;
        Debug.Log("UIPlaceCellItemCaiCaiLe pic="+pic);
        if (!FileUtil.FileIsExistAsset(pic))
        {
            pic = CloudRes.main.rootPathGameRes + "/place/image/PlaceItemBg.png";
            isShowTitle = true;

        }

        if (isShowTitle == true)
        {
            textTitle.gameObject.SetActive(true);
            LanguageManager.main.UpdateLanguagePlace();
            textTitle.text = LanguageManager.main.languagePlace.GetString("STR_PLACE_" + info.id);
        }

        if (Common.appKeyName == GameRes.GAME_IdiomFlower)
        {
            textTitle.gameObject.SetActive(true);
            textTitle.text = info.title;
        }
         TextureUtil.UpdateImageTexture(imageBg.image, pic, true);
       // imageBg.UpdateImageByKey();

        imageIcon.gameObject.SetActive(info.isAd);
        LayOut();
    }
    public override bool IsLock()
    {
        return false;//imageBgLock.gameObject.activeSelf;
    }
    public override void LayOut()
    {
        base.LayOut();
        {
            // RectTransform rctran = imageBg.GetComponent<RectTransform>();
            // float w = imageBg.texture.width;//rectTransform.rect.width;
            // float h = imageBg.texture.height;//rectTransform.rect.height;
            // RectTransform rctranCellItem = objContent.GetComponent<RectTransform>();

            // float scalex = width / w;
            // float scaley = height / h;
            // float scale = Mathf.Min(scalex, scaley);
            // scale = scale * 0.8f;
            // imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);
        }
    }

}
