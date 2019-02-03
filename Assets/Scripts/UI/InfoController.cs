using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyMemory;
public class InfoController : MonoBehaviour
{
    public InfoManager mgr;
    public SubInfoManager subMgr;
    public PointPlayer pointPlayer;

    public Database Database;

    internal int displayNum;

    MemoryData data = MemoryData.New();

    void Update()
    {
        MemoryData newData = MemoryData.New();
        if (!MemoryTracker.GetData(out newData))
            return;

        bool isSel = newData.scene == 1;
        int targetNum = 0;

        //玩家數量
        if (isSel)
            targetNum = newData.sel_player_num;
        else
            targetNum = newData.player_num;

        if (data.Comparer(newData))
        {
            DisplayNumUpdate(targetNum);
            UpdateInfo(newData);
            UpdateSubInfo(newData);
            pointPlayer.SetIndex(newData.scene > 1 ? newData.cur : -1);
        }
        data = newData;
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
            gray &= data.total > 0 || data.cur >= i;
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

        subMgr.DiaplaySub(data.scene > 1);
    }
}
