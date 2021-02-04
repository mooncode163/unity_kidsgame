using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Moonma.Media;
public class UIWordMusic : UIWordContentBase, IUIWordBoardDelegate,IUIWordBarDelegate
{
    public UIImage imageBg;
    public UIImage imageCover;
    public UIImage imagePic;
    public UIImage imageBoard;
    public UIText textTitle;
    public UIText textType;
    public UIText textTips;
    public GameObject objAnswerBar;

    public UIWordBoard uiWordBoard;

    public UIAnswerBar uiAnswerBar;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        CaiCaiLeItemInfo info = GameLevelParse.main.GetItemInfo();
        uiAnswerBar.iDelegate = this;
        uiWordBoard.iDelegate = this;

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
        base.LayOut();

    }
    public override void UpdateGuankaLevel(int level)
    {
        UpdateItem();
    }

    public void UpdateItem()
    {
        CaiCaiLeItemInfo info = GameLevelParse.main.GetItemInfo();
        uiAnswerBar.UpdateItem(info);   
        MediaPlayer.main.Open(info.url);

    }
   public override void UpdateWord()
    {
        CaiCaiLeItemInfo info = GameLevelParse.main.GetItemInfo();
     
        //先计算行列数
        LayOut();
        uiWordBoard.InitItem();
        string strBoard = GameAnswer.main.GetWordBoardString(info, uiWordBoard.row, uiWordBoard.col);
        uiWordBoard.UpdateItem(info, strBoard); 
    }

    public override bool CheckAllFill()
    {
        bool isAllFill = true;

        return isAllFill;
    }
    public override bool CheckAllAnswerFinish()
    {
        bool ret = true;

        return ret;
    } 

    public void ClearWord()
    {
    }
    public void SetWordColor(Color color)
    {
        textTitle.color = color;
    }

    public void SetFontSize(int size)
    {
        textTitle.fontSize = size;
    }

    public override void OnTips()
    {

    }

    public override void OnAddWord(string word)
    {

    }



    public void UIWordBoardDidClick(UIWordBoard bd, UIWordItem item)
    {

        CaiCaiLeItemInfo infoGuanka = GameLevelParse.main.GetItemInfo();
        Debug.Log("UIWordBoardDidClick infoGuanka.gameType =" + infoGuanka.gameType);
         
      
            if (!uiAnswerBar.CheckAllFill())
            {
                uiAnswerBar.AddWord(item.wordDisplay);
                item.ShowContent(false);
            }
        
        

    }

    
    public void UIWordBarDidBackWord(UIWordBar ui, string word)
    {
        uiWordBoard.BackWord(word);
    }
    public void UIWordBarDidTipsWord(UIWordBar ui, string word)
    {
        uiWordBoard.HideWord(word);
    }

    public void OnClickItem()
    {

    }
}
