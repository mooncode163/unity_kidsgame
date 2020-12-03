using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;
using UnityEngine;

public class SpriteUtil : MonoBehaviour
{
    static public GameObject CreateSpriteObj(string name)
    {
        return CreateSpriteObj(name, null, null);
    }
    static public GameObject CreateSpriteObj(string name, GameObject objParent, Sprite sp)
    {
        GameObject obj = new GameObject(name);
        SpriteRenderer sprender = obj.AddComponent<SpriteRenderer>();
        if (sp != null)
        {
            sprender.sprite = sp;
        }
        if (objParent != null)
        {
            obj.transform.parent = objParent.transform;
        }
        return obj;
    }

}
