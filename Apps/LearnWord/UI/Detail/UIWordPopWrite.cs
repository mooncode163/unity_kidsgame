using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;
public class UIWordPopWrite : UIViewPop, IUIWordWriteDelegate
{
    public UIWordWrite uiWordWrite;
    List<object> listItem;
    public UIText textTitle;
    public string gameId;
    public UIButton btnModeAll;
    public UIButton btnModeOne;
    public UIButton btnModeNone;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        LoadPrefab();
        SelectBtn(btnModeAll);
        uiWordWrite.iDelegate = this;

    }

    // Use this for initialization
    void Start()
    {

        PopUpManager.main.ShowPannl(false);
        AppSceneBase.main.ShowRootViewController(false);
        LayOut();
        Invoke("GotoModeDelay", 0.2f);
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

    void OnDestroy()
    {
        AppSceneBase.main.ShowRootViewController(true);
        // AppSceneBase.main.ClearMainWorld();
    }


    public override void LayOut()
    {
        base.LayOut();
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;


    }
    public void GotoModeDelay()
    {
        uiWordWrite.GotoMode(UIWordWrite.Mode.ALL_STROKE);
        OnUIDidFinish();
    }
    public void SetBtnGray(UIButton btn)
    {
        btn.imageBg.UpdateImageByKey("BtnBgGrey");
        btn.textTitle.color = new Color32(100, 100, 100, 255);
    }

    public void SelectBtn(UIButton btn)
    {
        SetBtnGray(btnModeAll);
        SetBtnGray(btnModeOne);
        SetBtnGray(btnModeNone);

        btn.imageBg.UpdateImageByKey("BtnBg");
        btn.textTitle.color = Color.white;

    }

    public void OnUIWordWriteDidWriteFinish(UIWordWrite ui)
    {
        this.Close();
        WriteFinishViewController.main.Show(null, null);
    }


    public void OnUIWordWriteDidMode(UIWordWrite ui, UIWordWrite.Mode md)
    {
        switch (md)
        {
            case UIWordWrite.Mode.ALL_STROKE:
                {
                    SelectBtn(btnModeAll);
                }
                break;
            case UIWordWrite.Mode.ONE_STROKE:
                {
                    SelectBtn(btnModeOne);
                }
                break;
            case UIWordWrite.Mode.NO_STROKE:
                {
                    SelectBtn(btnModeNone);
                }
                break;
        }
    }
    public void UpdateItem(WordItemInfo info)
    {
        uiWordWrite.UpdateItem(info);
    }
    public void OnBtnClickBack()
    {
        this.Close();
    }

    public void OnBtnClickDeleteAll()
    {
        DBLove.main.ClearDB();
    }
    public void OnBtnClickModeAll()
    {
        uiWordWrite.GotoMode(UIWordWrite.Mode.ALL_STROKE);
    }
    public void OnBtnClickModeOne()
    {
        uiWordWrite.GotoMode(UIWordWrite.Mode.ONE_STROKE);
    }
    public void OnBtnClickModeNone()
    {
        uiWordWrite.GotoMode(UIWordWrite.Mode.NO_STROKE);
    }
    public void OnCellItemDidClick(UICellItemBase item)
    {
        if (item.IsLock())
        {
            return;
        }

    }

    public void OnCellItemDidClickDelete(UILoveCellItem ui)
    {
        WordItemInfo info = listItem[ui.index] as WordItemInfo;
        if (info != null)
        {
        }
    }


}
