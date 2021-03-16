
using UnityEngine;
using System.Collections;
using Tacticsoft;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;
using LitJson;
public class NetImageParseBase
{
    public INetImageParseDelegate iDelegate;

    //w:h
    public virtual float GetImageRatio()
    {
        return 16f / 9;
    }
    public virtual void StartParseSortList()
    {

    }

    public virtual void StartParseImageList(ImageItemInfo info)
    {

    }

}

