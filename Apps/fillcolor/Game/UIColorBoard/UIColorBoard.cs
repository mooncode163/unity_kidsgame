using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public delegate void OnUIColorBoardDidClickDelegate(UIColorBoard ui, UIColorBoardCellItem item, bool isOutSide);
public class UIColorBoard : UIView
{
    public Text textTitle;
    public Image imageBg;
    public UIColorBoardList uiColorBoardList;

    public OnUIColorBoardDidClickDelegate callBackClick { get; set; }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (uiColorBoardList != null)
        {
            uiColorBoardList.callBackClick = OnUIColorBoardListDidClick;
        }

    }
    // Use this for initialization
    void Start()
    {
        textTitle.text = Language.main.GetString("STR_TITLE_COLOR_BOARD");
        LayoutChild();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LayoutChild()
    {

    }

    public void OnUIColorBoardListDidClick(UIColorBoardList ui, UIColorBoardCellItem item, bool isOutSide)
    {
        if (callBackClick != null)
        {
            callBackClick(this, item, isOutSide);
        }
    }
    public void OnClickBtnBack()
    {
         this.gameObject.SetActive(false);
    }

}
