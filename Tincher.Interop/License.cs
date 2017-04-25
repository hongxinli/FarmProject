using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Management;
using System.Globalization;
using System.Security.Cryptography;

namespace Tincher.Interop
{
    /// <summary>
    /// 注册使用
    /// </summary>
    public class License
    {
        private static readonly string _key="*^_^*-GANTZ-o(∩_∩)o";

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Ticket GetInstance(string path)
        {
            try
            {
                //初始化
                var tickt = new Ticket()
                {
                    Allow = false,
                    MachineCode = HardWare.GetSerialNo()
                };
                //是否存在唯一key文件
                if (!File.Exists(path))
                {
                    return tickt;
                }
                var products = new string[7];
                using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
                {
                    int i = 0;
                    while (sr.Peek() >= 0)
                    {
                        products[i] = sr.ReadLine();
                        i++;
                    }
                }
                //是否是在同一个机器
                var keyCode = Security.SHA256(tickt.MachineCode + _key);
                if (keyCode == products[0])
                {
                    byte[] btKey = Encoding.UTF8.GetBytes(_key);
                    //赋值
                    tickt.ProductNo = Security.DESDecrypt(products[1], btKey);
                    tickt.DueDate = Security.DESDecrypt(products[2], btKey);
                    tickt.RegDate = Security.DESDecrypt(products[3], btKey);
                    int count = Convert.ToInt32(Security.DESDecrypt(products[4], btKey));
                    tickt.UserCount = count == 0 ? int.MaxValue : count;
                    tickt.Trial = Security.DESDecrypt(products[5], btKey) == "0" ? true : false;
                    tickt.Customer = Security.DESDecrypt(products[6], btKey);
                    if (Convert.ToDateTime(tickt.DueDate) > DateTime.Now)
                    {
                        tickt.Allow = true;
                    }
                }
                return tickt;
            }
            catch
            {
                return new Ticket() { Allow = false, MachineCode = "-" };
            }
        }
    }
}
