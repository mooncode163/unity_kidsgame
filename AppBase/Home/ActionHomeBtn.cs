using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ActionHomeBtn : MonoBehaviour
{
    public Vector2 ptNormal;
    float timeAction;
    bool isActionFinish;
    void Awake()
    {

        timeAction = 0.3f;
        isActionFinish = false;

    }

    // Use this for initialization
    void Start()
    {
        isActionFinish = false;
        //  RunAction();

    }


    public void RunAction()
    {
        //动画：https://blog.csdn.net/agsgh/article/details/79447090
        //iTween.ScaleTo(info.obj, new Vector3(0f, 0f, 0f), 1.5f);
        float duration = timeAction;
        RectTransform rctran = this.gameObject.GetComponent<RectTransform>();
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        float x, y;
        x = 0;
        y = -sizeCanvas.y / 2 - rctran.rect.height;
        rctran.anchoredPosition = new Vector2(x, y);
        Vector2 toPos = new Vector2(ptNormal.x, ptNormal.y);
        rctran.DOLocalMove(toPos, duration).OnComplete(() =>
                  {
                      this.RunActionUpDown();
                      isActionFinish = true;
                  });
    }

    //上下晃动动画
    void RunActionUpDown()
    {
        //动画：https://blog.csdn.net/agsgh/article/details/79447090
        //iTween.ScaleTo(info.obj, new Vector3(0f, 0f, 0f), 1.5f);
        float duration = timeAction * 4;
        Vector2 size = AppSceneBase.main.sizeCanvas;
        float w, h;

        RectTransform rctran = this.gameObject.GetComponent<RectTransform>();
        h = rctran.rect.height;
        float y_step = h / 10;
        Vector2 pt = ptNormal;
        // var actionUp = cc.moveBy(duration, 0, y_step);
        // var actionDown = cc.moveBy(duration, 0, -y_step);
        //  var time = cc.delayTime(0.5 * i);
        //   var seq = cc.sequence([time, actionUp, actionUp.reverse(), actionDown, actionDown.reverse()]);
        //  btn.node.runAction(seq.repeatForever());
        float z = this.transform.localPosition.z;

        Vector3 posNormal = new Vector3(pt.x, pt.y, z);
        Vector3 toPos = new Vector3(pt.x, pt.y + y_step, z);
        Sequence seq = DOTween.Sequence();
        //actionUp
        Tweener acUp = rctran.DOLocalMove(toPos, duration);

        //normal
        Tweener acNormal = rctran.DOLocalMove(posNormal, duration);
        Tweener acNormal2 = rctran.DOLocalMove(posNormal, duration);
        //actionDown
        toPos = new Vector3(pt.x, pt.y - y_step, z);
        Tweener acDown = rctran.DOLocalMove(toPos, duration);
        float time = 0;
        seq.AppendInterval(time).Append(acUp).Append(acNormal).Append(acDown).Append(acNormal2).SetLoops(-1);

    }
}
