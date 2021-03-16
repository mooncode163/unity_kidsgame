using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PintuUtil
{

    static public Texture2D MergeTextureGPU(Texture2D texBg, Texture2D texFt)
    {
        int w_bg = texBg.width;
        int h_bg = texBg.height;
        Texture2D texRet = new Texture2D(w_bg, h_bg, TextureFormat.ARGB32, false);


        float scale = 0, scalex = 0, scaley = 0;

        scalex = w_bg * 1.0f / texFt.width;
        scaley = h_bg * 1.0f / texFt.height;

        scale = Mathf.Min(scalex, scaley);
        int w_scale = (int)(scale * texFt.width);
        int h_scale = (int)(scale * texFt.height);
        // long tick = Common.GetCurrentTimeMs();
        // Texture2D texScale = LoadTexture.ConvertSize(texFt, w_scale, h_scale);
        // tick = Common.GetCurrentTimeMs()-tick;
        // Debug.Log("MergeTextureGPU:ConvertSize,tick="+tick+"ms");
        
        Material mat = new Material(Shader.Find("Custom/ImageConvert"));
        mat.SetTexture("_Brush",texFt);

        //_BrushW，_BrushH:图片的完整像素为0.5f
        float brush_w = w_scale*0.5f/w_bg;
        float brush_h = h_scale*0.5f/h_bg;
        Debug.Log("scale:brush_w="+brush_w+" brush_h="+brush_h);
        mat.SetFloat("_BrushW",brush_w);//0.5f
        mat.SetFloat("_BrushH",brush_h);
        //在位置设置为中心点
        mat.SetVector("_PaintUV",new Vector4(0.5f,0.5f,0,0));
        //
        RenderTexture rt = new RenderTexture(w_bg, w_bg, 0);
        Graphics.Blit(texBg, rt,mat);
        texRet = TextureUtil.RenderTexture2Texture2D(rt);

        return texRet;
    }
    // Use this for initialization
    //CPU 合并连个纹理
    static public Texture2D MergeTexture(Texture2D texBg, Texture2D texFt)
    {
        int w = texBg.width;
        int h = texBg.height;
        Texture2D texRet = new Texture2D(w, h, TextureFormat.ARGB32, false);
        ColorImage colorImageRet = new ColorImage();
        colorImageRet.Init(texRet);

        ColorImage colorImageBg = new ColorImage();
        colorImageBg.Init(texBg);

        ColorImage colorImageFt = new ColorImage();
        colorImageFt.Init(texFt);

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                float x, y;
                Vector2 pt = new Vector2(i, j);
                Color colorbg = colorImageBg.GetImageColorOrigin(pt);
                Color color = colorbg;
                float x_center = i - w / 2;
                float y_center = j - h / 2;
                x = x_center + texFt.width / 2;
                y = y_center + texFt.height / 2;
                if ((x >= 0) && (x < texFt.width) && (y >= 0) && (y < texFt.height))
                {
                    //重叠区域 进行alpha
                    Color colorft = colorImageFt.GetImageColorOrigin(new Vector2(x, y));
                    float alpha = colorft.a;
                    color.r = colorft.r * alpha + colorbg.r * (1 - alpha);
                    color.g = colorft.g * alpha + colorbg.g * (1 - alpha);
                    color.b = colorft.b * alpha + colorbg.b * (1 - alpha);
                }

                colorImageRet.SetImageColor(pt, color);
            }
        }
        colorImageRet.UpdateTexture();

        return texRet;
    }

}
