using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UIView : MonoBehaviour
{
    UIViewController _controller;

    public UIViewController controller
    {
        get
        {
            UIViewController c = _controller;
            if (c == null)
            {
                //查找父对象
                foreach (Transform parent in this.GetComponentsInParent<Transform>(true))
                {
                    if (parent == null)
                    {
                        break;
                    }
                    if (parent.gameObject == this.gameObject)
                    {
                        continue;
                    }

                    UIView ui = parent.gameObject.GetComponent<UIView>();
                    if (ui != null)
                    {
                        UIViewController ctmp = ui.GetControllerInternal();
                        if (ctmp != null)
                        {
                            c = ctmp;
                            break;
                        }
                    }

                }

            }
            return c;
        }
        set
        {
            _controller = value;
        }
    }

    public Camera mainCam
    {
        get
        {
            if (AppSceneBase.main == null)
            {
                Debug.Log("UIView::AppSceneBase.main==null");
            }
            return AppSceneBase.main.mainCamera;
        }
    }

    public Rect frame
    {
        get
        {
            return GetFrame(this.GetComponent<RectTransform>());
        }
    }

    public Rect frameParent
    {
        get
        {
            return GetFrame(this.transform.parent.GetComponent<RectTransform>());
        }
    }

    public Rect frameMainWorld
    {
        get
        {
            return GetFrame(AppSceneBase.main.objMainWorld.GetComponent<RectTransform>());
        }
    }


    public string keyText;
    public string keyColor;

    public string keyImage;
    public string keyImageH;//only for landscap 横屏
    static public Rect GetFrame(RectTransform rctran)
    {
        Rect rc = Rect.zero;
        if (rctran != null)
        {
            rc = rctran.rect;
        }
        return rc;
    }


    public void Awake()
    {

    }
    // Use this for initialization
    public void Start()
    {
        LayOut();
    }

    public virtual void LayOut()
    {
        LayOutInternal();

        // 统一更新一次 解决子UIView更新后整体没有更新的bug
        Invoke("LayOutInternal", 0.1f);
    }

    public virtual void UpdateLanguage()
    {
        foreach (UIView child in this.gameObject.GetComponentsInChildren<UIView>(true))
        {
            if (child == null)
            {
                continue;
            }
            GameObject objtmp = child.gameObject;
            if (this.gameObject == objtmp)
            {
                continue;
            }
            child.UpdateLanguage();
        }
    }
    public UIViewController GetControllerInternal()
    {
        return _controller;
    }

    void LayOutObj(GameObject obj)
    {
        Component[] list = obj.GetComponents<LayOutBase>();
        foreach (LayOutBase ly in list)
        {
            if (ly)
            {
                ly.LayOut();
            }
        }
    }
    public void LayOutInternal()
    {
        //self 
        this.LayOutObj(this.gameObject);

        foreach (LayOutBase ly in this.gameObject.GetComponentsInChildren<LayOutBase>(true))
        {
            if (ly)
            {
                ly.LayOut();
            }
        }
    }

    public void SetController(UIViewController con)
    {
        if(con==null)
        {
            return;
        }
        controller = con;
        //this.transform.parent = controller.objController.transform;
        this.transform.SetParent(controller.objController.transform);
        con.view = this;
    }

    public void SetViewParent(GameObject obj)
    {
        this.transform.parent = obj.transform;
        this.transform.localScale = new Vector3(1f, 1f, 1f);
        this.transform.localPosition = new Vector3(0f, 0f, 0f);
    }
    // 
    public Color GetKeyColor()
    {
        return GetKeyColor(Color.white);
    }
    public Color GetKeyColor(Color def)
    {
        Color ret = def;
        if (!Common.isBlankString(keyColor))
        {
            ret = GetColorOfKey(keyColor);
        }
        return ret;
    }

    public Color GetColorOfKey(string key)
    {
        Color ret = Color.black;
        if (!Common.isBlankString(key))
        {
            ret = ColorConfig.main.GetColor(key);
        }
        return ret;
    }

    public string GetKeyText()
    {
        return GetTextOfKey(keyText);
    }
    public string GetTextOfKey(string key)
    {
        string ret = "";
        if (!Common.isBlankString(key))
        {
            ret = Language.main.GetString(key);
        }
        return ret;
    }

    public string GetKeyImage()
    {
        string ret = "";
        if (!Common.isBlankString(keyImage))
        {
            ret = ImageRes.main.GetImage(keyImage);
        }
        return ret;
    }
    public void OnUIDidFinish(float delay = 0)
    {
        float time = delay;
        if (time <= 0)
        {
            time = 0.1f;
        }
        Invoke("DoUIFinish", time);
    }

    void DoUIFinish()
    {
        if (this.controller != null)
        {
            if (controller.callbackUIFinish != null)
            {
                controller.callbackUIFinish();
            }
        }

        // Common.UnityStartUpFinish();
    }

    public Vector2 GetBoundSize()
    {
        Vector2 ret = Vector2.zero;
        Renderer rd = null;
        UISprite uisp = this.gameObject.GetComponent<UISprite>();
        if (uisp != null)
        {
            rd = uisp.objSp.GetComponent<Renderer>();
        }
        if (rd == null)
        {
            rd = this.gameObject.GetComponent<Renderer>();
        }

        if (rd != null)
        {
            ret = rd.bounds.size;
        }

        return ret;
    }

     public void SetActive(bool isActive)
    {
        this.gameObject.SetActive(isActive);
        LayOut();
    }

}
