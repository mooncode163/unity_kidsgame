
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Moonma.Share;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public interface IUIItemFlowerDelegate
{
    void OnUIItemFlowerTouchDown(UIItemFlower ui);
    void OnUIItemFlowerTouchMove(UIItemFlower ui);
    void OnUIItemFlowerTouchUp(UIItemFlower ui);
}
public class UIItemFlower : UIView
{
    public enum Status
    {
        NORMAL = 0,
        SELECT,
        LOCK,
    }
    public UIImage imageBg;
    public UIText textTitle;
    public int indexRow;
    public int indexCol;
    public int index;
    public bool isAnswerItem;
    public bool isHavePlaced;
    public string word;
    public string wordAnswer;

    private IUIItemFlowerDelegate _delegate;

    public IUIItemFlowerDelegate iDelegate
    {
        get { return _delegate; }
        set { _delegate = value; }
    }

    private Status _status;
    public Status status
    {
        get { return _status; }
        set
        {
            _status = value;
            // imageSel.gameObject.SetActive(false);

            if (_status == Status.NORMAL)
            {
                imageBg.UpdateImageByKey("LetterBgNormal");


            }


            if (_status == Status.SELECT)
            {
                imageBg.UpdateImageByKey("LetterBgSel");

            }

            if (_status == Status.LOCK)
            {
                imageBg.UpdateImageByKey("LetterBgLock");
                textTitle.color = Color.gray;

            }
        }
    }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {

        status = Status.NORMAL;
        LayOut();
    }


    public override void LayOut()
    {
        base.LayOut();
    }
    public void SetFontSize(int size)
    {
        textTitle.fontSize = size;
    }

    public void UpdateItem(string letter)
    {
        word = letter;
        textTitle.text = letter;
    }

    public void OnClickItem()
    {
        Debug.Log("OnUILetterItemDidClick OnClickItem");

    }
}
