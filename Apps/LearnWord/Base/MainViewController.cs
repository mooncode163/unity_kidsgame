using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainViewController : TabBarViewController //TabBarViewController NaviViewController
{
    static private MainViewController _main = null;
    public static MainViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new MainViewController();
                _main.Init();
            }
            return _main;
        }
    }

    void Init()
    {
        this.title = "Main";
        // this.Push(HomeViewController.main);
    }

    public override void ViewDidLoad()
    {
        //必须先调用基类方法以便初始化
        base.ViewDidLoad();
        string color = "TabBarItem";
        string colorSel = "TabBarItemSel";
        {
            TabBarItemInfo info = new TabBarItemInfo();
            info.keyTitle = "STR_HOME";
            info.keyColor = color;
            info.keyColorSel = colorSel;
            NaviViewController navi = new NaviViewController();
            navi.ShowTitle(false);
            navi.Push(WordListViewController.main);
            info.controller = navi;
            AddItem(info);
        }


        {
            TabBarItemInfo info = new TabBarItemInfo();
            info.keyTitle = "STR_WRITE"; info.keyColor = color;
            info.keyColorSel = colorSel;
            NaviViewController navi = new NaviViewController();
            navi.ShowTitle(false);
            WriteViewController.main.mode = UIWordWrite.Mode.ALL_STROKE;
            navi.Push(WriteViewController.main);
            info.controller = navi;
            AddItem(info);
        }

        {
            TabBarItemInfo info = new TabBarItemInfo();
            info.keyTitle = "STR_LOVE"; info.keyColor = color;
            info.keyColorSel = colorSel;
            NaviViewController navi = new NaviViewController();
            navi.ShowTitle(false);
            navi.Push(LoveViewController.main);
            info.controller = navi;
            AddItem(info);
        }

        {
            TabBarItemInfo info = new TabBarItemInfo();
            info.keyTitle = "STR_MOREGAME"; info.keyColor = color;
            info.keyColorSel = colorSel;
            NaviViewController navi = new NaviViewController();
            navi.ShowTitle(false);
            navi.Push(MoreGameViewController.main);
            info.controller = navi;
            AddItem(info);
        }
        {
            TabBarItemInfo info = new TabBarItemInfo();
            info.keyTitle = "STR_SETTING"; info.keyColor = color;
            info.keyColorSel = colorSel;
            NaviViewController navi = new NaviViewController();
            navi.ShowTitle(false);
            navi.Push(SettingViewController.main);
            info.controller = navi;
            AddItem(info);
        }
        this.ShowImageBg(false);
        this.SelectItem(0);
        Debug.Log("MainViewController ViewDidLoad");
    }
    public override void ViewDidUnLoad()
    {
        //必须先调用基类方法
        base.ViewDidUnLoad();
    }

}
