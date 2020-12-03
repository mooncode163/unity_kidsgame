using UnityEngine;

using System.Collections;

using System;
//http://www.tuicool.com/articles/rQ7Vfee
public class DelayRun : MonoBehaviour

{

    public static IEnumerator Run(Action action, float delaySeconds)

    {
        yield return new WaitForSeconds(delaySeconds);

        action();

    }

    void OnClick()

    {

        StartCoroutine(DelayRun.Run(() =>

        {
            Debug.Log("test DelayInvoke");

        }, 0.1f));

    }


    //http://www.bubuko.com/infodetail-587750.html
   // 如果想要传递参数，并且实现延迟调用，可以考虑采用Coroutine

  


}