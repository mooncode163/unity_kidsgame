using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentGateViewController : PopViewController
{
    UIView uiPrefab;
    public UIParentGate ui;

    public int index;
    static private ParentGateViewController _main = null;
    public static ParentGateViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new ParentGateViewController();
                _main.Init();
            }
            return _main;
        }
    }

    void Init()
    {

        string strPrefab = "Common/Prefab/ParentGate/UIParentGate";
        GameObject obj = PrefabCache.main.Load(strPrefab);
        uiPrefab = obj.GetComponent<UIView>();
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        CreateUI();
    }

    public void CreateUI()
    {
        ui = (UIParentGate)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }


}
