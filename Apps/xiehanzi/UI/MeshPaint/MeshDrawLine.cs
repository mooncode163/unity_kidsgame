using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
//代码算法改编来源：
http://merowing.info/2012/04/drawing-smooth-lines-with-cocos2d-ios-inspired-by-paper/
git:
https://github.com/krzysztofzablocki/KZLineDrawer

*/
public delegate void OnMeshDrawLineDidDrawDelegate();
[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter), typeof(BoxCollider))]
public class MeshDrawLine : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{

    public const int DIRECTION_BISHUN_DOWN = 0;
    public const int DIRECTION_BISHUN_RIGHT = 1;

    public Camera mainCamera;
    public Rect rectDraw = Rect.zero;
    private Mesh mesh;
    private Vector3 point = Vector3.up;
    private int numbeOfPoints = 10;
    private Vector3[] vertices;
    private int[] triangles;
    private int maxPoint = 25;//20


    //private Color color = Color.red;
    // private List<Vector3>listPoint;
    public List<Vector3> listPoint;
    public int totalPoint;
    
    private float lineWidth = 0.5f;

    Vector3 prevC, prevD, prevG, prevI;
    //  float overdraw = 10.0f;
    BoxCollider boxCollider;

    public OnMeshDrawLineDidDrawDelegate callbackDraw{get;set;}


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {


        Renderer render = GetComponent<Renderer>();
        Material mat = new Material(Shader.Find("Custom/MeshDrawLine"));
        render.material = mat;

        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "WordWrite Mesh";
        // Debug.Log("setDrawTriangles");
        listPoint = new List<Vector3>();
        //setDrawTriangles();
        // lineWidth = 0.5f;//0.5f
       // SetColor(Color.red);


    }
    // Use this for initialization
    void Start()
    {

        //initDraw();
    }

    public void Init()
    {
        Vector2 sizeworld = Common.GetWorldSize(mainCamera);
        boxCollider = gameObject.GetComponent<BoxCollider>();
        boxCollider.size = new Vector3(sizeworld.x, sizeworld.y, 1f);
         
    }

    //相当于touchDown
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("MeshDrawLine OnPointerDown:p=" + eventData.position);
        ClearDraw();
    }
    //相当于touchUp
    public void OnPointerUp(PointerEventData eventData)
    {

    }
    //相当于touchMove
    public void OnDrag(PointerEventData eventData)
    {
        OnDraw();
    }

    bool EnableDraw(Vector2 pos)
    {
        bool ret = true;
        if (rectDraw == Rect.zero)
        {
            return true;
        }
        if (!rectDraw.Contains(pos))
        {
            ret = false;
        }
        return ret;
    }
    bool Vector3FuzzyEqual(Vector3 a, Vector3 b, float var)
    {
        if (a.x - var <= b.x && b.x <= a.x + var)
            if (a.y - var <= b.y && b.y <= a.y + var)
                return true;
        return false;
    }

    //逆时针选择90度
    Vector3 Vector3Perp(Vector3 v)
    {
        Vector3 ret;
        ret.x = -v.y;
        ret.y = v.x;
        ret.z = v.z;
        return ret;
    }


    public void SetColor(Color color)
    {
        Renderer render = GetComponent<Renderer>();

        render.material.SetColor("_DrawColor", color);
    }

    public void setDrawLineWidth(float w)
    {
        lineWidth = w;
    }
    void setDrawTriangles()
    {

        vertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(1, 5, 0), new Vector3(-1, 5, 0), new Vector3(0, 0, 0), new Vector3(1, -5, 0), new Vector3(-1, -5, 0) };
        triangles = new int[] { 0, 1, 2, 3, 4, 5 };
        // float angle = -360f/numbeOfPoints;
        // for(int v=1,t=1;v<vertices.Length;v++,t+=3){
        // 	vertices[v] = Quaternion.Euler(0f,0f,angle*(v-1))*point;
        // 	triangles[t] = v;
        // 	triangles[t+1] = v+1;

        // }
        // triangles[triangles.Length-1]=1;
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }

    void setDraw()
    {

        vertices = new Vector3[numbeOfPoints + 1];
        triangles = new int[numbeOfPoints * 3];
        float angle = -360f / numbeOfPoints;
        for (int v = 1, t = 1; v < vertices.Length; v++, t += 3)
        {
            vertices[v] = Quaternion.Euler(0f, 0f, angle * (v - 1)) * point;
            triangles[t] = v;
            triangles[t + 1] = v + 1;

        }
        triangles[triangles.Length - 1] = 1;
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }


    public void AddPoint(Vector3 vec)
    {
        if (listPoint == null)
        {
            Debug.Log("AddPoint:listPoint=null");
            return;
        }
        if (listPoint.Count > maxPoint)
        {
            // listPoint.RemoveAt(0);
        }
        listPoint.Add(vec);


    }
    public void ClearDraw()
    {
        listPoint.Clear();
        mesh.Clear();
    }
    public void DrawLine()
    {

        if (listPoint == null)
        {
            return;
        }
        List<Vector3> listVert = new List<Vector3>();


        // DrawLine1();
        // return;

        if (listPoint.Count < 2)
        {
            //清空
            mesh.Clear();
            return;
        }
        //Debug.Log("DrawLine:count="+listPoint.Count);

        Vector3 prevPoint = listPoint[0];
        float prevValue = lineWidth;
        float curValue;

        int size = listPoint.Count;
        int index = 0;
        for (int i = 1; i < (size); i++)
        {

            Vector3 curPoint = listPoint[i];
            curValue = lineWidth;


            //! equal points, skip them 0.0001f
            if (Vector3FuzzyEqual(curPoint, prevPoint, 0.01f))
            {
                continue;
            }

            Vector3 dir = curPoint - prevPoint;
            Vector3 perpendicular = Vector3Perp(dir).normalized;
            Vector3 A = prevPoint + perpendicular * (prevValue / 2);
            Vector3 B = prevPoint - perpendicular * (prevValue / 2);
            Vector3 C = curPoint + perpendicular * (curValue / 2);
            Vector3 D = curPoint - perpendicular * (curValue / 2);

            //! continuing line
            if (index > 0)
            {
                A = prevC;
                B = prevD;
            }
            else if (index == 0)
            {
                //! circle at start of line, revert direction
                // [circlesPoints addObject:pointValue];
                // [circlesPoints addObject:linePoints[i - 1]];
            }

            //ABC TRIANGLES
            AddTriAngle(A, B, C);
            listVert.Add(A);
            listVert.Add(B);
            listVert.Add(C);

            index = +3;
            //BCD TRIANGLES
            AddTriAngle(B, C, D);
            listVert.Add(B);
            listVert.Add(C);
            listVert.Add(D);
            index = +3;

            prevD = D;
            prevC = C;
            prevPoint = curPoint;




            // //! Add overdraw
            // Vector3 F = A + perpendicular * overdraw;
            // Vector3 G = C + perpendicular * overdraw;
            // Vector3 H = B - perpendicular * overdraw;
            // Vector3 I = D - perpendicular * overdraw;

            // //! end vertices of last line are the start of this one, also for the overdraw
            // if (index > 6)
            // {
            //     F = prevG;
            //     H = prevI;
            // }

            // prevG = G;
            // prevI = I;

            // AddTriAngle(F, A, G);
            // index = +3;
            // AddTriAngle(A, G, C);
            // index = +3;
            // AddTriAngle(B, H, D);
            // index = +3;
            // AddTriAngle(H, D, I);
            // index = +3;

        }

        mesh.Clear();
        vertices = new Vector3[listVert.Count];
        triangles = new int[listVert.Count];
        for (int i = 0; i < listVert.Count; i++)
        {
            vertices[i] = listVert[i];
            //z固定为0
            vertices[i].z = 0f;

            triangles[i] = i;
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }



    void AddTriAngle(Vector3 v1, Vector3 v2, Vector3 v3)
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDrawFail()
    {
        int count = listPoint.Count;
        if (count > 0)
        {

            //保证画每个笔画的总时间一致
            int one_bihua_draw_count = 10;

            int point_step_min = 2;
            int point_step = totalPoint / one_bihua_draw_count;
            if (point_step < point_step_min)
            {
                point_step = point_step_min;
            }
            //point_step = 1;
            //foreach (Vector2 point in listBihua)
            for (int i = 0; i < point_step; i++)
            {
                count = listPoint.Count;
                if (count >= 1)
                {
                    listPoint.RemoveAt(count - 1);
                }

            }
            DrawLine();
        }

    }
    public void OnDraw()
    {
        float z = gameObject.transform.position.z;
        Vector2 pos = Common.GetInputPosition();
        Vector2 posword = mainCamera.ScreenToWorldPoint(pos);
        z = 0f;

        if (!EnableDraw(posword))
        {
            return;
        }

        AddPoint(new Vector3(posword.x, posword.y, z));
        DrawLine();
        
        if (callbackDraw != null)
        {
            callbackDraw();
        }

    }



}
