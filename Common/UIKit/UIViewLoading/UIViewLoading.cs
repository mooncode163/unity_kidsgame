using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIViewLoading : UIView
{

    // Use this for initialization
    public Image imageBg;
    public RawImage imageLoading;
    private ActionImage acImage;
    void Awake()
    {
        acImage = imageLoading.gameObject.AddComponent<ActionImage>();
        acImage.duration = 3f;
        acImage.isLoop = true;
        for (int i = 0; i < 68; i++)
        {
            string pic = "Common/UI/UIKit/UIViewLoading/loading_animate" + i.ToString();
            acImage.AddPic(pic);
        }

        // acImage.callbackComplete = OnActionFinish;
    }
    void Start()
    {
        RunAction();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Show(bool isShow)
    {
        this.gameObject.SetActive(isShow);
    }

    void RunAction()
    {
        //   acImage.Run();
        acImage.Run();
    }

    public void OnActionFinish(GameObject obj)
    {

    }

}
