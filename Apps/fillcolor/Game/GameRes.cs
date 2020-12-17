using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class GameRes
{
    public const string GAME_WORDCONNECT = "WordCollect";
    public const string GAME_IDIOM = "Idiom";


    //type
    public const string GAME_TYPE_WORDLIST = "WordList";
    public const string GAME_TYPE_IMAGE = "Image";
    public const string GAME_TYPE_TEXT = "Text";
    public const string GAME_TYPE_IMAGE_TEXT = "ImageText";

    //audio

    public const string PREFAB_LETTER_ITEM = "AppCommon/Prefab/Game/UILetterItem";

    public const string Audio_LetterItemSel = "AppCommon/Audio/Game/LetterItemSel";
    public const string Audio_WordDuplicate = "AppCommon/Audio/Game/WordDuplicate";
    public const string Audio_WordError = "AppCommon/Audio/Game/WordError";
    public const string Audio_WordRight = "AppCommon/Audio/Game/WordRight";


    //image
    public const string Image_GameDotBg = "App/UI/Game/Draw/DotBg.png";
    public const string Image_GameDot = "App/UI/Game/Draw/Dot.png";
    //color
    //f88816 248,136,22
    public const string KEY_COLOR_TITLE = "title";
    public const string KEY_COLOR_PlaceItemTitle = "PlaceItemTitle";
    public const string KEY_COLOR_BoardTitle = "BoardTitle";
    public const string KEY_COLOR_GameText = "GameText";
    public const string KEY_COLOR_GameWinTitle = "GameWinTitle";
    public const string KEY_COLOR_GameWinTextView = "GameWinTextView";
    public const string KEY_COLOR_LevelTitle = "LevelTitle";
    public const string KEY_COLOR_HomeName = "HomeName";


}
