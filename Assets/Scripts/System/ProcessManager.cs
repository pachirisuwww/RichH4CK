using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProcessManager : MonoBehaviour
{
    internal static ProcessManager Instance;
    
    public string ProcessName;
    internal System.Diagnostics.Process Process;

    private void Awake()
    {
        Instance = this;
        UpdateState();
    }

    private void Update()
    {
        UpdateState();
    }

    void UpdateState()
    {
        //已離開則清空
        if (Process != null && Process.HasExited)
            Process = null;

        //確認關聯
        if (Process == null)
            Process = ProcessUtility.GetProcess(ProcessName);
        else
            Process = System.Diagnostics.Process.GetProcessById(Process.Id);

        bool isNull = Process == null;

        if (!isNull && !Process.HasExited && Input.GetKeyDown(KeyCode.K))
            Process.Kill();
    }
}
