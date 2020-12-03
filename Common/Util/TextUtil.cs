using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;
using UnityEngine;

public class TextUtil : MonoBehaviour
{
    static public string GetUnicode(string text)
    {
        string result = "";
        for (int i = 0; i < text.Length; i++)
        {
            if ((int)text[i] > 32 && (int)text[i] < 127)
            {
                result += text[i].ToString();
            }
            else
                //result += string.Format("\\u{0:x}", (int)text[i]);
                result += string.Format("\\u{0:x4}", (int)text[i]);
        }
        return result;
    }

}
