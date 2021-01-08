using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class AnimateCommon : MonoBehaviour
{

    public static AnimateCommon main;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (main == null)
        {
            main = this;
        }
    }


    public void RunFlipH(GameObject front, GameObject back, float duration)
    {
        StartCoroutine(Flip(front, back, duration, true));
    }

    public void RunFlipV(GameObject front, GameObject back, float duration)
    {
        StartCoroutine(Flip(front, back, duration, false));
    }
    // 正面 背面
    IEnumerator Flip(GameObject front, GameObject back, float duration, bool isH)
    {
        front.transform.eulerAngles = Vector3.zero;
        Vector3 angleFrontTo = new Vector3(0, 90, 0);
        if (!isH)
        {
            angleFrontTo = new Vector3(90, 0, 0);
        }

        front.transform.DORotate(angleFrontTo, duration);
        for (float i = duration; i >= 0; i -= Time.deltaTime)
            yield return 0;

        Vector3 angleBackTo = new Vector3(0, 0, 0); 
        back.transform.DORotate(angleBackTo, duration);


    }
}
