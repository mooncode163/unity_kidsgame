using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class UIHowPlayFlowerPage : UIHowToPlayPageBase
{
    public UIText textTitle;
    public UIText textDetail;
    public UIImage imageIcon;

    protected virtual void Awake()
    {
        textTitle.text = Language.main.GetString("STR_HOWPLAY_TITLE" + index.ToString());
        textDetail.text = Language.main.GetString("STR_HOWPLAY_DETAIL" + index.ToString());
    }
    void Start()
    {

    }
    public override void LayOut()
    {
        base.LayOut();
    }


}
