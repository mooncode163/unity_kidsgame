
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Moonma.Share;
using UnityEngine;
using UnityEngine.UI;


public interface IUILetterItemDelegate
{
    //回退word
    void OnUILetterItemDidClick(UILetterItem ui);

}
public class UILetterItem : UIView
{
    public enum Type
    {
        Connect = 0,
    }

    public enum Status
    {
        NORMAL = 0,
        LOCK,
        UNLOCK,
        DUPLICATE,//重复连线
        HIDE,
        LOCK_SEL,
        LOCK_UNSEL,
        ALL_RIGHT_ANSWER,//所有item
        RIGHT_ANSWER,//当前一个item
        ERROR_ANSWER,
    }
    public Image imageBg;
    public Image imageIcon;
    public Image imageSel;
    public Text textTitle;
    public int indexRow;
    public int indexCol;
    public int index;
    public bool isAnswerItem;
    public string wordDisplay;
    public string wordAnswer;
    Status status;

    private IUILetterItemDelegate _delegate;

    public IUILetterItemDelegate iDelegate
    {
        get { return _delegate; }
        set { _delegate = value; }
    }
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        SetStatus(Status.LOCK);
        LayOut();
    }


    public override void LayOut()
    {

    }
    public void SetStatus(Status st)
    {
        status = st;
        imageSel.gameObject.SetActive(false);
        if (st == Status.LOCK)
        {
            imageBg.gameObject.SetActive(true);
            TextureUtil.UpdateImageTexture(imageBg, GameRes.IMAGE_LetterBgNormal, true);
            textTitle.gameObject.SetActive(false);
            imageIcon.gameObject.SetActive(true);

        }

        if (st == Status.LOCK_SEL)
        {
            imageBg.gameObject.SetActive(true);
            TextureUtil.UpdateImageTexture(imageBg, GameRes.IMAGE_LetterBgLock, true);
            textTitle.gameObject.SetActive(false);
            imageIcon.gameObject.SetActive(false);
            imageSel.gameObject.SetActive(true);


        }
        if (st == Status.LOCK_UNSEL)
        {
            imageBg.gameObject.SetActive(true);
            TextureUtil.UpdateImageTexture(imageBg, GameRes.IMAGE_LetterBgLock, true);
            textTitle.gameObject.SetActive(false);
            imageIcon.gameObject.SetActive(false);

        }
        if (st == Status.DUPLICATE)
        {
            imageBg.gameObject.SetActive(true);
            TextureUtil.UpdateImageTexture(imageBg, GameRes.IMAGE_LetterBgNormal, true);
            textTitle.gameObject.SetActive(true);
            imageIcon.gameObject.SetActive(false);

            // 10 秒内， 物体 X,Y,Z 旋转角度在 自身-5 到 自身加 5 之间震动 
            this.transform.DOShakeRotation(1f, new Vector3(0, 0, 30)).OnComplete(() =>
               {
                   SetStatus(Status.UNLOCK);
               });
        }


        if (st == Status.NORMAL)
        {
            textTitle.color = Color.black;
            imageBg.gameObject.SetActive(true);
            TextureUtil.UpdateImageTexture(imageBg, GameRes.IMAGE_LetterBgNormal, true);
            textTitle.gameObject.SetActive(true);
            imageIcon.gameObject.SetActive(false);
        }

        if (st == Status.UNLOCK)
        {
            textTitle.color = Color.black;
            imageBg.gameObject.SetActive(true);
            TextureUtil.UpdateImageTexture(imageBg, GameRes.IMAGE_LetterBgNormal, true);
            textTitle.gameObject.SetActive(true);
            imageIcon.gameObject.SetActive(false);
        }
        if (st == Status.HIDE)
        {
            imageBg.gameObject.SetActive(false);
            textTitle.gameObject.SetActive(false);
            imageIcon.gameObject.SetActive(false);
        }

        if (st == Status.ALL_RIGHT_ANSWER)
        {
            textTitle.color = Color.white;
            imageBg.gameObject.SetActive(true);
            TextureUtil.UpdateImageTexture(imageBg, GameRes.IMAGE_LetterBgRightAnswer, true);
            textTitle.gameObject.SetActive(true);
            imageIcon.gameObject.SetActive(false);
        }
        if (st == Status.ERROR_ANSWER)
        {
            textTitle.color = Color.red;
            imageBg.gameObject.SetActive(true);
            TextureUtil.UpdateImageTexture(imageBg, GameRes.IMAGE_LetterBgAddWord, true);
            textTitle.gameObject.SetActive(true);
            imageIcon.gameObject.SetActive(false);
        }

        if (st == Status.RIGHT_ANSWER)
        {
            textTitle.color = Color.black;
            imageBg.gameObject.SetActive(true);
            TextureUtil.UpdateImageTexture(imageBg, GameRes.IMAGE_LetterBgAddWord, true);
            textTitle.gameObject.SetActive(true);
            imageIcon.gameObject.SetActive(false);
        }



    }

    public Status GetStatus()
    {
        return status;
    }

    public void UpdateItem(string letter)
    {
        wordDisplay = letter;
        textTitle.text = letter;
    }

    public void OnClickItem()
    {
        Debug.Log("OnUILetterItemDidClick OnClickItem");
        if (iDelegate != null)
        {
            iDelegate.OnUILetterItemDidClick(this);
        }
    }
}
