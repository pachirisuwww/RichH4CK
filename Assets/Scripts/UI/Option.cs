using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option : MonoBehaviour
{
    public RectTransform rectTrans;

    bool isDisplay;

    private void Awake()
    {
        Display(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
            Display(!isDisplay);
    }

    internal void Display(bool isDisplay)
    {
        if (isDisplay)
            rectTrans.anchoredPosition = Vector2.zero;
        else
            rectTrans.anchoredPosition = GlobalValue.UnseenPos;

        this.isDisplay = isDisplay;
    }
}
