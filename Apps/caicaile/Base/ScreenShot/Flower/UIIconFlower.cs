using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIIconFlower : UIShotBase
{
    public UIImage imageBg;
    public UIImage imageHD;
    public UIImage imageBoard;
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
        imageHD.gameObject.SetActive(false);
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
        IconViewController iconctroller = (IconViewController)this.controller;
        if (iconctroller != null)
        {
            if (iconctroller.deviceInfo.isIconHd)
            {
                imageHD.gameObject.SetActive(true);
            }

        }
        LayOut();
        OnUIDidFinish();
    }
    public override void LayOut()
    {
        base.LayOut();

        // for (int i = 0; i < listItem.Count; i++)
        // {
        //     UIItemFlower item = listItem[i];
        //     RectTransform rctran = item.GetComponent<RectTransform>();
        //     float w = rctran.rect.width;
        //     Debug.Log("SetFontSize w =" + w + " sizeDelta=" + rctran.sizeDelta);
        //     item.SetFontSize((int)(w * 0.7f));

        // }
    }

    public void UpdateItem()
    {
        List<object> listPos = LevelParseIdiomFlower.main.listPosition;
        int idx_pos = Random.Range(0, listPos.Count);
        //idx_pos = 0;
        PositionInfo infoPos = listPos[idx_pos] as PositionInfo;

        lygrid.row = row;
        lygrid.col = col;
        int idx = 0;
        for (int i = 0; i < 1; i++)
        {
            string strIdiom = "成语飞花";
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
                if ((j == 0) || (j == 2))
                {
                    ui.status = UIItemFlower.Status.SELECT;
                }

                float w = 500f;
                ui.SetFontSize((int)(w * 0.7f));
                idx++;
            }

        }

        LayOut();




    }


}
