
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppRes
{
    public const int GOLD_SHARE = 5;
    public const int GOLD_GUANKA = 3;
    public const int GOLD_COMMENT = 3;
    public const int GOLD_INIT_VALUE = 10;
    public const int GOLD_GUANKA_STEP = 4;
    //color
    //f88816 248,136,22
    static public Color colorTitle = new Color(248 / 255f, 136 / 255f, 22 / 255f);

    //audio 
    public const string AUDIO_BG = "App/Audio/Bg";
    public const string AUDIO_BTN_CLICK = "AppCommon/Audio/BtnClick";
    public const string AUDIO_PINTU_BLOCK_FINISH = "Audio/PintuBlockFinish";
    public const string AUDIO_WORD_OK = "AppCommon/Audio/word_ok";
    public const string AUDIO_WORD_FAIL = "AppCommon/Audio/word-failed";
    public const string AUDIO_LETTER_DRAG_1 = "AppCommon/Audio/letter-drag-1";
    public const string AUDIO_LETTER_DRAG_2 = "AppCommon/Audio/letter-drag-2";
    public const string AUDIO_LETTER_DRAG_3 = "AppCommon/Audio/letter-drag-3";
    public const string AUDIO_LETTER_DRAG_4 = "AppCommon/Audio/letter-drag-4";
    public const string AUDIO_SELECT = "AppCommon/Audio/select";
    public const string AUDIO_SUCCESS_1 = "AppCommon/Audio/success-1";
    public const string AUDIO_SUCCESS_2 = "AppCommon/Audio/success-2";


    public const string IMAGE_BtnSoundOn = "AppCommon/UI/Home/BtnSoundOn";
    public const string IMAGE_BtnSoundOff = "AppCommon/UI/Home/BtnSoundOff";
    //
    public const string Audio_PopupOpen = "AppCommon/Audio/PopUp/PopupOpen";
    public const string Audio_PopupClose = "AppCommon/Audio/PopUp/PopupClose";
    //prefab  

    public const string PREFAB_GUANKA_CELL_ITEM = "AppCommon/Prefab/Guanka/UIGuankaCellItem";
    public const string PREFAB_CmdItem = "AppCommon/Prefab/CmdItem/UICmdItem";
    public const string PREFAB_SETTING = "AppCommon/Prefab/Setting/UISettingController";
    public const string PREFAB_MOREAPP_CELL_ITEM = "AppCommon/Prefab/MoreApp/UIMoreAppCellItem";

    //image
    public const string IMAGE_BtnMusicBgOn = "App/UI/Common/Button/BtnBg";
    public const string IMAGE_BtnMusicBgOff = "App/UI/Common/Button/BtnBgGrey";

    public const string IMAGE_BtnMusicIconOn = "App/UI/Common/Button/BtnIconMusic";
    public const string IMAGE_BtnMusicIconOff = "App/UI/Common/Button/BtnIconMusic";


    public const string IMAGE_BtnSoundBgOn = IMAGE_BtnMusicBgOn;
    public const string IMAGE_BtnSoundBgOff = IMAGE_BtnMusicBgOff;

    public const string IMAGE_BtnSoundIconOn = "App/UI/Common/Button/BtnIconSound";
    public const string IMAGE_BtnSoundIconOff = "App/UI/Common/Button/BtnIconSound";


    public const string IMAGE_UIVIEWALERT_BG_BOARD = "AppCommon/UI/Setting/SettingCellBgBlue";
    static public Vector4 borderUIViewAlertBgBoard = new Vector4(18f, 18f, 18f, 18f);
    public const string IMAGE_UIVIEWALERT_BG_BTN = "AppCommon/UI/Setting/SettingCellBgOringe";
    static public Vector4 borderUIViewAlertBgBtn = new Vector4(18f, 18f, 18f, 18);


    public const string IMAGE_HOME_BG = Common.GAME_DATA_DIR + "/startup.jpg";


    public const string IMAGE_GUANKA_ITEM_DOT0 = "AppCommon/UI/Guanka/dot0";
    public const string IMAGE_GUANKA_ITEM_DOT1 = "AppCommon/UI/Guanka/dot1";
    public const string IMAGE_GUANKA_CELL_BG = "AppCommon/UI/Guanka/guanka_cell_bg";
    public const string IMAGE_GUANKA_CELL_BG_LOCK = "AppCommon/UI/Guanka/guanka_cell_bg_lock";


    public const string IMAGE_COMMON_BG = "App/UI/Guanka/GuankaBg";
    public const string IMAGE_CELL_BG_BLUE = "AppCommon/UI/Setting/SettingCellBgBlue";
    public const string IMAGE_CELL_BG_ORINGE = "AppCommon/UI/Setting/SettingCellBgOringe";
    public const string IMAGE_CELL_BG_YELLOW = "AppCommon/UI/Setting/SettingCellBgYellow";
    static public Vector4 borderCellSettingBg = new Vector4(18f, 18f, 18f, 18f);


    // cmditem
    public const string IMAGE_CMDITEM_BG = "AppCommon/UI/Game/CmdItem/CmdItemBg";


    //bg

    public const string IMAGE_MOREAPP_BG = "App/UI/Bg/SettingBg";
    public const string IMAGE_GAME_BG = "App/UI/Bg/GameBg";
    public const string IMAGE_PLACE_BG = "App/UI/Bg/PlaceBg";
    public const string IMAGE_GUANKA_BG = "App/UI/Bg/GuankaBg";
    public const string IMAGE_LEARN_BG = "App/UI/Bg/LearnBg";
    public const string IMAGE_SETTING_BG = "App/UI/Bg/SettingBg";

    public const string IMAGE_GUANKA_CELL_ITEM_BG_UNLOCK = "App/UI/Guanka/guanka_item_unlock";
    public const string IMAGE_GUANKA_CELL_ITEM_BG_LOCK = "App/UI/Guanka/guanka_item_lock";
    public const string IMAGE_GUANKA_CELL_ITEM_BG_PLAY = "App/UI/Guanka/guanka_item_playing";

}
