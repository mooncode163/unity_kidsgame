using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScrollViewDot : UIView
{

    UIDot uiDotPrefab;
    List<UIDot> listItem;
    int indexSel;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        Init();
    }
    void Init()
    {
        if (listItem == null)
        {
            indexSel = -1;
            listItem = new List<UIDot>();
            GameObject obj = PrefabCache.main.Load(UIDot.PREFAB_UIDOT);
            uiDotPrefab = obj.GetComponent<UIDot>();
        }
    }
    void AddItem()
    {
        float w, h;
        UIDot uiDot = (UIDot)GameObject.Instantiate(uiDotPrefab);
        uiDot.transform.parent = this.transform;
        uiDot.transform.localScale = new Vector3(1f, 1f, 1f);
        uiDot.index = listItem.Count;
        w = h = GetComponent<RectTransform>().rect.height;
        uiDot.GetComponent<RectTransform>().sizeDelta = new Vector2(w, h);
        uiDot.UpdateItem(false);
        listItem.Add(uiDot);
    }
    public void UpdateDotImage(string picsel,string picunsel)
    {

    }

    void ClearAllItem()
    {
        foreach (UIDot dot in listItem)
        {
            DestroyImmediate(dot);
        }
        listItem.Clear();
    }

    public void SetTotal(int total)
    {
        Init();
        ClearAllItem();
        for (int i = 0; i < total; i++)
        {
            AddItem();
        }

        UpdateItem(0);
    }
    public void UpdateItem(int sel)
    {
        if (indexSel == sel)
        {
            return;
        }
        indexSel = sel;
        if (indexSel >= listItem.Count)
        {
            return;
        } 
         foreach (UIDot dot in listItem)
        {
            if(dot.index == indexSel){
                dot.UpdateItem(true);
            }else{
                dot.UpdateItem(false);
            }
        }
    }
}

