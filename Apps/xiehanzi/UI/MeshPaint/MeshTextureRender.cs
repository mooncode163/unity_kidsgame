using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//mesh 平面图形的贴图 例子：http://blog.csdn.net/nanggong/article/details/54728823
[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class MeshTextureRender : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    Texture texRender;
    BoxCollider boxCollider;
    Material matRender;
    float width;
    float height;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {

        if (matRender == null)
        {
            matRender = new Material(Shader.Find("Custom/MeshTexture"));
        }
        gameObject.GetComponent<MeshRenderer>().material = matRender;
        boxCollider = gameObject.GetComponent<BoxCollider>();

    }
    // Use this for initialization
    void Start()
    {
        DrawSquare(0, 0, width, height);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateTexture(Texture tex)
    {
        texRender = tex;
        if (matRender == null)
        {
            matRender = new Material(Shader.Find("Custom/MeshTexture"));
        }
        matRender.SetTexture("_MainTex", tex);

    }

    public void UpdateSize(Vector2 size)
    {
        width = size.x;
        height = size.y;
         boxCollider = gameObject.GetComponent<BoxCollider>();
        boxCollider.size = new Vector3(width, height, 1f);

    }


    //相当于touchDown
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("MeshTextureRender OnPointerDown:p=" + eventData.position);
    }
    //相当于touchUp
    public void OnPointerUp(PointerEventData eventData)
    {

    }
    //相当于touchMove
    public void OnDrag(PointerEventData eventData)
    {

    }
    //画三角形
    void DrawTriangle()
    {
        // gameObject.AddComponent<MeshFilter>();
        // gameObject.AddComponent<MeshRenderer>();
        // gameObject.GetComponent<MeshRenderer>().material = mat;

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();

        //设置顶点
        mesh.vertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0) };
        mesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1) };

        mesh.triangles = new int[] { 0, 1, 2 };
    }

    //方形
    void DrawSquare(float x, float y, float w, float h)
    {
        // gameObject.AddComponent<MeshFilter>();
        // gameObject.AddComponent<MeshRenderer>();
        // gameObject.GetComponent<MeshRenderer>().material = mat;

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();

        // mesh.vertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0), new Vector3(1, 0, 0) };
        mesh.vertices = new Vector3[] { new Vector3(x - w / 2, y - h / 2, 0), new Vector3(x - w / 2, y + h / 2, 0), new Vector3(x + w / 2, y + h / 2, 0), new Vector3(x + w / 2, y - h / 2, 0) };


        mesh.triangles = new int[]
        { 0, 1, 2,
              0, 2, 3
        };
        mesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };
    }

    #region 画圆
    /// <summary>
    /// 画圆
    /// </summary>
    /// <param name="radius">圆的半径</param>
    /// <param name="segments">圆的分割数</param>
    /// <param name="centerCircle">圆心得位置</param>
    void DrawCircle(float radius, int segments, Vector3 centerCircle)
    {
        // gameObject.AddComponent<MeshFilter>();
        // gameObject.AddComponent<MeshRenderer>();
        // gameObject.GetComponent<MeshRenderer>().material = mat;

        //顶点
        Vector3[] vertices = new Vector3[segments + 1];
        vertices[0] = centerCircle;
        float deltaAngle = Mathf.Deg2Rad * 360f / segments;
        float currentAngle = 0;
        for (int i = 1; i < vertices.Length; i++)
        {
            float cosA = Mathf.Cos(currentAngle);
            float sinA = Mathf.Sin(currentAngle);
            vertices[i] = new Vector3(cosA * radius + centerCircle.x, sinA * radius + centerCircle.y, 0);
            currentAngle += deltaAngle;
        }

        //三角形
        int[] triangles = new int[segments * 3];
        for (int i = 0, j = 1; i < segments * 3 - 3; i += 3, j++)
        {
            triangles[i] = 0;
            triangles[i + 1] = j + 1;
            triangles[i + 2] = j;
        }
        triangles[segments * 3 - 3] = 0;
        triangles[segments * 3 - 2] = 1;
        triangles[segments * 3 - 1] = segments;

        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x / radius / 2 + 0.5f, vertices[i].y / radius / 2 + 0.5f);
        }


        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
    }
    #endregion

    #region 画圆环
    /// <summary>
    /// 画圆环
    /// </summary>
    /// <param name="radius">圆半径</param>
    /// <param name="innerRadius">内圆半径</param>
    /// <param name="segments">圆的分个数</param>
    /// <param name="centerCircle">圆心坐标</param>
    void DrawRing(float radius, float innerRadius, int segments, Vector3 centerCircle)
    {
        // gameObject.AddComponent<MeshFilter>();
        // gameObject.AddComponent<MeshRenderer>();
        // gameObject.GetComponent<MeshRenderer>().material = mat;

        //顶点
        Vector3[] vertices = new Vector3[segments * 2];
        float deltaAngle = Mathf.Deg2Rad * 360f / segments;
        float currentAngle = 0;
        for (int i = 0; i < vertices.Length; i += 2)
        {
            float cosA = Mathf.Cos(currentAngle);
            float sinA = Mathf.Sin(currentAngle);
            vertices[i] = new Vector3(cosA * innerRadius + centerCircle.x, sinA * innerRadius + centerCircle.y, 0);
            vertices[i + 1] = new Vector3(cosA * radius + centerCircle.x, sinA * radius + centerCircle.y, 0);
            currentAngle += deltaAngle;
        }

        //三角形
        int[] triangles = new int[segments * 6];
        for (int i = 0, j = 0; i < segments * 6; i += 6, j += 2)
        {
            triangles[i] = j;
            triangles[i + 1] = (j + 1) % vertices.Length;
            triangles[i + 2] = (j + 3) % vertices.Length;

            triangles[i + 3] = j;
            triangles[i + 4] = (j + 3) % vertices.Length;
            triangles[i + 5] = (j + 2) % vertices.Length;
        }

        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x / innerRadius / 2 + 0.5f, vertices[i].y / innerRadius / 2 + 0.5f);
        }

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

    }
    #endregion

}


