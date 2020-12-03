using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vectrosity;
public class WordWrite2 : MonoBehaviour
{

    public Camera mainCamera;
    private Vector3 point = Vector3.up;
    private int maxPoint = 25;//20


    //private Color color = Color.red;
    // private List<Vector3>listPoint;
    public List<Vector3> listPoint;
    public int totalPoint;
    public Rect rectDraw;

    Vector3 prevC, prevD, prevG, prevI;
    //  float overdraw = 10.0f;


    VectorLine lineCur;
    float lineWidth = 20f;//屏幕像素
    Material matLine;
    GameObject objLine;
    Color colorLine = Color.red;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {

        CreateLine();

    }
    // Use this for initialization
    void Start()
    {

    }
    public void setColor(Color color)
    {
        colorLine = color;
    }

    public void SetLineWidthPixsel(float w)
    {
         Debug.Log("SetLineWidthPixsel w="+w);
        lineWidth = w;
        if (lineCur != null)
        {
            lineCur.lineWidth = lineWidth;
        }
    }

    //world size
    public void setDrawLineWidth(float w)
    {
        SetLineWidthPixsel(Common.WorldToScreenWidth(mainCamera, w));
    }

    public void SetDrawLineWidthPixsel(float w)
    {
        SetLineWidthPixsel(w);
    }

    public void ClearDraw()
    {
        Clear();

    }
    public void DrawLine()
    {
        if (lineCur != null)
        {
            lineCur.Draw3D();
        }
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
        {

            z = 0f;
            Vector2 posword = Common.GetInputPositionWorld(mainCamera);
            Vector2 ptlocal = this.transform.InverseTransformPoint(posword);
            if (!rectDraw.Contains(ptlocal))
            {
                return;
            }

            AddPoint(new Vector3(ptlocal.x, ptlocal.y, z));
            DrawLine();
        }
    }

    public void Clear()
    {
        if (listPoint != null)
        {
            listPoint.Clear();
        }
        if (lineCur != null)
        {
            lineCur.Draw3D();
        }

    }
    void SetLineColor(Color cr)
    {
        if (matLine != null)
        {
            matLine.SetColor("_Color", cr);
        }
        if (lineCur != null)
        {
            lineCur.color = cr;
        }
    }
    void CreateLine()
    {
        matLine = new Material(Shader.Find("Custom/PaintLine"));
        listPoint = new List<Vector3>();
        // listPointCur.Add(Vector3.zero);
        // listPointCur.Add(new Vector3(5, 0, 0));

        lineCur = new VectorLine("line", listPoint, lineWidth);
        lineCur.lineType = LineType.Continuous;
        //圆滑填充画线
        lineCur.joins = Joins.Fill;
        lineCur.Draw3D();
        objLine = lineCur.GetObj();
        objLine.transform.SetParent(this.gameObject.transform);
        objLine.transform.localPosition = Vector3.zero;
        lineCur.material = matLine;
        lineCur.color = colorLine;
    }

    Vector3 GetTouchLocalPosition(Vector3 pos)
    {
        // Vector2 inputPos = Common.GetInputPosition();
        // Vector3 posTouchWorld = mainCam.ScreenToWorldPoint(inputPos);
        Vector3 loaclPos = this.transform.InverseTransformPoint(pos);
        loaclPos.z = 0;
        // posTouchWorld.z = 0;
        return loaclPos;
    }
    //添加世界坐标
    public void AddPoint(Vector3 pos)
    {
        pos.z = 0;
        Debug.Log("PaintLine AddPoint pos=" + pos);
        listPoint.Add(GetTouchLocalPosition(pos));
        //listPointCur.Add(pos);
        SetLineColor(colorLine);
        lineCur.Draw3D();
    }

}
