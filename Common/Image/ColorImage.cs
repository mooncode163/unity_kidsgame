using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 扫描线算法介绍：
 //http://blog.csdn.net/orbit/article/details/7343236
 
 //http://blog.csdn.net/orbit/article/details/7368996
 */

public class ColorImage
{

    public Rect fillRect
    {
        get
        {
            float x, y, w, h;
            x = fillXLeft;
            y = fillYBottom;
            w = fillXRight - fillXLeft + 1;
            h = fillYTop - fillYBottom + 1;
            return new Rect(x, y, w, h);
        }
    }


    int fillXLeft;
    int fillXRight;
    int fillYBottom;
    int fillYTop;
    bool isBit24;
    private Texture2D texImage;

    private byte[] pixselImage;//rgba 数据
    private byte[] pixselImageOrigin;//备份原始数据
    float alphaMax = 0.5f;

    public void InitOrigin(Texture2D tex)
    {
        pixselImageOrigin = tex.GetRawTextureData();
    }

    public void Init(Texture2D tex)
    {
        texImage = tex;
        pixselImage = texImage.GetRawTextureData();
        int size = tex.width * tex.height * 4;
        Debug.Log("Init: w=" + tex.width + " h=" + tex.height + " size=" + size + " len=" + pixselImage.Length);
        size = pixselImage.Length;
        pixselImageOrigin = new byte[size];
        System.Array.Copy(pixselImage, pixselImageOrigin, size);

        isBit24 = false;
        if (tex.format == TextureFormat.RGB24)
        {
            isBit24 = true;

        }
        else
        {

        }
        Debug.Log("Init: isBit24 =" + isBit24);

    }

    //恢复成原始数据
    public void RestoreOrigin()
    {
        int size = pixselImage.Length;
        Debug.Log("RestoreOrigin:size=" + size);
        System.Array.Copy(pixselImageOrigin, pixselImage, size);
    }

    public void UpdateTexture()
    {
        texImage.LoadRawTextureData(pixselImage);
        texImage.Apply(false);
    }
    public void ApplyTexture()
    {
        texImage.Apply();
    }

    //pt 相对左下脚的图片坐标
    int GetIndexOfImage(Vector2 pt)
    {
        int idx = 0;
        int w = texImage.width;
        int h = texImage.height;
        idx = ((int)pt.y * w) + (int)pt.x;//跳到指定的像素点。

        int byte_num = 4;//rgba
        if (isBit24)
        {
            byte_num = 3;
        }
        idx = idx * byte_num;
        return idx;
    }

    //pt 相对左下脚的图片坐标
    public Color GetImageColor(Vector2 pt)
    {
        if (isBit24)
        {
            return GetImageColorRGB24(pt);
        }
        Color color = Color.black;
        int idx = GetIndexOfImage(pt);

        //下面的打印会导致变得特别卡，需要关闭
        //Debug.Log("idx=" + idx + " size=" + pixselImage.Length);
        
        //argb
        color.a = pixselImage[idx] / 255f;
        if (color.a >= alphaMax && color.a < 1)
        {
            //将图片中透明度强制设置成1
            color.a = 1f;
        }
        color.r = pixselImage[idx + 1] / 255f;
        color.g = pixselImage[idx + 2] / 255f;
        color.b = pixselImage[idx + 3] / 255f;
        return color;

    }

    public Color GetImageColorOrigin(Vector2 pt)
    {
        if (isBit24)
        {
            return GetImageColorRGB24(pt);
        }
        Color color = Color.black;
        int idx = GetIndexOfImage(pt);
        //argb
        color.a = pixselImage[idx] / 255f;

        color.r = pixselImage[idx + 1] / 255f;
        color.g = pixselImage[idx + 2] / 255f;
        color.b = pixselImage[idx + 3] / 255f;
        return color;

    }

    //pt 相对左下脚的图片坐标
    public void SetImageColor(Vector2 pt, Color color)
    {
        if (isBit24)
        {
            SetImageColorRGB24(pt, color);
        }
        int idx = GetIndexOfImage(pt);
        //argb
        pixselImage[idx] = (byte)(color.a * 255);
        pixselImage[idx + 1] = (byte)(color.r * 255);
        pixselImage[idx + 2] = (byte)(color.g * 255);
        pixselImage[idx + 3] = (byte)(color.b * 255);

    }



    int GetIndexOfImageRGB24(Vector2 pt)
    {
        int idx = 0;
        int w = texImage.width;
        int h = texImage.height;
        idx = ((int)pt.y * w) + (int)pt.x;//跳到指定的像素点。
        idx = idx * 3;//rgba
        return idx;
    }

    //pt 相对左下脚的图片坐标
    public Color GetImageColorRGB24(Vector2 pt)
    {
        Color color = Color.black;
        int idx = GetIndexOfImageRGB24(pt);
        //argb
        color.a = 1f;

        color.r = pixselImage[idx + 0] / 255f;
        color.g = pixselImage[idx + 1] / 255f;
        color.b = pixselImage[idx + 2] / 255f;
        return color;

    }


    //pt 相对左下脚的图片坐标
    public void SetImageColorRGB24(Vector2 pt, Color color)
    {
        int idx = GetIndexOfImageRGB24(pt);
        //argb

        pixselImage[idx + 0] = (byte)(color.r * 255);
        pixselImage[idx + 1] = (byte)(color.g * 255);
        pixselImage[idx + 2] = (byte)(color.b * 255);

    }

    /**
     扫描线算法介绍：
     //http://blog.csdn.net/orbit/article/details/7343236
     
     //http://blog.csdn.net/orbit/article/details/7368996
     
     * 使用指定颜色填充多边形
     * @param shape 包含多边形的图片
     * @param sx    多边形区域内的任意一点X坐标
     * @param sy    多边形区域内的任意一点Y坐标
     * @param borderColor   多边形的边界颜色
     * @param fillColor     希望填充的颜色
     */
    public void RunFillColor(Vector2 pt, Color fillColor, Color borderColor)
    {

        // _center_x = sx;
        // _center_y = sy;
        // _borderColor = borderColor;
        // cleanPoint();
        fillXLeft = texImage.width;
        fillXRight = 0;
        fillYBottom = texImage.height;
        fillYTop = 0;

        int sx = (int)pt.x;
        int sy = (int)pt.y;
        //扫描内部点
        //横扫描
        ScanLineSeedFill(sx, sy, fillColor, borderColor, true);
        //竖扫描
        ScanLineSeedFill(sx, sy, fillColor, borderColor, false);//error

    }

    void ScanLineSeedFill(int x, int y, Color new_color, Color boundary_color, bool is_scan_by_x)
    {
        // std::stack<Point> stk;
        List<Vector2> stk = new List<Vector2>();
        stk.Add(new Vector2(x, y)); //第1步，种子点入站
        while (stk.Count != 0)
        {
            Vector2 seed = stk[0]; //第2步，取当前种子点
            stk.RemoveAt(0);

            if (IsPointOutOfRange((int)seed.x, (int)seed.y))
            {
                break;
            }


            //第3步，向左右填充
            if (is_scan_by_x)
            {

                int count = FillLineRight((int)seed.x, (int)seed.y, new_color, boundary_color);//向'cf?右'd3?填'cc?充'b3?
 
                int xRight = (int)seed.x + count - 1;
                count = FillLineLeft((int)seed.x - 1, (int)seed.y, new_color, boundary_color);//向'cf?左'd7?填'cc?充'b3?
                int xLeft = (int)seed.x - count;
                 
                //第4步，处理相邻两条扫描线
                if ((seed.y - 1) >= 0)
                {
                    SearchLineNewSeed(stk, xLeft, xRight, (int)seed.y - 1, new_color, boundary_color, is_scan_by_x);
                }
                if ((seed.y + 1) < texImage.height)
                {
                    SearchLineNewSeed(stk, xLeft, xRight, (int)seed.y + 1, new_color, boundary_color, is_scan_by_x);
                }
            }
            else
            {


                int count = FillLineTopBottom((int)seed.x, (int)seed.y, new_color, boundary_color, true);
                int yRight = (int)seed.y + count - 1;
                count = FillLineTopBottom((int)seed.x, (int)seed.y - 1, new_color, boundary_color, false);
                int yLeft = (int)seed.y - count;

                if ((seed.x - 1) >= 0)
                {
                    SearchLineNewSeed(stk, yLeft, yRight, (int)seed.x - 1, new_color, boundary_color, is_scan_by_x);
                }
                if ((seed.x + 1) < texImage.width)
                {
                    SearchLineNewSeed(stk, yLeft, yRight, (int)seed.x + 1, new_color, boundary_color, is_scan_by_x);
                }

            }



        }
    }

    bool IsPointOutOfRange(int x, int y)
    {
        bool ret = false;
        if ((x < 0) || (x >= texImage.width) || (y < 0) || (y >= texImage.height))
        {
            ret = true;
        }
        return ret;
    }

    //非边界且未填充的像素点
    bool IsPixelValid(int x, int y, Color new_color, Color boundary_color)
    {
        bool ret = false;
        // if(imageOrigin){
        //     //非边界且原始图片区域为透明
        //     Color colorOrigin = getImageColor4B(imageOrigin, Point(x,y));
        //     Color color = getImageColor4B(image, Point(x,y));
        //     //if ((color!=new_color)&&(colorOrigin.a==0))
        //     if ((color!=new_color)&&(colorOrigin.a<alphaMax))
        //     {
        //         ret = true;
        //     }
        // }else

        {
            Color color = GetImageColor(new Vector2(x, y));

            //忽略透明度
            // color.a = boundary_color.a;

            if ((color != new_color) && (color != boundary_color))
            {
                ret = true;
            }
        }
        return ret;
    }


    bool FinNextPoint(Vector2 pt, Color new_color, Color boundary_color)
    {
        bool ret = false;
        // if(imageOrigin){
        //     //非边界且原始图片区域为透明
        //     Color4B colorOrigin = getImageColor4B(imageOrigin, pt);
        //     //if (colorOrigin.a==0)
        //     if (colorOrigin.a<alphaMax)
        //     {
        //         ret = true;
        //     }
        // }else

        {
            Color color = GetImageColor(pt);
            //忽略透明度
            // color.a = boundary_color.a;

            if (color != boundary_color)
            {
                ret = true;
            }
        }
        return ret;
    }


    int FillLineTopBottom(int x, int y, Color new_color, Color boundary_color, bool isTop)
    {
        int count = 0;
        int yt = y;
        if (IsPointOutOfRange(x, y))
        {
            return count;
        }

        while (FinNextPoint(new Vector2(x, yt), new_color, boundary_color))
        {
            // setImageColor4B(image,Point(x, yt), new_color);
            SetImageColor(new Vector2(x, yt), new_color);
            UpdateFillRect(x, yt);
            if (isTop)
            {
                yt++;
            }
            else
            {
                yt--;
            }
            count++;

            if (IsPointOutOfRange(x, yt))
            {
                break;
            }
        }
        if (count == 0)
        {
            yt = y;
        }

        //count++;
        return count;
    }


    int FillLineLeft(int x, int y, Color new_color, Color boundary_color)
    {
        int count = 0;
        int xt = x;
        if (IsPointOutOfRange(x, y))
        {
            return count;
        }

        //while(getImageColor4B(image,Point(xt, y)) != boundary_color)
        while (FinNextPoint(new Vector2(xt, y), new_color, boundary_color))
        {
            SetImageColor(new Vector2(xt, y), new_color);
            UpdateFillRect(xt, y);
            xt--;
            count++;

            if (IsPointOutOfRange(xt, y))
            {
                break;
            }
        }
        if (count == 0)
        {
            xt = x;
        }


        return count;
    }


    int FillLineRight(int x, int y, Color new_color, Color boundary_color)
    {
        int count = 0;
        int xt = x;

        if (IsPointOutOfRange(x, y))
        {
            return count;
        }

        //Color4B colorNow = getImageColor4B(image,Point(xt, y));
        //while(getImageColor4B(image,Point(xt, y)) != boundary_color)
        while (FinNextPoint(new Vector2(xt, y), new_color, boundary_color))
        {

            SetImageColor(new Vector2(xt, y), new_color);
            UpdateFillRect(xt, y);
            xt++;

            count++;

            if (IsPointOutOfRange(xt, y))
            {
                break;
            }
            // break;
        }

        if (count == 0)
        {
            xt = x;
        }


        return count;
    }

    void UpdateFillRect(int x, int y)
    {
        if (x < fillXLeft)
        {
            fillXLeft = x;
        }
        if (x > fillXRight)
        {
            fillXRight = x;
        }


        if (y < fillYBottom)
        {
            fillYBottom = y;
        }
        if (y > fillYTop)
        {
            fillYTop = y;
        }
    }


    int SkipInvalidInLine(int x, int y, int right, Color new_color, Color boundary_color)
    {

        return 0;

    }
    bool IsPointInSeedList(List<Vector2> stk, Vector2 pt)
    {
        //  return true;
        bool ret = false;
        foreach (Vector2 ptlist in stk)
        {
            if (pt == ptlist)
            {
                ret = true;
                break;
            }

        }
        return ret;
    }

    //FillLineRight()和FillLineLeft()两个函数就是从种子点分别向右和向左填充颜色，直到遇到边界点，同时返回填充的点的个数。这两个函数返回填充点的个数是为了正确调整当前种子点所在的扫描线的区间[xLeft, xRight]。SearchLineNewSeed()函数完成算法第4步所描述的操作，就是在新扫描线上寻找种子点，并将种子点入栈，新扫描线的区间是xLeft和xRight参数确定的：

    void SearchLineNewSeed(List<Vector2> stk, int xLeft, int xRight,
                                       int y, Color new_color, Color boundary_color, bool is_scan_by_x)
   {

        if (is_scan_by_x)
        {
            int xt = xLeft;
            bool findNewSeed = false;

            while (xt <= xRight)
            {
                findNewSeed = false;
                while (IsPixelValid(xt, y, new_color, boundary_color) && (xt < xRight))
                {
                    findNewSeed = true;
                    xt++;
                }
                if (findNewSeed)
                {
                    Vector2 pt = Vector2.zero;
                    if (IsPixelValid(xt, y, new_color, boundary_color) && (xt == xRight))
                    {
                        pt = new Vector2(xt, y);
                    }
                    else
                    {
                        //stk.Add(new Vector2(xt - 1, y));
                        pt = new Vector2(xt - 1, y);
                    }

                    if (!IsPointInSeedList(stk, pt))
                    {
                        stk.Add(pt);
                    }

                }

                /*向右跳过内部的无效点（处理区间右端有障碍点的情况）*/

                int xspan = SkipInvalidInLine(xt, y, xRight, new_color, boundary_color);
                xt += (xspan == 0) ? 1 : xspan;

                /*处理特殊情况,以退出while(x<=xright)循环*/
            }


        }
        else

        {
            int xt = xLeft;
            bool findNewSeed = false;

            while (xt <= xRight)
            {
                findNewSeed = false;
                while (IsPixelValid(y, xt, new_color, boundary_color) && (xt < xRight))
                {
                    findNewSeed = true;
                    xt++;
                }
                if (findNewSeed)
                {
                    Vector2 pt = Vector2.zero;
                    if (IsPixelValid(y, xt, new_color, boundary_color) && (xt == xRight))
                        //stk.Add(new Vector2(y, xt));
                        pt = new Vector2(y, xt);
                    else
                        //stk.Add(new Vector2(y, xt - 1));
                        pt = new Vector2(y, xt - 1);

                    if (!IsPointInSeedList(stk, pt))
                    {
                        stk.Add(pt);
                    }
                }

                /*向右跳过内部的无效点（处理区间右端有障碍点的情况）*/

                int xspan = SkipInvalidInLine(y, xt, xRight, new_color, boundary_color);
                xt += (xspan == 0) ? 1 : xspan;

                /*处理特殊情况,以退出while(x<=xright)循环*/
            }


        }


    }



}
