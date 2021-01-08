using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistorySaveImageViewController : PopViewController
{ 
    UIView uiPrefab;
    public UIView ui;


    static private HistorySaveImageViewController _main = null;
    public static HistorySaveImageViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new HistorySaveImageViewController();
                _main.Init();
            }
            return _main;
        }
    }

    void Init()
    {
 
        GameObject obj = PrefabCache.main.LoadByKey("UIHistorySaveImage"); 
        uiPrefab = obj.GetComponent<UIView>();
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        CreateUI();
    }
 
    public void CreateUI()
    {
        ui = (UIView)GameObject.Instantiate(uiPrefab); 
        ui.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }
}
