using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LitJson;
using System.Text;

public class GameManager
{


    public UIViewController fromUIViewController;//来源

    public float heightAdWorld;
    public float heightAdScreen;
    public float heightAdCanvas;

    public bool isLoadGameScreenShot = false;

    public bool isShowGameAdInsert;

    public string pathGamePrefab;

    static private GameManager _main = null;
    public static GameManager main
    {
        get
        {
            if (_main == null)
            {
                _main = new GameManager();
            }
            return _main;
        }
    }
    public int gameMode
    {
        get
        {
            int ret = 0;
            if (GameViewController.main.gameBase != null)
            {

                ret = UIGameBase.gameMode;
            }

            return ret;
        }

        set
        {
            if (GameViewController.main.gameBase != null)
            {
                Debug.Log("GameScene.gameBase.gameMode = " + value);
                UIGameBase.gameMode = value;

            }

        }

    }


    public void GotoGame(UIViewController fromController)
    {
        fromUIViewController = fromController;
        //GameViewController.main.ShowOnController(AppSceneBase.main.rootViewController);
        NaviViewController navi = fromController.naviController;
        if (navi != null)
        {
            navi.Push(GameViewController.main);

        }

    }

    public void GotoPlayAgain()
    {
        GameViewController.main.gameBase.UpdateGuankaLevel(LevelManager.main.gameLevel);
    }


    //webgl 异步加载需要提前加载一些配置数据
    public void PreLoadDataForWeb()
    {
        //place list
        // ParsePlaceList();

        // //place
        // PlaceViewController.main.PreLoadDataForWeb();

        // //guanka 
        // GuankaViewController.main.PreLoadDataForWeb();

        // //game
        // UIGameBase game = GameViewController.main.gameBase;
        // if (game != null)
        // {
        //     game.PreLoadDataForWeb();
        // }

    }

 
    public void ShowPrivacy()
    {
         if ( GameManager.main.isLoadGameScreenShot)
        {
            return;
        }
        if (Common.GetBool(UIPrivacy.KEY_DISABLE_UIPRIVACY))
        {
            return;
        }
        string strPrefab = ConfigPrefab.main.GetPrefab("UIPrivacy");
        Debug.Log("ShowPrivacy strPrefab=" + strPrefab);
        // strPrefab = "Common/Prefab/Setting/UILanguage";
        // strPrefab = "Common/Prefab/Privacy/UIPrivacy";
        //
        // Common/Prefab/Privacy/UIPrivacy.prefab


        PopUpManager.main.Show<UIViewPop>(strPrefab, popup =>
     {
         Debug.Log("UIViewAlert Open ");

     }, popup =>
     {
         // OnUILanguageDidClose();

     });
    }
}
