using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIGameWinBase : UIViewPop
{

 public const string KEY_GAMEWIN_INFO_INTRO = "KEY_GAMEWIN_INFO_INTRO";
    public const string KEY_GAMEWIN_INFO_YUANWEN = "KEY_GAMEWIN_INFO_YUANWEN";

    public const string KEY_GAMEWIN_INFO_TRANSLATION = "KEY_GAMEWIN_INFO_TRANSLATION";
    public const string KEY_GAMEWIN_INFO_JIANSHUANG = "KEY_GAMEWIN_INFO_JIANSHUANG";
    public const string KEY_GAMEWIN_INFO_AUTHOR_INTRO = "KEY_GAMEWIN_INFO_AUTHOR_INTRO";


    public const string KEY_GAMEWIN_INFO_ALBUM = "KEY_GAMEWIN_INFO_ALBUM";


    public UISegment uiSegment;
    public UITextView textView;
    public UIText textTitle;
    public UIText textPinyin;
    public UIImage imageBg;
    public UIImage imageHead;
    public Button btnClose;

    public Button btnFriend;
    public Button btnNext;
    public UIButton btnAddLove;
    public GameObject objLayoutBtn;

   public CaiCaiLeItemInfo infoItem;
   public int indexSegment;

    /// <summary>
    /// Unity's Awake method.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
       
    }

    /// <summary>
    /// Unity's Start method.
    /// </summary>
    protected override void Start()
    {
        base.Start(); 
        LayOut();
    }
 
 
    public override void LayOut()
    {
        base.LayOut();
        
    }
      public virtual void UpdateItem(ItemInfo info)
    { 

    }

 
}
