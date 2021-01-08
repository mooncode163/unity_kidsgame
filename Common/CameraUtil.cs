using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;
using UnityEngine;

public class CameraUtil
{
    private const float k_NearClip = 9.5f;//0.3f;

    public static Vector3 ScreenPointToWorldPoint(Camera cam, Vector2 screenPoint, float distanceToCamera = k_NearClip)
    {
        //  float distanceToCamera = cam.nearClipPlane;
        //     distanceToCamera = 9.5f;
        return cam.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, distanceToCamera));
    }

    public static Vector3 GetWorldSize(Camera cam, float distanceToCamera = k_NearClip)
    {
        Vector3 ret = Vector2.zero;
        if (cam.orthographic)
        {
            float world_w = Common.GetCameraWorldSizeWidth(cam) * 2;
            float world_h = cam.orthographicSize * 2;
            return new Vector3(world_w, world_h, 1f);
        }
        float w = Screen.width;
        float h = Screen.height;
        Vector3 posworld_left = ScreenPointToWorldPoint(cam, new Vector2(0, 0), distanceToCamera);
        Vector3 posworld_right = ScreenPointToWorldPoint(cam, new Vector2(w - 1, 0), distanceToCamera);

        Vector3 posworld_up = ScreenPointToWorldPoint(cam, new Vector2(0, h - 1), distanceToCamera);
        Vector3 posworld_down = ScreenPointToWorldPoint(cam, new Vector2(0, 0), distanceToCamera);
        ret = new Vector3((posworld_right.x - posworld_left.x), (posworld_up.y - posworld_down.y), posworld_left.z);
        return ret;
    }

      public static Vector3 GetWorldSizeOfCanvasUI(Camera cam, GameObject objUI,float distanceToCamera = k_NearClip)
    {
        Vector3 ret = Vector2.zero;
        // 屏幕坐标
        Vector2 pos_screen = objUI.transform.position;
        RectTransform rctran = objUI.GetComponent<RectTransform>();

        float w = Common.CanvasToScreenWidth(AppSceneBase.main.sizeCanvas, rctran.rect.width * rctran.localScale.x);
        float h = Common.CanvasToScreenHeight(AppSceneBase.main.sizeCanvas, rctran.rect.height * rctran.localScale.y);
        
        Vector3 posworld_left = ScreenPointToWorldPoint(cam, new Vector2(pos_screen.x - w / 2, 0), distanceToCamera); 
        Vector3 posworld_right = ScreenPointToWorldPoint(cam, new Vector2(pos_screen.x +w / 2, 0), distanceToCamera); 
       

        Vector3 posworld_up = ScreenPointToWorldPoint(cam, new Vector2(0, pos_screen.y + h / 2), distanceToCamera);
        Vector3 posworld_down = ScreenPointToWorldPoint(cam, new Vector2(0, pos_screen.y - h / 2), distanceToCamera);
        ret = new Vector3((posworld_right.x - posworld_left.x), (posworld_up.y - posworld_down.y), posworld_left.z);
        return ret;
    }


    // 通过ray获取屏幕点击的世界坐标
    public static Vector3 GetWorldPositionByRay(Camera cam, Vector2 screenPoint)
    {
        Vector3 ret = Vector3.zero;
        var ray = cam.ScreenPointToRay(screenPoint);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            ret = hitInfo.point;
            Debug.Log("GetWorldPositionByRay posworld=" + ret);
        }

        return ret;

    }
}
