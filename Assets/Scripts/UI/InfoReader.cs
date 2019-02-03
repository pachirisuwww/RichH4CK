using MyMemory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InfoReader : NetworkBehaviour
{
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
        
        //Send To Receiver
        CmdRead(targetNum, newData);
    }

    [Command]
    internal void CmdRead(int targetNum, MemoryData data)
    {
        if (InfoController.Instance)
            InfoController.Instance.Receive(targetNum, data);
    }
}
