using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudResViewController : UIViewController
{

    UICloudResController uiPrefab;
    UICloudResController ui;
    static private CloudResViewController _main = null;
    public static CloudResViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new CloudResViewController();
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
            GameObject obj = PrefabCache.main.Load("Common/Prefab/CloudRes/UICloudResController");
            uiPrefab = obj.GetComponent<UICloudResController>();
        }
    }

    public void CreateUI()
    {
        if (this.naviController != null)
        {
            this.naviController.HideNavibar(true);
        }
        ui = (UICloudResController)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }
  

}
