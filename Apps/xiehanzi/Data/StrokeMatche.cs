using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class StrokeMatche
{
    const int AVG_DIST_THRESHOLD = 350; // bigger = more lenient
    const float COSINE_SIMILARITY_THRESHOLD = 0; // -1 to 1, smaller = more lenient
    const float START_AND_END_DIST_THRESHOLD = 100;// 0.25f;//250; // bigger = more lenient
    const float FRECHET_THRESHOLD = 0.40f; // bigger = more lenient
    const float MIN_LEN_THRESHOLD = 0.35f; // smaller = more lenient

    public const int IMAGE_SIZE = 512;
    public const int IMAGE_MAX_SIZE = 1024;
    Vector2 pos;
    public List<Vector2> listPointDraw;
    public List<Vector2> listPointStroke;
    public float scaleWord=1f;
    public Vector3 wordPosition;//world position
    static private StrokeMatche _main = null;

        public static StrokeMatche main
    {
        get
        {
            if (_main == null)
            {
                _main = new StrokeMatche();
            }
            return _main;
        }
    }

    public Vector2 StrokePoint2WorldPoint(Vector2 pt)
    {
        float size = IMAGE_MAX_SIZE;
        float show_size = (IMAGE_SIZE/100f)*scaleWord;
        float x = -show_size / 2 + show_size * pt.x / size;
        float y = -show_size / 2 + show_size * (pt.y + ParseWordPointInfo.medianDataOffsetY) / size;
        return new Vector2(x, y);

    }

    public Vector2 WorldPoint2StrokePoint(Vector2 pt)
    {
        float size = IMAGE_MAX_SIZE;
        float show_size = (IMAGE_SIZE/100f)*scaleWord;
        // float x = pt.x
        float x = (pt.x - (-show_size / 2)) * size / show_size;
        float y = (pt.y - (-show_size / 2)) * size / show_size;
        y -= ParseWordPointInfo.medianDataOffsetY;
        //float y = -show_size / 2 + show_size * (pt.y + ParseWordPointInfo.medianDataOffsetY) / size;
        return new Vector2(x, y);

    }

    public bool StartAndEndMatches(float leniency)
    {
        float startingDist = Vector2.Distance(listPointDraw[0], listPointStroke[0]);
        float endingDist = Vector2.Distance(listPointDraw[listPointDraw.Count - 1], (listPointStroke[listPointStroke.Count - 1]));
        // Debug.Log("matches listPointDraw=" + listPointDraw[0] + " listPointStroke=" + listPointStroke[0]);
        Debug.Log("matches startingDist=" + startingDist + " endingDist=" + endingDist);
        if (startingDist <= START_AND_END_DIST_THRESHOLD * leniency && endingDist <= START_AND_END_DIST_THRESHOLD * leniency)
        {
            return true;
        }
        return false;
    }

    public void UpdateDrawPoint(List<Vector3> list)
    {
        if (listPointDraw == null)
        {
            listPointDraw = new List<Vector2>();
        }
        listPointDraw.Clear();
        foreach (Vector3 pt in list)
        {
            listPointDraw.Add(StrokeMatche.main.WorldPoint2StrokePoint(pt-wordPosition));
        }
    }
    public bool IsMatches(float leniency)
    {
        if (DirectionMatches(leniency) && StartAndEndMatches(leniency) && LengthMatches(leniency))
        {
            return true;
        }
        return false;
    }

    // 减法
    public Vector2 Subtract(Vector2 p1, Vector2 p2)
    {
        return new Vector2(p1.x - p2.x, p1.y - p2.y);
    }
    // const subtract = (p1, p2) => ({x: p1.x - p2.x, y: p1.y - p2.y});

    // 摸
    public float Magnitude(Vector2 p)
    {
        return Mathf.Sqrt(Mathf.Pow(p.x, 2) + Mathf.Pow(p.y, 2));
    }
    public float CosineSimilarity(Vector2 p1, Vector2 p2)
    {
        float rawDotProduct = p1.x * p1.x + p2.y * p2.y;
        float m1 = Magnitude(p1);
        float m2 = Magnitude(p2);
        if((m1==0)||(m2==0))
        {
            return 0;
        } 
        return rawDotProduct / m1 / m2;
    }

    public List<Vector2> GetEdgeVectors(List<Vector2> list)
    {
        List<Vector2> listRet = new List<Vector2>();
        Vector2 lastPoint = list[0];
        for (int i = 1; i < list.Count; i++)
        {
            listRet.Add(Subtract(list[1], lastPoint));
            lastPoint = list[i];
        }
        return listRet;
    }

    public bool DirectionMatches(float leniency)
    {
        // return true;
        List<Vector2> edgeVectors = GetEdgeVectors(listPointDraw);
        List<Vector2> listNew = new List<Vector2>();
        foreach (Vector2 pt in listPointStroke)
        {
            listNew.Add((pt));
        }

        List<Vector2> strokeVectors = GetEdgeVectors(listNew);
        float total_value = 0;
        foreach (Vector2 ptedge in edgeVectors)
        {
            float max = 0;
            foreach (Vector2 ptstroke in strokeVectors)
            {
                float sim = CosineSimilarity(ptstroke, ptedge);
                // Debug.Log("matches DirectionMatches sim=" + sim);
                if (sim > max)
                    max = sim;
            }
            total_value += max;
        }
        float avgSimilarity = total_value / edgeVectors.Count;
        Debug.Log("matches DirectionMatches avgSimilarity=" + avgSimilarity + " Count=" + edgeVectors.Count);
        return (avgSimilarity > COSINE_SIMILARITY_THRESHOLD) ? true : false;

        // const similarities = edgeVectors.map(edgeVector =>
        // {
        //     const strokeSimilarities = strokeVectors.map(strokeVector => cosineSimilarity(strokeVector, edgeVector));
        //     return Math.max.apply(Math, strokeSimilarities);
        // });
        // const avgSimilarity = average(similarities);
        // return avgSimilarity > COSINE_SIMILARITY_THRESHOLD;
    }
    public bool LengthMatches(float leniency)
    {
        // return true;
        float ratio = (leniency * (listPointDraw.Count + 25) / (listPointStroke.Count + 25));
        Debug.Log("matches ratio=" + ratio);
        return ratio >= MIN_LEN_THRESHOLD ? true : false;
    }


}
