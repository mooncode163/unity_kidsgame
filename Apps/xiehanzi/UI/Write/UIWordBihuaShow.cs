using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class UIWordBihuaShow : UIViewPop
{
    public Button btnClose; 
    public UIImage imageBg;
    public UIImage imageWord;
  

    public void Updateitem(WordItemInfo info)
    {
        imageWord.UpdateImage(info.imageBihua);
        LayOut();

    }
    public void OnClickBtnClose()
    {
        this.Close();

    } 
}
