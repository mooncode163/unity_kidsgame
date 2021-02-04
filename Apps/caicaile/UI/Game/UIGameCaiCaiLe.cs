using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIGameCaiCaiLe : UIGameBase, IPopViewControllerDelegate, IUIWordBoardDelegate, IUIWordContentBaseDelegate, IUIWordBarDelegate
{
    public GameObject objTopBar;
    public Button btnHelp;
    public Button btnTips;
    public Button btnRetry;
    public RawImage imageBg;
    public GameObject objLayouBtn;
    public Text textTitle;
    public Image imageTitle;

    public UIWordFillBox uiWordFillBoxPrefab;
    public UIWordFlower uiWordFlowerPrefab;
    public UIWordPlacePoem uiWordPlacePoemPrefab;
    public UIWordImageText uiWordImageTextPrefab;
    public UIWordMusic uiWordMusicPrefab;
    UIWordContentBase uiWordContent;

    public GameObject objGoldBar;
    public Image imageGoldBg;
    public Text textGold;
    public UIShop uiShopPrefab;
    public UIWordBoard uiWordBoard;
    public UIWordBar uiWordBar;
    string strPlace;
    float goldBaroffsetYNormal;

    GameBase gameBase;
    static public Language languageWord;

    int rowWordBoard = 3;
    int colWordBoard = 8;

    void Awake()
    {
        LoadPrefab();
        CaiCaiLeItemInfo info = GameLevelParse.main.GetItemInfo();
        gameBase = this.gameObject.AddComponent<GameBase>();
        if (gameBase == null)
        {
            Debug.Log("gameBase is null");
        }
        UpdateLanguageWord();
        btnTips.gameObject.SetActive(Config.main.isHaveShop);

        if (info.gameType == GameRes.GAME_TYPE_TEXT)
        {
            btnRetry.gameObject.SetActive(true);
        }
        else
        {
            btnRetry.gameObject.SetActive(false);
        }

        btnHelp.gameObject.SetActive(false);
        if (Common.appKeyName == GameRes.GAME_IdiomConnect)
        {
            btnTips.gameObject.SetActive(true);
            btnRetry.gameObject.SetActive(true);
            btnHelp.gameObject.SetActive(true);
        }



        RectTransform rctran = objTopBar.GetComponent<RectTransform>();
        //bgs

        TextureUtil.UpdateRawImageTexture(imageBg, AppRes.IMAGE_GAME_BG, true);
        if (objGoldBar != null)
        {
            RectTransform rctranGold = objGoldBar.GetComponent<RectTransform>();
            goldBaroffsetYNormal = rctranGold.offsetMax.y;
            objGoldBar.SetActive(AppVersion.appCheckHasFinished);
            if (!Config.main.isHaveShop)
            {
                objGoldBar.SetActive(false);
            }
        }

        uiWordBar.iDelegate = this;
        //uiWordBoard.wordBar = uiWordBar;
        uiWordBar.callbackGameFinish = OnGameWinFinish;
        uiWordBar.callbackGold = OnNotEnoughGold;

        uiWordBoard.iDelegate = this;

        LanguageManager.main.UpdateLanguage(LevelManager.main.placeLevel);
        UpdateLanguage();
        UpdateBtnMusic();



        Common.SetButtonText(btnHelp, Language.main.GetString("STR_BTN_HELP"), 64);
        Common.SetButtonText(btnTips, Language.main.GetString("STR_BTN_TIPS"), 64);
        Common.SetButtonText(btnRetry, Language.main.GetString("STR_BTN_Retry"), 64);


        // Common.GetButtonText(btnTips).color = GameRes.main.colorTitle;
        // Common.GetButtonText(btnRetry).color = GameRes.main.colorTitle;

    }
    // Use this for initialization
    void Start()
    {
        LayOut();
        UpdateGuankaLevel(LevelManager.main.gameLevel);
        // OnGameWinFinish(uiWordBar, false);
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void UpdateLanguageWord()
    {
        ItemInfo info = LevelManager.main.GetPlaceItemInfo(LevelManager.main.placeLevel);
        string strlan = CloudRes.main.rootPathGameRes + "/language/" + info.language + ".csv";
        languageWord = new Language();
        languageWord.Init(strlan);
        languageWord.SetLanguage(SystemLanguage.Chinese);

    }

    public override void UpdateGuankaLevel(int level)
    {
        base.UpdateGuankaLevel(level);
        AppSceneBase.main.ClearMainWorld();
        CaiCaiLeItemInfo info = GameLevelParse.main.GetItemInfo();
        GameLevelParse.main.ParseItem(info);

        if (uiWordContent != null)
        {
            DestroyImmediate(uiWordContent.gameObject);
        }
        switch (info.gameType)
        {
            case GameRes.GAME_TYPE_CONNECT:
                {
                    uiWordContent = (UIWordContentBase)GameObject.Instantiate(uiWordFillBoxPrefab);
                }
                break;
            case GameRes.GAME_TYPE_PLACE:
                {
                    uiWordContent = (UIWordContentBase)GameObject.Instantiate(uiWordPlacePoemPrefab);
                    uiWordBoard.gameObject.SetActive(false);
                    uiWordBar.gameObject.SetActive(false);
                    objLayouBtn.SetActive(false);
                }
                break;
            case GameRes.GAME_TYPE_IMAGE:
            case GameRes.GAME_TYPE_TEXT:
            case GameRes.GAME_TYPE_IMAGE_TEXT:
                {
                    uiWordContent = (UIWordContentBase)GameObject.Instantiate(uiWordImageTextPrefab);
                }
                break;
            case GameRes.GAME_TYPE_Music:
                {
                    uiWordContent = (UIWordContentBase)GameObject.Instantiate(uiWordMusicPrefab);
                    uiWordBoard.gameObject.SetActive(false);
                    uiWordBar.gameObject.SetActive(false);
                    objLayouBtn.SetActive(false);
                    objTopBar.SetActive(false);
                }
                break;


            case GameRes.GAME_TYPE_FLOWER:
                {
                    uiWordContent = (UIWordContentBase)GameObject.Instantiate(uiWordFlowerPrefab);
                    uiWordBoard.gameObject.SetActive(false);
                    uiWordBar.gameObject.SetActive(false);
                    objLayouBtn.SetActive(false);
                }
                break;


            default:
                break;
        }
        if (uiWordContent != null)
        {
            uiWordContent.transform.SetParent(this.transform);
            uiWordContent.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            UIViewController.ClonePrefabRectTransform(uiWordFillBoxPrefab.gameObject, uiWordContent.gameObject);
            uiWordContent.iDelegate = this;
            uiWordContent.infoItem = info;
            uiWordContent.UpdateGuankaLevel(level);
            if (uiWordContent.objTopBar == null)
            {
                uiWordContent.objTopBar = objTopBar;
            }
        }
        // ShowAdInsert(GAME_AD_INSERT_SHOW_STEP,true);
        if (gameBase != null)
        {
            if (gameBase.GetGameItemStatus(info) == GameBase.GAME_STATUS_UN_START)
            {
                gameBase.SetGameItemStatus(info, GameBase.GAME_STATUS_PLAY);
            }

        }

        //  TextureUtil.UpdateImageTexture(imagePicBoard, "App/UI/Game/BoardPic", true);

        objLayouBtn.transform.SetAsLastSibling();
        objTopBar.transform.SetAsLastSibling();
        ItemInfo infoPlace = LevelManager.main.GetPlaceItemInfo(LevelManager.main.placeLevel);
        switch (info.gameType)
        {
            case GameRes.GAME_TYPE_IMAGE:
                {
                    uiWordBar.gameObject.SetActive(true);
                }
                break;
            case GameRes.GAME_TYPE_IMAGE_TEXT:
                {
                    uiWordBar.gameObject.SetActive(true);
                }
                break;
            case GameRes.GAME_TYPE_TEXT:
                {
                    uiWordBar.gameObject.SetActive(false);
                    if (Common.appKeyName == GameRes.GAME_RIDDLE)
                    {
                        uiWordBar.gameObject.SetActive(true);
                    }
                    else
                    {
                        if (infoPlace.id == GameRes.GAME_RIDDLE)
                        {
                            uiWordBar.gameObject.SetActive(true);
                        }
                    }
                }
                break;
            case GameRes.GAME_TYPE_CONNECT:
                {
                    uiWordBar.gameObject.SetActive(false);
                }
                break;

            case GameRes.GAME_TYPE_PLACE:
                {
                    objTopBar.SetActive(true);
                }
                break;


        }
        UpdateWord();
        UpdateTitle();
        UpdateGold();
        LayOut();

        OnUIDidFinish();

        ShowHowToPlay();
    }

    void LoadPrefab()
    {
        {
            string strPrefab = "AppCommon/Prefab/Game/PlacePoem/UIWordPlacePoem";
            if (Device.isLandscape)
            {
                strPrefab = "AppCommon/Prefab/Game/PlacePoem/UIWordPlacePoem_heng";
            }
            GameObject obj = PrefabCache.main.Load(strPrefab);

            if (obj != null)
            {
                uiWordPlacePoemPrefab = obj.GetComponent<UIWordPlacePoem>();
            }
        }
        {
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/Game/UIWordFillBox");
            if (obj != null)
            {
                uiWordFillBoxPrefab = obj.GetComponent<UIWordFillBox>();
            }
        }


        {
            string strPrefab = "AppCommon/Prefab/Game/Flower/UIWordFlower";
            if (Device.isLandscape)
            {
                strPrefab = "AppCommon/Prefab/Game/Flower/UIWordFlower_heng";
            }
            GameObject obj = PrefabCache.main.Load(strPrefab);
            if (obj != null)
            {
                uiWordFlowerPrefab = obj.GetComponent<UIWordFlower>();
            }
        }

        {
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/Game/UIWordImageText");
            if (obj != null)
            {
                uiWordImageTextPrefab = obj.GetComponent<UIWordImageText>();
            }
        }
        {
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/Game/Music/UIWordMusic");
            if (obj != null)
            {
                uiWordMusicPrefab = obj.GetComponent<UIWordMusic>();
            }
        }

    }

    public override void LayOut()
    {
        base.LayOut();
        float x = 0, y = 0, w = 0, h = 0;
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        CaiCaiLeItemInfo info = GameLevelParse.main.GetItemInfo();
        if ((info.gameType == GameRes.GAME_TYPE_PLACE) || (info.gameType == GameRes.GAME_TYPE_FLOWER))
        {
            if (uiWordContent != null)
            {
                uiWordContent.LayOut();
            }
            return;
        }

        float ratio = 1f;
        float topbarHeightCanvas = 160;

        Rect rectImage = Rect.zero;
        //game pic
        {

            ratio = 0.9f;
            if (Device.isLandscape)
            {
                w = (this.frame.width / 2) * ratio;
                x = -this.frame.width / 4 - w / 2;
                if (Common.appKeyName == GameRes.GAME_XIEHOUYU)
                {
                    h = (this.frame.height - topbarHeightCanvas * 3) * ratio;
                    float y1 = -sizeCanvas.y / 2 + topbarHeightCanvas * 2;
                    float y2 = sizeCanvas.y / 2 - topbarHeightCanvas;
                    y = (y1 + y2) / 2 - h / 2;
                }
                else
                {

                    h = (this.frame.height - topbarHeightCanvas * 2) * ratio;
                    y = 0 - h / 2;
                }


            }
            else
            {

                // w = this.frame.width - topbarHeightCanvas * 2;
                w = this.frame.width;
                h = (this.frame.height / 2 - topbarHeightCanvas * 2);
                if (Common.appKeyName == GameRes.GAME_XIEHOUYU)
                {
                    h = (this.frame.height / 2 - topbarHeightCanvas * 3);
                }
                y = this.frame.height / 4 - h / 2;
                x = 0 - w / 2;
            }

            if (Common.appKeyName == GameRes.GAME_IdiomConnect)
            {
                if (Device.isLandscape)
                {
                }
                else
                {
                    w = this.frame.width * 0.9f;
                    h = (this.frame.height / 2);
                    y = this.frame.height / 2 - topbarHeightCanvas - h;
                    x = 0 - w / 2;
                }

            }



            rectImage = new Rect(x, y, w, h);
            Debug.Log("rectImage =" + rectImage);

            if (uiWordContent != null)
            {
                RectTransform rctran = uiWordContent.GetComponent<RectTransform>();
                UIWordFillBox ui = uiWordContent as UIWordFillBox;
                if (ui != null)
                {
                    w = Mathf.Min(w, h);
                    h = w;
                }
                rctran.sizeDelta = new Vector2(w, h);
                rctran.anchoredPosition = rectImage.center;
                uiWordContent.LayOut();
            }
        }



        //wordboard
        {




            RectTransform rctran = uiWordBoard.GetComponent<RectTransform>();
            GridLayoutGroup gridLayout = uiWordBoard.GetComponent<GridLayoutGroup>();
            Vector2 cellSize = gridLayout.cellSize;
            Vector2 space = gridLayout.spacing;

            if ((info.gameType == GameRes.GAME_TYPE_TEXT) && (Common.appKeyName != GameRes.GAME_RIDDLE))
            {
                gridLayout.cellSize = new Vector2(160, 160);
                gridLayout.spacing = new Vector2(16, 16);

                cellSize = gridLayout.cellSize;
                space = gridLayout.spacing;
                rowWordBoard = 2;
                colWordBoard = 4;

            }
            else
            {
                if (Device.isLandscape)
                {
                    rowWordBoard = 3;
                    colWordBoard = 8;
                }
                else
                {
                    rowWordBoard = 3;
                    colWordBoard = 8;
                    w = (cellSize.x + space.x) * colWordBoard;
                    if (w > this.frame.width)
                    {
                        rowWordBoard = 4;
                        colWordBoard = 6;
                    }

                }
            }

            if (Device.isLandscape)
            {
                float x1 = rectImage.center.x + rectImage.size.x / 2;
                float x2 = this.frame.width / 2;
                x = (x1 + x2) / 2;
                y = -this.frame.height / 4;

                //6x4
                w = (cellSize.x + space.x) * colWordBoard;
                h = (cellSize.y + space.y) * rowWordBoard;
            }
            else
            {
                x = 0;
                y = -this.frame.height / 4;


                //8x3
                w = (cellSize.x + space.x) * colWordBoard;
                h = (cellSize.y + space.y) * rowWordBoard;

            }




            float y_bottom_limite = -sizeCanvas.y / 2 + topbarHeightCanvas + 16;
            if ((y - h / 2) < y_bottom_limite)
            {
                y = y_bottom_limite + h / 2;
            }
            rctran.sizeDelta = new Vector2(w, h);
            rctran.anchoredPosition = new Vector2(x, y);

            uiWordBoard.row = rowWordBoard;
            uiWordBoard.col = colWordBoard;

        }

        RectTransform rctranWordBar = uiWordBar.GetComponent<RectTransform>();
        RectTransform rctranBoard = uiWordBoard.GetComponent<RectTransform>();
        //wordbar
        {
            ratio = 0.9f;
            float offsetw = topbarHeightCanvas;
            if (Common.appKeyName == GameRes.GAME_Image)
            {
                offsetw = 0;
            }

            RectTransform rctran = uiWordBar.GetComponent<RectTransform>();
            if (Device.isLandscape)
            {
                x = rctranBoard.anchoredPosition.x;
                float y1 = rctranBoard.anchoredPosition.y + rctranBoard.rect.height / 2;
                float y2 = this.frame.height / 2 - topbarHeightCanvas;
                y = (y1 + y2) / 2;
                w = (this.frame.width / 2 - offsetw * 2) * ratio;
            }
            else
            {
                w = (this.frame.width - offsetw * 2) * ratio;
                x = 0;
                y = 0;

            }


            h = topbarHeightCanvas;
            rctran.sizeDelta = new Vector2(w, h);
            rctran.anchoredPosition = new Vector2(x, y);
        }

        //leftbtn
        {
            RectTransform rctran = objLayouBtn.GetComponent<RectTransform>();
            LayOutGrid lg = objLayouBtn.GetComponent<LayOutGrid>();
            lg.enableHide = false;

            if (info.gameType == GameRes.GAME_TYPE_CONNECT)
            {
                //横排显示
                w = rctranBoard.rect.size.x;
                h = topbarHeightCanvas;
                float y1 = rctranBoard.anchoredPosition.y + rctranBoard.rect.height / 2;
                float y2 = rectImage.center.y - rectImage.size.y / 2;
                if (Device.isLandscape)
                {
                    y2 = this.frame.height / 2 - topbarHeightCanvas;
                }
                y = (y1 + y2) / 2;
                x = rctranBoard.anchoredPosition.x;

                if (!Device.isLandscape)
                {

                    lg.col = lg.GetChildCount(lg.enableHide);
                    lg.row = 1;
                }
                else
                {
                    lg.col = 1;
                    lg.row = lg.GetChildCount(lg.enableHide);
                }
            }
            else
            {

                {
                    // 
                    h = topbarHeightCanvas;
                    w = rectImage.size.x;
                    x = rectImage.center.x;
                    y = rectImage.center.y - rectImage.size.y / 2 + h / 2 + 16;
                    lg.row = 1;
                    lg.col = lg.GetChildCount(lg.enableHide);
                }

            }

            rctran.sizeDelta = new Vector2(w, h);
            rctran.anchoredPosition = new Vector2(x, y);
            if (lg != null)
            {
                lg.LayOut();
            }
        }


    }

    public void UpdateGold()
    {

        string str = Language.main.GetString("STR_GOLD") + ":" + Common.gold.ToString();
        textGold.text = str;
        int fontsize = textGold.fontSize;
        float str_w = Common.GetStringLength(str, AppString.STR_FONT_NAME, fontsize);
        RectTransform rctran = imageGoldBg.transform as RectTransform;
        Vector2 sizeDelta = rctran.sizeDelta;

        sizeDelta.x = str_w + fontsize;
        rctran.sizeDelta = sizeDelta;
    }
    void UpdateTitle()
    {
        int idx = LevelManager.main.gameLevel + 1;
        textTitle.text = idx.ToString();
        if (Common.appKeyName == GameRes.GAME_IdiomFlower)
        {
            if (Device.isLandscape)
            {
                imageTitle.gameObject.SetActive(false);
                textTitle.gameObject.SetActive(false);
            }
        }
    }


    void UpdateWord()
    {
        CaiCaiLeItemInfo info = GameLevelParse.main.GetItemInfo();
        if (uiWordContent != null)
        {
            uiWordContent.UpdateWord();
        }
        //先计算行列数
        LayOut();
        uiWordBoard.InitItem();
        string strBoard = GameAnswer.main.GetWordBoardString(info, uiWordBoard.row, uiWordBoard.col);
        uiWordBoard.UpdateItem(info, strBoard);
        uiWordBar.UpdateItem(info);
    }

    public void ShowShop()
    {
        ShopViewController.main.Show(null, this);
    }


    public bool CheckAllAnswerFinish()
    {
        bool ret = false;
        if (uiWordContent != null)
        {
            ret = uiWordContent.CheckAllAnswerFinish();
        }
        return ret;
    }

    public void OnNotEnoughGold(UIWordBar bar, bool isUpdate)
    {
        if (isUpdate)
        {
            UpdateGold();
        }
        else
        {
            string title = Language.main.GetString(AppString.STR_UIVIEWALERT_TITLE_NOT_ENOUGH_GOLD);
            string msg = Language.main.GetString(AppString.STR_UIVIEWALERT_MSG_NOT_ENOUGH_GOLD);
            string yes = Language.main.GetString(AppString.STR_UIVIEWALERT_YES_NOT_ENOUGH_GOLD);
            string no = Language.main.GetString(AppString.STR_UIVIEWALERT_NO_NOT_ENOUGH_GOLD);

            ViewAlertManager.main.ShowFull(title, msg, yes, no, false, STR_KEYNAME_VIEWALERT_GOLD, OnUIViewAlertFinished);
        }
    }

    public void UIWordBoardDidClick(UIWordBoard bd, UIWordItem item)
    {

        CaiCaiLeItemInfo infoGuanka = GameLevelParse.main.GetItemInfo();
        Debug.Log("UIWordBoardDidClick infoGuanka.gameType =" + infoGuanka.gameType);
        // if (infoGuanka.gameType == GameRes.GAME_TYPE_TEXT)
        // {
        //     uiWordContent.OnAddWord(item.wordDisplay);
        //     item.ShowContent(false);
        //     if (uiWordContent.CheckAllFill())
        //     {
        //         if (CheckAllAnswerFinish())
        //         {
        //             OnGameWinFinish(uiWordBar, false);
        //         }
        //         else
        //         {
        //             OnGameWinFinish(uiWordBar, true);
        //         }
        //     }
        // }
        if (uiWordBar.gameObject.activeSelf)
        {
            if (!uiWordBar.CheckAllFill())
            {
                uiWordBar.AddWord(item.wordDisplay);
                item.ShowContent(false);
            }
        }
        else
        {
            if (uiWordContent != null)
            {
                uiWordContent.OnAddWord(item.wordDisplay);
                item.ShowContent(false);
                bool ret = uiWordContent.CheckAllAnswerFinish();
                // Debug.Log("CheckAllAnswer ret=" + ret);
                // if (ret)
                // {
                //     OnGameWinFinish(uiWordBar, false);
                // }
                if (uiWordContent.CheckAllFill())
                {
                    if (CheckAllAnswerFinish())
                    {
                        OnGameWinFinish(uiWordBar, false);
                    }
                    else
                    {
                        OnGameWinFinish(uiWordBar, true);
                    }
                }
            }

        }

    }
    public void OnGameWinFinish(UIWordBar bar, bool isFail)
    {
        ItemInfo infoPlace = LevelManager.main.GetPlaceItemInfo(LevelManager.main.placeLevel);
        CaiCaiLeItemInfo info = GameLevelParse.main.GetItemInfo();
        //show game win
        if (isFail)
        {
            PopUpManager.main.Show<UIGameFail>("App/Prefab/Game/UIGameFail");
        }
        else
        {
            Debug.Log("caicaile OnGameWin");
            LevelManager.main.gameLevelFinish = LevelManager.main.gameLevel;
            //gameEndParticle.Play();
            //  Invoke("ShowGameWin", 1f);
            OnGameWinBase();

            if (gameBase != null)
            {
                Debug.Log("caicaile OnGameWin GAME_STATUS_FINISH+info.id=" + info.id);
                gameBase.SetGameItemStatus(info, GameBase.GAME_STATUS_FINISH);
            }

            string strPrefab = "";
            // strPrefab = "AppCommon/Prefab/Game/GameFinish/UIGameWin" + infoPlace.gameId;
            string key = "UIGameWin" + infoPlace.gameId;
            if (Common.BlankString(ConfigPrefab.main.GetPrefab(key)))
            {
                key = "UIGameWin";
            }
            strPrefab = ConfigPrefab.main.GetPrefab(key);
            // switch (infoPlace.gameType)
            // {
            //     case GameRes.GAME_TYPE_CONNECT:
            //         {
            //             strPrefab = "App/Prefab/Game/UIGameWinIdiomConnect";
            //         }
            //         break;
            //     case GameRes.GAME_TYPE_FLOWER:
            //         {
            //             strPrefab = "App/Prefab/Game/UIGameWinFlower";
            //         }
            //         break;

            //     default:
            //         {
            //             strPrefab = "App/Prefab/Game/UIGameWin";
            //             break;
            //         }
            // }

            // if (Common.appKeyName == GameRes.GAME_Guess)
            // {
            //     strPrefab = "AppCommon/Prefab/Game/GameFinish/UIGameWin" + infoPlace.id;
            // }

            Debug.Log("game win strPrefab =" + strPrefab + " infoPlace.gameId=" + infoPlace.gameId);
            PopUpManager.main.Show<UIGameWinBase>(strPrefab, popup =>
          {
              Debug.Log("UIGameWinBase Open ");
              popup.UpdateItem(info);

          }, popup =>
          {


          });
        }

    }

    void ShowHowToPlay()
    {
        string key = "KEY_FIRST_RUN_HOT_TO_PLAY";
        bool isFirst = Common.GetKeyForFirstRun(key);
        // isFirst = true;
        if (Application.isEditor)
        {
            return;
        }
        if (Common.appKeyName == GameRes.GAME_IDIOM)
        {
            return;
        }
        if (Common.appKeyName == GameRes.GAME_RIDDLE)
        {
            return;
        }
        if (Common.appKeyName == GameRes.GAME_PlacePoem)
        {
            return;
        }
        if (isFirst)
        {
            if (Common.appKeyName == GameRes.GAME_IdiomFlower)
            {
                HowPlayFlowerViewController.main.Show(null, null);
            }
            else
            {
                HowToPlayViewController.main.Show2(null, null);
            }
            Common.SetKeyForFirstRun(key, false);
        }

    }
    void ShowGameWin()
    {
        //GameScene.ShowAdInsert(100);

        string title = Language.main.GetString(AppString.STR_UIVIEWALERT_TITLE_GAME_FINISH);
        string msg = Language.main.GetString(AppString.STR_UIVIEWALERT_MSG_GAME_FINISH);
        string yes = Language.main.GetString(AppString.STR_UIVIEWALERT_YES_GAME_FINISH);
        string no = Language.main.GetString(AppString.STR_UIVIEWALERT_NO_GAME_FINISH);

        ViewAlertManager.main.ShowFull(title, msg, yes, no, false, STR_KEYNAME_VIEWALERT_GAME_FINISH, OnUIViewAlertFinished);

        CaiCaiLeItemInfo info = GameLevelParse.main.GetItemInfo();
        string str = languageGame.GetString(info.id);
        TTS.main.Speak(str);
    }
    void OnUIViewAlertFinished(UIViewAlert alert, bool isYes)
    {

        if (STR_KEYNAME_VIEWALERT_GAME_FINISH == alert.keyName)
        {
            if (isYes)
            {
                LevelManager.main.GotoNextLevel();
            }
        }

        if (STR_KEYNAME_VIEWALERT_GOLD == alert.keyName)
        {
            if (isYes)
            {
                ShowShop();
            }
        }



    }

    public void UIWordContentBaseDidBackWord(UIWordContentBase ui, string word)
    {
        uiWordBoard.BackWord(word);
    }
    public void UIWordContentBaseDidTipsWord(UIWordContentBase ui, string word)
    {
        uiWordBoard.HideWord(word);
    }
    public void UIWordContentBaseDidAdd(UIWordContentBase ui, string word)
    {
        Debug.Log("UIWordContentBaseDidAdd ");

    }
    public void UIWordContentBaseDidGameFinish(UIWordContentBase ui, bool isFail)
    {
        OnGameWinFinish(uiWordBar, isFail);
    }

    public void UIWordBarDidBackWord(UIWordBar ui, string word)
    {
        uiWordBoard.BackWord(word);
    }
    public void UIWordBarDidTipsWord(UIWordBar ui, string word)
    {
        uiWordBoard.HideWord(word);
    }

    public void OnClickBtnHelp()
    {
        HowToPlayViewController.main.Show(null, null);
    }
    public void OnClickBtnRetry()
    {
        UpdateGuankaLevel(LevelManager.main.gameLevel);
    }
    public void OnClickBtnTips()
    {

        if (Common.gold <= 0)
        {
            OnNotEnoughGold(uiWordBar, false);
            return;
        }

        //if (isonlytext && (Common.appKeyName != GameRes.GAME_RIDDLE))
        if (!uiWordBar.gameObject.activeSelf)
        {


            if (uiWordContent != null)
            {
                uiWordContent.OnTips();
            }

            Common.gold--;
            if (Common.gold < 0)
            {
                Common.gold = 0;
            }
            OnNotEnoughGold(uiWordBar, true);
            if (CheckAllAnswerFinish())
            {
                OnGameWinFinish(uiWordBar, false);
            }
        }
        else
        {
            if (uiWordBar != null)
            {
                uiWordBar.OnTips();
            }
        }

    }


    public void OnClickGold()
    {
        ShowShop();
    }

    public override void OnClickBtnBack()
    {
        base.OnClickBtnBack();
    }

    public void OnPopViewControllerDidClose(PopViewController controller)
    {
        UpdateGold();
    }
}
