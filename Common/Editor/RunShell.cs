
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEditor.Build.Reporting;

public class RunShell
{
    static string dirRoot;
    static string dirWork;

    [MenuItem("Menu/RunShell")]
    public static void Run()
    {
        dirRoot = FileUtil.GetLastDir(Application.dataPath);
        dirRoot = FileUtil.GetLastDir(dirRoot);//kidsgame

        dirWork = dirRoot + "/ProjectConfig/" + Common.appType + "/" + Common.appKeyName + "/cmd_win";
        //  dirRoot += "/ProjectConfig/" + Common.appType + "/" + Common.appKeyName + "/cmd_win";
        // 这里不开线程的话，就会阻塞住unity的主线程，当然如果你需要阻塞的效果的话可以不开
        Thread newThread = new Thread(new ThreadStart(RunShellThreadStart));
        newThread.Start();
    }
    private static void RunShellThreadStart()
    {

        string cmdTxt = @"echo test
notepad C:\Users\pc\Desktop\1.txt
explorer.exe D:\
pause";
        //  RunCommand(cmdTxt);
        // RunProcessCommand("notepad", @"C:\Users\pc\Desktop\1.txt");
        RunProcessCommand("explorer.exe", @"D:\");


        //  RunCmd.main.Run("cmd", "");
        // RunCmd.main.Run("cmd", "1", dir);//apk_build_all.bat

        // RunProcessCommand("cmd", "update_appname.bat", dirRoot);//update_appname
        string dirScript = dirRoot + "/ProjectConfig/script";
        //  RunProcessCommand("c:/Python27/python", dirScript + "/appname.py " + dirWork, dirWork);

    }
    private static void RunCommand(string command)
    {
        Process process = new Process();
        process.StartInfo.FileName = "powershell";
        process.StartInfo.Arguments = command;
        process.StartInfo.CreateNoWindow = false; // 获取或设置指示是否在新窗口中启动该进程的值（不想弹出powershell窗口看执行过程的话，就=true）
        process.StartInfo.ErrorDialog = true; // 该值指示不能启动进程时是否向用户显示错误对话框
        process.StartInfo.UseShellExecute = false;
        //process.StartInfo.RedirectStandardError = true;
        //process.StartInfo.RedirectStandardInput = true;
        //process.StartInfo.RedirectStandardOutput = true;
        process.Start();
        //process.StandardInput.WriteLine(@"explorer.exe D:\");
        //process.StandardInput.WriteLine("pause");
        process.WaitForExit();
        process.Close();
    }


    public Process CreateCmdProcess(string cmd, string args, string workdir = null)
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
    public static void RunProcessCommand(string command, string argument, string workdir = null)
    {
        ProcessStartInfo start = new ProcessStartInfo();
        start.FileName = command;
        start.Arguments = argument;
        start.CreateNoWindow = false;
        start.ErrorDialog = true;
        start.UseShellExecute = false;
        if (!string.IsNullOrEmpty(workdir))
            start.WorkingDirectory = workdir;
        Process p = Process.Start(start);
        p.WaitForExit();
        p.Close();
    }
}