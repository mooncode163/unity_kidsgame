using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfoViewController : PopViewController
{

    UIGameInfo uiControllerPrefab;
    UIGameInfo uiController;


    static private GameInfoViewController _main = null;
    public static GameInfoViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new GameInfoViewController();
                _main.Init();
            }
            return _main;
        }
    }

    void Init()
    {
        GameObject obj = PrefabCache.main.Load("App/Prefab/Game/UIGameInfo");
        uiControllerPrefab = obj.GetComponent<UIGameInfo>();
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        CreateUI();
    }
    public void CreateUI()
    {
        uiController = (UIGameInfo)GameObject.Instantiate(uiControllerPrefab);
        uiController.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiControllerPrefab.gameObject, uiController.gameObject);
    }
}
