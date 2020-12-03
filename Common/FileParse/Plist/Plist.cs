
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;


    /* 
using Mono.Xml2;
using System.IO;
using System.Security;

//using DZetko.Xml;

public class Plist
{

    private string keyValue;
    private string stringValue;
    private Dictionary<string, string> dicRoot;


    SecurityElement GetXmlChild(SecurityElement item, string name)
    {

        foreach (SecurityElement child in item.Children)
        {
            if (child.Tag == name)
            {
                return child;
            }
        }
        return null;

    }


    //.plist 文件后缀需要改为.xml  xml解析不建议使用system.xml，它会有可能会去访问网络导致app启动时间变长，而且system.xml需要完整的c#库，这样会增加包的体积。
    //Mono.Xml 在文件头里包含!时可能出错，如：<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
    //所以想要把它删除再解析
    public void LoadPlist(string file)
    {
        // 

        Debug.Log("Plist LoadPlist 1");
        dicRoot = new Dictionary<string, string>();
        //         Debug.Log("Plist LoadPlist 2");
        //        string str = Resources.Load(file).ToString();
        //        Debug.Log(str);
        //         XmlParser parser = new XmlParser(XmlParser.InputType.Text,str );

        //         Debug.Log("Plist LoadPlist 3");

        //         XmlDocument doc = parser.Parse();
        //         Debug.Log("Plist LoadPlist 4");
        //         XmlElement root = doc.RootNode;
        //         Debug.Log("Plist LoadPlist 5");
        //         //plist/dict"
        //         //return;
        //         XmlElement dict = root.Children["plist"].Children["dict"];
        // Debug.Log("Plist LoadPlist 6");
        //         foreach (XmlElement element in dict.Children)
        //         {

        //             if (element.Name == "key")
        //             {
        //                 keyValue = element.Content;
        //             }
        //             if (element.Name == "string")
        //             {
        //                 stringValue = element.Content;
        //                 //Debug.Log(keyValue +":"+stringValue);
        //                 dicRoot.Add(keyValue, stringValue);
        //             }

        //         }
        // Debug.Log("Plist LoadPlist 7");



        //          XMLParser xmlParser = new XMLParser();     
        //             XMLNode xn = xmlParser.Parse(Resources.Load(file).ToString());     
        //            // server = xn.GetValue("items>0>server>0>_text");     
        //            // database = xn.GetValue("items>0>database>0>_text");     
        //             XMLNode temp=xn.GetNode("plist/dict");     
        //           //  string basePath=temp.GetValue("@basePath");//或直
        // foreach (XMLNode child in temp.ChildNodes)  
        //    {

        //    } 

        // 假设xml文件路径为 Resources/test.xml  
        //string xmlPath = "test";  
        SecurityParser sp = new SecurityParser();
        sp.LoadXml(Resources.Load(file).ToString());

        SecurityElement root = sp.ToXml();
        SecurityElement plist = GetXmlChild(root, "plist");
        if (plist != null)
        {
            SecurityElement dict = GetXmlChild(plist, "dict");
            if (dict != null)
            {
                foreach (SecurityElement child in dict.Children)
                {


                    if (child.Tag == "key")
                    {
                        keyValue = child.Text;
                    }
                    if (child.Tag == "string")
                    {
                        stringValue = child.Text;
                        //Debug.Log(keyValue +":"+stringValue);
                        dicRoot.Add(keyValue, stringValue);
                    }

                }
            }

        }




    }


    //     public void LoadPlist(string file)
    //     {
    //         //"Config/config"
    //         Debug.Log("Plist LoadPlist start");
    //         XmlDocument xmlDoc = new XmlDocument();
    //         TextAsset textAsset = (TextAsset)Resources.Load(file);
    //         if(!textAsset){
    //              Debug.Log("Plist Resources.Load fail"); 

    //         }
    //         Debug.Log("Plist LoadPlist 1");
    //         Debug.Log("Plist xmlDoc.LoadXml start");
    //         xmlDoc.LoadXml(textAsset.text);
    //         Debug.Log("Plist xmlDoc.LoadXml end");
    //         Debug.Log("Plist LoadPlist 2");
    //         XmlNodeList nodeList = xmlDoc.SelectNodes("plist/dict");
    //         XmlNode node = nodeList[0];
    //         dicRoot = new Dictionary<string, string>();
    // Debug.Log("Plist LoadPlist 3");
    //         foreach (XmlElement xe in node)
    //         {


    //             if (xe.Name == "key")
    //             {
    //                 keyValue = xe.InnerText;
    //             }
    //             if (xe.Name == "string")
    //             {
    //                 stringValue = xe.InnerText;
    //                 //Debug.Log(keyValue +":"+stringValue);
    //                 dicRoot.Add(keyValue, stringValue);
    //             }
    //             // Debug.Log(xe.Name);
    //             // Debug.Log(xe.InnerText);
    //             // 					if (xe.Name == "key") {
    //             // 						int i = 0;
    //             // 						_tips = new string[xe.ChildNodes.Count];
    //             // 						foreach(XmlElement xe1 in xe.ChildNodes) {
    //             // //							Debug.Log(xe1.InnerText);
    //             // 							_tips[i] = xe1.InnerText;
    //             // 							i++;
    //             // 						}
    //             // 						break;
    //             // 					}
    //         }

    //         // 	 XmlNode baseAttribute = node.ChildNodes[0];
    //         // 	Debug.Log(baseAttribute.Name);
    //         // Debug.Log(baseAttribute.InnerText);
    //         //APPSTORE_APPID 


    // Debug.Log("Plist LoadPlist end");


    //     }

    public string GetString(string key)
    {
        string values;
        Debug.Log("Plist GetString 1");
        bool ret = dicRoot.TryGetValue(key, out values);
        if (ret)
        {
            Debug.Log("Plist GetString 2");
            return values;
            //Debug.Log(values);
        }
        Debug.Log("Plist GetString 3");
        return "NULL";
    }

}
*/