using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// 将pdf转化为swf
    /// </summary>
    public class Pdf2Swf
    {
        /// <summary>
        /// 将pdf转化为swf文件
        /// </summary>
        /// <param name="pdfPath"></param>
        /// <param name="swfPath"></param>
        public void ConvertToSwf(string pdfPath, string swfPath)
        {
            string swfTools = ConfigHelper.GetAppSettings("SwfTools");
            Process pc = new Process();

            //-b,默认的swf导航文件;-o,output file;-T 设置swf文件所使用的flash文件版本号
            ProcessStartInfo psi = new ProcessStartInfo(swfTools, " -b "+pdfPath + " -o " + swfPath+" -T 9");
            pc.StartInfo = psi;
            pc.Start();
            pc.WaitForExit();
        }
    }
}
