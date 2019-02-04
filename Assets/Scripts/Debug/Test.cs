using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MyMemory;
using System.Net;
using System;
using System.Net.NetworkInformation;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            Debug.Log(HackManager.Instance.GetRandomCPI());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
