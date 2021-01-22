using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WriteFinishViewController : PopViewController
{
    public int indexPlace = 0;
    UIView uiPrefab;
    public UIWriteFinish ui;


    static private WriteFinishViewController _main = null;
    public static WriteFinishViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new WriteFinishViewController();
                _main.Init();
            }
            return _main;
        }
    }

    void Init()
    {

        {
            GameObject obj = PrefabCache.main.LoadByKey("UIWriteFinish");
            uiPrefab = obj.GetComponent<UIWriteFinish>();
        }

        // string strPrefab = "AppCommon/Prefab/Game/UIWriteFinish";
        // GameObject obj = PrefabCache.main.Load(strPrefab); 
        // uiPrefab = obj.GetComponent<UIView>();
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        CreateUI();
    }

    public void CreateUI()
    {
        ui = (UIWriteFinish)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }
}
