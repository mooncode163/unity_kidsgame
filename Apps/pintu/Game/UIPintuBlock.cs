using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public interface IUIPintuBlockDelegate
{
    void OnUIPintuBlockTouchUp(UIPintuBlock ui, PointerEventData eventData);
    void OnUIPintuBlockTouchDown(UIPintuBlock ui, PointerEventData eventData);
    void OnUIPintuBlockTouchMove(UIPintuBlock ui, PointerEventData eventData);
}

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class UIPintuBlock : UIView, IPointerUpHandler, IPointerDownHandler, IDragHandler
{
    //四边凹凸状态
    public const int SIDE_TYPE_NORMAL = 0;
    public const int SIDE_TYPE_IN = 1;//向内凹
    public const int SIDE_TYPE_OUT = 2;//向外凹

    public const int TAG_ITEM_LOCK = -1;
    public const int TAG_ITEM_UNLOCK = 0;

 
    public IUIPintuBlockDelegate iDelegate;
    public int tagItem = TAG_ITEM_UNLOCK;
    private Mesh mesh;
    MeshRenderer meshRender;
    Material meshMat;
    private Vector3[] vertices;
    private int[] triangles;
    public float centerWidth = 2f;//中间正方形区域 世界大小
    public float centerHeight = 2f;//中间正方形区域 世界大小
    public float sideWidth = 0f;//四周边框区域 世界大小
    public float sideHeight = 0f;

    public Vector3 posCenter = Vector3.zero;
    public bool enableTouch = true;
    public int indexRow;
    public int indexCol;
    public int row;
    public int col;

    public int sideTypeLeft;
    public int sideTypeRight;
    public int sideTypeTop;
    public int sideTypeBottom;

    bool isTouchDown;
    Vector2 posTouchDownScreen;
    Vector3 posTouchDown;
    //BlockMask_V
    Texture2D texBlockMaskH;
    //Texture2D texBlockMaskV;


    static public string picShape
    {
        get
        {
            string key = "KEY_PINTU_BLOCK_SHAPE1";
            return PlayerPrefs.GetString(key, "AppCommon/UI/Game/Shape/sanjiaoxing");
        }

        set
        {
            string key = "KEY_PINTU_BLOCK_SHAPE1";
            PlayerPrefs.SetString(key, value);
        }

    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        isTouchDown = false;
        mesh = GetComponent<MeshFilter>().mesh;
        meshRender = GetComponent<MeshRenderer>();

        string strshader = "Custom/PintuBlock";
        meshMat = new Material(Shader.Find(strshader));

    }
    // Use this for initialization
    void Start()
    {

        // Draw();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateTexture(Texture2D tex)
    {
        sideWidth = centerWidth / 4;
        sideHeight = centerHeight / 4;

        Debug.Log("centerWidth=" + centerWidth);
        meshMat.SetTexture("_MainTex", tex);


        //meshMat.SetFloat("centerWidth", centerWidth);
        meshMat.SetFloat("sideWidth", (sideWidth / centerWidth) / col);
        meshMat.SetFloat("sideHeight", (sideHeight / centerHeight) / row);
        meshMat.SetInt("sideTypeLeft", sideTypeLeft);
        meshMat.SetInt("sideTypeRight", sideTypeRight);
        meshMat.SetInt("sideTypeTop", sideTypeTop);
        meshMat.SetInt("sideTypeBottom", sideTypeBottom);

        meshMat.SetInt("indexRow", indexRow);
        meshMat.SetInt("indexCol", indexCol);
        meshMat.SetInt("row", row);
        meshMat.SetInt("col", col);

        meshRender.material = meshMat;

        UpdateTextureMask(picShape);//"AppCommon/UI/Game/Shape/sanjiaoxing"
        Draw();
    }
    void UpdateTextureMask(string pic)
    {
        int w, h;

        Texture2D texFile = TextureCache.main.Load(pic);
        // Texture2D texFile = TextureCache.main.Load("AppCommon/UI/Game/BlockMask_H");

        bool isUpdateMask = true;

        w = (int)Common.WorldToScreenWidth(mainCam, centerWidth);
        h = (int)Common.WorldToScreenWidth(mainCam, sideHeight);

        if (texBlockMaskH != null)
        {
            if ((w == texBlockMaskH.width) && (h == texBlockMaskH.height))
            {
                isUpdateMask = false;
            }
        }
        if (isUpdateMask)
        {
            int h_tex = h;
            int w_tex = h_tex * texFile.width / texFile.height;
            Texture2D texConvert = TextureUtil.ConvertSize(texFile, w_tex, h_tex);
            Texture2D texBg = new Texture2D(w, h, TextureFormat.ARGB32, false);//ARGB32
                                                                               //背景全透明
            {
                w = texBg.width;
                h = texBg.height;

                ColorImage colorImage = new ColorImage();
                colorImage.Init(texBg);


                for (int i = 0; i < w; i++)
                {
                    for (int j = 0; j < h; j++)
                    {
                        Vector2 pt = new Vector2(i, j);
                        Color color = colorImage.GetImageColorOrigin(pt);
                        color.a = 0f;
                        colorImage.SetImageColor(pt, color);
                    }
                }
                colorImage.UpdateTexture();

            }



            texBlockMaskH = PintuUtil.MergeTextureGPU(texBg, texConvert);
            // w = (int)Common.WorldToScreenWidth(mainCam, sideWidth);
            // h = (int)Common.WorldToScreenWidth(mainCam, centerHeight);
            // textmp = TextureCache.main.Load("AppCommon/UI/Game/BlockMask_V");
            // texBlockMaskV = TextureUtil.ConvertSize(textmp, w, h);

        }

        meshMat.SetTexture("_texMaskH", texBlockMaskH);
        // meshMat.SetTexture("_texMaskV", texBlockMaskV);
    }

    Vector3[] GetverticeOfPoint(Vector2 pt)
    {
        int count = 4;
        float z = 0f;
        Vector3[] v = new Vector3[count];
        float x, y;
        //left_bottom
        x = pt.x - centerWidth / 2;
        if (indexCol != 0)
        {
            x -= sideWidth;
        }
        y = pt.y - centerHeight / 2;
        if (indexRow != 0)
        {
            y -= sideHeight;
        }
        v[0] = new Vector3(x, y, z);

        //right_bottom
        x = pt.x + centerWidth / 2;
        if (indexCol != col - 1)
        {
            x += sideWidth;
        }
        y = pt.y - centerHeight / 2;
        if (indexRow != 0)
        {
            y -= sideHeight;
        }
        v[1] = new Vector3(x, y, z);


        //top_left
        x = pt.x - centerWidth / 2;
        if (indexCol != 0)
        {
            x -= sideWidth;
        }
        y = pt.y + centerHeight / 2;
        if (indexRow != row - 1)
        {
            y += sideHeight;
        }
        v[2] = new Vector3(x, y, z);


        //top_right
        x = pt.x + centerWidth / 2;
        if (indexCol != col - 1)
        {
            x += sideWidth;
        }
        y = pt.y + centerHeight / 2;
        if (indexRow != row - 1)
        {
            y += sideHeight;
        }
        v[3] = new Vector3(x, y, z);

        return v;
    }
    public void Draw()
    {

        //sideWidth = 0;
        mesh.Clear();
        int count = 1;
        vertices = new Vector3[count * 4];
        triangles = new int[count * 6];
        Vector2[] uvs = new Vector2[count * 4];
        int tri_index = 0;
        int i = 0;
        float x, y;
        {
            Vector3[] v = GetverticeOfPoint(posCenter);

            for (int j = 0; j < 4; j++)
            {
                vertices[i * 4 + j] = v[j];
            }


            //纹理坐标
            {
                float tex_w = 1.0f / col;
                float tex_h = 1.0f / row;
                float ratiox = sideWidth / centerWidth;
                float ratioy = sideHeight / centerHeight;
                float oft_x = tex_w * ratiox;
                float oft_y = tex_h * ratioy;
                //left_bottom 
                x = indexCol * tex_w - oft_x;
                if (indexCol == 0)
                {
                    //左边界
                    x = indexCol * tex_w;
                }
                y = indexRow * tex_h - oft_y;
                if (indexRow == 0)
                {
                    //底部边界
                    y = indexRow * tex_h;
                }
                uvs[i * 4 + 0] = new Vector2(x, y);//0f, 0f

                //right_bottom
                x = (indexCol + 1) * tex_w + oft_x;
                if (indexCol == col - 1)
                {
                    //最右边界
                    x = (indexCol + 1) * tex_w;
                }
                y = (indexRow) * tex_h - oft_y;
                if (indexRow == 0)
                {
                    //底部边界
                    y = indexRow * tex_h;
                }
                uvs[i * 4 + 1] = new Vector2(x, y);//(1f, 0f

                //top_left
                x = indexCol * tex_w - oft_x;
                if (indexCol == 0)
                {
                    //左边界
                    x = indexCol * tex_w;
                }
                y = (indexRow + 1) * tex_h + oft_y;
                if (indexRow == row - 1)
                {
                    //顶部边界
                    y = (indexRow + 1) * tex_h;
                }
                uvs[i * 4 + 2] = new Vector2(x, y);//(0f, 1f

                //top_right
                x = (indexCol + 1) * tex_w + oft_x;
                if (indexCol == col - 1)
                {
                    //最右边界
                    x = (indexCol + 1) * tex_w;
                }
                y = (indexRow + 1) * tex_h + oft_y;
                if (indexRow == row - 1)
                {
                    //顶部边界
                    y = (indexRow + 1) * tex_h;
                }
                uvs[i * 4 + 3] = new Vector2(x, y);//1f, 1f

                meshMat.SetFloat("uvCenterX", indexCol * tex_w + tex_w / 2);
                meshMat.SetFloat("uvCenterY", indexRow * tex_h + tex_h / 2);
            }

            int idx = 0;
            //三角型1
            {
                //top_left
                idx = i * 6 + 0;
                triangles[idx] = tri_index + 2;
                //right_bottom
                idx = i * 6 + 1;
                triangles[idx] = tri_index + 1;
                //left_bottom
                idx = i * 6 + 2;
                triangles[idx] = tri_index + 0;
            }

            //三角型2
            {
                //top_left
                idx = i * 6 + 3;
                triangles[idx] = tri_index + 2;
                //top_right
                idx = i * 6 + 4;
                triangles[idx] = tri_index + 3;
                //bottom_right
                idx = i * 6 + 5;
                triangles[idx] = tri_index + 1;
            }



            tri_index += 4;
        }



        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
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
            iDelegate.OnUIPintuBlockTouchDown(this, eventData);
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
            iDelegate.OnUIPintuBlockTouchUp(this, eventData);
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
            iDelegate.OnUIPintuBlockTouchMove(this, eventData);
        }

    }
}
