using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuankaCellItemPintuWord : UICellItemBase
{

    public UIText textTitle;
    public UIImage imageBg;
    public UIImage imageWord;//RawImage
    public UIImage imageSel;
    public UIImage imageIconLock;

    public override void UpdateItem(List<object> list)
    {

        //textTitle.gameObject.SetActive(false);
        ItemInfo info = list[index] as ItemInfo;
        //imageWord.UpdateImage(info.icon);
        imageSel.gameObject.SetActive(false);

        // float scale = Common.GetBestFitScale(imageBg.texture.width, imageBg.texture.height, width, height);
        // imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);
        textTitle.text = info.id;
        int idx_play = LevelManager.main.gameLevelFinish + 1;
        imageWord.gameObject.SetActive(false);
        if (index > idx_play)
        {
            textTitle.gameObject.SetActive(false);
            imageWord.gameObject.SetActive(true);
            imageWord.UpdateImageByKey("IMAGE_GUANKA_CELL_ITEM_BG_LOCK");

        }
        // else if (index == idx_play)
        // {
        //     textTitle.gameObject.SetActive(false);
        //     imageBg.UpdateImageByKey("IMAGE_GUANKA_CELL_ITEM_BG_PLAY");
        // }
        // else
        // {
        //     imageBg.UpdateImageByKey("IMAGE_GUANKA_CELL_ITEM_BG_UNLOCK");
        // }
    }
    public override bool IsLock()
    {
        if (index > (LevelManager.main.gameLevelFinish + 1))
        {
            return true;
        }
        return false;//imageBgLock.gameObject.activeSelf;
    }


}
