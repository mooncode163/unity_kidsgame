using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public delegate void OnUITabBarItemClickDelegate(UITabBarItem ui);
public class UITabBarItem : UIView
{
    public UIImage imageBg;
    public UIText textTitle;
    public int index;
 
    public string keyColorSel;

    public OnUITabBarItemClickDelegate callbackClick { get; set; }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void LayOut()
    {
        base.LayOut();
    }
    public void UpdateItem(TabBarItemInfo info)
    {
        textTitle.keyText = info.keyTitle;
        textTitle.UpdateTextByKey(info.keyTitle);
        keyColor = info.keyColor;
        keyColorSel = info.keyColorSel;
        // textTitle.color = GetColorOfKey("TabBarTitle");
        if (!Common.isBlankString(info.pic))
        {
            imageBg.UpdateImage(info.pic, imageBg.keyImage);
        }
        SelectItem(false);
        this.LayOut();
    }

     public void SelectItem(bool isSel)
    {
        textTitle.UpdateColorByKey(isSel?keyColorSel:keyColor);
        // textTitle.color = (isSel?Color.white:Color.yellow);
    }

    public void OnClickBtnItem()
    {
        Debug.Log("UITabBarItem OnClickBtnItem ");
        if (callbackClick != null)
        {
            Debug.Log("UITabBarItem OnClickBtnItem 2");
            callbackClick(this);
        }
    }
       public override void UpdateLanguage()
    {
        Debug.Log("UITabBarItem UpdateLanguage ");
        base.UpdateLanguage();
    }
}
