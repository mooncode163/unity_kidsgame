using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayOutBase : MonoBehaviour
{
    public enum DispLayVertical
    {
        TOP_TO_BOTTOM = 0,
        BOTTOM_TO_TOP,
    }

    public enum DispLayHorizontal
    {
        LEFT_TO_RIGHT = 0,
        RIGHT_TO_LEFT,
    }


    public enum Align
    {
        UP = 0,
        DOWN,
        LEFT,
        RIGHT,
        CENTER,
        UP_LEFT,
        UP_RIGHT,
        DOWN_LEFT,
        DOWN_RIGHT,
        Horizontal,
        Vertical,
        SAME_POSTION,
    }


    public DispLayVertical dispLayVertical;
    public DispLayHorizontal dispLayHorizontal;
    public bool enableLayout = true;
    public bool enableHide = true;//是否过虑Hide

    public bool enableOffsetAdBanner = false;
    public bool enableOffsetScreen = false;//全面屏 四周的偏移
    public bool isOnlyForLandscape = false;
    public bool isOnlyForPortrait = false;


    public Vector2 space = Vector2.zero;

    protected TextAnchor childAlignment;
    public Align align = Align.CENTER;


    public virtual void LayOut()
    {

    }

    public bool Enable()
    {
        bool ret = true;
        if (!enableLayout)
        {
            ret = false;
        }
        if (isOnlyForLandscape)
        {
            if (!Device.isLandscape)
            {
                ret = false;
            }
        }
        if (isOnlyForPortrait)
        {
            if (Device.isLandscape)
            {
                ret = false;
            }
        }

        return ret;
    }

    public int GetChildCount(bool includeHide = true)
    {
        return LayoutUtil.main.GetChildCount(this.gameObject, includeHide);
    }

    public bool IsSprite()
    {
        UISprite uisp = this.gameObject.GetComponent<UISprite>();
        if (uisp != null)
        {
            return true;
        }
        SpriteRenderer rd = this.gameObject.GetComponent<SpriteRenderer>();
        if (rd != null)
        {
            return true;
        }

        return false;
    }
    public SpriteRenderer GetSpriteRenderer(GameObject obj)
    {
        GameObject objSR = obj;
        UISprite uisp = obj.GetComponent<UISprite>();
        if (uisp != null)
        {
            objSR = uisp.objSp;
        }
        SpriteRenderer rd = objSR.GetComponent<SpriteRenderer>();
        return rd;
    }

}
