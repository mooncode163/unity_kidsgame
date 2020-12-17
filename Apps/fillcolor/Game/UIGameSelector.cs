using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public delegate void OnUIGameSelectorDidCloseDelegate(UIGameSelector ui, bool isNew);
public class UIGameSelector : UIView
{
    public Button btnNew;
    public Button btnContinue;
    public UIGameFillColor uiGameFillColor;
    public OnUIGameSelectorDidCloseDelegate callbackClose { get; set; }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        {
            Transform tr = btnNew.transform.Find("Text");
            Text btnText = tr.GetComponent<Text>();
            string str = Language.main.GetString("STR_GAME_SELECTOR_NEW");
            btnText.text = str;
            float str_w = Common.GetStringLength(str, AppString.STR_FONT_NAME, btnText.fontSize);
            RectTransform rctran = btnNew.transform as RectTransform;
            Vector2 sizeDelta = rctran.sizeDelta;
            float oft = 0;
            sizeDelta.x = str_w + btnText.fontSize;
            rctran.sizeDelta = sizeDelta;
        }
        {
            Transform tr = btnContinue.transform.Find("Text");
            Text btnText = tr.GetComponent<Text>();
            string str = Language.main.GetString("STR_GAME_SELECTOR_CONTINUE");
            btnText.text = str;
            float str_w = Common.GetStringLength(str, AppString.STR_FONT_NAME, btnText.fontSize);
            RectTransform rctran = btnContinue.transform as RectTransform;
            Vector2 sizeDelta = rctran.sizeDelta;
            float oft = 0;
            sizeDelta.x = str_w + btnText.fontSize;
            rctran.sizeDelta = sizeDelta;
        }




        this.gameObject.SetActive(true);

    }
    // Use this for initialization
    void Start()
    {
        if (Common.isMonoPlayer)
        {
            OnClickBtnContinue();
        }
        NaviViewController navi = uiGameFillColor.controller.naviController;
        if (navi.source == AppRes.SOURCE_NAVI_HISTORY)
        {
            this.gameObject.SetActive(false);
            OnClickBtnContinue();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Show(bool isShow)
    {
        this.gameObject.SetActive(isShow);
    }
    public void OnClickBtnBack()
    {
        NaviViewController navi = uiGameFillColor.controller.naviController;
        if (navi != null)
        {
            navi.Pop();
        }
    }
    public void OnClickBtnNew()
    {

        OnClose(true);
    }
    public void OnClickBtnContinue()
    {

        OnClose(false);
    }

    void OnClose(bool isNew)
    {
        if (callbackClose != null)
        {
            callbackClose(this, isNew);
        }
        Show(false);
    }
}
