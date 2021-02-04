using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIHowPlayFlowerPage0 : UIHowPlayFlowerPage
{
    public GameObject objWord;
    public UIItemFlower uiItemFlowerPrefab;
    public int row = 7;
    public int col = 7;
    public List<UIItemFlower> listItem;

    LayOutGrid lygrid;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        base.Awake();
        lygrid = objWord.GetComponent<LayOutGrid>();
        listItem = new List<UIItemFlower>();

        row = 2;
        col = 2;
        lygrid.row = row;
        lygrid.col = col;
        UpdateItem();
    }
    void Start()
    {
        LayOut();
    }

    public override void LayOut()
    {
        base.LayOut();
    }

    public void UpdateItem()
    {
        List<object> listPos = LevelParseIdiomFlower.main.listPosition;
        int idx_pos = Random.Range(0, listPos.Count);
        //idx_pos = 0;
        PositionInfo infoPos = listPos[idx_pos] as PositionInfo;

        lygrid.row = row;
        lygrid.col = col;
        int level = LevelManager.main.gameLevel;
        CaiCaiLeItemInfo info = GameLevelParse.main.listGuanka[level] as CaiCaiLeItemInfo;
        int idx = 0;
        for (int i = 0; i < 1; i++)
        {
            string strIdiom = info.listIdiom[i];
            for (int j = 0; j < strIdiom.Length; j++)
            {
                UIItemFlower ui = (UIItemFlower)GameObject.Instantiate(uiItemFlowerPrefab);
                ui.transform.SetParent(objWord.transform);
                ui.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                UIViewController.ClonePrefabRectTransform(uiItemFlowerPrefab.gameObject, ui.gameObject);
                RowColInfo infoRowCol = infoPos.listRowCol[idx];
                ui.indexRow = infoRowCol.row;
                ui.indexCol = infoRowCol.col;
                ui.index = idx;
                ui.isAnswerItem = false;
                ui.status = UIItemFlower.Status.NORMAL;
                ui.UpdateItem(strIdiom.Substring(j, 1));
                listItem.Add(ui);
                idx++;
            }

        }

        LayOut();

    }

    public override void OnPageExit()
    {

    }
    public override void OnPageEnter()
    {

    }
}
