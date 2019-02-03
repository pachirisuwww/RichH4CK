using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionPanelSelection : MonoBehaviour
{
    internal static RectTransform current;
    public RectTransform Panel;

    void Awake()
    {
        Panel.gameObject.SetActive(false);
    }
    
    public void Change()
    {
        if (current == Panel)
            return;

        if (current != null)
            current.gameObject.SetActive(false);

        current = Panel;
        Panel.gameObject.SetActive(true);
    }
}
