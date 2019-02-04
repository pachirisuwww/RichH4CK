using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceExitButton : MonoBehaviour
{
    public void ForceExitGame()
    {
        var p = ProcessManager.Instance.Process;
        if (p != null && !p.HasExited)
            p.Kill();
    }
}
