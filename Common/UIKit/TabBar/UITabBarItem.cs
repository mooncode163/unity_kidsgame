using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public delegate void OnUITabBarItemClickDelegate(UITabBarItem ui);
public class UITabBarItem : UIView
{
    float animaeTime = 0.15f;//0.3
    private bool isActive = false;//true代表正在执行翻转，不许被打断
    public UIImage imageBg;
    public UIText textTitle; 

    public string keyColorSel;

    public OnUITabBarItemClickDelegate callbackClick { get; set; }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void LayOut()
    {
        base.LayOut();
    }
    public void UpdateItem(TabBarItemInfo info)
    {
        textTitle.keyText = info.keyTitle;
        textTitle.UpdateTextByKey(info.keyTitle);
        keyColor = info.keyColor;
        keyColorSel = info.keyColorSel;
        // textTitle.color = GetColorOfKey("TabBarTitle");
        if (!Common.isBlankString(info.pic))
        {
            imageBg.UpdateImage(info.pic, imageBg.keyImage);
        }
        SelectItem(false);
        this.LayOut();
    }

    public void SelectItem(bool isSel)
    {
        textTitle.UpdateColorByKey(isSel ? keyColorSel : keyColor);
        // textTitle.color = (isSel?Color.white:Color.yellow);
    }

    public void OnClickBtnItem()
    {

        StartAnimate();
        // OnAnimateEnd();
        Invoke("OnAnimateEnd", animaeTime * 2);
        // Invoke("OnAnimateEnd", 0.1f);
    }
    public override void UpdateLanguage()
    {
        Debug.Log("UITabBarItem UpdateLanguage ");
        base.UpdateLanguage();
    }
    public void OnAnimateEnd()
    {
        Debug.Log("UITabBarItem OnClickBtnItem ");
        if (callbackClick != null)
        {
            Debug.Log("UITabBarItem OnClickBtnItem 2");
            callbackClick(this);
        }
    }
    public void StartAnimate()
    {
        if (isActive)
            return;
        StartCoroutine(AnimatFlip());
    }
    /// <summary>
    /// 翻转到背面
    /// </summary>
    IEnumerator AnimatFlip()
    {
        GameObject mFront = this.gameObject;//卡牌正面
        GameObject mBack = this.gameObject;//卡牌背面
        mFront.transform.eulerAngles = Vector3.zero;
        // mBack.transform.eulerAngles = new Vector3(0, 90, 0);
        isActive = true;
        mFront.transform.DORotate(new Vector3(0, 90, 0), animaeTime);
        for (float i = animaeTime; i >= 0; i -= Time.deltaTime)
            yield return 0;
        mBack.transform.DORotate(new Vector3(0, 0, 0), animaeTime);
        isActive = false;

    }

}
