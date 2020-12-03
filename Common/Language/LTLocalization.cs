using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class LTLocalization
{

    public const string LANGUAGE_ENGLISH = "EN";
    public const string LANGUAGE_CHINESE = "CN";
    public const string LANGUAGE_JAPANESE = "JP";
    public const string LANGUAGE_FRENCH = "FR";
    public const string LANGUAGE_GERMAN = "GE";
    public const string LANGUAGE_ITALY = "IT";
    public const string LANGUAGE_KOREA = "KR";
    public const string LANGUAGE_RUSSIA = "RU";
    public const string LANGUAGE_SPANISH = "SP";


    //public string csvFilePath;
    byte[] dataContent;
    private const string KEY_CODE = "KEY";
    private const string FILE_PATH = "Language/test_utf8";// test_utf8 LTLocalization/localization

    private SystemLanguage language = SystemLanguage.Chinese;
    private Dictionary<string, string> textData = new Dictionary<string, string>();

    //private static LTLocalization mInstance;

    // private LTLocalization()
    // {
    // }

    private static string GetLanguageAB(SystemLanguage language)
    {
        switch (language)
        {
            case SystemLanguage.Afrikaans:
            case SystemLanguage.Arabic:
            case SystemLanguage.Basque:
            case SystemLanguage.Belarusian:
            case SystemLanguage.Bulgarian:
            case SystemLanguage.Catalan:
                return LANGUAGE_ENGLISH;
            case SystemLanguage.Chinese:
            case SystemLanguage.ChineseTraditional:
            case SystemLanguage.ChineseSimplified:
                return LANGUAGE_CHINESE;
            case SystemLanguage.Czech:
            case SystemLanguage.Danish:
            case SystemLanguage.Dutch:
            case SystemLanguage.English:
            case SystemLanguage.Estonian:
            case SystemLanguage.Faroese:
            case SystemLanguage.Finnish:
                return LANGUAGE_ENGLISH;
            case SystemLanguage.French:
                return LANGUAGE_FRENCH;
            case SystemLanguage.German:
                return LANGUAGE_GERMAN;
            case SystemLanguage.Greek:
            case SystemLanguage.Hebrew:
            case SystemLanguage.Icelandic:
            case SystemLanguage.Indonesian:
                return LANGUAGE_ENGLISH;
            case SystemLanguage.Italian:
                return LANGUAGE_ITALY;
            case SystemLanguage.Japanese:
                return LANGUAGE_JAPANESE;
            case SystemLanguage.Korean:
                return LANGUAGE_KOREA;
            case SystemLanguage.Latvian:
            case SystemLanguage.Lithuanian:
            case SystemLanguage.Norwegian:
            case SystemLanguage.Polish:
            case SystemLanguage.Portuguese:
            case SystemLanguage.Romanian:
                return LANGUAGE_ENGLISH;
            case SystemLanguage.Russian:
                return LANGUAGE_RUSSIA;
            case SystemLanguage.SerboCroatian:
            case SystemLanguage.Slovak:
            case SystemLanguage.Slovenian:
                return LANGUAGE_ENGLISH;
            case SystemLanguage.Spanish:
                return LANGUAGE_SPANISH;
            case SystemLanguage.Swedish:
            case SystemLanguage.Thai:
            case SystemLanguage.Turkish:
            case SystemLanguage.Ukrainian:
            case SystemLanguage.Vietnamese:
            case SystemLanguage.Unknown:
                return LANGUAGE_ENGLISH;
        }
        return LANGUAGE_CHINESE;
    }

    public void Init(byte[] data)
    {
        dataContent = data;
    }
    private void ReadData()
    {
        textData.Clear();
        //string fileName = Application.dataPath + "/Resources/LTLocalization/localization.csv";
        //FILE_PATH 
        string csvStr = Encoding.UTF8.GetString(dataContent);
        //Debug.Log(csvStr);
        LTCSVLoader loader = new LTCSVLoader();
        // loader.ReadFile(fileName);
        loader.ReadMultiLine(csvStr);
        int languageIndex = loader.GetFirstIndexAtRow(GetLanguageAB(language), 0);
        if (-1 == languageIndex)
        {
            Debug.LogError("未读取到" + language + "任何数据，请检查配置表");
            return;
        }
        int tempRow = loader.GetRow();
        for (int i = 0; i < tempRow; ++i)
        {
            textData.Add(loader.GetValueAt(0, i), loader.GetValueAt(languageIndex, i));
        }
    }

    public void SetLanguage(SystemLanguage language)
    {
        this.language = language;
        this.ReadData();
    }
    public SystemLanguage GetLanguage()
    {
        return this.language;

    }

    public string GetText(string key)
    {

        // if (null == mInstance)
        // {
        //     Debug.Log("LTLocalization Init");
        //     Init();
        // }
        //Debug.Log("LTLocalization ContainsKey");
        if (this.textData.ContainsKey(key))
        {
            //Debug.Log("LTLocalization ContainsKey yes");
            return this.textData[key];
        }

        //Debug.Log("LTLocalization ContainsKey NO");
        return "[NoDefine]" + key;
    }

    public bool IsContainsKey(string key)
    {
        return this.textData.ContainsKey(key);
    }

}