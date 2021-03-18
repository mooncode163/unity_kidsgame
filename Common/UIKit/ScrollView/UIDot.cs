using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDot : UIView
{
    public const string PREFAB_UIDOT = "Common/Prefab/UIKit/ScrollView/UIDot";
    // public const string IMAGE_DOT_UNSEL = "Common/Prefab/UIKit/ScrollView/scroll_dot_unsel";
    // public const string IMAGE_DOT_SEL = "Common/Prefab/UIKit/ScrollView/scroll_dot_sel";

    public const string IMAGE_DOT_UNSEL = "AppCommon/UI/UIKit/ScrollView/scroll_dot_unsel";
    public const string IMAGE_DOT_SEL = "AppCommon/UI/UIKit/ScrollView/scroll_dot_sel";
    public Image image; 
    public void UpdateItem(bool isSel)
    {
        TextureUtil.UpdateImageTexture(image, isSel ? IMAGE_DOT_SEL : IMAGE_DOT_UNSEL, false);
    }
}

