using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchViewController : UIViewController
{

    UIMatchController uiPrefab;
    UIMatchController ui;

    static private MatchViewController _main = null;
    public static MatchViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new MatchViewController();
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
            GameObject obj = PrefabCache.main.LoadByKey("UIMatchController");
            uiPrefab = obj.GetComponent<UIMatchController>();
        }
    }

    public void CreateUI()
    {
        if (this.naviController != null)
        {
            this.naviController.HideNavibar(true);
        }
        ui = (UIMatchController)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }
    public void GotoPlayAgain()
    {
        ui.GotoPlayAgain();
    }
    public void GotoNextLevel()
    {
        ui.GotoNextLevel();
    }


}
