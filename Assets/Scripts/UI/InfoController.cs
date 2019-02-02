using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyMemory;
public class InfoController : MonoBehaviour
{
    public Database Database;

    int displayNum;

    MemoryData data = MemoryData.New();

    void Update()
    {
        InfoManager mgr = InfoManager.Instance;

        MemoryData newData = MemoryData.New();
        if (!MemoryTracker.GetData(out newData))
            return;

        bool isSel = false;
        int targetNum = 0;

        //玩家數量
        if (CheckSelChange(newData))
        {
            targetNum = newData.sel_player_num;
            isSel = true;
        }
        if (CheckInGameChange(newData)) targetNum = newData.player_num;

        if (isSel)
        {
            //回合 (選擇角色時借來標記未進遊戲)
            ProcessUtility.WriteMem(ProcessManager.Instance.Process, MemoryTracker.GetPtr(MemoryTracker.MemTypeEnum.cur), 255);
            newData.cur = 255;
        }

        if (newData.cur == 255)
        {
            //Money
            for (int i = 0; i < 4; i++)
            {
                newData.cash[i] = 0;
                newData.bank[i] = 0;
            }
        }

        if (data.Comparer(newData))
        {
            DisplayNumUpdate(targetNum);
            UpdateInfo(newData);

            data = newData;
        }
    }

    bool CheckSelChange(MemoryData other)
    {
        bool changed = false;
        changed |= data.sel_player_num != other.sel_player_num;

        return changed;
    }

    bool CheckInGameChange(MemoryData other)
    {
        bool changed = false;
        changed |= data.player_num != other.player_num;

        return changed;
    }

    void DisplayNumUpdate(int target)
    {
        var mgr = InfoManager.Instance;

        int sign = (int)Mathf.Sign(target - displayNum);

        int i = sign >= 0 ? 0 : 3;
        while (i >= 0 && i <= 3)
        {
            if (i < target)
                mgr.InfoTrans(i);
            else
                mgr.InfoTrans(i, -1);

            i += sign;
        }

        displayNum = target;
    }

    void UpdateInfo(MemoryData data)
    {
        var mgr = InfoManager.Instance;
        bool isSel = data.cur == 255;
        byte[] p = isSel ? data.sel_p : data.p;

        for (int i = 0; i < displayNum; i++)
        {
            var info = mgr.Infos[i];

            byte id = p[i];
            info.ChaName.text = Database.datas[id].name;
            info.Icon.sprite = Database.datas[id].Icon;

            if (isSel)
            {
                info.Cash.text = "-";
                info.Bank.text = "-";
            }
            else
            {
                info.Cash.text = data.cash[i].ToString();
                info.Bank.text = data.bank[i].ToString();
            }

        }
    }
}
