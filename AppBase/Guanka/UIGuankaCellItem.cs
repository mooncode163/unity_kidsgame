using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuankaCellItem : UICellItemBase
{

    public UIText textTitle;
    public UIImage imageBg;//RawImage
    public UIImage imageSel;
    public UIImage imageIconLock;

    public override void UpdateItem(List<object> list)
    {
        textTitle.text = (index + 1).ToString();
        textTitle.fontSize = (int)(height * 0.5f);
        imageSel.gameObject.SetActive(false);
        textTitle.gameObject.SetActive(true);
        int idx_play = LevelManager.main.gameLevelFinish + 1;
        if (index > idx_play)
        {
            // if (!Application.isEditor)
            {
                textTitle.gameObject.SetActive(false);
                imageBg.UpdateImageByKey("IMAGE_GUANKA_CELL_ITEM_BG_LOCK"); 
            }

        }
        else if (index == idx_play)
        {
            textTitle.gameObject.SetActive(false);
            imageBg.UpdateImageByKey("IMAGE_GUANKA_CELL_ITEM_BG_PLAY"); 
        }
        else
        { 
            imageBg.UpdateImageByKey("IMAGE_GUANKA_CELL_ITEM_BG_UNLOCK"); 
        }
        LayOut();
    }
    public override bool IsLock()
    {
        if (index > (LevelManager.main.gameLevelFinish + 1))
        {
            return true;
        }
        return false;//imageBgLock.gameObject.activeSelf;
    }

    public override void LayOut()
    {
        base.LayOut();
        RectTransform rctran = imageBg.GetComponent<RectTransform>();
        float ratio = 0.9f;
        if (Common.appType == AppType.SHAPECOLOR)
        {
            ratio = 0.9f;
        }
        float scale = Common.GetBestFitScale(rctran.rect.width, rctran.rect.height, width, height) * ratio;
      //  imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);

    }
}
