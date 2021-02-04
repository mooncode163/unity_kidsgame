using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public interface IUIWordContentBaseDelegate
{
    //回退word
    void UIWordContentBaseDidBackWord(UIWordContentBase ui, string word);

    //提示
    void UIWordContentBaseDidTipsWord(UIWordContentBase ui, string word);
    //
    void UIWordContentBaseDidAdd(UIWordContentBase ui, string word);

    void UIWordContentBaseDidGameFinish(UIWordContentBase ui, bool isFail);
}

public class UIWordContentBase : UIView
{
    public const string STR_KEYNAME_VIEWALERT_GOLD = "keyname_viewalert_gold";
    public ItemInfo infoItem;
     public GameObject objTopBar;
    private IUIWordContentBaseDelegate _delegate;

    public IUIWordContentBaseDelegate iDelegate
    {
        get { return _delegate; }
        set { _delegate = value; }
    }
    public void ShowShop()
    {
        //  ShopViewController.main.Show(null, this);
        UIGameCaiCaiLe game = GameViewController.main.gameBase as UIGameCaiCaiLe;
        game.ShowShop();
    }
    public void UpdateGold()
    {
        //  ShopViewController.main.Show(null, this);
        UIGameCaiCaiLe game = GameViewController.main.gameBase as UIGameCaiCaiLe;
        game.UpdateGold();
    }
    public virtual void UpdateGuankaLevel(int level)
    {
    }
    public virtual void OnTips()
    {
    }

    public virtual void OnAddWord(string word)
    {
    }
    public virtual void OnReset()
    {
    }


    public virtual bool CheckAllFill()
    {
        return false;
    }
    public virtual bool CheckAllAnswerFinish()
    {
        return false;
    }
    public virtual void UpdateWord()
    {
    }
}
