using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetImageListViewController : NaviViewController
{

    UINetImageList uiPrefab;
    UINetImageList ui;//GuankaItemCell GameObject 
    public int index; 
    static private NetImageListViewController _main = null;
    public static NetImageListViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new NetImageListViewController();
                _main.Init();
            }
            return _main;
        }
    }


    void Init()
    {

        string strPrefab = "AppCommon/Prefab/NetImage/UINetImageList";
        GameObject obj = PrefabCache.main.Load(strPrefab);
        if (obj != null)
        {
            uiPrefab = obj.GetComponent<UINetImageList>();
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
        ui = (UINetImageList)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        ui.index = this.index;
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }

    public void StartParseImageList(ImageItemInfo info)
    {
        if (ui != null)
        {
            ui.StartParseImageList(info);
        }
    }
}

