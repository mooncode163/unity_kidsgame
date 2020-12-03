using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppsConfig
{

    static public string ROOT_DIR_PC
    {
        get
        {
            string ret = DIR_PRODUCT_PC_MAC;


            ret = Application.dataPath;//assets
            ret = FileUtil.GetLastDir(ret);
            ret = FileUtil.GetLastDir(ret);
            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                ret = DIR_PRODUCT_PC_WIN + "/" + NAME;
            }
            if (Application.platform == RuntimePlatform.OSXPlayer)
            {
                ret = DIR_PRODUCT_PC_MAC + "/" + NAME;
            }

            // Debug.Log("ROOT_DIR_PC=" + ret);
            return ret;
        }
    }
    public const string NAME = "kidsgame";
    public const string DIR_PRODUCT_PC_MAC = "/Users/moon/sourcecode/unity/product";
    public const string DIR_PRODUCT_PC_WIN = "F:/sourcecode/unity/product";
}
