using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotConfig
{

    public ShotDeviceInfo deviceInfo;
    public int GetTotalPage()
    {
        return 5;
    }
    public ShotItemInfo GetPage(ShotDeviceInfo dev, int idx)
    {
        ShotItemInfo info = new ShotItemInfo();
        UIViewController controller = null;
        info.isRealGameUI = true;
        if (dev.name == ScreenDeviceInfo.DEVICE_NAME_ICON)
        {
            controller = IconViewController.main;
            IconViewController.main.deviceInfo = dev;
        }
        else if (dev.name == ScreenDeviceInfo.DEVICE_NAME_AD)
        {
            controller = AdHomeViewController.main;
        }
        else if (dev.name == ScreenDeviceInfo.DEVICE_NAME_COPY_RIGHT_HUAWEI)
        {
            controller = CopyRightViewController.main;
            CopyRightViewController.main.deviceInfo = dev;
        }
        else
        {
            LevelManager.main.gameLevel = idx;
            switch (idx)
            {
                case 0:
                    controller = WordListViewController.main;
                    break;
                case 1:
                    controller = WriteViewController.main;
                    break;
                case 2:
                    LevelManager.main.gameLevel = 5;
                    controller = DetailViewController.main;
                    break;
                case 3:
                    controller = MoreGameViewController.main;
                    break;
                case 4:
                    controller = MatchViewController.main;
                    break;

                default:
                    controller = WordListViewController.main;
                    break;


            }
        }
        info.controller = controller;

        return info;
    }

}
