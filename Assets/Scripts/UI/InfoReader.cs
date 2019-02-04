using MyMemory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InfoReader : NetworkBehaviour
{
    MemoryData data = MemoryData.New();

    void Update()
    {
        //從第一個場景開始 且是伺服器模式
        if (!isClient)
            return;

        if (ProcessManager.Instance == null || ProcessManager.Instance.Process == null)
            return;

        MemoryData newData = MemoryTracker.GetData();

        bool isSel = newData.scene == 1;
        int targetNum = 0;

        //玩家數量
        if (isSel)
            targetNum = newData.sel_player_num;
        else
            targetNum = newData.player_num;

        if (data.Comparer(newData))
        {
            //Hack
            if (data.scene > 1)
            {
                //回合
                if (newData.cur < data.cur)
                {
                    if (HackManager.Instance.isRandomCPI)
                    {
                        int cpi = HackManager.Instance.GetRandomCPI();
                        ProcessUtility.WriteMem(ProcessManager.Instance.Process, MemoryTracker.GetPtr(MemoryTracker.MemTypeEnum.CPI), cpi);
                    }
                }
            }

            //Send To Receiver
            CmdRead(targetNum, newData);

            data = newData;
        }
    }

    [Command]
    internal void CmdRead(int targetNum, MemoryData data)
    {
        if (InfoController.Instance)
            InfoController.Instance.Receive(targetNum, data);
    }
}
