using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIIconController : UIShotBase
{
    public UIImage imageBg;
    public UIImage imageHD;
    public UIImage imageBoard; 
 
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        imageHD.gameObject.SetActive(false);
        imageBoard.gameObject.SetActive(true);
        string pic_name = Common.GAME_DATA_DIR + "/screenshot/icon/bg";
        string pic = pic_name + ".jpg";
        if (!FileUtil.FileIsExistAsset(pic))
        {
            pic = pic_name + ".png";
        }
        imageBg.UpdateImage(pic);
        

        LayOut();
    }
    void Start()
    {
        IconViewController iconctroller = (IconViewController)this.controller;
        if (iconctroller != null)
        {
            if (iconctroller.deviceInfo.isIconHd)
            {
                imageHD.gameObject.SetActive(true);
            }

        }

       
        LayOut();
        OnUIDidFinish();
    }


    public override void LayOut()
    {
        base.LayOut();

    }
}
