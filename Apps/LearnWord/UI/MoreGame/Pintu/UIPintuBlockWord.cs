using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public interface IUIPintuBlockWordDelegate
{
    void OnUIPintuBlockWordTouchUp(UIPintuBlockWord ui, PointerEventData eventData);
    void OnUIPintuBlockWordTouchDown(UIPintuBlockWord ui, PointerEventData eventData);
    void OnUIPintuBlockWordTouchMove(UIPintuBlockWord ui, PointerEventData eventData);
}

public class UIPintuBlockWord : UIView, IPointerUpHandler, IPointerDownHandler, IDragHandler
{
    //四边凹凸状态
    public const int SIDE_TYPE_NORMAL = 0;
    public const int SIDE_TYPE_IN = 1;//向内凹
    public const int SIDE_TYPE_OUT = 2;//向外凹

    public const int TAG_ITEM_LOCK = -1;
    public const int TAG_ITEM_UNLOCK = 0;


    public IUIPintuBlockWordDelegate iDelegate;
    public int tagItem = TAG_ITEM_UNLOCK;

    public Vector3 posCenter = Vector3.zero;
    public bool enableTouch = true;
    public int indexRow;
    public int indexCol;
    public int index;

    public Vector3 posNormal;
    bool isTouchDown;
    Vector2 posTouchDownScreen;
    Vector3 posTouchDown;
    //BlockMask_V
    Texture2D texBlockMaskH;
    //Texture2D texBlockMaskV;

    UIViewSVG uiViewSVGPrefab;
  public  UIViewSVG uiSVG;
      Color colorWord = Color.black; 
    public void Awake()
    {
        base.Awake();
        if (uiViewSVGPrefab == null)
        {
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/SVG/UIViewSVG");
            if (obj != null)
            {
                uiViewSVGPrefab = obj.GetComponent<UIViewSVG>();
            }
        }
        uiSVG = (UIViewSVG)GameObject.Instantiate(uiViewSVGPrefab);
        UIViewController.ClonePrefabRectTransform(uiViewSVGPrefab.gameObject, uiSVG.gameObject);
        uiSVG.transform.SetParent(this.transform, false);
        uiSVG.renderMode = UIViewSVG.RenderMode.WORLD;
    }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>

    // Use this for initialization
    void Start()
    {
        isTouchDown = false;
        // Draw();
    }

   public void LoadWord(string svg )
    {

        // string svg = ParseWordPointInfo.main.GetWordSVG(infoWord.pointInfo, i);
        uiSVG.LoadData(svg);
        // 实际显示颜色
        uiSVG.SetColor(colorWord);
    }


    public Texture2D GetTexture()
    {
        Texture2D tex = null;
        if(uiSVG!=null){
            tex = uiSVG.texSVG;
        }
        return tex;
    }

    public void SetItemLock(bool isLock)
    {
        if (isLock)
        {
            tagItem = TAG_ITEM_LOCK;
        }
        else
        {
            tagItem = TAG_ITEM_UNLOCK;
        }
    }

    public bool IsItemLock()
    {
        bool ret = false;
        if (tagItem == TAG_ITEM_LOCK)
        {
            ret = true;
        }
        return ret;
    }

    //相当于touchDown
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!enableTouch)
        {
            return;
        }
        isTouchDown = true;
        posTouchDownScreen = eventData.position;
        posTouchDown = this.transform.localPosition;
        if (iDelegate != null)
        {
            iDelegate.OnUIPintuBlockWordTouchDown(this, eventData);
        }
    }
    //相当于touchUp
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!enableTouch)
        {
            return;
        }

        if (!isTouchDown)
        {
            return;
        }

        isTouchDown = false;
        bool isLock = IsItemLock();
        if (isLock)
        {
            return;
        }

        if (iDelegate != null)
        {
            iDelegate.OnUIPintuBlockWordTouchUp(this, eventData);
        }
    }
    //相当于touchMove
    public void OnDrag(PointerEventData eventData)
    {
        if (!enableTouch)
        {
            return;
        }
        if (!isTouchDown)
        {
            return;
        }
        bool isLock = IsItemLock();
        if (isLock)
        {
            return;
        }

        Vector3 pos = mainCam.ScreenToWorldPoint(eventData.position);
        Vector3 ptMove = pos - mainCam.ScreenToWorldPoint(posTouchDownScreen);

        float z = this.gameObject.transform.localPosition.z;
        Vector3 posNow = posTouchDown + ptMove;
        posNow.z = z;
        this.gameObject.transform.localPosition = posNow;

        if (iDelegate != null)
        {
            iDelegate.OnUIPintuBlockWordTouchMove(this, eventData);
        }

    }
}
