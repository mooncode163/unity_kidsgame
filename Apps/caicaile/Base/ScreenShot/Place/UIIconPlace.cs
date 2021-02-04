
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIIconPlace : UIShotBase
{
    public UIImage imageBg;
    public UIImage imageHD;
    public UIImage imageBoard;
    public UIText textTitle0; 
public UIText textTitle1; 
 
    public int row = 7;
    public int col = 7;
    public List<UIItemFlower> listItem;

    LayOutGrid lygrid;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        imageHD.gameObject.SetActive(false);  
        textTitle0.text = "诗";
         textTitle1.text = "词";
        UpdateItem();
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

    public void UpdateItem()
    { 
        LayOut();
 
    }


}
