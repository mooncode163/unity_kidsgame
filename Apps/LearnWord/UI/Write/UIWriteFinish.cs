using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;
public class UIWriteFinish : UIView
{
    public UIText textTitle;
    public UIImage imageWord;
    public UIButton btnAddLove;
    List<UIButton> listItem = new List<UIButton>();
    WordItemInfo infoGuanka;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        LoadPrefab();


    }

    // Use this for initialization
    void Start()
    {
        infoGuanka = GameLevelParse.main.GetItemInfo();
        imageWord.UpdateImage(infoGuanka.imageBihua);
        textTitle.text = infoGuanka.id;
        UpdateLoveStatus();
        LayOut();
        OnUIDidFinish();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnBtnClickBack();
        }
    }
    void LoadPrefab()
    {

    }

    public void UpdateLoveStatus()
    {
        string strBtn = "";
        DBWordItemInfo dbInfo = infoGuanka.dbInfo as DBWordItemInfo;

        if (DBLoveWord.main.IsItemExistId(infoGuanka.id))
        {
            strBtn = Language.main.GetString("STR_DELETE_LOVE");
        }
        else
        {
            strBtn = Language.main.GetString("STR_ADD_LOVE");
        }
        btnAddLove.textTitle.text = strBtn;
    }

    public override void LayOut()
    {
        base.LayOut();
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;


    }

    public void OnBtnClickBack()
    {
        NaviViewController navi = this.controller.naviController;
        if (navi != null)
        {
            navi.Pop();
        }
    }

    public void OnBtnClickAddLove()
    {
        DBWordItemInfo infoidiom = infoGuanka.dbInfo as DBWordItemInfo;

        if (DBLoveWord.main.IsItemExist(infoidiom))
        {
            DBLoveWord.main.DeleteItem(infoidiom);
        }
        else
        {
            DBLoveWord.main.AddItem(infoidiom);
        }
        UpdateLoveStatus();
    }
}
