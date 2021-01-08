using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Moonma.AdKit.AdConfig;

public class UIWordSearchResult : UITableViewControllerBase
{

    public RawImage imageBg;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public void Awake()
    {
        base.Awake();
        heightCell = 512;
        oneCellNum = 4;
        listItem = new List<object>();


        //bg
        TextureUtil.UpdateRawImageTexture(imageBg, AppRes.IMAGE_PLACE_BG, true);//IMAGE_GAME_BG

    }
    public void Start()
    {
        base.Start();
        UpdateTable(false);
        LayOut();
    }

    public void UpdateList(string search)
    {
        listItem.Clear();
       listItem = DBWord.main.Search(search);
        UpdateTable(true);
    }


    public override void LayOut()
    {
        base.LayOut();
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        {
            RectTransform rectTransform = imageBg.GetComponent<RectTransform>();
            float w_image = rectTransform.rect.width;
            float h_image = rectTransform.rect.height;
            float scalex = sizeCanvas.x / w_image;
            float scaley = sizeCanvas.y / h_image;
            float scale = Mathf.Max(scalex, scaley);
            imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);
            //屏幕坐标 现在在屏幕中央
            imageBg.transform.position = new Vector2(Screen.width / 2, Screen.height / 2);
        }


    }

    void GotoGame()
    {
        GameManager.main.GotoGame(this.controller);
    }

    public override void OnCellItemDidClick(UICellItemBase item)
    {
        if (item.IsLock())
        {
            return;
        }
        GotoGame();

    }



    public void OnClickBtnBack()
    {
        NaviViewController navi = this.controller.naviController;
        if (navi != null)
        {
            navi.Pop();
        }

    }

}

