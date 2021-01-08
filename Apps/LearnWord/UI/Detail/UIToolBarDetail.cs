using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIToolBarDetail : UIView
{
    public Button btnSound;
    public Button btnDemo;
    public Button btnWrite;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {

    }
    // Use this for initialization
    void Start()
    {

    } 

    public void OnClickBtnSound()
    {
        WordItemInfo info = GameLevelParse.main.GetItemInfo();
        AudioPlay.main.PlayUrl(info.dbInfo.audio);

    }

    public void OnClickBtnDemo()
    {// UIWordGif
        PopUpManager.main.Show<UIViewPop>("App/Prefab/Detail/UIWordDemo", popup =>
     {
         Debug.Log("UIViewAlert Open ");
         UIWordDemo ui = popup as UIWordDemo;
         WordItemInfo info = GameLevelParse.main.GetItemInfo();
         ui.Updateitem(info);
     }, popup =>
     {


     });

    }
    public void OnClickBtnWrite()
    {
        PopUpManager.main.Show<UIViewPop>("App/Prefab/Game/UIWordPopWrite", popup =>
     {
         Debug.Log("UIViewAlert Open ");
         UIWordPopWrite ui = popup as UIWordPopWrite;
         WordItemInfo info = GameLevelParse.main.GetItemInfo();
         ui.UpdateItem(info);
     }, popup =>
     {


     });

    }
}
