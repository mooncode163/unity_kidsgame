using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WriteFinishViewController : PopViewController
{
    UIWriteFinish uiPrefab;
    UIWriteFinish ui;
    static private WriteFinishViewController _main = null;
    public static WriteFinishViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new WriteFinishViewController();
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
            GameObject obj = PrefabCache.main.LoadByKey("UIWriteFinish");
            uiPrefab = obj.GetComponent<UIWriteFinish>();
        }
    }

    public void CreateUI()
    {
        if (this.naviController != null)
        {
            this.naviController.HideNavibar(true);
        }
        ui = (UIWriteFinish)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }

}
