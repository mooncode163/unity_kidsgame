using System.Collections;
using System.Collections.Generic;
using Moonma.Share;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public interface IGameDelegate
{
    void OnGameDidWin(GameBase g);
    void OnGameDidFail(GameBase g);
    void OnGameUpdateTitle(GameBase g, ItemInfo info, bool isshow);

}

public class GameBase : UIView
{
    public const string KEY_GAME_STATUS_ITEM = "KEY_GAME_STATUS_ITEM_";
    public const int GAME_STATUS_UN_START = 0;//没有开始
    public const int GAME_STATUS_PLAY = 1;//进行中
    public const int GAME_STATUS_FINISH = 2;//完成

    public List<object> listItem;
    public ItemInfo infoGuankaItem;
    private IGameDelegate _delegate;

    public IGameDelegate iDelegate
    {
        get { return _delegate; }
        set { _delegate = value; }
    }


    public void OnGameWin()
    {
        if (iDelegate != null)
        {
            iDelegate.OnGameDidWin(this);
        }
    }
    public void OnGameFail()
    {
         Debug.Log("OnGameFail");
        if (iDelegate != null)
        {
            iDelegate.OnGameDidFail(this);
        }
    }

    public int GetGameItemStatus(ItemInfo info)
    {
        string key = GameBase.KEY_GAME_STATUS_ITEM + info.id;
        return PlayerPrefs.GetInt(key, GAME_STATUS_UN_START);
    }
    public void SetGameItemStatus(ItemInfo info, int status)
    {
        string key = GameBase.KEY_GAME_STATUS_ITEM + info.id;
        PlayerPrefs.SetInt(key, status);
    }

    public virtual void UpdateGuankaLevel(int level)
    {
    }
}
