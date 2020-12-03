using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//相对布局 位于target的左边 右边 下边 上边
public class LayOutRelation : LayOutBase
{
    public GameObject target;
    public bool isOutSide = false;
    public Vector2 _offset;
    public Vector2 offset
    {
        get
        {
            return this._offset;
        }
        set
        {
            this._offset = value;
            this.LayOut();
        }
    }

    public Type _type = Type.PARENT;
    public Type type
    {
        get
        {
            return _type;
        }

        set
        {
            _type = value;
            LayOut();
        }

    }
    public enum Type
    {
        NONE = 0,// 
        PARENT,//相对父窗口 
        TARGET,//相对目标 
    }

    void Awake()
    {
        this.LayOut();
        if (isOnlyForLandscape)
        {
            //  this.name = "LayOutRelationH";
        }
    }
    void Start()
    {
        this.LayOut();
    }


    public override void LayOut()
    {
        if (!Enable())
        {
            return;
        }
        base.LayOut();

        float x, y, w, h;

        RectTransform rctran = this.gameObject.GetComponent<RectTransform>();
        if (rctran == null)
        {
            return;
        }

        Vector3 pt = Vector3.zero;//this.transform.position;//getPosition
        if (IsSprite())
        {
            pt = this.transform.localPosition;
        }
        else
        {
            pt = rctran.anchoredPosition;
        }
        x = pt.x;
        y = pt.y;
        w = rctran.rect.width;
        h = rctran.rect.height;

        // if (IsSprite())
        {
            w = w * this.transform.localScale.x;
            h = h * this.transform.localScale.y;
        }

        if (isOutSide)
        {
            w = 0;
            h = 0;
        }
        // Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;

        switch (this.type)
        {
            case Type.NONE:
                {

                }
                break;


            case Type.PARENT:
                {

                    RectTransform rctranParent = this.transform.parent as RectTransform;
                    if (rctranParent == null)
                    {
                        break;
                    }
                    float w_parent = rctranParent.rect.width;
                    float h_parent = rctranParent.rect.height;
                    if (this.align == LayOutBase.Align.LEFT)
                    {
                        x = -w_parent / 2 + w / 2 + this.offset.x;
                    }
                    if (this.align == LayOutBase.Align.RIGHT)
                    {
                        x = w_parent / 2 - w / 2 - this.offset.x;
                    }
                    if (this.align == LayOutBase.Align.UP)
                    {
                        y = h_parent / 2 - h / 2 - this.offset.y;
                    }
                    if (this.align == LayOutBase.Align.DOWN)
                    {
                        y = -h_parent / 2 + h / 2 + this.offset.y;
                    }



                    if (this.align == LayOutBase.Align.UP_LEFT)
                    {
                        x = -w_parent / 2 + w / 2 + this.offset.x;
                        y = h_parent / 2 - h / 2 - this.offset.y;
                    }
                    if (this.align == LayOutBase.Align.UP_RIGHT)
                    {
                        x = w_parent / 2 - w / 2 - this.offset.x;
                        y = h_parent / 2 - h / 2 - this.offset.y;
                    }
                    if (this.align == LayOutBase.Align.DOWN_LEFT)
                    {
                        x = -w_parent / 2 + w / 2 + this.offset.x;
                        y = -h_parent / 2 + h / 2 + this.offset.y;
                    }

                    if (this.align == LayOutBase.Align.DOWN_RIGHT)
                    {
                        x = w_parent / 2 - w / 2 - this.offset.x;
                        y = -h_parent / 2 + h / 2 + this.offset.y;
                    }


                }
                break;
            case Type.TARGET:
                {
                    if (this.target == null)
                    {
                        break;
                    }
                    //相对目标

                    RectTransform rctranTarget = this.target.GetComponent<RectTransform>();
                    Vector2 ptTarget = rctranTarget.anchoredPosition;//this.target.getPosition();
                                                                     //位于target的左边
                    if (this.align == LayOutBase.Align.LEFT)
                    {
                        x = ptTarget.x - rctranTarget.rect.width / 2 - w / 2 - this.offset.x;
                    }
                    if (this.align == LayOutBase.Align.RIGHT)
                    {
                        x = ptTarget.x + rctranTarget.rect.width / 2 + w / 2 + this.offset.x;
                    }
                    if (this.align == LayOutBase.Align.UP)
                    {
                        y = ptTarget.y + rctranTarget.rect.height / 2 + h / 2 + this.offset.y;
                    }
                    if (this.align == LayOutBase.Align.DOWN)
                    {
                        y = ptTarget.y - rctranTarget.rect.height / 2 - h / 2 - this.offset.y;
                    }

                    //相同位置
                    if (this.align == LayOutBase.Align.SAME_POSTION)
                    {
                        x = ptTarget.x;
                        y = ptTarget.y;
                    }

                    if (this.align == LayOutBase.Align.UP_LEFT)
                    {
                        x = ptTarget.x - rctranTarget.rect.width / 2 - w / 2 - this.offset.x;
                        y = ptTarget.y + rctranTarget.rect.height / 2 + h / 2 + this.offset.y;
                    }
                    if (this.align == LayOutBase.Align.UP_RIGHT)
                    {
                        x = ptTarget.x + rctranTarget.rect.width / 2 + w / 2 + this.offset.x;
                        y = ptTarget.y + rctranTarget.rect.height / 2 + h / 2 + this.offset.y;
                    }
                    if (this.align == LayOutBase.Align.DOWN_LEFT)
                    {
                        x = ptTarget.x - rctranTarget.rect.width / 2 - w / 2 - this.offset.x;
                        y = ptTarget.y - rctranTarget.rect.height / 2 - h / 2 - this.offset.y;
                    }

                    if (this.align == LayOutBase.Align.DOWN_RIGHT)
                    {
                        x = ptTarget.x + rctranTarget.rect.width / 2 + w / 2 + this.offset.x;
                        y = ptTarget.y - rctranTarget.rect.height / 2 - h / 2 - this.offset.y;
                    }
                }
                break;

        }
        if (enableOffsetAdBanner)
        {

            if (IsSprite())
            {
                y += AdKitCommon.main.heightAdWorld;
            }
            else
            {
                y += AdKitCommon.main.heightAdCanvas;
            }
        }
        //this.node.setPosition(x, y);
        if (IsSprite())
        {
            this.transform.localPosition = new Vector3(x, y, this.transform.localPosition.z);
        }
        else
        {
            rctran.anchoredPosition = new Vector2(x, y);
        }

    }

}
