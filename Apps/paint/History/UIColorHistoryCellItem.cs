using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEngine.UI.Button;

public class UIColorHistoryCellItem : UICellItemBase
{

    public const int ITEM_TYPE_SORT = 0;
    public const int ITEM_TYPE_WORD = 1;
    public Image imageBg;
    public Text textTitle;
    public int itemType;
    public float itemWidth;
    public float itemHeight;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }


    public void SetItemType(int type)
    {
        itemType = type;
        switch (itemType)
        {
            case ITEM_TYPE_SORT:
                //textTitle.
                break;
            case ITEM_TYPE_WORD:
                // textTitle.text = "";
                break;
        }
    }

    public override void UpdateItem(List<object> list)
    {
        DBItemInfo info = list[index] as DBItemInfo;
        if (FileUtil.FileIsExist(info.filesave))
        {
            Texture2D tex = LoadTexture.LoadFromFile(info.filesave);
            imageBg.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            int w = imageBg.sprite.texture.width;
            int h = imageBg.sprite.texture.height;
            RectTransform rctan = imageBg.GetComponent<RectTransform>();
            rctan.sizeDelta = new Vector2(tex.width, tex.height);
 
        }
        LayOut();
    }
    public override bool IsLock()
    {
        return false;//imageBgLock.gameObject.activeSelf;
    }

    public override void LayOut()
    {
        RectTransform rctran = imageBg.GetComponent<RectTransform>();
        float ratio = 0.8f;

        float scale = Common.GetBestFitScale(rctran.rect.width, rctran.rect.height, width, height) * ratio;
        imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);

    }

}
