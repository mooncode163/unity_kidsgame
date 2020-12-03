using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void OnUINaviBarClickBackDelegate(UINaviBar bar);

public class UINaviBar : UIView
{
    public UIImage imageBg;
    public UIText textTitle;
    public UIButton btnBack;
    public OnUINaviBarClickBackDelegate callbackBackClick { get; set; }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {

    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowTitle(bool isShow)
    {
        textTitle.gameObject.SetActive(isShow);
    }

    public void UpdateTitle(string title)
    {
        textTitle.text = title;
        textTitle.color = GetColorOfKey("NaviBarTitle");
    }

    public void HideBtnBack(bool isHide)
    {
        btnBack.gameObject.SetActive(!isHide);
    }

    public void OnClickBtnBack()
    {
        if (callbackBackClick != null)
        {
            callbackBackClick(this);
        }
    }
}

