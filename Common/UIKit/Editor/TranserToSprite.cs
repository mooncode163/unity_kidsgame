using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;

//【Unity3D Editor】导入图片自动转换成Sprite https://blog.csdn.net/eazey_wj/article/details/61287214
public class TranserToSprite : AssetPostprocessor {

    void OnPreprocessTexture()
    {
        Debug.Log("OnPreprocessTexture assetPath="+assetImporter.assetPath);
        string filepath = assetImporter.assetPath; 
        string strext = FileUtil.GetFileExt(assetImporter.assetPath);
        if ((strext=="png")||(strext=="jpg"))
        {
            //texImpoter是图片的Import Settings对象
            //AssetImporter是TextureImporter的基类
            TextureImporter texImpoter = assetImporter as TextureImporter;
            //TextureImporterType是结构体，包含所有Texture Type
            texImpoter.textureType = TextureImporterType.Sprite;
        }
    }
} 