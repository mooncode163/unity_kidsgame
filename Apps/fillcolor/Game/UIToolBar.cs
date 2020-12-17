using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IUIToolBarDelegate
{
    void OnUIToolBarDidClick(UIToolBar ui);

}

public class UIToolBar : UIView
{
    public const string STR_KEYNAME_VIEWALERT_SAVE_TIPS = "STR_KEYNAME_VIEWALERT_SAVE_TIPS";
    public const string STR_KEYNAME_VIEWALERT_DELETE_ALL = "keyname_viewalert_delete_all_paint";

    public const string STR_KEYNAME_VIEWALERT_SAVE_FINISH = "STR_KEYNAME_VIEWALERT_SAVE_FINISH";
    public const string STR_KEYNAME_VIEWALERT_FIRST_USE_FUNCTION = "keyname_viewalert_first_use_function";


    public const string KEY_STR_FIRST_USE_STRAW = "KEY_STR_FIRST_USE_STRAW";
    public const string KEY_STR_FIRST_USE_COLOR_INPUT = "KEY_STR_FIRST_USE_COLOR_INPUT";

    public const int BTN_CLICK_MODE_STRAW = 0;
    public const int BTN_CLICK_MODE_COLOR_INPUT = 1;
    public Button btnColor;
    public Button btnMusic;
    public Button btnDelLast;//删除最后一画
                             //public UIViewAlert viewAlert;
    public UIGameFillColor uiGameFillColor;
    int btnClickMode;




    bool isFirstUseStraw
    {
        get
        {
            return false;
            if (Common.noad)
            {
                return false;
            }
            return Common.Int2Bool(PlayerPrefs.GetInt(KEY_STR_FIRST_USE_STRAW, Common.Bool2Int(true)));
        }
        set
        {

            PlayerPrefs.SetInt(KEY_STR_FIRST_USE_STRAW, Common.Bool2Int(value));
        }
    }

    bool isFirstUseColorInput
    {
        get
        {
            return false;
            if (Common.noad)
            {
                return false;
            }
            return Common.Int2Bool(PlayerPrefs.GetInt(KEY_STR_FIRST_USE_COLOR_INPUT, Common.Bool2Int(true)));
        }
        set
        {

            PlayerPrefs.SetInt(KEY_STR_FIRST_USE_COLOR_INPUT, Common.Bool2Int(value));
        }
    }

    void Awake()
    {
        btnDelLast.gameObject.SetActive(false);
        UIHomeBase.UpdateBtnMusic(btnMusic);
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnClickBtnBack();
        }
    }

    public void UpdateColor(Color cr)
    {
        Image imageColor = btnColor.gameObject.GetComponent<UIButton>().imageBg.image;
        imageColor.color = cr;
    }

    public void ShowFirstUseAlert()
    {
        string title = Language.main.GetString("STR_UIVIEWALERT_TITLE_FIRST_USE_FUNCTION");
        string msg = Language.main.GetString("STR_UIVIEWALERT_MSG_FIRST_USE_FUNCTION");
        string yes = Language.main.GetString("STR_UIVIEWALERT_YES_FIRST_USE_FUNCTION");
        string no = "no";

        ViewAlertManager.main.ShowFull(title, msg, yes, no, false, STR_KEYNAME_VIEWALERT_FIRST_USE_FUNCTION, OnUIViewAlertFinished);

    }

    public void ShowSaveFinishAlert()
    {

        string title = Language.main.GetString("STR_UIVIEWALERT_TITLE_SAVE_FINISH");
        string msg = Language.main.GetString("STR_UIVIEWALERT_MSG_SAVE_FINISH");
        string yes = Language.main.GetString("STR_UIVIEWALERT_YES_SAVE_FINISH");
        string no = "no";
        //  viewAlert.HideDelay(2f);
        ViewAlertManager.main.ShowFull(title, msg, yes, no, false, STR_KEYNAME_VIEWALERT_SAVE_FINISH, OnUIViewAlertFinished);
    }

    //返回保存提示
    public void ShowSaveTipsAlert()
    {
        string title = Language.main.GetString("STR_UIVIEWALERT_TITLE_SAVE");
        string msg = Language.main.GetString("STR_UIVIEWALERT_MSG_SAVE");
        string yes = Language.main.GetString("STR_UIVIEWALERT_YES_SAVE");
        string no = Language.main.GetString("STR_UIVIEWALERT_NO_SAVE");

        ViewAlertManager.main.ShowFull(title, msg, yes, no, true, STR_KEYNAME_VIEWALERT_SAVE_TIPS, OnUIViewAlertFinished);

    }
    void OnUIViewAlertFinished(UIViewAlert alert, bool isYes)

    {


        if (STR_KEYNAME_VIEWALERT_FIRST_USE_FUNCTION == alert.keyName)
        {
            if (isYes)
            {
                if (btnClickMode == BTN_CLICK_MODE_STRAW)
                {
                    DoClickBtnStrawAlert();
                }
                if (btnClickMode == BTN_CLICK_MODE_COLOR_INPUT)
                {
                    DoClickBtnColorInputAlert();
                }
            }
            else
            {

            }
        }

        if (STR_KEYNAME_VIEWALERT_SAVE_TIPS == alert.keyName)
        {
            if (isYes)
            {
                OnClickBtnSave();
            }
            else
            {

            }
            DoClickBtnBack();
        }

        if (STR_KEYNAME_VIEWALERT_DELETE_ALL == alert.keyName)
        {
            if (isYes)
            {
                DoDeleteAll();
            }

        }



    }


    void DoClickBtnBack()
    {
        uiGameFillColor.OnClickBtnBack();
        //uiGameFillColor.ShowAdInsert(UIGameBase.GAME_AD_INSERT_SHOW_STEP);
    }
    public void OnClickBtnBack()
    {
        if ((!uiGameFillColor.gameFillColor.isHasSave) && (uiGameFillColor.gameFillColor.isHasPaint))
        {
            ShowSaveTipsAlert();
            return;
        }
        DoClickBtnBack();
    }
    public void OnClickBtnSave()
    {
        //  UpdateSpriteFill();
        if (!uiGameFillColor.gameFillColor.IsHaveStartDraw())
        {
            //没有作画
            return;
        }

        ColorItemInfo info = GameLevelParse.main.GetItemInfo();
        string filepath = GameLevelParse.main.GetFileSave(info);
        info.infoDB.filesave = filepath;
        uiGameFillColor.gameFillColor.DoSave(filepath);
        bool isexist = DBColor.main.IsItemExist(info.infoDB);
        Debug.Log("IsItemExist:" + isexist + " filepath=" + filepath);
        if (isexist)
        {
            DBColor.main.UpdateItemTime(info.infoDB);
        }
        else
        {
            DBColor.main.AddItem(info.infoDB);
        }

        ShowSaveFinishAlert();

    }

    //清除当前
    public void OnClickBtnDelLast()
    {
        // tickDraw = 0;
        List<ColorItemInfo> listColorFill = uiGameFillColor.gameFillColor.listColorFill;

        if (!uiGameFillColor.gameFillColor.IsHaveStartDraw())
        {
            //没有作画
            return;
        }

        uiGameFillColor.gameFillColor.OnDelLast();

        if (listColorFill.Count == 0)
        {
            btnDelLast.gameObject.SetActive(false);
        }
    }
    public void OnClickBtnColorBoard()
    {
        uiGameFillColor.gameFillColor.ShowStraw(false);
        Debug.Log("OnClickBtnColorBoard");
        uiGameFillColor.uiColorBoard.gameObject.SetActive(!uiGameFillColor.uiColorBoard.gameObject.activeSelf);
    }


    public void OnClickBtnMusic()
    {
        bool ret = Common.GetBool(AppString.STR_KEY_BACKGROUND_MUSIC);
        bool value = !ret;
        Common.SetBool(AppString.STR_KEY_BACKGROUND_MUSIC, value);
        if (value)
        {
            AudioPlay.main.Play();
        }
        else
        {
            AudioPlay.main.Stop();
        }
        UIHomeBase.UpdateBtnMusic(btnMusic);
    }
    void DoDeleteAll()
    {
        uiGameFillColor.gameFillColor.DoDeleteAll();
    }
    public void OnClickBtnDelAll()
    {
        // tickDraw = 0;
        {

            string title = Language.main.GetString("STR_UIVIEWALERT_TITLE_DELETE_ALL_PAINT_POINT");
            string msg = Language.main.GetString("STR_UIVIEWALERT_MSG_DELETE_ALL_PAINT_POINT");
            string yes = Language.main.GetString("STR_UIVIEWALERT_YES");
            string no = Language.main.GetString("STR_UIVIEWALERT_NO");

            ViewAlertManager.main.ShowFull(title, msg, yes, no, true, STR_KEYNAME_VIEWALERT_DELETE_ALL, OnUIViewAlertFinished);

        }

    }

    void DoClickBtnStraw()
    {
        uiGameFillColor.gameFillColor.ShowStraw(!uiGameFillColor.gameFillColor.IsStrawActive());
        isFirstUseStraw = false;
    }
    void DoClickBtnStrawAlert()
    {
        if (AppVersion.appCheckHasFinished && !Application.isEditor)
        {
            if (isFirstUseStraw)
            {
                //show ad video
                AdKitCommon.main.callbackAdVideoFinish = OnAdKitAdVideoFinish;
                AdKitCommon.main.ShowAdVideo();
            }
            else
            {
                DoClickBtnStraw();
            }
        }
        else
        {
            DoClickBtnStraw();
        }
    }
    public void OnClickBtnStraw()
    {
        Debug.Log("OnClickBtnStraw");
        btnClickMode = BTN_CLICK_MODE_STRAW;
        if (AppVersion.appCheckHasFinished && isFirstUseStraw)
        {
            ShowFirstUseAlert();
        }
        else
        {
            DoClickBtnStraw();
        }


    }
    void DoClickBtnColorInput()
    {
        isFirstUseColorInput = false;
        uiGameFillColor.uiColorInput.UpdateInitColor(uiGameFillColor.gameFillColor.colorFill);
        uiGameFillColor.uiColorInput.gameObject.SetActive(!uiGameFillColor.uiColorInput.gameObject.activeSelf);
        uiGameFillColor.uiColorInput.ColorNow = uiGameFillColor.gameFillColor.colorFill;
        uiGameFillColor.uiColorInput.UpdateColorNow();
    }
    void DoClickBtnColorInputAlert()
    {
        if (AppVersion.appCheckHasFinished && !Application.isEditor)
        {
            if (isFirstUseColorInput)
            {
                //show ad video
                AdKitCommon.main.callbackAdVideoFinish = OnAdKitAdVideoFinish;
                AdKitCommon.main.ShowAdVideo();
            }
            else
            {
                DoClickBtnColorInput();
            }
        }
        else
        {
            DoClickBtnColorInput();
        }
    }

    public void OnClickBtnColorInput()
    {
        btnClickMode = BTN_CLICK_MODE_COLOR_INPUT;
        uiGameFillColor.gameFillColor.ShowStraw(false);
        if (AppVersion.appCheckHasFinished && isFirstUseColorInput)
        {
            ShowFirstUseAlert();
        }
        else
        {
            DoClickBtnColorInput();
        }
    }


    public void OnAdKitAdVideoFinish(AdKitCommon.AdType type, AdKitCommon.AdStatus status, string str)
    {
        switch (status)
        {
            case AdKitCommon.AdStatus.SUCCESFULL:
                {
                    if (btnClickMode == BTN_CLICK_MODE_STRAW)
                    {
                        DoClickBtnStraw();
                    }
                    if (btnClickMode == BTN_CLICK_MODE_COLOR_INPUT)
                    {
                        DoClickBtnColorInput();
                    }
                }
                break;
            case AdKitCommon.AdStatus.FAIL:
                {
                    uiGameFillColor.ShowAdVideoFailAlert();
                }
                break;
        }
    }

}
