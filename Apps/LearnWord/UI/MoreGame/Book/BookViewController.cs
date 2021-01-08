using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookViewController : UIViewController
{

    UIBookController uiPrefab;
    UIBookController ui;

    static private BookViewController _main = null;
    public static BookViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new BookViewController();
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
            GameObject obj = PrefabCache.main.LoadByKey("UIBookController");
            uiPrefab = obj.GetComponent<UIBookController>();
        }
    }

    public void CreateUI()
    {
        if (this.naviController != null)
        {
            this.naviController.HideNavibar(true);
        }
        ui = (UIBookController)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }

}
