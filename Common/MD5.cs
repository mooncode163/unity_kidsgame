using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
 
public class MD5
{

    static public string GetMD5(string msg)
    {  
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();  
        byte[] data = System.Text.Encoding.UTF8.GetBytes(msg);  
        byte[] md5Data = md5.ComputeHash(data, 0, data.Length);  
        md5.Clear();  

        string destString = "";  
        for (int i = 0; i < md5Data.Length; i++) {  
            destString += Convert.ToString(md5Data[i], 16).PadLeft(2, '0');  
        }  
        destString = destString.PadLeft(32, '0');  
        return destString;
    } 
}
   