using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WriteViewController : UIViewController
{
    UIWriteController uiPrefab;
    UIWriteController ui;

    public UIWordWrite.Mode mode;
    static private WriteViewController _main = null;
    public static WriteViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new WriteViewController();
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
            GameObject obj = PrefabCache.main.LoadByKey("UIWriteController");
            uiPrefab = obj.GetComponent<UIWriteController>();
        }
    }

    public void CreateUI()
    {
        if (this.naviController != null)
        {
            this.naviController.HideNavibar(true);
        }
        ui = (UIWriteController)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }

        public void OnMode(UIWordWrite.Mode md)
    {
        if(ui!=null)
        {
            ui.OnMode(md);
        }
    }

}
