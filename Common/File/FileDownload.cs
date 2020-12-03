using System;
using System.Collections.Generic;

using UnityEngine;
using BestHTTP;
public interface IFileDownloadDelegate
{
    void OnFileDownloadProgress(FileDownload d);
    void OnFileDownloadDidFinish(FileDownload d);
    void OnFileDownloadDidFail(FileDownload d, string error);
}


public class FileDownload
{
    public IFileDownloadDelegate iDelegate;
    string saveFilePath;
    public float downloadSpeed;//B/s
    public int downloadPercent;
    public int downloadByte;
    public int lengthByte;
    long tickDownloadStart;

    #region Private Fields

    /// <summary>
    /// Cached request to be able to abort it
    /// </summary>
    HTTPRequest request;

    /// <summary>
    /// Debug status of the request
    /// </summary>
    string status = string.Empty;

    /// <summary>
    /// Download(processing) progress. Its range is between [0..1]
    /// </summary>
    float progress;

    /// <summary>
    /// The fragment size that we will set to the request
    /// </summary>
    int fragmentSize = HTTPResponse.MinBufferSize;

    #endregion

    #region Unity Events

    void Init()
    {
        // We are done, delete the progress key
        PlayerPrefs.DeleteKey("DownloadProgress");
        PlayerPrefs.DeleteKey("DownloadLength");
        PlayerPrefs.Save();

        // If we have a non-finished download, set the progress to the value where we left it
        if (PlayerPrefs.HasKey("DownloadLength"))
        {
            //关闭断点下载
            PlayerPrefs.SetInt("DownloadProgress", 0);
            progress = PlayerPrefs.GetInt("DownloadProgress") / (float)PlayerPrefs.GetInt("DownloadLength");
        }

        downloadSpeed = 0;
    }

    void Destroy()
    {
        // Stop the download if we are leaving this example
        if (request != null && request.State < HTTPRequestStates.Finished)
        {
            request.OnProgress = null;
            request.Callback = null;
            request.Abort();
        }
    }


    #endregion

    #region Private Helper Functions

    // Calling this function again when the "DownloadProgress" key in the PlayerPrefs present will
    //	continue the download
    public void Download(string url, string filePath)
    {
        saveFilePath = filePath;
        Destroy();
        Init();
        request = new HTTPRequest(new Uri(url), (req, resp) =>
        {
            switch (req.State)
            {
                // The request is currently processed. With UseStreaming == true, we can get the streamed fragments here
                case HTTPRequestStates.Processing:

                    // Set the DownloadLength, so we can display the progress
                    if (!PlayerPrefs.HasKey("DownloadLength"))
                    {
                        string value = resp.GetFirstHeaderValue("content-length");
                        if (!string.IsNullOrEmpty(value))
                        {
                            // lengthByte = int.Parse(value);
                            PlayerPrefs.SetInt("DownloadLength", int.Parse(value));
                            tickDownloadStart = Common.GetCurrentTimeMs();
                        }

                    }

                    // Get the fragments, and save them
                    ProcessFragments(resp.GetStreamedFragments());

                    status = "Processing";
                    break;

                // The request finished without any problem.
                case HTTPRequestStates.Finished:
                    if (resp.IsSuccess)
                    {
                        // Save any remaining fragments
                        ProcessFragments(resp.GetStreamedFragments());

                        // Completely finished
                        if (resp.IsStreamingFinished)
                        {
                            status = "Streaming finished!";

                            // We are done, delete the progress key
                            PlayerPrefs.DeleteKey("DownloadProgress");
                            PlayerPrefs.Save();
                            if (iDelegate != null)
                            {
                                iDelegate.OnFileDownloadDidFinish(this);
                            }
                            request = null;
                        }
                        else
                            status = "Processing";

                    }
                    else
                    {
                        status = string.Format("Request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}",
                                                        resp.StatusCode,
                                                        resp.Message,
                                                        resp.DataAsText);
                        Debug.LogWarning(status);

                        request = null;
                    }
                    break;

                // The request finished with an unexpected error. The request's Exception property may contain more info about the error.
                case HTTPRequestStates.Error:
                    status = "Request Finished with Error! " + (req.Exception != null ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception");
                    Debug.LogError(status);
                    if (iDelegate != null)
                    {
                        iDelegate.OnFileDownloadDidFail(this, status);
                    }
                    request = null;
                    break;

                // The request aborted, initiated by the user.
                case HTTPRequestStates.Aborted:
                    status = "Request Aborted By User!";
                    Debug.LogWarning(status);

                    request = null;
                    break;

                // Connecting to the server is timed out.
                case HTTPRequestStates.ConnectionTimedOut:
                    status = "Connection Timed Out!";
                    Debug.LogError(status);
                    if (iDelegate != null)
                    {
                        iDelegate.OnFileDownloadDidFail(this, status);
                    }
                    request = null;
                    break;

                // The request didn't finished in the given time.
                case HTTPRequestStates.TimedOut:
                    status = "Processing the request Timed Out!";
                    Debug.LogError(status);
                    if (iDelegate != null)
                    {
                        iDelegate.OnFileDownloadDidFail(this, status);
                    }
                    request = null;
                    break;
            }
        });

        // Are there any progress, that we can continue?
        if (PlayerPrefs.HasKey("DownloadProgress"))
            // Set the range header
            request.SetRangeHeader(PlayerPrefs.GetInt("DownloadProgress"));
        else
            // This is a new request
            PlayerPrefs.SetInt("DownloadProgress", 0);

#if !BESTHTTP_DISABLE_CACHING && (!UNITY_WEBGL || UNITY_EDITOR)
        // If we are writing our own file set it true(disable), so don't duplicate it on the file-system
        request.DisableCache = true;
#endif

        // We want to access the downloaded bytes while we are still downloading
        request.UseStreaming = true;

        // Set a reasonable high fragment size. Here it is 5 megabytes.
        request.StreamFragmentSize = fragmentSize;

        // Start Processing the request
        request.Send();
    }

    /// <summary>
    /// In this function we can do whatever we want with the downloaded bytes. In this sample we will do nothing, just set the metadata to display progress.
    /// </summary>
    void ProcessFragments(List<byte[]> fragments)
    {
        if (fragments != null && fragments.Count > 0)
        {
            string dir = "TODO!";
            string filename = "TODO!";
            // System.IO.Path.Combine(dir, filename)
            using (System.IO.FileStream fs = new System.IO.FileStream(saveFilePath, System.IO.FileMode.Append))
                for (int i = 0; i < fragments.Count; ++i)
                    fs.Write(fragments[i], 0, fragments[i].Length);

            for (int i = 0; i < fragments.Count; ++i)
            {
                // Save how many bytes we wrote successfully
                int downloaded = PlayerPrefs.GetInt("DownloadProgress") + fragments[i].Length;
                PlayerPrefs.SetInt("DownloadProgress", downloaded);
            }

            PlayerPrefs.Save();

            // Set the progress to the actually processed bytes
            progress = PlayerPrefs.GetInt("DownloadProgress") / (float)PlayerPrefs.GetInt("DownloadLength");
            if (iDelegate != null)
            {
                long ticktime = Common.GetCurrentTimeMs() - tickDownloadStart;
                downloadByte = PlayerPrefs.GetInt("DownloadProgress");
                lengthByte = PlayerPrefs.GetInt("DownloadLength");
                Debug.Log("ticktime = " + ticktime / 1000.0f + " tickDownloadStart=" + tickDownloadStart);
                downloadSpeed = downloadByte / (ticktime / 1000.0f);
                // downloadSpeed = downloadSpeed/1024.0f;
                downloadPercent = (int)(progress * 100);
                iDelegate.OnFileDownloadProgress(this);
            }
        }
    }

    #endregion
}
