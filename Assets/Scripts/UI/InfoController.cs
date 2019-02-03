using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyMemory;
public class InfoController : MonoBehaviour
{
    public static InfoController Instance;

    public InfoManager mgr;
    public SubInfoManager subMgr;
    public PointPlayer pointPlayer;

    public Database Database;

    internal int displayNum;

    MemoryData data = MemoryData.New();

    private void Awake()
    {
        Instance = this;
    }

    internal void Receive(int targetNum, MemoryData newData)
    {
        if (data.Comparer(newData))
        {
            DisplayNumUpdate(targetNum);
            UpdateInfo(newData);
            UpdateSubInfo(newData);
            pointPlayer.SetIndex(newData.scene > 1 ? newData.cur : -1);

            ////Hack
            //if (data.scene > 1)
            //{
            //    //回合
            //    if (newData.cur == 0 && data.cur != 0)
            //    {
            //        if(HackManager.Instance.isRandomCPI)
            //        {
            //            int cpi = HackManager.Instance.GetRandomCPI();
            //            ProcessUtility.WriteMem(ProcessManager.Instance.Process, MemoryTracker.GetPtr(MemoryTracker.MemTypeEnum.CPI), cpi);
            //        }
            //    }

            //}
        }
        data = newData;
    }

    void DisplayNumUpdate(int target)
    {
        int sign = (int)Mathf.Sign(target - displayNum);

        int i = sign >= 0 ? 0 : 3;
        while (i >= 0 && i <= 3)
        {
            if (i < target)
                mgr.InfoTrans(i);
            else if (sign < 0)
                mgr.InfoTrans(i, false);

            i += sign;
        }

        mgr.SetDisplayNum(target);
        displayNum = target;
    }

    void UpdateInfo(MemoryData data)
    {
        bool isSel = data.scene == 1;
        byte[] p = isSel ? data.sel_p : data.p;

        for (int i = 0; i < displayNum; i++)
        {
            var info = mgr.Infos[i];

            byte id = p[i];
            info.ChaName.text = Database.datas[id].name;
            info.ChaName.textComponent.color = Database.datas[id].color;
            info.Icon.sprite = Database.datas[id].Icon;
            bool gray = data.scene != 1;
            gray &= data.life[i] == 0;
            gray &= data.total > 0 || data.cur > i;
            info.Icon.material.SetFloat("_GrayScale", gray ? 1 : 0);

            if (isSel)
            {
                info.Cash.ChangeText("-");
                info.Bank.ChangeText("-");
            }
            else
            {
                info.Cash.ChangeText(data.cash[i].ToString());
                info.Bank.ChangeText(data.bank[i].ToString());
            }

        }
    }

    void UpdateSubInfo(MemoryData data)
    {
        subMgr.CPIText.ChangeText(data.CPI.ToString());
        subMgr.YearText.ChangeText(data.year.ToString());
        subMgr.MonText.ChangeText(data.mon.ToString());
        subMgr.DayText.ChangeText(data.day.ToString());

        subMgr.DisplaySub(data.scene > 1);
    }
}
