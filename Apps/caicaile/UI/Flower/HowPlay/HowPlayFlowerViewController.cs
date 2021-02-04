using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowPlayFlowerViewController : PopViewController
{

    UIHowPlayFlower uiControllerPrefab;
    UIHowPlayFlower uiController;


    static private HowPlayFlowerViewController _main = null;
    public static HowPlayFlowerViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new HowPlayFlowerViewController(); 
                _main.Init();
            }
            return _main;
        }
    }

    void Init()
    { 
        GameObject obj = PrefabCache.main.Load("App/Prefab/HowPlay/UIHowPlayFlower");
        uiControllerPrefab = obj.GetComponent<UIHowPlayFlower>();
    }

    public override void ViewDidLoad()
    { 
        base.ViewDidLoad();
        CreateUI();
    } 
    public void CreateUI()
    {
        uiController = (UIHowPlayFlower)GameObject.Instantiate(uiControllerPrefab);
        uiController.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiControllerPrefab.gameObject, uiController.gameObject);
    }
}
