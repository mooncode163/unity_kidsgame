using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LitJson;
using System.Text;

public class LanguageManager
{

    public Language languageGame;
    public Language languagePlace;
    static private LanguageManager _main = null;
    public static LanguageManager main
    {
        get
        {
            if (_main == null)
            {
                _main = new LanguageManager();
            }
            return _main;
        }
    }


    public void UpdateLanguage(int indexPlace)
    {
        ItemInfo infoPlace = LevelManager.main.GetPlaceItemInfo(indexPlace);
        string filepath = CloudRes.main.rootPathGameRes +"/language/" + infoPlace.language + ".csv";
        Debug.Log("UpdateLanguage filepath=" + filepath);
        byte[] data = FileUtil.ReadDataAuto(filepath);
        if (languageGame == null)
        {
            languageGame = new Language();
        }
        languageGame.Init(data);
        languageGame.SetLanguage(Language.main.GetLanguage());
    }
    public void UpdateLanguagePlace()
    {
        if (languagePlace != null)
        {
            languagePlace.SetLanguage(Language.main.GetLanguage());
            return;
        }
        string strlan = CloudRes.main.rootPathGameRes +"/place/language/language.csv";
        if (FileUtil.FileIsExistAsset(strlan))
        {
            languagePlace = new Language();
            languagePlace.Init(strlan);
            languagePlace.SetLanguage(Language.main.GetLanguage());
        }
        else
        {
            languagePlace = Language.main;
        }
    }
    public string LanguageKeyOfPlaceItem(ItemInfo info)
    {
        return "STR_PLACE_" + info.id;
    }
    public string LanguageOfPlaceItem(ItemInfo info)
    {
        return languagePlace.GetString(LanguageKeyOfPlaceItem(info));
    }

    public string LanguageOfGameItem(ItemInfo info)
    {
        string key = info.id;
        if (Common.BlankString(key))
        {
            key = "KEY_ID";
        }
        return languageGame.GetString(key);
    }

    public string StringOfGameStatusItem(ItemInfo info)
    {
        int status = PlayerPrefs.GetInt(GameBase.KEY_GAME_STATUS_ITEM + info.id);
        string str = "";
        switch (status)
        {
            case GameBase.GAME_STATUS_UN_START:
                str = Language.main.GetString("STR_GAME_STATUS_UN_START");
                break;
            case GameBase.GAME_STATUS_PLAY:
                str = Language.main.GetString("STR_GAME_STATUS_PLAY");
                break;
            case GameBase.GAME_STATUS_FINISH:
                str = Language.main.GetString("STR_GAME_STATUS_FINISH");
                break;
        }

        return str;
    }
}
