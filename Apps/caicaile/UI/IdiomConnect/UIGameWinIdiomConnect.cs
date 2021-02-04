using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIGameWinIdiomConnect : UIGameWinBase
{
    public const string KEY_GAMEWIN_INFO_INTRO = "KEY_GAMEWIN_INFO_INTRO";
    public const string KEY_GAMEWIN_INFO_YUANWEN = "KEY_GAMEWIN_INFO_YUANWEN";

    public const string KEY_GAMEWIN_INFO_TRANSLATION = "KEY_GAMEWIN_INFO_TRANSLATION";
    public const string KEY_GAMEWIN_INFO_JIANSHUANG = "KEY_GAMEWIN_INFO_JIANSHUANG";
    public const string KEY_GAMEWIN_INFO_AUTHOR_INTRO = "KEY_GAMEWIN_INFO_AUTHOR_INTRO";


    public const string KEY_GAMEWIN_INFO_ALBUM = "KEY_GAMEWIN_INFO_ALBUM";


    public Text textTitle;
    public Image imageBg;
    public Image imageHead;
    public Button btnClose;
    public GameObject objIdiom;

    public Button btnFriend;
    public Button btnNext;
    public GameObject objLayoutBtn;

    public UIButtonIdiom uiButtonIdiomPrefab;
    public List<UIButtonIdiom> listIdiom;
    /// <summary>
    /// Unity's Awake method.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        CaiCaiLeItemInfo info = GameLevelParse.main.GetItemInfo();

        //Common.SetButtonText(btnFriend, Language.main.GetString("STR_GameWin_BtnFriend"));
        Common.SetButtonText(btnNext, Language.main.GetString("STR_GameWin_BtnNext"), 0, false);

        textTitle.text = Language.main.GetString("STR_GameWin_TITLE");
        textTitle.color = GameRes.main.colorGameWinTitle;

        UpdateIdiom();

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

    void UpdateIdiom()
    {
        if (listIdiom == null)
        {
            listIdiom = new List<UIButtonIdiom>();
        }
        CaiCaiLeItemInfo info = GameLevelParse.main.GetItemInfo();
        for (int i = 0; i < info.listIdiom.Count; i++)
        {
            CaiCaiLeItemInfo dbInfo = new CaiCaiLeItemInfo();
            dbInfo.title = info.listIdiom[i];
            dbInfo.id = dbInfo.title;
            UIButtonIdiom ui = (UIButtonIdiom)GameObject.Instantiate(uiButtonIdiomPrefab);
            ui.transform.SetParent(objIdiom.transform);
            ui.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            UIViewController.ClonePrefabRectTransform(uiButtonIdiomPrefab.gameObject, ui.gameObject);
            ui.UpdateItem(dbInfo);
            listIdiom.Add(ui);
        }
    }
    public override void LayOut()
    {
        float x = 0, y = 0, w = 0, h = 0;
        float ratio = 0.8f;
        if (Device.isLandscape)
        {
            ratio = 0.8f;
        }

        RectTransform rctranRoot = this.GetComponent<RectTransform>();
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        {

            w = sizeCanvas.x;
            h = sizeCanvas.y;
            rctranRoot.sizeDelta = new Vector2(w, h);

        }
        float w_btns_landscape = 420;
        float space = 32f;
        //textView
        {


        }

        //objLayoutBtn
        {
            RectTransform rctran = objLayoutBtn.GetComponent<RectTransform>();
            // if (Device.isLandscape)
            // {
            //     w = w_btns_landscape;
            //     h = rctranRoot.rect.height;
            //     y = 0;
            //     x = rctranRoot.rect.width / 2 - w / 2 - space;
            // }
            // else
            {
                w = rctranRoot.rect.width;
                h = 160;
                x = 0;
                y = -rctranRoot.rect.height / 2 + h / 2 + space;
            }
            rctran.sizeDelta = new Vector2(w, h);
            rctran.anchoredPosition = new Vector2(x, y);


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

        //objIdiom
        {
            LayOutGrid lg = objIdiom.GetComponent<LayOutGrid>();
            RectTransform rctran = objIdiom.GetComponent<RectTransform>();
            w = rctranRoot.rect.width - 64;
            h = 256;
            x = 0;
            y = 0;
            rctran.sizeDelta = new Vector2(w, h);
            rctran.anchoredPosition = new Vector2(x, y);

            lg.row = 2;
            lg.col = 3;
            if (lg != null)
            {
                lg.LayOut();
            }
        }

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
}
