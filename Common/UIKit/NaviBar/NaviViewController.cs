using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaviViewController : UIViewController
{
    public GameObject objContent;
    UINaviBar uiNaviBarPrefab;
    UINaviBar uiNaviBar;
    public string source;

    List<UIViewController> listController;
    UIViewController rootController;

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        Debug.Log("NaviViewController:ViewDidLoad");
        CreateContent();
        CreateBar();
        UpdateController();
    }
    public override void LayOutView()
    {
        base.LayOutView();
        if (objContent != null)
        {
            RectTransform rctran = objContent.GetComponent<RectTransform>();


            RectTransform rctranParent = objContent.transform.parent.GetComponent<RectTransform>();
            rctran.sizeDelta = rctranParent.sizeDelta;

            //必须在sizeDelta后面设置
            rctran.anchorMin = new Vector2(0, 0);
            rctran.anchorMax = new Vector2(1, 1);
            rctran.offsetMin = new Vector2(0, 0);
            rctran.offsetMax = new Vector2(0, 0);

        }

        if (rootController != null)
        {
            rootController.LayOutView();
        }
    }
    public override void UpdateLanguage()
    {
        base.UpdateLanguage();
        if (uiNaviBar != null)
        {
            uiNaviBar.UpdateLanguage();
        }
    }
    public void CreateContent()
    {
        string classname = "Content";
        objContent = new GameObject(classname);
        RectTransform rctran = objContent.AddComponent<RectTransform>();
        objContent.transform.parent = objController.transform;
        // RectTransform rctranParent = objController.GetComponent<RectTransform>();
        // rctran.sizeDelta = rctranParent.sizeDelta;

        rctran.anchorMin = new Vector2(0, 0);
        rctran.anchorMax = new Vector2(1, 1);

        rctran.offsetMin = new Vector2(0, 0);
        rctran.offsetMax = new Vector2(0, 0);
    }
    public void CreateBar()
    {
        string strPrefabCommon = "Common/Prefab/NaviBar/UINaviBar";
        string strPrefabApp = "App/Prefab/NaviBar/UINaviBar";
        GameObject obj = (GameObject)PrefabCache.main.Load(strPrefabApp);
        if (obj == null)
        {
            obj = (GameObject)PrefabCache.main.Load(strPrefabCommon);
        }
        uiNaviBarPrefab = obj.GetComponent<UINaviBar>();

        //Debug.Log("rctranPrefab.offsetMin=" + rctranPrefab.offsetMin + " rctranPrefab.offsetMax=" + rctranPrefab.offsetMax);
        uiNaviBar = (UINaviBar)GameObject.Instantiate(uiNaviBarPrefab);
        uiNaviBar.transform.parent = objController.transform;
        uiNaviBar.callbackBackClick = OnUINaviBarClickBack;
        UIViewController.ClonePrefabRectTransform(uiNaviBarPrefab.gameObject, uiNaviBar.gameObject);
        //Debug.Log("rctran.offsetMin=" + rctran.offsetMin + " rctran.offsetMax=" + rctran.offsetMax);

    }

    void UpdateController()
    {
        if (listController == null)
        {
            listController = new List<UIViewController>();
        }
        if (listController.Count == 0)
        {
            return;
        }
        DestroyController();

        UIViewController controller = listController[listController.Count - 1];
        if (objContent != null)
        {
            controller.SetViewParent(objContent);
            rootController = controller;
            //controller.LayOutView();
        }
        if (uiNaviBar != null)
        {
            uiNaviBar.HideBtnBack((listController.Count < 2) ? true : false);
            uiNaviBar.UpdateTitle(controller.title);
        }

    }
    void DestroyController()
    {
        if (rootController != null)
        {
            rootController.DestroyObjController();
            rootController = null;
        }
    }

    public void Push(UIViewController controller)
    {
        if (listController == null)
        {
            listController = new List<UIViewController>();
        }
        if (controller == null)
        {
            return;
        }
        listController.Add(controller);
        controller.type = UIViewController.Type.NAVIBAR;
        controller.naviController = this;
        UpdateController();

    }

    // 替换最后一个
    public void Replace(UIViewController controller)
    {
        if (listController == null)
        {
            listController = new List<UIViewController>();
        }
        if (controller == null)
        {
            return;
        }
        if (listController.Count >= 1)
        {
            listController.RemoveAt(listController.Count - 1);
        }
        listController.Add(controller);
        controller.type = UIViewController.Type.NAVIBAR;
        controller.naviController = this;
        UpdateController();

    }

    public void Pop()
    {
        if (listController.Count == 0)
        {
            return;
        }

        DestroyController();
        listController.RemoveAt(listController.Count - 1);

        UpdateController();
    }
    public void HideNavibar(bool isHide)
    {
        if (uiNaviBar)
        {
            uiNaviBar.gameObject.SetActive(!isHide);
        }
    }
    public void ShowTitle(bool isShow)
    {
        if (uiNaviBar)
        {
            uiNaviBar.ShowTitle(isShow);
        }
    }
    public void UpdateTitle(string title)
    {
        if (uiNaviBar)
        {
            uiNaviBar.UpdateTitle(title);
        }
    }
    public void OnUINaviBarClickBack(UINaviBar bar)
    {
        Pop();
    }
}
