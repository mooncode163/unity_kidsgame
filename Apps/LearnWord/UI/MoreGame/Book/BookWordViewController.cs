using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookWordViewController : UIViewController
{
    public ItemInfo infoBook;
    UIBookWordList uiPrefab;
    UIBookWordList ui;

    static private BookWordViewController _main = null;
    public static BookWordViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new BookWordViewController();
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
            GameObject obj = PrefabCache.main.LoadByKey("UIBookWordList");
            uiPrefab = obj.GetComponent<UIBookWordList>();
        }
    }

    public void CreateUI()
    {
        if (this.naviController != null)
        {
            this.naviController.HideNavibar(true);
        }
        ui = (UIBookWordList)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }

}
