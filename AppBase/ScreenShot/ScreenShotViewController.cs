using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotViewController : NaviViewController
{
    public UIScreenShotController uiPrefab;
    public UIScreenShotController ui;
    static private ScreenShotViewController _main = null;
    public static ScreenShotViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new ScreenShotViewController();
                _main.Init();
            }
            return _main;
        }
    }

    void Init()
    {
        int v = Common.Bool2Int(false);
        PlayerPrefs.SetInt(AppVersion.STRING_KEY_APP_CHECK_FINISHED, v);
        AppCommon.USER_COMMENT_DAY_STEP = 100000;
        
        string strPrefab = "Common/Prefab/ScreenShot/UIScreenShotController";
        GameObject obj = PrefabCache.main.Load(strPrefab);
        uiPrefab = obj.GetComponent<UIScreenShotController>();
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        CreateUI();
        HideNavibar(true);
    }
    public override void LayOutView()
    {
        base.LayOutView();
        Debug.Log("ScreenShotViewController LayOutView");
        if ((uiPrefab != null) && (ui != null))
        {
            UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
        }

    }
    public void CreateUI()
    {
        ui = (UIScreenShotController)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }
}
