using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdHomeViewController : UIViewController
{
    public UIShotBase uiPrefab;
    public UIShotBase ui;
    static private AdHomeViewController _main = null;
    public static AdHomeViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new AdHomeViewController();
                _main.Init();
            }
            return _main;
        }
    }

    void Init()
    {
        GameObject obj = null;
        string strPrefabApp = "App/Prefab/ScreenShot/UIAdHomeController";
        string strPrefab = "AppCommon/Prefab/ScreenShot/UIAdHomeController";
        obj = PrefabCache.main.Load(strPrefabApp);
        if (obj == null)
        {
            obj = PrefabCache.main.Load(strPrefab);
        }

        uiPrefab = obj.GetComponent<UIShotBase>();
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        CreateUI();
    }
    public override void LayOutView()
    {
        base.LayOutView();
        Debug.Log("AdHomeViewController LayOutView ");
        if (ui != null)
        {
            ui.LayOut();
        }
    }
    public void CreateUI()
    {
        ui = (UIShotBase)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }
}
