using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;

public class UICellBase : TableViewCell
{
    public int totalItem;
    public int rowIndex;
    public int oneCellNum;
    public List<UICellItemBase> listItem;
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        listItem = new List<UICellItemBase>();
    }

    public void UpdateItem(List<object> list)
    {
        Debug.Log("UICellBase UpdateItem count=" + listItem.Count);
        int i = 0;
        foreach (UICellItemBase item in listItem)
        {
            item.index = rowIndex * oneCellNum + i;
            if (item.index < totalItem)
            {
                item.ShowContent(true);
                item.UpdateItem(list);
            }
            else
            {
                item.ShowContent(false);
            }
            i++;
        }

    }
    public void ClearAllItem()
    {
        if (listItem == null)
        {
            return;
        }
        foreach (UICellItemBase item in listItem)
        {
            DestroyImmediate(item.gameObject);
        }
        listItem.Clear();
    }
    public void AddItem(UICellItemBase item)
    {
        if (listItem == null)
        {
            return;
        }
        listItem.Add(item);

    }

    public UICellItemBase GetItem(int idx)
    {
        if (listItem == null)
        {
            return null;
        }
        UICellItemBase item = listItem[idx];
        return item;

    }

}
