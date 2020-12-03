using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LayOutScale : LayOutBase
{
    public GameObject target;
    public Type _scaleType;
    public float ratio = 1f;

    public Type scaleType
    {
        get
        {
            return _scaleType;
        }

        set
        {
            _scaleType = value;
            LayOut();
        }

    }

    public Vector2 _offsetMin;
    public Vector2 offsetMin
    {
        get
        {
            return _offsetMin;
        }

        set
        {
            _offsetMin = value;
            LayOut();
        }

    }

    public Vector2 _offsetMax;
    public Vector2 offsetMax
    {
        get
        {
            return _offsetMax;
        }

        set
        {
            _offsetMax = value;
            LayOut();
        }

    }

    public enum Type
    {
        MIN = 0,
        MAX,
        SCREEN_MAX,//按屏幕 最大化
        TARGET,//相对目标 
    }

    void Awake()
    {
        Debug.Log("LayOutScale Awake=");
    }
    void Start()
    {
        // Debug.Log("LayOutScale Start=");
        // this.LayOut();

        //保持和layoutgroup同步
        StartCoroutine(OnLayOutEnumerator());
    }
    IEnumerator OnLayOutEnumerator()
    {
        float time = 0;
        yield return new WaitForSeconds(time);
        this.LayOut();
    }

    public override void LayOut()
    {
        if (!Enable())
        {
            return;
        }
        base.LayOut();
        UpdateType(scaleType);
    }


    void UpdateType(Type ty)
    {
        _scaleType = ty;
        switch (this._scaleType)
        {
            case Type.MIN:
                {
                    this.ScaleObj(this.gameObject, false);
                }
                break;
            case Type.MAX:
                {
                    this.ScaleObj(this.gameObject, true);
                }
                break;
            case Type.TARGET:
                {
                    this.ScaleObjByTarget(this.gameObject);
                }
                break;


            case Type.SCREEN_MAX:
                {
                    Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
                    {
                        RectTransform rectTransform = this.GetComponent<RectTransform>();
                        float w_image = rectTransform.rect.width;
                        float h_image = rectTransform.rect.height;
                        print(rectTransform.rect);
                        float scalex = sizeCanvas.x / w_image;
                        float scaley = sizeCanvas.y / h_image;
                        float scale = Mathf.Max(scalex, scaley);
                        this.transform.localScale = new Vector3(scale, scale, 1.0f);
                        //屏幕坐标 现在在屏幕中央
                        this.transform.position = new Vector2(Screen.width / 2, Screen.height / 2);

                    }
                }
                break;


        }
    }


    void ScaleObj(GameObject obj, bool isMaxFit)
    {

        float x, y, w = 0, h = 0;
        RectTransform rctranParent = this.transform.parent as RectTransform;
        RectTransform rctran = this.transform as RectTransform;

        if (IsSprite())
        {
            SpriteRenderer rd = GetSpriteRenderer(obj);
            if (rd == null)
            {
                return;
            }
            if (rd.sprite == null)
            {
                return;
            }
            if (rd.sprite.texture == null)
            {
                return;
            }
            w = rd.sprite.texture.width / 100f;
            h = rd.sprite.texture.height / 100f;
        }
        else
        {
            //image
            RawImage rawimage = obj.GetComponent<RawImage>();
            if (rawimage != null)
            {
                if (rawimage.texture == null)
                {
                    return;
                }
                w = rawimage.texture.width;
                h = rawimage.texture.height;
            }
            else
            {
                Image image = obj.GetComponent<Image>();
                if (image != null)
                {
                    if (image.sprite == null)
                    {
                        return;
                    }
                    if (image.sprite.texture == null)
                    {
                        return;
                    }
                    w = image.sprite.texture.width;
                    h = image.sprite.texture.height;
                }
                else
                {
                    //empty gameobject
                    w = rctran.rect.width;
                    h = rctran.rect.height;
                }
            }
        }


        var w_parent = rctranParent.rect.width;
        var h_parent = rctranParent.rect.height;
        w_parent -= (this.offsetMin.x + this.offsetMax.x);
        h_parent -= (this.offsetMin.y + this.offsetMax.y);

        float scale = 1f;
        if (w != 0 && h != 0)
        {
            if (isMaxFit == true)
            {
                scale = Common.GetMaxFitScale(w, h, w_parent, h_parent) * ratio;
            }
            else
            {
                scale = Common.GetBestFitScale(w, h, w_parent, h_parent) * ratio;
            }
        }



        Debug.Log("LayOutScale scale=" + scale + " w_parent=" + w_parent + " h_parent=" + h_parent + " w=" + w + " h=" + h);
        obj.transform.localScale = new Vector3(scale, scale, 1f);
    }


    void ScaleObjByTarget(GameObject obj)
    {
        if (target == null)
        {
            return;
        }

        float scalex = target.transform.localScale.x * ratio;
        float scaley = target.transform.localScale.x * ratio;
        //Debug.Log("LayOutScale scale=" + scale + " w_parent=" + w_parent + " h_parent=" + h_parent + " w=" + w + " h=" + h);
        obj.transform.localScale = new Vector3(scalex, scaley, 1f);
    }
}
