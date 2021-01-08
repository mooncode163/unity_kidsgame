using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PintuViewController : UIViewController
{

    UIPintuController uiPrefab;
    UIPintuController ui;

    static private PintuViewController _main = null;
    public static PintuViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new PintuViewController();
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
            GameObject obj = PrefabCache.main.LoadByKey("UIPintuController");
            uiPrefab = obj.GetComponent<UIPintuController>();
        }
    }

    public void CreateUI()
    {
        if (this.naviController != null)
        {
            this.naviController.HideNavibar(true);
        }
        ui = (UIPintuController)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }
    public void GotoNextLevel()
    {
        ui.GotoNextLevel();
    }
}
