
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class ImageRes
{
    //color
    //f88816 248,136,22
    JsonData rootJson;
    public bool isOldVersion;

    ImageResInternal imageResApp;
    ImageResInternal imageResAppCommon;

    //old
    ImageResInternal imageResOld;
    ImageResInternal imageResCommon;


    ImageResInternal imageResDefault;
    //old

    static private ImageRes _main = null;
    public static ImageRes main
    {
        get
        {
            if (_main == null)
            {
                _main = new ImageRes();
                _main.Init();
            }
            return _main;
        }
    }

    public void Init()
    {

        string filepath = Common.RES_CONFIG_DATA + "/Image/ImageRes.json";
        if (FileUtil.FileIsExist(filepath))
        {
            imageResOld = new ImageResInternal();
            imageResOld.isOldVersion = true;
            imageResOld.Init(filepath);
        }

        imageResApp = new ImageResInternal();
        filepath = Common.RES_CONFIG_DATA + "/Image/ImageResApp.json";
        isOldVersion = true;
        if (FileUtil.FileIsExist(filepath))
        {
            isOldVersion = false;
        }
        else
        {
            filepath = Common.RES_CONFIG_DATA + "/Image/ImageRes.json";

        }
        imageResApp.isOldVersion = isOldVersion;
        imageResApp.Init(filepath);




        filepath = Common.RES_CONFIG_DATA_COMMON + "/Image/ImageRes.json";
        if (FileUtil.FileIsExist(filepath))
        {
             Debug.Log("imageResCommon file  exist");
            imageResCommon = new ImageResInternal();
            imageResCommon.isOldVersion = true;
            imageResCommon.Init(filepath);
        }else
        {
            Debug.Log("imageResCommon file is not exist");
        }

        filepath = Common.RES_CONFIG_DATA + "/Image/ImageResAppCommon.json";
        if (FileUtil.FileIsExist(filepath))
        {
            imageResAppCommon = new ImageResInternal();
            imageResAppCommon.isOldVersion = false;
            imageResAppCommon.Init(filepath);
        }

        // F:\sourcecode\unity\product\kidsgame\kidsgameUnity\Assets\Script\Common\Resources\Common\UI\UIKit\UIProgress

        filepath = "Common/UI/ImageResDefault.json";
        if (FileUtil.FileIsExist(filepath))
        {
            imageResDefault = new ImageResInternal();
            imageResDefault.isOldVersion = false;
            imageResDefault.Init(filepath);
        }

    }

    public string GetImageBoardString(string path)
    {
        string ret = "";
        if (imageResOld != null)
        {
            string key = imageResOld.FindKeyByPath(path);
            if (!Common.BlankString(key))
            {
                ret = imageResOld.GetImageBoardString(key);
            }
        }

        if (Common.BlankString(ret))
        {
            if (imageResCommon != null)
            {
                string key = imageResCommon.FindKeyByPath(path);
                if (!Common.BlankString(key))
                {
                    ret = imageResCommon.GetImageBoardString(key);
                }
            }
        }


        if (Common.BlankString(ret))
        {
            if (imageResApp != null)
            {
                string key = imageResApp.FindKeyByPath(path);
                if (!Common.BlankString(key))
                {
                    ret = imageResApp.GetImageBoardString(key);
                }
            }
        }

        if (Common.BlankString(ret))
        {
            if (imageResAppCommon != null)
            {
                string key = imageResAppCommon.FindKeyByPath(path);
                if (!Common.BlankString(key))
                {
                    ret = imageResAppCommon.GetImageBoardString(key);
                }
            }
        }

        return ret;
    }

    public bool IsHasBoard(string key)
    {
        bool ret = false;
        if (imageResApp.IsHasKey(key))
        {
            ret = imageResApp.IsHasBoard(key);
        }
        else
        {
            if (imageResAppCommon != null)
            {
                ret = imageResAppCommon.IsHasBoard(key);
            }
            else
            {
                if (imageResCommon != null)
                {
                    ret = imageResCommon.IsHasBoard(key);
                }
            }
        }

        //old
        if (ret == false)
        {
            if (imageResOld != null)
            {
                ret = imageResOld.IsHasBoard(key);
                Debug.Log("imageResOld IsHasBoard key=" + key + " ret=" + ret);
            }


            if (!ret)
            {
                if (imageResCommon != null)
                {
                    ret = imageResCommon.IsHasBoard(key);
                }
            }

        }
        if (ret == false)
        {
            if (imageResDefault != null)
            {
                ret = imageResDefault.IsHasBoard(key);
            }
        }



        return ret;
    }


     public bool IsContainsKey(string key)
    {
        bool ret = false;
        if (imageResApp.IsHasKey(key))
        {
            ret = true;
        }
        else
        {
            if (imageResAppCommon != null)
            {
                ret = imageResAppCommon.IsHasKey(key);
            }
            else
            {
                if (imageResCommon != null)
                {
                    ret = imageResCommon.IsHasKey(key);
                }
            }
        }

        //old
        if (ret == false)
        {
            if (imageResOld != null)
            {
                ret = imageResOld.IsHasKey(key);
                Debug.Log("imageResOld IsHasBoard key=" + key + " ret=" + ret);
            }


            if (!ret)
            {
                if (imageResCommon != null)
                {
                    ret = imageResCommon.IsHasKey(key);
                }
            }

        }
        if (ret == false)
        {
            if (imageResDefault != null)
            {
                ret = imageResDefault.IsHasKey(key);
            }
        }
  
        return ret;
    }

    public string GetImage(string key)
    {
        string ret = "";
        if (imageResApp.IsHasKey(key))
        {
            ret = imageResApp.GetImage(key);
        }
        else
        {
            if (imageResAppCommon != null)
            {
                ret = imageResAppCommon.GetImage(key);
            }
            else
            {
                if (imageResCommon != null)
                {
                    ret = imageResCommon.GetImage(key);
                }
            }

            // if (ret == "_NO_KEY_")
            if (Common.BlankString(ret))
            {
                if (imageResOld != null)
                {
                    ret = imageResOld.GetImage(key);
                    Debug.Log("imageResOld GetImage _NO_KEY_ =" + key + " ret=" + ret);
                }
                // if (ret == "_NO_KEY_")
                if (Common.BlankString(ret))
                {
                    if (imageResCommon != null)
                    {
                        ret = imageResCommon.GetImage(key);
                    }
                }

            }
        }


        if (Common.BlankString(ret))
        {
            if (imageResDefault != null)
            {
                if (imageResDefault != null)
                {
                    ret = imageResDefault.GetImage(key);
                }
            }
        }

        return ret;
    }



    public Vector4 GetImageBoard(string key)
    {
        Vector4 ret = Vector4.zero;
        if (imageResApp.IsHasKey(key))
        {
            ret = imageResApp.GetImageBoard(key);
        }
        else
        {

            if (imageResAppCommon != null)
            {
                if (imageResAppCommon.IsHasKey(key))
                {
                    ret = imageResAppCommon.GetImageBoard(key);
                }

            }
            // else
            // {
            //     if (imageResCommon != null)
            //     {
            //         if (imageResCommon.IsHasKey(key))
            //         {
            //             ret = imageResCommon.GetImageBoard(key);
            //         }
            //     }
            // }
        }



        //old
        if (ret == Vector4.zero)
        {
            if (imageResOld != null)
            {
                if (imageResOld.IsHasKey(key))
                {
                    ret = imageResOld.GetImageBoard(key);
                }
            }
        }

        if (ret == Vector4.zero)
        {
            if (imageResCommon != null)
            {
                if (imageResCommon.IsHasKey(key))
                {
                    ret = imageResCommon.GetImageBoard(key);
                }
            }
        }


        if (ret == Vector4.zero)
        {
            if (imageResDefault != null)
            {
                if (imageResDefault.IsHasKey(key))
                {
                    ret = imageResDefault.GetImageBoard(key);
                }
            }
        }

        return ret;
    }
}
