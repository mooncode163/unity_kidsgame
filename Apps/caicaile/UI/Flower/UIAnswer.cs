
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Moonma.Share;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIAnswer : UIView
{
    public UICellAnswer uiCellAnswerPrefab;
    public List<UICellAnswer> listItem;
    LayOutGrid layOutGrid;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        listItem = new List<UICellAnswer>();
        layOutGrid = this.GetComponent<LayOutGrid>();
        LayOut();
    }

    public void UpdateItem(int level)
    {
        CaiCaiLeItemInfo info = GameLevelParse.main.listGuanka[level] as CaiCaiLeItemInfo;
        for (int i = 0; i < info.listIdiom.Count; i++)
        {
            string strIdiom = info.listIdiom[i];
            UICellAnswer ui = (UICellAnswer)GameObject.Instantiate(uiCellAnswerPrefab);
            ui.transform.SetParent(this.transform);
            ui.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            UIViewController.ClonePrefabRectTransform(uiCellAnswerPrefab.gameObject, ui.gameObject);
            ui.UpdateItem(strIdiom);
            listItem.Add(ui);
        }
        if (info.listIdiom.Count <= 2)
        {
            if (Device.isLandscape)
            {
                layOutGrid.row = 1;
                layOutGrid.col = 2;
            }
            else
            {
                layOutGrid.row = 2;
                layOutGrid.col = 1;
            }
            layOutGrid.LayOut();
        }
        LayOut();
    }

    public bool IsRightAnswer(string word)
    {
        bool ret = false;
        foreach (UICellAnswer ui in listItem)
        {
            if (ui.IsRightAnswer(word))
            {
                ret = true;
                break;
            }
        }
        return ret;
    }
    public int GetIndexAnswer(string word)
    {
        int ret = -1;
        for (int i = 0; i < listItem.Count; i++)
        {
            UICellAnswer ui = listItem[i];
            if (ui.IsRightAnswer(word))
            {
                ret = i;
                break;
            }
        }
        return ret;
    }

    public Vector2 GetPosAnswerLetter(int idxAnswer, int idxLetter)
    {
        UICellAnswer ui = listItem[idxAnswer];
        return ui.GetPosAnswerLetter(idxLetter);
    }

    public void ShowAnswerWord(int idxAnswer, int idxLetter)
    {
        UICellAnswer ui = listItem[idxAnswer];
        ui.Show(idxLetter);

    }

    UIItemAnswer GetFirstItemNotAnswer()
    {
        foreach (UICellAnswer ui in listItem)
        {
            UIItemAnswer item = ui.GetFirstItemNotAnswer();
            if (item != null)
            {
                return item;
            }
        }
        return null;
    }
    public void OnTips()
    {
        UIItemAnswer item = GetFirstItemNotAnswer();
        if (item != null)
        {
            item.Show();
        }
    }

    public override void LayOut()
    {
        base.LayOut();
    }


}
