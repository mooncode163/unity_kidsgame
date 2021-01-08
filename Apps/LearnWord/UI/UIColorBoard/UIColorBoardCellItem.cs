using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEngine.UI.Button;

public class UIColorBoardCellItem : UICellItemBase
{

    public Image imageBg;
 
    public Color color; 
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void UpdateItem(List<object> list)
    {
        Color cr = (Color)list[index];
        imageBg.color = cr;
        color = cr;
        LayOut();
    }
    public override bool IsLock()
    {

        return false;//imageBgLock.gameObject.activeSelf;
    }

    public override void LayOut()
    {
        // RectTransform rctran = imageBg.GetComponent<RectTransform>();
        // float ratio = 1f;
      
        // float scale = Common.GetBestFitScale(rctran.rect.width, rctran.rect.height, width, height) * ratio;
        // imageBg.transform.localScale = new Vector3(scale, scale, 1.0f);

    }
}
