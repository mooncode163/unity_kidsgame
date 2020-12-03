
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;


public class Language
{


    private LTLocalization ltLocalization;
    static private Language _common = null;
    static private Language _appCommon = null; 

    static private Language _main = null;
    public static Language main//GameData
    {
        get
        {
            if (_main == null)
            {


                _common = new Language();
                string fileName = Common.RES_CONFIG_DATA_COMMON + "/language/language.csv";
                _common.Init(fileName);
                //_common.SetLanguage(SystemLanguage.Chinese);

                //appcommon
                fileName = "AppCommon/language/language";//.csv//resource
                if (FileUtil.FileIsExist(fileName))
                {
                    _appCommon = new Language();
                    _appCommon.Init(fileName);
                }
 
                _main = new Language();
                // _main.Init("Language/Language");
                //fileName = Common.GAME_DATA_DIR + "/language/language.csv";
                fileName = Common.RES_CONFIG_DATA + "/language/language.csv";
                _main.Init(fileName);
                _main.SetLanguage(SystemLanguage.Chinese);


            }
            return _main;
        }
    }


    static private Language _game = null;
    public static Language game//GameRes
    {
        get
        {
            if (_game == null)
            {
                _game = new Language();
                string filepath = CloudRes.main.rootPathGameRes +"/language/language.csv";
                _game.Init(filepath);
                _game.SetLanguage(main.GetLanguage());
            }
            return _game;
        }
    }
    // void setLanguageType(LanguageType languageType);
    public void Init(string file)
    {
        // if(instance!=null){
        //     return;
        // }
        // instance = new Language();


        //csv需要在pc上先转换成utf8格式

        byte[] data = FileUtil.ReadDataAuto(file);
        Init(data);
    }

    public void Init(byte[] data)
    {
        ltLocalization = new LTLocalization();
        ltLocalization.Init(data);
    }
    public void SetLanguage(SystemLanguage lan, bool isUpdateUI = false)
    {
        // Init();
        ltLocalization.SetLanguage(lan);
        if (_common != null)
        {
            _common.ltLocalization.SetLanguage(lan);
        }
        if (_game != null)
        {
            _game.ltLocalization.SetLanguage(lan);
        }
        if (_appCommon != null)
        {
            _appCommon.ltLocalization.SetLanguage(lan);
        }
 

        //更新ui
        if (isUpdateUI)
        {
            AppSceneBase.main.UpdateLanguage();
        }
    }

    public bool IsChinese()
    {
        SystemLanguage lan = GetLanguage();
        if ((lan == SystemLanguage.Chinese) || (lan == SystemLanguage.ChineseSimplified) || (lan == SystemLanguage.ChineseTraditional))
        {
            return true;
        }
        return false;
    }

    public SystemLanguage GetLanguage()
    {
        return ltLocalization.GetLanguage();

    }
    public string GetString(string key)
    {
        // Init();
        string str = "0";
        if (IsContainsKey(key))
        {
            str = ltLocalization.GetText(key);
        }
        else if ((_appCommon != null) && _appCommon.IsContainsKey(key))
        {
            str = _appCommon.ltLocalization.GetText(key);
        }
        else
        {
            str = _common.ltLocalization.GetText(key);
        }

        return str;

    }


    //
    public string GetReplaceString(string key, string replace, string strnew)
    {
        string str = GetString(key);
        str = str.Replace(replace, strnew);
        return str;
    }

    public bool IsContainsKey(string key)
    {
        return ltLocalization.IsContainsKey(key);
    }


}
