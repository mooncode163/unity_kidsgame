using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchViewController : PopViewController
{

    UISearch uiPrefab;
    UISearch ui;
    public List<object> litItem;

    static private SearchViewController _main = null;
    public static SearchViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new SearchViewController();
                _main.Init();
            }
            return _main;
        }
    }
    void Init()
    {
        GameObject obj = PrefabCache.main.Load("App/Prefab/Search/UISearch");
        uiPrefab = obj.GetComponent<UISearch>();
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        CreateUI();
    }
    public void CreateUI()
    {
        if (this.naviController != null)
        {
            this.naviController.HideNavibar(true);
        }
        ui = (UISearch)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        ui.uiItemList.SetList(litItem);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }
}
