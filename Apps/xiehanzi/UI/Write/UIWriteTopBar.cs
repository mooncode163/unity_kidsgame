using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIWriteTopBar : UIView
{
    public const string STR_KEYNAME_VIEWALERT_FIRST_USE_FUNCTION = "keyname_viewalert_first_use_function";
    public const string STR_KEYNAME_VIEWALERT_SAVE = "STR_KEYNAME_VIEWALERT_SAVE";
    public const string STR_KEYNAME_VIEWALERT_DELETE = "STR_KEYNAME_VIEWALERT_DELETE";
    public const string STR_KEYNAME_VIEWALERT_SAVE_TIPS = "STR_KEYNAME_VIEWALERT_SAVE_TIPS";
    public UIWriteController uiWriteController;


    public Image imageGoldBg;
    public Text textGold;

    public Button btnBack;
    public Button btnModeAll;
    public Button btnModeOne;
    public Button btnModeNone;
    public Button btnSave;
    public Button btnDel;
    public UIButton btnColorInput;//任意颜色
    public Button btnLineSetting;
    public Button btnShare;
    public Button btnColorBoard;
 
    bool isShowBihuaImage = false;
    int indexBihua;
    int indexBihuaPoint;

    bool isFirstUseColorInput
    {
        get
        {
            return false;
            // if (Common.noad)
            // {
            //     return false;
            // }
            // if (!AppVersion.appCheckHasFinished)
            // {
            //     return false;
            // }
            // return Common.Int2Bool(PlayerPrefs.GetInt("KEY_STR_FIRST_USE_COLOR_INPUT", Common.Bool2Int(true)));
        }
        set
        {

            PlayerPrefs.SetInt("KEY_STR_FIRST_USE_COLOR_INPUT", Common.Bool2Int(value));
        }
    }
    public Color colorWord
    {
        get
        {
            return Common.RGBString2Color(PlayerPrefs.GetString("KEY_STR_COLOR_WORD", "255,0,0"));
        }
        set
        {

            PlayerPrefs.SetString("KEY_STR_COLOR_WORD", Common.Color2RGBString(value));
        }
    }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (btnShare != null)
        {
            btnShare.gameObject.SetActive(Config.main.isHaveShare);
        }

    }
    // Use this for initialization
    void Start()
    {

    }
    public void ShowFirstUseAlert()
    {

        string title = Language.main.GetString("STR_UIVIEWALERT_TITLE_FIRST_USE_FUNCTION");
        string msg = Language.main.GetString("STR_UIVIEWALERT_MSG_FIRST_USE_FUNCTION");
        string yes = Language.main.GetString("STR_UIVIEWALERT_YES_FIRST_USE_FUNCTION");
        string no = "no";
        ViewAlertManager.main.ShowFull(title, msg, yes, no, false, STR_KEYNAME_VIEWALERT_FIRST_USE_FUNCTION, OnUIViewAlertFinished);

    }


    public void ShowSaveAlert()
    {

        string title = Language.main.GetString("STR_UIVIEWALERT_TITLE_SAVE");
        string msg = Language.main.GetString("STR_UIVIEWALERT_MSG_SAVE");
        string yes = Language.main.GetString("STR_UIVIEWALERT_YES_SAVE");
        string no = Language.main.GetString("STR_UIVIEWALERT_NO_SAVE");
        ViewAlertManager.main.ShowFull(title, msg, yes, no, true, STR_KEYNAME_VIEWALERT_SAVE, OnUIViewAlertFinished);
    }

    public void ShowDeleteAlert()
    {

        string title = Language.main.GetString("STR_UIVIEWALERT_TITLE_DELETE");
        string msg = Language.main.GetString("STR_UIVIEWALERT_MSG_DELETE");
        string yes = Language.main.GetString("STR_UIVIEWALERT_YES_DELETE");
        string no = Language.main.GetString("STR_UIVIEWALERT_NO_DELETE");
        ViewAlertManager.main.ShowFull(title, msg, yes, no, true, STR_KEYNAME_VIEWALERT_DELETE, OnUIViewAlertFinished);
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

    public void OnMode(UIWordWrite.Mode md)
    {

        if (md == UIWordWrite.Mode.FREE_WRITE)
        {
            btnSave.gameObject.SetActive(true);
            btnDel.gameObject.SetActive(true);
            // btnColorBoard.gameObject.SetActive(true);
            btnLineSetting.gameObject.SetActive(true);
            // btnColorInput.gameObject.SetActive(true);
        }
        else
        {
            btnSave.gameObject.SetActive(true);
            btnDel.gameObject.SetActive(false);
            // btnColorBoard.gameObject.SetActive(false);
            btnLineSetting.gameObject.SetActive(false);
            // btnColorInput.gameObject.SetActive(false);
        }
        LayOut();
    }


    void OnUIViewAlertFinished(UIViewAlert alert, bool isYes)
    {
        //  if (STR_KEYNAME_VIEWALERT_FIRST_USE_FUNCTION == alert.keyName)
        //  {
        //         if (isYes)
        //         {
        //             GameManager.GotoNextLevel();
        //         }
        //         else
        //         {
        //             OnClickBtnBack();
        //         }
        //  }

        if (STR_KEYNAME_VIEWALERT_FIRST_USE_FUNCTION == alert.keyName)
        {
            if (isYes)
            {
                if (isFirstUseColorInput)
                {

                    //show ad video
                    AdKitCommon.main.ShowAdVideo();

                }
                else
                {
                    DoClickBtnColorInput();
                }
            }

        }

        if (STR_KEYNAME_VIEWALERT_SAVE == alert.keyName)
        {
            if (isYes)
            {
                DoBtnSave();
            }
        }

        if (STR_KEYNAME_VIEWALERT_DELETE == alert.keyName)
        {
            if (isYes)
            {
                DoBtnDelete();
            }
        }

        if (STR_KEYNAME_VIEWALERT_SAVE_TIPS == alert.keyName)
        {
            if (isYes)
            {
                DoBtnSave();
            }

            DoBtnBack();
        }


    }



    void DoBtnBack()
    {
        AudioPlay.main.PlayFile(AppRes.AUDIO_BTN_CLICK);
        // NaviViewController navi = uiGameXieHanzi.controller.naviController;
        // if (navi != null)
        // {
        //     navi.Pop();
        // }
        uiWriteController.OnBtnClickBack();
        //uiGameXieHanzi.ShowAdInsert(1);
    }
    public void OnClickBtnBack()
    {
        if (uiWriteController.uiWordWrite.mode == UIWordWrite.Mode.FREE_WRITE)
        {
            if ((!uiWriteController.uiWordWrite.isHasSave) && (uiWriteController.uiWordWrite.isHasPaint))
            {
                ShowSaveTipsAlert();
                return;
            }

        }

        DoBtnBack();

    }



    public void OnClickBtnBihuaAll()
    {
        indexBihua = 0;
        uiWriteController.uiWordWrite.GotoMode(UIWordWrite.Mode.ALL_STROKE);
    }
    public void OnClickBtnBihuaOne()
    {
        indexBihua = 0;
        // uiGameXieHanzi.gameXieHanzi.GotoWordWriteMode(WordWriteMode.WriteWithOneBihua);
        uiWriteController.uiWordWrite.GotoMode(UIWordWrite.Mode.ONE_STROKE);
    }
    public void OnClickBtnBihuaNone()
    {
        indexBihua = 0;
        // uiGameXieHanzi.gameXieHanzi.GotoWordWriteMode(WordWriteMode.WriteWithNone);
        uiWriteController.uiWordWrite.GotoMode(UIWordWrite.Mode.NO_STROKE);

        //ShowGameWin();
    }

    void DoClickBtnColorInput()
    {
        isFirstUseColorInput = false;
        uiWriteController.uiColorInput.UpdateInitColor(colorWord);
        uiWriteController.uiColorInput.gameObject.SetActive(!uiWriteController.uiColorInput.gameObject.activeSelf);
        uiWriteController.uiColorInput.ColorNow = colorWord;
        uiWriteController.uiColorInput.UpdateColorNow();
    }

    void DoBtnSave()
    {
        WordItemInfo info = GameLevelParse.main.GetItemInfo();
        // DBWordItemInfo dbInfo = new DBWordItemInfo();
        info.dbInfo.id = "id_free_write_" + Common.GetCurrentTimeMs();
        // info.dbInfo = dbInfo;
        string filePath = GameLevelParse.main.GetSavePath(info);
        info.dbInfo.filesave = filePath;
        uiWriteController.uiWordWrite.SaveImage(filePath);

        if (info == null)
        {
            return;
        }
        DBHistory.mainFreeWrite.AddItem(info.dbInfo);
    }
    public void OnClickBtnSave()
    {
        if (uiWriteController.uiWordWrite.isHasPaint)
        {
            ShowSaveAlert();
        }

    }
    public void DoBtnDelete()
    {
        uiWriteController.uiWordWrite.ClearAll();
    }
    public void OnClickBtnDelete()
    {

        if (uiWriteController.uiWordWrite.isHasPaint)
        {
            ShowDeleteAlert();
        }

    }
    public void OnClickBtnColorInput()
    {
        if (Application.isEditor)
        {
            DoClickBtnColorInput();
            return;
        }
        if (isFirstUseColorInput)
        {
            ShowFirstUseAlert();
        }
        else
        {
            DoClickBtnColorInput();
        }
    }

    public void OnClickBtnColorBoard()
    {
        uiWriteController.uiColorBoard.gameObject.SetActive(!uiWriteController.uiColorBoard.gameObject.activeSelf);
    }

    public void OnClickBtnLineSetting()
    {
        uiWriteController.uiLineSetting.gameObject.SetActive(true);
    }
    public void OnClickBtnShare()
    {

    }
}
