using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//闪烁动画
public class ActionBlink : ActionBase
{
    public int count = 20;
    int count_step = 0;
    public override void InitAction()
    {
        Debug.Log("ActionBlink:InitAction");
    }
    public override void UpdateAction()
    {
        int percent = (int)(100 * percentage);
        if (percent <= 100)
        {
            if (count_step % count == 0)
            {
                target.gameObject.SetActive(true);
            }
            else
            {
                target.gameObject.SetActive(false);
            }
            count_step++;
        }
        else
        {
            target.gameObject.SetActive(true);

        }




    }

    public override void OnActionComplete()
    {
        if (target != null)
        {
            target.gameObject.SetActive(true);
        }

    }
}
