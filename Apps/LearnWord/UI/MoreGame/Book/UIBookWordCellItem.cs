using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBookWordCellItem : UICellItemBase, IUIWordTextDelegate
{ 
    public UIText textTitle;
    public UIImage imageBg;
    public UIImage imageIcon;
    UIWordText uiPrefab;
    public GameObject objWord;
    List<UIWordText> listItem = new List<UIWordText>();
    ItemInfo infoItem;
    public void Awake()
    {
        base.Awake();
        LoadPrefab();

    }
    void LoadPrefab()
    {
        GameObject obj = (GameObject)PrefabCache.main.LoadByKey("UIWordText");
        if (obj != null)
        {
            uiPrefab = obj.GetComponent<UIWordText>();
        }
    }

    public UIWordText CreateItem(string word)
    {
        UIWordText ui = (UIWordText)GameObject.Instantiate(uiPrefab);
        ui.transform.SetParent(objWord.transform);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
        ui.index = listItem.Count;
        listItem.Add(ui);
        ui.UpdateTitle(word);
        ui.iDelegate = this;
        LayOut();
        return ui;
    }
    public override void UpdateItem(List<object> list)
    {
        ItemInfo info = list[index] as ItemInfo;
        infoItem = info;
        textTitle.text = info.title;
        // textTitle.gameObject.SetActive(false);
        imageBg.UpdateImage(info.pic);
        // imageIcon.gameObject.SetActive(info.isAd);

        for (int i = 0; i < info.detail.Length; i++)
        {
            CreateItem(info.detail.Substring(i, 1));
        }
        LayOut();
    }
    public override bool IsLock()
    {
        return false;//imageBgLock.gameObject.activeSelf;
    }
    public override void LayOut()
    {
        base.LayOut();

    }


    public void OnUIWordTextDidFail(UIWordText ui)
    {

    }
    public void OnUIWordTextDidOK(UIWordText ui)
    {

    }
    public void OnUIWordTextDidClick(UIWordText ui)
    { 
        // Debug.Log("OnCellItemDidClick index=" + infoItem.index + " info.id=" + infoItem.id);
        AudioPlay.main.PlayBtnSound();
         LevelManager.main.gameLevel = LevelParseLearnWord.main.GetLevelByWord(ui.word);
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            if (navi != null)
            {
                DetailViewController.main.word = ui.word;
                navi.Push(DetailViewController.main);

            }
        }
    }

}
