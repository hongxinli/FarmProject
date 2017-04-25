using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Tincher.Interop
{
    public class Security
    {
        /// <summary>
        /// 32位Key值：
        /// </summary>
        private static byte[] DESKey = new byte[] { 0x3B, 0x43, 0x9B, 0x93, 0x4B, 0x03, 0x0B, 0x13, 0x1B, 0x23, 0x2B, 0x33, 0x8B, 0x83, 0x7B, 0x73, 0x6B, 0x63, 0x5B, 0x53, 0xF3, 0xFB, 0xA3, 0xAB, 0xEB, 0xB3, 0xBB, 0xC3, 0xE3, 0xD3, 0xDB, 0xCB };

        #region DES加密
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="strSource">待加密字串</param>
        /// <returns>加密后的字符串</returns>
        public static string DESEncrypt(string strSource)
        {
            return DESEncrypt(strSource, DESKey);
        }
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="input">待加密字串</param>
        /// <param name="key">Key值</param>
        /// <returns>加密后的字符串</returns>
        public static string DESEncrypt(string input, byte[] key)
        {
            if (key.Length != 32)
            {
                int i = 0;
                foreach (var k in key)
                {
                    DESKey[i] = k;
                }
            }
            SymmetricAlgorithm sa = Rijndael.Create();
            sa.Key = DESKey;
            sa.Mode = CipherMode.ECB;
            sa.Padding = PaddingMode.Zeros;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, sa.CreateEncryptor(), CryptoStreamMode.Write);
            byte[] byt = Encoding.Unicode.GetBytes(input);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();
            return Convert.ToBase64String(ms.ToArray());
        }
        #endregion

        #region DES解密
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="input">待解密的字串</param>
        /// <returns>解密后的字符串</returns>
        public static string DESDecrypt(string input)
        {
            return DESDecrypt(input, DESKey);
        }
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="input">待解密的字串</param>
        /// <param name="key">32位Key值</param>
        /// <returns>解密后的字符串</returns>
        public static string DESDecrypt(string input, byte[] key)
        {
            if (key.Length != 32)
            {
                int i = 0;
                foreach (var k in key)
                {
                    DESKey[i] = k;
                }
            }
            SymmetricAlgorithm sa = Rijndael.Create();
            sa.Key = DESKey;
            sa.Mode = CipherMode.ECB;
            sa.Padding = PaddingMode.Zeros;
            ICryptoTransform ct = sa.CreateDecryptor();
            byte[] byt = Convert.FromBase64String(input);
            MemoryStream ms = new MemoryStream(byt);
            CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cs, Encoding.Unicode);
            return sr.ReadToEnd().Trim('\0');
        }
        #endregion

        #region SHA256
        /// <summary>
        /// SHA256
        /// </summary>
        /// <param name="regNum"></param>
        /// <returns></returns>
        public static string SHA256(string regNum)
        {
            SHA256 sha256 = new SHA256CryptoServiceProvider();//建立一个SHA256
            byte[] source = Encoding.Default.GetBytes(regNum);//将字符串转为Byte[]
            byte[] crypto = sha256.ComputeHash(source);//进行SHA256加密
            return Convert.ToBase64String(crypto);//把加密后的字符串从Byte[]转为字符串
        }
        #endregion
    }
}
