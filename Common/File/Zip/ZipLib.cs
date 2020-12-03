using UnityEngine;
using System;
using System.Text;
using System.Threading;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public interface IZipLibDelegate
{
    void OnZipLibDidFinish(ZipLib z);

}

// 安卓解压中文名字文件 乱码
public class ZipLib
{
    public IZipLibDelegate iDelegate;

    // 文件数量
    int[] progress = new int[1];

    // 文件大小
    ulong[] progress2 = new ulong[1];
    private string zipFilePath;
    private string zipOutPut;
    int totalFile;
    Thread threadZip;
    static private ZipLib _main = null;
    public static ZipLib main
    {
        get
        {
            if (_main == null)
            {
                _main = new ZipLib();
            }
            return _main;
        }
    }
    /// 解压
    /// </summary>
    /// <param name="zipPath">压缩文件路径</param>
    /// <param name="outPath">解压出去路径</param>
    public void UnZipFile(string zipPath, string outPath)
    {
        zipFilePath = zipPath;
        zipOutPut = outPath;
        totalFile = lzip.getTotalFiles(zipPath);
        // Thread th = new Thread(decompressFunc2);
        // th.Start();
        threadZip = new Thread(DecompressionThread);
        threadZip.Start();
    }

    // 解压进度 0-100
    public int GetPercent()
    {
        int percent = 0;
        Debug.Log("ZipLib progress=" + progress[0].ToString() + " progress2=" + progress2[0].ToString() + " totalFile=" + totalFile);
        int cur = (int)progress[0];
        if (totalFile != 0)
        {
            percent = cur * 100 / totalFile;
        }
        return percent;
    }

    void DecompressionThread()
    {
        // Windows  only (see lzip.cs for more info)
#if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
        lzip.setEncoding(1);//CP_UTF8 = 65001  // CP_OEMCP/UNICODE = 1
#else
        // lzip.setEncoding(65001);//CP_UTF8
#endif  
        int zres = lzip.decompress_File(zipFilePath, zipOutPut, progress, null, progress2);
        Debug.Log("decompress: " + zres.ToString());
        if (iDelegate != null)
        {
            iDelegate.OnZipLibDidFinish(this);
        }
    }

    void Stop()
    {
    }

}

