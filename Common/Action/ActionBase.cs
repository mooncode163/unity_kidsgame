using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//动画
public delegate void OnActionCompleteDelegate(GameObject obj);


/*
DOTween 用法
https://blog.csdn.net/rookie0518/article/details/53157730
 */
 
public class ActionBase : MonoBehaviour
{
    public GameObject target;
    public float percentage;
    public float duration;//总时间
    public int index;
    public bool isLoop = false;
    public bool isUpdateByPercent = true;
    float runningTime;


    bool reverse;
    bool isPaused;
    public OnActionCompleteDelegate callbackComplete { get; set; }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        runningTime = 0;
        percentage = 0;
        reverse = false;
        isPaused = true;
        isLoop = false;
        isUpdateByPercent = true;
        InitAction();
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            if (isUpdateByPercent)
            {
                UpdatePercentage();
            }

            UpdateAction();
            if (percentage > 1f)
            {
                OnFinish();
            }
        }

    }


    public void UpdatePercentage()
    {
        runningTime += Time.deltaTime;

        if (duration != 0)
        {
            if (reverse)
            {
                percentage = 1 - runningTime / duration;
            }
            else
            {
                percentage = runningTime / duration;
            }
            Debug.Log("UpdatePercentage:percentage=" + percentage);

        }


    }


    public void OnFinish()
    {

        if (isLoop)
        {
            runningTime = 0;
            percentage = 0;
            return;
        }
        Pause();
        OnActionComplete();
        if (callbackComplete != null)
        {
            callbackComplete(this.gameObject);
        }
        DestroyImmediate(this);

    }

    public void Pause()
    {
        isPaused = true;
    }
    public void Run()
    {
        isPaused = false;
    }


    public virtual void InitAction()
    {

    }
    public virtual void UpdateAction()
    {

    }

    public virtual void OnActionComplete()
    {

    }
}
