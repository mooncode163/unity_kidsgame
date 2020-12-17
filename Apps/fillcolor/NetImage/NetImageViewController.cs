using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetImageViewController : NaviViewController
{

    UINetImage uiPrefab;
    UINetImage ui;//GuankaItemCell GameObject 

    static private NetImageViewController _main = null;
    public static NetImageViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new NetImageViewController();
                _main.Init();
            }
            return _main;
        }
    }


    void Init()
    {

        string strPrefab = "AppCommon/Prefab/NetImage/UINetImage";
        GameObject obj = PrefabCache.main.Load(strPrefab);
        if (obj != null)
        {
            uiPrefab = obj.GetComponent<UINetImage>();
        }

    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        CreateUI();
    }
    public override void LayOutView()
    {
        base.LayOutView();
        Debug.Log("HomeViewController LayOutView ");
        if (ui != null)
        {
            ui.LayOut();
        }
    }
    public void CreateUI()
    {
        ui = (UINetImage)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }

  

}
