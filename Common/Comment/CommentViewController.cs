
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommentViewController : PopViewController
{
    public UIComment uiPrefab;
    public UIComment ui;
    static private CommentViewController _main = null;
    public static CommentViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new CommentViewController();
                _main.Init();
            }
            return _main;
        }
    }

    public OnUICommentDidClickDelegate callBackClick { get; set; }

    void Init()
    {
        string strPrefab = "Common/Prefab/Comment/UIComment";
        GameObject obj = PrefabCache.main.Load(strPrefab);
        uiPrefab = obj.GetComponent<UIComment>();
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        CreateUI();
    }
    public override void LayOutView()
    {
        base.LayOutView();

    }
    public void CreateUI()
    {
        ui = (UIComment)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
        ui.callBackClick = OnUICommentDidClick;
    }



    public void OnUICommentDidClick(ItemInfo item)
    {
        if (callBackClick != null)
        {
            callBackClick(item);
        }
    }
}
