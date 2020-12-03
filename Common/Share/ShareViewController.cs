using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShareViewController : PopViewController
{
    public UIShare uiPrefab;
    public UIShare ui;
    static private ShareViewController _main = null;
    public static ShareViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new ShareViewController();
                _main.Init();
            }
            return _main;
        }
    }

    public OnUIShareDidClickDelegate callBackClick { get; set; }

    void Init()
    {
        string strPrefab = "Common/Prefab/Share/UIShare";
        GameObject obj = PrefabCache.main.Load(strPrefab);
        uiPrefab = obj.GetComponent<UIShare>();
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        CreateUI();
    }
    public override void LayOutView()
    {
        base.LayOutView();

    }
    public void CreateUI()
    {
        ui = (UIShare)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
        ui.callBackClick = OnUIShareDidClick;
    }



    public void OnUIShareDidClick(ItemInfo item)
    {
        if (callBackClick != null)
        {
            callBackClick(item);
        }
    }
}
