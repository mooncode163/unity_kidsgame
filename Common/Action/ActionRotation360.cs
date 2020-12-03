using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionRotation360 : ActionBase
{

    public override void InitAction()
    {
        Debug.Log("ActionRotation360:InitAction");
    }
    public override void UpdateAction()
    {
        float anglez = 360 * percentage;
		if(percentage>1f){
			percentage = 0f;
		}
        this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, anglez));
    }
}
