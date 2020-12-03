using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShopCellItem : UICellItemBase
{

    public UIImage imageBg;
    public UIText textTitle;
    public UIButton btnBuy;
    public bool isDisable = false;//显示灰色 
    Color colorDisable = new Color(102 / 255.0f, 102 / 255.0f, 102 / 255.0f, 1f);
    static public string[] strImageBg = { "IMAGE_CELL_BG_BLUE", "IMAGE_CELL_BG_ORINGE", "IMAGE_CELL_BG_YELLOW" };

    public override void UpdateItem(List<object> list)
    {
        ShopItemInfo info = list[index] as ShopItemInfo;
        textTitle.text = info.title;
        // Vector4 border = AppRes.borderCellSettingBg;
        //  TextureUtil.UpdateImageTexture(imageBg, strImageBg[index % 3], false, border);
        imageBg.UpdateImageByKey(strImageBg[index % 3]);
        // Common.SetButtonText(btnBuy, info.artist, 0);
        btnBuy.textTitle.text = info.artist;
        if (info.isIAP)
        {
            btnBuy.gameObject.SetActive(true);
            // offsetMax.x = -232;
        }
        else
        {
            btnBuy.gameObject.SetActive(false);

        }

        //btnBuy.gameObject.SetActive(true);
        LayOut();
        Invoke("LayOut", 0.2F);
    }
    public override bool IsLock()
    {
        return false;//imageBgLock.gameObject.activeSelf;
    }

    public void SetDisable(bool disable)
    {
        isDisable = disable;
        if (disable)
        {
            textTitle.color = colorDisable;
        }
        else
        {
            textTitle.color = Color.white;
        }
    }
    public void OnClickBtnBuy()
    {


    }


}
