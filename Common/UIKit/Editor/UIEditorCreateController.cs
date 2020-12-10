using UnityEngine;
using System.Collections;
using UnityEditor;
public class UIEditorCreateController : EditorWindow
{
    bool groupEnabled = false;
    bool myBool1 = true;
    bool myBool2 = false;
    float myFloat1 = 1.0f;
    float myFloat2 = .5f;

    string strInput = "";

    // Use this for initialization
    void Start()
    {
        //窗口弹出时候调用
        Debug.Log("My Window　Start");
    }

    // Update is called once per frame
    void Update()
    {
        //窗口弹出时候每帧调用
        //Debug.Log("My Window　Update");
    }




    void OnGUI()
    {
        //在弹出窗口中控制变量 
        // myBool1 = EditorGUILayout.Toggle("Open Optional Settings", myBool1);
        // myFloat1 = EditorGUILayout.Slider("myFloat1", myFloat1, -3, 3);


        //创建一个GUILayout 通过groupEnabled 来控制当前GUILayout是否在Editor里面可以编辑
        // groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        // myBool2 = EditorGUILayout.Toggle("myBool2", myBool2);
        // myFloat2 = EditorGUILayout.Slider("myFloat2", myFloat2, -3, 3);
        // EditorGUILayout.EndToggleGroup();


        if (GUI.Button(new Rect(65, 180, 100, 30), "BtnCreate"))
        {
            Debug.Log("My Button On Pressed Do Create");
            OnCreateController(strInput);
        }

        //创建input
        strInput = GUI.TextField(new Rect(65, 280, 100, 30), strInput, 100);
        // Debug.Log("strInput=" + strInput);

    }

    void OnCreateController(string key)
    {



        // SampleViewController UISampleController
        // F:\sourcecode\unity\product\kidsgame\kidsgameUnity\Assets\Script\Common\UIKit\ViewController\Sample
        string assetsFolderPath = Application.dataPath;
        string dir = Application.dataPath + "/Script/Common/UIKit/ViewController/Sample/";
        string fileController = dir + "SampleViewController.cs";
        string fileUIController = dir + "UISampleController.cs";
        // Unity 编辑器下获取选择文件路径
        string[] strs = Selection.assetGUIDs;
        if(strs.Length==0)
        {
            return;
        }
        string pathSelect = AssetDatabase.GUIDToAssetPath(strs[0]);
        string fileControllerCreate = pathSelect + "/" + key + "ViewController.cs";
        string fileUIControllerCreate = pathSelect + "/" + "UI" + key + "Controller.cs";

        Debug.Log("OnCreateController fileController=" + fileController);
        Debug.Log("OnCreateController fileControllerCreate=" + fileControllerCreate);


        FileUtil.CopyOneFile(fileController, fileControllerCreate);
        FileUtil.CopyOneFile(fileUIController, fileUIControllerCreate);

        {
            string content = FileUtil.ReadStringFromFile(fileControllerCreate);
            content = content.Replace("Sample", key);
            FileUtil.WriteStringToFile(fileControllerCreate, content);
        }
        {
            string content = FileUtil.ReadStringFromFile(fileUIControllerCreate);
            content = content.Replace("Sample", key);
            FileUtil.WriteStringToFile(fileUIControllerCreate, content);
        }


    }

}