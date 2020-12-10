using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Moonma.IAP;
using Moonma.Share;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIKitEditor : Editor
{

    public const string KEY_MENU_GameObject_UI = "Moonma/UI";
    void Awake()
    {

    }
    // Use this for initialization
    void Start()
    {

    }

       //定义弹出当前窗口的菜单位置  
    //[MenuItem("Window/My Window")]
     [MenuItem(KEY_MENU_GameObject_UI + "/CreateController", false, 4)] 
    static void CreateController()
    {
        //弹出窗口
        EditorWindow.GetWindow(typeof(UIEditorCreateController));
    }

    [MenuItem(KEY_MENU_GameObject_UI + "/UIView", false, 4)]
    static void CreateUIView()
    {
        var selectedObj = Selection.activeObject as GameObject;
        if (selectedObj != null)
        {
            GameObject obj = new GameObject("UIView");
            if (obj != null)
            {
                UIView ui = obj.AddComponent<UIView>();
                ui.transform.SetParent(selectedObj.transform);
                Selection.activeGameObject = ui.gameObject;
                ui.transform.localScale = new Vector3(1f, 1f, 1f);
                RectTransform rctran = obj.AddComponent<RectTransform>();
                rctran.anchoredPosition = Vector2.zero;
            }
        }

    }

    [MenuItem(KEY_MENU_GameObject_UI + "/UIButton", false, 4)]
    static void CreateUIButton()
    {
        var selectedObj = Selection.activeObject as GameObject;
        if (selectedObj != null)
        {
            GameObject obj = PrefabCache.main.Load("Common/Prefab/UIKit/UIButton/UIButton");
            if (obj != null)
            {
                UIButton uiPrefab = obj.GetComponent<UIButton>();
                UIButton ui = (UIButton)GameObject.Instantiate(uiPrefab);
                ui.transform.SetParent(selectedObj.transform);
                Selection.activeGameObject = ui.gameObject;
                ui.transform.localScale = new Vector3(1f, 1f, 1f);
                RectTransform rctran = ui.GetComponent<RectTransform>();
                rctran.anchoredPosition = Vector2.zero;
            }
        }

    }

    [MenuItem(KEY_MENU_GameObject_UI + "/UIImage", false, 4)]
    static void CreateUIImage()
    {
        var selectedObj = Selection.activeObject as GameObject;
        if (selectedObj != null)
        {
            GameObject obj = PrefabCache.main.Load("Common/Prefab/UIKit/UIImage/UIImage");
            if (obj != null)
            {
                UIImage uiPrefab = obj.GetComponent<UIImage>();
                UIImage ui = (UIImage)GameObject.Instantiate(uiPrefab);
                ui.transform.SetParent(selectedObj.transform);
                Selection.activeGameObject = ui.gameObject;
                ui.transform.localScale = new Vector3(1f, 1f, 1f);
                RectTransform rctran = ui.GetComponent<RectTransform>();
                rctran.anchoredPosition = Vector2.zero;
            }
        }

    }

    [MenuItem(KEY_MENU_GameObject_UI + "/UIImageText", false, 4)]
    static void CreateUIImageText()
    {
        var selectedObj = Selection.activeObject as GameObject;
        if (selectedObj != null)
        {
            GameObject obj = PrefabCache.main.Load("Common/Prefab/UIKit/UIImage/UIImageText");
            if (obj != null)
            {
                UIImageText uiPrefab = obj.GetComponent<UIImageText>();
                UIImageText ui = (UIImageText)GameObject.Instantiate(uiPrefab);
                ui.name = "UIImageText";
                ui.transform.SetParent(selectedObj.transform);
                Selection.activeGameObject = ui.gameObject;
                ui.transform.localScale = new Vector3(1f, 1f, 1f);
                RectTransform rctran = ui.GetComponent<RectTransform>();
                rctran.anchoredPosition = Vector2.zero;
            }
        }

    }


    [MenuItem(KEY_MENU_GameObject_UI + "/UIRawImage", false, 4)]
    static void CreateUIRawImage()
    {
        var selectedObj = Selection.activeObject as GameObject;
        if (selectedObj != null)
        {
            GameObject obj = PrefabCache.main.Load("Common/Prefab/UIKit/UIImage/UIRawImage");
            if (obj != null)
            {
                UIRawImage uiPrefab = obj.GetComponent<UIRawImage>();
                UIRawImage ui = (UIRawImage)GameObject.Instantiate(uiPrefab);
                ui.transform.SetParent(selectedObj.transform);
                Selection.activeGameObject = ui.gameObject;
                ui.transform.localScale = new Vector3(1f, 1f, 1f);
                RectTransform rctran = ui.GetComponent<RectTransform>();
                rctran.anchoredPosition = Vector2.zero;
            }
        }

    }


    [MenuItem(KEY_MENU_GameObject_UI + "/UIText", false, 4)]
    static void CreateUIText()
    {
        var selectedObj = Selection.activeObject as GameObject;
        if (selectedObj != null)
        {
            GameObject obj = PrefabCache.main.Load("Common/Prefab/UIKit/UIText/UIText");
            if (obj != null)
            {
                UIText uiPrefab = obj.GetComponent<UIText>();
                UIText ui = (UIText)GameObject.Instantiate(uiPrefab);
                ui.transform.SetParent(selectedObj.transform);
                Selection.activeGameObject = ui.gameObject;
                ui.transform.localScale = new Vector3(1f, 1f, 1f);
                RectTransform rctran = ui.GetComponent<RectTransform>();
                rctran.anchoredPosition = Vector2.zero;
            }
        }

    }

    [MenuItem(KEY_MENU_GameObject_UI + "/UISprite", false, 4)]
    static void CreateUISprite()
    {
        var selectedObj = Selection.activeObject as GameObject;
        if (selectedObj != null)
        {
            GameObject obj = PrefabCache.main.Load(UIKitRes.Prefab_UISprite);
            if (obj != null)
            {
                UISprite uiPrefab = obj.GetComponent<UISprite>();
                UISprite ui = (UISprite)GameObject.Instantiate(uiPrefab);
                ui.transform.SetParent(selectedObj.transform);
                Selection.activeGameObject = ui.gameObject;
                ui.name = "UISprite";
                ui.transform.localScale = new Vector3(1f, 1f, 1f);
                RectTransform rctran = ui.GetComponent<RectTransform>();
                rctran.anchoredPosition = Vector2.zero;
            }
        }

    }

    [MenuItem(KEY_MENU_GameObject_UI + "/UIInputBar", false, 4)]
    static void CreateUIInputBar()
    {
        var selectedObj = Selection.activeObject as GameObject;
        if (selectedObj != null)
        {
            GameObject obj = PrefabCache.main.Load("Common/Prefab/UIKit/UIInputBar/UIInputBar");
            if (obj != null)
            {
                UIInputBar uiPrefab = obj.GetComponent<UIInputBar>();
                UIInputBar ui = (UIInputBar)GameObject.Instantiate(uiPrefab);
                ui.name = "UIInputBar";
                ui.transform.SetParent(selectedObj.transform);
                Selection.activeGameObject = ui.gameObject;
                ui.transform.localScale = new Vector3(1f, 1f, 1f);
                RectTransform rctran = ui.GetComponent<RectTransform>();
                rctran.anchoredPosition = Vector2.zero;
            }
        }

    }

    [MenuItem(KEY_MENU_GameObject_UI + "/UIViewLoading", false, 4)]
    static void CreateUIViewLoading()
    {
        var selectedObj = Selection.activeObject as GameObject;
        if (selectedObj != null)
        {
            GameObject obj = PrefabCache.main.Load("Common/Prefab/UIKit/UIViewLoading/UIViewLoading");
            if (obj != null)
            {
                UIViewLoading uiPrefab = obj.GetComponent<UIViewLoading>();
                UIViewLoading ui = (UIViewLoading)GameObject.Instantiate(uiPrefab);
                ui.name = "UIViewLoading";
                ui.transform.SetParent(selectedObj.transform);
                Selection.activeGameObject = ui.gameObject;
                ui.transform.localScale = new Vector3(1f, 1f, 1f);
                RectTransform rctran = ui.GetComponent<RectTransform>();
                rctran.anchoredPosition = Vector2.zero;
            }
        }

    }

    [MenuItem(KEY_MENU_GameObject_UI + "/UISegment", false, 4)]
    static void CreateUISegment()
    {
        var selectedObj = Selection.activeObject as GameObject;
        if (selectedObj != null)
        {
            GameObject obj = PrefabCache.main.Load("Common/Prefab/UIKit/UISegment/UISegment");
            if (obj != null)
            {
                UISegment uiPrefab = obj.GetComponent<UISegment>();
                UISegment ui = (UISegment)GameObject.Instantiate(uiPrefab);
                ui.name = "UISegment";
                ui.transform.SetParent(selectedObj.transform);
                Selection.activeGameObject = ui.gameObject;
                ui.transform.localScale = new Vector3(1f, 1f, 1f);
                RectTransform rctran = ui.GetComponent<RectTransform>();
                rctran.anchoredPosition = Vector2.zero;
            }
        }

    }

    [MenuItem(KEY_MENU_GameObject_UI + "/UITextView", false, 4)]
    static void CreateUITextView()
    {
        var selectedObj = Selection.activeObject as GameObject;
        if (selectedObj != null)
        {
            GameObject obj = PrefabCache.main.Load("Common/Prefab/UIKit/UITextView/UITextView");
            if (obj != null)
            {
                UITextView uiPrefab = obj.GetComponent<UITextView>();
                UITextView ui = (UITextView)GameObject.Instantiate(uiPrefab);
                ui.name = "UITextView";
                ui.transform.SetParent(selectedObj.transform);
                Selection.activeGameObject = ui.gameObject;
                ui.transform.localScale = new Vector3(1f, 1f, 1f);
                RectTransform rctran = ui.GetComponent<RectTransform>();
                rctran.anchoredPosition = Vector2.zero;
            }
        }

    }
    [MenuItem(KEY_MENU_GameObject_UI + "/UIProgress", false, 4)]
    static void CreateUIProgress()
    {
        var selectedObj = Selection.activeObject as GameObject;
        if (selectedObj != null)
        {
            GameObject obj = PrefabCache.main.Load("Common/Prefab/UIKit/UIProgress/UIProgress");
            if (obj != null)
            {
                UIProgress uiPrefab = obj.GetComponent<UIProgress>();
                UIProgress ui = (UIProgress)GameObject.Instantiate(uiPrefab);
                ui.name = "UIProgress";
                ui.transform.SetParent(selectedObj.transform);
                Selection.activeGameObject = ui.gameObject;
                ui.transform.localScale = new Vector3(1f, 1f, 1f);
                RectTransform rctran = ui.GetComponent<RectTransform>();
                rctran.anchoredPosition = Vector2.zero;
            }
        }

    }


}
