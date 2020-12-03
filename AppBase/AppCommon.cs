using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class AppCommon
{
    //string
    public const string STR_LANGUAGE_REPLACE = "xxx";

    //prefab
    public const string PREFAB_UICELLBASE = "Common/Prefab/TableView/UICellBase";
    public const string PREFAB_UISettingCellItem = "Common/Prefab/Setting/UISettingCellItem";
    public const string PREFAB_UILanguageCellItem = "Common/Prefab/Setting/UILanguageCellItem";
    public const string PREFAB_GUANKA_CELL_ITEM_COMMON = "Common/Prefab/Guanka/UIGuankaCellItem";
    public const string PREFAB_GUANKA_CELL_ITEM_APP = "App/Prefab/Guanka/UIGuankaCellItem";
    public const string PREFAB_GUANKA_CELL_ITEM_APPCommon = "AppCommon/Prefab/Guanka/UIGuankaCellItem";
    public const string PREFAB_MOREAPP_CELL_ITEM = "Common/Prefab/MoreApp/UIMoreAppCellItem";

    public const string PREFAB_PLACE_CELL_ITEM_COMMON = "Common/Prefab/Place/UIPlaceCellItem";
    public const string PREFAB_PLACE_CELL_ITEM_APP = "AppCommon/Prefab/Place/UIPlaceCellItem";

    public const string PREFAB_SHOP_CELL_ITEM = "Common/Prefab/Shop/UIShopCellItem";
    public const string PREFAB_SETTING = "Common/Prefab/Setting/UISettingController";


    //prefab uikit
    public const string PREFAB_UIImageText = "Common/Prefab/UIKit/UIImageText";

    //image
    static public float scaleBase
    {
        get
        {
            float ret = 1;
            if (Screen.width > Screen.height)
            {
                ret = Screen.width / 2048f;//基于2048x1536
            }
            else
            {
                ret = Screen.width / 1536f;
            }
            return ret;
        }
    }

    static public string urlAdConfig
    {
        get
        {
            //http://www.mooncore.cn/moonma/adconfig/kidsgame/pintu/animal/ad_config_ios.json

            string filename = "ad_config_ios.json";
            if (AppVersion.appForPad)
            {
                filename = "ad_config_ios_hd.json";
            }
            if (Common.isAndroid)
            {
                filename = "ad_config_android.json";
                if (AppVersion.appForPad)
                {
                    filename = "ad_config_android_hd.json";
                }
            }
            if (Common.isWinUWP)
            {
                filename = "ad_config_win.json";
                if (AppVersion.appForPad)
                {
                    filename = "ad_config_win_hd.json";
                }
            }

            string url = "http://www.mooncore.cn/moonma/kidsgame/" + Common.appType + "/" + Common.appKeyName + "/adconfig/" + filename;

            return url;
        }
    }

    //

    static public int USER_COMMENT_DAY_STEP = 2;//每隔几天弹一次评论

    public const int NO_APPCENTER_DAYS = 1;
    public const int NO_APPVERSION_UPDATE_DAYS = 3;

    public const string NAME_SCENE_PLACE = "PlaceScene";
    public const string NAME_SCENE_GUANKA = "GuankaScene";
    public const string NAME_SCENE_MAIN = "MainScene";
    public const string NAME_SCENE_GAME = "GameScene";
    public const string NAME_SCENE_SETTING = "SettingScene";
    public const string NAME_SCENE_MOREAPP = "MoreAppScene";

    public const string NAME_SCENE_APPSCENE = "AppScene";

    public const string NAME_SCENE_SCREENSHOT = "ScreenShotScene";
    public const string NAME_SCENE_INTRODUCE = "IntroduceScene";

    public const float HEIGHT_AD_BANNER_CANVAS = 128;

}
