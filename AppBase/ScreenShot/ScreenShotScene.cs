using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.UI;
public class ScreenShotScene : AppSceneBase
{  
    public override void RunApp()
    {
        base.RunApp();
        ScreenShotViewController controller = ScreenShotViewController.main;
        SetRootViewController(controller);
    }


}
