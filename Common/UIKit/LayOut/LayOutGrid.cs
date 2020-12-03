using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//方格布局
public class LayOutGrid : LayOutBase
{

    /// <summary>
    /// Which corner is the starting corner for the grid.
    /// </summary>
    public enum Corner
    {
        /// <summary>
        /// Upper Left corner.
        /// </summary>
        UpperLeft = 0,
        /// <summary>
        /// Upper Right corner.
        /// </summary>
        UpperRight = 1,
        /// <summary>
        /// Lower Left corner.
        /// </summary>
        LowerLeft = 2,
        /// <summary>
        /// Lower Right corner.
        /// </summary>
        LowerRight = 3
    }

    /// <summary>
    /// The grid axis we are looking at.
    /// </summary>
    /// <remarks>
    /// As the storage is a [][] we make access easier by passing a axis.
    /// </remarks>
    public enum Axis
    {
        /// <summary>
        /// Horizontal axis
        /// </summary>
        Horizontal = 0,
        /// <summary>
        /// Vertical axis.
        /// </summary>
        Vertical = 1
    }


    public int row = 1;//行
    public int col = 1;//列  

    //是否控制大小
    public bool childControlHeight;
    public bool childControlWidth;

    //是否整个区域展开
    public bool childForceExpandHeight;
    public bool childForceExpandWidth;



    public bool childScaleHeight;
    public bool childScaleWidth;


    [SerializeField] protected Vector2 cellSize = new Vector2(100, 100);
    public Corner startCorner;

    [SerializeField] protected Axis startAxis = Axis.Horizontal;


    private void Awake()
    {

    }

    private void Start()
    {
        LayOut();
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

        if (childControlWidth)
        {
            item_w = (w - (space.x * (col + 1))) / col;
            rctranItem.sizeDelta = new Vector2(item_w, rctranItem.sizeDelta.y);
        }
        else
        {
            item_w = rctranItem.rect.width;
        }

        if (childControlHeight)
        {
            item_h = (h - (space.y * (row + 1))) / row;
            rctranItem.sizeDelta = new Vector2(rctranItem.sizeDelta.x, item_h);
        }
        else
        {
            item_h = rctranItem.rect.height;
        }



        float x_left = 0, y_bottom = 0;

        if (childForceExpandWidth)
        {
            space.x = (w - item_w * col) / (col + 1);
        }

        if (childForceExpandHeight)
        {
            space.y = (h - item_h * row) / (row + 1);
        }

        float w_total = item_w * col + (space.x * (col + 1));
        float h_total = item_h * row + (space.y * (row + 1));

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
        if (childForceExpandWidth)
        {
            // x += space.x;
        }

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
        if (childForceExpandHeight)
        {
            //y += space.y;
        }
        return new Vector2(x, y);

    }

    public override void LayOut()
    {
        int idx = 0;
        int r = 0, c = 0;
        if (!Enable())
        {
            return;
        }
        base.LayOut();
        /* 
        foreach (Transform child in objMainWorld.transform)这种方式遍历子元素会漏掉部分子元素
        */

        //GetComponentsInChildren寻找的子对象也包括父对象自己本身和子对象的子对象
        foreach (Transform child in this.gameObject.GetComponentsInChildren<Transform>(true))
        {
            if (child == null)
            {
                // 过滤已经销毁的嵌套子对象
                Debug.Log("LayOut child is null idx=" + idx);
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

            Vector2 pt = GetItemPostion(child.gameObject, r, c);
            RectTransform rctran = child.gameObject.GetComponent<RectTransform>();
            if (rctran != null)
            {
                rctran.anchoredPosition = pt;
                Debug.Log("GetItemPostion:idx=" + idx + " r=" + r + " c=" + c + " pt=" + pt);
            }
            idx++;
        }
    }
}
