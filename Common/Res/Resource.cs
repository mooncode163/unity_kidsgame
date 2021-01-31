using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;
using LitJson;
public class Resource
{
    static public string dirResource
    {
        get
        {
            string ret = Application.dataPath + "/Resources";
            return ret;
        }
    }

    static public string dirScript
    {
        get
        {
            string ret = Application.dataPath + "/Script";
            return ret;
        }
    }

    static public string dirProject
    {
        get
        {

            // dataPath:Assets
            // 

            string applicationPath = Application.dataPath;
            if (Application.isEditor)
            {
                applicationPath = Application.dataPath.Replace("/Assets", "");
            }



            //  Debug.Log("Resource Application.dataPath=" + Application.dataPath);
            Debug.Log("Resource dirProject=" + applicationPath);

            if (GameManager.main.isLoadGameScreenShot && !Application.isEditor)
            {
                applicationPath = AppsConfig.ROOT_DIR_PC + "/" + AppsConfig.NAME + "Unity";//kidsgame
            }

            return applicationPath;
        }
    }


    static public string dirProduct
    {
        get
        {
            string str = FileUtil.GetLastDir(dirProject);//kidsgame
            str = FileUtil.GetLastDir(str);//product
            string key = "/product/";
            int idx = str.IndexOf(key);
            if (idx >= 0)
            {
                idx += key.Length - 1;
                str = str.Substring(0, idx);
            }
            if (GameManager.main.isLoadGameScreenShot && !Application.isEditor)
            {
                str = AppsConfig.DIR_PRODUCT_PC_WIN;
                if (Application.platform == RuntimePlatform.WindowsPlayer)
                {
                    str = AppsConfig.DIR_PRODUCT_PC_WIN;
                }
                if (Application.platform == RuntimePlatform.OSXPlayer)
                {
                    str = AppsConfig.DIR_PRODUCT_PC_MAC;
                }
            }
            Debug.Log("Resource dirProduct=" + str);
            return str;
        }
    }

    static public string dirProductCommon
    {
        get
        {
            string str = dirProduct + "/Common";
            return str;
        }
    }

    static public string dirResourceData
    {
        get
        {
            string str = FileUtil.GetLastDir(dirProject) + "/ResourceData";
            Debug.Log("Resource dirResourceData=" + str);
            return str;
        }
    }

    static public string dirResourceDataApp
    {
        get
        {
            string str = dirResourceData + "/" + Common.appType + "/" + Common.appKeyName;
            return str;
        }
    }


    static public string dirResourceDataGameRes
    {
        get
        {
            string str = dirResourceDataApp + "/GameRes";
            if (!Directory.Exists(str))
            {
                str = dirGameResCommon + "/" + Common.appKeyName;
            }
            return str;
        }
    }

    static public string dirGameResCommon
    {
        get
        {
            string str = dirProductCommon + "/GameResCommon";
            return str;
        }
    }

    static public string dirProjectIos
    {
        get
        {
            // string str = FileUtil.GetLastDir(dirResourceData) + "/project_ios";
            string str = dirProduct + "/project_ios";
            return str;
        }
    }
    static public string dirProjectXcode
    {
        get
        {
            string str = dirProduct + "/project_ios/" + "game_device_" + Common.appType + "_" + Common.appKeyName;
            return str;
        }
    }
}
