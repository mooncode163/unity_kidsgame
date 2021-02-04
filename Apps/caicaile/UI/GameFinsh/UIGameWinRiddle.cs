using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIGameWinRiddle : UIGameWinBase
{


    /// <summary>
    /// Unity's Awake method.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        CaiCaiLeItemInfo info = GameLevelParse.main.GetItemInfo();


        textView.SetFontSize(80); 
       
        UpdateText(null);
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

    public void UpdateText(ItemInfo info)
    {
        CaiCaiLeItemInfo infoGuanka = GameLevelParse.main.GetItemInfo();
        GameLevelParse.main.ParseItem(infoGuanka);
        infoItem = infoGuanka;
        string str = ""; 
       
        str = infoGuanka.head+"\n"+infoGuanka.end+"\n"+infoGuanka.type; 
        if (Common.BlankString(str))
        {
            str = Language.main.GetString("STR_UIVIEWALERT_MSG_GAME_FINISH");
        }
        Debug.Log("UpdateText str="+str);
        textView.text = str;
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
        GameManager.main.GotoPlayAgain();
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
