
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Vectrosity;
using DG.Tweening;
public class UIAdHomePlace : UIShotBase
{
    public UIImage imageBg;
    public UIText textTitle;   
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        base.Awake();
       
       
        string appname = Common.GetAppNameDisplay();
        textTitle.text = appname;

    }
    void Start()
    {
        UpdateItem();
        LayOut();
        OnUIDidFinish();
    }


    public override void LayOut()
    {
        base.LayOut();
        float x, y, w, h;
       

    }

    public void UpdateItem()
    {
     

        LayOut(); 

    }
     

}

