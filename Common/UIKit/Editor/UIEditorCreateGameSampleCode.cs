using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class UIEditorCreateGameSampleCode : EditorWindow
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
            OnCreateSampleCode(strInput);
        }

        //创建input
        strInput = GUI.TextField(new Rect(65, 280, 100, 30), strInput, 100);
        // Debug.Log("strInput=" + strInput);

    }

    void ChangeFile(string filepath, string key)
    {
        string filepathnew = filepath.Replace("Sample", key);
        string content = FileUtil.ReadStringFromFile(filepath);
        content = content.Replace("Sample", key);
        FileUtil.WriteStringToFile(filepath, content);
        FileUtil.Rename(filepath, filepathnew);
    }

    // 先拷贝 GameProjectSample 然后再执行make
    void OnCreateSampleCode(string key)
    {
        string dirSampleCode = Resource.dirProductCommon + "/PythonUnity/GameProjectSample";
        string dirDstCode = Resource.dirScript + "/Apps/" + key;

        if (Directory.Exists(dirDstCode))
        {
            return;
        }
        FileUtil.CreateDir(dirDstCode);
        FileUtil.CopyDir(dirSampleCode, dirDstCode);



        {
            string filepath = dirDstCode + "/UI/Home/UIHomeSample.cs";
            ChangeFile(filepath, key);

        }

        {

            string filepath = dirDstCode + "/UI/Game/UIGameSample.cs";
            ChangeFile(filepath, key);
        }
        {

            string filepath = dirDstCode + "/UI/Game/GameSample.cs";
            ChangeFile(filepath, key);
        }

          dirDstCode = Resource.dirScript + "/Apps/GameProjectSample";
        FileUtil.DeleteDir(dirDstCode);

    

    }

}