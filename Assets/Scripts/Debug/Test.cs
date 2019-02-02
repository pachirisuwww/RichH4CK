using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MyMemory;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MemoryData a = MemoryData.New();
        MemoryData b = MemoryData.New();

        Debug.Log(a.Comparer(b));

        a.cur = 255;
        b.cur = 234;

        Debug.Log(a.Comparer(b));

        b.cur = 255;

        Debug.Log(a.Comparer(b));

        a.cash[0] = 1000;
        b.cash[0] = 1000;

        Debug.Log(a.Comparer(b));

        b.cash[0] = 500;

        Debug.Log(a.Comparer(b));

        Debug.Log(a.cash[0]);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
