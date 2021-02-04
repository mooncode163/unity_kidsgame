using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public interface IUIWordItemDelegate
{
    /// <summary>
    /// Get the number of rows that a certain table should display
    /// </summary>

    void WordItemDidClick(UIWordItem item);

}
public class UIWordItem : UIView
{
    public const float TIME_ANIMATE_ERROR = 2.0f;
    public const int COUNT_ANIMATE_ERROR = 10;
    public Image imageBg;
    public Text textTitle;
    public int index;
    public string wordDisplay;
    public string wordAnswer;//答案字符
    public bool isShowContent = true;
    private IUIWordItemDelegate _delegate;

    float timerAnimate = TIME_ANIMATE_ERROR / COUNT_ANIMATE_ERROR;
    bool enableAnimate = false;
    int countTemp = 0;

    public bool isWordTips;
    public IUIWordItemDelegate iDelegate
    {
        get { return _delegate; }
        set { _delegate = value; }
    }

    // Use this for initialization
    void Start()
    {
        isShowContent = true;
        isWordTips = false;
    }


    // Update is called once per frame
    void Update()
    {

        if (enableAnimate)
        {
            timerAnimate -= Time.deltaTime;
            if (timerAnimate <= 0)
            {
                OnTimerAnimate();
                timerAnimate = TIME_ANIMATE_ERROR / COUNT_ANIMATE_ERROR;
            }
        }

    }

    public void StartAnimateError()
    {
        enableAnimate = true;
        countTemp = 0;
        timerAnimate = TIME_ANIMATE_ERROR / COUNT_ANIMATE_ERROR;
    }

    public void StopAnimateError()
    {
        if (enableAnimate)
        {
            enableAnimate = false;
            StartCoroutine(TimerDidStop());
            Debug.Log("StopAnimateError");
        }

    }




    IEnumerator TimerDidStop()
    {
        yield return null;//new WaitForSeconds(1.0f);
        textTitle.gameObject.SetActive(true);
    }

    void OnTimerAnimate()
    {
        countTemp++;
        if (countTemp % 2 == 0)
        {
            textTitle.gameObject.SetActive(true);
        }
        else
        {
            textTitle.gameObject.SetActive(false);
        }
    }
    public void UpdateTitle(string title)
    {
        wordDisplay = title;
        textTitle.text = title;
    }
    public void ShowContent(bool isShow)
    {
        isShowContent = isShow;
        imageBg.gameObject.SetActive(isShow);
        textTitle.gameObject.SetActive(isShow);
    }

    //是否答对
    public bool IsAnswer()
    {
        bool ret = false;
        if (wordDisplay == wordAnswer)
        {
            ret = true;
        }
        return ret;
    }
    public void ClearWord()
    {
        UpdateTitle("");
    }
    public void SetWordColor(Color color)
    {
        textTitle.color = color;
    }

    public void SetFontSize(int size)
    {
        textTitle.fontSize = size;
    }
    public void OnClickItem()
    {
        if (isWordTips)
        {
            //提示字  不响应
            return;
        }
        if (_delegate != null)
        {
            Debug.Log("UIWordItem OnClickItem index=" + index);
            _delegate.WordItemDidClick(this);
        }
    }
}
