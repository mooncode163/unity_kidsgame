using UnityEngine;
 
public class PhotoTool : MonoBehaviour
{
    public static PhotoTool instance;
    Vector3 pMax = Vector3.zero;
    Vector3 pMin = Vector3.zero;
    Vector3 center = Vector3.zero;
    private void Awake()
    {
        instance = this;
    }
    /// <summary>
    /// 拍照 返回texture2d
    /// </summary>
    /// <param name="obj">目标</param>
    /// <returns>texture2d</returns>
    public Texture2D TakePhotoTexture2D(GameObject obj)
    {
        if (obj == null)
        {
            return null;
        }
        int oldLayer = obj.layer;
 
        ChangeObjLayer(obj, LayerMask.NameToLayer("PhotoLayer"));
 
        Bounds b = ClacBounds(obj);
 
 
        GameObject cameraPhoto = new GameObject("CameraPhoto");
        cameraPhoto.hideFlags = HideFlags.HideAndDontSave;
        cameraPhoto.transform.position = b.center - new Vector3(0, 0, b.extents.z + 10);
        cameraPhoto.transform.LookAt(b.center);
 
        Camera c = cameraPhoto.AddComponent<Camera>();
        c.cullingMask = 1 << LayerMask.NameToLayer("PhotoLayer");
        c.clearFlags = CameraClearFlags.SolidColor;
        c.orthographic = true;
 
        float size = Mathf.Max(b.extents.x, b.extents.y);
 
        print(b.extents.x + "   " + b.extents.y);
        c.orthographicSize = size;
 
        Texture2D tex = CaptureCamera(c, new Rect(0, 0, 500, 500));
        Destroy(cameraPhoto);
        ChangeObjLayer(obj, oldLayer);
 
        return tex;
    }
    /// <summary>
    /// 更改目标所在层
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="layer"></param>
    private void ChangeObjLayer(GameObject obj, int layer)
    {
        obj.layer = layer;
 
 
        foreach (Transform item in obj.transform)
        {
            item.gameObject.layer = layer;
            ChangeObjLayer(item.gameObject, layer);
        }
    }
    /// <summary>
    /// 拍照 返回Sprite
    /// </summary>
    /// <param name="obj">目标</param>
    /// <returns>Sprite</returns>
    public Sprite TakePhotoSprite(GameObject obj)
    {
        Texture2D tex = TakePhotoTexture2D(obj);
        Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        return sprite;
    }
 
    /// <summary>  
    /// 对相机截图。   
    /// </summary>  
    /// <returns>The screenshot2.</returns>  
    /// <param name="camera">Camera.要被截屏的相机</param>  
    /// <param name="rect">Rect.截屏的区域</param>  
    Texture2D CaptureCamera(Camera camera, Rect rect)
    {
        // 创建一个RenderTexture对象  
        RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0);
        // 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机  
        camera.targetTexture = rt;
        camera.Render();
        //ps: --- 如果这样加上第二个相机，可以实现只截图某几个指定的相机一起看到的图像。  
        //ps: camera2.targetTexture = rt;  
        //ps: camera2.Render();  
        //ps: -------------------------------------------------------------------  
 
        // 激活这个rt, 并从中中读取像素。  
        RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(rect, 0, 0);// 注：这个时候，它是从RenderTexture.active中读取像素  
        screenShot.Apply();
 
        // 重置相关参数，以使用camera继续在屏幕上显示  
        camera.targetTexture = null;
        //ps: camera2.targetTexture = null;  
        RenderTexture.active = null; // JC: added to avoid errors  
        GameObject.Destroy(rt);
        // 最后将这些纹理数据，成一个png图片文件  
        byte[] bytes = screenShot.EncodeToPNG();
        string filename = Application.dataPath + "/Screenshot.png";
        System.IO.File.WriteAllBytes(filename, bytes);
        Debug.Log(string.Format("截屏了一张照片: {0}", filename));
 
        return screenShot;
    }
    /// <summary>
    /// 计算目标包围盒
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    private Bounds ClacBounds(GameObject obj)
    {
        Renderer mesh = obj.GetComponent<Renderer>();
 
        if (mesh != null)
        {
            Bounds b = mesh.bounds;
            pMax = b.max;
            pMin = b.min;
            center = b.center;
        }
 
 
        RecursionClacBounds(obj.transform);
 
        ClacCenter(pMax, pMin, out center);
 
        Vector3 size = new Vector3(pMax.x - pMin.x, pMax.y - pMin.y, pMax.z - pMin.z);
        Bounds bound = new Bounds(center, size);
        bound.size = size;
 
        print("size>" + size);
        bound.extents = size / 2f;
 
        return bound;
    }
    /// <summary>
    /// 计算包围盒中心坐标
    /// </summary>
    /// <param name="max"></param>
    /// <param name="min"></param>
    /// <param name="center"></param>
    private void ClacCenter(Vector3 max, Vector3 min, out Vector3 center)
    {
        float xc = (pMax.x + pMin.x) / 2f;
        float yc = (pMax.y + pMin.y) / 2f;
        float zc = (pMax.z + pMin.z) / 2f;
 
        center = new Vector3(xc, yc, zc);
 
        print("center>" + center);
    }
    /// <summary>
    /// 计算包围盒顶点
    /// </summary>
    /// <param name="obj"></param>
    private void RecursionClacBounds(Transform obj)
    {
        if (obj.transform.childCount <= 0)
        {
            return;
        }
 
        foreach (Transform item in obj)
        {
            Renderer m = item.GetComponent<Renderer>();
 
            if (m != null)
            {
                Bounds b = m.bounds;
                if (pMax.Equals(Vector3.zero) && pMin.Equals(Vector3.zero))
                {
                    pMax = b.max;
                    pMin = b.min;
                }
 
                if (b.max.x > pMax.x)
                {
                    pMax.x = b.max.x;
                }
 
                if (b.max.y > pMax.y)
                {
                    pMax.y = b.max.y;
                }
                if (b.max.z > pMax.z)
                {
                    pMax.z = b.max.z;
                }
                if (b.min.x < pMin.x)
                {
                    pMin.x = b.min.x;
                }
 
                if (b.min.y < pMin.y)
                {
                    pMin.y = b.min.y;
                }
                if (b.min.z < pMin.z)
                {
                    pMin.z = b.min.z;
                }
            }
            RecursionClacBounds(item);
        }
    }
}