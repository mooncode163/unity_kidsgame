using UnityEngine;
using System.Collections;
using UnityEditor;
public class UIEditorCreateController : EditorWindow {
 
    string myString = "My String";
 
    bool groupEnabled = false;
    bool myBool1 = true;
    bool myBool2 = false;
    float myFloat1 = 1.0f;
    float myFloat2 = .5f;
 
	// Use this for initialization
	void Start () {
        //窗口弹出时候调用
        Debug.Log("My Window　Start");
	}
	
	// Update is called once per frame
	void Update () {
        //窗口弹出时候每帧调用
        //Debug.Log("My Window　Update");
	}
 
 
    //定义弹出当前窗口的菜单位置  
    //[MenuItem("Window/My Window")]
    [MenuItem("MyGame/My Window")]
    static void Init()
    {
        //弹出窗口
        EditorWindow.GetWindow(typeof(MyWindow));
    }
 
    void OnGUI()
    {
        //在弹出窗口中控制变量
        myString = EditorGUILayout.TextField("My String", myString);
        myBool1 = EditorGUILayout.Toggle("Open Optional Settings", myBool1);
        myFloat1 = EditorGUILayout.Slider("myFloat1", myFloat1, -3, 3);
 
 
        //创建一个GUILayout 通过groupEnabled 来控制当前GUILayout是否在Editor里面可以编辑
        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        myBool2 = EditorGUILayout.Toggle("myBool2", myBool2);
        myFloat2 = EditorGUILayout.Slider("myFloat2", myFloat2, -3, 3);
        EditorGUILayout.EndToggleGroup();
 
        //创建一个按钮
        if (GUI.Button(new Rect(65, 180, 100, 30), "My Button"))
        {
            Debug.Log("My Button On Pressed");
        }
    }
}