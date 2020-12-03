using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;
using UnityEngine;
using System.Diagnostics;
using System;

//https://blog.csdn.net/linxinfa/article/details/52982384
public class RunCmd
{
    static private RunCmd _main = null;
    public static RunCmd main
    {
        get
        {
            if (_main == null)
            {
                _main = new RunCmd();
            }
            return _main;
        }
    }
    public static bool ExecuteProgram(string exeFilename, string args, string workDir)
    {
        System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
        info.FileName = exeFilename;
        info.WorkingDirectory = workDir;
        info.UseShellExecute = true;
        info.Arguments = args;
        info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

        System.Diagnostics.Process task = null;
        bool rt = true;
        try
        {
            //Debug.Log("ExecuteProgram:" + args);

            task = System.Diagnostics.Process.Start(info);
            if (task != null)
            {
                task.WaitForExit(100000);
            }
            else
            {
                return false;
            }
        }
        catch (Exception e)
        {
            // Debug.LogError("ExecuteProgram:" + e.ToString());
            return false;
        }
        finally
        {
            if (task != null && task.HasExited)
            {
                rt = (task.ExitCode == 0);
            }
        }

        return rt;
    }
    public string FormatPath(string path)
    {
        path = path.Replace("/", "\\");
        if (Application.platform == RuntimePlatform.OSXEditor)
            path = path.Replace("\\", "/");

        return path;
    }

    public void Run(string cmd, string args, string workdir = null)
    {
        string[] res = new string[2];
        var p = CreateCmdProcess(cmd, args, workdir);
        res[0] = p.StandardOutput.ReadToEnd();
        res[1] = p.StandardError.ReadToEnd();
        p.Close();
    }

    public System.Diagnostics.Process CreateCmdProcess(string cmd, string args, string workdir = null)
    {
        var pStartInfo = new System.Diagnostics.ProcessStartInfo(cmd);
        pStartInfo.Arguments = args;
        pStartInfo.CreateNoWindow = false;
        pStartInfo.UseShellExecute = false;
        pStartInfo.RedirectStandardError = true;
        pStartInfo.RedirectStandardInput = true;
        pStartInfo.RedirectStandardOutput = true;
        pStartInfo.StandardErrorEncoding = System.Text.UTF8Encoding.UTF8;
        pStartInfo.StandardOutputEncoding = System.Text.UTF8Encoding.UTF8;
        if (!string.IsNullOrEmpty(workdir))
            pStartInfo.WorkingDirectory = workdir;
        return System.Diagnostics.Process.Start(pStartInfo);
    }

}
