using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIFillColor : UIView
{
    public Camera cam;
    public GameObject objSpriteMask;
    public Texture2D texPicMask;
    public Color colorMask;
    public Color colorFill;

    int layerLine = 10;
    Material matFillColor;
    ColorImage colorImageMask;
    public bool isHasPaint = false;
    void Awake()
    {
        mainCam.cullingMask &= ~(1 << layerLine); // 关闭层x
        // mainCam.cullingMask |= (1 << layer);  // 打开层x

        cam.cullingMask = (1 << layerLine);

        objSpriteMask.layer = layerLine;
        matFillColor = new Material(Shader.Find("Custom/PiantFillColor"));

        //matFillColor.SetTexture("_MainTex", rtMainPaint);
        colorFill = Color.red;
        Renderer rd = objSpriteMask.GetComponent<Renderer>();
        rd.material = matFillColor;

        isHasPaint = false;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    Color GetColorMask(Vector2 uv)
    {
        int x = (int)(uv.x * texPicMask.width);
        int y = (int)(uv.y * texPicMask.height);
        if (x >= texPicMask.width)
        {
            x = texPicMask.width - 1;
        }
        if (y >= texPicMask.height)
        {
            y = texPicMask.height - 1;
        }

        Vector2 ptImage = new Vector2(x, y);
        Color color = colorImageMask.GetImageColor(ptImage);

        Debug.Log("GetColorMask:x=" + x + " y=" + y + " texPicMask.width=" + texPicMask.width + " .height=" + texPicMask.height + " color=" + color);
        return color;
    }
    public void UpdateMask(Texture2D tex)
    {
        texPicMask = tex;
        matFillColor.SetTexture("_TexMask", tex);
        if (colorImageMask == null)
        {
            colorImageMask = new ColorImage();
            colorImageMask.Init(texPicMask);
        }

        TextureUtil.UpdateSpriteTexture(objSpriteMask, texPicMask);
    }

    public void Clear()
    {
        if (matFillColor != null)
        {
            matFillColor.SetInt("_IsClear", 1);
        }
    }
    void onFillColor(Vector2 pt)
    {

        Vector2 uv = Vector2.zero;

        //RaycastHit hitInfo;
        // var ray = mainCam.ScreenPointToRay(Input.mousePosition);
        // if (Physics.Raycast(ray, out hitInfo))
        // {
        //     uv = hitInfo.textureCoord;
        // }
        Vector2 inputPos = Common.GetInputPosition();
        Vector3 posworld = mainCam.ScreenToWorldPoint(inputPos);
        Vector3 posSprite = objSpriteMask.transform.InverseTransformPoint(posworld);
        posSprite.z = 0;
        SpriteRenderer rd = objSpriteMask.GetComponent<SpriteRenderer>();
        float x = posSprite.x + rd.size.x / 2;
        float y = posSprite.y + rd.size.y / 2;

        uv.x = x / rd.size.x;
        uv.y = y / rd.size.y;

        Debug.Log("posSprite=" + posSprite + "rd=" + rd.size + " uv=" + uv);
        Color colorMask = GetColorMask(uv);
        if (colorMask == Color.black)
        {
            //点中边界的黑色
            return;
        }
        matFillColor.SetInt("_IsClear", 0);
        matFillColor.SetColor("_ColorMask", colorMask);
        matFillColor.SetColor("_ColorFill", colorFill);
        isHasPaint = true;
    }

    public void OnUITouchEvent(UITouchEvent ev, PointerEventData eventData, int status)
    {
        switch (status)
        {
            case UITouchEvent.STATUS_TOUCH_DOWN:
                onTouchDown();
                break;
            case UITouchEvent.STATUS_TOUCH_MOVE:
                onTouchMove();
                break;
            case UITouchEvent.STATUS_TOUCH_UP:
                onTouchUp();
                break;
        }
    }

    void onTouchDown()
    {


    }
    void onTouchMove()
    {

    }
    void onTouchUp()
    {
        onFillColor(Vector2.zero);
    }
}
