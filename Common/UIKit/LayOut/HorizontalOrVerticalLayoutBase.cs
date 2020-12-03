using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//垂直布局
public class HorizontalOrVerticalLayoutBase : LayOutBase
{
    public List<GameObject> listItem;
    //是否控制大小
    public bool childControlHeight;
    public bool childControlWidth;

    //是否整个区域展开
    public bool childForceExpandHeight;
    public bool childForceExpandWidth;



    public bool childScaleHeight;
    public bool childScaleWidth;

    public int row = 1;//行
    public int col = 1;//列  

    public float GetWidthTotalItem(int end = -1)
    {
        float ret = 0;
        int idx = 0;
        int end_idx = 0;
        float oft = 0;
        if (end < 0)
        {
            end_idx = listItem.Count - 1;
            oft = (space.x * (listItem.Count + 1));
        }
        else
        {
            end_idx = end;
            oft = (space.x * (end + 1));
        }
        foreach (GameObject obj in listItem)
        {
            float w = GetWidthItem(obj, row, col);
            if (idx <= end_idx)
            {
                ret += w;
            }
            idx++;
        }
        ret += oft;
        return ret;
    }
    public float GetHeightTotalItem(int end = -1)
    {
        float ret = 0;
        int idx = 0;
        int end_idx = 0;
        float oft = 0;
        if (end < 0)
        {
            end_idx = listItem.Count - 1;
            oft = (space.y * (listItem.Count + 1));
        }
        else
        {
            end_idx = end;
            oft = (space.y * (end + 1));
        }
        foreach (GameObject obj in listItem)
        {
            if (idx <= end_idx)
            {
                ret += GetHeightItem(obj, row, col);
            }
            idx++;
        }
        ret += oft;
        return ret;
    }

    public void GetTotalItem()
    {
        if (listItem == null)
        {
            listItem = new List<GameObject>();
        }
        else
        {
            listItem.Clear();
            // return;
        }

        /* 
      foreach (Transform child in objMainWorld.transform)这种方式遍历子元素会漏掉部分子元素
      */

        //GetComponentsInChildren寻找的子对象也包括父对象自己本身和子对象的子对象
        foreach (Transform child in this.gameObject.GetComponentsInChildren<Transform>(true))
        {
            if (child == null)
            {
                // 过滤已经销毁的嵌套子对象
                // Debug.Log("LayOut child is null idx=" + idx);
                continue;
            }
            GameObject objtmp = child.gameObject;
            if (this.gameObject == objtmp)
            {
                continue;
            }

            {
                LayoutElement le = objtmp.GetComponent<LayoutElement>();
                if (le != null && le.ignoreLayout)
                {
                    continue;
                }
            }

            {
                LayOutElement le = objtmp.GetComponent<LayOutElement>();
                if (le != null && le.ignoreLayout)
                {
                    continue;
                }

            }

            if (!enableHide)
            {
                if (!objtmp.activeSelf)
                {
                    //过虑隐藏的
                    continue;
                }
            }

            if (objtmp.transform.parent != this.gameObject.transform)
            {
                //只找第一层子物体
                continue;
            }

            listItem.Add(objtmp);

        }
    }

    public float GetWidthItem(GameObject obj, int r, int c)
    {
        float x, y, w, h;
        RectTransform rctran = this.gameObject.GetComponent<RectTransform>();
        w = rctran.rect.width;
        h = rctran.rect.height;
        float item_w = 0;
        RectTransform rctranItem = obj.GetComponent<RectTransform>();

        if (childControlWidth)
        {
            item_w = (w - (space.x * (col + 1))) / col;
        }
        else
        {
            item_w = rctranItem.rect.width;
        }

        return item_w;

    }


    public float GetHeightItem(GameObject obj, int r, int c)
    {
        float x, y, w, h;
        RectTransform rctran = this.gameObject.GetComponent<RectTransform>();
        w = rctran.rect.width;
        h = rctran.rect.height;
        float item_h = 0;

        RectTransform rctranItem = obj.GetComponent<RectTransform>();

        if (childControlHeight)
        {
            item_h = (h - (space.y * (row + 1))) / row;
        }
        else
        {
            item_h = rctranItem.rect.height;
        }
        return item_h;

    }

    // r 行 ; c 列  返回中心位置
    public Vector2 GetItemPostion(GameObject obj, int r, int c)
    {
        float x, y, w, h;
        RectTransform rctran = this.gameObject.GetComponent<RectTransform>();
        w = rctran.rect.width;
        h = rctran.rect.height;
        float item_w = 0;
        float item_h = 0;

        RectTransform rctranItem = obj.GetComponent<RectTransform>();

        item_w = GetWidthItem(obj, r, c);
        if (childControlWidth)
        {
            // item_w = (w - (space.x * (col + 1))) / col;

            rctranItem.sizeDelta = new Vector2(item_w, rctranItem.sizeDelta.y);
        }
        else
        {
            // item_w = rctranItem.rect.width;
        }

        item_h = GetHeightItem(obj, r, c);
        if (childControlHeight)
        {
            //  item_h = (h - (space.y * (row + 1))) / row;
            rctranItem.sizeDelta = new Vector2(rctranItem.sizeDelta.x, item_h);
        }
        else
        {
            //  item_h = rctranItem.rect.height;
        }



        float x_left = 0, y_bottom = 0;

        // float w_total = item_w * col + (space.x * (col + 1));
        // float h_total = item_h * row + (space.y * (row + 1));
        float w_total = GetWidthTotalItem();
        float h_total = GetHeightTotalItem();
        if (col == 1)
        {
            w_total = item_w * col + (space.x * (col + 1));
        }
        // w_total = item_w * col + (space.x * (col + 1));
        Debug.Log("GetItemPostion:w_total=" + w_total + " GetWidthTotalItem=" + GetWidthTotalItem() + " col=" + col + " w=" + w + " space.x=" + space.x);

        if (row == 1)
        {
            h_total = item_h * row + (space.y * (row + 1));
        }
        Debug.Log("GetItemPostion:h_total=" + h_total + " GetHeightTotalItem=" + GetHeightTotalItem() + " col=" + col + " w=" + w);

        x_left = -w_total / 2;
        if (childForceExpandWidth)
        {
            x_left = -w / 2;
        }
        else
        {
            if (align == Align.LEFT)
            {
                x_left = -w / 2;
            }
            else if (align == Align.RIGHT)
            {
                x_left = w / 2 - w_total;
            }
            else if (align == Align.CENTER)
            {
                x_left = -w_total / 2;
            }

        }

        x = x_left + item_w * c + item_w / 2 + space.x * (c + 1);
        float tmp_w = 0;
        if (col == 1)
        {
            tmp_w = item_w * c;
        }
        else
        {
            tmp_w = c > 0 ? GetWidthTotalItem(c - 1) : 0;
        }
        // x = x_left + tmp_w + item_w / 2 + space.x * (c + 1);
        x = x_left + tmp_w + item_w / 2 + space.x;

        y_bottom = -h_total / 2;
        if (childForceExpandHeight)
        {
            y_bottom = -h / 2;
        }
        else
        {
            if (align == Align.DOWN)
            {
                y_bottom = -h / 2;
            }
            else if (align == Align.UP)
            {
                y_bottom = h / 2 - h_total;
            }
            else if (align == Align.CENTER)
            {
                y_bottom = -h_total / 2;
            }
        }
        y = y_bottom + item_h * r + item_h / 2 + space.y * (r + 1);

        float tmp_h = 0;
        if (row == 1)
        {
            tmp_h = item_h * r;
        }
        else
        {
            tmp_h = r > 0 ? GetHeightTotalItem(r - 1) : 0;
        }
        // y = y_bottom + tmp_h + item_h / 2 + space.y * (r + 1);
        y = y_bottom + tmp_h + item_h / 2 + space.y;
        Vector2 pt = new Vector2(x, y);
        Debug.Log("GetItemPostion:item_w=" + item_w + " item_h=" + item_h + " r=" + r + " c=" + c + " pt=" + pt + " w=" + w + " h=" + h);
        return pt;

    }
    public override void LayOut()
    {
        int idx = 0;
        int r = 0, c = 0;
        float w, h;
        if (!Enable())
        {
            return;
        }
        base.LayOut();
        GetTotalItem();
        RectTransform rctran = this.gameObject.GetComponent<RectTransform>();
        w = rctran.rect.width;
        h = rctran.rect.height;
        if (childForceExpandWidth && (!childControlWidth))
        {
            space.x = 0;
            space.x = (w - GetWidthTotalItem()) / (col + 1);
        }

        if (childForceExpandHeight && (!childControlHeight))
        {
            space.y = 0;
            space.y = (h - GetHeightTotalItem()) / (row + 1);
        }
        /* 
        foreach (Transform child in objMainWorld.transform)这种方式遍历子元素会漏掉部分子元素
        */

        //GetComponentsInChildren寻找的子对象也包括父对象自己本身和子对象的子对象
        foreach (GameObject obj in listItem)
        {

            //  LayoutElement
            r = idx / col;
            c = idx - r * col;

            //从顶部往底部显示
            if (dispLayVertical == DispLayVertical.TOP_TO_BOTTOM)
            {
                r = row - 1 - r;
            }

            //从右往左显示
            if (dispLayHorizontal == DispLayHorizontal.RIGHT_TO_LEFT)
            {
                c = col - 1 - c;
            }

            Vector2 pt = GetItemPostion(obj, r, c);
            RectTransform rctranItem = obj.GetComponent<RectTransform>();
            if (rctranItem != null)
            {
                rctranItem.anchoredPosition = pt;
                // Debug.Log("GetItemPostion:idx=" + idx + " r=" + r + " c=" + c + " pt=" + pt);
            }
            idx++;
        }
    }
}
