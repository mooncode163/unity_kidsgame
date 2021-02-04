using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIButtonIdiom : UIView
{
    public Image imageBg;
    public Text textTitle;
    public int index;
    CaiCaiLeItemInfo infoItem;
    private IUIWordItemDelegate _delegate;
    public IUIWordItemDelegate iDelegate
    {
        get { return _delegate; }
        set { _delegate = value; }
    }

    // Use this for initialization
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {


    }
    public void UpdateItem(CaiCaiLeItemInfo info)
    {
        infoItem = info;
        textTitle.text = info.title;
    } 
    public void OnClickItem()
    {

        if (_delegate != null)
        {
            //  _delegate.WordItemDidClick(this);
        }


        PopUpManager.main.Show<UIIdiomDetail>("App/Prefab/Game/UIIdiomDetail", popup =>
        {
            Debug.Log("UIIdiomDetail Open ");
            popup.UpdateItem(infoItem);

        }, popup =>
        {
            Debug.Log("UIIdiomDetail Close ");

        });
    }
}
