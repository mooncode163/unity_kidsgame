using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vectrosity;

public class UIBgWrite : UIView
{
    public UISprite uiBg;
    public GameObject objBoard;
    public UISprite uiWriteBoard;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {

    }
    // Use this for initialization
    void Start()
    {
        Vector2 size = uiWriteBoard.GetBoundSize();
        Debug.Log("UIBgWrite GetBoundSize size=" + size);
        LayOut();
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {

    }

    public void OnMode(UIWordWrite.Mode md)
    {
        if (objBoard != null)
        {

            foreach (LayOutRelation ly in objBoard.GetComponents<LayOutRelation>())
            {
                ly.enableLayout = (md == UIWordWrite.Mode.FREE_WRITE) ? false : true;
                ly.LayOut();
            }
            if (md == UIWordWrite.Mode.FREE_WRITE)
            {
                float z = objBoard.transform.localPosition.z;
                objBoard.transform.localPosition = new Vector3(0, 0, z);
            }

        }
        LayOut();
    }
    public override void LayOut()
    {
        base.LayOut();
        LayOutScale ly = uiWriteBoard.GetComponent<LayOutScale>();
        float ofty = Common.CanvasToWorldHeight(mainCam, AppSceneBase.main.sizeCanvas, 160f);
        ly.offsetMax = new Vector2(0, ofty);
        ly.offsetMin = new Vector2(0, ofty);
        ly.LayOut();
        base.LayOut();
    }

    public Vector2 GetBoundBoard()
    {
        return uiWriteBoard.GetBoundSize();
    }
}
