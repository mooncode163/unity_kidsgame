using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayOut
{
    public int row;//行
    public int col;//列
    public Rect rect;
    public bool isAutoFitSize = false;
    public float itemScaleRatio = 1.0f;
    private List<GameObject> listItem;

    public int count
    {
        get
        {
            int ret = 0;
            if (listItem != null)
            {
                ret = listItem.Count;
            }
            return ret;
        }
    }
    public void AddItem(GameObject obj)
    {
        if (listItem == null)
        {
            listItem = new List<GameObject>();
        }

        listItem.Add(obj);
    }

    public void ReplaceItem(int idx, GameObject obj)
    {
        listItem.RemoveAt(idx);
        listItem.Insert(idx, obj);
    }
    public GameObject GetItem(int idx)
    {
        GameObject obj = null;
        if (listItem != null)
        {
            if (idx < listItem.Count)
            {
                obj = listItem[idx];
            }
        }
        return obj;
    }

    public void Clear()
    {
        if (listItem != null)
        {
            listItem.Clear();
        }
    }

     public void DestroyAllItem()
    {
         if (listItem == null)
        {
            return;
        }
       foreach(GameObject obj in listItem){
           GameObject.DestroyImmediate(obj);
       }

        listItem.Clear();
    }

    public Vector2 GetItemPostion(int i_row, int j_col)
    {
        float x, y;
        float item_w = rect.size.x / col;
        float item_h = rect.size.y / row;

        x = item_w * j_col + rect.x;
        y = item_h * i_row + rect.y;
        //rect item : x,y,item_w,item_h
        float x_center, y_center;
        x_center = x + item_w / 2;
        y_center = y + item_h / 2;
        //Debug.Log("i=" + i + " item_h=" + item_h + " rect.size.y=" + rect.size.y + " row=" + row + " y_center=" + y_center);
        return new Vector2(x_center, y_center);

    }
    public void LayOutItem()
    {
        float x, y;
        float item_w = rect.size.x / col;
        float item_h = rect.size.y / row;
        int idx = 0;
        if (listItem.Count == 0)
        {
            return;
        }
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (idx < listItem.Count)
                {
                    x = item_w * j + rect.x;
                    y = item_h * i + rect.y;
                    //rect item : x,y,item_w,item_h
                    float x_center, y_center;
                    x_center = x + item_w / 2;
                    y_center = y + item_h / 2;
                    //Debug.Log("i=" + i + " item_h=" + item_h + " rect.size.y=" + rect.size.y + " row=" + row + " y_center=" + y_center);
                    GameObject obj = listItem[idx];
                    RectTransform rctran = obj.transform as RectTransform;
                    rctran.anchoredPosition = new Vector2(x_center, y_center);
                    if (isAutoFitSize)
                    {
                        SpriteRenderer spRender = obj.GetComponent<SpriteRenderer>();
                        if (spRender != null)
                        { 
                            float scalez = obj.transform.localScale.z;
                            float scalex = item_w*itemScaleRatio/spRender.size.x;
                            float scaley = item_h*itemScaleRatio/spRender.size.y;
                            float scale = Mathf.Min(scalex, scaley);
                            obj.transform.localScale = new Vector3(scale, scale, scalez);
                        }

                    }
                    idx++;
                }

            }

        }
    }
}
