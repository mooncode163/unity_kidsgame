
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;



using Mono.Xml2;
using System.IO;
using System.Security;


public class Plist2Dictionary
{

public Dictionary<string, object> dicRoot;


//.plist 文件后缀需要改为.xml  xml解析不建议使用system.xml，它会有可能会去访问网络导致app启动时间变长，而且system.xml需要完整的c#库，这样会增加包的体积。
//Mono.Xml 在文件头里包含!时可能出错，如：<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
//所以想要把它删除再解析
public void LoadFile(string fileName)
{  
    string xml = Resources.Load(fileName).ToString();
    LoadXmlData(xml);
}

 public void LoadFileAsset(string file)
{  
    string xml = FileUtil.ReadStringAsset(file);
    LoadXmlData(xml);
}
public void LoadXmlData(string xml)
{
    //Dictionary<string, string> dic = null;

    dicRoot = new Dictionary<string, object>();
    SecurityParser sp = new SecurityParser();
    sp.LoadXml(xml);
    SecurityElement rootXml = sp.ToXml();
    SecurityElement xmlItemDic = GetXmlChild(rootXml, "dict");
    ParserXmlItemDic(xmlItemDic, dicRoot);

    //return dicRoot;
}


void ParserXmlItemDic(SecurityElement item, Dictionary<string, object> dicParent)
{
    string keyValue = "key";
    // Debug.Log("Plist2Dictionary:ParserXmlItemDic");
    foreach (SecurityElement child in item.Children)
    {
        if (child.Tag == "key")
        {
            keyValue = child.Text;
        }

        if (child.Tag == "string")
        {
            string str = child.Text;

            dicParent.Add(keyValue, str);
        }
        if (child.Tag == "integer")
        {
            string str = child.Text;
            dicParent.Add(keyValue, str);
        }
        if (child.Tag == "array")
        {
            List<object> l = new List<object>();
            ParserXmlItemArray(child, l);
            dicParent.Add(keyValue, l);
        }

        if (child.Tag == "dict")
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            ParserXmlItemDic(child, dic);
            //Debug.Log("Add dict:key="+keyValue);
            dicParent.Add(keyValue, dic);
        }
    }
}

void ParserXmlItemArray(SecurityElement item, List<object> list)
{
    // Debug.Log("Plist2Dictionary:ParserXmlItemArray");
    string keyValue = "key";
    foreach (SecurityElement child in item.Children)
    {
        if (child.Tag == "key")
        {
            keyValue = child.Text;
        }

        if (child.Tag == "string")
        {
            string str = child.Text;
            //Debug.Log("Add Array:key="+keyValue+" value="+str);
            list.Add(str);
        }
        if (child.Tag == "integer")
        {
            string str = child.Text;
            list.Add(str);
        }

        if (child.Tag == "array")
        {
            List<object> l = new List<object>();
            ParserXmlItemArray(child, l);
            list.Add(l);
        }

        if (child.Tag == "dict")
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();
            //Debug.Log("Add Array:key="+keyValue+" value="+str);
            ParserArrayItemDict(child, dic);
            list.Add(dic);
        }
    }
}

//array的元素是dictionary
void ParserArrayItemDict(SecurityElement item, Dictionary<string, object> dic)
{
    string keyValue = "key";
    foreach (SecurityElement child in item.Children)
    {
        if (child.Tag == "key")
        {
            keyValue = child.Text;
        }

        if (child.Tag == "string")
        {
            string str = child.Text;
            dic.Add(keyValue, str);
        }

        if (child.Tag == "integer")
        {
            string str = child.Text;
            dic.Add(keyValue, str);
        }

        if (child.Tag == "array")
        {
            List<object> l = new List<object>();
            ParserXmlItemArray(child, l);
        }
    }
}

SecurityElement GetXmlChild(SecurityElement item, string name)
{

    foreach (SecurityElement child in item.Children)
    {
       // Debug.Log("GetXmlChild:tag=" + child.Tag);
        if (child.Tag == name)
        {
           // Debug.Log("GetXmlChild:find child " + child.Tag);
            return child;
        }
    }
   // Debug.Log("GetXmlChild:not find child " + name);
    return null;

}



}

