
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Moonma.Share;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UICellAnswer : UIView
{
    public int index;
    public UIItemAnswer uiItemAnswerPrefab;
    string strAnswer;
    public List<UIItemAnswer> listItem;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        listItem = new List<UIItemAnswer>();

        // listItem.Add(uiItem0);
        // listItem.Add(uiItem1);
        // listItem.Add(uiItem2);
        // listItem.Add(uiItem3);

        LayOut();
    }


    public override void LayOut()
    {
        base.LayOut();
    }

    public void UpdateItem(string word)
    {
        strAnswer = word;
        for (int i = 0; i < word.Length; i++)
        {
            UIItemAnswer item = (UIItemAnswer)GameObject.Instantiate(uiItemAnswerPrefab);
              item.transform.SetParent(this.transform);
            item.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            listItem.Add(item);
            item.UpdateItem(word.Substring(i, 1));
            LayOutSize lysize = item.GetComponent<LayOutSize>();
            lysize.ratioW = 1.0f / word.Length;
            lysize.LayOut();
        }
    }
    public bool IsRightAnswer(string word)
    {
        bool ret = false;
        if (strAnswer == word)
        {
            ret = true;
        }
        return ret;
    }

    public void Show(int idx)
    {
        UIItemAnswer item = listItem[idx];
        item.Show();
    }
    public Vector2 GetPosAnswerLetter(int idxLetter)
    {
        UIItemAnswer item = listItem[idxLetter];
        return item.transform.position;

    }
    public UIItemAnswer GetFirstItemNotAnswer()
    {
        foreach (UIItemAnswer ui in listItem)
        {
            if (!ui.textTitle.gameObject.activeSelf)
            {
                return ui;
            }
        }
        return null;
    }
}
