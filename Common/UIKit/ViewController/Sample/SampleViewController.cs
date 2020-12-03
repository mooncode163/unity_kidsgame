using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleViewController : UIViewController
{

    UISampleController uiPrefab;
    UISampleController ui;
    static private SampleViewController _main = null;
    public static SampleViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new SampleViewController();
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
            GameObject obj = PrefabCache.main.LoadByKey("UISampleController");
            uiPrefab = obj.GetComponent<UISampleController>();
        }
    }

    public void CreateUI()
    {
        if (this.naviController != null)
        {
            this.naviController.HideNavibar(true);
        }
        ui = (UISampleController)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }
  

}
