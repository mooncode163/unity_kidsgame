using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceViewController : UIViewController
{

    UIPlaceBase uiPrefab;
    UIPlaceBase ui;


    static private PlaceViewController _main = null;
    public static PlaceViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new PlaceViewController();
                _main.Init();
            }
            return _main;
        }
    }

    void Init()
    {

        string strPrefab = "AppCommon/Prefab/Place/" + GetPrefabName();
        string strPrefabDefault = "Common/Prefab/Place/UIPlaceController";
        GameObject obj = PrefabCache.main.Load(strPrefab);
        if (obj == null)
        {
            obj = PrefabCache.main.Load(strPrefabDefault);
        }
        if (obj != null)
        {
            uiPrefab = obj.GetComponent<UIPlaceBase>();
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
        Debug.Log("PlaceViewController LayOutView");
        if ((uiPrefab != null) && (ui != null))
        {
            UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
            ui.LayOut();
        }

    }
    string GetPrefabName()
    {
        //Resources.Load 文件可以不区分大小写字母
        name = "UIPlace" + Common.appType;
        return name;
    }
    public void CreateUI()
    {
        ui = (UIPlaceBase)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }
    public void PreLoadDataForWeb()
    {
        UIPlaceBase uiplace = ui;
        if (uiplace == null)
        {
            uiplace = uiPrefab;
        }
        if (uiplace != null)
        {
            uiplace.PreLoadDataForWeb();
        }
    }
}
