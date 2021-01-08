using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWordListCellItem : UICellItemBase
{

    public UIText textTitle;
    public UIImage imageBg;
    public UIImage imageVideo;
    public override void UpdateItem(List<object> list)
    {
        WordItemInfo info = list[index] as WordItemInfo;
        textTitle.text = info.dbInfo.title;
        LayOut();
        if (index <= GameLevelParse.ADVIDEO_LEVEL_MIN)
        {
            imageVideo.gameObject.SetActive(false);
        }
        Invoke("LayOut", 0.1f);
    }
    public override bool IsLock()
    {
        return false;//imageBgLock.gameObject.activeSelf;
    }
    public override void LayOut()
    {
        base.LayOut();
        RectTransform rctran = textTitle.GetComponent<RectTransform>();
        Debug.Log("UIWordListCellItem rctran=" + rctran.rect);
        //text 
        textTitle.fontSize = (int)(rctran.rect.size.x * 0.7f);
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
