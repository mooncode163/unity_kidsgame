using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;

public class PayCell : TableViewCell
{
    public Image imageBg;

    int cellDisplayItemNum;
    int cellItemNum;
    int cellIndex;

    Dictionary<int, PayCellItem> dicItems;

    void Awake()
    {
        dicItems = new Dictionary<int, PayCellItem>();
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetRowNumber(int row, int cellNum, int dispNum)
    {
        cellItemNum = cellNum;
        cellIndex = row;
        cellDisplayItemNum = dispNum;
        if (dicItems.Count == 0)
        {
            return;
        }


        // m_rowNumberText.text = "Row " + rowNumber.ToString();
        // m_background.color = GetColorForRow(rowNumber);
        for (int i = 0; i < dicItems.Count; i++)
        {
            PayCellItem item = dicItems[i];

            // item.itemDelegate = this;
            item.index = cellIndex * cellItemNum + i;
            if (i < cellDisplayItemNum)
            {
                item.Hide(false);
            }
            else
            {
                item.Hide(true);
            }
        }
    }

    public void SetItem(PayCellItem item)
    {

    } 

    public void AddItem(int idx, PayCellItem item)
    {
        dicItems[idx] = item;
        //item.iDelegate = this;
    }



    public void UpdateItem(List<ItemInfo> list)
    {
        for (int i = 0; i < cellDisplayItemNum; i++)
        {
            PayCellItem item = dicItems[i];
            int idx = cellIndex * cellItemNum + i;
            if (idx < list.Count)
            {
                ItemInfo info = list[idx];
                item.UpdateInfo(info);

               
            }

        }
    }

}
