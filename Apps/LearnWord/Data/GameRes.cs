
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRes
{
    public const string GAME_ID_Xiehanzi = "xiehanzi"; 
    public const string GAME_ID_LearnWord = "LearnWord"; 
    //type 
    public const string GAME_TYPE_IMAGE = "Image"; 
 

    //color
    //f88816 248,136,22
    static private GameRes _main = null;
    public static GameRes main
    {
        get
        {
            if (_main == null)
            {
                _main = new GameRes();
            }
            return _main;
        }
    } 
 

}
