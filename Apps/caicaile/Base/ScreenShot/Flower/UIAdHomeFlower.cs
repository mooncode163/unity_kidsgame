
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Vectrosity;
using DG.Tweening;
public class UIAdHomeFlower : UIShotBase
{
    public UIImage imageBg;
    public UIText textTitle;
    public GameObject objWord;
    public UIItemFlower uiItemFlowerPrefab;
    public int row = 7;
    public int col = 7;
    public List<UIItemFlower> listItem;
    public List<UIItemFlower> listItemSel;
    LayOutGrid lygrid;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        base.Awake();
        lygrid = objWord.GetComponent<LayOutGrid>();
        listItem = new List<UIItemFlower>();
        listItemSel = new List<UIItemFlower>();
        row = 4;
        col = 4;
        lygrid.row = row;
        lygrid.col = col;
        lygrid.enableLayout = false;
        lygrid.dispLayVertical = LayOutBase.DispLayVertical.TOP_TO_BOTTOM;
        string appname = Common.GetAppNameDisplay();
        textTitle.text = appname;

    }
    void Start()
    {
        UpdateItem();
        LayOut();
        OnUIDidFinish();
    }


    public override void LayOut()
    {
        base.LayOut();
        float x, y, w, h;
        RectTransform rctranRoot = objWord.GetComponent<RectTransform>();
        Debug.Log("rctranRoot w=" + rctranRoot.rect.width + " h=" + rctranRoot.rect.height);

        if (lygrid != null)
        {
            // lygrid.LayOut();
            foreach (UIItemFlower item in listItem)
            {

                RectTransform rctran = item.GetComponent<RectTransform>();
                w = (rctranRoot.rect.width - (lygrid.space.x) * (col + 1)) / col;
                h = w;
                rctran.sizeDelta = new Vector2(w, h);
                item.SetFontSize((int)(w * 0.7f));
                Vector2 pos = lygrid.GetItemPostion(item.gameObject, item.indexRow, item.indexCol);
                rctran.anchoredPosition = pos;
                item.LayOut();
            }
        }


    }

    public void UpdateItem()
    {
        List<object> listPos = LevelParseIdiomFlower.main.listPosition;
        int idx_pos = Random.Range(0, listPos.Count);
        idx_pos = 10;
        Debug.Log("UpdateItem listPos.Count=" + listPos.Count + " idx_pos=" + idx_pos);
        //idx_pos = 0;
        PositionInfo infoPos = listPos[idx_pos] as PositionInfo;

        lygrid.row = row;
        lygrid.col = col;
        int level = LevelManager.main.gameLevel;
        CaiCaiLeItemInfo info = GameLevelParse.main.listGuanka[level] as CaiCaiLeItemInfo;
        int idx = 0;
        for (int i = 0; i < info.listIdiom.Count; i++)
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
                if (i == 0)
                {
                    listItemSel.Add(ui);
                    ui.status = UIItemFlower.Status.SELECT;
                }
                idx++;
            }

        }



        LayOut();
        CreateLine();

    }
    Vector2 GetHandPos(int idx)
    {
        float x, y, w, h;
        UIItemFlower item = listItemSel[idx];
        RectTransform rcitem = item.GetComponent<RectTransform>();

        x = rcitem.anchoredPosition.x;
        y = rcitem.anchoredPosition.y;
        return new Vector2(x, y);
    }


    Vector3 GetHandTransformPos(int idx)
    {
        float x, y, w, h;
        UIItemFlower item = listItemSel[idx];
        x = item.transform.position.x;
        y = item.transform.position.y;
        return new Vector3(x, y, item.transform.position.z);
    }
    void SelectItem(int idx, bool isSel)
    {
        UIItemFlower item = listItemSel[idx];
        if (isSel)
        {
            item.status = UIItemFlower.Status.SELECT;
        }
        else
        {
            item.status = UIItemFlower.Status.NORMAL;
        }
    }


    void CreateLine()
    {
        List<Vector2> listPoint = new List<Vector2>();
        foreach (UIItemFlower ui in listItemSel)
        {
            Vector2 pos = ui.transform.position;

            // Debug.Log("line pos =" + pos + " Screen.width=" + Screen.width + " Screen.height=" + Screen.height);
            // pos.x = pos.x - (Screen.width) * index;
            // pos.y = pos.y - (Screen.height) * index;
            Debug.Log("line pos2 =" + pos);
            listPoint.Add(pos);
        }
        var line = new VectorLine("Line", listPoint, 20.0f, LineType.Continuous);
        line.color = Color.red;
        //    Undo.RegisterCreatedObjectUndo(line.rectTransform.gameObject, "Create VectorLine");

        //   var useVectorCanvas = true;
        //    var selectedObj = Selection.activeObject as GameObject;
        //    if (selectedObj != null)
        {
            var canvas = AppSceneBase.main.canvasMain;
            if (canvas != null && canvas.renderMode != RenderMode.WorldSpace)
            {
                // line.SetCanvas(canvas);
            }
        }
        line.Draw();

    }

}

