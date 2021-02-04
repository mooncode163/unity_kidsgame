using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIItemDetail : UIViewPop
{
    public const string KEY_GAMEWIN_INFO_INTRO = "KEY_GAMEWIN_INFO_INTRO";
    public const string KEY_GAMEWIN_INFO_YUANWEN = "KEY_GAMEWIN_INFO_YUANWEN";

    public const string KEY_GAMEWIN_INFO_TRANSLATION = "KEY_GAMEWIN_INFO_TRANSLATION";
    public const string KEY_GAMEWIN_INFO_JIANSHUANG = "KEY_GAMEWIN_INFO_JIANSHUANG";
    public const string KEY_GAMEWIN_INFO_AUTHOR_INTRO = "KEY_GAMEWIN_INFO_AUTHOR_INTRO";


    public const string KEY_GAMEWIN_INFO_ALBUM = "KEY_GAMEWIN_INFO_ALBUM";



    public const string KEY_GAMEWIN_ContentPinYin = "KEY_GAMEWIN_ContentPinYin";


    public UIImage imageBg;
    public Button btnClose;

    public UIButton btnAdd;

    public CaiCaiLeItemInfo infoItem;

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

    // Update is called once per frame
    void Update()
    {

    }


    public override void LayOut()
    {
        base.LayOut();
    }

    public virtual void UpdateItem(CaiCaiLeItemInfo info)
    {


    }

    public void UpdateLoveStatus()
    {

        string strkey = "";
        if (DBLove.main.IsItemExist(infoItem.dbInfo))
        {
            strkey = "STR_IdiomDetail_DELETE_LOVE";
        }
        else
        {
            strkey = "STR_IdiomDetail_ADD_LOVE";
        }
        btnAdd.textTitle.UpdateTextByKey(strkey);

    }



    public void OnClickBtnClose()
    {
        Close();
    }

    public void OnClickBtnAdd()
    {
        // Close();
        if (DBLove.main.IsItemExist(infoItem.dbInfo))
        {
            DBLove.main.DeleteItem(infoItem.dbInfo);
        }
        else
        {
            DBLove.main.AddItem(infoItem.dbInfo);
        }
        UpdateLoveStatus();
    }
}
