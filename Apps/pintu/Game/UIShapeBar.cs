using System.Collections;
using System.Collections.Generic;
using LitJson;
using Moonma.SysImageLib;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIShapeBar : UIView//, ISysImageLibDelegate
{
    public GameObject objScrollView;
    public GameObject objScrollViewContent;
    ScrollRect scrollRect;


    public List<object> listItem;
    UIShapeBarItem uiShapeBarItemPrefab;
    float widthItem;
    public OnUIShapeBarItemDidClickDelegate callBackDidClick { get; set; }
    void Awake()
    {
        listItem = new List<object>();
        GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/Game/UIShapeBarItem");
        uiShapeBarItemPrefab = obj.GetComponent<UIShapeBarItem>();

        scrollRect = objScrollView.GetComponent<ScrollRect>();

    }


    // Use this for initialization
    void Start()
    {
        ParseShapList();
        LayOut();
    } 



    public override void LayOut()
    {
        base.LayOut();

    }

    void ParseShapList()
    {
        if (listItem.Count != 0)
        {
            return;
        }
        string filePath = "AppCommon/UI/Game/Shape/shape_list";
        //FILE_PATH
        string json = FileUtil.ReadStringFromResources(filePath);//((TextAsset)Resources.Load(fileName, typeof(TextAsset))).text;
        JsonData root = JsonMapper.ToObject(json);
        JsonData items = root["items"];
        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];
            string pic = "AppCommon/UI/Game/Shape/" + (string)item["id"]+".png";
            AddItem(pic);
        }

    }

    public void AddItem(string pic)
    {
        ItemInfo info = new ItemInfo();
        listItem.Add(info);
        info.pic = pic;
        int idx = listItem.Count - 1;
        UIShapeBarItem item = (UIShapeBarItem)GameObject.Instantiate(uiShapeBarItemPrefab);
        item.transform.SetParent(objScrollViewContent.transform);
        item.callBackDidClick = OnUIShapeBarItemDidClick;
        // //this.transform;
        item.transform.localScale = new Vector3(1, 1, 1);
        item.transform.localPosition = new Vector3(0, 0, 0);
        item.index = idx;

        //更新scrollview 内容的长度
        RectTransform rctranItem = item.GetComponent<RectTransform>();
        RectTransform rctran = objScrollViewContent.GetComponent<RectTransform>();
        RectTransform rctranScroll = objScrollView.GetComponent<RectTransform>();
        Vector2 size = rctran.sizeDelta;
        size.y = rctranScroll.rect.height;

        widthItem = size.y;
        // Debug.Log("widthItem=" + widthItem);
        size.x = widthItem * (idx + 1);

        //   item.width = widthItem;
        //   item.height = size.y;
        rctran.sizeDelta = size;
        item.UpdateItem(info);
        // listTopItem.Add(item);
    }

    public void OnUIShapeBarItemDidClick(UIShapeBarItem item)
    {

        if (callBackDidClick != null)
        {
            callBackDidClick(item);
        }
    }
}
