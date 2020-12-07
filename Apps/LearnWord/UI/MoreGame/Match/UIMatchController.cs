using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 认一认
public class UIMatchController : UIView, IUIWordTextDelegate
{
    public UIImage imageWord;
    public UIText textWord;
    public GameObject objWord;
    List<UIWordText> listItem = new List<UIWordText>();

    UIWordText uiPrefab;
    string strAnswer;

    int errorCount = 0;
    int MaxErrorCount = 3;
    /// <summary>
    /// Unity's Awake method.
    /// </summary>
    public void Awake()
    {
        LoadPrefab();
        errorCount = 0;
        for (int i = 0; i < 9; i++)
        {
            CreateItem();
        }
    }

    /// <summary>
    /// Unity's Start method.
    /// </summary>
    public void Start()
    {
        LevelManager.main.gameLevel = 0;
        if(Application.isEditor)
        {
             LevelManager.main.gameLevel = 2;
        }
        UpdateItem(GameLevelParse.main.GetItemInfo());
        LayOut();
        OnUIDidFinish(); 
    }
    void LoadPrefab()
    {
        GameObject obj = (GameObject)PrefabCache.main.LoadByKey("UIWordText");
        if (obj != null)
        {
            uiPrefab = obj.GetComponent<UIWordText>();
        }
    }
    public override void LayOut()
    {
        base.LayOut();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        // LayOut();
    }
    public UIWordText CreateItem()
    {
        UIWordText ui = (UIWordText)GameObject.Instantiate(uiPrefab);
        ui.transform.SetParent(objWord.transform);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
        listItem.Add(ui);
        LayOut();
        return ui;
    }
    public void UpdateItem(WordItemInfo info)
    {
        GameLevelParse.main.ParseItem(info);
        strAnswer = info.dbInfo.id;
        textWord.text = info.dbInfo.pinyin;
        imageWord.UpdateImage(info.pic);
        TTS.main.Speak(textWord.text);
        List<WordItemInfo> listOther = LevelParseLearnWord.main.GetOtherWord(info, listItem.Count - 1);
        listOther.Insert(Random.Range(0, listOther.Count), info);
        int idx = 0;
        foreach (UIWordText ui in listItem)
        {
            WordItemInfo infotmp = listOther[idx];
            ui.textTitle.text = infotmp.id;
            ui.strAnswer = strAnswer;
            ui.iDelegate = this;
            idx++;
        }

        LayOut();

        AdKitCommon.main.ShowAdInsertWithStep(UIGameBase.GAME_AD_INSERT_SHOW_STEP, false);
    }
    public void OnClickBtnBack()
    {
        if (this.controller.naviController != null)
        {
            this.controller.naviController.Pop();
        }
    }

    void OnGameFail()
    {
        WordItemInfo info = GameLevelParse.main.GetItemInfo() as WordItemInfo;
        ItemInfo infoPlace = LevelManager.main.GetPlaceItemInfo(LevelManager.main.placeLevel);
        string strPrefab = "AppCommon/Prefab/Game/GameFinish/UIGameFailMatch";
        PopUpManager.main.Show<UIGameWinBase>(strPrefab, popup =>
        {
            Debug.Log("UIGameWinBase Open ");
            popup.UpdateItem(info);

        }, popup =>
        {


        });
    }
    void OnGameWin()
    {
        LevelManager.main.gameLevelFinish = LevelManager.main.gameLevel;
        WordItemInfo info = GameLevelParse.main.GetItemInfo() as WordItemInfo;
        ItemInfo infoPlace = LevelManager.main.GetPlaceItemInfo(LevelManager.main.placeLevel);
        string strPrefab = ConfigPrefab.main.GetPrefab("UIGameWinPintu");
        Debug.Log("GameWin strPrefab=" + strPrefab);

        PopUpManager.main.Show<UIGameWinBase>(strPrefab, popup =>
        {
            Debug.Log("UIGameWinBase Open ");
            popup.UpdateItem(info);

        }, popup =>
        {


        });
    }
    public void GotoPlayAgain()
    {
        UpdateItem(GameLevelParse.main.GetItemInfo());
    }
    public void GotoNextLevel()
    {
        LevelManager.main.gameLevel++;
        if (LevelManager.main.gameLevel >= LevelManager.main.maxGuankaNum)
        {
            LevelManager.main.gameLevel = 0;
        }
        // AppSceneBase.main.ClearMainWorld();
        UpdateItem(GameLevelParse.main.GetItemInfo());
    }
    public void OnUIWordTextDidFail(UIWordText ui)
    {
        errorCount++;
        if (errorCount >= MaxErrorCount)
        {
            OnGameFail();
        }
    }
    public void OnUIWordTextDidOK(UIWordText ui)
    {
        OnGameWin();
        // LevelManager.main.gameLevel++;
        // if (LevelManager.main.gameLevel >= LevelManager.main.maxGuankaNum)
        // {
        //     LevelManager.main.gameLevel = 0;
        // }
        // UpdateItem(GameLevelParse.main.GetItemInfo());
    }
    public void OnUIWordTextDidClick(UIWordText ui)
    {

    }

}
