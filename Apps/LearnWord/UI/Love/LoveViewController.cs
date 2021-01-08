using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoveViewController : PopViewController
{

    UILoveController uiPrefab;
    UILoveController ui;

    static private LoveViewController _main = null;
    public static LoveViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new LoveViewController();
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
            GameObject obj = PrefabCache.main.Load("App/Prefab/Love/UILoveController");
            uiPrefab = obj.GetComponent<UILoveController>();
        }
    }

    public void CreateUI()
    {
        if (this.naviController != null)
        {
            this.naviController.HideNavibar(true);
        }
        ui = (UILoveController)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }

}
