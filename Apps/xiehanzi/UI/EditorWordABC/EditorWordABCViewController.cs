using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorWordABCViewController : UIViewController
{

    UIEditorWordABCController uiPrefab;
    UIEditorWordABCController ui;
    static private EditorWordABCViewController _main = null;
    public static EditorWordABCViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new EditorWordABCViewController();
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
            GameObject obj = PrefabCache.main.LoadByKey("UIEditorWordABC");
            uiPrefab = obj.GetComponent<UIEditorWordABCController>();
        }
    }

    public void CreateUI()
    {
        if (this.naviController != null)
        {
            this.naviController.HideNavibar(true);
        }
        ui = (UIEditorWordABCController)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }
  

}
