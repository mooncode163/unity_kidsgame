using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabBarItemInfo
{
    public UIViewController controller;
    public string title;
    public string keyTitle;
    public string pic;
    public string keyColor;
    public string keyColorSel;//选中
}

public class TabBarViewController : UIViewController
{

    public GameObject objContent;
    UITabBar uiTabBarPrefab;
    UITabBar uiTabBar;
    List<TabBarItemInfo> listItem;
    int selectIndex = -1;
    UIViewController rootController;

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        CreateTabBar();
    }
    public override void LayOutView()
    {
        base.LayOutView();
        if (objContent != null)
        {
            // RectTransform rctranParent = objContent.transform.parent.GetComponent<RectTransform>();
            // if (objContent != null)
            // {
            //     RectTransform rctran = objContent.GetComponent<RectTransform>();
            //     rctran.sizeDelta = rctranParent.sizeDelta;
            // }

        }

        if (rootController != null)
        {
            rootController.LayOutView();
        }
    }

    public override void UpdateLanguage()
    {
        base.UpdateLanguage();
        if (uiTabBar != null)
        {
            uiTabBar.UpdateLanguage();
        }
    }
    public void CreateContent()
    {
        string classname = "Content";
        objContent = new GameObject(classname);
        RectTransform rctran = objContent.AddComponent<RectTransform>();
        objContent.transform.parent = objController.transform;
        // rctran.sizeDelta = sizeCanvas;

        rctran.anchorMin = new Vector2(0, 0);
        rctran.anchorMax = new Vector2(1, 1);

        rctran.offsetMin = new Vector2(0, 0);
        rctran.offsetMax = new Vector2(0, 0);
    }
    public void CreateTabBar()
    {
        CreateContent();

        string strPrefab = "Common/Prefab/TabBar/UITabBar";
        GameObject obj = PrefabCache.main.Load(strPrefab);
        uiTabBarPrefab = obj.GetComponent<UITabBar>();

        uiTabBar = (UITabBar)GameObject.Instantiate(uiTabBarPrefab);
        uiTabBar.transform.parent = objController.transform;
        uiTabBar.callbackClick = OnUITabBarClick;
        UIViewController.ClonePrefabRectTransform(uiTabBarPrefab.gameObject, uiTabBar.gameObject);
    }
    // Use this for initialization
    public void AddItem(TabBarItemInfo info)
    {
        if (listItem == null)
        {
            listItem = new List<TabBarItemInfo>();
        }
        listItem.Add(info);
        uiTabBar.AddItem(info, listItem.Count - 1);
    }

    public TabBarItemInfo GetItem(int idx)
    {
        if (listItem == null)
        {
            return null;
        }
        if ((idx < 0) || (idx >= listItem.Count))
        {
            return null;
        }

        TabBarItemInfo info = listItem[idx];
        return info;
    }
    public void ShowImageBg(bool isShow)
    {
        uiTabBar.imageBg.gameObject.SetActive(isShow);
    }
    public void DestroyController()
    {
        if (objController == null)
        {
            return;
        }

        TabBarItemInfo info = GetItem(selectIndex);
        if (info == null)
        {
            Debug.Log("DestroyController null,selectIndex=" + selectIndex);
            return;
        }

        info.controller.DestroyObjController();

        // foreach (Transform child in objView.transform)
        // {
        //     GameObject objtmp = child.gameObject;
        //     GameObject.DestroyImmediate(objtmp);
        //     objtmp = null;
        // }
    }

    public void SelectItem(int idx)
    {
        if (selectIndex == idx)
        {
            Debug.Log("tabbar click the same item selectIndex=" + idx);
            return;
        }
        TabBarItemInfo info = GetItem(idx);
        if (info == null)
        {
            Debug.Log("SelectItem null,idx=" + idx);
            return;
        }
        for (int i = 0; i < listItem.Count; i++)
        {
            uiTabBar.SelectItem(i, (i == idx) ? true : false);
        }

        DestroyController();

        selectIndex = idx;
        //info.controller.CreateView(sizeCanvas);
        info.controller.SetViewParent(objContent);
        rootController = info.controller;
    }

    public void OnUITabBarClick(UITabBar bar, UITabBarItem item)
    {
        SelectItem(item.index);
    }
    public float GetBarHeight()
    {
        float h = 0;
        if (uiTabBar != null)
        {
            h = uiTabBar.GetBarHeight();
        }
        return h;
    }
}
