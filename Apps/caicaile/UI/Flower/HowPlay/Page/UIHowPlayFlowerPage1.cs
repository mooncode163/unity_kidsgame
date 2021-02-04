using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Vectrosity;
using DG.Tweening;
public class UIHowPlayFlowerPage1 : UIHowPlayFlowerPage
{
    public GameObject objWord;
    public UIItemFlower uiItemFlowerPrefab;
    public UIImage imageHand;
    public int row = 7;
    public int col = 7;
    public List<UIItemFlower> listItem;
    public List<UIItemFlower> listItemSel;
    LayOutGrid lygrid;
    Sequence seqAnimate;
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

    }
    void Start()
    {
        UpdateItem();
        LayOut();
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

                }
                idx++;
            }

        }



        LayOut();
        // CreateLine();

        RectTransform rchand = imageHand.GetComponent<RectTransform>();
        rchand.anchoredPosition = GetHandPos(0);
        rchand.SetAsLastSibling();

    }
    Vector2 GetHandPos(int idx)
    {
        float x, y, w, h;
        UIItemFlower item = listItemSel[idx];
        RectTransform rcitem = item.GetComponent<RectTransform>();
        RectTransform rchand = imageHand.GetComponent<RectTransform>();
        w = rchand.rect.width;
        h = rchand.rect.height;
        x = rcitem.anchoredPosition.x + w * 0.3f;
        y = rcitem.anchoredPosition.y - h / 2;
        return new Vector2(x, y);
    }


    Vector3 GetHandTransformPos(int idx)
    {
        float x, y, w, h;
        UIItemFlower item = listItemSel[idx];
        RectTransform rcitem = item.GetComponent<RectTransform>();
        RectTransform rchand = imageHand.GetComponent<RectTransform>();
        w = rchand.rect.width;
        h = rchand.rect.height;
        x = rcitem.anchoredPosition.x + w * 0.3f;
        y = rcitem.anchoredPosition.y - h / 2;
        w = rchand.rect.width;
        h = rchand.rect.height;
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        w = Common.CanvasToScreenWidth(sizeCanvas, rchand.rect.width);
        h = Common.CanvasToScreenHeight(sizeCanvas, rchand.rect.height);
        x = item.transform.position.x + w * 0.3f;
        y = item.transform.position.y - h / 2;
        return new Vector3(x, y, rchand.position.z);
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

    void RunAnimate()
    {

        RectTransform rchand = imageHand.GetComponent<RectTransform>();
        Vector3 pt0 = GetHandPos(0);
        Vector3 pt1 = GetHandPos(1);
        Vector3 pt2 = GetHandPos(2);
        Vector3 pt3 = GetHandPos(3);
        seqAnimate = DOTween.Sequence();
        float t_animation = 1f;

        //RectTransform 动画必须用DOLocalMove
        Tweener action0 = rchand.DOLocalMove(pt0, t_animation).SetEase(Ease.InSine);
        Tweener action1 = rchand.DOLocalMove(pt1, t_animation).SetEase(Ease.InSine);
        Tweener action2 = rchand.DOLocalMove(pt2, t_animation).SetEase(Ease.InSine);
        Tweener action3 = rchand.DOLocalMove(pt3, t_animation).SetEase(Ease.InSine);

        Tweener actionEnd = rchand.DOLocalMove(pt3, 0.1f).SetEase(Ease.InSine);

        SelectItem(0, true);
        // rchand.position = pt1;
        seqAnimate
         .Append(action0.OnComplete(
            () =>
            {
                Debug.Log("Animate start");
                SelectItem(0, true);
            }
        ))

        .Append(action1.OnComplete(
            () =>
            {
                SelectItem(1, true);
            }
        ))

        .Append(action2.OnComplete(
            () =>
            {
                SelectItem(2, true);
            }
        ))

    .Append(action3.OnComplete(
            () =>
            {
                SelectItem(3, true);

            }
        ))
        //delay
        .AppendInterval(t_animation / 2)
         .Append(actionEnd.OnComplete(
            () =>
            {
                SelectItem(1, false);
                SelectItem(2, false);
                SelectItem(3, false);

                rchand = imageHand.GetComponent<RectTransform>();
                rchand.anchoredPosition = GetHandPos(0);
                SelectItem(0, true);

                Debug.Log("Animate Finish");

            }
        ))
       .SetLoops(-1);

        seqAnimate.Play();
    }


    void StopAnimate()
    {
        seqAnimate.Pause();
        LayOut();
    }
    void CreateLine()
    {
        List<Vector2> listPoint = new List<Vector2>();
        foreach (UIItemFlower ui in listItemSel)
        {
            Vector2 pos = ui.transform.position;

            Debug.Log("line pos =" + pos + " Screen.width=" + Screen.width + " Screen.height=" + Screen.height);
            pos.x = pos.x - (Screen.width) * index;
            pos.y = pos.y - (Screen.height) * index;
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

    public override void OnPageExit()
    {
        StopAnimate();
    }
    public override void OnPageEnter()
    {
        RunAnimate();
    }
}
