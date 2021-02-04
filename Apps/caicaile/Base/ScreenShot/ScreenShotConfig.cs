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
            switch (idx)
            {
                case 0:
                    int total = LevelManager.main.placeTotal;
                    if (Common.appKeyName == GameRes.GAME_IdiomFlower)
                    {
                        total = 2;
                    }
                    if (total > 1)
                    {
                        controller = PlaceViewController.main;
                    }
                    else
                    {
                        LevelManager.main.gameLevel = 0;
                        controller = GameViewController.main;
                    }

                    //    controller = HomeViewController.main;

                    break;
                case 1:
                    LevelManager.main.gameLevel = 1;
                    controller = GameViewController.main;
                    break;
                case 2:
                    controller = GuankaViewController.main;
                    break;
                case 3:
                    controller = HomeViewController.main;
                    break;
                case 4:

                    if (Common.appKeyName == GameRes.GAME_IdiomFlower)
                    {
                        controller = HowPlayFlowerViewController.main;

                    }
                    else
                    {
                        LevelManager.main.gameLevel = 2;
                        controller = GameViewController.main;

                    }

                    break;

                default:
                    controller = HomeViewController.main;
                    break;


            }
        }
        info.controller = controller;

        return info;
    }

}
