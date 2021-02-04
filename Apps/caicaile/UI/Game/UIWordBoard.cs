using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUIWordBoardDelegate
{
    /// <summary>
    /// Get the number of rows that a certain table should display
    /// </summary>

    void UIWordBoardDidClick(UIWordBoard bd, UIWordItem item);

}


public class UIWordBoard : UIView, IUIWordItemDelegate
{
    public UIWordItem wordItemPrefab;
    List<object> listItem;
    public int row = 6;
    public int col = 4;
    Sprite spriteBg;

    private IUIWordBoardDelegate _delegate;
    public IUIWordBoardDelegate iDelegate
    {
        get { return _delegate; }
        set { _delegate = value; }
    }


    void Awake()
    {
        spriteBg = TextureUtil.CreateSpriteFromResource("App/UI/Common/word");
        InitItem();
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }



    public void InitItem()
    {
        if (listItem == null)
        {
            listItem = new List<object>();
        }
        else
        {
            foreach (UIWordItem item in listItem)
            {
                Destroy(item.gameObject);
            }
            listItem.Clear();
        }

        int len = row * col;
        for (int i = 0; i < len; i++)
        {
            string word = i.ToString();
            //Debug.Log(word);
            UIWordItem item = GameObject.Instantiate(wordItemPrefab);
            item.index = i;
            item.iDelegate = this;
            item.transform.SetParent(this.transform);
            item.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            item.UpdateTitle(word);
            item.imageBg.sprite = spriteBg;
            item.SetWordColor(ColorConfig.main.GetColor("BoardTitle"));
            item.SetFontSize(64);
            listItem.Add(item);
        }
    }

    public int GetItemCount()
    {
        int count = 0;
        if (listItem != null)
        {
            count = listItem.Count;
        }
        return count;
    }

    public UIWordItem GetItem(int idx)
    {
        foreach (UIWordItem item in listItem)
        {
            if (idx == item.index)
            {
                return item;
            }
        }
        return null;
    }



    public void UpdateItem(CaiCaiLeItemInfo info, string strBoard)
    {
        foreach (UIWordItem item in listItem)
        {
            item.ShowContent(true);
        }

        int idx = 0;

        for (int i = 0; i < strBoard.Length; i++)
        {
            string word = strBoard.Substring(i, 1);
            if (idx >= listItem.Count)
            {
                Debug.Log("UIWordBoard idx:" + idx);
                continue;
            }
            UIWordItem item = listItem[idx] as UIWordItem;

            item.UpdateTitle(word);
            idx++;
        }
    }

    //退回字符
    public void BackWord(string word)
    {
        foreach (UIWordItem item in listItem)
        {
            if (word == item.wordDisplay)
            {
                item.ShowContent(true);
                break;
            }
        }
    }

    public void HideWord(string word)
    {
        foreach (UIWordItem item in listItem)
        {
            if (item.isShowContent)
            {
                if (word == item.wordDisplay)
                {
                    item.ShowContent(false);
                    break;
                }
            }
        }
    }

    public void OnReset()
    {
        foreach (UIWordItem item in listItem)
        {
            item.ShowContent(true);
        }
    }

    public void WordItemDidClick(UIWordItem item)
    {
        if (!item.isShowContent)
        {
            return;
        }


        if (_delegate != null)
        {
            _delegate.UIWordBoardDidClick(this, item);
        }


    }
}
