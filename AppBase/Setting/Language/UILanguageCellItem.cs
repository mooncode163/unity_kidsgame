using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class UILanguageCellItem : UICellItemBase
{
    public UIText textTitle;
    public UIImage imageBg;

    static public string[] strImageBg = { "IMAGE_CELL_BG_BLUE", "IMAGE_CELL_BG_ORINGE", "IMAGE_CELL_BG_YELLOW" };

    public override void UpdateItem(List<object> list)
    {
        if (index < list.Count)
        {
            ItemInfo info = list[index] as ItemInfo;
            textTitle.text = info.title;
            tagValue = info.tag;
            string key = strImageBg[index % 3];
            imageBg.keyImage = key;
            imageBg.UpdateImageByKey(key);
        }
    }

}



