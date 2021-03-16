using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuankaCellItemPintu : UICellItemBase
{

    public UIText textTitle;

    public UIImage imageBg;//RawImage
    public UIImage imageSel;
    public UIImage imageIconLock;

    public override void UpdateItem(List<object> list)
    {
        textTitle.text = (index + 1).ToString();
        textTitle.gameObject.SetActive(false);
        ItemInfo info = list[index] as ItemInfo;
        //    TextureUtil.UpdateRawImageTexture(imageBg, info.icon, true);
        imageBg.UpdateImage(info.icon);
        imageSel.gameObject.SetActive(false);

        // float scale = Common.GetBestFitScale(imageBg.texture.width, imageBg.texture.height, width, height);
        // imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);

        // int idx_play = LevelManager.main.gameLevelFinish + 1;
        // if (index > idx_play)
        // {
        //     // if (!Application.isEditor)
        //     {
        //         textTitle.gameObject.SetActive(false);
        //         imageBg.UpdateImageByKey("IMAGE_GUANKA_CELL_ITEM_BG_LOCK");
        //     }

        // }
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
        return false;//imageBgLock.gameObject.activeSelf;
    }


}
