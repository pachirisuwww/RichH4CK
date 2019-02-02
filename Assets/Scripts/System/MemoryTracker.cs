using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.Linq;

namespace MyMemory
{
    internal struct MemoryData
    {
        //Select Cha
        internal byte sel_player_num;
        internal byte[] sel_p;

        //In Game Cha
        internal byte player_num;
        internal byte[] p;

        //Money
        internal int[] cash;
        internal int[] bank;

        //物價指數
        internal int CPI; //consumer price index

        //當前玩家
        internal byte cur;

        //總回合數
        internal int total;

        //日期
        internal byte day;
        internal byte mon;

        //關卡狀態
        internal byte scene;

        static internal MemoryData New()
        {
            MemoryData data = new MemoryData();

            data.sel_p = new byte[4];
            data.p = new byte[4];
            data.cash = new int[4];
            data.bank = new int[4];

            return data;
        }

        internal bool Comparer(MemoryData obj)
        {
            bool ret = true;

            ret &= sel_player_num == obj.sel_player_num;
            ret &= player_num == obj.player_num;

            ret &= p.SequenceEqual(obj.p);
            ret &= sel_p.SequenceEqual(obj.sel_p);

            ret &= cash.SequenceEqual(obj.cash);
            ret &= bank.SequenceEqual(obj.bank);

            ret &= cur == obj.cur;
            ret &= total == obj.total;

            ret &= CPI == obj.CPI;

            ret &= day == obj.day;
            ret &= mon == obj.mon;

            ret &= scene == obj.scene;
            
            return !ret;
        }
    }

    public class MemoryTracker
    {
        internal enum MemTypeEnum
        {
            //Select Cha
            sel_player_num = 0x8A40D,
            sel_p1 = 0x8A35C,

            //In Game Cha
            player_num = 0x99104,
            p1 = 0x96B7B,

            //Money
            cash_p1 = 0x96B84,
            bank_p1 = 0x96B88,

            //物價指數
            CPI = 0x990E8, //consumer price index

            //當前玩家
            cur = 0x9910C,

            //總回合數
            total = 0x96738,

            //日期
            day = 0x97160,
            mon = 0x97161,

            //關卡狀態(not in main module)
            scene = 0xCF9F0,
        }

        //差值
        static int sub_sel_p = 0xC;
        static int sub_p = 0x68;

        static internal bool GetData(out MemoryData data)
        {
            data = MemoryData.New();

            Process p = ProcessManager.Instance.Process;
            if (p == null)
                return false;

            //Cha
            data.sel_player_num = ProcessUtility.ReadMemByte(p, GetPtr(MemTypeEnum.sel_player_num));
            data.player_num = ProcessUtility.ReadMemByte(p, GetPtr(MemTypeEnum.player_num));
            for (int i = 0; i < 4; i++)
            {
                data.sel_p[i] = ProcessUtility.ReadMemByte(p, IntPtr.Add(GetPtr(MemTypeEnum.sel_p1), sub_sel_p * i));
                data.p[i] = ProcessUtility.ReadMemByte(p, IntPtr.Add(GetPtr(MemTypeEnum.p1), sub_p * i));
                data.cash[i] = ProcessUtility.ReadMemInt(p, IntPtr.Add(GetPtr(MemTypeEnum.cash_p1), sub_p * i));
                data.bank[i] = ProcessUtility.ReadMemInt(p, IntPtr.Add(GetPtr(MemTypeEnum.bank_p1), sub_p * i));
            }

            //Course
            data.cur = ProcessUtility.ReadMemByte(p, GetPtr(MemTypeEnum.cur));
            data.total = ProcessUtility.ReadMemByte(p, GetPtr(MemTypeEnum.total));
            data.day = ProcessUtility.ReadMemByte(p, GetPtr(MemTypeEnum.day));
            data.mon = ProcessUtility.ReadMemByte(p, GetPtr(MemTypeEnum.mon));

            data.CPI = ProcessUtility.ReadMemInt(p, GetPtr(MemTypeEnum.CPI));
            data.scene = ProcessUtility.ReadMemByte(p, GetPtr(MemTypeEnum.scene, false));
            return true;
        }

        internal static IntPtr GetPtr(MemTypeEnum type, bool module = true)
        {
            if (module)
                return IntPtr.Add(ProcessManager.Instance.Process.MainModule.BaseAddress, (int)type);
            else
                return (IntPtr)type;
        }
    }
}