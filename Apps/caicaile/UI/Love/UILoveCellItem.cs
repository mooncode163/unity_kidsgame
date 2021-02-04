using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEngine.UI.Button;
public delegate void OnUILoveCellItemDelegate(UILoveCellItem ui);
public class UILoveCellItem : UICellItemBase
{

    public UIImage imageBg;
    public UIButton btnDelete;
    public UIText textTitle;
    public UIText textPinyin;
    public float itemWidth;
    public float itemHeight;
    CaiCaiLeItemInfo infoItem;
    public string gameId;
    public OnUILoveCellItemDelegate callbackClickDelete { get; set; }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        LevelManager.main.ParseGuanka();
    }


    public override void UpdateItem(List<object> list)
    {
        infoItem = list[index] as CaiCaiLeItemInfo;
        UpdateInfo(infoItem.dbInfo);
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

    public void UpdateInfo(DBItemInfoBase info)
    {

        switch (gameId)
        {
            case GameRes.GAME_IDIOM:
                {

                    LevelParseIdiom.main.ParseItem(infoItem);
                    IdiomItemInfo dbinfo = info as IdiomItemInfo;
                    Debug.Log("Love UpdateInfo" + dbinfo.title);
                    textTitle.text = dbinfo.title;
                    textPinyin.text = dbinfo.pronunciation;
                    textPinyin.gameObject.SetActive(true);
                    if (Common.BlankString(dbinfo.pronunciation))
                    {
                        textPinyin.gameObject.SetActive(false);
                    }
                }

                break;
            case GameRes.GAME_RIDDLE:
                {
                    DBInfoRiddle dbinfo = info as DBInfoRiddle;
                    textTitle.text = dbinfo.head;
                    textPinyin.gameObject.SetActive(false);
                }
                break;
            case GameRes.GAME_XIEHOUYU:
                {
                    DBInfoXiehouyu dbinfo = info as DBInfoXiehouyu;
                    textTitle.text = dbinfo.head;
                    textPinyin.gameObject.SetActive(false);
                }
                break;

        }

        LayOut();

    }

    public void OnClickBtnDelete()
    {
        if (this.callbackClickDelete != null)
        {
            this.callbackClickDelete(this);
        }
    }

}
