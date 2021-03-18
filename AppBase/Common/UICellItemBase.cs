using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public delegate void OnUICellItemClickDelegate(UICellItemBase item);
public class UICellItemBase : UIView
{
    public GameObject objContent;
    public float width;
    public float height; 
    public int totalItem;
    public int tagValue;
    bool isItemLock = false;
    public OnUICellItemClickDelegate callbackClick { get; set; }
    /// Awake is called when the script instance is being loaded.
    /// </summary> 
    public virtual int GetCellHeight()
    {
        return 0;
    }
    public virtual void UpdateItem(List<object> list)
    {
        Debug.Log("UICellItemBase UpdateItem");
    } 
      public virtual bool IsLock()
    {
        return false;
    }
    public void OnItemClick()
    {
        if (callbackClick != null)
        {
            callbackClick(this);
        }
    }

    public void ShowContent(bool isShow)
    {
        if (objContent != null)
        {
            objContent.SetActive(isShow);
        }
    }


}
