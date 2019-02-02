using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProcessDebug : MonoBehaviour
{
    public Text DebugText;

    void Start()
    {

    }

    void Update()
    {
        var mgr = ProcessManager.Instance;

        bool isNull = mgr.Process == null;
        DebugText.text = string.Format("{0}: E: {1}\n", mgr.ProcessName, !isNull);
    }
}
