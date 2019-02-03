using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QueueTextLog : MonoBehaviour
{
    Queue<string> Logs = new Queue<string>();
    public Text logText;
    public int limit = 20;

    public void AddLog(string log)
    {
        Logs.Enqueue(log);
        if (Logs.Count > limit)
            Logs.Dequeue();

        string text = "";
        foreach (var item in Logs)
            text += string.Format("{0}\n", item);

        logText.text = text;
    }
}
