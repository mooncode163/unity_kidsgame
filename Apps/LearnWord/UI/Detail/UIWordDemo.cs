using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OldMoatGames;
public class UIWordDemo : UIViewPop
{
    public Button btnClose;
    public UIWordSVG uiWordSVG;
    WordItemInfo infoWord;
    public GameObject objStatusBar;
    public UIWordStatusItem uiWordStatusItemPrefab;

    public List<UIWordStatusItem> listItem = new List<UIWordStatusItem>();
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        uiWordSVG.colorWord = Color.gray;
        uiWordSVG.type = UIWordSVG.Type.DEMO;
        uiWordSVG.callBackAnimate = OnUIWordSVGAnimate;
    }
    // Use this for initialization
    void Start()
    {
        uiWordSVG.LoadWord(infoWord);
        // AppSceneBase.main.AddObjToMainWorld(uiWordSVG.gameObject);
        // uiWordSVG.gameObject.transform.localPosition = new Vector3(0, 0, 0);
    }


    public void Updateitem(WordItemInfo info)
    {
        GameLevelParse.main.ParseItem(info);
        infoWord = info;
        listItem.Clear();
        for (int i = 0; i < info.listBiHuaName.Count; i++)
        {
            UIWordStatusItem ui = (UIWordStatusItem)GameObject.Instantiate(uiWordStatusItemPrefab);
            ui.transform.SetParent(objStatusBar.transform);
            ui.index = i;
            UIViewController.ClonePrefabRectTransform(uiWordStatusItemPrefab.gameObject, ui.gameObject);
            ui.UpdateItem(info);
            listItem.Add(ui);
        }
        LayOut();

    }
    public void OnClickBtnClose()
    {
        this.Close();

    }

    public void OnUIWordSVGAnimate(UIWordSVG ui, int index)
    {
        for (int i = 0; i < listItem.Count; i++)
        {
            UIWordStatusItem item = listItem[i];
            item.SetStatus((i == index) ? true : false);
        }
        Debug.Log("OnUIWordSVGAnimate index="+index);
        TTS.main.Speak(infoWord.listBiHuaName[index]);
    }
}
