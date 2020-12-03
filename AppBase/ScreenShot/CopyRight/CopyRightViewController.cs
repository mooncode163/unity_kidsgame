using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyRightViewController : UIViewController
{
    public UIView uiPrefab;
    public UIView ui;
    public ShotDeviceInfo deviceInfo;
    static private CopyRightViewController _main = null;
    public static CopyRightViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new CopyRightViewController();
                _main.Init();
            }
            return _main;
        }
    }

    void Init()
    {
        string strPrefab = "Common/Prefab/ScreenShot/UICopyRightHuawei";
        GameObject obj = PrefabCache.main.Load(strPrefab);
        uiPrefab = obj.GetComponent<UIView>();
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
        ui = (UIView)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }
}
