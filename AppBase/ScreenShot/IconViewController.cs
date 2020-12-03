using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconViewController : UIViewController
{
    public UIShotBase uiPrefab;
    public UIShotBase ui;
    public ShotDeviceInfo deviceInfo;
    static private IconViewController _main = null;
    public static IconViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new IconViewController();
                _main.Init();
            }
            return _main;
        }
    }

    void Init()
    {
        GameObject obj = null;
        string strPrefabApp = "App/Prefab/ScreenShot/UIIconController";
        string strPrefab = "AppCommon/Prefab/ScreenShot/UIIconController";
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
