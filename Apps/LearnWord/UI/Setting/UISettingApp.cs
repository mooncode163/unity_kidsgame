using System.Collections;
using System.Collections.Generic;
using Moonma.IAP;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;


public class UISettingApp : UISettingControllerBase
{
 
    // Update is called once per frame
    void Update()
    {

        if (Device.isDeviceDidRotation)
        {
            LayOut();
        }
    }


    public override void LayOut()
    {
        base.LayOut();

    }



}
