using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingViewController : PopViewController
{

    UISettingControllerBase uiSettingPrefab;
    UISettingControllerBase uiSetting;

    static private SettingViewController _main = null;
    public static SettingViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new SettingViewController();
                _main.Init();
            }
            return _main;
        }
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        CreateUI();
    }
    void Init()
    {
        {
            GameObject obj = PrefabCache.main.Load("App/Prefab/Setting/UISettingController");
            if (obj == null)
            {
                obj = PrefabCache.main.Load(AppCommon.PREFAB_SETTING);
            }
            uiSettingPrefab = obj.GetComponent<UISettingControllerBase>();
        }
    }

    public void CreateUI()
    {
        if (this.naviController != null)
        {
            this.naviController.HideNavibar(true);
        }
        uiSetting = (UISettingControllerBase)GameObject.Instantiate(uiSettingPrefab);
        uiSetting.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiSettingPrefab.gameObject, uiSetting.gameObject);
    }

}
