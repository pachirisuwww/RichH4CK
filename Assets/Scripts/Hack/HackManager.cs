using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackManager : MonoBehaviour
{
    public static HackManager Instance;

    public bool isRandomCPI;

    //public bool isMoveCPI;
    //public bool isCourseCPI;
    //public float moveCPIMul;
    //public float courseCPIMul;

    //internal float moveSum;
    //internal float courseSum;

    [System.Serializable]
    public class RandomCPI
    {
        public int CPI;
        public int weight;
    }
    public List<RandomCPI> RandomCPITable;
    int totalWeight;

    private void Awake()
    {
        Instance = this;

        foreach (var item in RandomCPITable)
            totalWeight += item.weight;
    }

    public void ChangeRandomCPI(bool val)
    {
        isRandomCPI = val;
    }

    public int GetRandomCPI()
    {
        int cpi = MathUtility.CalcWeight(RandomCPITable, (x) => x.weight, totalWeight).CPI;
        int sub = Random.Range(-cpi, cpi + 1);
        cpi = Mathf.Max(0, cpi + sub);
        return cpi;
    }

    //public void ChangeIsMoveCPI(bool val)
    //{
    //    isMoveCPI = val;
    //}

    //public void ChangeIsCourseCPI(bool val)
    //{
    //    isCourseCPI = val;
    //}

    //public void ChangeMoveCPIMul(string val)
    //{
    //    float ret;
    //    if (float.TryParse(val, out ret))
    //        moveCPIMul = ret;
    //}

    //public void ChangeCourseCPIMul(string val)
    //{
    //    float ret;
    //    if (float.TryParse(val, out ret))
    //        courseCPIMul = ret;
    //}
}
