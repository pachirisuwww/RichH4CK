using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemAutoDestroy : MonoBehaviour
{
    public EventSystem evt;
    
    void Update()
    {
        if (EventSystem.current != evt)
            Destroy(gameObject);
    }
}
