
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIDetailRiddle : UIDetailBase
{


    /// <summary>
    /// Unity's Awake method.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        textView.SetFontSize(80);


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

        }
        base.LayOut();

        //objLayoutBtn
        {

            LayOutGrid lg = objLayoutBtn.GetComponent<LayOutGrid>();
            lg.enableHide = false;
            int btn_count = lg.GetChildCount(false);
            if (Device.isLandscape)
            {
                lg.row = btn_count;
                lg.col = 1;
            }
            else
            {
                lg.row = 1;
                lg.col = btn_count;
            }
            lg.LayOut();
        }
        base.LayOut();
        // imageHead.transform.localScale = new Vector3(1f, 1f, 1f);
    }
    public override void UpdateItem(CaiCaiLeItemInfo info)
    {
        UpdateText(info);

    }
    public void UpdateText(CaiCaiLeItemInfo info)
    {  
        infoItem = info;
        DBInfoRiddle dbInfo = info.dbInfo as DBInfoRiddle;
        string str = "";

        str = dbInfo.head + "\n" + dbInfo.end + "\n" + dbInfo.type;
        if (Common.BlankString(str))
        {
            str = Language.main.GetString("STR_UIVIEWALERT_MSG_GAME_FINISH");
        }
        Debug.Log("UpdateText str=" + str);
        textView.text = str;
        UpdateLoveStatus();
    }

    public void UpdateLoveStatus()
    {
        string strBtn = "";
        DBInfoRiddle infoRiddle = infoItem.dbInfo as DBInfoRiddle;

        if (DBLoveRiddle.main.IsItemExist(infoRiddle))
        {
            strBtn = Language.main.GetString("STR_Detail_DELETE_LOVE");
        }
        else
        {
            strBtn = Language.main.GetString("STR_Detail_ADD_LOVE");
        }

        // Common.SetButtonText(btnAddLove, strBtn, 0, false);
        btnAddLove.textTitle.text = strBtn;
    }

    public void OnClickBtnClose()
    {
        Close(); 
    }
    public void OnClickBtnFriend()
    {
    }
    public void OnClickBtnNext()
    {
        Close();
        LevelManager.main.GotoNextLevel();
    }
    public void OnClickBtnAddLove()
    {
        DBInfoRiddle infoRiddle = infoItem.dbInfo as DBInfoRiddle;
        if (DBLoveRiddle.main.IsItemExist(infoRiddle))
        {
            DBLoveRiddle.main.DeleteItem(infoRiddle);
        }
        else
        {
            DBLoveRiddle.main.AddItem(infoRiddle);
        }
        UpdateLoveStatus();
    }
}

