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
        internal byte[] life;

        //Money
        internal int[] cash;
        internal int[] bank;
        internal int[] loanDate;

        //物價指數
        internal int CPI; //consumer price index

        //當前玩家
        internal byte cur;

        //總回合數
        internal int total;

        //日期
        internal int date;

        //關卡狀態
        internal byte scene;

        static internal MemoryData New()
        {
            MemoryData data = new MemoryData();

            data.sel_p = new byte[4];
            data.p = new byte[4];
            data.cash = new int[4];
            data.bank = new int[4];
            data.loanDate = new int[4];
            data.life = new byte[4];

            return data;
        }

        internal bool Comparer(MemoryData obj)
        {
            bool ret = true;

            ret &= sel_player_num == obj.sel_player_num;
            ret &= player_num == obj.player_num;

            ret &= p.SequenceEqual(obj.p);
            ret &= sel_p.SequenceEqual(obj.sel_p);
            ret &= life.SequenceEqual(obj.life);

            ret &= cash.SequenceEqual(obj.cash);
            ret &= bank.SequenceEqual(obj.bank);
            ret &= loanDate.SequenceEqual(obj.loanDate);

            ret &= cur == obj.cur;
            ret &= total == obj.total;

            ret &= CPI == obj.CPI;

            ret &= date == obj.date;

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
            player_num = 0x99114,
            p1 = 0x96B7B,
            p1_life = 0x96B7D,

            //Money
            cash_p1 = 0x96B84,
            bank_p1 = 0x96B88,

            //物價指數
            CPI = 0x990E8, //consumer price index

            //當前玩家
            cur = 0x9910C,

            //總回合數
            total = 0x96738,

            //日期 (day/mon/year)
            date = 0x97160,

            //貸款日
            p1_loanDay = 0x96B94,

            //關卡狀態
            scene = 0x7E772,
        }

        //差值
        static internal int sub_sel_p = 0xC;
        static internal int sub_p = 0x68;

        static internal MemoryData GetData()
        {
            var data = MemoryData.New();

            var p = ProcessManager.Instance.Process;

            //Cha
            data.sel_player_num = ProcessUtility.ReadMemByte(p, GetPtr(MemTypeEnum.sel_player_num));
            data.player_num = ProcessUtility.ReadMemByte(p, GetPtr(MemTypeEnum.player_num));
            for (int i = 0; i < 4; i++)
            {
                data.sel_p[i] = ProcessUtility.ReadMemByte(p, AddPtr(GetPtr(MemTypeEnum.sel_p1), sub_sel_p * i));
                data.p[i] = ProcessUtility.ReadMemByte(p, AddPtr(GetPtr(MemTypeEnum.p1), sub_p * i));
                data.life[i] = ProcessUtility.ReadMemByte(p, AddPtr(GetPtr(MemTypeEnum.p1_life), sub_p * i));
                data.cash[i] = ProcessUtility.ReadMemInt(p, AddPtr(GetPtr(MemTypeEnum.cash_p1), sub_p * i));
                data.bank[i] = ProcessUtility.ReadMemInt(p, AddPtr(GetPtr(MemTypeEnum.bank_p1), sub_p * i));
                data.loanDate[i] = ProcessUtility.ReadMemInt(p, AddPtr(GetPtr(MemTypeEnum.p1_loanDay), sub_p * i));
            }

            //Course
            data.cur = ProcessUtility.ReadMemByte(p, GetPtr(MemTypeEnum.cur));
            data.total = ProcessUtility.ReadMemByte(p, GetPtr(MemTypeEnum.total));
            data.date = ProcessUtility.ReadMemInt(p, GetPtr(MemTypeEnum.date));

            data.CPI = ProcessUtility.ReadMemInt(p, GetPtr(MemTypeEnum.CPI));
            data.scene = ProcessUtility.ReadMemByte(p, GetPtr(MemTypeEnum.scene));
            return data;
        }

        internal static IntPtr GetPtr(MemTypeEnum type, bool mainModule = true)
        {
            if (mainModule)
                return AddPtr(ProcessManager.Instance.Process.MainModule.BaseAddress, (int)type);
            else
                return (IntPtr)type;
        }

        static IntPtr AddPtr(IntPtr a, int b)
        {
            return new IntPtr((int)a + b);
        }

        internal static DateTime GetDate(int date)
        {
            byte[] bytes = BitConverter.GetBytes(date);

            DateTime ret = new DateTime(BitConverter.ToInt16(bytes, 2), bytes[1], bytes[0]);

            return ret;
        }

        internal static int ConvertDate(DateTime date)
        {
            byte[] bytes = new byte[4];

            bytes[0] = (byte)date.Day;
            bytes[1] = (byte)date.Month;
            byte[] year = BitConverter.GetBytes((short)date.Year);
            bytes[2] = year[0];
            bytes[3] = year[1];

            return BitConverter.ToInt32(bytes, 0);
        }
    }
}