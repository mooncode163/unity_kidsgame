
using UnityEngine;

public class MathUtil
{
    //两点之间的角度 0-360
    static public float GetAngleOfTwoPoint(Vector2 ptStart, Vector2 ptEnd)
    {
        float angle = 0;
        float a = 360 * (Mathf.Atan(Mathf.Abs(ptEnd.y - ptStart.y) / Mathf.Abs(ptEnd.x - ptStart.x))) / (Mathf.PI * 2);
        // 第一象限
        if ((ptEnd.x > ptStart.x) && (ptEnd.y > ptStart.y))
        {
            angle = a;
        }
        // 第二象限
        if ((ptEnd.x < ptStart.x) && (ptEnd.y > ptStart.y))
        {
            angle = 180 - a;
        }
        // 第三象限
        if ((ptEnd.x < ptStart.x) && (ptEnd.y < ptStart.y))
        {
            angle = 180 + a;
        }
        // 第四象限
        if ((ptEnd.x > ptStart.x) && (ptEnd.y < ptStart.y))
        {
            angle = 360 - a;
        }

        return angle;
    }

}