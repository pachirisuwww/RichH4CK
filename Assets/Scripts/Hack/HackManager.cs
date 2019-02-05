using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackManager : MonoBehaviour
{
    public static HackManager Instance;
    public QueueTextLog Logs;

    public bool isRandomCPI;
    public bool isDecember;
    public bool isDecreaseLoanDate;

    public bool blockChangeCPI;

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
        Logs.AddLog(string.Format("Set RandomCPI: {0}", isRandomCPI));
    }

    public void ChangeDecember(bool val)
    {
        isDecember = val;
        Logs.AddLog(string.Format("Set December: {0}", isDecember));
    }

    public void ChangeDecreaseLoanDate(bool val)
    {
        isDecreaseLoanDate = val;
        Logs.AddLog(string.Format("Set DecreaseLoanDate: {0}", isDecreaseLoanDate));
    }


    public int GetRandomCPI()
    {
        int cpi = MathUtility.CalcWeight(RandomCPITable, (x) => x.weight, totalWeight).CPI;
        int sub = Random.Range(-cpi, cpi + 1);
        cpi = Mathf.Max(0, cpi + sub);
        return cpi;
    }

    public void GetNewLoanDate(ref System.DateTime newDate)
    {
        int add = Instance.isDecember ? 365 : 30;
        newDate = newDate.AddDays(-90 + add * (Instance.isDecreaseLoanDate ? 1 : 3));
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
