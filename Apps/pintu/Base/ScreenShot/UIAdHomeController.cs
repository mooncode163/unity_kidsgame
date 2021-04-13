using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIAdHomeController : UIShotBase
{
    public UIImage imageBg;
    public UIText textTitle;

    List<object> listItem;
    GamePintu gamePintu;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        AppSceneBase.main.SetCanvasRenderMode(RenderMode.ScreenSpaceCamera);
        LevelManager.main.ParseGuanka();
        string appname = Common.GetAppNameDisplay();
        textTitle.text = appname;
        string pic_name = Common.GAME_DATA_DIR + "/screenshot/AdBg";
        string pic = pic_name + ".jpg";
        if (!FileUtil.FileIsExistAsset(pic))
        {
            pic = pic_name + ".png";
        }
        imageBg.UpdateImage(pic);
        // imageBg.SetActive(false);
        AppSceneBase.main.objSpriteBg.SetActive(false);
        UpdateGuankaLevel(1);
        LayOut();
    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        LayOut();
        OnUIDidFinish(2.0f);
    }

    void OnDestroy()
    {
        AppSceneBase.main.objSpriteBg.SetActive(true);
        AppSceneBase.main.SetCanvasRenderMode(RenderMode.ScreenSpaceOverlay);
    }
    public override void LayOut()
    {
        base.LayOut();
        if(gamePintu!=null)
        {
            gamePintu.LayOut();
        }
    }

    public void UpdateGuankaLevel(int level)
    {

        ItemInfo info = GameLevelParse.main.GetGuankaItemInfo(level);
        AppSceneBase.main.ClearMainWorld();
        GameObject objgame = new GameObject("GamePintu");
        gamePintu = objgame.AddComponent<GamePintu>();
        // gamePintu.iDelegate = this;
        gamePintu.row = 3;
        gamePintu.col = 3;
        gamePintu.infoGuankaItem = info;
        gamePintu.CreateBlock();
        AppSceneBase.main.AddObjToMainWorld(objgame);
        gamePintu.transform.localPosition = new Vector3(0, 0, -1f);
        Texture2D texGamePic = LoadTexture.LoadFromAsset(info.pic);
        gamePintu.UpdateTexture(texGamePic);
    }




}
