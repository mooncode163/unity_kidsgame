using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UITipsMsg : UIViewPop
{

    public const string KEY_TIPS_MSG_TITLE = "KEY_TIPS_MSG_TITLE1";
    public const string KEY_TIPS_MSG_TRANSLATION = "KEY_TIPS_MSG_TRANSLATION1";
    public const string KEY_TIPS_MSG_VIDEO = "KEY_TIPS_MSG_VIDEO";
    public UIText textTitle;
    public UIText textMsg;
    public UIButton btnTips;
    public UIButton btnNo;
    public UIText textTips;
     public GameObject objTips;
    public string keyMsg;
    public bool isBtnYes;

    /// <summary>
    /// Unity's Awake method.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        textTips.text = Language.main.GetString("STR_TIPS_NEVER");
        LayOut();
    }

    /// <summary>
    /// Unity's Start method.
    /// </summary>
    protected override void Start()
    {
        base.Start();
        LayOut();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void LayOut()
    {
        base.LayOut();
        float x = 0, y = 0, w = 0, h = 0;
        float ratio = 0.8f;
        if (Device.isLandscape)
        {
            ratio = 0.8f;
        }

        RectTransform rctranRoot = this.GetComponent<RectTransform>();
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        {

            w = sizeCanvas.x * ratio;
            h = sizeCanvas.y * ratio;//rctran.rect.size.y * w / rctran.rect.size.x;
            rctranRoot.sizeDelta = new Vector2(w, h);
            base.LayOut();
        }
    }


    public void UpdateItem( string msg)
    {
        textTitle.text = Language.main.GetString("STR_TIPS");
        textMsg.text = msg;
        if(keyMsg==KEY_TIPS_MSG_VIDEO)
        {
            objTips.SetActive(false);
            btnNo.gameObject.SetActive(false);
        }

        bool ret = Common.GetBool(keyMsg); 
        btnTips.UpdateSwitch(ret);
    }
    public void OnClickBtnNo()
    {
         isBtnYes = false;
        Close();
    }
    public void OnClickBtnYes()
    {
        isBtnYes = true;
        Close();
    }

    public void OnClickBtnTips()
    {
        string key = keyMsg;
        bool ret = Common.GetBool(key);
        bool value = !ret;
        Common.SetBool(key, value);
        btnTips.UpdateSwitch(value);
    }

}
