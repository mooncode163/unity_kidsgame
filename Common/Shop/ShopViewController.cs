using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopViewController : PopViewController
{

    UIShop uiPrefab;
    UIShop ui;


    static private ShopViewController _main = null;
    public static ShopViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new ShopViewController();
                _main.Init();
            }
            return _main;
        }
    }

    void Init()
    {  
        string strPrefab = "Common/Prefab/Shop/UIShop";
        GameObject obj = PrefabCache.main.Load(strPrefab);
       

        uiPrefab = obj.GetComponent<UIShop>();
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        CreateUI();
    } 
    public void CreateUI()
    {
        ui = (UIShop)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }
}
