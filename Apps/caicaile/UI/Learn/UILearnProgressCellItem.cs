using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEngine.UI.Button;

public class UILearnProgressCellItem : UICellItemBase
{

    public Image imageBg;
    public Image imageIcon;
    public GameObject objIconContent;
    public Text textTitle;
    public Text textDetail;
    public float itemWidth;
    public float itemHeight;
    public int itemType;
    //public TableView tableView; 

    public Color colorSel;
    public Color colorUnSel;

    Shader shaderColor;



    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        string strshader = "Custom/ShapeColor";
        shaderColor = Shader.Find(strshader);
        LevelManager.main.ParseGuanka();

    }


    public override void UpdateItem(List<object> list)
    {
        ItemInfo info = list[index] as ItemInfo;
        UpdateInfo(info);

    }
    public override bool IsLock()
    {
        return false;//imageBgLock.gameObject.activeSelf;
    }

    public override void LayOut()
    {
        if (imageIcon.sprite == null)
        {
            return;
        }
        if (imageIcon.sprite.texture == null)
        {
            return;
        }
        int width = imageIcon.sprite.texture.width;
        int height = imageIcon.sprite.texture.height;
        RectTransform rctran = objIconContent.transform as RectTransform;
        LayOutScale lyscale = imageIcon.gameObject.GetComponent<LayOutScale>();
        if (lyscale != null)
        {
            lyscale.LayOut();
        }

    }
    public void SetItemType(int type)
    {
        itemType = type;

    }

    void SetSelect(bool isSel)
    {
        if (isSel)
        {
            textTitle.color = colorSel;
        }
        else
        {
            textTitle.color = colorUnSel;
        }
    }

    public void UpdateInfo(ItemInfo info)
    {
        Debug.Log("UpdateInfo info.pic=" + info.pic + " info.id=" + info.id);
        Texture2D tex = TextureCache.main.Load(info.pic);
        UpdateIcon(tex, colorSel);
        string str = LanguageManager.main.LanguageOfGameItem(info);
        textTitle.text = str;
        int status = PlayerPrefs.GetInt(GameBase.KEY_GAME_STATUS_ITEM + info.id);
        if (status != GameBase.GAME_STATUS_FINISH)
        {
            textTitle.text = "******";
        }
        textDetail.text = LanguageManager.main.StringOfGameStatusItem(info);
        LayOut();


    }

    Texture2D GetIconFillColor(Texture2D tex, Color color)
    {
        int w = tex.width;
        int h = tex.height;
        RenderTexture rt = new RenderTexture(w, h, 0);
        Material mat = new Material(shaderColor);
        mat.SetColor("_ColorShape", color);
        Graphics.Blit(tex, rt, mat);
        Texture2D texRet = TextureUtil.RenderTexture2Texture2D(rt, tex.format, new Rect(0, 0, rt.width, rt.height));
        return texRet;
    }

    void UpdateIcon(Texture2D tex, Color color)
    {
        Debug.Log("UpdateIcon color=" + color);
        //Texture2D texNew = GetIconFillColor(tex, color);
        // imageIcon.sprite = LoadTexture.CreateSprieFromTex(texNew);
        // RectTransform rctan = imageIcon.GetComponent<RectTransform>();
        // rctan.sizeDelta = new Vector2(texNew.width, texNew.height);
        TextureUtil.UpdateImageTexture(imageIcon, tex, true);

    }
}
