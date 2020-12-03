using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class UIWordStatusItem : UIView
{
    public UIText textName;
    public UIText textOrder;
    public int index;
    Color colorSel = Color.red;
    Color colorUnSel = Color.white;
    public bool isSelectStatus = false;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {

    }

    public void UpdateItem(WordItemInfo info)
    {
        textName.text = info.listBiHuaName[index];
        textOrder.text = info.listBiHuaOrder[index];
    }
     public bool IsSelect()
     {
         return isSelectStatus;
     }
    public void SetStatus(bool isSel)
    {
        isSelectStatus = isSel;
        if (isSel)
        {
            textName.color = colorSel;
            textOrder.color = colorSel;
        }
        else
        {
            textName.color = colorUnSel;
            textOrder.color = colorUnSel;
        }

    }

}
