using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public interface IUIWordTextDelegate
{
    void OnUIWordTextDidFail(UIWordText ui);
    void OnUIWordTextDidOK(UIWordText ui);

    void OnUIWordTextDidClick(UIWordText ui);
}

public class UIWordText : UIView
{
    public UIImage imageBg;
    public UIImage imageStatus;
    public UIText textTitle;
    public string strAnswer;
    public int index;
    public string word;
    bool isStatusOk = false;

    public IUIWordTextDelegate iDelegate;
    void Start()
    {
        imageStatus.gameObject.SetActive(false);
        LayOut();
    }
    public void UpdateTitle(string title)
    {
        isStatusOk = false;
        textTitle.text = title;
        word = title;
        LayOut();
    }

    public override void LayOut()
    {
        base.LayOut();

    }

    public void UpdateStatus(bool isOK)
    {
        imageStatus.gameObject.SetActive(true);
        imageStatus.UpdateImageByKey(isOK ? "StatusOK" : "StatusFail");
        isStatusOk = isOK;
    }
    public void OnItemClick()
    {
        Debug.Log("OnItemClick UIWordText");
        if (!Common.BlankString(strAnswer))
        {
            if (!imageStatus.gameObject.activeSelf)
            {
                if (textTitle.text == strAnswer)
                {
                    UpdateStatus(true);
                    if (iDelegate != null)
                    {
                        iDelegate.OnUIWordTextDidOK(this);
                    }
                }
                else
                {
                    UpdateStatus(false);
                    if (iDelegate != null)
                    {
                        iDelegate.OnUIWordTextDidFail(this);
                    }
                }
            }


        }
        if (iDelegate != null)
        {
            iDelegate.OnUIWordTextDidClick(this);
        }
    }
}
