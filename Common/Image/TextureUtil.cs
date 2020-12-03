using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public delegate void OnTextureHttpRequestFinishedDelegate(bool isSuccess, Texture2D tex, object data);
public class TextureUtil : MonoBehaviour
{
    public HttpRequest httpReqSprite;
    public HttpRequest httpReqImage;
    GameObject objSprite;
    object dataSprite;

    Image imageHttp;
    object dataImage;
    bool isAutoUpdateImage;


    public OnTextureHttpRequestFinishedDelegate callbackHttp { get; set; }

    static public Sprite CreateSpriteFromResource(string file)
    {
        Texture2D tex = LoadTexture.LoadFromResource(file);
        Sprite sp = CreateSpriteFromTex(tex);
        return sp;
    }

    static public Sprite CreateSpriteFromResource(string file, Vector4 border)
    {
        Texture2D tex = LoadTexture.LoadFromResource(file);
        Sprite sp = CreateSpriteFromTex(tex, border);
        return sp;
    }
    static public Sprite CreateSpriteFromTex(Texture2D tex)
    {
        Sprite sp = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        return sp;
    }

    static public Sprite CreateSpriteFromTex(Texture2D tex, Vector4 border)
    {
        Sprite sp = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.Tight, border);
        return sp;
    }

    static public Sprite CreateSpriteFromAsset(string file)
    {
        Texture2D tex = LoadTexture.LoadFromAsset(file);
        Sprite sp = CreateSpriteFromTex(tex);
        return sp;
    }

    static public Rect GetRectNotAlpha(Texture2D tex)
    {
        int fillXLeft = tex.width;
        int fillXRight = 0;
        int fillYBottom = tex.height;
        int fillYTop = 0;
        int x, y, w, h;
        ColorImage colorImage = new ColorImage();
        colorImage.Init(tex);
        for (int j = 0; j < tex.height; j++)
        {
            for (int i = 0; i < tex.width; i++)
            {
                Vector2 pt = new Vector2(i, j);
                Color color = colorImage.GetImageColorOrigin(pt);
                if (color.a >= 1f)
                {
                    x = i;
                    y = j;
                    if (x < fillXLeft)
                    {
                        fillXLeft = x;
                    }
                    if (x > fillXRight)
                    {
                        fillXRight = x;
                    }


                    if (y < fillYBottom)
                    {
                        fillYBottom = y;
                    }
                    if (y > fillYTop)
                    {
                        fillYTop = y;
                    }
                }

            }
        }
        Debug.Log("tex.width=" + tex.width + " fillXLeft=" + fillXLeft + " fillXRight=" + fillXRight);
        return new Rect(fillXLeft, fillYBottom, (fillXRight - fillXLeft), (fillYTop - fillYBottom));

    }


    //jpg转为透明png
    static public Texture2D ConvertJpg2AlphaPng(Texture2D tex)
    {
        int w = tex.width;
        int h = tex.height;
        Texture2D texRet = new Texture2D(w, h, TextureFormat.ARGB32, false);
        ColorImage colorImageRet = new ColorImage();
        colorImageRet.Init(texRet);

        ColorImage colorImage = new ColorImage();
        colorImage.Init(tex);


        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                Vector2 pt = new Vector2(i, j);
                Color color = colorImage.GetImageColorOrigin(pt);
                color.a = 1f;
                colorImageRet.SetImageColor(pt, color);
            }
        }
        colorImageRet.UpdateTexture();

        return texRet;
    }


    static public Texture2D RenderTexture2Texture2D(RenderTexture rt)
    {
        return RenderTexture2Texture2D(rt, TextureFormat.ARGB32, new Rect(0, 0, rt.width, rt.height));
    }

    static public Texture2D RenderTexture2Texture2D(RenderTexture rt, Rect rc)
    {
        return RenderTexture2Texture2D(rt, TextureFormat.ARGB32, rc);
    }
    static public Texture2D RenderTexture2Texture2D(RenderTexture rt, TextureFormat format, Rect rc)
    {
        RenderTexture prev = RenderTexture.active;
        RenderTexture.active = rt;
        Texture2D tex = new Texture2D((int)rc.width, (int)rc.height, format, false);
        //new Rect(0, 0, rt.width, rt.height)
        tex.ReadPixels(rc, 0, 0);
        tex.Apply();
        RenderTexture.active = prev;
        // RenderTexture.active = null;
        return tex;
    }

    static public Texture2D ConvertSize(Texture2D tex, int w_new, int h_new)
    {
        return ConvertSize(tex, w_new, h_new, TextureFormat.ARGB32);
    }
    static public Texture2D ConvertSize(Texture2D tex, int w_new, int h_new, TextureFormat format)
    {
        int w = tex.width;
        int h = tex.height;
        RenderTexture rt = new RenderTexture(w_new, h_new, 0);
        Graphics.Blit(tex, rt);
        Texture2D texRet = RenderTexture2Texture2D(rt, format, new Rect(0, 0, rt.width, rt.height));
        return texRet;
    }
    static public Texture2D GetSubRenderTexture(RenderTexture rt, Rect rc, bool flipY = false)
    {
        // rc.x =0;

        // rc.width = Screen.width/2;
        Debug.Log("GetSubRenderTexture rc=" + rc);
        // rc.y = 0;
        // rc.height = Screen.height*0.7f;
        if (flipY)
        {
            rc.y = rt.height - rc.y - rc.height;
        }
        return RenderTexture2Texture2D(rt, rc);
    }

    static public Texture2D GetSubTexture(Texture2D tex, Rect rc, bool flipY = false)
    {
        int w = tex.width;
        int h = tex.height;
        if (flipY)
        {
            rc.y = tex.height - rc.y - rc.height;
        }
        // RenderTexture rt = new RenderTexture(w, h, 0);
        RenderTexture rt = new RenderTexture(w, h, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
        Graphics.Blit(tex, rt);
        Texture2D texRet = RenderTexture2Texture2D(rt, rc);
        return texRet;
    }

    static public void SaveTextureToFile(Texture2D tex, string filepath)
    {
        string strExt = FileUtil.GetFileExt(filepath);
        byte[] bytes = null;
        if (strExt == "png")
        {
            bytes = tex.EncodeToPNG();
        }
        else if (strExt == "jpg")
        {
            bytes = tex.EncodeToJPG();
        }
        else
        {
            bytes = tex.EncodeToPNG();
        }
        System.IO.File.WriteAllBytes(filepath, bytes);
    }

    static public void UpdateSpriteTexture(GameObject obj, string filepath)
    {
        Texture2D tex = TextureCache.main.Load(filepath);
        UpdateSpriteTexture(obj, tex);

    }

    static public void UpdateSpriteTexture(GameObject obj, Texture2D tex)
    {

        SpriteRenderer rd = obj.GetComponent<SpriteRenderer>();
        if (rd != null)
        {
            rd.sprite = CreateSpriteFromTex(tex);
        }

    }

    void OnHttpRequestFinished(HttpRequest req, bool isSuccess, byte[] data)
    {
        if (req == httpReqSprite)
        {
            Texture2D tex = LoadTexture.LoadFromData(data);
            OnGetSpriteDidFinish(isSuccess, tex, false, req.strUrl);

        }
        if (req == httpReqImage)
        {
            Texture2D tex = LoadTexture.LoadFromData(data);
            OnGetImageDidFinish(isSuccess, tex, false, req.strUrl);

        }

    }
    void OnGetSpriteDidFinish(bool isSuccess, Texture2D tex, bool isLocal, string filepath)
    {
        if (isSuccess && (tex != null))
        {
            TextureCache.main.AddCache(filepath, tex);
            SpriteRenderer render = objSprite.GetComponent<SpriteRenderer>();
            render.sprite = CreateSpriteFromTex(tex);

        }
        if (!isLocal)
        {
            if (this.callbackHttp != null)
            {
                this.callbackHttp(isSuccess, tex, dataSprite);
            }
        }

    }
    void OnGetImageDidFinish(bool isSuccess, Texture2D tex, bool isLocal, string filepath)
    {
        if (isSuccess && (tex != null))
        {
            TextureCache.main.AddCache(filepath, tex);
            if (isAutoUpdateImage)
            {
                imageHttp.sprite = CreateSpriteFromTex(tex);
            }


        }

        //if (!isLocal)
        {
            if (this.callbackHttp != null)
            {
                this.callbackHttp(isSuccess, tex, dataImage);
            }
        }

    }
    public void UpdateSpriteTextureWeb(GameObject obj, string url, OnTextureHttpRequestFinishedDelegate callback, object data)
    {
        this.callbackHttp = callback;
        objSprite = obj;
        dataSprite = data;
        bool is_cache = TextureCache.main.IsInCache(url);
        if (is_cache)
        {
            Texture2D tex = TextureCache.main.Load(url);
            OnGetSpriteDidFinish(true, tex, true, url);
        }
        else
        {
            if (Common.isWeb)
            {
                httpReqSprite = new HttpRequest(OnHttpRequestFinished);
                httpReqSprite.Get(url);
            }

        }


    }
    public void UpdateImageTextureWeb(Image image, string url, OnTextureHttpRequestFinishedDelegate callback, object data)
    {
        UpdateImageTextureWeb(image, url, callback, data, true);
    }

    public void UpdateImageTextureWeb(Image image, string url, OnTextureHttpRequestFinishedDelegate callback, object data, bool isAutoUpdate)
    {
        this.callbackHttp = callback;
        imageHttp = image;
        dataImage = data;
        isAutoUpdateImage = isAutoUpdate;
        bool is_cache = TextureCache.main.IsInCache(url);
        if (is_cache)
        {
            Texture2D tex = TextureCache.main.Load(url);
            OnGetImageDidFinish(true, tex, true, url);
        }
        else
        {
            if (Common.isWeb)
            {
                httpReqImage = new HttpRequest(OnHttpRequestFinished);
                httpReqImage.Get(url);
            }

        }

    }
    static public void UpdateImageTexture(Image image, string filepath, bool isUpdateSize)
    {
        UpdateImageTexture(image, filepath, isUpdateSize, Vector4.zero);
    }

    static public void UpdateImageTexture(Image image, string filepath, bool isUpdateSize, Vector4 border)
    {
        Texture2D tex = TextureCache.main.Load(filepath);
        UpdateImageTexture(image, tex, isUpdateSize, border);
    }
    static public void UpdateImageTexture(Image image, Texture2D tex, bool isUpdateSize)
    {
        UpdateImageTexture(image, tex, isUpdateSize, Vector4.zero);
    }

    static public void UpdateImageTexture(Image image, Texture2D tex, bool isUpdateSize, Vector4 border)
    {
        if (tex)
        {
            RectTransform rctranOrigin = image.GetComponent<RectTransform>();
            Vector2 offsetMin = rctranOrigin.offsetMin;
            Vector2 offsetMax = rctranOrigin.offsetMax;
            Sprite sp = CreateSpriteFromTex(tex, border);
            image.sprite = sp;
            if (border != Vector4.zero)
            {
                image.type = Image.Type.Sliced;
            }
            if (isUpdateSize)
            {
                RectTransform rctran = image.GetComponent<RectTransform>();

                rctran.sizeDelta = new Vector2(tex.width, tex.height);

                if ((rctran.anchorMin == new Vector2(0.5f, 0.5f)) && (rctran.anchorMax == new Vector2(0.5f, 0.5f)))
                {
                }
                else
                {
                    //sizeDelta 会自动修改offsetMin和offsetMax 所以需要还原
                    rctran.offsetMin = offsetMin;
                    rctran.offsetMax = offsetMax;
                }
                Debug.Log("rctranOrigin rctran.offsetMin=" + rctran.offsetMin + " rctran.offsetMax=" + rctran.offsetMax);

            }

        }
    }


    static public void UpdateButtonTexture(Button btn, string filepath, bool isUpdateSize)
    {
        UpdateButtonTexture(btn, filepath, isUpdateSize, Vector4.zero);
    }

    static public void UpdateButtonTexture(Button btn, string filepath, bool isUpdateSize, Vector4 border)
    {
        Texture2D tex = TextureCache.main.Load(filepath);
        UpdateButtonTexture(btn, tex, isUpdateSize, border);
    }

    static public void UpdateButtonTexture(Button btn, Texture2D tex, bool isUpdateSize)
    {
        UpdateButtonTexture(btn, tex, isUpdateSize, Vector4.zero);
    }
    static public void UpdateButtonTexture(Button btn, Texture2D tex, bool isUpdateSize, Vector4 border)
    {
        Image image = btn.GetComponent<Image>();
        UpdateImageTexture(image, tex, isUpdateSize, border);
    }

    //RawImage
    static public void UpdateRawImageTexture(RawImage image, string filepath, bool isUpdateSize)
    {
        Texture2D tex = TextureCache.main.Load(filepath);
        UpdateRawImageTexture(image, tex, isUpdateSize, Vector4.zero);
    }

    static public void UpdateRawImageTexture(RawImage image, Texture2D tex, bool isUpdateSize)
    {
        UpdateRawImageTexture(image, tex, isUpdateSize, Vector4.zero);
    }
    static public void UpdateRawImageTexture(RawImage image, Texture2D tex, bool isUpdateSize, Vector4 border)
    {
        if (tex)
        {
            RectTransform rctranOrigin = image.GetComponent<RectTransform>();
            Vector2 offsetMin = rctranOrigin.offsetMin;
            Vector2 offsetMax = rctranOrigin.offsetMax;
            image.texture = tex;
            if (border != Vector4.zero)
            {
                // image.type = Image.Type.Sliced;
            }
            if (isUpdateSize)
            {
                RectTransform rctran = image.GetComponent<RectTransform>();

                rctran.sizeDelta = new Vector2(tex.width, tex.height);

                if ((rctran.anchorMin == new Vector2(0.5f, 0.5f)) && (rctran.anchorMax == new Vector2(0.5f, 0.5f)))
                {
                }
                else
                {
                    //sizeDelta 会自动修改offsetMin和offsetMax 所以需要还原
                    rctran.offsetMin = offsetMin;
                    rctran.offsetMax = offsetMax;
                }
                Debug.Log("rctranOrigin rctran.offsetMin=" + rctran.offsetMin + " rctran.offsetMax=" + rctran.offsetMax);

            }

        }
    }

    //圆角   value = (156 * 1f / 1024);
    static public Texture2D RoundRectTexture(Texture2D tex, float value)
    {
        int w = tex.width;
        int h = tex.height;
        RenderTexture rt = new RenderTexture(w, h, 0);
        string strshader = "Custom/RoundRect";
        //string str = FileUtil.ReadStringAsset(ShotBase.STR_DIR_ROOT_SHADER+"/ShaderRoundRect.shader");
        Material mat = new Material(Shader.Find(strshader));//
        //Debug.Log("RoundRectTexture:value=" + value);
        //value = 0.1f;
        //设置半径 最大0.5f
        mat.SetFloat("_RADIUSBUCE", value);
        Graphics.Blit(tex, rt, mat);
        Texture2D texRet = TextureUtil.RenderTexture2Texture2D(rt);
        return texRet;
    }


}