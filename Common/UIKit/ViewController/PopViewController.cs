using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPopViewControllerDelegate
{
    void OnPopViewControllerDidClose(PopViewController controller);

}


public class PopViewController : UIViewController
{
    IPopViewControllerDelegate iDelegate;
    public void Show(UIViewController controller,IPopViewControllerDelegate dele)
    {
        iDelegate = dele;
        UIViewController root = controller;
        if (root == null)
        {
            root = AppSceneBase.main.rootViewController;
        }
        SetViewParent(root.objController);
    }
    public void Close()
    {
        if (iDelegate != null)
        {
            iDelegate.OnPopViewControllerDidClose(this);
        }
        DestroyObjController();
    }
}
