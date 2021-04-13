

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureCache
{
    Dictionary<string, Texture2D> dicItem;
    Dictionary<string, Texture2D> dicItemMat;
    static private TextureCache _main = null;
    public static TextureCache main
    {
        get
        {
            if (_main == null)
            {
                _main = new TextureCache();
                _main.Init();
            }
            return _main;
        }
    }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Init()
    {
        dicItem = new Dictionary<string, Texture2D>();
        dicItemMat = new Dictionary<string, Texture2D>();
    }

    public bool IsInCache(string filepath)
    {
        return dicItem.ContainsKey(filepath);
    }
    public void AddCache(string key, Texture2D tex)
    {
        if (!IsInCache(key))
        {
            if (tex != null)
            {
                dicItem.Add(key, tex);
            }
        }
    }
    public Texture2D Load(string filepath, Material mat)
    {
        Texture2D tex = null;
        string key = filepath;
        if (dicItemMat.ContainsKey(key))
        {
            tex = dicItemMat[key];
        }
        else
        {
            Texture2D texOld = Load(filepath);
            RenderTexture rtTmp = new RenderTexture(texOld.width, texOld.height, 0);
            Graphics.Blit(texOld, rtTmp, mat);
            tex = TextureUtil.RenderTexture2Texture2D(rtTmp);

            if (tex != null)
            {
                dicItemMat.Add(key, tex);
            }

        }
        return tex;
    }

    public Texture2D LoadImageKey(string key)
    {
        return Load(ImageRes.main.GetImage(key));
    }

    public Texture2D Load(string filepath, bool isCache = true)
    {
        if (Common.isBlankString(filepath))
        {
            return null;
        }
        Texture2D tex = null;
        string key = filepath;
        if (isCache)
        {
            if (dicItem.ContainsKey(key))
            {
                tex = dicItem[key];
            }
        }
        if (tex == null)
        {
            if (FileUtil.FileIsExistAsset(key))
            {
                tex = LoadTexture.LoadFromAsset(key);
            }
            else
            {
                if (tex == null)
                {
                    tex = LoadTexture.LoadFromResource(key);
                }

                if (tex == null)
                {
                    tex = LoadTexture.LoadFromFile(key);
                }

            }
            if (isCache)
            {
                if (tex != null)
                {
                    dicItem.Add(key, tex);
                }

            }

        }
        return tex;
    }

    public void DestoryAllItem()
    {
        foreach (KeyValuePair<string, Texture2D> item in dicItem)
        {
            string key = item.Key;
            Texture2D tex = item.Value;
            if (tex != null)
            {
                GameObject.DestroyImmediate(tex);
                tex = null;
            }
        }
        dicItem.Clear();
    }

    public void DestoryAllItemMat()
    {
        foreach (KeyValuePair<string, Texture2D> item in dicItemMat)
        {
            string key = item.Key;
            Texture2D tex = item.Value;
            if (tex != null)
            {
                GameObject.DestroyImmediate(tex);
                tex = null;
            }
        }
        dicItemMat.Clear();
    }

    public void DeleteItem(string key)
    {
        if (!dicItem.ContainsKey(key))
        {
            return;
        }
        Texture2D tex = dicItem[key];
        if (tex != null)
        {
            GameObject.DestroyImmediate(tex);
            tex = null;
        }
        dicItem.Remove(key);
    }


    public void DeleteItemMat(string key)
    {
        if (!dicItemMat.ContainsKey(key))
        {
            return;
        }
        Texture2D tex = dicItemMat[key];
        if (tex != null)
        {
            GameObject.DestroyImmediate(tex);
            tex = null;
        }
        dicItemMat.Remove(key);
    }
}
