using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemListViewController : PopViewController
{

    UIItemList uiPrefab;
    public UIItemList ui;
    public List<object> litItem;

    static private ItemListViewController _main = null;
    public static ItemListViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new ItemListViewController();
                _main.Init();
            }
            return _main;
        }
    }
    void Init()
    {
        GameObject obj = PrefabCache.main.Load("App/Prefab/Item/UIItemList");
        uiPrefab = obj.GetComponent<UIItemList>();
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
            this.naviController.HideNavibar(false);
        }
        ui = (UIItemList)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        ui.SetList(litItem);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);



    }
}
