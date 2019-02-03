using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtility
{
    public static T CalcWeight<T>(IList<T> list, System.Func<T, int> getWeight, int total)
    {
        int rng = Random.Range(0, total);

        for (int i = 0; i < list.Count; i++)
        {
            T item = list[i];
            int w = getWeight.Invoke(item);

            if (rng < w)
                return item;

            rng -= w;
        }

        return list[0];
    }
}
