using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
成语大全
http://chengyu.t086.com/
 */
public class GameCaiCaiLe : GameBase
{

    public enum Type
    {
        IMAGE = 0,
        TEXT = 1,
        IMAGE_TEXT = 1
    }

    public Type _scaleType;
    public Type scaleType
    {
        get
        {
            return _scaleType;
        }

        set
        {
            _scaleType = value;
        }

    }

    void Awake()
    {

    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void LayOut()
    {
        float x = 0, y = 0, w = 0, h = 0;

    }


}
