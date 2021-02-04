using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearnViewController : PopViewController
{

    UILearnProgress uiPrefab;
    UILearnProgress ui;

    static private LearnViewController _main = null;
    public static LearnViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new LearnViewController();
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
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/Learn/UILearnProgress");
            uiPrefab = obj.GetComponent<UILearnProgress>();
        }
    }

    public void CreateUI()
    {
        ui = (UILearnProgress)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }

}
