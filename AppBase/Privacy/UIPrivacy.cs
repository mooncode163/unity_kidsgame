using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class UIPrivacy : UIViewPop
{
    public const string KEY_DISABLE_UIPRIVACY = "KEY_DISABLE_UIPRIVACY";
    public GameObject objContent; 
    public UIImage imageBoard;
    public UITextView textView; 
    public UIText textTitle;
    public UIButton btnYes;
    public UIButton btnNo; 

    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        textTitle.text = "隐私政策";
           Debug.Log("ParsePrivacy Awake 1");
        textView.text = ParsePrivacy();
          Debug.Log("ParsePrivacy Awake 2 =");
        
    }
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        LayOut();

    }

   
    public override void LayOut()
    {
        base.LayOut();
        float w, h; 
        {
            float ratio = 0.8f;
            RectTransform rctran = objContent.GetComponent<RectTransform>();
            // w = Mathf.Min(this.frame.width, this.frame.height) * 0.8f;
            w = this.frame.width*ratio;
            h = this.frame.height*ratio;
            rctran.sizeDelta = new Vector2(w, h);
            base.LayOut();
        }
    }

        string ParsePrivacy()
    { 
        string filepath = Common.RES_CONFIG_DATA_COMMON + "/Privacy/"+Config.main.PrivacyPolicy; 
        
         Debug.Log("ParsePrivacy start =");
         string text = FileUtil.ReadStringFromResources(filepath);
        Debug.Log("ParsePrivacy text ="+text);
        return text;
    }
 
      public void OnClickBtnYes()
    {
        Common.SetBool(KEY_DISABLE_UIPRIVACY,true);
         Close();
    }
        public void OnClickBtnNo()
    { 
         Close();
        Application.Quit();
    }
}
