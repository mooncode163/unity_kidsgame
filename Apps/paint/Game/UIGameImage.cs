using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameImage : UIView
{
    public Camera cameraPic;
    public GameObject objSpritePic;
    public GameObject objSpriteSign;
    public GameObject objSpriteErase;
    public GameObject objErase;
    public RenderTexture rtMain;
    public Rect rectMain;//world中的显示区域 
    int layerLine = 9;
    Material matSign;
    Material matEraseMeshTex;
    public Texture2D texBrush;
    int mode;
    MeshTexture meshTex;
    void Awake()
    {
        matSign = new Material(Shader.Find("Custom/Sign"));
        matEraseMeshTex = new Material(Shader.Find("Custom/EraseMeshTex"));
        texBrush = TextureCache.main.Load("AppCommon/UI/Brush/brush_dot");

        SpriteRenderer rd = objSpriteSign.GetComponent<SpriteRenderer>();
        rd.material = matSign;
        objSpriteErase.SetActive(false);
        objErase.SetActive(false);
        meshTex = objErase.AddComponent<MeshTexture>();
        meshTex.UpdateMaterial(matEraseMeshTex);

    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void LayOut()
    {
        float x, y, z, w, h;
        {
            SpriteRenderer render = objSpriteErase.GetComponent<SpriteRenderer>();
            if (render.sprite && render.sprite.texture)
            {
                w = render.sprite.texture.width / 100f;
                h = render.sprite.texture.height / 100f;
                if ((w != 0) && (h != 0))
                {
                    //float scale = Common.ScreenToWorldWidth(mainCam, (w * 100f * AppCommon.scaleBase)) / w;
                    float scale = (rectMain.width / 10) / w;
                    objSpriteErase.transform.localScale = new Vector3(scale, scale, 1f);
                }


            }

        }


        {
            SpriteRenderer render = objSpriteSign.GetComponent<SpriteRenderer>();
            if (render.sprite && render.sprite.texture)
            {
                w = render.sprite.texture.width / 100f;
                h = render.sprite.texture.height / 100f;
                if ((w != 0) && (h != 0))
                {
                    float scale = (rectMain.width / 5) / w;
                    objSpriteSign.transform.localScale = new Vector3(scale, scale, 1f);
                }


            }

        }
    }
    public void Init()
    {
        cameraPic.targetTexture = rtMain;

        mainCam.cullingMask &= ~(1 << layerLine); // 关闭层x
        // mainCam.cullingMask |= (1 << layer);  // 打开层x

        cameraPic.cullingMask = (1 << layerLine);
        objSpritePic.layer = layerLine;
        objSpriteSign.layer = layerLine;
        objSpriteErase.layer = layerLine;
        objErase.layer = layerLine;
    }
    public void UpdateSignColor(Color cr)
    {
        if (matSign != null)
        {
            matSign.SetColor("_Color", cr);
        }
    }
    public void UpdateEraseMaterial(Material mat)
    {
        SpriteRenderer rd = objSpriteErase.GetComponent<SpriteRenderer>();
        rd.material = mat;
    }
    public Material GetEraseMaterial()
    {
        SpriteRenderer rd = objSpriteErase.GetComponent<SpriteRenderer>();
        Material mat = rd.material;
        return mat;
    }


    public void UpdateSignPic()
    {
        int idx = Random.Range(0, 3);
        Texture2D tex = TextureCache.main.Load("AppCommon/UI/Game/Sign/icon_sign" + idx);
        TextureUtil.UpdateSpriteTexture(objSpriteSign, tex);
        float rotation_min = -30f;
        float rotation_max = 30f;
        int rdm = Random.Range(0, 100);
        float rotation = rotation_min + (rotation_max - rotation_min) * rdm / 100;

        objSpriteSign.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, rotation));
        Vector3 posworld = Common.GetInputPositionWorld(mainCam);
        Vector3 poslocal = this.transform.InverseTransformPoint(posworld);
        poslocal.z = objSpriteSign.transform.localPosition.z;
        objSpriteSign.transform.localPosition = poslocal;
        LayOut();
    }

    public void UpdateErase()
    {

        meshTex.UpdateMaterial(matEraseMeshTex);
        matEraseMeshTex = meshTex.GetMaterial();
        meshTex.UpdateTexture(rtMain);
        Vector2 worldsize = Common.GetWorldSize(mainCam);
        meshTex.UpdateSize(worldsize.x, worldsize.y);

        float x, y, w, h;


        w = (texBrush.width) * 1f / (rtMain.width);
        h = (texBrush.height) * 1f / (rtMain.height);

        matEraseMeshTex.SetFloat("_EraseW", w);//0.5f
        matEraseMeshTex.SetFloat("_EraseH", h);
        matEraseMeshTex.SetTexture("_TexErase", texBrush);

        Vector3 posworld = Common.GetInputPositionWorld(mainCam);
        Vector3 poslocal = this.transform.InverseTransformPoint(posworld);
        w = meshTex.width;
        h = meshTex.height;
        x = (poslocal.x + w / 2) / w;
        y = (poslocal.y + h / 2) / h;

        matEraseMeshTex.SetFloat("_EraseUvX", x);
        matEraseMeshTex.SetFloat("_EraseUvY", y);

    }
    public void UpdateMode(int m)
    {
        mode = m;
        objSpriteSign.SetActive(false);
        objErase.SetActive(false);
        objSpriteErase.SetActive(false);
        {
            //显示在屏幕之外
            Vector3 pos = new Vector3(0, mainCam.orthographicSize * 2, objSpriteErase.transform.position.z);
            objSpriteErase.transform.position = pos;
        }



        switch (mode)
        {
            case GamePaint.MODE_PAINT:

                break;
            case GamePaint.MODE_FILLCOLR:
                break;

            case GamePaint.MODE_MAGIC:

                break;
            case GamePaint.MODE_ERASE:
                {
                    objSpriteErase.SetActive(true);
                    objErase.SetActive(true);


                }

                break;
            case GamePaint.MODE_SIGN:
                objSpriteSign.SetActive(true);
                break;


        }
    }
}
