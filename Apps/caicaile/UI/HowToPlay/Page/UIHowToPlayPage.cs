using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIHowToPlayPage : UIHowToPlayPageBase
{
    public Text textTitle;
    public Text textDetail;
    public GameObject objTop;
    public GameObject objBottom;
    public Image imageBgTop;
    public Image imageBgBottom;
    public Image imageHand;
    protected UIWordFillBox uiWordFillBoxPrefab;
    public UIWordFillBox uiWordFillBox;

    public UIWordBoard uiWordBoard;

    public int gameLevel;
    public CaiCaiLeItemInfo infoItem;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
    }
    void Start()
    {

    }
    public void LoadPrefab()
    {
        {
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/Game/UIWordFillBox");
            if (obj != null)
            {
                uiWordFillBoxPrefab = obj.GetComponent<UIWordFillBox>();
            }
        }

    }
    public override void LayOut()
    {
        base.LayOut();
    }


}
