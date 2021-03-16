using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LayOutSize : LayOutBase
{

    public enum SideType
    {
        LEFT = 0,// 
        RIGHT,
        UP,
        DOWN,
    }
    public float ratioW = 1f;//宽
    public float ratioH = 1f;//高
    public GameObject target;
    public GameObject target2;
    // public float width = 1f;//宽
    // public float height = 1f;//宽

    // 横屏参数
    public float widthH = 1f;//宽
    public float heightH = 1f;//宽

    public SideType sideType;

    public float _width= 1f;
 public float width
    {
        get
        {

            if (IsUseLandscape())
            {
               return widthH;
            }

            return _width;
        }

        set
        {
            
             if (IsUseLandscape())
            {
                widthH = value;
            }else{
                _width = value;
            }
            LayOut();
        }

    }

        public float _height= 1f;
 public float height
    {
        get
        {

            if (IsUseLandscape())
            {
               return heightH;
            }

            return _height;
        }

        set
        {
            
             if (IsUseLandscape())
            {
                heightH = value;
            }else{
                _height = value;
            }
            LayOut();
        }

    }

    //左下角偏移量 单位为canvas
    public Vector2 _offsetMin;

    public Vector2 offsetMin
    {
        get
        {

            if (IsSprite())
            {
                float x = Common.CanvasToWorldWidth(AppSceneBase.main.mainCamera, AppSceneBase.main.sizeCanvas, _offsetMin.x);
                float y = Common.CanvasToWorldHeight(AppSceneBase.main.mainCamera, AppSceneBase.main.sizeCanvas, _offsetMin.y);
                return new Vector2(x, y);
            }

            return _offsetMin;
        }

        set
        {
            _offsetMin = value;
            LayOut();
        }

    }

    //右上角角偏移量 单位为canvas
    public Vector2 _offsetMax;
    public Vector2 offsetMax
    {
        get
        {
            if (IsSprite())
            {
                float x = Common.CanvasToWorldWidth(AppSceneBase.main.mainCamera, AppSceneBase.main.sizeCanvas, _offsetMax.x);
                float y = Common.CanvasToWorldHeight(AppSceneBase.main.mainCamera, AppSceneBase.main.sizeCanvas, _offsetMax.y);
                return new Vector2(x, y);
            }

            return _offsetMax;
        }

        set
        {
            _offsetMax = value;
            LayOut();
        }

    }

    public Type _typeX;
    public Type typeX
    {
        get
        {
            return _typeX;
        }

        set
        {
            _typeX = value;
            LayOut();
        }

    }

    public Type _typeY;
    public Type typeY
    {
        get
        {
            return _typeY;
        }

        set
        {
            _typeY = value;
            LayOut();
        }

    }

    public TypeWidthHeightScale _typeWidthHeightScale;
    public TypeWidthHeightScale typeWidthHeightScale
    {
        get
        {
            return _typeWidthHeightScale;
        }

        set
        {
            _typeWidthHeightScale = value;
            LayOut();
        }

    }



    public enum Type
    {
        MATCH_CONTENT = 0,//按内容设置
        MATCH_PARENT,//与父窗口等大或者按比例 
        MATCH_TARGET,//与目标等大或者按比例 
        MATCH_PARENT_MIN,//父窗口width 和 height 的 min
        MATCH_PARENT_MAX,//父窗口width 和 height 的 max
        MATCH_WIDTH,//width 和 height 相等
        MATCH_HEIGHT,//width 和 height相等
        BETWEEN_SIDE_TARGET,//夹在边界和target之间
        BETWEEN_TWO_TARGET,//夹在两个target之间

        // 和widht height同步
        MATCH_VALUE,

         // 和widht height同步 canvas大小
        MATCH_VALUE_Canvas,
    }

    public enum TypeWidthHeightScale// 保持 w=h
    {
        NONE = 0,
        MIN,// 
        MAX,
    }

    void Awake()
    {
        this.LayOut();
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
        UpdateSize();
        if ((this.typeX == Type.MATCH_HEIGHT) || (this.typeY == Type.MATCH_WIDTH))
        {
            UpdateSize();
        }

    }


    void UpdateSize()
    {
        float x = 0, y = 0, w = 0, h = 0;
        RectTransform rctranParent = this.transform.parent as RectTransform;
        if (rctranParent == null)
        {
            return;
        }
        RectTransform rctran = this.transform as RectTransform;
        var w_parent = rctranParent.rect.width;
        var h_parent = rctranParent.rect.height;
        w = rctran.rect.width;
        h = rctran.rect.height;
        if (this.gameObject.name == "Board")
        {

            Debug.Log("Board w_parent = " + w_parent + " h_parent=" + h_parent + " cam=" + Common.GetWorldSize(AppSceneBase.main.mainCamera));
        }
        switch (this.typeX)
        {
            case Type.MATCH_CONTENT:
                {
                    w = rctran.rect.width;
                    x = rctran.anchoredPosition.x;
                }
                break;
            case Type.MATCH_VALUE:
                {
                    w = width;
                    x = rctran.anchoredPosition.x;
                }
                break;
           case Type.MATCH_VALUE_Canvas:
                {
                    w = width;
                     if (IsSprite())
                    {
                        w= Common.CanvasToWorldWidth(AppSceneBase.main.mainCamera, AppSceneBase.main.sizeCanvas, width);
                    }
                   x = rctran.anchoredPosition.x;
                }
                break;
            case Type.MATCH_PARENT:
                {
                    w = w_parent * ratioW;
                    x = rctran.anchoredPosition.x;
                }
                break;
            case Type.MATCH_PARENT_MIN:
                {
                    w = Mathf.Min(w_parent, h_parent) * ratioW;
                    x = rctran.anchoredPosition.x;
                }
                break;
            case Type.MATCH_PARENT_MAX:
                {
                    w = Mathf.Max(w_parent, h_parent) * ratioW;
                    x = rctran.anchoredPosition.x;
                }
                break;
            case Type.MATCH_TARGET:
                {
                    if (this.target != null)
                    {
                        RectTransform rctranTarget = this.target.GetComponent<RectTransform>();
                        Vector2 ptTarget = rctranTarget.anchoredPosition;//this.target.getPosition();
                        w = rctranTarget.rect.width * ratioW;
                        x = rctran.anchoredPosition.x;
                    }

                }
                break;
            case Type.MATCH_HEIGHT:
                {
                    w = rctran.rect.height;
                }
                break;

            case Type.BETWEEN_SIDE_TARGET:
                {

                    if ((this.sideType == SideType.LEFT) || (this.sideType == SideType.RIGHT))
                    {
                        w = LayoutUtil.main.GetBetweenSideAndTargetSize(this.target, this.sideType) * ratioW;
                    }

                }
                break;
            case Type.BETWEEN_TWO_TARGET:
                {
                    w = LayoutUtil.main.GetBetweenTwoTargetSize(this.target, this.target2, false);

                }
                break;
        }

        switch (this.typeY)
        {
            case Type.MATCH_CONTENT:
                {
                    h = rctran.rect.height;
                    y = rctran.anchoredPosition.y;
                }
                break;
            case Type.MATCH_VALUE:
                {
                    h = height;
                    y = rctran.anchoredPosition.y;
                }
                break;
           case Type.MATCH_VALUE_Canvas:
                {
                    h = height;
                     if (IsSprite())
                    {
                    h= Common.CanvasToWorldHeight(AppSceneBase.main.mainCamera, AppSceneBase.main.sizeCanvas, height);
                    }
                    y = rctran.anchoredPosition.y;
                }
                break;
                
            case Type.MATCH_PARENT:
                {
                    h = h_parent * ratioH;
                    y = rctran.anchoredPosition.y;
                }
                break;
            case Type.MATCH_PARENT_MIN:
                {
                    h = Mathf.Min(w_parent, h_parent) * ratioH;
                    y = rctran.anchoredPosition.y;
                }
                break;
            case Type.MATCH_PARENT_MAX:
                {
                    h = Mathf.Max(w_parent, h_parent) * ratioH;
                    y = rctran.anchoredPosition.y;
                }
                break;
            case Type.MATCH_TARGET:
                {
                    if (this.target != null)
                    {
                        RectTransform rctranTarget = this.target.GetComponent<RectTransform>();
                        Vector2 ptTarget = rctranTarget.anchoredPosition;//this.target.getPosition();
                        h = rctranTarget.rect.height * ratioH;
                        y = rctran.anchoredPosition.y;
                    }

                }
                break;
            case Type.MATCH_WIDTH:
                {
                    h = rctran.rect.width;
                }
                break;

            case Type.BETWEEN_SIDE_TARGET:
                {

                    if ((this.sideType == SideType.UP) || (this.sideType == SideType.DOWN))
                    {
                        h = LayoutUtil.main.GetBetweenSideAndTargetSize(this.target, this.sideType) * ratioH;
                    }

                }
                break;
            case Type.BETWEEN_TWO_TARGET:
                {
                    h = LayoutUtil.main.GetBetweenTwoTargetSize(this.target, this.target2, true);

                }
                break;

        }

        w -= (this.offsetMin.x + this.offsetMax.x);
        h -= (this.offsetMin.y + this.offsetMax.y);

        if (enableOffsetAdBanner)
        {

            if (IsSprite())
            {
                h -= AdKitCommon.main.heightAdWorld;
            }
            else
            {
                h -= AdKitCommon.main.heightAdCanvas;
            }
        }

        switch (this.typeWidthHeightScale)
        {
            case TypeWidthHeightScale.MIN:
                {
                    w = Mathf.Min(w, h);
                    h = w;
                }
                break;

            case TypeWidthHeightScale.MAX:
                {
                    w = Mathf.Max(w, h);
                    h = w;
                }
                break;

        }

        rctran.sizeDelta = new Vector2(w, h);
        //rctran.anchoredPosition = new Vector2(x, y);
    }

}
