using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBookCellItem : UICellItemBase
{

    public UIText textTitle;
    public UIImage imageBg;
    public UIImage imageItem;
    public override void UpdateItem(List<object> list)
    {
        ItemInfo info = list[index] as ItemInfo;
        textTitle.text = Language.main.GetString("Book_"+info.id);
        // textTitle.gameObject.SetActive(false);
        imageItem.UpdateImage(info.pic); 
        // imageIcon.gameObject.SetActive(info.isAd);
        LayOut();
    }
    public override bool IsLock()
    {
        return false;//imageBgLock.gameObject.activeSelf;
    }
    public override void LayOut()
    {
        base.LayOut();
       
    }

}
