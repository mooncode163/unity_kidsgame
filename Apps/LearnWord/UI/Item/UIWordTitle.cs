
using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;
// public delegate void OnUILoveControllerDidCloseDelegate(UILoveController ui);
public class UIWordTitle : UIView
{
    UIWordItem uiPrefab;
    List<object> listItem;
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
        LayOut();
        // Invoke("LayOut",1f);
    }
    void LoadPrefab()
    {
        if (uiPrefab == null)
        {
            GameObject obj = PrefabCache.main.Load("App/Prefab/Item/UIWordItem");
            if (obj != null)
            {
                uiPrefab = obj.GetComponent<UIWordItem>();
            }
        }

    }
    public void UpdateItem(string word, string pinyin)
    {
        LoadPrefab();
        if (listItem == null)
        {
            listItem = new List<object>();
        }
        else
        {
            listItem.Clear();
        }


        for (int i = 0; i < word.Length; i++)
        {
            UIWordItem ui = (UIWordItem)GameObject.Instantiate(uiPrefab);
            ui.transform.SetParent(this.transform);
            ui.transform.localScale = new Vector3(1f, 1f, 1f);
            ui.UpdateItem(word.Substring(i, 1), pinyin);
            listItem.Add(ui);
        }
        LayOut();
    }


    public void UpdateColor(Color color)
    {
        for (int i = 0; i < listItem.Count; i++)
        {
            UIWordItem ui = (UIWordItem)listItem[i];
            ui.textTitle.color = color;
        }

    }
    public override void LayOut()
    {
        base.LayOut();
        for (int i = 0; i < listItem.Count; i++)
        {
            UIWordItem ui = (UIWordItem)listItem[i];
            ui.LayOut();
        }
    }


}
