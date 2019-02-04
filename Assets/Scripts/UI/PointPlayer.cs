using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointPlayer : MonoBehaviour
{
    public RectTransform rectTrans;
    public CanvasGroup canvasGroup;

    public InfoManager mgr;
    public InfoController Controller;

    int index = -1;
    internal bool isInvalidIndex { get { return index < 0 || index >= Controller.displayNum; } }

    public Vector2 invalidPos;
    public Vector2 offset;
    public Vector3 velocity;

    void Update()
    {
        if (isInvalidIndex)
        {
            canvasGroup.alpha = 0;
            velocity = Vector2.zero;
            rectTrans.anchoredPosition = GetPos();
        }
        else
        {
            canvasGroup.alpha = mgr.Infos[index].CanvasGroup.alpha;
            rectTrans.anchoredPosition = Vector3.SmoothDamp(rectTrans.anchoredPosition, GetPos(), ref velocity, 0.1f);
        }
    }

    Vector2 GetPos()
    {
        if (isInvalidIndex)
            return invalidPos;
        else
            return mgr.Infos[index].Rect.anchoredPosition + offset;
    }

    internal void SetIndex(int index)
    {
        this.index = index;
    }
}
