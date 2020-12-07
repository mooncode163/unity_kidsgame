using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeWriteViewController : UIViewController
{

    UIFreeWriteController uiPrefab;
    UIFreeWriteController ui;
    static private FreeWriteViewController _main = null;
    public static FreeWriteViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new FreeWriteViewController();
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
            GameObject obj = PrefabCache.main.LoadByKey("UIFreeWriteController");
            uiPrefab = obj.GetComponent<UIFreeWriteController>();
        }
    }

    public void CreateUI()
    {
        if (this.naviController != null)
        {
            this.naviController.HideNavibar(true);
        }
        ui = (UIFreeWriteController)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }
  

}
