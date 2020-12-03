using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UICloudResController : UIView, IFileDownloadDelegate, IZipLibDelegate
{
    public const string RES_URL = "http://47.242.56.146/CloudRes/unity/kidsgame/LearnWord/LearnWord/GameRes.zip";
    public UIProgress uiProgress;
    public UIText textTitle;
    public UIText textVersionLocal;
    public UIText textVersionWeb;
    public UIButton btnAgain;
    string filePathZip;

    bool isStartZip = false;

    /// <summary>
    /// Unity's Awake method.
    /// </summary>
    public void Awake()
    {
        base.Awake();
        LoadPrefab();
        uiProgress.percent = 0;
        uiProgress.type = UIProgress.Type.NO_SLIDER;
        // filePathZip = Application.dataPath + "/GameRes.zip";
        filePathZip = CloudRes.main.rootPathGameRes + ".zip";

        Debug.Log("CheckVersion UICloudResController CloudRes resVersionWeb=" + CloudRes.main.resVersionWeb + " resVersionLocal=" + CloudRes.main.resVersionLocal);
        textVersionLocal.text = Language.main.GetString("CLOUD_RES_VERSION_NOW") + ":" + CloudRes.main.resVersionLocal;
        textVersionWeb.text = Language.main.GetString("CLOUD_RES_VERSION_UPDATE") + ":" + CloudRes.main.resVersionWeb;



    }

    /// <summary>
    /// Unity's Start method.
    /// </summary>
    public void Start()
    {
        base.Start();

        LayOut();
        OnUIDidFinish();
        Common.UnityStartUpFinish();
        DeleteUnDownloadFile();
        StartDownload();
        // StartCoroutine(UnZip());
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (isStartZip)
        {
            int percent = ZipLib.main.GetPercent();
            textTitle.text = Language.main.GetString("CLOUD_RES_ZIPFILE") + "(" + percent.ToString() + "%" + ")";
            Debug.Log("CloudRes UnZip  GetPercent =" + percent);
            if (percent >= 100)
            {



            }
        }
    }
    void LoadPrefab()
    {

    }
    public override void LayOut()
    {
        base.LayOut();
    }

    void GotoMain()
    {
        
        DeleteUnDownloadFile();
        AppSceneBase.main.SetRootViewController(MainViewController.main);
    }
    public void StartDownload()
    {
        FileDownload fd = new FileDownload();
        fd.iDelegate = this;
        // string url = RES_URL; 
        fd.Download(Config.main.urlGameRes, filePathZip);

    }

    public void DeleteUnDownloadFile()
    {
        return;

        if (File.Exists(filePathZip))
        {
            File.Delete(filePathZip);
        }
    }

    IEnumerator UnZip()
    {
        yield return null;
        string outPath = FileUtil.GetFileDir(filePathZip);
        Debug.Log("CloudRes UnZip outPath =" + outPath);
        FileUtil.DeleteDir(outPath + "/GameRes");
        isStartZip = true;
        ZipLib.main.iDelegate = this;
        ZipLib.main.UnZipFile(filePathZip, outPath);


    }
    public void OnFileDownloadProgress(FileDownload d)
    {
        uiProgress.percent = d.downloadPercent;
        float speed = d.downloadSpeed / 1024;//KB/s
        string strUnit = "KB/s";
        if (speed > 1024)
        {
            speed = speed / 1024;//MB/s
            strUnit = "MB/s";
        }
        if (speed > 1024)
        {
            speed = speed / 1024;//GB/s
            strUnit = "GB/s";
        }


        string strUnitSize = "K";
        float filesize = d.lengthByte / 1024;//K
        if (filesize > 1024)
        {
            filesize = filesize / 1024;//M
            strUnitSize = "M";
        }
        if (filesize > 1024)
        {
            filesize = filesize / 1024;//G
            strUnitSize = "G";
        }

        textTitle.text = d.downloadPercent.ToString() + "%" + " " + speed.ToString("0.00") + strUnit + " " + filesize.ToString("0.0") + strUnitSize;
        // Debug.Log(textTitle.text);

        LayOut();
    }

    public void OnFileDownloadDidFinish(FileDownload d)
    {
        btnAgain.gameObject.SetActive(false);
        StartCoroutine(UnZip());
    }
    public void OnZipLibDidFinish(ZipLib z)
    {
        Loom.QueueOnMainThread(() =>
            {
                Debug.Log("CloudRes UnZip OnZipLibDidFinish GetPercent =" + ZipLib.main.GetPercent());
                textTitle.text = Language.main.GetString("CLOUD_RES_ZIPFILE_FINISH");
                isStartZip = false;
                Invoke("GotoMain", 0.1f);
            });
    }

    public void OnFileDownloadDidFail(FileDownload d, string error)
    {
        textTitle.text = error;
    }
    public void OnClickBtnAgain()
    {
        DeleteUnDownloadFile();
        StartDownload();
    }

}
