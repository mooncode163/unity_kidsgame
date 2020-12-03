using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreViewController : PopViewController
{

    UIMoreAppController uiMoreAppPrefab;
    UIMoreAppController uiMoreApp;
    static private MoreViewController _main = null;
    public static MoreViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new MoreViewController();
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
            string strPrefab = "Common/Prefab/MoreApp/UIMoreAppController";
            GameObject obj = PrefabCache.main.Load(strPrefab);
            uiMoreAppPrefab = obj.GetComponent<UIMoreAppController>();
        }
    }
    public void CreateUI()
    {

        uiMoreApp = (UIMoreAppController)GameObject.Instantiate(uiMoreAppPrefab);
        uiMoreApp.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiMoreAppPrefab.gameObject, uiMoreApp.gameObject);
    }

}
