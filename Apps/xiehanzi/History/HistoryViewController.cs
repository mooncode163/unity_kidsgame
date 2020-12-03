using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoryViewController : PopViewController
{ 
    UIView uiPrefab;
    UIView ui;


    static private HistoryViewController _main = null;
    public static HistoryViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new HistoryViewController();
                _main.Init();
            }
            return _main;
        }
    }

    void Init()
    {

        string strPrefab = "AppCommon/Prefab/History/UIWordWriteHistory"; 
        GameObject obj = PrefabCache.main.Load(strPrefab); 
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
