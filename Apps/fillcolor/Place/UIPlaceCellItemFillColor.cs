using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlaceCellItemFillColor : UICellItemBase
{

    public Text textTitle;
    public Image imageBg;

    public override void UpdateItem(List<object> list)
    {
        ItemInfo info = list[index] as ItemInfo;
        //textTitle.text = (index + 1).ToString();
        textTitle.gameObject.SetActive(false);
        TextureUtil.UpdateImageTexture(imageBg, info.pic, true);
        LayOut();
    }
    public override bool IsLock()
    {
        return false;//imageBgLock.gameObject.activeSelf;
    }
    public override void LayOut()
    {
        {
            RectTransform rctran = imageBg.GetComponent<RectTransform>();
            float w = imageBg.sprite.texture.width;//rectTransform.rect.width;
            float h = imageBg.sprite.texture.height;//rectTransform.rect.height;
            RectTransform rctranCellItem = objContent.GetComponent<RectTransform>();

            float scalex = width / w;
            float scaley = height / h;
            float scale = Mathf.Min(scalex, scaley);
            scale = scale * 0.8f;
            imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);
        }
    }

}
