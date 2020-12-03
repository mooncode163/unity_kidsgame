using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageViewController : PopViewController
{

    UILanguage uiPrefab;
    public UILanguage ui;

    static private LanguageViewController _main = null;
    public static LanguageViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new LanguageViewController();
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
            string strPrefab = "Common/Prefab/Setting/UILanguage";
            GameObject obj = PrefabCache.main.Load(strPrefab);
            uiPrefab = obj.GetComponent<UILanguage>();
        }
    }

    public void CreateUI()
    {
        ui = (UILanguage)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }

}
