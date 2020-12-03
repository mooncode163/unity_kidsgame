using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;
using UnityEngine;
public class Util
{

    static private Util _main = null;
    public static Util main
    {
        get
        {
            if (_main == null)
            {
                _main = new Util();
            }
            return _main;
        }
    }


    //从数组里随机抽取newsize个元素
    public int[] RandomIndex(int size, int newsize)
    {
        List<object> listIndex = new List<object>();
        int total = size;
        for (int i = 0; i < total; i++)
        {
            listIndex.Add(i);
        }

        int[] idxTmp = new int[newsize];
        for (int i = 0; i < newsize; i++)
        {
            total = listIndex.Count;
            int rdm = Random.Range(0, total);
            int idx = (int)listIndex[rdm];
            idxTmp[i] = idx;
            listIndex.RemoveAt(rdm);
        }

        return idxTmp;
    }


}
