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

    int progressiveCount;
    int lastCPI;
    int sign;
    int realCPI;

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

    public void ResetProgressiveCPI()
    {
        progressiveCount = 0;
        lastCPI = 1;
    }

    internal void RefreshRealCPI(MyMemory.MemoryData data)
    {
        long allMoney = 0;
        int life = 0;
        for (int i = 0; i < 4; i++)
        {
            allMoney += data.cash[i];
            allMoney += data.bank[i];
            if (data.life[i] == 1)
                life++;
        }
        realCPI = (int)(allMoney / (MyMemory.MemoryTracker.ReadInitMoney() * life));
    }

    public int CalcBalanceCPI()
    {
        int chance = Random.Range(0, 3);

        int cpi;
        if (chance == 0 || lastCPI < 10)
            cpi = GetRandomCPI();
        else
            cpi = Mathf.Max(1, (int)Random.Range(realCPI * 0.9f, realCPI * 1.2f));

        Logs.AddLog(string.Format("Rng: {0}, CPI: {1}, RCPI: {2}", chance, cpi, realCPI));
        lastCPI = cpi;
        return cpi;
    }

    public void SetPorgressiveCPI()
    {
        if (lastCPI >= 20000)
            sign = -1;
        if (progressiveCount <= 0)
        {
            progressiveCount = Random.Range(3, 6);
            if (lastCPI < 10 || lastCPI <= realCPI * 1.1f)
                sign = 1;
            else
            {
                float signRnd = Random.Range(0, 2);
                sign = signRnd >= 0.5f ? 1 : -1;
            }
        }
    }

    internal int GetProgressiveCPI()
    {
        int ret = 1;

        int phase = Random.Range(0, 100);
        if (phase < 3)
            ret = 0;
        else if (phase < 6)
            ret = Random.Range(1, 11);
        else
        {
            progressiveCount--;

            int rndBase = sign == 1 ? lastCPI : lastCPI / 2;
            int rnd = Random.Range(1, rndBase + 1);
            
            int min = Mathf.Max(1, realCPI);

            lastCPI = Mathf.Clamp(lastCPI + rnd * sign, min, 20000);

            ret = lastCPI;
        }

        return ret;
    }

    public void GetNewLoanDate(ref System.DateTime newDate)
    {
        int add = Instance.isDecember ? 365 : 30;
        newDate = newDate.AddDays(-90 + add * (Instance.isDecreaseLoanDate ? 1 : 3));
        if (Instance.isDecember && newDate.Month != 12)
            newDate = new System.DateTime(newDate.Year, 12, newDate.Day);
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
