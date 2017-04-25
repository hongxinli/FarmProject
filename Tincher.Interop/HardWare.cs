using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Management;

namespace Tincher.Interop
{
    /// <summary>
    /// 环境类
    /// </summary>
    public class HardWare
    {
        /// <summary>
        /// 验证key
        /// </summary>
        private static readonly int[] keys = new int[] { 1, 9, 8, 8, 0, 2, 0, 3, 1, 9, 8, 9, 1, 2, 3, 0 };
        /// <summary>
        /// 获得唯一码
        /// </summary>
        /// <returns></returns>
        public static string GetSerialNo()
        {
            var serial = new string[] 
            { 
                GetBIOSSerialNumber().Replace("-",""),
                GetCPUSerialNumber().Replace("-",""),
                GetNetCardMACAddress().Replace("-",""),
                GetHardDiskSerialNumber().Replace("-","")
            };
            string bios = serial[0].Substring(serial[0].Length % 5, 4);
            string cpu = serial[1].Substring(serial[1].Length % 4, 4);
            string net = serial[2].Substring(serial[2].Length % 2, 4);
            string hard = serial[3].Substring(serial[3].Length %3, 4);
            return Format(cpu + hard + bios + net);
        }

        private static string Format(string code)
        {
            char[] origenal = code.ToCharArray();
            string endCode = "";
            int index = 1;
            foreach (char o in origenal)
            {
                int asc = Convert.ToInt32(o) + keys[index - 1];
                //0-9,a-c
                if (asc < 48)
                {
                    asc = 84;
                }
                else if ((asc >= 48 && asc <= 57))
                {
                    asc = asc + 20;
                }
                else if ((asc >= 58 && asc <= 64))
                {
                    asc = asc + 15;
                }
                else if (asc >= 65 && asc <= 90)
                {
                    asc = asc + 3;
                    if (asc > 90)
                    {
                        asc = asc - 4;
                    }
                }
                else if (asc >= 91 && asc <= 96)
                {
                    asc = asc - 10;
                }
                else if (asc >= 97 && asc <= 122)
                {
                    asc = asc - 32;
                }
                else if (asc > 122)
                {
                    asc = asc - 6;
                }
                endCode += Convert.ToChar(asc);
                if (index % 4 == 0)
                {
                    endCode += "-";
                }
                index++;
            }
            return endCode.Substring(0, 19); 
        }
        #region 硬件信息
        public static string GetBIOSSerialNumber()
        {

            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From Win32_BIOS");
                string sBIOSSerialNumber = "";
                foreach (ManagementObject mo in searcher.Get())
                {
                    sBIOSSerialNumber = mo["SerialNumber"].ToString().Trim();
                }
                return sBIOSSerialNumber == "0" ? "11SC10A3393ZZ0LR15929B" : sBIOSSerialNumber;
            }
            catch
            {
                return "11SC10A3393ZZ0LR15929B";
            }
        }
        //获取CPU序列号
        public static string GetCPUSerialNumber()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From Win32_Processor");
                string sCPUSerialNumber = "";
                foreach (ManagementObject mo in searcher.Get())
                {
                    sCPUSerialNumber = mo["ProcessorId"].ToString().Trim();
                }
                return sCPUSerialNumber == "" ? "BFABFBAF00D206AB" : sCPUSerialNumber;
            }
            catch
            {
                return "BFABFBAF00D206AB";
            }
        }
        //获取硬盘序列号
        public static string GetHardDiskSerialNumber()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
                string sHardDiskSerialNumber = "";
                foreach (ManagementObject mo in searcher.Get())
                {
                    sHardDiskSerialNumber = mo["SerialNumber"].ToString().Trim();
                    break;
                }
                return sHardDiskSerialNumber == "" ? "110C17PAN40E17D3210B" : sHardDiskSerialNumber;
            }
            catch
            {
                return "110C17PAN40E17D3210B";
            }
        }
        /// <summary>
        /// 获取网卡地址
        /// </summary>
        /// <returns></returns>
        public static string GetNetCardMACAddress()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter WHERE ((MACAddress Is Not NULL) AND (Manufacturer <> 'Microsoft'))");
                string NetCardMACAddress = "";
                foreach (ManagementObject mo in searcher.Get())
                {
                    NetCardMACAddress = mo["MACAddress"].ToString().Trim();
                }
                return NetCardMACAddress == "" ? "E89C8FDA92B0" : NetCardMACAddress.Replace(":", "");
            }
            catch
            {
                return "E89C8FDA92B0";
            }
        }
        #endregion
    }
}
