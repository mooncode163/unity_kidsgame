using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEngine.UI.Button;

public class UIWordWriteHistoryCellItem : UICellItemBase
{

    public const int ITEM_TYPE_SORT = 0;
    public const int ITEM_TYPE_WORD = 1;
    public UIImage imageBg;
    public UIImage imageWord;
    public UIText textTitle;
    public int itemType;
    public TableView tableView;

    Color colorSel;
    Color colorUnSel;

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
                textTitle.text = "";
                break;
        }
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

    void UpdateInfo(WordItemInfo info)
    {
        //Sprite sp = Resources.Load(info.pic, typeof(Sprite)) as Sprite;
        //imageBg.sprite = sp;
        switch (itemType)
        {
            case ITEM_TYPE_SORT:
                {
                    imageWord.gameObject.SetActive(false);
                    if (Common.BlankString(info.id))
                    {
                        textTitle.SetFontSize(48);
                        textTitle.text = DBWord.getDateDisplay(info.dbInfo.date);
               
                    }
                    else
                    {
                        textTitle.SetFontSize(64);
                        textTitle.text = info.id;
                        // string str = GameLevelParse.main.GetItemThumb(info.dbInfo.id);
                        // if (FileUtil.FileIsExistAsset(str))
                        // {
                        //     TextureUtil.UpdateImageTexture(imageBg, str, true);
                        // }
                 
                    }
                    imageBg.UpdateImageByKey("HistorySortBg");
                }

                break;
            case ITEM_TYPE_WORD:
                {
                    imageWord.gameObject.SetActive(true);
                    imageBg.UpdateImageByKey("WordBoard_H");
                    textTitle.text = "";//
                    if (FileUtil.FileIsExist(info.dbInfo.filesave))
                    {
                        Debug.Log("word image = " + info.dbInfo.filesave); 
                        imageWord.UpdateImage(info.dbInfo.filesave);
                    }


                }
                break;
        }
    }

    public override void UpdateItem(List<object> list)
    {
        WordItemInfo info = list[index] as WordItemInfo;
        UpdateInfo(info);

        LayOut();
    }
    public override bool IsLock()
    {

        return false;//imageBgLock.gameObject.activeSelf;
    }

    public override void LayOut()
    {
        base.LayOut();

    }
}
