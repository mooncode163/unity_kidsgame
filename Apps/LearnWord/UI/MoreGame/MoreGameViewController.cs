using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreGameViewController : UIViewController
{

    UIMoreGameController uiPrefab;
    UIMoreGameController ui;

    public string currentGameId;
    static private MoreGameViewController _main = null;
    public static MoreGameViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new MoreGameViewController();
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
            GameObject obj = PrefabCache.main.LoadByKey("UIMoreGameController");
            uiPrefab = obj.GetComponent<UIMoreGameController>();
        }
    }

    public void CreateUI()
    {
        if (this.naviController != null)
        {
            this.naviController.HideNavibar(true);
        }
        ui = (UIMoreGameController)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }

}
